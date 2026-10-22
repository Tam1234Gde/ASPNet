var WIS_M_Code_Auth;
(function (WIS_M_Code_Auth) {
    var $Elem = $('.wis-js-code');
    var $Input = $Elem.find('input');
    if ($Elem != undefined && $Elem.length > 0) {
        $Input.keyup(function (e) {
            var val = $(this).val();
            var fullval = '';
            $('.wis-js-code-elem').each(function () {
                fullval = fullval + $(this).val();
            });
            $('.wis-js-code-elem').keyup(function () { $Elem.removeClass('wis-modal-code_error'); $('.wis-js-error-code_auth').text(''); });
            $(this).parent().find('input[type="hidden"]').val(fullval);
            if ($(this).parent().find('input[type="hidden"]').val().length == 4) {
                $('.wis-js-code-button').prop("disabled", false);
            }
            else {
                $('.wis-js-code-button').prop("disabled", true);
            }
        });
    }
})(WIS_M_Code_Auth || (WIS_M_Code_Auth = {}));
//# sourceMappingURL=WIS_M_Code_Auth.js.map