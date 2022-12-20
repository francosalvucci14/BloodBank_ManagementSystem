using BloodBank_ManagementSystem;
using FontAwesome.Sharp;
using log4net;
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
    public partial class ChangeLangForm : Form
    {
        public ChangeLangForm()
        {
            InitializeComponent();
        }
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private void button1_Click(object sender, EventArgs e)
        {
            if (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked)
            {
                MessageBox.Show("You should select a language", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (radioButton1.Checked)
            {

                try
                {
                    DialogResult dialogResult = MessageBox.Show("Some data have changed, save them?", "Change the data", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Log.Info("Culture change --> Now [EN]" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                        changeToEng();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("See log file for more information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Log.Error("Error -> " + ex.Message + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                }
                
                
                
            }
            if (radioButton2.Checked)
            {
                try
                {
                    DialogResult dialogResult = MessageBox.Show("Some data have changed, save them?", "Change the data", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Log.Info("Culture change --> Now [IT]" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                        changeToIt();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Guarda il file di log per maggiori informazioni", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Log.Error("Error -> " + ex.Message + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                }
                
            }
            if (radioButton3.Checked)
            {
                try
                {
                    DialogResult dialogResult = MessageBox.Show("Some data have changed, save them?", "Change the data", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Log.Info("Culture change --> Now [FR]" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                        changeToFr();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Consultez le fichier journal pour plus d'informations", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Log.Error("Error -> " + ex.Message + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                }

            }
        }
        private void changeToEng()
        {
            var changeL = new ChangeLanguage();
            changeL.update("language", "en-EN");
            RegistryKey regCulture = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SalvucciF\BloodBank", true);
            //regCreate.CreateSubKey("SaveData");
            regCulture.SetValue("CurrentCulture", "en-EN");
            regCulture.Close();
            Application.Restart();
        }

        private void changeToIt()
        {
            var changeL = new ChangeLanguage();
            changeL.update("language", "it-IT");
            RegistryKey regCulture = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SalvucciF\BloodBank", true);
            //regCreate.CreateSubKey("SaveData");
            regCulture.SetValue("CurrentCulture", "it-IT");
            regCulture.Close();
            Application.Restart();
        }
        private void changeToFr()
        {
            var changeL = new ChangeLanguage();
            changeL.update("language", "fr-FR");
            RegistryKey regCulture = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SalvucciF\BloodBank", true);
            //regCreate.CreateSubKey("SaveData");
            regCulture.SetValue("CurrentCulture", "fr-FR");
            regCulture.Close();
            Application.Restart();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        int c = 0;
        private void iconButton1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            if (c == 1)
            {
                c = 0;
                iconButton1.IconChar = FontAwesome.Sharp.IconChar.ToggleOn;
                //this.BackColor = Color.White;
                foreach (Control control in this.Controls)
                {
                    UpdateControlsColor(control);
                }
                
                f.BackColor = Color.White;
            }
            if (c == 0)
            {
                c = 1;
                iconButton1.IconChar = FontAwesome.Sharp.IconChar.ToggleOff;
                foreach (Control control in this.Controls)
                {
                    DeUpdateControlsColor(control);
                }
                //Form1 f = new Form1();
                f.BackColor = Color.FromArgb(35, 32, 39);
                
            }
        }

        private void UpdateControlsColor(Control c)
        {
           
            if (c is Button)
            {
                c.BackColor = Color.Blue;
                c.ForeColor = Color.White;
            }
            if (c is RadioButton)
            {
                c.BackColor = Color.Violet;
                c.ForeColor = Color.White;
            }

            // Any other non-standard controls should be implemented here aswell...

            foreach (Control subC in c.Controls)
            {
                UpdateControlsColor(subC);
            }
        }
        private void DeUpdateControlsColor(Control c)
        {
           

            if (c is Button)
            {
                c.BackColor = Color.Black;
                c.ForeColor = Color.Gainsboro;
            }
            if (c is IconButton)
            {
                c.BackColor = Color.FromArgb(35, 32, 39);
                c.ForeColor = Color.Gainsboro;
            }
            if (c is RadioButton)
            {
                c.BackColor = Color.FromArgb(35, 32, 39);
                c.ForeColor = Color.Gainsboro;
            }

            // Any other non-standard controls should be implemented here aswell...

            foreach (Control subC in c.Controls)
            {
                DeUpdateControlsColor(subC);
            }
        }
    }
}
