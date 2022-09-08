This application is getting created and its purpose it to demonstrate how the XML signature envelope file is verified

Instructions:
--------------

chmod u+x gen_cert.sh

Execute the script
./gen_cert.sh

Note:
if prompted for "Export password" leave that blank

Output: certificate.pem

Build the file, publish it at particular location, to get "exe" and its dependencies in certain folder
copy the "certificate.pem" in the publish folder
Launch the "exe" file

Function
----------
1. Create sample files if they do not exist
   >Create the sample Data.xml file
   >Create the sample XML signature Envelope file from SampleXML.xml file

2. If the sample file and the certificate already exist, copy that in folder where "exe" file is present

3. Verify XML signature




