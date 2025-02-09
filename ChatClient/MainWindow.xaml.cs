using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatClient.ServiceChat;
using static ChatClient.MainWindow;
namespace ChatClient
{
    public partial class MainWindow : Window, IServiceChatCallback
    {
        bool isConnected = false;
        ServiceChatClient client;
        int Id;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        void ChangeButtonToDisconnect()
        {
            textboxUserName.IsEnabled = false;
            buttonConnDicconn.Content = "Disconnect";
        }

        void ChangeButtonToConnect()
        {
            textboxUserName.IsEnabled = true;
            buttonConnDicconn.Content = "Connect";
        }

        public class User
        {
            public UserId Id { get; }
            public string Name { get; }

            public User(string name)
            {
                Name = name;
                Id = new UserId(name.GetHashCode());
            }
        }
        void ConnectUser()
        {
            if (!isConnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                var user = new User(textboxUserName.Text);
                userId = user.Id;
                ChangeButtonToDisconnect();
                isConnected = true;
            }
        }

        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(userId); 
                client = null;
                ChangeButtonToConnect();
                isConnected = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }


        }

        public class ChatMessage
        {
            public string Message { get; set; }
            public DateTime Timestamp { get; set; }
            public string Author { get; set; }

            public ChatMessage(string message, string author)
            {
                Message = message;
                Author = author;
                Timestamp = DateTime.Now;
            }

            public override string ToString()
            {
                return $"{Timestamp.ToShortTimeString()} {Author}: {Message}";
            }
        }

        public void MessageCallBack(ChatMessage chatMessage)
        {
            listboxChat.Items.Add(chatMessage);
            listboxChat.ScrollIntoView(listboxChat.Items[listboxChat.Items.Count - 1]);
        }


        private void Winsdow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            DisconnectUser();
        }

        private void textboxMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (client != null)
                {
                    var chatMessage = new ChatMessage(textboxMessage.Text, "Username");
                    client.SendMessage(chatMessage.Message, userId);
                    textboxMessage.Text = string.Empty;
                }
            }
        }
    }
}