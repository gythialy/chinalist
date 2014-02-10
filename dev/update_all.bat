@echo on
Rem Combine ChinaList
ABPUtils.exe -m -o=../adblock.txt -cl
Rem Combine ChinaList Privacy
ABPUtils.exe -b -i=chinalist/chinalist-privacy.txt -o=../adblock-privacy.txt
Rem Combine ChinaList Anti Social
ABPUtils.exe -b -i=chinalist/chinalist-anti-social.txt -o=../adblock-anti-social.txt
Rem Combine ChinaList Lazy
ABPUtils.exe -m -i=chinalist/ -o=../adblock-lazy.txt -patch
@pause