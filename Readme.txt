$ dotnet --version
$ dotnet new console --framework net5.0
dotnet new gitignore
$ dotnet add package System.Security.Cryptography.Xml

Do the following steps to create launch.json and tasks.json:
Select Run > Add Configuration from the menu.
Select .NET 5+ and .NET Core at the Select environment prompt.

$ dotnet run

git init
git status
git add .
git status

git commit - m "Initial Checkin"
git log

git remote add origin git@github.com:avikalsolution/<reponame>.git
git push -u origin master

--git remote remove origin
--git remote set-url origin git://new.url

git fetch origin
git status
git pull


View PEM encoded certificate
   openssl x509 -in cert.pem -text -noout
-->openssl x509 -in certificate.cer -text -noout
   openssl x509 -in cert.crt -text -noout

View DER encoded Certificate
-->openssl x509 -in certificate.cer -inform der -text -noout

Transform 
    PEM to DER
    openssl x509 -in cert.crt -outform der -out cert.der

    DER to PEM
    openssl x509 -in cert.crt -inform der -outform pem -out cert.pem
 -->openssl x509 -inform der -in certificate.cer -outform pem -out certificate.pem

 Remove the passphrase from the private key using openssl:
    openssl rsa -in EncryptedPrivateKey.pem -out PrivateKey.pem

