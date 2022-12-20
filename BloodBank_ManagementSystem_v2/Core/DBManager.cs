using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank_ManagementSystem_v2.Core
{
    public class DBManager
    {


        public static string connString = DBManager.getConnectionString();
          //"Data Source=localhost;Database=bloodbank_ms;userid=root;password=;";
        public static MySqlConnection conn = new MySqlConnection(connString);

        public static bool openConnection()
        {
            try
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.ConnectionString = DBManager.connString;
                    conn.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool closeConnection()
        {
            try
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string getConnectionString()
        {
            string conn = "";
            string ip = "";
            string db = "";
            string user = "";
            string psw = "";
            RegistryKey keyConn = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\SalvucciF\BloodBank\Connection");
            var data = (byte[])keyConn.GetValue("DbPassword");
            if (keyConn != null)
            {
                ip = (string)keyConn.GetValue("IP");
                db = (string)keyConn.GetValue("DB");
                user = (string)keyConn.GetValue("User");
                psw = Encoding.Unicode.GetString(data);
            }
            conn = $"Data Source={ip};Database={db};userid={user};password={psw};";//Data Source=localhost;Database=bloodbank_ms;userid=root;password=;
            return conn;
        }
    }
}
