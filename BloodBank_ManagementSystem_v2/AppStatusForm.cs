using BloodBank_ManagementSystem_v2.Core;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BloodBank_ManagementSystem_v2
{
    public partial class AppStatusForm : Form
    {
        public AppStatusForm()
        {
            InitializeComponent();
        }
        public static string exError;
        public bool CheckConnession(string conn)
        {
            bool result = false;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            try
            {
                mySqlConnection.Open();
                result = true;
                mySqlConnection.Close();
                
            }
            catch (Exception e)
            {
                result = false;
                exError = e.Message + "\n" + e.StackTrace +"\n" + e.ToString();
            }
            return result;
        }

        private void AppStatusForm_Load(object sender, EventArgs e)
        {
            bool result_check = CheckConnession(DBManager.connString);
            string okay = "\u2713";
            string noConn = "\u274C";
            labelAppStatus.Text = "Application is running " + okay;
            labelAppStatus.ForeColor = Color.Green;
            if (result_check == false)
            {
                labelDBStatus.Text = "DB Connection Error" + noConn;
                labelDBStatus.ForeColor = Color.Red;
                richTextBox1.Text += exError.ToUpper();
            }
            else
            {
                labelDBStatus.Text = "DB Connection is okay" + okay;
                labelDBStatus.ForeColor = Color.Green;
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "")
            {
                Clipboard.SetText(richTextBox1.Text);
                string okay = "\u2713";
                lbl_copy.Text = "Copied" + okay;
                lbl_copy.Visible = true;
            }
            
        }
    }
}
