using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ChatClient.Dialog;
using ChatClient.ServerContent;
using ChatClient.ServiceReference1;
using Model;

namespace ChatClient
{
    public partial class MainForm : Form
    {
        private enum ClientMessageType
        {
            Say,
            WhisperToMe,
            WhisperFromMe,
            System,
        }

        private ChannelFactory<IChatService> remoteFactory;
        private IChatService remoteProxy;
        private SettingsForm settingsForm;
        private TcpListener tcpListener;
        private Person me;
        private bool isConnected = false;
        private ChatExtendedFunctions chatExtFunctions;
        private ServerViewer serverViewer;

        public MainForm()
        {
            InitializeComponent();
            Init();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isConnected)
            {
                DialogResult result = MessageBox.Show("You are still connected!\r\n\r\nDo you want to continue?", "Close", MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    this.Disconnect();
                }
            }
        }
        private void verbindenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Auslesen der Adresse zur Laufzeit
            EndpointIdentity endId = EndpointIdentity.CreateSpnIdentity("chatclient");
            Uri uri = new Uri(ConfigurationManager.AppSettings["serviceURI"]);
            var address = new EndpointAddress(uri, endId);

            //Einbinden des Services
            remoteFactory = new ChannelFactory<IChatService>("WSHttpBinding_IChatService", address);
            remoteProxy = remoteFactory.CreateChannel();

            isConnected = this.Connect(this.GetIP(settingsForm.Address), int.Parse(settingsForm.Port));

            if (isConnected)
            {
                serverViewer = new ServerViewer(treeViewServer, remoteProxy);

                treeViewServer = serverViewer.Update(remoteProxy);

                tmrUpdate.Start();
            }
            else
            {
                this.WriteNewMessageToChat("Verbindung fehlgeschlagen", ClientMessageType.System, "System");
            }
        }
        private void verbindungTrennenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Disconnect();
        }
        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void benutzernameÄndernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsForm = new SettingsForm();
            settingsForm.Show();
        }
        private void txtBoxWhisperUsername_MouseClick(object sender, MouseEventArgs e)
        {
            if (txtBoxWhisperUsername.Text == "<Benutzer>")
            {
                txtBoxWhisperUsername.Text = string.Empty;
            }
        }
        private void txtBoxChatEnter_MouseClick(object sender, MouseEventArgs e)
        {
            if (txtBoxChatEnter.Text == "Enter text here...")
            {
                txtBoxChatEnter.Text = string.Empty;
            }
        }
        private void txtBoxChatEnter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnChatEnter_Click(sender, e);

                e.Handled = true;
            }           
        }
        private void txtBoxChatEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                string olderMessage = chatExtFunctions.GetNextOlderMessage();

                if (!String.IsNullOrEmpty(olderMessage))
                {
                    txtBoxChatEnter.Text = olderMessage;
                    txtBoxChatEnter.SelectionStart = txtBoxChatEnter.TextLength;
                }

                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                string olderMessage = chatExtFunctions.GetNextYoungerMessage();

                if (!String.IsNullOrEmpty(olderMessage))
                {
                    txtBoxChatEnter.Text = olderMessage;
                    txtBoxChatEnter.SelectionStart = txtBoxChatEnter.TextLength;
                }

                e.Handled = true;
            }
        }
        private void btnChatEnter_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBoxChatEnter.Text))
            {
                string chatText = txtBoxChatEnter.Text;
                txtBoxChatEnter.Text = string.Empty;

                chatExtFunctions.AddMyNewMessage(chatText);

                if (cboBoxMessageType.SelectedItem == cboBoxMessageType.Items[0])
                {
                    //Sagen
                    this.SendNewMessage(chatText, ClientMessageType.Say);
                }
                else if (cboBoxMessageType.SelectedItem == cboBoxMessageType.Items[1])
                {
                    //Flüstern
                    this.SendNewMessage(chatText, ClientMessageType.WhisperFromMe, txtBoxWhisperUsername.Text);
                }
            }
        }
        private void cboBoxMessageType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.SwitchChatType();
        }
        private void treeViewServer_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 1)
            {
                Channel selectedChannel = (Channel)e.Node.Tag;

                //Dienst - SwitchChannel();
                bool switchedCorrect = serverViewer.SwitchChannel(selectedChannel.Id, me.UserName, remoteProxy);

                if (!switchedCorrect)
                {
                    this.WriteNewMessageToChat("You could not switch the channel!", ClientMessageType.System, "System");
                }
            }
            else if (e.Node.Level >= 2)
            {
                //Flüstern
                cboBoxMessageType.SelectedItem = cboBoxMessageType.Items[1];

                this.SwitchChatType();

                txtBoxWhisperUsername.Text = e.Node.Text;
            }
        }
        private void bckGrWorkerTCP_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker tempBckGrWorker = sender as BackgroundWorker;
            TcpListener tempTcpListener = (TcpListener)e.Argument;

            e.Result = RunTCP(tempBckGrWorker, tempTcpListener);

            if (tempBckGrWorker.CancellationPending)
            {
                e.Cancel = true;
            }
        }
        private void bckGrWorkerTCP_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {

            }
            else if (e.Error != null)
            {
                throw new Exception(e.Error.Message, e.Error);
            }
            else
            {
                try
                {
                    //Serverantwort - hier unwichtig
                    TcpClient tcpClient = (TcpClient)e.Result;
                    Stream newstream = tcpClient.GetStream();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                this.bckGrWorkerTCP.RunWorkerAsync(tcpListener);
            }
        }
        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            if (this.AnyUserSwitched())
            {
                treeViewServer = serverViewer.Update(remoteProxy);
            }

            Model.Message[] messages = remoteProxy.RetrieveMessages(me.UserName);

            if (messages != null && messages.Length > 0 && messages[0] != null)
            {
                foreach (Model.Message message in messages)
                {
                    if (message.MessageType == EMessageType.Say)
                    {
                        this.WriteNewMessageToChat(message.Text, ClientMessageType.Say, message.Author, message.Recipient);
                    }
                    else if (message.MessageType == EMessageType.Whisper)
                    {
                        if (me.UserName == message.Author)
                        {
                            this.WriteNewMessageToChat(message.Text, ClientMessageType.WhisperFromMe, message.Author, message.Recipient);
                        }

                        if (me.UserName == message.Recipient)
                        {
                            this.WriteNewMessageToChat(message.Text, ClientMessageType.WhisperToMe, message.Author, message.Recipient);
                        }
                    }
                    else
                    {
                        this.WriteNewMessageToChat(message.Text, ClientMessageType.System, message.Author, message.Recipient);
                    }
                }
            }
        }

        private void Init()
        {
            this.Height = 600;
            this.Width = 800;

            this.ChatEnable(false);

            cboBoxMessageType.SelectedItem = cboBoxMessageType.Items[0];

            chatExtFunctions = new ChatExtendedFunctions();

            settingsForm = new SettingsForm();

            me = new Person();
            me.UserName = string.Empty;
        }
        private bool Connect(IPAddress address, int port)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (address == null)
                {
                    MessageBox.Show("Connection failed!\r\n\r\nThe given address is incorrect!", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }

                //tcpListener = new TcpListener(address, port);
                //tcpListener.Start();
                //this.bckGrWorkerTCP.RunWorkerAsync(tcpListener);

                Random random = new Random();
                me.UserName = (!String.IsNullOrEmpty(Properties.Settings.Default.Username)) ? Properties.Settings.Default.Username : "user_" + (random.Next(0, 999).ToString());

                //Verbindung zum Server aufbauen
                if (remoteProxy.Connect(me.UserName))
                {
                    //Chat-Komponenten enablen
                    this.ChatEnable(true);

                    this.Cursor = Cursors.Default;

                    return true;
                }
                else
                {
                    this.WriteNewMessageToChat("You could not connect to the server.", ClientMessageType.System, "System");

                    this.Cursor = Cursors.Default;

                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection failed!\r\n\r\n" + ex.Message, "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Cursor = Cursors.Default;

                return false;
            }
        }
        private void Disconnect()
        {
            //Verbindung zum Server trennen
            bool serviceDisconnected = remoteProxy.Disconnect(me.UserName);

            if (serviceDisconnected)
            {
                //Aufhören, Nachrichten vom Server abzufragen
                tmrUpdate.Stop();

                this.WriteNewMessageToChat("You are disconnected.", ClientMessageType.System, "System");

                //Verbindung trennen
                //bckGrWorkerTCP.CancelAsync();

                //Chat-Komponenten disablen
                this.ChatEnable(false);

                treeViewServer.Nodes.Clear();

                isConnected = false;
            }
            
        }
        private void WriteNewMessageToChat(string newMessage, ClientMessageType chatType, string fromUsername, string ToUsername = "")
        {
            DateTime date = DateTime.Now;
            string dateFormatted = date.ToString("dd.MM.yyyy HH:mm");

            string newChatLine = "";

            if (chatType == ClientMessageType.Say)
            {
                newChatLine = "[" + dateFormatted + "] " + fromUsername + ": " + newMessage + "\r\n";

                chatExtFunctions.AddTextToChat(ref rtxtBoxChat, newChatLine);
            }
            else if (chatType == ClientMessageType.WhisperToMe)
            {
                newChatLine = "[" + dateFormatted + "] " + fromUsername + " (whispers to you): " + newMessage + "\r\n";

                chatExtFunctions.AddTextToChat(ref rtxtBoxChat, newChatLine, Color.Green);
            }
            else if (chatType == ClientMessageType.WhisperFromMe)
            {
                newChatLine = "[" + dateFormatted + "] You whisper " + ToUsername + ": " + newMessage + "\r\n";

                chatExtFunctions.AddTextToChat(ref rtxtBoxChat, newChatLine, Color.Green);
            }
            else if (chatType == ClientMessageType.System)
            {
                newChatLine = "[" + dateFormatted + "] System:: " + newMessage + "\r\n";

                chatExtFunctions.AddTextToChat(ref rtxtBoxChat, newChatLine, Color.Red);
            }

            txtBoxChatEnter.Focus();
        }
        private void SendNewMessage(string myNewMessage, ClientMessageType chatType, string ToUsername = "")
        {
            if (chatType == ClientMessageType.Say)
            {
                remoteProxy.Say(me.UserName, myNewMessage);
            }
            else if (chatType == ClientMessageType.WhisperFromMe)
            {
                if (!String.IsNullOrEmpty(ToUsername))
                {
                    remoteProxy.Whisper(me.UserName, ToUsername, myNewMessage);
                }
                else
                {
                    this.WriteNewMessageToChat("A user for the whisper is missing.", ClientMessageType.System, "System");
                }
            }
        }
        private void ChatEnable(bool enable)
        {
            if (enable)
            {
                txtBoxChatEnter.Enabled = true;
                txtBoxWhisperUsername.Enabled = true;
                rtxtBoxChat.Enabled = true;
                cboBoxMessageType.Enabled = true;
                btnChatEnter.Enabled = true;

                verbindenToolStripMenuItem.Enabled = false;
                einstellungenToolStripMenuItem.Enabled = false;
            }
            else
            {
                txtBoxChatEnter.Enabled = false;
                txtBoxWhisperUsername.Enabled = false;
                rtxtBoxChat.Enabled = false;
                cboBoxMessageType.Enabled = false;
                btnChatEnter.Enabled = false;

                verbindenToolStripMenuItem.Enabled = true;
                einstellungenToolStripMenuItem.Enabled = true;
            }
        }
        private void SwitchChatType()
        {
            int push = 108;

            if (cboBoxMessageType.SelectedItem == cboBoxMessageType.Items[0])
            {
                //Sagen-Chat
                txtBoxChatEnter.Width += push;
                txtBoxChatEnter.Location = new Point(77, 486);

                txtBoxWhisperUsername.Visible = false;
                lblWhisper.Visible = false;
            }
            else if (cboBoxMessageType.SelectedItem == cboBoxMessageType.Items[1])
            {
                //Flüstern-Chat
                txtBoxWhisperUsername.Text = "<user>";
                txtBoxWhisperUsername.Visible = true;
                lblWhisper.Visible = true;

                txtBoxChatEnter.Width -= push;
                txtBoxChatEnter.Location = new Point(185, 486);

                txtBoxWhisperUsername.Focus();
            }
        }
        private TcpClient RunTCP(BackgroundWorker bcckGrWorker, TcpListener tcpListener)
        {
            TcpClient tempTcpClient = new TcpClient();

            while (!bcckGrWorker.CancellationPending)
            {

                for (; ; )
                {
                    tempTcpClient = tcpListener.AcceptTcpClient();

                    return tempTcpClient;
                }

            }

            return null;
        }
        private IPAddress GetIP(string address)
        {
            IPAddress ipAddress = null;
            bool isIP = IPAddress.TryParse(address, out ipAddress);

            if (!isIP)
            {
                try
                {
                    return Dns.GetHostAddresses(address)[0];
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return IPAddress.Parse(address);
            }
        }
        private bool AnyUserSwitched()
        {
            Person[] allUsers = remoteProxy.GetUsers();

            foreach (Person user in allUsers)
            {
                if (remoteProxy.HasSwitchedChannel(user.UserName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}