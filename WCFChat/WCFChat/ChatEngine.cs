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

        public Dictionary<string, List<Person>> GetChannels()
        {
            return channels; 
        }

        public bool Connect(string user)
        {
            if (channels.Any(x => x.Value.Find(y => y.UserName == user) != null))
            {
                messages[user].Add(new Message(user, "User name not available!", EMessageType.System));
                return false;
            }

            Person p = new Person() { UserName = user, IsConnected = true };
            p.LastChannelName = "DISCONNECTED";

            messages.Add(user, new List<Message>());            
            
            messages[user].Add(new Message(user, "Welcome to the chat!", EMessageType.System));
            JoinChannel(p, "Lobby");
            return true;
        }

        public bool Disconnect(string user)
        {
            bool success = false;

            foreach (var channel in channels)
            {
                Person leaver = channel.Value.Find(x => x.UserName == user);

                if (leaver != null)
                {
                    leaver.CurrentChannelName = "DISCONNECTED";
                    channel.Value.Remove(leaver);
                    success = true;
                }
            }
                        
            messages.Remove(user);
            return success;
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
                    messages[sender].Add(new Message(sender, recipient, message, EMessageType.Whisper));

                    if (sender != recpt.UserName)
                    {
                        messages[recpt.UserName].Add(new Message(sender, recipient, message, EMessageType.Whisper));
                    }

                    return;
                }
            }        

            messages[sender].Add(new Message(sender, recipient, "User is currently not connected", EMessageType.System));
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
            Person p = LeaveCurrentChannel(user);
            if (p != null)
            {
                JoinChannel(p, channelName);
            }
        }

        public bool HasSwitchedChannel(string user)
        {
            bool result = true;

            foreach (var c in channels)
            {
                Person p = c.Value.Find(x => x.UserName == user);

                if (p != null)
                {
                    result =  p.LastChannelName != p.CurrentChannelName;
                    p.LastChannelName = p.CurrentChannelName;
                }
            }

            return result;
        }

        private void JoinChannel(Person p, string channelName)
        {
            foreach (var c in channels)
            {
                if (c.Key == channelName)
                {
                    c.Value.Add(p);
                   
                    p.CurrentChannelName = channelName;

                    foreach (var u in c.Value)
                    {
                        if (u.UserName != p.UserName)
                        {
                            messages[u.UserName].Add(new Message("system", string.Format("{0} has joined the channel.", p.UserName), EMessageType.System));
                        }
                    }
                }
            }

            messages[p.UserName].Add(new Message("system", string.Format("Welcome to the channel {0}!", channelName), EMessageType.System));                    
        }

        private Person LeaveCurrentChannel(string user)
        {
            foreach (var c in channels)
            {
                Person p = c.Value.Find(x => x.UserName == user);

                if (p != null)
                {
                    c.Value.Remove(p);
                    p.LastChannelName = c.Key;

                    foreach (var u in c.Value)
                    {
                        messages[u.UserName].Add(new Message("system", string.Format("{0} has left the channel.", p.UserName), EMessageType.System));
                    }
                    return p;
                }
            }

            return null;
        }

        public List<string> GetChannelNames()
        {
            return channels.Keys.ToList();
        }
    }
}
