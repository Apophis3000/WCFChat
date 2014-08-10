using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WCFChat
{
    [DataContract]
    public class Channel
    {
        [DataMember]
        public List<Person> Users { get; set; }

        [DataMember]
        public string Name { get; set; }

        public Channel(string name)
        {
            Users = new List<Person>();
            Name = name;
        }
    }
}
