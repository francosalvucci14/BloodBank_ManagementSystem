using BloodBank_ManagementSystem_v2.Core;
using iTextSharp.text;
using iTextSharp.text.pdf;
using log4net;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BloodBank_ManagementSystem_v2
{
    public partial class UserDetails : Form
    {
        public UserDetails()
        {
            InitializeComponent();
        }
        public static string name, surname, rh, bloodg, addr, tel, email,UID;
        public static int countControls = 0,countLabel = 0;

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog() { Filter = "All files (*.*)|*.*|Png Image(*.png;*.PNG)|*.png;*.PNG|JPG Image(*.jpg,*.jpeg)|*.jpg;*.jpeg", Multiselect = false })
            {
                if (open.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = System.Drawing.Image.FromFile(open.FileName);
                    isChanged = true;
                    //labelImage.Text = Path.GetFileName(open.FileName);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to delete this user?", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {


                    DBManager.openConnection();
                    string queryD = "DELETE FROM donators WHERE Name ='"+name_txt.Text+"'";
                    MySqlCommand mscD = new MySqlCommand(queryD, DBManager.conn);
                    mscD.ExecuteNonQuery();

                    Log.Info($"Delete donator -> [{name_txt.Text}] " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    DBManager.closeConnection();
                    MessageBox.Show("User delete successfully", "Delete user", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    DBManager.closeConnection();
                    MessageBox.Show("See log file for more information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Log.Error("Error -> " + ex.Message + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                }


            }
            else
            {
                //
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Minimized;
            else
                WindowState = FormWindowState.Normal;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showColor(object sender, EventArgs e)
        {
            button6.BackColor = Color.Red;
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            //report of IdCard in .pdf file
            DialogResult dialogResult = MessageBox.Show("Do you want to print this IdCard?", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.Yes)
            {
               SaveFileDialog svg = new SaveFileDialog();
               svg.FileName = "IDCard_" + UID;
               svg.Filter = "PDF (*.pdf)|*.pdf";
               bool fileError = false;
               if(svg.ShowDialog() == DialogResult.OK)
               {

                    
                    if (File.Exists(svg.FileName))
                    {
                        try
                        {
                            File.Delete(svg.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            PdfPTable pdfTable1 = new PdfPTable(countLabel);
                            pdfTable1.DefaultCell.Padding = 3;
                            pdfTable1.WidthPercentage = 100;
                            pdfTable1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //con questo scrivo la prima riga, il noe delle colonne
                                PdfPCell cell = new PdfPCell(new Phrase(label1.Text));
                                pdfTable1.AddCell(cell);
                                PdfPCell cell2 = new PdfPCell(new Phrase(label2.Text));
                                pdfTable1.AddCell(cell2);
                                PdfPCell cell3 = new PdfPCell(new Phrase(label3.Text));
                                pdfTable1.AddCell(cell3);
                                PdfPCell cell4 = new PdfPCell(new Phrase(label4.Text));
                                pdfTable1.AddCell(cell4);
                                PdfPCell cell5 = new PdfPCell(new Phrase(label5.Text));
                                pdfTable1.AddCell(cell5);
                                PdfPCell cell6 = new PdfPCell(new Phrase(label6.Text));
                                pdfTable1.AddCell(cell6);
                                PdfPCell cell7 = new PdfPCell(new Phrase(label7.Text));
                                pdfTable1.AddCell(cell7);

                            
                                    //questo if per evitare problema con riferimento non impostato su instanmza di oggetto
                                    //con questo scrivo il contenuto delle celle
                                    if (name_txt.Text != null)
                                    {
                                        pdfTable1.AddCell(name_txt.Text.ToString());
                                    }
                                    if (sur_txt.Text != null)
                                    {
                                        pdfTable1.AddCell(sur_txt.Text.ToString());
                                    }
                                    if (rh_txt.Text != null)
                                    {
                                        pdfTable1.AddCell(rh_txt.Text.ToString());
                                    }
                                    if (bloodg_txt.Text != null)
                                    {
                                        pdfTable1.AddCell(bloodg_txt.Text.ToString());
                                    }
                                    if (addr_txt.Text != null)
                                    {
                                        pdfTable1.AddCell(addr_txt.Text.ToString());
                                    }
                                    if (tel_txt.Text != null)
                                    {
                                        pdfTable1.AddCell(tel_txt.Text.ToString());
                                    }
                                    if (email_txt.Text != null)
                                    {
                                        pdfTable1.AddCell(email_txt.Text.ToString());
                                    }
                            
                            using (FileStream stream = new FileStream(svg.FileName, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                pdfDoc.AddAuthor("Salvucci Franco");
                                pdfDoc.AddProducer();
                                pdfDoc.AddCreator("BloodBank");
                                pdfDoc.AddTitle("IdCard of the User: ["+ name_txt.Text.ToUpper() + "]");
                                Paragraph p = new Paragraph(@"IdCard of the User : ["+ name_txt.Text.ToUpper() + "]");
                                pdfTable1.HorizontalAlignment = Element.ALIGN_BASELINE;
                                
                                #region Image
                                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(pictureBox1.Image, System.Drawing.Imaging.ImageFormat.Jpeg);
                                /*if (pic.Height > pic.Width)
                                {
                                    //Maximum height is 800 pixels.
                                    float percentage = 0.0f;
                                    percentage = 700 / pic.Height;
                                    pic.ScalePercent(percentage * 100);
                                }
                                else
                                {
                                    //Maximum width is 600 pixels.
                                    float percentage = 0.0f;
                                    percentage = 540 / pic.Width;
                                    pic.ScalePercent(percentage * 100);
                                }*/
                                p.Alignment = Element.ALIGN_TOP;
                                
                                pic.ScaleToFit(200f,200f);
                                pic.Alignment = iTextSharp.text.Image.TEXTWRAP | iTextSharp.text.Image.ALIGN_MIDDLE;
                                pic.IndentationLeft = 9f;
                                pic.SpacingAfter = 9f;
                                pic.BorderWidthTop = 36f;
                                pic.BorderColorTop = iTextSharp.text.BaseColor.WHITE;
                                //pic.Border = iTextSharp.text.Rectangle.BOX;
                                pic.BorderColor = iTextSharp.text.BaseColor.BLACK;
                                pic.BorderWidth = 3f;
                                pic.SpacingAfter = 20;
                                pdfTable1.SpacingBefore = 20;
                                pdfDoc.Add(p);
                                pdfDoc.Add(pic);
                                
                                pdfDoc.Add(pdfTable1);
                                #endregion Image
                                pdfDoc.Close();
                                stream.Close();
                            }

                            MessageBox.Show("Data Exported Successfully !!!", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                            Log.Error("Error = " + ex.Message + "\n==========");
                            //logger.WriteErrorOnLog(LogId, 1, ex, "Error");
                        }
                    }
               }

            }
        }
        private static int CountControls(ref int tbCount, Control.ControlCollection controls)
        {
            int count = 0;
            foreach (Control wc in controls)
            {
                if (wc is TextBox)
                    tbCount++;
                else if (wc.Controls.Count > 0)
                    CountControls(ref tbCount, wc.Controls);
                count = tbCount;
            }
            return count;
        }

        private void disposeColor(object sender, EventArgs e)
        {
            button6.BackColor = Color.FromArgb(35, 32, 39);
        }

        private static int CountControlsLabel(ref int tbCount, Control.ControlCollection controls)
        {
            int count = 0;
            foreach (Control wc in controls)
            {
                if (wc is Label)
                    tbCount++;
                else if (wc.Controls.Count > 0)
                    CountControls(ref tbCount, wc.Controls);
                count = tbCount;
            }
            return count;
        }
        public bool isChanged = false;
        private void imageChanged(object sender, EventArgs e)
        {
            isChanged = true;

        }

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture; DA FARE
        private void button1_Click(object sender, EventArgs e)
        {
            
            /*if (currentCulture.Name.Equals("it-IT"))
            {

            }*/ //DA FARE

            // string myConnectionString = DBManager.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
            //MySqlConnection msqData = new MySqlConnection(myConnectionString);
            // DBManager.openConnection();
            //modify data
            if (name_txt.Text != name || sur_txt.Text != surname || rh_txt.Text != rh || bloodg_txt.Text != bloodg || addr_txt.Text != addr || tel_txt.Text != tel || email_txt.Text != email || isChanged)
            {
                DialogResult dialogResult = MessageBox.Show("Some data have changed, save them?", "Change the data", MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                       
                        
                        DBManager.openConnection();
                        string queryU = "UPDATE donators SET Name = '" + name_txt.Text + "', Surname = '" + sur_txt.Text + "', RH ='" + rh_txt.Text + "', BloodG ='" + bloodg_txt.Text + "', Address ='" + addr_txt.Text + "', Telephone ='" + tel_txt.Text + "', Email ='" + email_txt.Text + "' WHERE UID = '" + UID + "';";
                        MySqlCommand msc2 = new MySqlCommand(queryU, DBManager.conn);
                        msc2.ExecuteNonQuery();
                        
                        Log.Info($"Update donator -> [{name_txt.Text}] " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                        DBManager.closeConnection();
                        if (isChanged)
                        {
                            MemoryStream ms = new MemoryStream();
                            pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                            byte[] img = ms.ToArray();
                            //byte[] a;
                            //DialogResult dialogResult = MessageBox.Show("Some data have changed, save them?", "Change the data", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            //if (dialogResult == DialogResult.Yes)
                           //{
                                try
                                {
                                    DBManager.openConnection();
                                    string updateImage = "UPDATE donators set userPicture = @img where UID = '" + UID + "'";
                                    MySqlCommand msc = new MySqlCommand(updateImage, DBManager.conn);
                                    msc.Parameters.Add("@img", MySqlDbType.LongBlob);
                                    msc.Parameters["@img"].Value = img;
                                    msc.ExecuteNonQuery();
                                    //MessageBox.Show("User data changed", "Modified data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    Log.Info($"Update donator profile pic -> [{name_txt.Text}] " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                                    DBManager.closeConnection();
                                    this.Close();
                                }
                                catch (Exception exc)
                                {
                                    DBManager.closeConnection();
                                    MessageBox.Show("See log file for more information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    Log.Error("Error -> " + exc.Message + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                                }

                           // }

                        }
                        MessageBox.Show("User data changed", "Modified data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        DBManager.closeConnection();
                        MessageBox.Show("See log file for more information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Log.Error("Error -> " + ex.Message + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    }
                    

                }
                else
                {
                    //
                    ShowUserData s = new ShowUserData();
                   
                }
            }
            
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        byte[] img;
        private void UserDetails_Load(object sender, EventArgs e)
        {
            string myConnectionString = DBManager.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
            MySqlConnection msqConn = new MySqlConnection(myConnectionString);
            name = ShowUserData.UserValue;
            surname = ShowUserData.SurValue;
            rh = ShowUserData.RH;
            bloodg = ShowUserData.BloodG;
            addr = ShowUserData.Addr;
            tel = ShowUserData.Tel;
            email = ShowUserData.Email;


            name_txt.Text = name;
            sur_txt.Text = surname;
            rh_txt.Text = rh;
            bloodg_txt.Text = bloodg;
            addr_txt.Text = addr;
            tel_txt.Text = tel;
            email_txt.Text = email;
            UserDetails u = new UserDetails();
            var controls = u.Controls;
            var textBoxCount = 0;
            var labelCount = 0;
            countControls = CountControls(ref textBoxCount, controls);
            countLabel = CountControlsLabel(ref labelCount, controls);
            string queryS = "SELECT UID from donators where Name = '" + name_txt.Text + "'";
            //query update to modify data of the user
            msqConn.Open();
            try
            {
                //DBManager.openConnection();
                MySqlCommand msc = new MySqlCommand(queryS, msqConn);
                MySqlDataReader reader = msc.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UID = reader["UID"].ToString();

                    }

                }
                //msc.ExecuteNonQuery();
                msqConn.Close();
                //DBManager.closeConnection();
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
                if (msqConn.State == ConnectionState.Open)
                {
                    msqConn.Close();
                }
            }

        }
    }
}
