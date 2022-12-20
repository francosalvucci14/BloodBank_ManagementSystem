using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BloodBank_ManagementSystem_v2.Core
{
    public partial class ProgressBarLoading : Form
    {
        public ProgressBarLoading()
        {
            InitializeComponent();
        }

        private void ProgressBarLoading_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            progressBar1.Style = ProgressBarStyle.Marquee;
        }
    }
}
