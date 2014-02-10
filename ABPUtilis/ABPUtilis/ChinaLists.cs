using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bdev.Net.Dns;

namespace ABPUtils
{
    public class ChinaLists
    {
        private const string ListEndMark = "!------------------------End of List-------------------------";
        private static readonly Configurations Configs = Configurations.Default;

        private ChinaLists() { }

        /// <summary>
        /// Get assembly version
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            return fvi.ProductVersion;
        }

        /// <summary>
        /// CombineLazyList input list with part of EasyList and EasyPrivacy
        /// </summary>
        /// <param name="input"></param>
        /// <param name="proxy"></param>
        /// <param name="patch"></param>
        /// <param name="output"></param>
        public static void CombineLazyList(string input, WebProxy proxy, bool patch, string output = "../adblock-lazy.txt")
        {
            if (!DownloadEasyList(proxy))
                return;

            if (string.IsNullOrEmpty(output))
            {
                Console.WriteLine("Merget output file is empty.");
                return;
            }

            var path = input.ToFullPath();
            if (!IsDirectory(path))
            {
                Console.WriteLine("input {0} is not dicectory.", input);
                return;
            }

            // load ListUpdater content
            var sBuilder = new StringBuilder(Configs.ChinaListLazyHeader);
            var files = GetChinaLists(path);
            foreach (var file in files)
            {
                using (var sr = new StreamReader(file, Encoding.UTF8))
                {
                    var content = sr.ReadToEnd();
                    //remove header
                    var headerRegex = new Regex(@"\[Adblock Plus [\s\S]*?NO WARRANTY but Best Wishes----",
                        RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    content = headerRegex.Replace(content, string.Empty);
                    sBuilder.AppendFormat("!*** {0} ***\n", Path.GetFileName(file));
                    sBuilder.Append(content);
                }
            }

            sBuilder.AppendLine("!*** EasyList ***");
            sBuilder.AppendLine(TrimEasyList());

            sBuilder.AppendLine("!*** EasyPrivacy ***");
            sBuilder.Append(TrimEasyPrivacy());

            //apply patch settings
            var patchFile = Configs.PatchFile;
            if (File.Exists(patchFile) && patch)
            {
                Console.WriteLine("use {0} to patch {1}", patchFile, output);

                var pConfig = GetConfigurations();

                if (pConfig != null)
                {
                    foreach (var item in pConfig.RemovedItems)
                    {
                        sBuilder.Replace(item + "\n", string.Empty);
                        Console.WriteLine("remove filter {0}", item);
                    }

                    foreach (var item in pConfig.ModifyItems)
                    {
                        sBuilder.Replace(item.OldItem, item.NewItem);
                        Console.WriteLine("replace filter {0} with {1}", item.OldItem, item.NewItem);
                    }

                    if (pConfig.NewItems.Count > 0)
                        sBuilder.AppendLine("!-----------------additional for ChinaList Lazy-------------");

                    foreach (var item in pConfig.NewItems)
                    {
                        sBuilder.AppendLine(item);
                        Console.WriteLine("add filter {0}", item);
                    }
                }

                Console.WriteLine("Patch file end.");
            }

            sBuilder.AppendLine(ListEndMark);

            Console.WriteLine("Merge {0}, {1} and {2}.", input, Path.GetFileName(Configs.Easylist), Path.GetFileName(Configs.Easyprivacy));
            Save(output, sBuilder.ToString());

            var updater = new ListUpdater(output);
            updater.Update();
            updater.Validate();

            Console.WriteLine("End of combine ChinaList Lazy and validate.");
        }

        private static bool IsDirectory(string path)
        {
            var attr = File.GetAttributes(path);

            return (attr & FileAttributes.Directory) == FileAttributes.Directory;
        }

        private static IEnumerable<string> GetChinaLists(string input)
        {
            var files = Directory.GetFiles(input, "*.txt", SearchOption.TopDirectoryOnly).ToList();
            var enabled = Configs.ChinaListLazy;

            var result = files.Where(enabled.Contains).ToList();
            result.Sort();

            return result;
        }

        /// <summary>
        /// Clean Patch file
        /// </summary>
        /// <param name="chinaList"></param>
        /// <param name="proxy"></param>
        public static void CleanConfigurations(string chinaList, WebProxy proxy)
        {
            if (!DownloadEasyList(null)) return;

            var patchConfig = GetConfigurations();
            if (patchConfig == null)
            {
                Console.WriteLine("wrong Patch Confguration file.");
                return;
            }

            var sBuilder = new StringBuilder();
            using (var sr = new StreamReader(chinaList, Encoding.UTF8))
            {
                sBuilder.Append(sr.ReadToEnd());
            }

            using (var sr = new StreamReader(Configs.Easylist, Encoding.UTF8))
            {
                sBuilder.Append(sr.ReadToEnd());
            }

            using (var sr = new StreamReader(Configs.Easyprivacy, Encoding.UTF8))
            {
                sBuilder.Append(sr.ReadToEnd());
            }

            var s = sBuilder.ToString();

            var removedItems = new List<string>(patchConfig.RemovedItems);
            foreach (var item in patchConfig.RemovedItems.Where(item => !s.Contains(item)))
            {
                removedItems.Remove(item);
            }

            var modifyItems = new List<ModifyItem>(patchConfig.ModifyItems);
            foreach (var item in patchConfig.ModifyItems.Where(item => !s.Contains(item.OldItem)))
            {
                modifyItems.Remove(item);
            }

            patchConfig.ModifyItems = modifyItems;
            patchConfig.RemovedItems = removedItems;

            Save(Configs.PatchFile, SimpleSerializer.XmlSerialize(patchConfig));
        }

        /// <summary>
        /// validate domain by nslookup
        /// </summary>
        /// <param name="dns"></param>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public static void ValidateDomains(IPAddress dns, string input, string output = "invalid_domains.txt")
        {
            if (dns == null)
                dns = IPAddress.Parse("8.8.8.8");

            if (string.IsNullOrEmpty(output))
                output = "invalid_domains.txt";

            var cl = new ListUpdater(input);
            var domains = cl.GetDomains();
            //List<string> urls = cl.ParseURLs();
            var results = new StringBuilder();
            //StringBuilder fullResult = new StringBuilder();
            var whiteList = new List<string> { "ns1.dnsv2.com" };

            Parallel.ForEach(domains, domain =>
            {
                Console.WriteLine("Querying DNS records for domain: {0}", domain);
                var queryResult = DnsQuery(dns, domain);
                Console.Write(queryResult.ToString());
                //fullResult.Append(queryResult.ToString());
                var ret = false;

                if (queryResult.NsCount < 1)
                {
                    results.Append(queryResult.ToString());
                    return;
                }

                foreach (var ns in queryResult.NsList)
                {
                    var t = ns;
                    if (ns.Contains("="))
                        t = ParseNameServer(ns);

                    try
                    {
                        var ip = Dns.GetHostEntry(t);
                        var temp = DnsQuery(ip.AddressList[0], domain);
                        if (temp.NsCount > 0 || whiteList.Contains(t))
                        {
                            ret = true;
                            break;
                        }
                        queryResult.Error += string.Format("\n[V]: ns->{0}, Count->{1}", t, temp.NsCount);
                    }
                    catch (Exception ex)
                    {
                        queryResult.Error += string.Format("\n[V]: ns->{0}, Error->{1}", t, ex.Message);
                        Console.WriteLine("Validate domain: {0}, ns: {1} Error: {2}", domain, t, ex.Message);
                    }
                }

                if (!ret)
                {
                    queryResult.Error += "\n[V]: validate domian fail.";
                    results.Append(queryResult.ToString());
                }
            });

            Save(output, results.ToString());
        }

        public static QueryResult DnsQuery(IPAddress dnsServer, string domain)
        {
            if (dnsServer == null)
                dnsServer = IPAddress.Parse("8.8.8.8");

            var queryResult = new QueryResult
            {
                Domain = domain,
                Dns = dnsServer.ToString(),
                NsCount = -1
            };

            Response response = null;
            try
            {
                // create a DNS request
                var request = new Request();
                request.AddQuestion(new Question(domain, DnsType.NS, DnsClass.IN));

                response = Resolver.Lookup(request, dnsServer);
            }
            catch (Exception ex)
            {
                queryResult.Error = ex.Message;
            }

            if (response == null)
            {
                queryResult.Info = "No answer";
                return queryResult;
            }

            queryResult.Info = response.AuthoritativeAnswer ? "authoritative answer" : "Non-authoritative answer";

            // queryResult.NSCount = response.Answers.Length + response.AdditionalRecords.Length + response.NameServers.Length;

            foreach (var answer in response.Answers.Where(answer => answer.Record != null))
            {
                queryResult.NsList.Add(answer.Record.ToString());
            }

            foreach (var additionalRecord in response.AdditionalRecords.Where(additionalRecord => additionalRecord.Record != null))
            {
                queryResult.NsList.Add(additionalRecord.Record.ToString());
            }

            foreach (var nameServer in response.NameServers.Where(nameServer => nameServer.Record != null))
            {
                queryResult.NsList.Add(nameServer.Record.ToString());
            }

            queryResult.NsCount = queryResult.NsList.Count;

            return queryResult;
        }

        /// <summary>
        /// save file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public static void Save(string fileName, string content)
        {
            var path = fileName.ToFullPath();
            using (var sw = new StreamWriter(path, false))
            {
                sw.Write(content);
                sw.Flush();
            }
        }

        private static bool IsValid(string fileName)
        {
            var dt = File.GetLastWriteTime(fileName);
            var updater = new ListUpdater(fileName);

            return dt.ToString("yyyy-MM-dd").Equals(DateTime.Now.ToString("yyyy-MM-dd")) && updater.Validate() == 1;
        }

        private static string TrimEasyList()
        {
            var sBuilder = new StringBuilder();
            using (var sr = new StreamReader(Configs.Easylist, Encoding.UTF8))
            {
                var easyListContent = sr.ReadToEnd();
                var lists = Regex.Split(easyListContent, @"! \*\*\* ");

                foreach (var t in from list in lists select list.Trim() into t where IsEasyListItemOn(t) select t.TrimEnd(new[] { '\r', '\n' }))
                {
                    sBuilder.AppendLine("! *** " + t);
                }
            }

            return sBuilder.Replace("\r", string.Empty).ToString().TrimEnd(new[] { '\r', '\n' });
        }

        private static string TrimEasyPrivacy()
        {
            var sBuilder = new StringBuilder();
            using (var sr = new StreamReader(Configs.Easyprivacy, Encoding.UTF8))
            {
                var easyPrivacyContent = sr.ReadToEnd();

                var lists = Regex.Split(easyPrivacyContent, @"! \*\*\* ");

                var titleRegx = new Regex(@"^([\s\S]*?)(\*){3}", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                var contentRegx = new Regex(@"\! Chinese[\s\S]*?\!", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                foreach (var t in lists.Select(list => list.Trim()).Where(IsEasyPrivacyOff))
                {
                    if (t.Contains("_international.txt"))
                    {
                        var match = contentRegx.Match(t);
                        if (!match.Success)
                            continue;
                        var title = titleRegx.Match(t).Groups[0].Value;
                        var content = contentRegx.Match(t).Groups[0].Value.TrimEnd(new[] { '\r', '\n', '!' });
                        sBuilder.AppendFormat("! *** {0}\n {1}\n", title, content);
                    }
                    else
                    {
                        sBuilder.AppendLine("!*** " + t);
                    }
                }
            }

            return sBuilder.Replace("\r", string.Empty).ToString();
        }

        private static string ParseNameServer(string ns)
        {
            var temp = ns.Split('=')[1].Trim();
            temp = temp.Split('\n')[0].Trim();

            return temp;
        }

        private static PatchConfig GetConfigurations()
        {
            var file = Configs.PatchFile;
            if (!File.Exists(file)) return null;

            using (var sr = new StreamReader(file, Encoding.UTF8))
            {
                var xml = sr.ReadToEnd();
                return SimpleSerializer.XmlDeserialize<PatchConfig>(xml);
            }
        }

        private static bool IsEasyListItemOn(string value)
        {
            var easyList = Configs.EasyListFlag;
            return easyList.Count == 0 || easyList.Any(value.Contains);
        }

        private static bool IsEasyPrivacyOff(string value)
        {
            var easyPrivacy = Configs.EasyPrivacyFlag;
            return easyPrivacy.Count == 0 || easyPrivacy.Any(value.Contains);
        }

        private static bool DownloadEasyList(WebProxy proxy)
        {
            var folder = Configs.EasyListPath;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            using (var webClient = new WebClient())
            {
                if (proxy != null)
                {
                    webClient.Proxy = proxy;
                    Console.WriteLine("use proxy: {0}", proxy.Address.Authority);
                }

                var lists = new Dictionary<string, string>
                {
                    {Configs.Easylist, Configs.EasylistUrl},
                    {Configs.Easyprivacy, Configs.EasyprivacyUrl}
                };

                foreach (var s in lists)
                {
                    var name = Path.GetFileName(s.Key);

                    if (IsValid(s.Key))
                    {
                        Console.WriteLine("{0} is the latest, skip over downloading.", name);
                    }
                    else
                    {
                        Console.WriteLine("{0} is out of date, to start the update.", name);
                        webClient.DownloadFile(s.Value, s.Key);
                        Console.WriteLine("update {0} completed.", s.Key);
                        var updater = new ListUpdater(s.Key);
                        if (updater.Validate() == 1) continue;
                        Console.WriteLine("Download {0} error, pls try later.", s.Key);
                        return false;
                    }
                }
            }

            return true;
        }

        public static void CombineChinaList(string output = "../adblock.txt")
        {
            if (string.IsNullOrEmpty(output))
            {
                Console.WriteLine("Merget output file is empty.");
                return;
            }

            var sBuilder = new StringBuilder(Configs.ChinaListHeader);
            var files = Configs.ChinaList;
            foreach (var file in files)
            {
                Console.WriteLine("fetch data from {0}", Path.GetFileName(file));
                using (var sr = new StreamReader(file, Encoding.UTF8))
                {
                    var content = sr.ReadToEnd();
                    //remove header
                    var headerRegex = new Regex(@"\[Adblock Plus [\s\S]*?NO WARRANTY but Best Wishes----",
                        RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    content = headerRegex.Replace(content, string.Empty);
                    sBuilder.AppendFormat("!*** {0} ***\n", Path.GetFileName(file));
                    sBuilder.AppendLine(content);
                }
            }

            sBuilder.AppendLine(ListEndMark);

            Save(output, sBuilder.ToString());

            var updater = new ListUpdater(output);
            updater.Update();
            updater.Validate();

            Console.WriteLine("End of combine ChinaList and validate.");
        }

        public static bool CombineList(string input, string output)
        {
            var path = input.ToFullPath();
            var header = Configs.Header(Path.GetFileName(path));
            if (string.IsNullOrEmpty(header)) return false;
            var content = File.ReadAllText(path);
            var sb = new StringBuilder(header);
            sb.AppendLine(content);
            sb.AppendLine(ListEndMark);
            var target = output.ToFullPath();
            Save(target, sb.ToString());
            var updater = new ListUpdater(target);
            updater.Update();
            return updater.Validate() == 1;
        }
    }
}
