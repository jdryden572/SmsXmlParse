using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

// MMS images seem to be encoded ad Base64
// http://stackoverflow.com/questions/5083336/decoding-base64-image looks good for decoding them

namespace SmsXmlParse
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument archive = new XmlDocument();
            archive.Load(@"C:\Users\Jdryden\Downloads\02Aug2015.xml");

            XmlNodeList smsNodes = archive.DocumentElement.SelectNodes("/smses/sms");

            List<SMS> smsList = XMLtoSMSList(smsNodes);

            List<SMSThread> threads = new List<SMSThread>();

            foreach(SMS sms in smsList)
            {
                SMSThread parentThread = threads.Find(i => i.Contact == sms.Contact);   // check if thread exists

                if (parentThread == null)   // if not, make new one and use LINQ to get all relevant SMSs
                {
                    parentThread = new SMSThread();
                    parentThread.Contact = sms.Contact;
                    threads.Add(parentThread);

                    parentThread.Messages = new List<SMS>();
                    parentThread.Messages.AddRange(from s in smsList
                                                   where s.Contact == sms.Contact
                                                   select s);


                }

            }

            /*
            // check if any fields are null on any SMSs
            bool msgNull = false;
            bool numNull = false;
            bool conNull = false;

            foreach (SMS s in smsList)
            {
                msgNull |= (s.Message == null);
                numNull |= (s.Number == null);
                conNull |= (s.Contact == null);
            }
            Console.WriteLine("Null messages: {0}", msgNull);
            Console.WriteLine("Null numbers : {0}", numNull);
            Console.WriteLine("Null contacts: {0}", conNull);
            */

            foreach (SMSThread thread in threads)
            {
                string filePath = @"C:\Users\Jdryden\Documents\SMS\" + thread.Contact + @".txt";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
                {
                    foreach (SMS sms in thread.Messages)
                    {
                        file.WriteLine
                    }

                }
            }


            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Jdryden\Documents\SMS\test.txt", true))
            {
                file.WriteLine(smsList[12000].Contact);
                file.WriteLine(smsList[12000].Timestamp);
                file.WriteLine(smsList[12000].Type);
                file.WriteLine(smsList[12000].Message);
            }
            

                Console.WriteLine("Total SMS: " + smsList.Count);

            Console.WriteLine("Thread Msgs: " +
                threads.Find(i => i.Contact == "Megan Cass").Messages.Count);

            foreach (SMS msg in threads.Find(i => i.Contact == "Megan Cass").Sorted())
            {
                Console.WriteLine("{0} {1}", msg.Type, msg.Timestamp);
                Console.WriteLine(msg.Message);
                Console.WriteLine();

            }
            //Console.WriteLine(smsList[14000].Timestamp);
            //if(smsList[14000].Message != null) Console.WriteLine(smsList[14000].Message);
            Console.ReadLine();


        }

        static List<SMS> XMLtoSMSList(XmlNodeList nodeList)
        {
            List<SMS> smsList = new List<SMS>();

            // instantiate an SMS object for every text in the XML file and add it to smsList
            foreach (XmlNode node in nodeList)
            {

                SMS sms = new SMS();

                sms.Contact = node.Attributes["contact_name"]?.Value;
                sms.Number = node.Attributes["address"]?.Value;
                DateTime.TryParse(node.Attributes["readable_date"]?.Value, out sms.Timestamp);
                sms.Message = node.Attributes["body"]?.Value;
                sms.Type = (node.Attributes["type"]?.Value == "1") ? MSGType.Received : MSGType.Sent;

                smsList.Add(sms);
            }

            return smsList;
        }
    }

    enum MSGType {Sent, Received};

    class SMS
    {
        public string Contact;
        public string Number;
        public string Message;
        public DateTime Timestamp;
        public MSGType Type;

    }

    class SMSThread
    {
        public string Contact;
        public List<SMS> Messages;

        public List<SMS> Sorted()
        {
            return Messages.OrderBy(d => d.Timestamp).ToList();
        }
    }

    // MMS is much trickier
    class MMS
    {
        public string Contact;
        public DateTime Timestamp;
        public bool TextOnly;


    }
}
