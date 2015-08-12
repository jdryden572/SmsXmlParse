using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmsXmlParse
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument archive = new XmlDocument();
            archive.Load(@"E:\Google Drive\General\SMS Backups\02Aug2015.xml");

            XmlNode nodes = archive.DocumentElement.SelectSingleNode("/smses");

            List<SMS> smsList = new List<SMS>();

            foreach (XmlNode node in nodes)
            {
                SMS sms = new SMS();

                sms.Contact = node.Attributes["contact_name"].Value;
                sms.Number = node.Attributes["address"].Value;
                DateTime.TryParse(node.Attributes["readable_date"].Value, out sms.Timestamp);
                //Int32.TryParse(node.Attributes["type"].InnerText, out sms.type);
                //sms.Message = node.Attributes["body"].InnerText;

                smsList.Add(sms);

            }

            Console.WriteLine("Total SMS: " + smsList.Count);
            Console.WriteLine(smsList[15000].Timestamp);
            Console.ReadLine();


        }
    }

    class SMS
    {
        public string Contact;
        public string Number;
        public string Message;
        public DateTime Timestamp;
        public int type;

    }
}
