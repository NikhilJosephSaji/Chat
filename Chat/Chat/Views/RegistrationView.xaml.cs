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
    /// Interaction logic for RegistrationView.xaml
    /// </summary>
    public partial class RegistrationView : Window
    {
        private RegistrationViewModel vm;
        public RegistrationView()
        {
            InitializeComponent();
            vm = new RegistrationViewModel();
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

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var password = ((PasswordBox)sender);
            vm.ConfirmPassword = password.Password;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new LoginView().Show();
        }
    }
}
