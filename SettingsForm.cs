using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tracking
{
    public partial class SettingsForm : Form
    {
        private Form1 mainform;
        public SettingsForm(Form1 form1)
        {
            InitializeComponent();
            mainform = form1;
            text_server.Text = Properties.Settings.Default.DbServer;
            text_database.Text = Properties.Settings.Default.DbDatabase;
            text_user.Text = Properties.Settings.Default.DbUserID;
            text_pass.Text = Properties.Settings.Default.DbPass;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            // Save the values from the text boxes to settings
            Properties.Settings.Default.DbServer = text_server.Text;
            Properties.Settings.Default.DbDatabase = text_database.Text;
            Properties.Settings.Default.DbUserID = text_user.Text;
            Properties.Settings.Default.DbPass = text_pass.Text;

            // Save the settings
            Properties.Settings.Default.Save();

            // Display a message confirming save
            MessageBox.Show("Connection settings saved successfully.");
            text_database.Clear();
            text_pass.Clear();
            text_server.Clear();
            text_user.Clear();
            this.Close();
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainform.Show();
            mainform.BringToFront();
            mainform.refreshdata();

        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.FormClosed += new FormClosedEventHandler(SettingsForm_FormClosed);
        }
    }
}
