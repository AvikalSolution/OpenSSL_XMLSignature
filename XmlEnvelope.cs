using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Text;

namespace openssl_xml_envelope
{
    class XmlEnv
    {
        XmlDocument xmldoc = new XmlDocument {PreserveWhitespace = true};
        X509Certificate2 self_sign_cert=null;
        //Command cmd= new Command();
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
            
            byte[] certData = self_sign_cert.Export(X509ContentType.Cert);
            string base64String = Convert.ToBase64String(certData, 0, certData.Length);
            // Console.WriteLine("++++++++");
            // Console.WriteLine(base64String);
            File.WriteAllBytes("certificate.cer", certData);
            //Command cmd= new Command();
            //cmd.ProcessCommand(1);
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
            
            byte[] publicPemBytes = File.ReadAllBytes("certificate.pem");
            X509Certificate2 publicCert = new X509Certificate2(publicPemBytes);
            //1st version->var publicCert = new X509Certificate2(self_sign_cert.Export(X509ContentType.Cert));
            //need 2 expore var publicCert = X509Certificate2.CreateFromPemFile("pem");
            //2nd version-> X509Certificate2 publicCert = new X509Certificate2("certificate.cer");
            return signedXml.CheckSignature((AsymmetricAlgorithm) publicCert.GetRSAPublicKey());
        }

    }
}