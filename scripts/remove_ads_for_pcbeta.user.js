// ==UserScript==
// @name pcbeta ads Remover
// @namespace https://github.com/gythialy/chinalist/
// @author Gythialy
// @version 1.0.5
// @description Remove bbs.pcbeta.com ads for ChinaList
// @homepage https://github.com/gythialy/chinalist/
// @updateURL https://github.com/gythialy/chinalist/raw/master/scripts/remove_ads_for_pcbeta.user.js
// @include http://bbs.pcbeta.com/*
// @grant GM_log
// ==/UserScript==
(function() {
    var DEBUG = 0;

    function log(message) {
        if (DEBUG && GM_log) {
            GM_log(message);
        }
    }

    function x(xpath, parent, type, result) {
        return document.evaluate(xpath, parent || document, null, type || XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, result);
    }

    function remove(elm) {
        if (elm.snapshotItem) {
            for (var i = 0; i < elm.snapshotLength; i++) {
                remove(elm.snapshotItem(i));
            }
        } else if (elm[0]) {
            while (elm[0]) {
                remove(elm[0]);
            }
        } else {
            elm.parentNode.removeChild(elm);
        }
    }

    var t = x(".//*[@id='wp']/div[2]");
    for (var i = 0; i < t.snapshotLength; i++) {
        var node = t.snapshotItem(i);
        log(node.style.height);
        if (node) {
            node.style.height = 'inherit';
            break;
        }
    }

    // remove(x(".//*[@id='wp']/div[2]/div"));
    remove(x(".//*[@id='wp']/div[contains(@style, 'background:#d0dae5;')]/div[contains(@style, 'clear:both;')]"));
    remove(x("//*[@class='asdad']/following-sibling::div"));
    remove(x("//*[@id='diynavtop']/following-sibling::style"));
    remove(x('.//div[@id="sitefocus"][@class="focus"]'));
})();