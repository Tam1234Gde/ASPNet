var WIS_B_Notification;
(function (WIS_B_Notification) {
    var cBlock = "wis-jscss-notification";
    var cBlock_Item = "wis-jscss-notification-elem";
    var cBlock_ItemSuccess = "wis-jscss-notification-elem__success";
    var cBlock_ItemWarning = "wis-jscss-notification-elem__warning";
    var cBlock_ItemError = "wis-jscss-notification-elem__error";
    var cBlock_Item_Text = "wis-jscss-notification-elem-text";
    var cBlock_Item_Close = "wis-jscss-notification-elem-close";
    var nTime = 5000;
    var eType;
    (function (eType) {
        eType[eType["Success"] = 1] = "Success";
        eType[eType["Warning"] = 2] = "Warning";
        eType[eType["Error"] = 3] = "Error";
    })(eType = WIS_B_Notification.eType || (WIS_B_Notification.eType = {}));
    function Show(data) {
        var elem_class = "".concat(cBlock_Item, " ").concat(cBlock_ItemSuccess);
        switch (data.Type) {
            case eType.Warning:
                elem_class = "".concat(cBlock_Item, " ").concat(cBlock_ItemWarning);
                break;
            case eType.Error:
                elem_class = "".concat(cBlock_Item, " ").concat(cBlock_ItemError);
                break;
            default:
                break;
        }
        var elem_bar = $(".".concat(cBlock));
        if (elem_bar.length == 0) {
            elem_bar = $('<div>').addClass(cBlock);
            $("body").prepend(elem_bar);
        }
        elem_bar.show();
        var elem = $('<div>').addClass(elem_class).html("<div class=\"".concat(cBlock_Item_Text, "\">").concat(data.Value_html, "</div>"));
        var elem_close = $('<button>').addClass(cBlock_Item_Close);
        elem_close.click(function () {
            elem.hide();
        });
        elem.append(elem_close);
        elem_bar.append(elem);
        elem.slideDown("show");
        setTimeout(function () {
            elem_bar.find(elem).slideUp("slow", function () {
                $(this).remove();
                if (elem_bar.find(".".concat(cBlock_Item)).length == 0) {
                    elem_bar.hide();
                }
            });
        }, nTime);
    }
    WIS_B_Notification.Show = Show;
})(WIS_B_Notification || (WIS_B_Notification = {}));
//# sourceMappingURL=WIS_B_Notification.js.map