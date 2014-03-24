#! /bin/bash

echo "Combine ChinaList"
mono ABPUtils.exe -m -o=../chinalist-lite -cl
echo "Combine ChinaList Privacy"
mono ABPUtils.exe -b -i=chinalist/chinalist-privacy.txt -o=../chinalist-privacy.txt
echo "Combine ChinaList Anti Social"
mono ABPUtils.exe -b -i=chinalist/chinalist-anti-social.txt -o=../chinalist-anti-social.txt
echo "Combine ChinaList Lazy"
mono ABPUtils.exe -m -i=chinalist/ -o=../chinalist.txt -patch

