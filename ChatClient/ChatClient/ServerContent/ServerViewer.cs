using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Windows.Forms;

using Model;
using ChatClient.ServiceReference1;

namespace ChatClient.ServerContent
{
    public class ServerViewer
    {
        List<Channel> serverChannels;
        private TreeView treeViewServer;

        public ServerViewer(TreeView treeViewServer, IChatService remoteProxy)
        {
            this.treeViewServer = treeViewServer;

            //Channel-Struktur aufbauen
            this.BuildServerStructure(remoteProxy);
        }

        public TreeView Update(IChatService remoteProxy, Person me)
        {
            Person[] allUsers = remoteProxy.GetUsers();

            //Serveransicht aktualisieren
            treeViewServer.BeginUpdate();

            foreach (Channel serverChannel in serverChannels)
            {
                List<string> usersInChannel = new List<string>();

                foreach (Person user in allUsers)
                {
                    if (user.CurrentChannelName == serverChannel.Name)
                    {
                        usersInChannel.Add(user.UserName);
                    }
                }

                UpdateUsersInChannel(serverChannel.Id, usersInChannel, me);
                usersInChannel.Clear();
            }

            treeViewServer.EndUpdate();
            treeViewServer.ExpandAll();

            return treeViewServer;
        }
        public bool SwitchChannel(int channelId, string username, IChatService remoteProxy)
        {
            try
            {
                remoteProxy.SwitchChannel(username, serverChannels[channelId].Name);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void BuildServerStructure(IChatService remoteProxy)
        {
            string[] channelNames = remoteProxy.GetChannels();

            serverChannels = new List<Channel>();
            int channelId = 0;

            foreach (string channelName in channelNames)
            {
                serverChannels.Add(new Channel(channelId, channelName));
                channelId++;
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
        private void UpdateUsersInChannel(int channelId, List<string> users, Person me)
        {
            Channel tempChannel = (Channel)treeViewServer.Nodes[0].Nodes[channelId].Tag;
            tempChannel.ContainingUsers = users;

            treeViewServer.Nodes[0].Nodes[channelId].Tag = tempChannel;

            treeViewServer.Nodes[0].Nodes[channelId].Nodes.Clear();

            for (int index = 0; index < users.Count; index++)
            {
                treeViewServer.Nodes[0].Nodes[channelId].Nodes.Add(users[index]);
                treeViewServer.Nodes[0].Nodes[channelId].Nodes[index].ForeColor = Color.Green;

                if (users[index] == me.UserName)
                {
                    treeViewServer.Nodes[0].Nodes[channelId].Nodes[index].NodeFont = new Font(treeViewServer.Font, FontStyle.Bold);
                }
            }
        }
    }
}