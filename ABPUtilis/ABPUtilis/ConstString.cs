
namespace ABPUtils
{
    internal class ChinaListConst
    {
        public const string PatchFile = "patch.xml";
        public const string Easylist = "easylist.txt";
        public const string ChinalistLazyHeader = @"[Adblock Plus 2.1]
!  Adblock Plus List with Main Focus on Chinese Sites.
!  Last Modified:  
!  Homepage: http://chinalist.github.io/
!
!  ChinaList Lazy = Part of EasyList + ChinaList + Part of EasyPrivacy
!  If you need to know the details,
!  please visit: https://github.com/chinalist/chinalist/wiki/something_about_ChinaList_Lazy
!
!  If you need help or have any question,
!  please visit: https://github.com/chinalist/chinalist
!
!  coding: utf-8, expires: 5 days
!--CC-BY-SA 3.0 + Licensed, NO WARRANTY but Best Wishes----
";
        public const string HelpInfo = @"Copyright (C) 2008 - {0} Adblock Plus ChinaList Project
This is free software. You may redistribute copies of it under the terms of
the GNU LGPL License <http://www.gnu.org/copyleft/lesser.html>.
Usage: ABPUtils.exe -n -d=google.com -dns=8.8.8.8
       ABPUtils.exe -v -i=adblock.txt
       ABPUtils.exe -u -i=adblock.txt
       ABPUtils.exe -m -i=adblock.txt -patch -o=adblock-lazy.txt
       ABPUtils.exe -i=adblock.txt -conf

  version        Show ABPUtils version.

  c, check       Check the domains in the specific input file.

  conf           Clean patch.xml if exist.

  d, domain      The domain need to be checked (required).

  m, merge       Merge the specific input file with Part of EasyList and
                 EasyPrivacy.

  n, nsookup     Show the ns server of the specific domain.

  u, update      Update and validate the checksum of the specific input file.

  v, validate    Validate the checksum of the specific input file.

  i, input       Input file with filters to process (required).

  o, output      Output file with processed filters.

  patch          Use the patch.xml.

  p, proxy       Proxy server used when download the lastest EasyList and EasyPrivacy files (optional).

  dns            DNS server (optional).

  h, help        Dispaly this help screen.";

        public const string EasylistUrl = "https://easylist-downloads.adblockplus.org/easylist.txt";
        public const string Easyprivacy = "easyprivacy.txt";
        public const string EasyprivacyUrl = "https://easylist-downloads.adblockplus.org/easyprivacy.txt";
        public const string ChinalistLazyHeaderMark = "!----------------------------White List--------------------";
        public const string ChinalistEndMark = "!------------------------End of List-------------------------";
    }
}
