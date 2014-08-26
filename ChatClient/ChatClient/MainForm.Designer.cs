namespace ChatClient
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtBoxChatEnter = new System.Windows.Forms.TextBox();
            this.btnChatEnter = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.verbindungenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verbindenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verbindungTrennenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.einstellungenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.benutzernameÄndernToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainerClient = new System.Windows.Forms.SplitContainer();
            this.treeViewServer = new System.Windows.Forms.TreeView();
            this.lblWhisper = new System.Windows.Forms.Label();
            this.txtBoxWhisperUsername = new System.Windows.Forms.TextBox();
            this.cboBoxMessageType = new System.Windows.Forms.ComboBox();
            this.rtxtBoxChat = new System.Windows.Forms.RichTextBox();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.bckGrWorkerTCP = new System.ComponentModel.BackgroundWorker();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerClient)).BeginInit();
            this.splitContainerClient.Panel1.SuspendLayout();
            this.splitContainerClient.Panel2.SuspendLayout();
            this.splitContainerClient.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBoxChatEnter
            // 
            this.txtBoxChatEnter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxChatEnter.Location = new System.Drawing.Point(77, 486);
            this.txtBoxChatEnter.MaxLength = 100;
            this.txtBoxChatEnter.Name = "txtBoxChatEnter";
            this.txtBoxChatEnter.Size = new System.Drawing.Size(362, 21);
            this.txtBoxChatEnter.TabIndex = 2;
            this.txtBoxChatEnter.Text = "Enter text here...";
            this.txtBoxChatEnter.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtBoxChatEnter_MouseClick);
            this.txtBoxChatEnter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBoxChatEnter_KeyDown);
            this.txtBoxChatEnter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxChatEnter_KeyPress);
            // 
            // btnChatEnter
            // 
            this.btnChatEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChatEnter.Location = new System.Drawing.Point(445, 483);
            this.btnChatEnter.Name = "btnChatEnter";
            this.btnChatEnter.Size = new System.Drawing.Size(89, 27);
            this.btnChatEnter.TabIndex = 0;
            this.btnChatEnter.Text = "Enter";
            this.btnChatEnter.UseVisualStyleBackColor = true;
            this.btnChatEnter.Click += new System.EventHandler(this.btnChatEnter_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Gainsboro;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verbindungenToolStripMenuItem,
            this.einstellungenToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(784, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // verbindungenToolStripMenuItem
            // 
            this.verbindungenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verbindenToolStripMenuItem,
            this.verbindungTrennenToolStripMenuItem,
            this.beendenToolStripMenuItem});
            this.verbindungenToolStripMenuItem.Name = "verbindungenToolStripMenuItem";
            this.verbindungenToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.verbindungenToolStripMenuItem.Text = "Connection";
            // 
            // verbindenToolStripMenuItem
            // 
            this.verbindenToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.verbindenToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("verbindenToolStripMenuItem.Image")));
            this.verbindenToolStripMenuItem.Name = "verbindenToolStripMenuItem";
            this.verbindenToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.verbindenToolStripMenuItem.Text = "Connect";
            this.verbindenToolStripMenuItem.Click += new System.EventHandler(this.verbindenToolStripMenuItem_Click);
            // 
            // verbindungTrennenToolStripMenuItem
            // 
            this.verbindungTrennenToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.verbindungTrennenToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("verbindungTrennenToolStripMenuItem.Image")));
            this.verbindungTrennenToolStripMenuItem.Name = "verbindungTrennenToolStripMenuItem";
            this.verbindungTrennenToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.verbindungTrennenToolStripMenuItem.Text = "Disconnect";
            this.verbindungTrennenToolStripMenuItem.Click += new System.EventHandler(this.verbindungTrennenToolStripMenuItem_Click);
            // 
            // beendenToolStripMenuItem
            // 
            this.beendenToolStripMenuItem.BackColor = System.Drawing.Color.Gainsboro;
            this.beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
            this.beendenToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.beendenToolStripMenuItem.Text = "Close";
            this.beendenToolStripMenuItem.Click += new System.EventHandler(this.beendenToolStripMenuItem_Click);
            // 
            // einstellungenToolStripMenuItem
            // 
            this.einstellungenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.benutzernameÄndernToolStripMenuItem});
            this.einstellungenToolStripMenuItem.Name = "einstellungenToolStripMenuItem";
            this.einstellungenToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.einstellungenToolStripMenuItem.Text = "Settings";
            // 
            // benutzernameÄndernToolStripMenuItem
            // 
            this.benutzernameÄndernToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.benutzernameÄndernToolStripMenuItem.Name = "benutzernameÄndernToolStripMenuItem";
            this.benutzernameÄndernToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.benutzernameÄndernToolStripMenuItem.Text = "Change Username";
            this.benutzernameÄndernToolStripMenuItem.Click += new System.EventHandler(this.benutzernameÄndernToolStripMenuItem_Click);
            // 
            // splitContainerClient
            // 
            this.splitContainerClient.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerClient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerClient.Location = new System.Drawing.Point(12, 35);
            this.splitContainerClient.Name = "splitContainerClient";
            // 
            // splitContainerClient.Panel1
            // 
            this.splitContainerClient.Panel1.Controls.Add(this.treeViewServer);
            this.splitContainerClient.Panel1MinSize = 175;
            // 
            // splitContainerClient.Panel2
            // 
            this.splitContainerClient.Panel2.Controls.Add(this.lblWhisper);
            this.splitContainerClient.Panel2.Controls.Add(this.txtBoxWhisperUsername);
            this.splitContainerClient.Panel2.Controls.Add(this.cboBoxMessageType);
            this.splitContainerClient.Panel2.Controls.Add(this.rtxtBoxChat);
            this.splitContainerClient.Panel2.Controls.Add(this.btnChatEnter);
            this.splitContainerClient.Panel2.Controls.Add(this.txtBoxChatEnter);
            this.splitContainerClient.Panel2MinSize = 400;
            this.splitContainerClient.Size = new System.Drawing.Size(760, 515);
            this.splitContainerClient.SplitterDistance = 217;
            this.splitContainerClient.TabIndex = 2;
            // 
            // treeViewServer
            // 
            this.treeViewServer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewServer.Location = new System.Drawing.Point(3, 3);
            this.treeViewServer.Name = "treeViewServer";
            this.treeViewServer.Size = new System.Drawing.Size(209, 515);
            this.treeViewServer.TabIndex = 3;
            this.treeViewServer.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewServer_NodeMouseDoubleClick);
            // 
            // lblWhisper
            // 
            this.lblWhisper.AutoSize = true;
            this.lblWhisper.Location = new System.Drawing.Point(177, 489);
            this.lblWhisper.Name = "lblWhisper";
            this.lblWhisper.Size = new System.Drawing.Size(10, 15);
            this.lblWhisper.TabIndex = 5;
            this.lblWhisper.Text = ":";
            this.lblWhisper.Visible = false;
            // 
            // txtBoxWhisperUsername
            // 
            this.txtBoxWhisperUsername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtBoxWhisperUsername.Location = new System.Drawing.Point(77, 486);
            this.txtBoxWhisperUsername.Name = "txtBoxWhisperUsername";
            this.txtBoxWhisperUsername.Size = new System.Drawing.Size(100, 21);
            this.txtBoxWhisperUsername.TabIndex = 1;
            this.txtBoxWhisperUsername.Text = "<user>";
            this.txtBoxWhisperUsername.Visible = false;
            this.txtBoxWhisperUsername.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtBoxWhisperUsername_MouseClick);
            // 
            // cboBoxMessageType
            // 
            this.cboBoxMessageType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboBoxMessageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBoxMessageType.FormattingEnabled = true;
            this.cboBoxMessageType.Items.AddRange(new object[] {
            "Say",
            "Whisper"});
            this.cboBoxMessageType.Location = new System.Drawing.Point(3, 486);
            this.cboBoxMessageType.MaxDropDownItems = 2;
            this.cboBoxMessageType.Name = "cboBoxMessageType";
            this.cboBoxMessageType.Size = new System.Drawing.Size(68, 23);
            this.cboBoxMessageType.TabIndex = 3;
            this.cboBoxMessageType.SelectionChangeCommitted += new System.EventHandler(this.cboBoxMessageType_SelectionChangeCommitted);
            // 
            // rtxtBoxChat
            // 
            this.rtxtBoxChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtBoxChat.BackColor = System.Drawing.Color.White;
            this.rtxtBoxChat.Location = new System.Drawing.Point(3, 3);
            this.rtxtBoxChat.Name = "rtxtBoxChat";
            this.rtxtBoxChat.ReadOnly = true;
            this.rtxtBoxChat.Size = new System.Drawing.Size(531, 474);
            this.rtxtBoxChat.TabIndex = 4;
            this.rtxtBoxChat.Text = "";
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 200;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // bckGrWorkerTCP
            // 
            this.bckGrWorkerTCP.WorkerSupportsCancellation = true;
            this.bckGrWorkerTCP.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bckGrWorkerTCP_DoWork);
            this.bckGrWorkerTCP.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bckGrWorkerTCP_RunWorkerCompleted);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.splitContainerClient);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Direct Connect";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainerClient.Panel1.ResumeLayout(false);
            this.splitContainerClient.Panel2.ResumeLayout(false);
            this.splitContainerClient.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerClient)).EndInit();
            this.splitContainerClient.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnChatEnter;
        private System.Windows.Forms.TextBox txtBoxChatEnter;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem verbindungenToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainerClient;
        private System.Windows.Forms.RichTextBox rtxtBoxChat;
        private System.Windows.Forms.TreeView treeViewServer;
        private System.Windows.Forms.ToolStripMenuItem verbindenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verbindungTrennenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
        private System.Windows.Forms.ComboBox cboBoxMessageType;
        private System.Windows.Forms.Label lblWhisper;
        private System.Windows.Forms.TextBox txtBoxWhisperUsername;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.ComponentModel.BackgroundWorker bckGrWorkerTCP;
        private System.Windows.Forms.ToolStripMenuItem einstellungenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem benutzernameÄndernToolStripMenuItem;
    }
}

