using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient.Dialog
{
    public partial class SettingsForm : Form
    {
        public string Address
        {
            get;
            private set;
        }
        public string Port
        {
            get;
            private set;
        }

        public SettingsForm()
        {
            InitializeComponent();

            txtBoxName.Text = Properties.Settings.Default.Username;

            this.Address = Properties.Settings.Default.Address;
            this.Port = Properties.Settings.Default.Port;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Username = txtBoxName.Text;
            Properties.Settings.Default.Save();

            this.Close();
        }
    }
}