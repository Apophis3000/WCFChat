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
        static ChatEngine engine = new ChatEngine();

        public ChatService()
        {
        }

        bool IChatService.Connect(string p)
        {
            return engine.Connect(p);
        }

        bool IChatService.Disconnect(string p)
        {
            return engine.Disconnect(p); 
        }


        void IChatService.Say(string user, string message)
        {
            engine.Say(user, message);
        }

        void IChatService.Whisper(string sender, string recipient, string message)
        {
            engine.Whisper(sender, recipient, message);
        }

        List<Message> IChatService.RetrieveMessages(string user)
        {
            return engine.RetrieveMessages(user);
        }

        void IChatService.SwitchChannel(string person, string channelName)
        {
            engine.SwitchChannel(person, channelName);
        }

        List<string> IChatService.GetChannels()
        {
            return engine.GetChannelNames();
        }

        List<Person> IChatService.GetUsers()
        {
            List<Person> users = new List<Person>();

            foreach (var channel in engine.GetChannels())
            {
                foreach(var user in channel.Value)
                {
                    users.Add(user);
                }              
            }

            return users;
        }

        bool IChatService.HasSwitchedChannel(string user)
        {
            return engine.HasSwitchedChannel(user);
        }
    }
}
