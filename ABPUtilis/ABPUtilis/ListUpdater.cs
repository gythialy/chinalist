using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ABPUtils
{
    public class ListUpdater
    {
        private const string ChecksumRegx = @"(\!\s*checksum[\s\-:]+)([\w\+\/=]+).*(\n)";
        private const string UrlRegx = @"([a-z0-9][a-z0-9\-]*?\.(?:com|edu|cn|net|org|gov|im|info|la|co|tv|biz|mobi)(?:\.(?:cn|tw))?)";

        public String FileName
        {
            get;
            private set;
        }

        public ListUpdater(string fileName)
        {
            FileName = fileName.ToFullPath();
        }

        /// <summary>
        /// update list time and checksum
        /// </summary>
        public void Update()
        {
            var content = LoadContent();
            content = UpdateTime(content);
            content = RemoveChecksum(content);

            var result = UpdateCheckSum(content);
            ChinaLists.Save(FileName, result);
        }

        /// <summary>
        /// validate list checksum
        /// </summary>
        /// <returns></returns>
        public int Validate()
        {
            var content = ReadListToEnd();
            var checkSum = FindCheckSum(content);
            if (string.IsNullOrEmpty(checkSum))
            {
                Console.WriteLine("Couldn't find a checksum in the file {0}", FileName);
                return -1;
            }

            content = RemoveChecksum(content);
            var genearteCheckSum = CalculateMd5Hash(RemoveEmptyLines(content));

            if (checkSum.Equals(genearteCheckSum))
            {
                Console.WriteLine("Checksum in the file {0} is valid.", FileName);
                return 1;
            }
            Console.WriteLine("Wrong checksum [{0}] found in the file {1}, expected is [{2}]", checkSum, FileName, genearteCheckSum);
            return 0;
        }

        /// <summary>
        /// get domains from list
        /// </summary>
        /// <returns></returns>
        public List<string> GetDomains()
        {
            var urls = new List<string>();

            string s;
            using (var sr = new StreamReader(FileName, Encoding.UTF8))
            {
                s = sr.ReadToEnd();
            }

            var regex = new Regex(UrlRegx, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var matches = regex.Matches(s);
            foreach (var match in from Match match in matches let url = match.Value where !urls.Contains(url) select match)
            {
                urls.Add(match.Value);
            }
            urls.Sort();

            return urls;
        }

        /// <summary>
        /// Read list content then remove duplicate and empty line
        /// </summary>
        /// <returns></returns>
        private string LoadContent()
        {
            var lines = File.ReadAllLines(FileName, Encoding.UTF8).Where(s => !string.IsNullOrEmpty(s)).Distinct();
            return string.Join("\n", lines);
        }

        /// <summary>
        /// Read list content one time
        /// </summary>
        /// <returns></returns>
        private string ReadListToEnd()
        {
            string content;

            using (var sr = new StreamReader(FileName, Encoding.UTF8))
            {
                content = sr.ReadToEnd();
                content = content.Replace("\r", string.Empty);
            }

            return content;
        }

        /// <summary>
        /// change list update time
        /// </summary>
        /// <returns></returns>
        private static string UpdateTime(string content)
        {
            var dt = DateTime.Now;
            //Wed, 22 Jul 2009 16:39:15 +0800
            var time = string.Format("Last Modified:  {0}", dt.ToString("r")).Replace("GMT", "+0800");
            var regex = new Regex(@"Last Modified:.*$", RegexOptions.Multiline);
            content = regex.Replace(content, time);

            return content;
        }

        /// <summary>
        /// Remove empty lines
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string RemoveEmptyLines(string content)
        {
            content = Regex.Replace(content, "\n+", "\n");
            return content;
        }

        /// <summary>
        /// remove checksum
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string RemoveChecksum(string content)
        {
            return Regex.Replace(content, ChecksumRegx, string.Empty, RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Get checksum for list
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string FindCheckSum(string content)
        {
            var regex = new Regex(ChecksumRegx, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var match = regex.Match(content);

            return match.Success ? match.Groups[2].Value.Trim() : string.Empty;
        }

        /// <summary>
        /// update checksum
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string UpdateCheckSum(string content)
        {
            return Regex.Replace(content, @"(\[Adblock Plus \d\.\d\])",
                        string.Format("$1\n!  Checksum: {0}", CalculateMd5Hash(RemoveEmptyLines(content))),
                        RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Calculate md5 hash of the string content
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string CalculateMd5Hash(string content)
        {
            string result;
            using (var x = new MD5CryptoServiceProvider())
            {
                var md5Hash = Encoding.UTF8.GetBytes(content);
                var hashResult = x.ComputeHash(md5Hash);
                result = Convert.ToBase64String(hashResult);
                //remove trailing = characters if any
                result = result.TrimEnd('=');
            }

            return result;
        }
    }
}
