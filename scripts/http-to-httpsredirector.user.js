// ==UserScript==
// @name HTTP-to-HTTPS redirector
// @namespace https://code.google.com/p/adblock-chinalist/
// @author Gythialy
// @version 1.0.0
// @description Replace http:// with https:// in the address bar(only for Scriptish)
// @homepage https://bitbucket.org/gythialy/scripts
// @include	http://mail.qq.com/*
// @include http://www.google.com/reader/*
// @include http://www.twimbow.com/*
// @include http://code.google.com/*
// @include http://groups.google.com/*
// @exclude	https://*
// @run-at document-start
// ==/UserScript==
(function() {
	var DEBUG = 0;
	function log(message) {
		if (DEBUG && GM_log) {
			GM_log(message);
		}
	}

	log("Hash: " + location.hash + "\nHost: " + location.host + "\nHostname: " + location.hostname + "\nHREF " + location.href + "\nPathname: " + location.pathname + "\nPort:  " + location.port + "\nProtocol: " + location.protocol + "\n" + "\nNew Location: "
			+ location.href.replace(/http\:/, 'https:'));
	
	location.href = location.href.replace(/http\:/, 'https:');
})();