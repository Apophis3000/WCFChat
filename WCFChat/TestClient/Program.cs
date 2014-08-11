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

            bool val1 = service.Connect(p1.UserName);
            bool val2 = service.Connect(p2.UserName);

            Console.WriteLine(service.RetrieveMessages(p1.UserName)[0].Text);

            service.Say(p1.UserName, "Hallo Welt");
            Console.WriteLine(service.RetrieveMessages(p1.UserName)[0].Text);
            Console.WriteLine(service.RetrieveMessages(p2.UserName)[1].Text);

            service.Whisper(p1.UserName, p2.UserName, "Hallo Rexxar!");

            Console.WriteLine(service.RetrieveMessages(p1.UserName)[0].Text);
            Console.WriteLine(service.RetrieveMessages(p2.UserName)[0].Text);

            Console.ReadLine();            
        }
    }
}
