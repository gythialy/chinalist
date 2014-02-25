#! /bin/bash

echo "Combine ChinaList"
mono ABPUtils.exe -m -o=../adblock.txt -cl
echo "Combine ChinaList Privacy"
mono ABPUtils.exe -b -i=chinalist/chinalist-privacy.txt -o=../adblock-privacy.txt
echo "Combine ChinaList Anti Social"
mono ABPUtils.exe -b -i=chinalist/chinalist-anti-social.txt -o=../adblock-anti-social.txt
echo "Combine ChinaList Lazy"
mono ABPUtils.exe -m -i=chinalist/ -o=../adblock-lazy.txt -patch
read -n1 -p "Press any key to continue...\n"
