using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string FromUserName { get; set; }

        [DataMember]
        public string ToUserName { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public bool IsPrivate { get; set; }

        public Message(string text, string fromUserName, string toUserName = null)
        {
            Text = text;
            FromUserName = fromUserName;
            ToUserName = toUserName;

            if (!String.IsNullOrEmpty(toUserName))
            {
                IsPrivate = true;
            }
            else
            {
                IsPrivate = false;
            }
        }
    }
}