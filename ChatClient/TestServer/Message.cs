using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{
    public class Message
    {
        public string Text
        {
            get;
            set;
        }
        public string FromUsername
        {
            get;
            set;
        }
        public string ToUsername
        {
            get;
            set;
        }
        //0 = Say, 1 = Whisper
        public int MessageType
        {
            get;
            set;
        }
    }
}