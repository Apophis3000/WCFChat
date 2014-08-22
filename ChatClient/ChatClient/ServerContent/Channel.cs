using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.ServerContent
{
    public class Channel
    {
        public int Id
        {
            get;
            private set;
        }
        public string Name
        {
            get;
            private set;
        }
        public List<string> ContainingUsers
        {
            get;
            set;
        }

        public Channel(int id, string name)
        {
            this.Id = id;
            this.Name = name;

            ContainingUsers = new List<string>();
        }
    }
}