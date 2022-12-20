using BloodBank_ManagementSystem_v2.Core;
using log4net;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Runtime.InteropServices;
namespace BloodBank_ManagementSystem_v2
{
    public partial class ShowUserData : Form
    {
        BackgroundWorker worker = new BackgroundWorker();
        public ShowUserData()
        {
            InitializeComponent();
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();
            worker.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            

        }

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string UserValue = "", SurValue = "", RH = "", BloodG = "", Addr = "", Tel = "", Email = "";

        
        private void openDetails(object sender, DataGridViewCellEventArgs e)
        {
            //dataGridView1.SelectedCells[0].Value.ToString() per prendere il valore della cella selezionata
            string myConnectionString = DBManager.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
            MySqlConnection msqData = new MySqlConnection(myConnectionString);
            //string Username, Pass, Email;
            string username = dataGridView1.SelectedCells[1].Value.ToString();
            try
            {


                string queryS = "SELECT * FROM donators WHERE Name = '" + username + "';";
                MySqlCommand msc = new MySqlCommand(queryS, msqData);


                Console.WriteLine(queryS);
                msqData.Open();
                MySqlDataReader reader = msc.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserValue = reader["Name"].ToString();
                        SurValue = reader["Surname"].ToString();
                        RH = reader["RH"].ToString();
                        BloodG = reader["BloodG"].ToString();
                        Addr = reader["Address"].ToString();
                        Tel = reader["Telephone"].ToString();
                        Email = reader["Email"].ToString();
                        img = (byte[])reader["userPicture"];
                    }
                    UserDetails u = new UserDetails();
                    if (img.Length <= 0)
                    {

                        u.pictureBox1.Image = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + @"\Image\404-removebg-preview.png");
                        msqData.Close();
                        u.ShowDialog();
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(img);
                        u.pictureBox1.Image = Image.FromStream(ms);
                        //msqData.Close();
                        //this.Close();
                        u.ShowDialog();
                    }




                    //logger.WriteOnLog(LogId, "Show User Control", 3);
                    ////loggerS.WriteOnLogSetup(LogIdS, "Show User Control", 3);
                    Log.Info("Open form UserDetails " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    //this.Hide();
                }
                else
                {
                    MessageBox.Show("No User Found");
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("See log file for more information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //logger.WriteErrorOnLog(LogId, 1, ex, "Error");
                Log.Error("Error = " + ex.ToString() + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                ////loggerS.WriteErrorOnLogSetup(LogIdS, 1, ex, "Error");
            }
            finally
            {
                if (msqData.State == ConnectionState.Open)
                {
                    msqData.Close();
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            List<string> selectedItem = new List<string>();
            DataGridViewRow drow = new DataGridViewRow();
            for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
            {
                drow = dataGridView1.Rows[i];
                if (drow.Cells[0].Selected) //checking if  checked or not.  
                {
                    string id = drow.Cells[0].Value.ToString();
                    selectedItem.Add(id); //If checked adding it to the list  
                }
            }
            Log.Info("selected items = " + selectedItem + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            string myConnectionString = DBManager.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
            MySqlConnection msqData = new MySqlConnection(myConnectionString);
            //string Username, Pass, Email;
            //string username = dataGridView1.SelectedCells[0].Value.ToString();
            try
            {
                
             
                msqData.Open();
                foreach (string s in selectedItem) //using foreach loop to delete the records stored in list.  
                {
                    Log.Info("selected items = " + s + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    string queryS = "delete FROM donators WHERE UID = '" + s + "';";
                    MySqlCommand cmd = new MySqlCommand(queryS, msqData);
                    cmd.ExecuteNonQuery();
                }
                msqData.Close();
                //dataGridView1.Rows.Clear();
                MySqlConnection msq = new MySqlConnection(myConnectionString);
                ExecuteShowDataQuery(msq);
            }
            catch (Exception ex)
            {
                MessageBox.Show("See log file for more information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //logger.WriteErrorOnLog(LogId, 1, ex, "Error");
                ////loggerS.WriteErrorOnLogSetup(LogIdS, 1, ex, "Error");
                Log.Error("Error = " + ex.Message + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            finally
            {
                if (msqData.State == ConnectionState.Open)
                {
                    msqData.Close();
                }
            }
            
        }

        private void openMenuStrip(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }
            
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            string myConnectionString = DBManager.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
            MySqlConnection msqData = new MySqlConnection(myConnectionString);
            try
            {
                ExecuteShowDataQuery(msqData);

            }
            catch (Exception ex)
            {
                MessageBox.Show("See log file for more information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //logger.WriteErrorOnLog(LogId, 1, ex, "Error");
                ////loggerS.WriteErrorOnLogSetup(LogIdS, 1, ex, "Error");
                Log.Error("Error = " + ex.Message + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            finally
            {
                if (msqData.State == ConnectionState.Open)
                {
                    msqData.Close();
                }
            }
        }

        public byte[] img;
        private void showData(object sender, EventArgs e)
        {

      

            circularProgressBar1.Location = new Point(
                this.ClientSize.Width / 2 - circularProgressBar1.Size.Width / 2,
                this.ClientSize.Height / 2 - circularProgressBar1.Size.Height / 2);
            circularProgressBar1.Anchor = AnchorStyles.None;
            circularProgressBar1.Maximum = 100;
            circularProgressBar1.Step = 10;
            circularProgressBar1.Value = 0;
            circularProgressBar1.Style = ProgressBarStyle.Marquee;
            //worker async qui
            circularProgressBar1.Visible = true;
            ProgressLabel.Visible = true;
            
        }


        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //var backgroundWorker = sender as BackgroundWorker;
            string myConnectionString = DBManager.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
            MySqlConnection msqData = new MySqlConnection(myConnectionString);
            int size = 0;
            try
            {
                //for (int j = 0; j < 100000; j++)
                //{


                //backgroundWorker.ReportProgress(circularProgressBar1.Value);
                //}
                
                ExecuteShowDataQuery(msqData);
                


            }
            catch (Exception ex)
            {
                MessageBox.Show("See log file for more information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //logger.WriteErrorOnLog(LogId, 1, ex, "Error");
                ////loggerS.WriteErrorOnLogSetup(LogIdS, 1, ex, "Error");
                Log.Error("Error = " + ex.Message + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            finally
            {
                if (msqData.State == ConnectionState.Open)
                {
                    msqData.Close();
                }
            }
            
        }
        private void ExecuteShowDataQuery(MySqlConnection msqData)
        {
            //MessageBox.Show("CIAO", "",MessageBoxButtons.OK,MessageBoxIcon.Information) ;
            string queryS = "SELECT UID,Name,Surname,RH,BloodG,Address,Telephone,Email FROM donators;";
            MySqlCommand msc = new MySqlCommand(queryS, msqData);

            //Console.WriteLine(queryS);
            msqData.Open();

            msc.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter ad = new MySqlDataAdapter(msc);
            ad.Fill(dt);
            //dataGridView1.RowsAdded += (obj, arg) => AutoHeightGrid(dataGridView1);
            dataGridView1.DataSource = dt;
            //size = dgvHeight();

            //dataGridView1.Height = size;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            Log.Info("Load all donators " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        }
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            circularProgressBar1.Visible = false;
            ProgressLabel.Visible = false;
            //panel1.Visible = false;
            //MessageBox.Show("FINE", "", MessageBoxButtons.OK, MessageBoxIcon.Information); 
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.ProgressPercentage == -1)
                circularProgressBar1.Maximum = Convert.ToInt32(e.UserState);
            else
            {
                circularProgressBar1.Value = e.ProgressPercentage;
                Thread.Sleep(50);
                worker.ReportProgress(e.ProgressPercentage);
                ProgressLabel.Text = "Progress: " + e.ProgressPercentage.ToString() + "%";
            }
                
            //circularProgressBar1.Value = e.ProgressPercentage;
            //circularProgressBar1.Text += circularProgressBar1.Value.ToString();

            //Log.Info("ProgressValue : " + e.ProgressPercentage.ToString());
            //MessageBox.Show("ProgressValue : " + circularProgressBar1.Value);
            //label1.Text += progressBar1.Value;
        }
        
    }
}
