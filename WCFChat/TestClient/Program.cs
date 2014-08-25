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
        static void PrintMessages(List<Message> msgs)
        {
            foreach (var msg in msgs)
            {
                Console.WriteLine(msg.Text);
            }
        }

        static void Main(string[] args)
        {
            ChatService.ChatServiceClient service = new ChatService.ChatServiceClient();
            Person p1 = new Person("Thrall");
            Person p2 = new Person("Rexxar");

            bool val1 = service.Connect(p1.UserName);
            bool val2 = service.Connect(p2.UserName);

            PrintMessages(service.RetrieveMessages(p1.UserName).ToList());

            service.Say(p1.UserName, "Hallo Welt");
            PrintMessages(service.RetrieveMessages(p1.UserName).ToList());
            PrintMessages(service.RetrieveMessages(p2.UserName).ToList());

            service.SwitchChannel("Rexxar", "Channel1");

            service.Whisper(p1.UserName, p2.UserName, "Hallo Rexxar!");

            PrintMessages(service.RetrieveMessages(p1.UserName).ToList());
            PrintMessages(service.RetrieveMessages(p2.UserName).ToList());

            var users = service.GetUsers();

            bool t = service.HasSwitchedChannel(p1.UserName);
            bool t2 = service.HasSwitchedChannel(p2.UserName);

            Console.ReadLine();            
        }
    }
}
