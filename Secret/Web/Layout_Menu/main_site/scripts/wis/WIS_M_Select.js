var WIS_M_Select;
(function (WIS_M_Select) {
    var $Elem = $('.wis-js-select');
    var $Block = $('.wis-js-select-block');
    var $Button = $Elem.find('.wis-js-select-button');
    var $ButtonCLose = $Elem.find('.wis-js-select-close');
    var $Radio = $Elem.find('.wis-js-select-elem');
    var $Submit = $Elem.find('.wis-js-select-submit');
    if ($Elem != undefined && $Elem.length > 0) {
        $Button.click(function () {
            $Block.addClass('wis-jscss-select-block_active');
        });
        console.log($ButtonCLose);
        $ButtonCLose.click(function () {
            $Block.removeClass('wis-jscss-select-block_active');
        });
        $Radio.change(function () {
            $Submit.prop('disabled', false);
        });
        $Submit.click(function () {
            $Radio.each(function () {
                if ($(this).is(":checked")) {
                    $Button.text($(this).val());
                }
            });
            $Block.removeClass('wis-jscss-select-block_active');
        });
    }
})(WIS_M_Select || (WIS_M_Select = {}));
//# sourceMappingURL=WIS_M_Select.js.map