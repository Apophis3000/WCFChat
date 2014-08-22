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
using WcfService;

namespace ChatClient
{
    public partial class MainForm : Form
    {
        private TestServer.TestService testService = new TestServer.TestService();

        private enum MessageType
        {
            Say,
            WhisperToMe,
            WhisperFromMe,
            System,
        }

        private TcpListener tcpListener;
        private string myUsername = string.Empty;
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
            //Dialog für Verbindungsdaten öffnen
            ConnectionForm connectionForm = new ConnectionForm();
            connectionForm.ShowDialog();

            if (!connectionForm.IsCanceled)
            {
                isConnected = this.Connect(this.GetIP(connectionForm.Address), int.Parse(connectionForm.Port));
                if (isConnected)
                {
                    serverViewer = new ServerViewer(treeViewServer, testService);

                    tmrUpdate.Start();
                }
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
                    this.SendNewMessage(chatText, MessageType.Say);
                }
                else if (cboBoxMessageType.SelectedItem == cboBoxMessageType.Items[1])
                {
                    //Flüstern
                    this.SendNewMessage(chatText, MessageType.WhisperFromMe, txtBoxWhisperUsername.Text);
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
                string systemText = serverViewer.SwitchChannel(selectedChannel.Id, myUsername, testService);

                if (!String.IsNullOrEmpty(systemText))
                {
                    this.WriteNewMessageToChat(systemText, MessageType.System, "System");
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
            if (testService.NeedServerViewUpdate())
            {
                treeViewServer = serverViewer.Update(testService);
            }

            //Dienst - GetMessages();
            List<TestServer.Message> messages = testService.GetMessages(myUsername);

            if (messages != null)
            {
                foreach (TestServer.Message message in messages)
                {
                    if (message.MessageType == 0)
                    {
                        this.WriteNewMessageToChat(message.Text, MessageType.Say, message.FromUsername, message.ToUsername);
                    }
                    else if (message.MessageType == 1)
                    {
                        if (myUsername == message.FromUsername)
                        {
                            this.WriteNewMessageToChat(message.Text, MessageType.WhisperFromMe, message.FromUsername, message.ToUsername);
                        }
                        
                        if (myUsername == message.ToUsername)
                        {
                            this.WriteNewMessageToChat(message.Text, MessageType.WhisperToMe, message.FromUsername, message.ToUsername);
                        }
                    }
                    else
                    {
                        this.WriteNewMessageToChat(message.Text, MessageType.System, message.FromUsername, message.ToUsername);
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
                myUsername = (!String.IsNullOrEmpty(Properties.Settings.Default.Username)) ? Properties.Settings.Default.Username : "Benutzer_" + (random.Next(0, 999).ToString());

                //Verbindung zum Service aufbauen
                string systemText = testService.Connect(myUsername);

                if (!String.IsNullOrEmpty(systemText))
                {
                    this.WriteNewMessageToChat(systemText, MessageType.System, "System");

                    //Chat-Komponenten enablen
                    this.ChatEnable(true);

                    return true;
                }
                else
                {
                    this.WriteNewMessageToChat("Verbindung zum Serverdienst konnte nicht hergestellt werden.", MessageType.System, "System");

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
            //Verbindung zum Serverdienst trennen
            bool serviceDisconnected = testService.Disconnect(myUsername);

            if (serviceDisconnected)
            {
                //Verbindung trennen
                bckGrWorkerTCP.CancelAsync();

                //Chat-Komponenten disablen
                this.ChatEnable(false);

                treeViewServer.Nodes.Clear();

                this.WriteNewMessageToChat("Verbindung beendet.", MessageType.System, "System");

                isConnected = false;
            }
            
        }
        private void WriteNewMessageToChat(string newMessage, MessageType chatType, string fromUsername, string ToUsername = "")
        {
            DateTime date = DateTime.Now;
            string dateFormatted = date.ToString("dd.MM.yyyy HH:mm");

            string newChatLine = "";

            if (chatType == MessageType.Say)
            {
                newChatLine = "[" + dateFormatted + "] " + fromUsername + ": " + newMessage + "\r\n";

                chatExtFunctions.AddTextToChat(ref rtxtBoxChat, newChatLine);
            }
            else if (chatType == MessageType.WhisperToMe)
            {
                newChatLine = "[" + dateFormatted + "] " + fromUsername + " (flüstert dir): " + newMessage + "\r\n";

                chatExtFunctions.AddTextToChat(ref rtxtBoxChat, newChatLine, Color.Green);
            }
            else if (chatType == MessageType.WhisperFromMe)
            {
                newChatLine = "[" + dateFormatted + "] Du flüsterst " + ToUsername + ": " + newMessage + "\r\n";

                chatExtFunctions.AddTextToChat(ref rtxtBoxChat, newChatLine, Color.Green);
            }
            else if (chatType == MessageType.System)
            {
                newChatLine = "[" + dateFormatted + "] System:: " + newMessage + "\r\n";

                chatExtFunctions.AddTextToChat(ref rtxtBoxChat, newChatLine, Color.Red);
            }

            txtBoxChatEnter.Focus();
        }
        private void SendNewMessage(string myNewMessage, MessageType chatType, string ToUsername = "")
        {
            if (chatType == MessageType.Say)
            {
                //Dienst - Say();
                testService.Say(myNewMessage, myUsername);
            }
            else if (chatType == MessageType.WhisperFromMe)
            {
                if (!String.IsNullOrEmpty(ToUsername))
                {
                    //Dienst - Whisper();
                    testService.Whisper(myNewMessage, myUsername, ToUsername);
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


//http://msdn.microsoft.com/de-de/library/ms731144(v=vs.110).aspx -> automatische Erstellung einer Clientbindung (Dienst muss vorhanden sein)
//http://msdn.microsoft.com/de-de/library/aa751847(v=vs.110).aspx
//http://msdn.microsoft.com/de-de/library/aa347733(v=vs.110).aspx