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

        void IChatService.Disconnect(string p)
        {
            engine.Disconnect(p); 
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
    }
}
