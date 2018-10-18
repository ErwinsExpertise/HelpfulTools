using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using ARSoft.Tools.Net.Dns;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace HelpfulTools
{
    class Worker
    {
        private const int Whois_Server_Default_PortNumber = 43;
        private const string Domain_Record_Type = "domain";
        private const string DotCom_Whois_Server = "whois.verisign-grs.com";
        private static StringBuilder bldr = new StringBuilder();

        public static string hostResolve(string fqdn)
        {
            bldr.Clear();
            try
            {
                using (TcpClient whoisClient = new TcpClient())
                {
                    whoisClient.Connect(DotCom_Whois_Server, Whois_Server_Default_PortNumber);

                    string domainQuery = Domain_Record_Type + " " + fqdn + "\r\n";
                    byte[] domainQueryBytes = Encoding.ASCII.GetBytes(domainQuery.ToCharArray());

                    Stream whoisStream = whoisClient.GetStream();
                    whoisStream.Write(domainQueryBytes, 0, domainQueryBytes.Length);

                    StreamReader whoisStreamReader = new StreamReader(whoisClient.GetStream(), Encoding.ASCII);

                    string streamOutputContent = "";
                    List<string> whoisData = new List<string>();
                    while (null != (streamOutputContent = whoisStreamReader.ReadLine()))
                    {
                        whoisData.Add(streamOutputContent);
                    }

                    whoisClient.Close();

                    return String.Join(Environment.NewLine, whoisData);

                }//end host resolve
            }
            catch (WebException ex)
            {
                return ex.ToString();
            }
            catch (IOException ex)
            {
                return ex.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


        }//end host resolve


        public static string recordResolve(string fqdn)
        {
            bldr.Clear();
            var resolver = new DnsStubResolver();
            bldr.Append("---------A RECORDS---------" + Environment.NewLine);
            var arecords = resolver.Resolve<ARecord>(fqdn, RecordType.A);
            foreach (var record in arecords)
            {
                bldr.Append(record.ToString() + Environment.NewLine);
            }
            bldr.Append("---------NS RECORDS---------" + Environment.NewLine);
            var nsrecords = resolver.Resolve<NsRecord>(fqdn, RecordType.Ns);
            foreach (var record in nsrecords)
            {
                bldr.Append(record.ToString() + Environment.NewLine);
            }
            bldr.Append("---------CNAME RECORDS---------" + Environment.NewLine);
            var crecords = resolver.Resolve<CNameRecord>(fqdn, RecordType.CName);
            foreach (var record in crecords)
            {
                bldr.Append(record.ToString() + Environment.NewLine);
            }
            bldr.Append("---------MX RECORDS---------" + Environment.NewLine);
            var mxrecords = resolver.Resolve<MxRecord>(fqdn, RecordType.Mx);
            foreach (var record in mxrecords)
            {
                bldr.Append(record.ToString() + Environment.NewLine);
            }
            bldr.Append("---------TXT RECORDS---------" + Environment.NewLine);
            var txtrecords = resolver.Resolve<TxtRecord>(fqdn, RecordType.Txt);
            foreach (var record in txtrecords)
            {
                bldr.Append(record.ToString() + Environment.NewLine);
            }
            bldr.Append("---------CAA RECORDS---------" + Environment.NewLine);
            var carecords = resolver.Resolve<CAARecord>(fqdn, RecordType.CAA);
            foreach (var record in carecords)
            {
                bldr.Append(record.ToString() + Environment.NewLine);
            }

            return bldr.ToString();
        }//end record resolve


        public static string sslCheck(string fqdn)
        {
            //Do webrequest to get info on secure site
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://" + fqdn);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            response.Close();

            //retrieve the ssl cert and assign it to an X509Certificate object
            X509Certificate cert = request.ServicePoint.Certificate;

            //convert the X509Certificate to an X509Certificate2 object by passing it into the constructor
            X509Certificate2 cert2 = new X509Certificate2(cert);


            return cert2.ToString();
        }//end sslCheck

        public static string spfCheck(string fqdn)
        {
            var spfValidator = new ARSoft.Tools.Net.Spf.SpfValidator();
            var ip = IPAddress.Parse("8.8.8.8");

            spfValidator.CheckHost(ip, fqdn, "admin@lifeoferwin.com");


            return spfValidator.ToString();
        }//end spfCheck


    }//end worker
}
