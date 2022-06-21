using Chat.Constants;
using Chat.Helper;
using Chat.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Chat.ViewModel
{
    public class RegistrationViewModel : ViewModelBase
    {
        #region Private Variables

        private string _username;
        private string _firstName;
        private string _lastName;
        private string _emailId;
        private Visibility _loader;
        private string _loaderText;

        #endregion

        #region Public Properties

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
        public string EmailId
        {
            get { return _emailId; }
            set
            {
                _emailId = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        #endregion

        #region Public Commands

        public ICommand RegisterCommand { get; set; }
        public Action Close { get; set; }

        #endregion

        public RegistrationViewModel()
        {
            RegisterCommand = new RelayCommand(RegisterBtnClick);
            ShowLoader(false);
        }

        private async void RegisterBtnClick(object value)
        {
            if (ValidateFeilds())
            {
                ShowLoader(true, "Registering User..");
                var user = new User()
                {
                    EmailId = EmailId,
                    Password = Password,
                    FirstName = FirstName,
                    LastName = LastName,
                    UserName = UserName,
                    Id = Guid.NewGuid().ToString()
                };

                var http = new HTTP();
                var result = await http.CreateProductAsync(ChatConstants.PostUser, user);
                if (result == "OK")
                {
                    ClearFeilds();
                    MessageBox.Show("User Registered");
                    Close?.Invoke();
                }
                ShowLoader(false);
            }
        }

        private bool ValidateFeilds()
        {
            if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName) &&
                string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(EmailId) &&
                string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(ConfirmPassword))
            {
                MessageBox.Show("Feilds are Empty");
                return false;
            }

            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Passord not Matching");
                return false;
            }

            ShowLoader(false);
            return true;
        }

        private void ClearFeilds()
        {
            FirstName = ""; LastName = "";
            UserName = ""; EmailId = "";
            Password = ""; ConfirmPassword = "";
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
