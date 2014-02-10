@echo on
Rem Combine ChinaList
ABPUtils.exe -b -i=chinalist/chinalist.txt -o=../adblock.txt
Rem Combine ChinaList Privacy
ABPUtils.exe -b -i=chinalist/chinalist.txt -o=../adblock-privacy.txt
Rem Combine ChinaList Anti Social
ABPUtils.exe -b -i=chinalist/chinalist.txt -o=../adblock-anti-social.txt
Rem Combine ChinaList Lazy
ABPUtils.exe -m -i=chinalist/ -o=../adblock-lazy.txt -patch
@pause