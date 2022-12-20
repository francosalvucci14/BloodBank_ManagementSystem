using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BloodBank_ManagementSystem
{
    public partial class SplashScreen : Form
    {
        private int iProgressBarValue;
        public SplashScreen()
        {
            InitializeComponent();
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //X PROGRESS BAR
            //iProgressBarValue++;
            //switch (iProgressBarValue)
            //{
            //    case 1:case 3:case 5:case 7:case 9:
            //        progressBar1.Value = (iProgressBarValue * 100);
            //        break;
            //    case 2: case 4: case 6: case 8: case 10:
            //        progressBar1.Value = (iProgressBarValue * 100);
            //        break;
            //    case 13:
            //        timer1.Stop();
            //        timer1.Enabled = false;
            //        Login login = new Login();
            //        login.Show();
            //        this.Hide();
            //        break;
            //    default:
            //        break;
            //}

            // NEW SPLASH SCREEN TEST
            panelSplash.Width += 3;
            if (panelSplash.Width >= 500)
            {
                timer1.Stop();
                timer1.Enabled = false;
                Login login = new Login();
                login.Show();
                this.Hide();
            }
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            //progressBar1.Minimum = 0;
            //progressBar1.Maximum = 1000;
            //progressBar1.Value = 0;

            timer1.Enabled = true;
            timer1.Interval = 15;
            timer1.Start();
        }
    }
}
