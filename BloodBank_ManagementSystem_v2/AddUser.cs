using BloodBank_ManagementSystem_v2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using BloodBank_ManagementSystem_v2.Core;
using log4net;
using System.IO;

namespace BloodBank_ManagementSystem
{
    public partial class AddUser : Form
    {
        public AddUser()
        {
            InitializeComponent();
            labelEmail.Visible = true;
            email_txt.Visible = true;
            
        }
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string comboValue = "";
        public int saveEmail = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                //labelEmail.Visible = true;
                //email_txt.Visible = true;
                saveEmail = 1;
            }
            else
            {
                //labelEmail.Visible = false;
                //email_txt.Visible = false;
                saveEmail = 0;
            }
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = new ComboItem[]
            {
                new ComboItem{ID = 1,text = "POSITIVE"},
                new ComboItem{ID = 2,text = "NEGATIVE"}
            };
        }
        public bool isChangedProfile = false;
        private void imageChanged(object sender, EventArgs e)
        {
            isChangedProfile = true;

        }
        private void button2_Click(object sender, EventArgs e)
        {        
            comboValue = comboBox1.SelectedValue.ToString();
            //byte[] img = ConvertImagesToByte(pictureBox1.Image);
            try
            {
                
                byte[] img;
                if (isChangedProfile)
                {
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    img = ms.ToArray();
                }
                else
                {
                    img = new byte[0];
                }
                
                DBManager.openConnection();                   
                StringBuilder sb = new StringBuilder();
                String insert =" INSERT INTO donators (UID,Name,Surname,RH,BloodG,Address,Telephone,Email,SenderMail,userPicture) VALUES (null,@name,@surname,@rh,@bloodg,@add,@tel,@email,@savemail,@img)";


                MySqlCommand cmd = new MySqlCommand(insert,DBManager.conn);
                cmd.Parameters.Add("@name",MySqlDbType.VarChar,80);
                cmd.Parameters.Add("@surname", MySqlDbType.VarChar,100);
                cmd.Parameters.Add("@rh", MySqlDbType.VarChar,100);
                cmd.Parameters.Add("@bloodg", MySqlDbType.VarChar,100);
                cmd.Parameters.Add("@add", MySqlDbType.VarChar,100);
                cmd.Parameters.Add("@tel", MySqlDbType.VarChar,100);
                cmd.Parameters.Add("@email", MySqlDbType.VarChar,100);
                cmd.Parameters.Add("@savemail", MySqlDbType.Int32);
                cmd.Parameters.Add("@img", MySqlDbType.LongBlob);

                cmd.Parameters["@name"].Value = user_txt_name.Text;
                cmd.Parameters["@surname"].Value = user_txt_surn.Text;
                cmd.Parameters["@rh"].Value = comboValue;
                cmd.Parameters["@bloodg"].Value = user_txt_bg.Text;
                cmd.Parameters["@add"].Value = user_txt_add.Text;
                cmd.Parameters["@tel"].Value = user_txt_tel.Text;
                cmd.Parameters["@email"].Value = email_txt.Text;
                cmd.Parameters["@savemail"].Value = saveEmail;
                cmd.Parameters["@img"].Value = img;


                cmd.ExecuteNonQuery();
                Log.Info($"Insert donator -> {user_txt_name.Text} " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                user_txt_add.Text = "";
                user_txt_bg.Text = "";
                user_txt_name.Text = "";
                user_txt_surn.Text = "";
                user_txt_tel.Text = "";
                email_txt.Text = "";
                pictureBox1.Image = null;
                MessageBox.Show("Donators insert correctly","OK",MessageBoxButtons.OK,MessageBoxIcon.Information);
                DBManager.closeConnection();
                isChangedProfile = false;
            }
            catch (Exception ex)
            {
                DBManager.closeConnection();
                MessageBox.Show("See log file for more information","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Log.Error("Error -> " + ex.Message + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog() { Filter= "All files (*.*)|*.*|Png Image(*.png;*.PNG)|*.png;*.PNG|JPG Image(*.jpg,*.jpeg)|*.jpg;*.jpeg", Multiselect = false })
            {
                if (open.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(open.FileName);
                    isChangedProfile = true;
                    //labelImage.Text = Path.GetFileName(open.FileName);
                }
            }
        }

        byte[] ConvertImagesToByte(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms,System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public Image ConvertByteToImage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return Image.FromStream(ms);
            }
        }
        
    }
}
