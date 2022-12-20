using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using log4net;
using System.IO;

namespace BloodBank_ManagementSystem
{
    public partial class Login : Form
    {
        public Login()
        {
            
            InitializeComponent();
            notifier = this.notifyIcon1;
        }

        public static NotifyIcon Notifier { get { return notifier; } }

        private static NotifyIcon notifier;

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private void StartProgram()
        {
            Application.Run(new SplashScreen());
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string user = "", psw = "";
            
            RegistryKey reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SalvucciF\BloodBank");
            var data = (byte[])reg.GetValue("BloodBankPsw");
            if (reg != null)
            {
                user = (string)reg.GetValue("BloodBankUserID");
                //MessageBox.Show(user); 
                //psw = (string)reg.GetValue("BloodBankPsw");
                psw = Encoding.Unicode.GetString(data);
                // MessageBox.Show(psw);
                reg.Close();
            }
            bool resultUser = userID_txt.Text.Equals(user);
            bool resultPsw = psw_txt.Text.Equals(psw);
            if (checkBox1.Checked)
            {
                RegistryKey regCreate = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SalvucciF\BloodBank",true);
                //regCreate.CreateSubKey("SaveData");
                regCreate.SetValue("SaveData", 1, RegistryValueKind.DWord);
                regCreate.Close();
                Log.Info("SaveData checked! --> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            else
            {
                RegistryKey regCreate2 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SalvucciF\BloodBank", true);
                //regCreate.CreateSubKey("SaveData");
                regCreate2.SetValue("SaveData", 0, RegistryValueKind.DWord);
                regCreate2.Close();
                Log.Info("SaveData unchecked! --> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            if (resultUser && resultPsw)
            {
                //try
                //{
                //    Thread t = new Thread(new ThreadStart(StartProgram));
                //    t.Start();
                //    Thread.Sleep(5000);
                //    t.Abort();
                //}
                //catch (Exception e)
                //{
                //    //
                //}
                string happy = "\u263A";
                notifier.ShowBalloonTip(500, "Welcome", "Welcome back : " + userID_txt.Text + "\nHave a good day " + happy + " !", ToolTipIcon.Info);
                Log.Info("Authentication successful ! --> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                //notifyIcon1.Dispose();
                Form1 f = new Form1();

                this.Visible = false;
                f.ShowDialog();
                //Application.Exit();
                this.Close();

            }
            else
            {
                string noentry = "\u26D4";
                notifier.ShowBalloonTip(1000, noentry + "WAIT" + noentry, "Error : \nUsername or Password are invalid " + noentry + "!!!", ToolTipIcon.Error);
                //MessageBox.Show("User ID or Password are invalid", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Log.Error("User ID or Password are invalid! --> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            string user = "", psw = "";
            int saved = 0;

            Log.Info("Upload the data  --> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            RegistryKey regLoad = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SalvucciF\BloodBank");
            var data = (byte[])regLoad.GetValue("BloodBankPsw");
            int count = regLoad.ValueCount;
            if (regLoad != null)
            {
                user = (string)regLoad.GetValue("BloodBankUserID");           
                psw = Encoding.Unicode.GetString(data);
                saved = (int)regLoad.GetValue("SaveData");
                regLoad.Close();
                Log.Info($"Found {count} data  --> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            if (saved == 1)
            {
                checkBox1.Checked = true;
                userID_txt.Text = user.ToString();
                psw_txt.Text = psw.ToString();
            }
        }
    }
    //public class Threader
    //{
    //    private bool condition = false;
    //    public void doWork()
    //    {
    //        while (!condition)
    //        {
    //            Application.Run(new SplashScreen());
    //        }
    //    }
    //    public void kill()
    //    {
    //        condition = true;
    //    }
    //}
}
