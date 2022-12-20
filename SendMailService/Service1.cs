using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SendMailService
{
    public partial class Service1 : ServiceBase
    {
       // System.Diagnostics.EventLog BloodBank_MS = new System.Diagnostics.EventLog();
        public Service1()
        {
            InitializeComponent();
            
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //FlappyLog.WriteEntry("In OnStart --- Sending Mail");

                System.Timers.Timer time = new System.Timers.Timer();

                time.Start();

                time.Interval = 10000;//300000;

                time.Elapsed += Time_Elapsed;
            }
            catch (SecurityException s)
            {
                //Console.WriteLine(s.ToString());
                using (EventLog eventLog = new EventLog())
                {
                    eventLog.Source = "BloodBank_MS";
                    eventLog.WriteEntry($"Security Exception  : [{s.Message}]", EventLogEntryType.Error, 1002);
                }
            }
        }

        public void Time_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {//------------disabilitato per soluzione migliore--------------


            //.WriteEntry("Mail Sending on " + DateTime.Now.ToString());
            //string toEmail = SearchUserEmail();
            //string toEmail = SearchUserEmailAndSend();
            //int n = getEmailNumber();
            //string[] email = new String[n];
            //FlappyLog.WriteEntry("Mail : " + toEmail);
            //string[] split = toEmail.Split(new Char[] { '|' });


            /*string delimStr = "|";
            char[] delimiter = delimStr.ToCharArray();
            string[] split = null;
            for (int x = 1; x<toEmail.Length; x++)
            {
                split = toEmail.Split(delimiter,x);
                foreach (string emails in split)
                {      
                        SendEmail(emails, "noreplyflappybirdcs@gmail.com", "Automatic Mail sending", "Successfully working contact with" + emails);

                }

            }*/
            SearchUserEmailAndSend();//richiamo funzioe ce cerca le email degli utenti abilitiati alla email automatica e la mando.
        }
        public bool SendEmail(string strTo, string strFrom, string strSubject, string strBody)
        {

            bool flag = false;

            //try
            //{
            //    var fromAddress = new MailAddress("noreplyflappybirdcs@gmail.com", "FlappyBird Team");
            //    var toAddress = new MailAddress(strTo);
            //    const string fromPassword = "FlappyBird14cs";
            //    //const string subject = "Recovery Password";
            //    // string body = "Automatic Email Sender";
            //    var smtp = new SmtpClient
            //    {
            //        Host = "smtp.gmail.com",
            //        Port = 587,
            //        EnableSsl = true,
            //        DeliveryMethod = SmtpDeliveryMethod.Network,
            //        UseDefaultCredentials = false,
            //        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            //    };
            //    using (var message = new MailMessage(fromAddress, toAddress)
            //    {
            //        Subject = strSubject,
            //        Body = strBody,

            //        //IsBodyHtml = true
            //    })
            //    {
            //        smtp.Send(message);
            //    }
            //    flag = true;
            //    //FlappyLog.WriteEntry("Mail Send Successfully");
            //}
            //catch (Exception ex)
            //{
            //    //FlappyLog.WriteEntry("Error occured: " + ex.ToString());
            //    //Response.Write(ex.Message);
            //    Console.WriteLine(ex.ToString());
            //}
            //MODIFICATO IL {30/08/2021} PER SOLUZIONE MIGLIORE
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("noreplyflappybirdcs@gmail.com", "FlappyBird14cs"),
                    EnableSsl = true
                };
                client.Send(strFrom, strTo, strSubject, strBody);
                using (EventLog eventLog = new EventLog())
                {
                    eventLog.Source = "BloodBank_MS";
                    eventLog.WriteEntry($"Mail Send Successfully ", EventLogEntryType.Information, 0);
                }
                flag = true;
            }
            catch (Exception e)
            {
                //FlappyLog.WriteEntry("Error occured: " + ex.ToString());
                //    //Response.Write(ex.Message);
                //    Console.WriteLine(ex.ToString());
                using (EventLog eventLog = new EventLog())
                {
                    eventLog.Source = "BloodBank_MS";
                    eventLog.WriteEntry($"Error while sending mail from the system : [{e.Message}]", EventLogEntryType.Error, 1002);
                }
            }



            return flag;
        }

        public void SearchUserEmailAndSend()
        {
            string myConnectionString = "server=localhost;database=bloodbank_ms;uid=root;pwd=;";
            MySqlConnection msq = new MySqlConnection(myConnectionString);
            string queryS = "SELECT * FROM donators WHERE SenderMail = 1;";
            MySqlCommand msc = new MySqlCommand(queryS, msq);

            Console.WriteLine(queryS);
            msq.Open();
            MySqlDataReader reader = msc.ExecuteReader();
            string componeEmailUser = "";
            string emailUser = "";
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    emailUser = reader["Email"].ToString();
                    componeEmailUser = emailUser + "|";
                    SendEmail(emailUser, "noreplyflappybirdcs@gmail.com", "Automatic Mail sending", "Successfully working contact with " + emailUser);
                }
            }
            reader.Close();
            //return componeEmailUser;
        }
        protected override void OnStop()
        {
        }
        
        
    }
}
