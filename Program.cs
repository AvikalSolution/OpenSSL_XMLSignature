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
            var verifybool=Console.ReadKey().KeyChar;
            XmlEnv _xmlEnv = new XmlEnv();            
            if(char.ToLower(verifybool) != 'y')
            {
                Console.WriteLine("---- Create XML Data ---- ");
                MakeData();
                Console.WriteLine("---- File saved [Data.xml] ---- ");
                
                Console.WriteLine("===== XML Signature ==== ");
                Console.WriteLine("Please specify data file path: ");
                var filePath=Console.ReadLine();
                
                _xmlEnv.CreateXmlData(filePath);
                Console.WriteLine("==== File saved [SignedXML.xml] ==== ");
            }

            Console.WriteLine("---- Verify Signature ---- ");
            Console.WriteLine("Please specify signed XML data file path: ");
            var signed_filePath=Console.ReadLine();
            Console.WriteLine(_xmlEnv.VerifySignature(signed_filePath));
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

    class XmlEnv
    {
        XmlDocument xmldoc = new XmlDocument {PreserveWhitespace = true};
        X509Certificate2 self_sign_cert=null;
        public XmlEnv()
        {
            GetCertificate();
        }
        
        public void CreateXmlData(string filePath)
        {
            xmldoc.Load(filePath);
            var signedXml = GetSignedXml();
            xmldoc.DocumentElement?.AppendChild(signedXml);
            xmldoc.Save("SignedXML.xml");
        }

        public void GetCertificate()
        {
            var rsa = RSA.Create(2048);
            var csr= new CertificateRequest("CN=avikal organisation", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            self_sign_cert = csr.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(365), DateTimeOffset.UtcNow.AddDays(365));
        }
       
        private XmlElement GetSignedXml()
        {
            XmlElement xml= xmldoc.DocumentElement;
            var key = (AsymmetricAlgorithm) self_sign_cert.GetRSAPrivateKey();
                        
            var signedXml = new SignedXml(xml) {SigningKey = key};
            signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
            signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";

            var reference = new Reference {Uri = string.Empty}; 
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(reference);

            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(self_sign_cert));
            signedXml.KeyInfo = keyInfo;
            signedXml.ComputeSignature();
            return signedXml.GetXml();
        }

        public bool VerifySignature(string filePath)
        {
            XmlDocument signedXmldoc = new XmlDocument {PreserveWhitespace = true};
            signedXmldoc.Load(filePath);
            XmlElement xml= signedXmldoc.DocumentElement;

            var signedXml = new SignedXml(xml);
            var signatureElement = xml.GetElementsByTagName("Signature");
            
            signedXml.LoadXml((XmlElement) signatureElement[0]);
            
            var publicCert = new X509Certificate2(self_sign_cert.Export(X509ContentType.Cert));
            return signedXml.CheckSignature((AsymmetricAlgorithm) publicCert.GetRSAPublicKey());
        }

    }
}
