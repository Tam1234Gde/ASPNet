var WIS_M_Tabs;
(function (WIS_M_Tabs) {
    var $Elem = $('.wis-js-tabs');
    var $Button = $('.wis-js-tabs-button');
    var $Block = $('.wis-js-tabs-block');
    var $HandingButton = $('#wis-handing');
    var $HandingBlock = $('.wis-js-block-handing');
    var $Policy = $('#wis-policy');
    var $PolicyButton = $('.wis-js-policy-button');
    if ($Elem != undefined && $Elem.length > 0) {
        $Button.click(function () {
            var $this = $(this);
            if ($this.hasClass('wis-order-block-tab-buttons-elem_active')) {
            }
            else {
                $Button.removeClass('wis-order-block-tab-buttons-elem_active');
                $this.addClass('wis-order-block-tab-buttons-elem_active');
            }
            $Block.each(function () {
                if ($(this).data('id') == $this.data('id')) {
                    $(this).show();
                }
                else {
                    $(this).hide();
                }
            });
        });
        $HandingButton.change(function () {
            if ($(this).is(':checked')) {
                $HandingBlock.show();
            }
            else {
                $HandingBlock.hide();
            }
        });
        $Policy.change(function () {
            if ($(this).is(':checked')) {
                $PolicyButton.prop('disabled', false);
            }
            else {
                $PolicyButton.prop('disabled', true);
            }
        });
    }
})(WIS_M_Tabs || (WIS_M_Tabs = {}));
//# sourceMappingURL=WIS_M_Tabs.js.map