using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ABPUtils
{
    class Whois
    {
        public enum RecordType { Domain, Nameserver, Registrar };

        /// <summary>
        /// retrieves whois information
        /// </summary>
        /// <param name="domainname">The registrar or domain or name server whose whois information to be retrieved</param>
        /// <param name="recordType">The type of record i.e a domain, nameserver or a registrar</param>
        /// <param name="whoisServerAddress"></param>
        /// <returns>The string list containg the whois information</returns>
        public static List<string> Lookup(string domainname, RecordType recordType, string whoisServerAddress = "whois.internic.net")
        {
            var tcp = new TcpClient();
            tcp.Connect(whoisServerAddress, 43);
            var strDomain = recordType.ToString() + " " + domainname + "\r\n";
            var bytDomain = Encoding.ASCII.GetBytes(strDomain.ToCharArray());
            Stream s = tcp.GetStream();
            s.Write(bytDomain, 0, strDomain.Length);
            var sr = new StreamReader(tcp.GetStream(), Encoding.ASCII);
            string strLine;
            var result = new List<string>();
            while (null != (strLine = sr.ReadLine()))
            {
                result.Add(strLine);
            }
            tcp.Close();
            return result;
        }
    }
}
