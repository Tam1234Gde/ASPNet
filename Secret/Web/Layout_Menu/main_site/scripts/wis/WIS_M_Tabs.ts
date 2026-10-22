/* Автор: Инфографика (Висерв) - https://wiserv.ru */

/**
 * Компонент "Tabs"
 */
module WIS_M_Tabs{

    let $Elem = $('.wis-js-tabs');

    let $Button = $('.wis-js-tabs-button');

    let $Block = $('.wis-js-tabs-block');

    let $HandingButton = $('#wis-handing');

    let $HandingBlock = $('.wis-js-block-handing');


    let $Policy = $('#wis-policy');

    let $PolicyButton = $('.wis-js-policy-button');



    if ($Elem != undefined && $Elem.length > 0) {

        $Button.click(function () {

            let $this = $(this);

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


            })


        })

        $HandingButton.change(function () {


            if ($(this).is(':checked')) {

                $HandingBlock.show();

            }

            else {
                $HandingBlock.hide();
            }


        })

        $Policy.change(function () {

            if ($(this).is(':checked')) {

                $PolicyButton.prop('disabled', false);
            }

            else {
                $PolicyButton.prop('disabled', true);
            }
        })

    }

}