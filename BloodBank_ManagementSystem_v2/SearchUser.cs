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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BloodBank_ManagementSystem_v2
{
    public partial class SearchUser : Form
    {
        public SearchUser()
        {
            InitializeComponent();
        }
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string UserValue = "", SurValue = "", RH = "", BloodG = "", Addr = "", Tel = "", Email = "";
        public byte[] img;
        private void button1_Click(object sender, EventArgs e)
        {
            string myConnectionString = DBManager.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
            MySqlConnection msqData = new MySqlConnection(myConnectionString);
            //string Username, Pass, Email;
            if (textBox1.Text == "")
            {
                MessageBox.Show("Choose an user");
            }
            try
            {


                string queryS = "SELECT * FROM donators WHERE Name = '" + textBox1.Text + "';";
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
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("No User Found");
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("See log file for more information","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
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

        private void button2_Click(object sender, EventArgs e)
        {
            //showData();
            ShowUserData showUserData = new ShowUserData();
            showUserData.ShowDialog();
        }
        //private void showData()
        //{
        //    string myConnectionString = DBManager.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
        //    MySqlConnection msqData = new MySqlConnection(myConnectionString);

        //    try
        //    {


        //        string queryS = "SELECT Name,Surname,RH,BloodG,Address,Telephone,Email FROM donators;";
        //        MySqlCommand msc = new MySqlCommand(queryS, msqData);

        //        //Console.WriteLine(queryS);
        //        msqData.Open();
        //        msc.ExecuteNonQuery();
        //        DataTable dt = new DataTable();
        //        MySqlDataAdapter ad = new MySqlDataAdapter(msc);
        //        ad.Fill(dt);
        //        dataGridView1.DataSource = dt;//to show data from datatable, in form add datagridview
        //                                      //logger.WriteOnLog(LogId, "Load All User", 3);
        //                                      // //loggerS.WriteOnLogSetup(LogIdS, "Load All User", 3);
        //        dataGridView1.RowHeadersVisible = false;
        //        //dataGridView1.AutoResizeColumns();
        //        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //        dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
        //        Log.Info("Load all donators " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("See log file for more information","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
        //        //logger.WriteErrorOnLog(LogId, 1, ex, "Error");
        //        ////loggerS.WriteErrorOnLogSetup(LogIdS, 1, ex, "Error");
        //        Log.Error("Error = " + ex.Message + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        //    }
        //    finally
        //    {
        //        if (msqData.State == ConnectionState.Open)
        //        {
        //            msqData.Close();
        //        }
        //    }
        //}
    }
}
