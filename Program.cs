using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Text;

namespace openssl_xml_envelope
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---- Verify if sample file is present: Y/N ---- ");
            var verifybool=Console.ReadLine();
            
            XmlEnv _xmlEnv = new XmlEnv();            
            
            if(!verifybool.ToLower().Equals("y"))
            {
                Console.WriteLine("---- [XML Data] creation getting started --- ");
                MakeData();
                Console.WriteLine("File saved [Data.xml] ...... ");
                
                Console.WriteLine("===== [XML Signature] Initilizing ==== ");
                Console.WriteLine(">> Please specify data file path: ");
                var filePath=Console.ReadLine();
                
                _xmlEnv.CreateXmlData(filePath);
                Console.WriteLine("File saved [SignedXML.xml] ..... ");
            }

            Console.WriteLine("---- Verify Signature ---- ");
            Console.WriteLine(">> Please specify signed XML data file path: ");
            var signed_filePath=Console.ReadLine();
            Console.WriteLine("Is Signature Valid ? "+ _xmlEnv.VerifySignature(signed_filePath));
        }

        static void MakeData()
        {
            string[] Query = {"Sender Name: ", "Sender Bank Name: ","Receiver Name:", "Sender Account#: ","Receiver Bank Name: ","Receiver Account#: " , "Transaction Amount: "};
            string[] OpenTagName= {"<SenderName>", "<SenderBankName>","<ReceiverName>", "<SenderAccount>","<ReceiverBankName>","<ReceiverAccount>" , "<Amount>"};
            string[] CloseTagName= {"</SenderName>", "</SenderBankName>","</ReceiverName>", "</SenderAccount>","</ReceiverBankName>","</ReceiverAccount>" , "</Amount>"};
            StringBuilder _xMessage= new StringBuilder();
            _xMessage.Append("<data>");
            for(int counter=0;counter<Query.Length;counter++)
            {
                Console.WriteLine(Query[counter]);
                var input = Console.ReadLine();
                _xMessage.Append(OpenTagName[counter]);
                _xMessage.Append(input);
                _xMessage.Append(CloseTagName[counter]);

            }
            
            _xMessage.Append("</data>");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_xMessage.ToString());
            doc.Save("Data.xml");
            
        }

    }

    
}
