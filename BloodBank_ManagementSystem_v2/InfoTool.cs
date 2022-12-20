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
    public partial class InfoTool : Form
    {
        public InfoTool()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFormDetails();
        }
        private void openFormDetails()
        {
            TreeNode sel = treeView1.SelectedNode;
            Details details = new Details();
            details.ShowDialog();
        }
    }
}
