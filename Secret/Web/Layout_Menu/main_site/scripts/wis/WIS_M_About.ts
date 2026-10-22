/* Автор: Инфографика (Висерв) - https://wiserv.ru */

/**
 * Компонент "About"
 */
module WIS_M_About {

    let $Elem = $('.wis-js-about');

    if ($Elem != undefined && $Elem.length > 0) {

        let input = $Elem.find('.wis-js-about-input');


        let button = $Elem.find('.wis-js-open-order');

        //input.mask("9999", {
        //    //placeholder:"XXXX"
        //});

        input.keyup(function () {

            console.log(button)

            if ($(this).val().indexOf("X") != -1) {
                button.prop("disabled", true);
            }

            else {
                button.prop("disabled", false);
            }
        })
    }
}