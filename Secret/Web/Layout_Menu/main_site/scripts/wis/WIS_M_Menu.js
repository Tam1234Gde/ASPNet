var WIS_M_Menu;
(function (WIS_M_Menu) {
    var $Elem = $('.wis-js-menu-scroll');
    if ($Elem != undefined && $Elem.length > 0) {
        var item_1 = $Elem.find('.wis-js-menu-scroll-elem');
        item_1.click(function () {
            item_1.each(function () {
                $(this).removeClass('wis-menu-scroll-block-elem_active');
            });
            $(this).addClass('wis-menu-scroll-block-elem_active');
        });
    }
})(WIS_M_Menu || (WIS_M_Menu = {}));
//# sourceMappingURL=WIS_M_Menu.js.map