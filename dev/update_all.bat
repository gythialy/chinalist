@echo on
Rem Combine ChinaList
ABPUtils.exe -m -o=../chinalist-lite.txt -cl
Rem Combine ChinaList Privacy
ABPUtils.exe -b -i=chinalist/chinalist-privacy.txt -o=../chinalist-privacy.txt
Rem Combine ChinaList Anti Social
ABPUtils.exe -b -i=chinalist/chinalist-anti-social.txt -o=../chinalist-anti-social.txt
Rem Combine ChinaList Lazy
ABPUtils.exe -m -i=chinalist/ -o=../chinalist.txt -patch
@pause