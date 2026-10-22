var WIS_M_MenuMobile;
(function (WIS_M_MenuMobile) {
    var $Elem = $(".wis-js-menu_mobile");
    if ($Elem != undefined && $Elem.length > 0) {
        var $Menu_Button = $(".wis-js-menu_mobile-open");
        var $Menu_Button_Close = $(".wis-menu_mobile-box-title-close");
        var $Menu_Mobile_1 = $(".wis-js-menu_mobile-box");
        var cMenu_Mobile_Active_1 = "wis-jscss-menu_mobile-box__active";
        var $Bg_1 = $(".wis-js-menu_mobile-bg");
        var cBg_Active_1 = "wis-jscss-menu_mobile-bg__active";
        var $Mobile_Content_Buttons = $Menu_Mobile_1.find(".wis-js-menu_mobile-box-nav-elem-button");
        var cMobile_Content_Button_Active = "wis-jscss-menu_mobile-box-nav-elem-button__open";
        var $Mobile_Content_Buttons_Child = $Menu_Mobile_1.find(".wis-js-menu_mobile-box-nav-elem-child-button");
        var cMobile_Content_Button_Child_Active = "wis-jscss-menu_mobile-box-nav-elem-child-button__open";
        var cMobile_Content_Nav_Elem_Child = "wis-js-menu_mobile-box-nav-elem-child";
        var cMobile_Content_Nav_Elem_Child_Menu = "wis-js-menu_mobile-box-nav-elem-child-menu";
        $Bg_1.click(function () {
            $Menu_Mobile_1.removeClass(cMenu_Mobile_Active_1);
            $Bg_1.removeClass(cBg_Active_1);
        });
        $Menu_Button_Close.click(function () {
            $Menu_Mobile_1.removeClass(cMenu_Mobile_Active_1);
            $Bg_1.removeClass(cBg_Active_1);
        });
        $Menu_Button.click(function () {
            $Menu_Mobile_1.addClass(cMenu_Mobile_Active_1);
            $Bg_1.addClass(cBg_Active_1);
        });
    }
})(WIS_M_MenuMobile || (WIS_M_MenuMobile = {}));
//# sourceMappingURL=WIS_M_MenuMobile.js.map