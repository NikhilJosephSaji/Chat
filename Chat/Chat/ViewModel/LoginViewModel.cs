using Chat.Constants;
using Chat.Helper;
using Chat.Model;
using Chat.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Chat.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        #region Private Variables

        private string _username;
        private Visibility _loader;
        private string _loaderText;

        #endregion

        #region Public Properties

        public string UserName
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public Visibility Loader
        {
            get { return _loader; }
            set
            {
                _loader = value;
                OnPropertyChanged();
            }
        }

        public string LoaderText
        {
            get { return _loaderText; }
            set
            {
                _loaderText = value;
                OnPropertyChanged();
            }
        }

        public string Password { get; set; }

        #endregion

        #region Public Commands

        public ICommand SiginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        public Action Close { get; set; }

        #endregion

        public LoginViewModel()
        {
            SiginCommand = new RelayCommand(SiginClicked);
            RegisterCommand = new RelayCommand(RegisterClicked);
            ShowLoader(false);
        }

        private async void SiginClicked(object value)
        {
            if (ValidateFeilds())
            {
                ShowLoader(true, "Please Wait...");
                var http = new HTTP();
                var url = ChatConstants.HTTPURL + ChatConstants.Login;
                var result = await Task.Run(() => http.HttpResultByURl(string.Format(url, UserName, Password)));
                if (result != "")
                {
                    var jason = new Jason<User>();
                    new ChatView(jason.Deserialize(result)).Show();
                    Close?.Invoke();
                }
                else
                    MessageBox.Show("User Not Found.");
                ShowLoader(false);
            }
        }

        private void RegisterClicked(object value)
        {
            new RegistrationView().Show();
            Close?.Invoke();
        }

        private bool ValidateFeilds()
        {
            if (string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Feilds are Empty");
                return false;
            }
            else
                return true;
        }

        private void ShowLoader(bool IsShow, string loadingText = "")
        {
            if (IsShow)
            {
                Loader = Visibility.Visible;
                LoaderText = loadingText;
            }
            else
            {
                Loader = Visibility.Collapsed;
                LoaderText = "";
            }
        }
    }
}
