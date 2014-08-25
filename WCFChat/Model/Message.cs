using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum EMessageType
    {
        Say = 0,
        Whisper = 1,
        System = 2
    }

    [DataContract]
    public class Message
    {
        [DataMember] 
        public string Author { get; set; }

        [DataMember]
        public string Recipient { get; set; }

        [DataMember] 
        public string Text { get; set; }

        [DataMember] 
        public EMessageType MessageType { get; set; }

        public Message(string author, string recipient, string text, EMessageType type)
        {
            Author = author;
            Recipient = recipient;
            Text = text;
            MessageType = type;
        }

        public Message(string author, string text, EMessageType type)
            : this(author, "all", text, type)
        {
        }       
    }
}
