#! /bin/bash
openssl req -new -newkey rsa:2048 -nodes -keyout private.key -out certificateReq.csr -subj "/C=IN/ST=Pune/L=MH/O=avikal Company, Inc./OU=IT/CN=avikaldomain.com"
openssl x509 -signkey private.key -in certificateReq.csr -req -days 365 -out certificate.crt
openssl pkcs12 -inkey private.key -in certificate.crt -export -out pkcs#12certificate.pfx
#openssl pkcs12 -in pkcs#12certificate.pfx -nodes -out pkcs#12_pem_certificate.crt
openssl pkcs12 -in pkcs#12certificate.pfx -nodes -out certificate.pem

#chmod u+x gen_cert.sh
