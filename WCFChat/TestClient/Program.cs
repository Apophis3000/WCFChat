using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatService.ChatServiceClient service = new ChatService.ChatServiceClient();
            Person p1 = new Person("Thrall");
            Person p2 = new Person("Rexxar");

            bool val1 = service.Connect(p1);
            bool val2 = service.Connect(p2);

            Console.WriteLine(val1);
            Console.WriteLine(val2);
        }
    }
}
