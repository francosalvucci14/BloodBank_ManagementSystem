using BloodBank_ManagementSystem_v2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using System.IO;

namespace BloodBank_ManagementSystem
{
    public partial class Form1 : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Font fontUse = new Font("Segoe UI", 12, FontStyle.Regular, GraphicsUnit.Pixel);

        public Form1()
        {
            
            InitializeComponent();
            
            customize();

            Login.Notifier.Text = "BloodBank_ManagementSystem v2";
            Login.Notifier.Icon = new System.Drawing.Icon(Path.GetDirectoryName(Application.ExecutablePath) + @"\Image\blood-bank.ico");
            Login.Notifier.Click += NotifyIcon1_Click;
            
            Login.Notifier.ContextMenuStrip = new ContextMenuStrip();
            Login.Notifier.ContextMenuStrip.Font = fontUse;
            Login.Notifier.ContextMenuStrip.Items.Add("Status", Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + @"\Image\blood-bank.ico"), OnStatusClicked);
            Login.Notifier.ContextMenuStrip.Items.Add("Exit", Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + @"\Image\blood-bank.ico"), OnExitClicked);
            Login.Notifier.ContextMenuStrip.Items.Add("Info", Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + @"\Image\blood-bank.ico"), OnInfoClicked);
        }

        private void OnInfoClicked(object sender, EventArgs e)
        {
            InfoTool infoTool = new InfoTool();
            infoTool.Show();
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void customize()
        {
            
            panelSubMenu.Visible = false;
            panelDisplayData.Visible = false;
            panelTools.Visible = false;

        }

        private void OnStatusClicked(object sender, EventArgs e)
        {
            AppStatusForm appStatusForm = new AppStatusForm();
            appStatusForm.ShowDialog();
            //MessageBox.Show("Application is running", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void NotifyIcon1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void hideSubMenu()
        {
            if (panelSubMenu.Visible == true)
               panelSubMenu.Visible = false;
            if (panelDisplayData.Visible == true)
                panelDisplayData.Visible = false;
            if (panelTools.Visible == true)
                panelTools.Visible = false;
        }
        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }
        
        
        
        //to show subForm in panel
        private Form activeForm = null;
        private void openChildForm(Form Child)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = Child;
            Child.TopLevel = false;
            Child.FormBorderStyle = FormBorderStyle.None;
            Child.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(Child);
            panelChildForm.Tag = Child;
            Child.BringToFront();
            Child.Show();
            
        }
       
        private void closeChildForm(Form Child)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = Child;
            Child.Close();
        }
        
        /*
        private void button5_Click(object sender, EventArgs e)
        {
            var changeL = new ChangeLanguage();
            changeL.update("language", "en-EN");
            Application.Restart();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var changeL = new ChangeLanguage();
            changeL.update("language", "it-IT");
            Application.Restart();
        }
        */
        private void button5_Click_1(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            closeChildForm(this.activeForm);
        }
        #region tools

        
        private void button4_Click_1(object sender, EventArgs e)
        {
            openChildForm(new ChangeLangForm());
            hideSubMenu();
        }

        private void btn_ToolsIcon_Click(object sender, EventArgs e)
        {
            showSubMenu(panelTools);
            //Log.Info("Open Child Form {InfoTool}! --> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            //hideSubMenu();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            openChildForm(new ChangeSuperAccount());
            Log.Info("Open Child Form {ChangeSuperAccount}! --> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            hideSubMenu();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            openChildForm(new InfoTool());
            Log.Info("Open Child Form {InfoTool}! --> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            hideSubMenu();
        }
        #endregion tools
        #region user
        

        private void button9_Click(object sender, EventArgs e)
        {
            openChildForm(new AddUser());
            Log.Info("Open Child Form {AddUser}! --> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            hideSubMenu();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            openChildForm(new ShowUserData());
            hideSubMenu();
        }
        #endregion user

        private void btn_UserIcon_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenu);
        }
        #region display
        private void button8_Click_1(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void btn_DisplayIcon_Click(object sender, EventArgs e)
        {
            showSubMenu(panelDisplayData);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            hideSubMenu();
        }
        #endregion display

        private void followInsta(object sender, EventArgs e)
        {
            Log.Info("Open Web Page Instagam --> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            System.Diagnostics.Process.Start("https://www.instagram.com/franco.salvucci.9/");
        }
        #region exit
        private void bnt_ExitIcon_Click(object sender, EventArgs e)
        {
            notifyIcon1.Dispose();

            DialogResult dialogResult = MessageBox.Show("Do you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
        }

        #endregion exit

        private void followFacebook(object sender, EventArgs e)
        {
            Log.Info("Open Web Page Instagam --> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            System.Diagnostics.Process.Start("https://www.facebook.com/franco.salvucci.9");
        }

       

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Log.Info("Open Web Page --> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            System.Diagnostics.Process.Start("https://salvuccif.altervista.org/");
        }

        private void followTwitter_Click(object sender, EventArgs e)
        {
            Log.Info("Open Web Page Twitter --> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            System.Diagnostics.Process.Start("https://twitter.com/Salvucci2Franco");
        }
    }
}
