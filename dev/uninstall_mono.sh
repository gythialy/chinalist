#!/bin/sh -x

#This script removes Mono from an OS X System.  It must be run as root

rm -r /Library/Frameworks/Mono.framework

rm -r /Library/Receipts/MonoFramework-*

for dir in /usr/bin /usr/share/man/man1 /usr/share/man/man3 /usr/share/man/man5; do
   (cd ${dir};
    for i in `ls -al | grep /Library/Frameworks/Mono.framework/ | awk '{print $9}'`; do
      rm ${i}
    done);
done