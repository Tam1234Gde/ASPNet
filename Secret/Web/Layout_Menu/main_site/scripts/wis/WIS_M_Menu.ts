/* Автор: Инфографика (Висерв) - https://wiserv.ru */

/**
 * Компонент "Rating"
 */
module WIS_M_Menu {

    let $Elem = $('.wis-js-menu-scroll');

    

    if ($Elem != undefined && $Elem.length > 0) {

        let item = $Elem.find('.wis-js-menu-scroll-elem');



        item.click(function () {

            item.each(function () {
                $(this).removeClass('wis-menu-scroll-block-elem_active')


            })

            $(this).addClass('wis-menu-scroll-block-elem_active')

        })

    }


}