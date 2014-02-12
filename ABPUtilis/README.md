# ABPUtilis 工具集 [![Build status](https://ci.appveyor.com/api/projects/status?id=r0jpuofg7rsnmdx4)](https://ci.appveyor.com/project/chinalist-abputilis)


## 介绍

ABPUtilis 为 C# 编写的校验和的生成工具。
主要功能:

- 生成并更新列表校验和
- 根据配置生成 ChinaList
- 校验校验和是否正确
- DNS 查询
- 检查列表中域名是否有效
- 清理 `patch.xml` 中的无效规则
- 根据 `patch.xml` 中的配置合并 EasyList，EasyPrivacy 中的规则生成 ChinaList Lazy 列表

### 参数说明
	Copyright (C) 2008 - {0} Adblock Plus ChinaList Project
	This is free software. You may redistribute copies of it under the terms of
	the GNU LGPL License <http://www.gnu.org/copyleft/lesser.html>.
	Usage: ABPUtils.exe -n -d=google.com -dns=8.8.8.8
	       ABPUtils.exe -v -i=adblock.txt
	       ABPUtils.exe -u -i=adblock.txt
	       ABPUtils.exe -m -i=adblock.txt -patch -o=adblock-lazy.txt
	       ABPUtils.exe -i=adblock.txt -conf
	
	  version        Show ABPUtils version.
	
	  c, check       Check the domains in the specific input file.
	
	  cl 			Merge as ChinaList, otherwise as ChinaList Lazy.
	
	  conf           Clean patch.xml if exist.
	
	  d, domain      The domain need to be checked (required).
	
	  m, merge       Merge the specific input file with Part of EasyList and
	                 EasyPrivacy.
	
	  b, build       Build the rules from the specific input file to a ABP subscribable list.
	
	  n, nsookup     Show the ns server of the specific domain.
	
	  u, update      Update and validate the checksum of the specific input file.
	
	  v, validate    Validate the checksum of the specific input file.
	
	  i, input       Input file with filters to process (required).
	
	  o, output      Output file with processed filters.
	
	  patch          Use the patch.xml.
	
	  p, proxy       Proxy server used when download the lastest EasyList and EasyPrivacy files (optional).
	
	  dns            DNS server (optional).
	
	  h, help        Dispaly this help screen.

## `app.config` 详解

### 参数说明

| 参数 | 说明 |
| :----------| :------------ |
| ChinaList | ChinaList 文件来源（路径），多个文件以回车分割 |
| ChinaListAntiSocial | ChinaListAntiSocial 文件来源 （路径）|
| ChinaListPrivacy |  ChinaListPrivacy 文件来源（路径） |
| **EasylistUrl** | Easylist 的获取地址 |
| **EasyprivacyUrl** | Easyprivacy 获取地址 |
| **HelpInfo** | 帮助信息 |
| PatchFile | 补丁文件路径 |
| ChinaListLazy | ChinaList 文件来源（分组/路径） |
| EasyListFlag | EasyList 标识是否合并到 ChinaList Lazy |
| **ChinaListLazyHeader** | ChinaList Lazy 中头信息|
| **ChinaListHeader** | ChinaList 中头信息 |
| **ChinaListAntiSocialHeader** | ChinaList Anti Social 中头信息 |
| **ChinaListPrivacyHeader** | ChinaList Privacy 中头信息 |
| EasyPrivacyFlag | EasyPrivacy 标识是否合并到 ChinaList Lazy |
| **ABPVersion** | 支持的 ABP 的最小版本 |

*注*：
- 路径均为 `ABPUtils.exe` 相对路径，如果填绝对路径，需要在 `ABPUtils.exe` 的同级或下级目录。
- 粗体字标注的字段尽量不要修改，除非你知道你在做什么。

### 样例

		<?xml version="1.0" encoding="utf-8" ?>
		<configuration>
		    <configSections>
		        <sectionGroup name="userSettings" type="System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" >
		            <section name="ABPUtils.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" allowExeDefinition="MachineToLocalUser" requirePermission="false" />
		        </sectionGroup>
		    </configSections>
		    <userSettings>
		        <ABPUtils.Properties.Settings>
		            <setting name="ChinaList" serializeAs="String">
		                <value>chinalist/chinalist-miscellaneous.txt
		chinalist/chinalist-adservers.txt
		chinalist/chinalist-generic.txt
		chinalist/chinalist-hide.txt
		chinalist/chinalist-specific.txt
		chinalist/chinalist-temp.txt
		chinalist/chinalist-white.txt</value>
		            </setting>
		            <setting name="ChinaListAntiSocial" serializeAs="String">
		                <value>chinalist/chinalist-anti-social.txt</value>
		            </setting>
		            <setting name="ChinaListPrivacy" serializeAs="String">
		                <value>chinalist/chinalist-privacy.txt</value>
		            </setting>
		            <setting name="EasylistUrl" serializeAs="String">
		                <value>https://easylist-downloads.adblockplus.org/easylist.txt</value>
		            </setting>
		            <setting name="EasyprivacyUrl" serializeAs="String">
		                <value>https://easylist-downloads.adblockplus.org/easyprivacy.txt</value>
		            </setting>
		            <setting name="HelpInfo" serializeAs="String">
		                <value>Copyright (C) 2008 - {0} Adblock Plus ChinaList Project
		This is free software. You may redistribute copies of it under the terms of
		the GNU LGPL License &lt;http://www.gnu.org/copyleft/lesser.html&gt;.
		Usage: ABPUtils.exe -n -d=google.com -dns=8.8.8.8
		       ABPUtils.exe -v -i=adblock.txt
		       ABPUtils.exe -u -i=adblock.txt
		       ABPUtils.exe -m -i=adblock.txt -patch -o=adblock-lazy.txt
		       ABPUtils.exe -i=adblock.txt -conf
		
		  version        Show ABPUtils version.
		
		  c, check       Check the domains in the specific input file.
		
		  cl 			 Merge as ChinaList, otherwise as ChinaList Lazy.
		
		  conf           Clean patch.xml if exist.
		
		  d, domain      The domain need to be checked (required).
		
		  m, merge       Merge the specific input file with Part of EasyList and
		                 EasyPrivacy.
		
		  b, build       Build the rules from the specific input file to a ABP subscribable list.
		
		  n, nsookup     Show the ns server of the specific domain.
		
		  u, update      Update and validate the checksum of the specific input file.
		
		  v, validate    Validate the checksum of the specific input file.
		
		  i, input       Input file with filters to process (required).
		
		  o, output      Output file with processed filters.
		
		  patch          Use the patch.xml.
		
		  p, proxy       Proxy server used when download the lastest EasyList and EasyPrivacy files (optional).
		
		  dns            DNS server (optional).
		
		  h, help        Dispaly this help screen.</value>
		            </setting>
		            <setting name="PatchFile" serializeAs="String">
		                <value>patch.xml</value>
		            </setting>
		            <setting name="ChinaListLazy" serializeAs="String">
		                <value>ChinaList,ChinaListAntiSocial,ChinaListPrivacy</value>
		            </setting>
		            <setting name="EasyListFlag" serializeAs="String">
		                <value>easylist:easylist/easylist_general_block.txt
		easylist:easylist/easylist_general_block_dimensions.txt
		easylist:easylist/easylist_general_block_popup.txt
		easylist:easylist/easylist_general_hide.txt
		easylist:easylist/easylist_adservers.txt</value>
		            </setting>
		            <setting name="ChinaListLazyHeader" serializeAs="String">
		                <value>[Adblock Plus {0}]
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
		</value>
		            </setting>
		            <setting name="ChinaListHeader" serializeAs="String">
		                <value>[Adblock Plus {0}]
		!  Title: ChinaList
		!  Adblock Plus List with Main Focus on Chinese Sites.
		!  Last Modified:  
		!  Homepage: http://chinalist.github.io/
		!
		!  As a supplement for EasyList,ChinaList won't provide the filters
		!  which in EasyList already.Please subscribe EasyList also.
		!  If you need help or have any question,
		!  please visit: https://github.com/chinalist/chinalist
		!
		!  coding: utf-8, expires: 5 days
		!----GNU LGPL Licensed, NO WARRANTY but Best Wishes----
		</value>
		            </setting>
		            <setting name="ChinaListAntiSocialHeader" serializeAs="String">
		                <value>[Adblock Plus {0}]
		!  Title: ChinaList Anti Social
		!  Adblock Plus List with Main Focus on Chinese Sites.
		!  Last Modified:  
		!  Homepage: http://chinalist.github.io/
		!
		!  This is an unofficial version, 
		!  does not provide any technical support
		!
		!  please visit: https://github.com/chinalist/chinalist
		!
		!  coding: utf-8, expires: 5 days
		!----GNU LGPL Licensed, NO WARRANTY but Best Wishes----
		</value>
		            </setting>
		            <setting name="ChinaListPrivacyHeader" serializeAs="String">
		                <value>[Adblock Plus {0}]
		!  Title: ChinaList Privacy
		!  Adblock Plus List with Main Focus on Chinese Sites.
		!  Last Modified:  
		!  Homepage: http://chinalist.github.io/
		!
		!  As a supplement for EasyPrivacy,ChinaList won't provide the filters
		!  which in EasyPrivacy already.Please subscribe EasyPrivacy also.
		!  If you need help or have any question,
		!  please visit: https://github.com/chinalist/chinalist
		!
		!  coding: utf-8, expires: 5 days
		!----GNU LGPL Licensed, NO WARRANTY but Best Wishes----
		</value>
		            </setting>
		            <setting name="EasyPrivacyFlag" serializeAs="String">
		                <value>easylist:easyprivacy/easyprivacy_general.txt
		easylist:easyprivacy/easyprivacy_trackingservers_international.txt
		easylist:easyprivacy/easyprivacy_thirdparty_international.txt
		easylist:easyprivacy/easyprivacy_specific_international.txt
		easylist:easyprivacy/easyprivacy_whitelist_international.txt</value>
		            </setting>
		            <setting name="ABPVersion" serializeAs="String">
		                <value>2.0</value>
		            </setting>
		        </ABPUtils.Properties.Settings>
		    </userSettings>
		</configuration>

## `Check` 结果详解

### 结果示例

    Domain:	bmgad.com
    DNS:	8.8.8.8
    Info:	Non-authoritative answer
    Count:	1
    NS => primary name server = 
    responsible mail addr = hostmaster.nameserver
    serial  = 65
    refresh = 28800
    retry   = 7200
    expire  = 604800
    default TTL = 5
    Error:	NONE.
    [V]: ns->responsible mail addr, Error->No such host is known
    [V]: validate domian fail.
    -------------------------------

### `Check` 结果参数说明

| 参数 | 说明 | 
| ---- | :----- |
| Domain | 校验的域名 |
| DNS | 查询域名时用的DNS服务 | 
| Info | Non-authoritative answer: 结果从缓存中获得；authoritative answer: 结果服务器中取得 |
| Count | 查询到的结果个数 |
| NS | 查询的name server集合 |
| Error | NONE: 查询 name server 时无异常 |
| [V] | 验证 name server 可用性时的异常信息，```[V]: validate domian fail.查询到的name server都不可用``` |
