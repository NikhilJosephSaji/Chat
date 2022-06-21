using Chat.Model;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chat
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class Window : System.Windows.Window
    {
        HubConnection connection;
        //private List<User> userlist = new List<User>();
        public Window()
        {
            InitializeComponent();
            connection = new HubConnectionBuilder()
         .WithUrl("https://localhost:44386/chathub")
         .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
        }

        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
            var url = "https://localhost:44386/User/GetUser?userid={0}";
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{user}: {message}";
                    messagesList.Items.Add(newMessage);
                });
            });
            connection.On<string>("LoginMessage", (userid) =>  
            {
                this.Dispatcher.Invoke(() => 
                {

                    var result = ReturnHttpResult(string.Format(url, userid));
                    var user = JsonConvert.DeserializeObject<User>(result);
                    Userlist.Items.Add(user.Id);
                });
            });
            connection.On<string>("LogoutMessage", (userid) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var result = ReturnHttpResult(string.Format(url, userid));
                    var user = JsonConvert.DeserializeObject<User>(result);
                    Userlist.Items.Remove(user.Id);
                });
            });

            try
            {
                await connection.StartAsync();
                messagesList.Items.Add("Connection started");
                connectButton.IsEnabled = false;
                sendButton.IsEnabled = true;
                var url1 = "https://localhost:44386/User/Login?username={0}&password={1}";
                var username = User.Text.Trim();
                var password = Password.Text.Trim();
                var result = await Task.Run(() => ReturnHttpResult(string.Format(url1, username, password)));
                var user = JsonConvert.DeserializeObject<User>(result);
                var userid = user.Id;
                await connection.InvokeAsync<bool>("Login", userid);
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.InvokeAsync("SendMessage", "HUJDSH6757687", "Hellooo");
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

        private string ReturnHttpResult(string url)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "application/json";
                string responseData = string.Empty;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseData = reader.ReadToEnd();
                        reader.Close();
                    }
                    response.Close();
                }
                return responseData;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
