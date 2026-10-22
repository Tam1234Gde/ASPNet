var WIS_M_Login_Check;
(function (WIS_M_Login_Check) {
    var $Elem = $('.wis-js-login');
    var $Input_Phone = $Elem.find('.wis-js-login-input_phone');
    var $Input_Email = $Elem.find('.wis-js-login-input_email');
    var $Button = $Elem.find('.wis-js-login-button');
    if ($Elem != undefined && $Elem.height() > 0) {
        function Validate_Phone(val) {
            var value = val.replace("+", "").replace(/\s+/g, "").replace(/-/g, "").replace(/_/g, "").replace("(", "").replace(")", "");
            var pattern = /^[\d\+][\d\(\)\ -]{4,14}\d$/;
            if (pattern.test(val) && value.length >= 11) {
                $Button.prop("disabled", false);
            }
            else {
                $Button.prop("disabled", true);
            }
        }
        function Validate_Email(val) {
            var pattern = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (pattern.test(val)) {
                $Button.prop("disabled", false);
            }
            else {
                $Button.prop("disabled", true);
            }
        }
        ;
        $Input_Phone.keyup(function () {
            Validate_Phone($(this).val());
        });
        $Input_Email.keyup(function () {
            Validate_Email($(this).val());
        });
    }
})(WIS_M_Login_Check || (WIS_M_Login_Check = {}));
//# sourceMappingURL=WIS_M_Login_%D0%A1heck.js.map