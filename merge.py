#! /usr/bin/env python
#coding=utf-8
#version=1.0

#############################################################################
# This is a reference script to add checksums to downloadable               #
# subscriptions. The checksum will be validated by Adblock Plus on download #
# and checksum mismatches (broken downloads) will be rejected.              #
#                                                                           #
# To add a checksum to a subscription file, run the script like this:       #
#                                                                           #
#   python merge.py adblock.txt adblock-lazy.txt                      		#
#   or  python merge.py adblock-lazy.txt                      				#
#                                                                           #
# Note: your subscription file should be saved in UTF-8 encoding, otherwise #
# the generated checksum might be incorrect.                                #
#                                                                           #
# This script is wrote by Gythialy for ChinaList Project                    #
#############################################################################

import os
import time
import codecs
import hashlib
import base64
import re
import string
import sys
from urllib import urlretrieve

EASYLIST = 'easylist.txt'
EASYLISTURL = 'https://easylist-downloads.adblockplus.org/easylist.txt'
EASYLISTHEAD  = '\n! EasyList - https://easylist.adblockplus.org/\n\n'
EASYLISTMARK = '!-----------------General advert blocking filters-----------------!'
EASYPRIVACY = 'easyprivacy.txt'
EASYPRIVACYURL = 'https://easylist-downloads.adblockplus.org/easyprivacy.txt'
EASYPRIVACYHEAD = '\n! EasyPrivacy - https://easylist.adblockplus.org/\n\n'
EASYPRIVACYMARK = '!-----------------General tracking systems-----------------!'

def download(url,filename):
    urlretrieve(url,filename)

def fileexist(filename):
    path = os.path.join(os.getcwd(),filename)
    if os.path.exists(path):
        fileinfo = os.stat(path)
        current =  time.strftime('%Y%m%d',time.localtime(time.time()))
        filetime = time.strftime('%Y%m%d',time.localtime(fileinfo.st_mtime))
        if current == filetime:
            return {"success":True,"reason":'well done'}
        else:
            return {"success":False,"reason":'file is too old'}
    else:
        return {"success":False,"reason":'file is not exist'}

def updatelist(url,filename):
    result = fileexist(filename)
    if not result["success"]:
        print '{0} , start update {1}'.format(result["reason"],filename)
        download(url,filename)
        print 'End of update ' + filename

def read(filename):
    path = os.path.join(os.getcwd(),filename)
    f =  codecs.open(filename,'rt',encoding = 'utf-8')
    data = f.read()
    f.close()

    return data

def save(content,filename):
    path = os.path.join(os.getcwd(),filename)
    f =  codecs.open(filename,'w',encoding = 'utf-8')
    f.write(content)
    f.close()

def merge(chinalazy,chinalist = 'adblock.txt'):
    chinalazycontent = ''

    updatelist(EASYLISTURL,EASYLIST)
    updatelist(EASYPRIVACYURL,EASYPRIVACY)

    chinalazycontent = read(chinalist)
    easylistcontent = read(EASYLIST)
    index = string.index(easylistcontent,EASYLISTMARK)
    easylistcontent = EASYLISTHEAD + easylistcontent[index:]
    #print easylistcontent
    easyprivacycontent = read(EASYPRIVACY)
    index= string.index(easyprivacycontent,EASYPRIVACYMARK)
    easyprivacycontent = EASYPRIVACYHEAD + easyprivacycontent[index:]
    #print easyprivacycontent

    chinalazycontent += (easylistcontent + easyprivacycontent)
    save(chinalazycontent,chinalazy)

if __name__ == '__main__':
    reload(sys)
    sys.setdefaultencoding( "utf-8" )
    chinalist = ''
    chinalazy = ''

    if len(sys.argv) == 2:
        chinalazy = sys.argv[1]
        merge(chinalazy)
    elif len(sys.argv) == 3:
        chinalist = sys.argv[1]
        chinalazy = sys.argv[2]
        merge(chinalazy,chinalazy)
    else:
        print 'Pls input file names.'
        sys.exit(0)