using System.Collections.Generic;
using System.Text;

namespace ABPUtils
{
    public class QueryResult
    {
        public string Domain
        {
            get;
            set;
        }

        public string Dns
        {
            get;
            set;
        }

        public int NsCount
        {
            get;
            set;
        }

        public List<string> NsList
        {
            get;
            set;
        }

        public string Info
        {
            get;
            set;
        }

        public string Error
        {
            get;
            set;
        }

        public QueryResult()
        {
            NsList = new List<string>();
            Error = "NONE.";
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("Domain:\t{0}\n", Domain);
            sb.AppendFormat("DNS:\t{0}\n", Dns);
            sb.AppendFormat("Info:\t{0}\n", Info);
            sb.AppendFormat("Count:\t{0}\n", NsCount);
            foreach (var ns in NsList)
            {
                sb.AppendFormat("NS => {0}\n", ns);
            }
            sb.AppendFormat("Error:\t{0}\n", Error);
            sb.AppendLine("--------------------------------------------------------------");

            return sb.ToString();
        }
    }
}
