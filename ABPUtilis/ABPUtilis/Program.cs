using System;
using System.Net;

namespace ABPUtils
{
    class Program
    {
        static void Main(string[] args)
        {
            if (null == args || args.Length == 0)
            {
                Console.WriteLine("wrong input argument.");
                return;
            }

            var arguments = new Arguments(args);
            DispatcherTask(arguments);
#if DEBUG
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
#endif
        }

        private static void DispatcherTask(Arguments args)
        {
            var configurations = Configurations.Default;

            if (args.IsTrue("help") || args.IsTrue("h"))
            {
                Console.WriteLine(configurations.HelpInfo, DateTime.Now.ToString("yyyy"));
            }
            else if (args.IsTrue("version"))
            {
                Console.WriteLine("ABPUtils version: {0}", ChinaLists.GetVersion());
            }
            else if (args.IsTrue("ns") || args.IsTrue("nslookup"))
            {
                var domain = args.Single("d");

                if (string.IsNullOrEmpty(domain))
                    domain = args.Single("domain");

                if (string.IsNullOrEmpty(domain))
                {
                    Console.WriteLine("wrong input domain.");
                    return;
                }

                var result = ChinaLists.DnsQuery(string.IsNullOrEmpty(args.Single("dns")) ? null : IPAddress.Parse(args.Single("dns")), domain);

                if (result == null)
                {
                    Console.WriteLine("Query result is null.");
                }
                else
                {
                    Console.Write(result.ToString());
                }
            }
            else if (args.IsTrue("v") || args.IsTrue("validate"))
            {
                var input = args.Single("i");
                if (string.IsNullOrEmpty(input))
                    input = args.Single("input");

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("wrong input file.");
                    return;
                }

                var updater = new ListUpdater(input);
                updater.Validate();
            }
            else if (args.IsTrue("u") || args.IsTrue("update"))
            {
                var input = args.Single("i");
                if (string.IsNullOrEmpty(input))
                    input = args.Single("input");

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("wrong input file.");
                    return;
                }

                var updater = new ListUpdater(input);
                updater.Update();
                updater.Validate();
            }
            else if (args.IsTrue("c") || args.IsTrue("check"))
            {
                var input = args.Single("i");
                if (string.IsNullOrEmpty(input))
                    input = args.Single("input");

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("wrong input file.");
                    return;
                }

                var output = args.Single("o");

                if (string.IsNullOrEmpty(output))
                    output = args.Single("output");

                var dns = args.Single("dns");
                ChinaLists.ValidateDomains(string.IsNullOrEmpty(dns) ? null : IPAddress.Parse(args.Single("dns")), input,
                    output);
            }
            else if (args.IsTrue("b") || args.IsTrue("build"))
            {
                var input = args.Single("i");

                if (string.IsNullOrEmpty(input))
                    input = args.Single("input");

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("wrong input file.");
                    return;
                }

                var output = args.Single("o");
                if (string.IsNullOrEmpty(output))
                    output = args.Single("output");

                ChinaLists.CombineList(input, output);
            }
            else if (args.IsTrue("m") || args.IsTrue("merge"))
            {
                var input = args.Single("i");

                if (string.IsNullOrEmpty(input))
                    input = args.Single("input");

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("wrong input file.");
                    return;
                }

                WebProxy proxy = null;
                var p = args.Single("p");
                if (string.IsNullOrEmpty(p))
                    p = args.Single("proxy");

                if (!string.IsNullOrEmpty(p))
                {
                    var temp = p.Split(':');
                    proxy = new WebProxy(temp[0], int.Parse(temp[1])) {BypassProxyOnLocal = true};
                }

                var output = args.Single("o");
                if (string.IsNullOrEmpty(output))
                    output = args.Single("output");

                ChinaLists.CombineLazyList(input, proxy, args.IsTrue("patch"), output);
            }
            else if (args.IsTrue("conf"))
            {
                var input = args.Single("i");

                if (string.IsNullOrEmpty(input))
                    input = args.Single("input");

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("wrong input file.");
                    return;
                }

                ChinaLists.CleanConfigurations(input, null);
                Console.WriteLine("Clean configuration file successful.");
            }
            else
            {
                Console.WriteLine("Wrong args");
            }
        }
    }
}
