using Microsoft.Win32;
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
    public partial class ChangeSuperAccount : Form
    {
        public ChangeSuperAccount()
        {
            InitializeComponent();
        }
        public static string name, psw;
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string user = username_txt.Text, pass = psw_txt.Text;
            if (username_txt.Text != name || psw_txt.Text != psw)
            {
                DialogResult dialogResult = MessageBox.Show("Some data have changed, save them?", "Change the data", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.Yes)
                {
                    var bytePass = Encoding.Unicode.GetBytes(pass);
                    RegistryKey registryKeyChange = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SalvucciF\BloodBank", true);
                    registryKeyChange.SetValue("BloodBankPsw", bytePass, RegistryValueKind.Binary);
                    registryKeyChange.SetValue("BloodBankUserID", user);

                    registryKeyChange.Close();
                    this.Close();
                }
                
            }
            else
            {
                MessageBox.Show("No data changed","No data",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            

        }

        private void show_hide(object sender, EventArgs e)
        {
            if (check_show_hide.Checked)
            {
                psw_txt.UseSystemPasswordChar = true;
                
            }
            else
            {
                psw_txt.UseSystemPasswordChar = false;
            }
        }

        private void loadData(object sender, EventArgs e)
        {
            string user = "", passw = "";
            RegistryKey regLoad = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SalvucciF\BloodBank");
            var data = (byte[])regLoad.GetValue("BloodBankPsw");

            user = (string)regLoad.GetValue("BloodBankUserID");
            passw = Encoding.Unicode.GetString(data);           
            regLoad.Close();
            name = user;
            psw = passw;
            username_txt.Text = user.ToString();
            psw_txt.Text = psw.ToString();
        }
            
    }
}
