var WIS_B_Common;
(function (WIS_B_Common) {
    WIS_B_Common.cValidate_Error = "wis-jscss-validate-error";
    WIS_B_Common.sError_ComboBoxEmpty = "Не выбрано";
    var sError_Space = "Поле не может содержать пробелы";
    var sError_Incorrectly = "Заполнено неверно";
    var sError_Empty = "Не заполнено";
    function _Check_Space(data) {
        if (data.$Elem != undefined && data.$Elem != null) {
            var value = data.$Elem.val();
            var value_new = value.replace(/\s+/g, '');
            if (value.length != value_new.length) {
                data.$Elem.addClass(WIS_B_Common.cValidate_Error);
                if (data.$Validate != undefined && data.$Validate != null) {
                    if (data.s_Error_Space !== undefined) {
                        data.$Validate.text(data.s_Error_Space);
                    }
                    else {
                        data.$Validate.text(sError_Space);
                    }
                }
                return false;
            }
            else {
                data.$Elem.removeClass(WIS_B_Common.cValidate_Error);
                if (data.$Validate != undefined && data.$Validate != null) {
                    data.$Validate.text("");
                }
                return true;
            }
        }
        return false;
    }
    function _Check_Length(data, Flag_Check) {
        if (!Flag_Check) {
            var value = data.$Elem.val();
            if (data.$Elem !== undefined) {
                data.$Elem.addClass(WIS_B_Common.cValidate_Error);
            }
            if (value.length > 0) {
                if (data.$Validate != undefined && data.$Validate != null) {
                    if (data.s_Error_Length != undefined) {
                        data.$Validate.text(data.s_Error_Length);
                    }
                    else {
                        data.$Validate.text("\u0414\u043B\u0438\u043D\u0430 \u043F\u043E\u043B\u044F '".concat(data.sFieldName, "' \u0434\u043E\u043B\u0436\u043D\u0430 \u0431\u044B\u0442\u044C \u043E\u0442 ").concat(data.nMin, " \u0434\u043E ").concat(data.nMax, " \u0441\u0438\u043C\u0432\u043E\u043B\u043E\u0432."));
                    }
                }
            }
            else {
                if (data.$Validate != undefined && data.$Validate != null) {
                    if (data.s_Error_Empty !== undefined) {
                        data.$Validate.text(data.s_Error_Empty);
                    }
                    else {
                        data.$Validate.text(sError_Empty);
                    }
                }
            }
            return false;
        }
        else {
            if (data.$Validate != undefined && data.$Validate != null)
                data.$Validate.text("");
            if (data.$Elem !== undefined) {
                data.$Elem.removeClass(WIS_B_Common.cValidate_Error);
            }
            return true;
        }
    }
    function Check_Length(elem) {
        var value = elem.$Elem.val();
        var Flag_Check = !(elem.$Elem.val().length > elem.nMax || elem.$Elem.val().length < elem.nMin);
        return _Check_Length(elem, Flag_Check);
    }
    WIS_B_Common.Check_Length = Check_Length;
    function Check_Length_WithoutSpace(elem) {
        var value = elem.$Elem.val();
        var Flag_Check = !(elem.$Elem.val().replace(/\s+/g, '').length > elem.nMax || elem.$Elem.val().replace(/\s+/g, '').length < elem.nMin);
        return _Check_Length(elem, Flag_Check);
    }
    WIS_B_Common.Check_Length_WithoutSpace = Check_Length_WithoutSpace;
    function Check_Length_Trim(elem) {
        var value = elem.$Elem.val();
        var Flag_Check = !(elem.$Elem.val().trim().length > elem.nMax || elem.$Elem.val().trim().length < elem.nMin);
        return _Check_Length(elem, Flag_Check);
    }
    WIS_B_Common.Check_Length_Trim = Check_Length_Trim;
    function Check_Phone(data) {
        var val = data.$Elem.val().replace("+", "").replace(/\s+/g, "").replace(/-/g, "").replace(/_/g, "").replace("(", "").replace(")", "");
        if (val.length == 11) {
            if (data.$Elem !== undefined) {
                data.$Elem.removeClass(WIS_B_Common.cValidate_Error);
            }
            if (data.$Validate != undefined && data.$Validate != null) {
                data.$Validate.text("");
            }
            return true;
        }
        else {
            if (data.$Elem !== undefined) {
                data.$Elem.addClass(WIS_B_Common.cValidate_Error);
            }
            if (data.$Validate != undefined && data.$Validate != null) {
                if (data.s_Error_Incorrectly !== undefined) {
                    data.$Validate.text(data.s_Error_Incorrectly);
                }
                else {
                    data.$Validate.text(sError_Incorrectly);
                }
            }
            return false;
        }
    }
    WIS_B_Common.Check_Phone = Check_Phone;
    function Check_Email(data) {
        var flag = _Check_Space({
            $Elem: data.$Elem,
            $Validate: data.$Validate,
            s_Error_Space: data.s_Error_Space
        });
        if (flag) {
            var val = data.$Elem.val();
            if (val.replace(/\s+/g, '').length > 0) {
                var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                if (re.test(val)) {
                    if (data.$Elem !== undefined) {
                        data.$Elem.removeClass(WIS_B_Common.cValidate_Error);
                    }
                    if (data.$Validate != undefined && data.$Validate != null) {
                        data.$Validate.text("");
                    }
                    return true;
                }
                else {
                    if (data.$Elem !== undefined) {
                        data.$Elem.addClass(WIS_B_Common.cValidate_Error);
                    }
                    if (data.$Validate != undefined && data.$Validate != null) {
                        if (data.s_Error_Incorrectly !== undefined) {
                            data.$Validate.text(data.s_Error_Incorrectly);
                        }
                        else {
                            data.$Validate.text('Проверьте ввод, пример - info@play-helper.ru');
                        }
                    }
                    return false;
                }
            }
            else {
                if (data.$Elem !== undefined) {
                    data.$Elem.addClass(WIS_B_Common.cValidate_Error);
                }
                if (data.$Validate != undefined && data.$Validate != null) {
                    if (data.s_Error_Empty !== undefined) {
                        data.$Validate.text(data.s_Error_Empty);
                    }
                    else {
                        data.$Validate.text(sError_Empty);
                    }
                }
            }
        }
        return false;
    }
    WIS_B_Common.Check_Email = Check_Email;
    function Check_Password(data) {
        var flag = _Check_Space({
            $Elem: data.$Elem,
            $Validate: data.$Validate,
            s_Error_Space: data.s_Error_Space
        });
        if (flag) {
            return Check_Length_WithoutSpace(data);
        }
        return false;
    }
    WIS_B_Common.Check_Password = Check_Password;
    function Check_ComboBox_Str(elem) {
        if (elem.anyValue) {
            Check_ComboBox(elem, true);
            return true;
        }
        else {
            Check_ComboBox(elem, false);
            return false;
        }
    }
    WIS_B_Common.Check_ComboBox_Str = Check_ComboBox_Str;
    function Check_ComboBox_Num(elem) {
        if (elem.anyValue >= 0) {
            Check_ComboBox(elem, true);
            return true;
        }
        else {
            Check_ComboBox(elem, false);
            return false;
        }
    }
    WIS_B_Common.Check_ComboBox_Num = Check_ComboBox_Num;
    function Check_ComboBox(elem, Flag_Check) {
        if (Flag_Check) {
            if (elem.$Elem !== undefined) {
                elem.$Elem.removeClass(WIS_B_Common.cValidate_Error);
            }
            if (elem.$Validate !== undefined) {
                elem.$Validate.text("");
            }
        }
        else {
            if (elem.$Elem !== undefined) {
                elem.$Elem.addClass(WIS_B_Common.cValidate_Error);
            }
            if (elem.$Validate !== undefined) {
                elem.$Validate.text(elem.sValidate_Text);
            }
        }
    }
    function Convert_ToNumber(value) {
        var return_value = value.replace(new RegExp(",", 'g'), ".").replace(new RegExp(',', 'g'), '.').replace(/\s+/g, '');
        return Number(return_value);
    }
    WIS_B_Common.Convert_ToNumber = Convert_ToNumber;
    function Validation_Error_Del(elem) {
        if (elem.$Elem.hasClass(WIS_B_Common.cValidate_Error)) {
            if (elem.$Elem != undefined && elem.$Elem.length > 0) {
                elem.$Elem.removeClass(WIS_B_Common.cValidate_Error);
            }
            if (elem.$Validate != undefined && elem.$Validate.length > 0) {
                elem.$Validate.text("");
            }
        }
    }
    WIS_B_Common.Validation_Error_Del = Validation_Error_Del;
    function Keypress_NumOnlyPlus(elem) {
        elem.$Elem.keypress(function (e) {
            var verified = false;
            var Symbol_Max = 30;
            if (elem.nSymbol_Max !== undefined && elem.nSymbol_Max > 0) {
                Symbol_Max = elem.nSymbol_Max;
            }
            if (elem.$Elem.val().length > Symbol_Max) {
                verified = true;
            }
            else {
                verified = ((e.which === 8 || e.which === undefined || e.which === 0) ? null : String.fromCharCode(e.which).match(/[^0-9,.]/));
            }
            if (verified) {
                e.preventDefault();
            }
            if (elem.fCallback_KeyPress !== undefined && typeof elem.fCallback_KeyPress === "function") {
                elem.fCallback_KeyPress();
            }
        });
        elem.$Elem.focus(function () {
            if (parseFloat(elem.$Elem.val()) === 0) {
                elem.$Elem.val("");
            }
            if (elem.fCallback_Focus !== undefined && typeof elem.fCallback_Focus === "function") {
                elem.fCallback_Focus();
            }
        });
    }
    WIS_B_Common.Keypress_NumOnlyPlus = Keypress_NumOnlyPlus;
})(WIS_B_Common || (WIS_B_Common = {}));
//# sourceMappingURL=WIS_B_Common.js.map