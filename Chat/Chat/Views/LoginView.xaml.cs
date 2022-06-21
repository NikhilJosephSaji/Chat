using Chat.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chat.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : System.Windows.Window
    {
        private LoginViewModel vm;
        public LoginView()
        {
            InitializeComponent();
            vm = new LoginViewModel();
            this.DataContext = vm;
            vm.Close += ClosingWindow;
        }
        private void ClosingWindow()
        {
            this.Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var password = ((PasswordBox)sender);
            vm.Password = password.Password;
        }
    }
}
