using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ABPUtils.Properties;

namespace ABPUtils
{
    public class Configurations
    {
        private static readonly Settings UserSettings = Settings.Default;
        private const string EasyListFolder = "easylist";

        private Configurations()
        {
        }

        public static string Version()
        {
            return UserSettings["ABPVersion"].ToString();
        }

        public static string ChinaListHeader()
        {
            return UserSettings["ChinaListHeader"].ToString();
        }

        public static string ChinaListLazyHeader()
        {
            return UserSettings["ChinaListLazyHeader"].ToString();
        }

        public static string ChinaListAntiSocialHeader()
        {
            return UserSettings["ChinaListAntiSocialHeader"].ToString();
        }

        public static string ChinaListPrivacyHeader()
        {
            return UserSettings["ChinaListPrivacyHeader"].ToString();
        }

        public static string ChinaList()
        {
            return Path.Combine(RunTime(), UserSettings["ListUpdater"].ToString());
        }

        public static string ChinaListPrivacy()
        {
            return Path.Combine(RunTime(), UserSettings["ChinaListPrivacy"].ToString());
        }

        public static string ChinaListAntiSocial()
        {
            return Path.Combine(RunTime(), UserSettings["ChinaListAntiSocial"].ToString());
        }

        public static string PatchFile()
        {
            return Path.Combine(RunTime(), UserSettings["PatchFile"].ToString());
        }

        public static string Easylist()
        {
            var easyList = EasylistUrl();
            var index = easyList.LastIndexOf("/", StringComparison.Ordinal);
            return Path.Combine(RunTime(), EasyListFolder, easyList.Substring(index));
        }

        public static string EasylistUrl()
        {
            return UserSettings["EasylistUrl"].ToString();
        }

        public static string Easyprivacy()
        {
            var easyPrivacy = EasyprivacyUrl();
            var index = easyPrivacy.LastIndexOf("/", StringComparison.Ordinal);
            return Path.Combine(RunTime(), EasyListFolder, easyPrivacy.Substring(index));
        }

        public static string EasyprivacyUrl()
        {
            return UserSettings["EasyprivacyUrl"].ToString();
        }

        public static List<string> EasyListFlag()
        {
            return UserSettings["EasyListFlag"].ToString().Split('\n').Where(s => !string.IsNullOrEmpty(s.Trim())).ToList();
        }

        public static List<string> EasyPrivacyFlag()
        {
            return UserSettings["EasyPrivacyFlag"].ToString().Split('\n').Where(s => !string.IsNullOrEmpty(s.Trim())).ToList();
        }

        public static List<string> EnabledList()
        {
            return UserSettings["EnabledList"].ToString().Split(',').Where(s => !string.IsNullOrEmpty(s.Trim())).ToList();
        }

        public static string HelpInfo()
        {
            return UserSettings["HelpInfo"].ToString();
        }

        public static string RunTime()
        {
            return Environment.CurrentDirectory;
        }

        public static string ParentFolder()
        {
            return Directory.GetParent(RunTime()).FullName;
        }
    }
}
