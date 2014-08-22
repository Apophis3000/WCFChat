using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{
    public class TestService
    {
        private int channelCount = 5;
        private bool serverViewChanged = true;
        private Dictionary<string, int> usersAndChannels = new Dictionary<string, int>();
        private List<Message> messages = new List<Message>();

        public string Connect(string username)
        {
            int temp = -1;
            if (!usersAndChannels.TryGetValue(username, out temp))
            {
                usersAndChannels.Add(username, 0);

                return "### Willkommen auf unserem Chatserver ###";
            }
            else
            {
                return string.Empty;
            }
        }
        public bool Disconnect(string username)
        {
            if (usersAndChannels.ContainsKey(username))
            {
                usersAndChannels.Remove(username);

                return true;
            }
            else
            {
                return false;
            }
        }
        public void Say(string message, string fromUsername)
        {
            Message newMessage = new Message();
            newMessage.MessageType = 0;
            newMessage.Text = message;
            newMessage.FromUsername = fromUsername;
            newMessage.ToUsername = string.Empty;

            this.messages.Add(newMessage);
        }
        public void Whisper(string message, string fromUsername, string toUsername)
        {
            Message newMessage = new Message();
            newMessage.MessageType = 1;
            newMessage.Text = message;
            newMessage.FromUsername = fromUsername;
            newMessage.ToUsername = toUsername;

            this.messages.Add(newMessage);
        }
        public List<Message> GetMessages(string username)
        {
            List<Message> leftMessages = new List<Message>();
            List<Message> userMessages = new List<Message>();

            int channelIdUser = -1;
            bool userConnected = usersAndChannels.TryGetValue(username, out channelIdUser);

            if (userConnected)
            {
                foreach (Message mes in messages)
                {
                    if (mes.MessageType == 0) //Say
                    {
                        if (UserSameChannel(channelIdUser, mes.FromUsername))
                        {
                            userMessages.Add(mes);
                        }
                        else
                        {
                            leftMessages.Add(mes);
                        }
                    }
                    else if (mes.MessageType == 1) //Whisper
                    {
                        if (mes.ToUsername == username || mes.FromUsername == username)
                        {
                            userMessages.Add(mes);
                        }
                        else
                        {
                            leftMessages.Add(mes);
                        }
                    }
                }

                messages = leftMessages;

                return userMessages;
            }
            else
            {
                return null;
            }
        }
        public int GetCountChannels()
        {
            return channelCount;
        }
        public bool NeedServerViewUpdate()
        {
            return serverViewChanged;
        }
        public List<List<string>> GetUsersAndTheirChannels()
        {
            List<List<string>> usersInChannels = new List<List<string>>();

            for (int index = 0; index < channelCount; index++)
            {
                usersInChannels.Add(new List<string>());
            }

            foreach (string user in usersAndChannels.Keys)
            {
                int channelId = -1;
                usersAndChannels.TryGetValue(user, out channelId);

                usersInChannels[channelId].Add(user);
            }

            serverViewChanged = false;

            return usersInChannels;
        }
        public string SwitchChannel(int channelId, string username)
        {
            int channelIdUser = -1;
            bool userConnected = usersAndChannels.TryGetValue(username, out channelIdUser);

            if (userConnected && channelIdUser != -1 && channelId != channelIdUser)
            {
                usersAndChannels[username] = channelId;

                serverViewChanged = true;

                if (channelId > 0)
                    return "### Du bist jetzt im Channel - " + channelId.ToString() + " ###";
                else
                    return "### Du bist jetzt in der Lobby ###";
            }

            return string.Empty;
        }

        private bool UserSameChannel(int channelId, string username)
        {
            int tempChannelId = -1;
            if (usersAndChannels.TryGetValue(username, out tempChannelId))
            {
                return (channelId == tempChannelId);
            }

            return false;
        }
    }
}