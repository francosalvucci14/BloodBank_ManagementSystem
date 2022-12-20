using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BloodBank_ManagementSystem_v2
{
    [RunInstaller(true)]
    public partial class AppInstaller : System.Configuration.Install.Installer
    {
        public AppInstaller()
        {
            InitializeComponent();
        }
        public static string supervisorName = "SUPER";
        public static string supervisorPsw = "super";
        public static string defaultCulture = "en-EN";
        #region dbconnection
        public static string user = "";
        public static string psw = "";
        public static string db = "";
        public static string serverIP = "";
        public static string assemblyPath = ""; 
        #endregion
        /*public static string user = "";
        public static string pass = "";*/
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            //supervisorName = Context.Parameters["Supervisor"];
            //supervisorPsw = Context.Parameters["Password"];
            user = Context.Parameters["User"];
            psw = Context.Parameters["DbPassword"];
            db = Context.Parameters["Database"];
            serverIP = Context.Parameters["IP"];
            
            //getting the full path including the filename
            assemblyPath = Context.Parameters["assemblyPath"];         
            //removing the filename from the path
            int i = assemblyPath.Length - 1;
            while (assemblyPath[i] != '\\') --i;
            string path = assemblyPath.Substring(0, i);

            //MessageBox.Show(path);
            ShareFolderPermission(path, "BloodBankClient","Blood Bank Client Application");
            var pswByte = Encoding.Unicode.GetBytes(supervisorPsw);
            RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\SalvucciF\BloodBank");
            key.SetValue("BloodBankUserID", supervisorName);
            key.SetValue("BloodBankPsw", pswByte, RegistryValueKind.Binary);
            key.SetValue("SaveData", 0,RegistryValueKind.DWord);
            key.SetValue("Culture_Default", defaultCulture);
            key.SetValue("CurrentCulture", "");
            key.SetValue("INSTALL_PATH",path);
            /*key.SetValue("User", user);
            key.SetValue("Password", pass);*/
            key.Close();
            var dbpswByte = Encoding.Unicode.GetBytes(psw);
            RegistryKey keyConn = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\SalvucciF\BloodBank\Connection");
            keyConn.SetValue("IP", serverIP);
            keyConn.SetValue("DB", db);
            keyConn.SetValue("User", user);
            keyConn.SetValue("DbPassword", dbpswByte, RegistryValueKind.Binary);
            /*key.SetValue("User", user);
            key.SetValue("Password", pass);*/
            keyConn.Close();
        }
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\SalvucciF\BloodBank");
            key.DeleteValue("BloodBankUserID");
            key.DeleteValue("BloodBankPsw");
            key.DeleteValue("SaveData");
            key.DeleteValue("Culture_Default");
            key.DeleteValue("CurrentCulture");
            key.DeleteValue("INSTALL_PATH");
            key.Close();
            

            RegistryKey keyConn = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\SalvucciF\BloodBank\Connection");
            keyConn.DeleteValue("IP");
            keyConn.DeleteValue("DB");
            keyConn.DeleteValue("User");
            keyConn.DeleteValue("DbPassword");
            /*key.SetValue("User", user);
            key.SetValue("Password", pass);*/
            keyConn.Close();
            
            RemoveSharedFolder("BloodBankClient");
        }
        public void ShareFolderPermission(string FolderPath, string ShareName, string Description)
        {
            try
            {
                // Calling Win32_Share class to create a shared folder
                ManagementClass managementClass = new ManagementClass("Win32_Share");
                // Get the parameter for the Create Method for the folder
                ManagementBaseObject inParams = managementClass.GetMethodParameters("Create");
                ManagementBaseObject outParams;
                
                // Assigning the values to the parameters
                inParams["Description"] = Description;
                inParams["Name"] = ShareName;
                inParams["Path"] = FolderPath;
                inParams["Type"] = 0x0;
                // Finally Invoke the Create Method to do the process
                outParams = managementClass.InvokeMethod("Create", inParams, null);
                // Validation done here to check sharing is done or not
                if ((uint)(outParams.Properties["ReturnValue"].Value) != 0)
                    MessageBox.Show("Folder might be already in share or unable to share the directory");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal static void RemoveSharedFolder(string ShareName)
        {
            try
            {
                // Create a ManagementClass object
                ManagementClass managementClass = new ManagementClass("Win32_Share");
                ManagementObjectCollection shares = managementClass.GetInstances();
                foreach (ManagementObject share in shares)
                {
                    if (Convert.ToString(share["Name"]).Equals(ShareName))
                    {
                        var result = share.InvokeMethod("Delete", new object[] { });

                        // Check to see if the method invocation was successful
                        if (Convert.ToInt32(result) != 0)
                        {
                            MessageBox.Show("Unable to unshare directory.");
                        }
                        else
                        {
                            //MessageBox.Show("Folder successfuly unshared.");
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
