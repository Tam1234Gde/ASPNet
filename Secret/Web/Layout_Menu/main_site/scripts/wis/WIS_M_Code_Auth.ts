/* Поддержка браузеров: IE8+ и остальные браузеры. */
/* Автор: Инфографика (Висерв) - https://wiserv.ru */

/**
 * Компонент "Вот кода"
 */
module WIS_M_Code_Auth {

    /**
     * Элементы - Блоки с вкладками
     */
    let $Elem: JQuery = $('.wis-js-code');

    /**
     * Элементы - Блоки с вкладками
    */
    let $Input: JQuery = $Elem.find('input');

    if ($Elem != undefined && $Elem.length > 0) {

        //$Input.keydown(function (e) {

        //    if (e.keyCode == 8 || e.keyCode == 46) {

        //        if ($(this).val() == '') {
        //            $Input.eq($Input.index(this) - 1).focus();
        //        }

        //        else {
        //            $(this).val('');

        //        }

        //    }

        //}); 

        $Input.keyup(function (e) {
            var val = $(this).val();

            //// Ввод только цифр
            //if (val == val.replace(/[0-9]/, '') && e.keyCode !== 8 && e.keyCode !== 46 && e.keyCode !== 13) {
            //    $(this).val('');
            //    return false;
            //};
 
            //if (e.keyCode == 13) {
            //    e.preventDefault();
            //}
                
            //else if (val !== '') {
            //    $Input.eq($Input.index(this) + 1).focus();
            //}

            // Заполнение input hidden 
            let fullval = '';

            $('.wis-js-code-elem').each(function () {
                fullval = fullval + $(this).val();
            });

            $('.wis-js-code-elem').keyup(function () { $Elem.removeClass('wis-modal-code_error'); $('.wis-js-error-code_auth').text(''); })

            $(this).parent().find('input[type="hidden"]').val(fullval);

            if ($(this).parent().find('input[type="hidden"]').val().length == 4) {
                $('.wis-js-code-button').prop("disabled", false);

               
            }
            else {
                $('.wis-js-code-button').prop("disabled", true);
            }
        });

    }
}