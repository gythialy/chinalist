using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ABPUtils.Properties;

namespace ABPUtils
{
    public class Configurations
    {
        private const string EasyListFolder = "easylist";

        private readonly string _version;
        private readonly string _chinaListHeader;
        private readonly string _lazyHeader;
        private readonly string _antiSocialHeader;
        private readonly string _privacyHeader;
        private readonly string _chinaListPath;
        private readonly string _chinaListPrivacyPath;
        private readonly string _chinaListAntiSocialPath;
        private readonly string _patchPath;
        private readonly string _easyListUrl;
        private readonly string _easyPrivacyUrl;
        private readonly List<string> _easyListFlag;
        private readonly List<string> _easyPrivacyFlag;
        private readonly List<string> _lazyList;
        private readonly string _helpInfo;
        private readonly string _easyListName;
        private readonly string _easyPrivacyName;

        private static Configurations _one;
        public static Configurations Default
        {
            get { return _one ?? (_one = new Configurations()); }
        }

        private Configurations()
        {
            var userSettings = Settings.Default;
            _version = userSettings["ABPVersion"].ToString();
            _chinaListHeader = string.Format(userSettings["ChinaListHeader"].ToString(), _version);
            _lazyHeader = string.Format(userSettings["ChinaListLazyHeader"].ToString(), _version);
            _antiSocialHeader = string.Format(userSettings["ChinaListAntiSocialHeader"].ToString(), _version);
            _privacyHeader = string.Format(userSettings["ChinaListPrivacyHeader"].ToString(), _version);

            _chinaListPath = Path.GetFullPath(Path.Combine(RunTime, userSettings["ChinaList"].ToString()));
            _chinaListPrivacyPath = Path.GetFullPath(Path.Combine(RunTime, userSettings["ChinaListPrivacy"].ToString()));
            _chinaListAntiSocialPath = Path.GetFullPath(Path.Combine(RunTime, userSettings["ChinaListAntiSocial"].ToString()));
            _patchPath = Path.GetFullPath(Path.Combine(RunTime, userSettings["PatchFile"].ToString()));

            _easyListUrl = userSettings["EasylistUrl"].ToString();
            var index = _easyListUrl.LastIndexOf("/", StringComparison.Ordinal) + 1;
            _easyListName = Path.GetFullPath(Path.Combine(RunTime, EasyListFolder, _easyListUrl.Substring(index)));

            _easyPrivacyUrl = userSettings["EasyprivacyUrl"].ToString();
            index = _easyPrivacyUrl.LastIndexOf("/", StringComparison.Ordinal) + 1;
            _easyPrivacyName = Path.GetFullPath(Path.Combine(RunTime, EasyListFolder, _easyPrivacyUrl.Substring(index)));

            _easyListFlag = userSettings["EasyListFlag"].ToString().Split('\n').Where(s => !string.IsNullOrEmpty(s.Trim())).ToList();
            _easyPrivacyFlag = userSettings["EasyPrivacyFlag"].ToString().Split('\n').Where(s => !string.IsNullOrEmpty(s.Trim())).ToList();
            _lazyList = userSettings["ChinaLazyList"].ToString().Split(',').Where(s => !string.IsNullOrEmpty(s.Trim())).ToList();
            _helpInfo = userSettings["HelpInfo"].ToString();
        }

        public string Version
        {
            get
            {
                return _version;
            }
        }

        public string ChinaListHeader
        {
            get
            {
                return _chinaListHeader;
            }
        }

        public string ChinaListLazyHeader
        {
            get
            {
                return _lazyHeader;
            }
        }

        public string ChinaListAntiSocialHeader
        {
            get
            {
                return _antiSocialHeader;
            }
        }

        public string ChinaListPrivacyHeader
        {
            get
            {
                return _privacyHeader;
            }
        }

        public string ChinaList
        {
            get
            {
                return _chinaListPath;
            }
        }

        public string ChinaListPrivacy
        {
            get
            {
                return _chinaListPrivacyPath;
            }
        }

        public string ChinaListAntiSocial
        {
            get
            {
                return _chinaListAntiSocialPath;
            }
        }

        public string PatchFile
        {
            get
            {
                return _patchPath;
            }
        }

        public string Easylist
        {
            get
            {
                return _easyListName;
            }
        }

        public string EasylistUrl
        {
            get
            {
                return _easyListUrl;
            }
        }

        public string Easyprivacy
        {
            get
            {
                return _easyPrivacyName;
            }
        }

        public string EasyprivacyUrl
        {
            get
            {
                return _easyPrivacyUrl;
            }
        }

        public List<string> EasyListFlag
        {
            get
            {
                return _easyListFlag;
            }
        }

        public List<string> EasyPrivacyFlag
        {
            get
            {
                return _easyPrivacyFlag;
            }
        }

        public List<string> ChinaLazyList
        {
            get
            {
                return _lazyList.Select(GetPropValue).Where(value => !string.IsNullOrEmpty(value)).ToList();
            }
        }

        public string HelpInfo
        {
            get
            {
                return _helpInfo;
            }
        }

        public string RunTime
        {
            get
            {
                return Environment.CurrentDirectory;
            }
        }

        public string ParentFolder
        {
            get
            {
                return Directory.GetParent(RunTime).FullName;
            }
        }

        public string EasyListPath
        {
            get
            {
                return Path.Combine(RunTime, EasyListFolder);
            }
        }

        public string Header(string name)
        {
            switch (name)
            {
                case "chinalist.txt":
                    return _chinaListHeader;
                case "chinalist-privacy.txt":
                    return _privacyHeader;
                case "chinalist-anti-social.txt":
                    return _antiSocialHeader;
                default:
                    return string.Empty;
            }
        }

        private string GetPropValue(string propName)
        {
            return GetType().GetProperty(propName).GetValue(this, null).ToString();
        }
    }
}
