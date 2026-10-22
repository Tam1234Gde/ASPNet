/* Автор: Инфографика (Висерв) - https://wiserv.ru */

/**
 * Компонент "Select"
 */
module WIS_M_Select{

    let $Elem = $('.wis-js-select');

    let $Block = $('.wis-js-select-block');

    let $Button = $Elem.find('.wis-js-select-button');

    let $ButtonCLose = $Elem.find('.wis-js-select-close');

    let $Radio = $Elem.find('.wis-js-select-elem');

    

    let $Submit = $Elem.find('.wis-js-select-submit');

    if ($Elem != undefined && $Elem.length > 0) {

        $Button.click(function () {


            $Block.addClass('wis-jscss-select-block_active');

        });
        console.log($ButtonCLose)
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


        })


    }


}