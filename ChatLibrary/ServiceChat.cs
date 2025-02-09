using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        int NextID = 1;

        public int Connect(string name)
        {
            ServerUser user = new ServerUser()
            {
                ID = NextID,
                Name = name,
                operationContext = OperationContext.Current
            };
            NextID++;
            
            SendMessage(": " +user.Name+ " підключився до чату!",0);
            users.Add(user);
            return user.ID;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if (user != null)
            {
                users.Remove(user);
                SendMessage(": " +user.Name+ " відключився з чату!",0);
            }
        }

        public void SendMessage(string message, int id)
        {
            foreach(var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();
                var user = users.FirstOrDefault(i => i.ID == id);
                if (user != null)
                {
                    answer += ": " + user.Name + " ";
                }

                answer += message;

                item.operationContext.GetCallbackChannel<IServiceChatCallBack>().MessageCallBack(answer);
            }
        }

        
    }
}