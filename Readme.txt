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

Ref:
https://yetanotherchris.dev/net-core/ssl-public-private-key-in-netcore-by-example
https://gist.github.com/svrooij/ec6f664cd93cd09e84414112d23f6a42
https://paulstovell.com/x509certificate2/
https://www.limilabs.com/blog/import-certificate-private-public-keys-pem-cer-pfx
https://gist.github.com/yetanotherchris/d8330dd6f541f85903a9bdd5dd13bb1f
https://theitbros.com/convert-crt-to-pem/
https://support.citrix.com/article/CTX124783/how-to-convert-a-pkcs-7-certificate-to-pem-format-for-use-with-netscaler