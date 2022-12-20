using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BloodBank_ManagementSystem_v2
{
    public class ChangeLanguage
    {
        public void update(string key,string value)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            foreach (XmlElement xmlElement in xmlDoc.DocumentElement)
            {
                if (xmlElement.Name.Equals("appSettings"))
                {
                    foreach (XmlNode xmlNode in xmlElement.ChildNodes)
                    {
                        if (xmlNode.Attributes[0].Value.Equals(key))
                        {
                            xmlNode.Attributes[1].Value = value;
                        }
                    }
                }
            }
            ConfigurationManager.RefreshSection("appSettings");
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }
    }
}
