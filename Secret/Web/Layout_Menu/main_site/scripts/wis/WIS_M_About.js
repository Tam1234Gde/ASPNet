var WIS_M_About;
(function (WIS_M_About) {
    var $Elem = $('.wis-js-about');
    if ($Elem != undefined && $Elem.length > 0) {
        var input = $Elem.find('.wis-js-about-input');
        var button_1 = $Elem.find('.wis-js-open-order');
        input.keyup(function () {
            console.log(button_1);
            if ($(this).val().indexOf("X") != -1) {
                button_1.prop("disabled", true);
            }
            else {
                button_1.prop("disabled", false);
            }
        });
    }
})(WIS_M_About || (WIS_M_About = {}));
//# sourceMappingURL=WIS_M_About.js.map