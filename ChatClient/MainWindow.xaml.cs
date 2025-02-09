﻿using System;
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
namespace ChatClient
{
    public partial class MainWindow : Window , IServiceChatCallback
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

        void ConnectUser()
        {
            if(!isConnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                Id = client.Connect(textboxUserName.Text);
                string userName = textboxUserName.Text;
                textboxUserName.IsEnabled = false;
                buttonConnDicconn.Content = "Disconnect";
                isConnected = true;

            }
        }

        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(Id);
                client = null;
                textboxUserName.IsEnabled = true;
                buttonConnDicconn.Content = "Connect";
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

        public void MessageCallBack(string message)
        {
            listboxChat.Items.Add(message);
            listboxChat.ScrollIntoView(listboxChat.Items[listboxChat.Items.Count-1]);
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
                    client.SendMessage(textboxMessage.Text, Id);
                    textboxMessage.Text = string.Empty;
                }
            }
        }

        


    }
}