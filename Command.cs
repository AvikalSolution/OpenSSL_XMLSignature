using System;
using System.IO;

namespace openssl_xml_envelope
{
    class Command
    {
        string[] command={
            "openssl x509 -inform der -in certificate.cer -outform pem -out certificate.pem",
            "openssl x509 -in certificate.cer -text -noout"
        };
        //openssl genrsa -aes256 -out c:\\folder\\test_file.key.pem 2048
        
        public string[] commandDesc={
            "1. Convert DER to PEM encoding",
            "2. View DER encoded Certificate"
        };
        
        public void ProcessCommand(int code)
        {
            System.Diagnostics.Process cmd = new System.Diagnostics.Process();

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();

            using (StreamWriter sw = cmd.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    switch(code)
                    {
                        case 1:
                            sw.WriteLine(command[code-1]);
                            break;
                        case 2:
                            sw.WriteLine(command[code-1]);
                            break;
                    }
                    
                }
            }
        }
    }
}