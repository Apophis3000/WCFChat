using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFChat
{
    [ServiceContract(SessionMode = SessionMode.Required, 
        CallbackContract = typeof(IChatCallback))]
    public interface IChatService
    {
        [OperationContract]
        bool Connect(string p);

        [OperationContract]
        bool Disconnect(string p);

        [OperationContract]
        void Say(string user, string message);

        [OperationContract]
        void Whisper(string sender, string recipient, string message);

        [OperationContract]
        List<Message> RetrieveMessages(string user);

        [OperationContract]
        void SwitchChannel(string person, string channelName);

        [OperationContract]
        List<string> GetChannels();

        [OperationContract]
        List<Person> GetUsers();

        [OperationContract]
        bool HasSwitchedChannel(string user);
    }
}
