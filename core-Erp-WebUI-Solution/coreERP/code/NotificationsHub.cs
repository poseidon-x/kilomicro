using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP
{
    public class NotificationsHub:Hub
    {
        public void sendMessage(string userName, string message, int noOfMessages)
        {
            if (Global.htUsers_ConIds.ContainsKey(userName.ToLower()) == true)
            {
                Clients.Client(Global.htUsers_ConIds[userName.ToLower()])
                    .sendMessage(userName, message, noOfMessages);
            } 
        }

        public void sendAck(string userName)
        {
            if (Global.Acks.ContainsKey(userName.ToLower()))
            {
                Global.Acks[userName.ToLower()] = new Global.Ack
                {
                    UserName = userName.ToLower(),
                    LastAck = DateTime.Now
                };
            }
            else
            {
                Global.Acks.Add(userName.ToLower(), new Global.Ack
                {
                    UserName = userName.ToLower(),
                    LastAck = DateTime.Now
                });
            }
        }

        public bool getAck(string userName)
        {
            if (Global.Acks.ContainsKey(userName.ToLower()) &&
                (DateTime.Now - Global.Acks[userName.ToLower()].LastAck).TotalMinutes < 60)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void registerConId(string userID)
        {
            bool alreadyExists = false;
            if (Global.htUsers_ConIds.Count == 0)
            {
                Global.htUsers_ConIds.Add(userID, Context.ConnectionId);
            }
            else
            {
                foreach (string key in Global.htUsers_ConIds.Keys)
                {
                    if (key.ToLower() == userID.ToLower())
                    {
                        Global.htUsers_ConIds[key.ToLower()] = Context.ConnectionId;
                        alreadyExists = true;
                        break;
                    }
                }
                if (!alreadyExists)
                {
                    Global.htUsers_ConIds.Add(userID.ToLower(), Context.ConnectionId);
                }
            }
        }
    }
}