using Chat.Model;
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
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : Window
    {
        private ChatViewModel vm;
        public ChatView(User user)
        {
            InitializeComponent();
            vm = new ChatViewModel(user);
            this.DataContext = vm;
            vm.Close += ClosingWindow;
        }

        private void ClosingWindow()
        {
            this.Close();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            vm.LiistBoxHeight = (int)this.ActualHeight > 90 ? (int)this.ActualHeight - 90 : (int)this.ActualHeight;
        }

        private async void Window_Closed(object sender, EventArgs e)
        {
            await vm.LogoutUser();
        }
    }
}