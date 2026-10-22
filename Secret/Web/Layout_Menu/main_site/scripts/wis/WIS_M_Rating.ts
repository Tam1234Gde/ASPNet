/* Автор: Инфографика (Висерв) - https://wiserv.ru */

/**
 * Компонент "Rating"
 */
module WIS_M_Rating {

    let $Elem = $('.wis-js-rating');

    if ($Elem != undefined && $Elem.length > 0) {

        let $Input_Full = $Elem.find('.wis-js-rating-input_full');

        let $Input_Elem = $Elem.find('.wis-js-rating-input_elem');

        let $Text = $Elem.find('.wis-js-rating-text')

        $Input_Elem.click(function () {

            let title = $(this).attr('title');

            let val = $(this).val()

            $Text.text(title);

            $Text.attr('data-rating', val);

            $Input_Full.val(val);

            if (Number(val) <= 3) {
                $('.wis-js-form').slideDown();
            }

            else {
                $('.wis-js-form').slideUp();
            }

            $('.wis-js-rating-button').show();

        })

    }
}