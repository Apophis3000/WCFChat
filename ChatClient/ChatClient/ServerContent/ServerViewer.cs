using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient.ServerContent
{
    public class ServerViewer
    {
        List<Channel> serverChannels;
        private TreeView treeViewServer;

        public ServerViewer(TreeView treeViewServer, ChatService.ChatServiceClient testService)
        {
            this.treeViewServer = treeViewServer;

            //Channel-Struktur aufbauen
            this.BuildServerStructure(testService);
        }

        public TreeView Update(TestServer.TestService testService)
        {
            //Dienst - Channels und ihre Benutzer
            List<List<string>> usersInChannels = testService.GetUsersAndTheirChannels();

            //Wenn falsche Struktur vom Serverdienst erhalten
            if (usersInChannels.Count != serverChannels.Count)
                return null;

            //Serveransicht aktualisieren
            treeViewServer.BeginUpdate();

            for (int index = 0; index < serverChannels.Count; index++)
            {
                UpdateUsersInChannel(index, usersInChannels[index]);
            }

            treeViewServer.EndUpdate();
            treeViewServer.ExpandAll();

            return treeViewServer;
        }
        public void SwitchChannel(int channelId, string username, ChatService.ChatServiceClient testService)
        {
            //Dienst - SwitchChannel();
            testService.SwitchChannel(username, channelId.ToString());
        }

        private void BuildServerStructure(ChatService.ChatServiceClient testService)
        {
            //Dienst - GetCountChannels();
            int countChannels = testService.GetChannels().Count();

            serverChannels = new List<Channel>();

            serverChannels.Add(new Channel(0, "Lobby"));

            for (int index = 1; index < countChannels; index++)
            {
                serverChannels.Add(new Channel(index, "Channel - " + index));
            }

            TreeNode rootNode = new TreeNode();
            rootNode.Text = "Server";

            foreach (Channel channel in serverChannels)
            {
                TreeNode channelNode = new TreeNode();
                channelNode.Text = channel.Name;
                channelNode.Tag = channel;

                rootNode.Nodes.Add(channelNode);
            }

            this.treeViewServer.Nodes.Add(rootNode);
        }
        private void UpdateUsersInChannel(int channelId, List<string> users)
        {
            Channel tempChannel = (Channel)treeViewServer.Nodes[0].Nodes[channelId].Tag;
            tempChannel.ContainingUsers = users;

            treeViewServer.Nodes[0].Nodes[channelId].Tag = tempChannel;

            treeViewServer.Nodes[0].Nodes[channelId].Nodes.Clear();

            for (int index = 0; index < users.Count; index++)
            {
                treeViewServer.Nodes[0].Nodes[channelId].Nodes.Add(users[index]);
                treeViewServer.Nodes[0].Nodes[channelId].Nodes[index].ForeColor = Color.Green;
            }
        }
    }
}