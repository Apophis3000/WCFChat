using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFChat
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ChatService : IChatService
    {
        static List<Person> connections;
        static List<Channel> channels;

        public ChatService()
        {
            connections = new List<Person>();
            channels = new List<Channel>();

            channels.Add(new Channel("Lobby"));
            channels.Add(new Channel("Channel1"));
            channels.Add(new Channel("Channel2"));
            channels.Add(new Channel("Channel3"));
        }

        bool IChatService.Connect(Person p)
        {
            if (connections.Find(x => x.UserName == p.UserName) != null)
            {
                return false;                
            }

            connections.Add(p);
            p.IsConnected = true;

            SwitchChannel(p, "Lobby");
            
            return true;
        }

        void SwitchChannel(Person person, string channelName)
        {
            foreach (Channel c in channels)
            {
                Person p = c.Users.Find(x => x.UserName == person.UserName);
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
        }
    }
}
