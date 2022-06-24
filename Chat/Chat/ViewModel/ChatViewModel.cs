using Chat.Constants;
using Chat.Helper;
using Chat.Model;
using Chat.Views;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Chat.ViewModel
{
    public class ChatViewModel : ViewModelBase
    {
        #region Private Variables

        private ObservableCollection<OnlineUser> _onlineUsers;
        private string _defaultPhotoUrl;
        private int _listBoxHeight;
        private string _logedUserPhoto;
        private string _loggedUserName;
        private User _loggedUser;
        private HubConnection connection;
        private HTTP http;
        private string _selectedUserPhoto;
        private string _selectedUser;
        private OnlineUser _selectedOnlineUser;
        private Visibility _isUserSelected;
        private string _messageContent;
        private Dictionary<string, ObservableCollection<UserChat>> userChats;
        private ObservableCollection<UserChat> _chat;

        #endregion

        #region Public Properties

        public ObservableCollection<OnlineUser> OnlineUsers
        {
            get { return _onlineUsers; }
            set
            {
                _onlineUsers = value;
                OnPropertyChanged();
            }
        }

        public int LiistBoxHeight
        {
            get { return _listBoxHeight; }
            set
            {
                _listBoxHeight = value;
                OnPropertyChanged();
            }
        }
        public string LogedUserPhoto
        {
            get { return _logedUserPhoto; }
            set
            {
                _logedUserPhoto = value;
                OnPropertyChanged();
            }
        }

        public string LoggedUserName
        {
            get { return _loggedUserName; }
            set
            {
                _loggedUserName = value;
                OnPropertyChanged();
            }
        }

        public string SelectedUserPhoto
        {
            get { return _selectedUserPhoto; }
            set
            {
                _selectedUserPhoto = value;
                OnPropertyChanged();
            }
        }

        public string SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        public OnlineUser SelectedOnlineUser
        {
            get { return _selectedOnlineUser; }
            set
            {
                _selectedOnlineUser = value;
                OnPropertyChanged();
            }
        }

        public Visibility IsUserSelected
        {
            get { return _isUserSelected; }
            set
            {
                _isUserSelected = value;
                OnPropertyChanged();
            }
        }

        public string MessageContent
        {
            get { return _messageContent; }
            set
            {
                _messageContent = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UserChat> Chat
        {
            get { return _chat; }
            set
            {
                _chat = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Public Commands

        public ICommand LogoutCommand { get; set; }
        public ICommand ListBoxUserSelected { get; set; }
        public Action Close { get; set; }
        public ICommand SendCommand { get; set; }

        #endregion

        public ChatViewModel(User user)
        {
            IsUserSelected = Visibility.Collapsed;
            http = new HTTP();
            OnlineUsers = new ObservableCollection<OnlineUser>();
            userChats = new Dictionary<string, ObservableCollection<UserChat>>();
            Chat = new ObservableCollection<UserChat>();
            SendCommand = new RelayCommand(SendBtnClicked);
            LogoutCommand = new RelayCommand(Logout);
            ListBoxUserSelected = new RelayCommand(ListBoxUserSelectionChanged);
            _defaultPhotoUrl = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\images\avatar_circle.png";
            _loggedUser = user;
            SetLoggedUser();
            RegisterChatHub();
        }

        private async void RegisterChatHub()
        {
            connection = new HubConnectionBuilder()
                        .WithUrl(ChatConstants.URLCHAT)
                        .WithAutomaticReconnect()
                        .Build();
            connection.On<string, string>("ReceiveCallerMessage", (user, message) =>
            {
                var userchat = new UserChat();
                userchat.Message = message;
                userchat.Author = user;
                userchat.ChatUserImage = _defaultPhotoUrl;
                userchat.IsNativeSender = true;
                var privatechat = userChats[user];
                privatechat.Add(userchat);
                Chat = userChats[user];
            });
            connection.On<string, string>("ReceiveClientMessage", (user, message) =>
            {
                var userchat = new UserChat();
                userchat.Message = message;
                userchat.Author = user;
                userchat.ChatUserImage = _defaultPhotoUrl;
                var privatechat = userChats[user];
                privatechat.Add(userchat);
            });
            connection.On<Dictionary<string, string>>("LoginMessage", async (users) =>
            {
                var userlist = users.Select(x => x.Key).ToList();
                var url = ChatConstants.HTTPURL + ChatConstants.GetUser;
                foreach (var user in userlist)
                {
                    if (!userChats.ContainsKey(user))
                        userChats.Add(user, new ObservableCollection<UserChat>());
                    if (user != _loggedUser.Id)
                    {
                        var isUserAvail = OnlineUsers.Any(x => x.Id == user);
                        if (!isUserAvail)
                        {
                            var jason = new Jason<User>();
                            var result = await Task.Run(() => http.HttpResultByURl(string.Format(url, user)));
                            var dt = jason.Deserialize(result);
                            OnlineUsers.Add(new OnlineUser { Id = dt.Id, Name = dt.UserName, Photo = _defaultPhotoUrl });
                        }
                    }
                }
            });
            connection.On<string>("LogoutMessage", (userid) =>
            {
                if (userid != _loggedUser.Id)
                {
                    var user = OnlineUsers.FirstOrDefault(x => x.Id == userid);
                    if (user == SelectedOnlineUser)
                        IsUserSelected = Visibility.Collapsed;
                    OnlineUsers.Remove(user);
                    userChats.Remove(user.Id);
                }
            });

            await connection.StartAsync();
            await connection.InvokeAsync<bool>("Login", _loggedUser.Id);
        }

        private void SetLoggedUser()
        {
            LoggedUserName = _loggedUser.UserName;
            LogedUserPhoto = _defaultPhotoUrl;
        }

        private async void Logout(object value)
        {
            await LogoutUser();
            new LoginView().Show();
            Close?.Invoke();
        }

        public async Task LogoutUser()
        {
            await connection.InvokeAsync<bool>("LogOut", _loggedUser.Id);
        }

        private void ListBoxUserSelectionChanged(object value)
        {
            if (SelectedOnlineUser != null)
            {
                IsUserSelected = Visibility.Visible;
                SelectedUser = SelectedOnlineUser.Name;
                SelectedUserPhoto = SelectedOnlineUser.Photo;
                Chat = userChats[SelectedOnlineUser.Id];
            }
        }
        private async void SendBtnClicked(object value)
        {
            if (SelectedOnlineUser != null)
                await connection.InvokeAsync("SendMessage", SelectedOnlineUser.Id, _loggedUser.Id, MessageContent);
        }
    }
}
