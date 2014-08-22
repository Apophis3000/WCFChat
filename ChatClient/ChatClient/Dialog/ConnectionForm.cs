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
    public partial class ConnectionForm : Form
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
        public bool IsCanceled
        {
            get;
            private set;
        }

        public ConnectionForm()
        {
            InitializeComponent();

            txtBoxAddress.Text = Properties.Settings.Default.StandardAddress;
            txtBoxPort.Text = Properties.Settings.Default.StandardPort;
            txtBoxName.Text = Properties.Settings.Default.Username;

            IsCanceled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!chckBoxWcfService.Checked)
            {
                if (String.IsNullOrEmpty(txtBoxAddress.Text) || String.IsNullOrEmpty(txtBoxPort.Text))
                {
                    MessageBox.Show("Sie müssen eine Adresse und einen Port angeben.", "Fehler", MessageBoxButtons.OK);
                }
                else
                {
                    this.Address = txtBoxAddress.Text;
                    this.Port = txtBoxPort.Text;

                    IsCanceled = false;

                    this.Close();
                }
            }
            else
            {
                this.Address = txtBoxAddress.Text;
                this.Port = txtBoxPort.Text;

                Properties.Settings.Default.Username = txtBoxName.Text;
                Properties.Settings.Default.Save();

                IsCanceled = false;

                this.Close();
            }
        }
        private void chckBoxWcfService_CheckedChanged(object sender, EventArgs e)
        {
            if (chckBoxWcfService.Checked)
            {
                txtBoxAddress.Text = Properties.Settings.Default.StandardAddress;
                txtBoxPort.Text = Properties.Settings.Default.StandardPort;

                txtBoxAddress.Enabled = false;
                txtBoxPort.Enabled = false;
            }
            else
            {
                txtBoxAddress.Text = string.Empty;
                txtBoxPort.Text = string.Empty;

                txtBoxAddress.Enabled = true;
                txtBoxPort.Enabled = true;
            }
        }
        private void txtBoxPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}