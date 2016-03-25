// ==UserScript==
// @name inoreader ads Remover
// @namespace https://github.com/gythialy/chinalist/
// @author Gythialy
// @version 1.0.0
// @description Remove Inoreader ads for ChinaList
// @homepage https://github.com/gythialy/chinalist/
// @updateURL https://github.com/gythialy/chinalist/raw/master/scripts/remove_ads_for_inoreader.user.js
// @include http*://www.inoreader.com/*
// @grant none
// ==/UserScript==
(function() {
  $('#sinner_container').css('display', 'none');
  $('#reader_pane.reader_pane_sinner').css('padding-right', '0px');
})();
