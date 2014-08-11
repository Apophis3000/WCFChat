using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFChat
{
    public class ChatEngine
    {
        private Dictionary<string, List<Person>> channels;
        private Dictionary<string, List<Message>> messages;

        public ChatEngine()
        {
            channels = new Dictionary<string, List<Person>>();
            messages = new Dictionary<string, List<Message>>();

            channels.Add("Lobby", new List<Person>());
            channels.Add("Channel1", new List<Person>());
            channels.Add("Channel2", new List<Person>());
            channels.Add("Channel3", new List<Person>());
        }

        public bool Connect(string user)
        {
            if (channels.Any(x => x.Value.Find(y => y.UserName == user) != null))
            {
                messages[user].Add(new Message(user, "User name not available!", EMessageType.System));
                return false;
            }

            Person p = new Person() { UserName = user, IsConnected = true };

            channels["Lobby"].Add(p);
            //SwitchChannel(user, "Lobby");

            messages.Add(user, new List<Message>());
            messages[user].Add(new Message(user, "Welcome to the chat!", EMessageType.System));
            return true;
        }

        public void Disconnect(string user)
        {
            foreach (var channel in channels)
            {
                Person leaver = channel.Value.Find(x => x.UserName == user);

                if (leaver != null)
                {
                    channel.Value.Remove(leaver); 
                }
            }

            messages.Remove(user);
        }

        public void Say(string sender, string message)
        {
            foreach (var channel in channels)
            {
                Person author = channel.Value.Find(x => x.UserName == sender);

                if (author != null)
                {
                    foreach (var user in channel.Value)
                    {
                        messages[user.UserName].Add(new Message(sender, message, EMessageType.Say));
                    }
                }
            }            
        }

        public void Whisper(string sender, string recipient, string message)
        {
            foreach (var channel in channels)
            {
                Person recpt = channel.Value.Find(x => x.UserName == recipient);

                if (recpt != null)
                {
                    messages[sender].Add(new Message(sender, message, EMessageType.Whisper));
                    messages[recpt.UserName].Add(new Message(sender, message, EMessageType.Whisper));
                    return;
                }
            }        

            messages[sender].Add(new Message(sender, "User is currently not connected", EMessageType.System));
        }

        public List<Message> RetrieveMessages(string user)
        {
            List<Message> msgs = new List<Message>();

            foreach (Message m in messages[user])
            {
                msgs.Add(m);
            }
            
            messages[user].Clear();
            return msgs;
        }

        public void SwitchChannel(string user, string channelName)
        {
            /*
            foreach (Channel c in channels)
            {
                Person p = c.Users.Find(x => x.UserName == user.UserName);
                if (p != null)
                {
                    c.Users.Remove(p);
                }
            }

            Channel newChannel = channels.Find(x => x.Name == channelName);

            if (!newChannel.Users.Contains(person))
            {
                newChannel.Users.Add(person);                
            }
             */
        }

        public List<string> GetChannelNames()
        {
            return channels.Keys.ToList();
        }
    }
}
