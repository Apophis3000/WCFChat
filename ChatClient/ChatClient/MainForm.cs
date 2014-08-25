using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private TestServer.TestService testService = new TestServer.TestService(); //old

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

        private void MainForm_Load(object sender, EventArgs e)
        {
            settingsForm = new SettingsForm();

            me = new Person();
            me.UserName = string.Empty;
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isConnected)
            {
                DialogResult result = MessageBox.Show("Sie sind noch verbunden.\r\n\r\nWollen Sie die Anwendung wirklich beenden?", "Beenden", MessageBoxButtons.YesNo);

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
            remoteFactory = new ChannelFactory<IChatService>("WSHttpBinding_IChatService");
            remoteProxy = remoteFactory.CreateChannel();

            isConnected = this.Connect(this.GetIP(settingsForm.Address), int.Parse(settingsForm.Port));

            if (isConnected)
            {
                serverViewer = new ServerViewer(treeViewServer, remoteProxy);

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
            if (txtBoxChatEnter.Text == "Text hier eingeben...")
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
                string systemText = serverViewer.SwitchChannel(selectedChannel.Id, me.UserName, remoteProxy);

                if (!String.IsNullOrEmpty(systemText))
                {
                    this.WriteNewMessageToChat(systemText, ClientMessageType.System, "System");
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
            if (testService.NeedServerViewUpdate()) //Ersetzen??
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
                        this.WriteNewMessageToChat(message.Text, ClientMessageType.Say, message.Author, message.ToUsername);
                    }
                    else if (message.MessageType == EMessageType.Whisper)
                    {
                        if (me.UserName == message.Author)
                        {
                            this.WriteNewMessageToChat(message.Text, ClientMessageType.WhisperFromMe, message.Author, message.ToUsername);
                        }
                        
                        if (me.UserName == message.ToUsername)
                        {
                            this.WriteNewMessageToChat(message.Text, ClientMessageType.WhisperToMe, message.Author, message.ToUsername);
                        }
                    }
                    else
                    {
                        this.WriteNewMessageToChat(message.Text, ClientMessageType.System, message.Author, message.ToUsername);
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
        }
        private bool Connect(IPAddress address, int port)
        {
            //if (isConnected)
            //{
            //    this.Disconnect();

            //    isConnected = false;
            //}

            try
            {
                if (address == null)
                {
                    MessageBox.Show("Es konnte keine Verbindung hergestellt werden.\r\n\r\nDie angegebene Adresse ist ungültig.", "Verbindungsfehler", MessageBoxButtons.OK);

                    return false;
                }

                tcpListener = new TcpListener(address, port);
                tcpListener.Start();

                this.bckGrWorkerTCP.RunWorkerAsync(tcpListener);

                Random random = new Random();
                me.UserName = (!String.IsNullOrEmpty(Properties.Settings.Default.Username)) ? Properties.Settings.Default.Username : "Benutzer_" + (random.Next(0, 999).ToString());

                //Verbindung zum Server aufbauen
                if (remoteProxy.Connect(me.UserName))
                {
                    this.WriteNewMessageToChat("### Willkommen auf unserem Chatserver ###", ClientMessageType.System, "System");

                    //Chat-Komponenten enablen
                    this.ChatEnable(true);

                    return true;
                }
                else
                {
                    this.WriteNewMessageToChat("Verbindung zum Serverdienst konnte nicht hergestellt werden.", ClientMessageType.System, "System");

                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Es konnte keine Verbindung hergestellt werden.\r\n\r\n" + ex.Message, "Verbindungsfehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }
        private void Disconnect()
        {
            //Verbindung zum Server trennen
            bool serviceDisconnected = testService.Disconnect(me.UserName);
            remoteProxy.Disconnect(me.UserName); //bool-Rückgabe fehlt!!

            if (serviceDisconnected)
            {
                //Verbindung trennen
                bckGrWorkerTCP.CancelAsync();

                //Chat-Komponenten disablen
                this.ChatEnable(false);

                treeViewServer.Nodes.Clear();

                this.WriteNewMessageToChat("Verbindung beendet.", ClientMessageType.System, "System");

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
                newChatLine = "[" + dateFormatted + "] " + fromUsername + " (flüstert dir): " + newMessage + "\r\n";

                chatExtFunctions.AddTextToChat(ref rtxtBoxChat, newChatLine, Color.Green);
            }
            else if (chatType == ClientMessageType.WhisperFromMe)
            {
                newChatLine = "[" + dateFormatted + "] Du flüsterst " + ToUsername + ": " + newMessage + "\r\n";

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
                    MessageBox.Show("Sie haben keinen Benutzer angegeben zum Anflüstern.", "Fehler", MessageBoxButtons.OK);
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
            }
            else
            {
                txtBoxChatEnter.Enabled = false;
                txtBoxWhisperUsername.Enabled = false;
                rtxtBoxChat.Enabled = false;
                cboBoxMessageType.Enabled = false;
                btnChatEnter.Enabled = false;
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
                txtBoxWhisperUsername.Text = "<Benutzer>";
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
    }
}
