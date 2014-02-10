using System;
using System.Collections.Generic;
using System.Linq;
using ABPUtils.Properties;

namespace ABPUtils
{
    public class Configurations
    {
        private const string EasyListFolder = "easylist/";

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

            _chinaListPath = userSettings["ChinaList"].ToString().ToFullPath();
            _chinaListPrivacyPath = userSettings["ChinaListPrivacy"].ToString().ToFullPath();
            _chinaListAntiSocialPath = userSettings["ChinaListAntiSocial"].ToString().ToFullPath();
            _patchPath = userSettings["PatchFile"].ToString().ToFullPath();

            _easyListUrl = userSettings["EasylistUrl"].ToString();
            var index = _easyListUrl.LastIndexOf("/", StringComparison.Ordinal) + 1;
            _easyListName = EasyListFolder.ToFullPath(_easyListUrl.Substring(index));

            _easyPrivacyUrl = userSettings["EasyprivacyUrl"].ToString();
            index = _easyPrivacyUrl.LastIndexOf("/", StringComparison.Ordinal) + 1;
            _easyPrivacyName = EasyListFolder.ToFullPath(_easyPrivacyUrl.Substring(index));

            _easyListFlag = userSettings["EasyListFlag"].ToString().Split('\n').Where(s => !string.IsNullOrEmpty(s.Trim())).ToList();
            _easyPrivacyFlag = userSettings["EasyPrivacyFlag"].ToString().Split('\n').Where(s => !string.IsNullOrEmpty(s.Trim())).ToList();
            _lazyList = userSettings["ChinaListLazy"].ToString().Split(',').Where(s => !string.IsNullOrEmpty(s.Trim())).ToList();
            _helpInfo = userSettings["HelpInfo"].ToString();
        }

        /// <summary>
        /// The minimum ABP version supported 
        /// </summary>
        public string Version
        {
            get
            {
                return _version;
            }
        }

        /// <summary>
        /// ABP header info for ChinaList
        /// </summary>
        public string ChinaListHeader
        {
            get
            {
                return _chinaListHeader;
            }
        }

        /// <summary>
        /// ABP header info for ChinaList Lazy
        /// </summary>
        public string ChinaListLazyHeader
        {
            get
            {
                return _lazyHeader;
            }
        }

        /// <summary>
        /// ABP header info for ChinaList AntiSocial
        /// </summary>
        public string ChinaListAntiSocialHeader
        {
            get
            {
                return _antiSocialHeader;
            }
        }

        /// <summary>
        /// ABP header info for ChinaList Privacy
        /// </summary>
        public string ChinaListPrivacyHeader
        {
            get
            {
                return _privacyHeader;
            }
        }

        /// <summary>
        /// Path of ChinaList
        /// </summary>
        public string ChinaList
        {
            get
            {
                return _chinaListPath;
            }
        }

        /// <summary>
        /// Path of ChinaList Privacy
        /// </summary>
        public string ChinaListPrivacy
        {
            get
            {
                return _chinaListPrivacyPath;
            }
        }

        /// <summary>
        /// Path of ChinaList Anti Social
        /// </summary>
        public string ChinaListAntiSocial
        {
            get
            {
                return _chinaListAntiSocialPath;
            }
        }

        /// <summary>
        /// Path of the patch file
        /// </summary>
        public string PatchFile
        {
            get
            {
                return _patchPath;
            }
        }

        /// <summary>
        /// Path of EasyList (abs full path)
        /// </summary>
        public string Easylist
        {
            get
            {
                return _easyListName;
            }
        }

        /// <summary>
        /// Url of EasyList
        /// </summary>
        public string EasylistUrl
        {
            get
            {
                return _easyListUrl;
            }
        }

        /// <summary>
        /// Path of Easyprivacy (abs full path)
        /// </summary>
        public string Easyprivacy
        {
            get
            {
                return _easyPrivacyName;
            }
        }

        /// <summary>
        /// Url of EasyprivacyUrl
        /// </summary>
        public string EasyprivacyUrl
        {
            get
            {
                return _easyPrivacyUrl;
            }
        }

        /// <summary>
        /// Flag of EasyList (To be mergered into ChinaList Lazy)
        /// </summary>
        public List<string> EasyListFlag
        {
            get
            {
                return _easyListFlag;
            }
        }

        /// <summary>
        /// Flag of EasyPrivacy (To be mergered into ChinaList Lazy)
        /// </summary>
        public List<string> EasyPrivacyFlag
        {
            get
            {
                return _easyPrivacyFlag;
            }
        }

        /// <summary>
        /// Flag of ChinaList Lazy (To be mergered into ChinaList Lazy)
        /// </summary>
        public List<string> ChinaListLazy
        {
            get
            {
                return _lazyList.Select(GetPropValue).Where(value => !string.IsNullOrEmpty(value)).ToList();
            }
        }

        /// <summary>
        /// ABPUtils help info
        /// </summary>
        public string HelpInfo
        {
            get
            {
                return _helpInfo;
            }
        }

        public string EasyListPath
        {
            get
            {
                return EasyListFolder.ToFullPath();
            }
        }

        /// <summary>
        /// Get ABP Header by file name
        /// TODO: reflection
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
