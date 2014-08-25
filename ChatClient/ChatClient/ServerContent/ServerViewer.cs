using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.ServiceModel;
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

        public TreeView Update(IChatService remoteProxy)
        {
            //Dienst - Channels und ihre Benutzer
            List<List<string>> usersInChannels = testService.GetUsersAndTheirChannels();
            //remoteProxy. // GetUsers() fehlt!!


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
        public string SwitchChannel(int channelId, string username, IChatService remoteProxy)
        {
            try
            {
                remoteProxy.SwitchChannel(username, serverChannels[channelId].Name);

                return "Du bist dem Channel " + serverChannels[channelId].Name + " beigetreten";
            }
            catch
            {
                return "";
            }
        }

        private void BuildServerStructure(IChatService remoteProxy)
        {
            string[] channelNames = remoteProxy.GetChannels();

            serverChannels = new List<Channel>();

            foreach (string channelName in channelNames)
            {
                serverChannels.Add(new Channel(0, channelName));
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