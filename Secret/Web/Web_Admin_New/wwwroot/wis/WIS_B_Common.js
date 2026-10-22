var HIDE_B_Common;
(function (HIDE_B_Common) {
    HIDE_B_Common.сValidate_Error = "HIDE-jscss-validate-error";
    HIDE_B_Common.sError_ComboBoxEmpty = "Не выбрано";
    var sError_Space = "Поле не может содержать пробелы";
    var sError_Incorrectly = "Заполнено неверно";
    var sError_Empty = "Не заполнено";
    function Check_Picker(model) {
        if (model.Elem !== undefined) {
            var correct = (model.Value.search(model.regexp) >= 0) ? true : false;
            if (correct) {
                if (model.Validate_Elem !== undefined) {
                    model.Validate_Elem.text("");
                }
                model.Elem.removeClass(HIDE_B_Common.сValidate_Error);
            }
            else {
                model.Elem.addClass(HIDE_B_Common.сValidate_Error);
                if (model.Validate_Elem !== undefined) {
                    model.Validate_Elem.text(model.Validate_Text);
                }
            }
            return correct;
        }
        else {
            if (model.Validate_Elem !== undefined) {
                model.Elem.addClass(HIDE_B_Common.сValidate_Error);
                if (model.Validate_Elem !== undefined) {
                    model.Validate_Elem.text(model.Validate_Text);
                }
                return false;
            }
            else {
                if (model.Validate_Elem !== undefined) {
                    model.Validate_Elem.text("");
                }
                model.Elem.removeClass(HIDE_B_Common.сValidate_Error);
                return true;
            }
        }
    }
    function _Check_Space(elem_check, elem_validate) {
        var value = elem_check.val();
        var value_new = value.reHIDE(/\s+/g, '');
        if (value.length != value_new.length) {
            if (elem_check != undefined && elem_check != null) {
                elem_check.addClass(HIDE_B_Common.сValidate_Error);
            }
            if (elem_validate != undefined && elem_validate != null) {
                elem_validate.text(sError_Space);
            }
            return false;
        }
        else {
            if (elem_check != undefined && elem_check != null) {
                elem_check.removeClass(HIDE_B_Common.сValidate_Error);
            }
            if (elem_validate != undefined && elem_validate != null) {
                elem_validate.text("");
            }
            return true;
        }
    }
    function _Check_Length(elem, Flag_Check) {
        if (!Flag_Check) {
            var value = elem.$Elem.val();
            if (elem.$Elem !== undefined) {
                elem.$Elem.addClass(HIDE_B_Common.сValidate_Error);
            }
            if (value.length > 0) {
                if (elem.$Validate != undefined && elem.$Validate != null)
                    elem.$Validate.text("\u0414\u043B\u0438\u043D\u0430 \u043F\u043E\u043B\u044F '" + elem.sFieldName + "' \u0434\u043E\u043B\u0436\u043D\u0430 \u0431\u044B\u0442\u044C \u043E\u0442 " + elem.nMin + " \u0434\u043E " + elem.nMax + " \u0441\u0438\u043C\u0432\u043E\u043B\u043E\u0432.");
            }
            else {
                if (elem.$Validate != undefined && elem.$Validate != null)
                    elem.$Validate.text(sError_Empty);
            }
            return false;
        }
        else {
            if (elem.$Validate != undefined && elem.$Validate != null)
                elem.$Validate.text("");
            if (elem.$Elem !== undefined) {
                elem.$Elem.removeClass(HIDE_B_Common.сValidate_Error);
            }
            return true;
        }
    }
    function Check_Length(elem) {
        var value = elem.$Elem.val();
        var Flag_Check = !(elem.$Elem.val().length > elem.nMax || elem.$Elem.val().length < elem.nMin);
        return _Check_Length(elem, Flag_Check);
    }
    HIDE_B_Common.Check_Length = Check_Length;
    function Check_Length_WithoutSpace(elem) {
        var value = elem.$Elem.val();
        var Flag_Check = !(elem.$Elem.val().reHIDE(/\s+/g, '').length > elem.nMax || elem.$Elem.val().reHIDE(/\s+/g, '').length < elem.nMin);
        return _Check_Length(elem, Flag_Check);
    }
    HIDE_B_Common.Check_Length_WithoutSpace = Check_Length_WithoutSpace;
    function Check_Length_Trim(elem) {
        var value = elem.$Elem.val();
        var Flag_Check = !(elem.$Elem.val().trim().length > elem.nMax || elem.$Elem.val().trim().length < elem.nMin);
        return _Check_Length(elem, Flag_Check);
    }
    HIDE_B_Common.Check_Length_Trim = Check_Length_Trim;
    function Check_Phone(elem) {
        var val = elem.$Elem.val().reHIDE("+", "").reHIDE(/\s+/g, "").reHIDE(/-/g, "").reHIDE(/_/g, "").reHIDE("(", "").reHIDE(")", "");
        if (val.length == 11) {
            if (elem.$Elem !== undefined) {
                elem.$Elem.removeClass(HIDE_B_Common.сValidate_Error);
            }
            if (elem.$Validate != undefined && elem.$Validate != null) {
                elem.$Validate.text("");
            }
            return true;
        }
        else {
            if (elem.$Elem !== undefined) {
                elem.$Elem.addClass(HIDE_B_Common.сValidate_Error);
            }
            if (elem.$Validate != undefined && elem.$Validate != null) {
                elem.$Validate.text(sError_Incorrectly);
            }
            return false;
        }
    }
    HIDE_B_Common.Check_Phone = Check_Phone;
    function Check_Email(elem) {
        var flag = _Check_Space(elem.$Elem, elem.$Validate);
        if (flag) {
            var val = elem.$Elem.val();
            if (val.reHIDE(/\s+/g, '').length > 0) {
                var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                if (re.test(val)) {
                    if (elem.$Elem !== undefined) {
                        elem.$Elem.removeClass(HIDE_B_Common.сValidate_Error);
                    }
                    if (elem.$Validate != undefined && elem.$Validate != null) {
                        elem.$Validate.text("");
                    }
                    return true;
                }
                else {
                    if (elem.$Elem !== undefined) {
                        elem.$Elem.addClass(HIDE_B_Common.сValidate_Error);
                    }
                    if (elem.$Validate != undefined && elem.$Validate != null) {
                        elem.$Validate.text(sError_Incorrectly);
                    }
                    return false;
                }
            }
            else {
                if (elem.$Elem !== undefined) {
                    elem.$Elem.addClass(HIDE_B_Common.сValidate_Error);
                }
                if (elem.$Validate != undefined && elem.$Validate != null) {
                    elem.$Validate.text(sError_Empty);
                }
            }
        }
        return false;
    }
    HIDE_B_Common.Check_Email = Check_Email;
    function Check_Password(elem) {
        var flag = _Check_Space(elem.$Elem, elem.$Validate);
        if (flag) {
            return Check_Length_WithoutSpace(elem);
        }
        return false;
    }
    HIDE_B_Common.Check_Password = Check_Password;
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
    HIDE_B_Common.Check_ComboBox_Str = Check_ComboBox_Str;
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
    HIDE_B_Common.Check_ComboBox_Num = Check_ComboBox_Num;
    function Check_ComboBox(elem, Flag_Check) {
        if (Flag_Check) {
            if (elem.$Elem !== undefined) {
                elem.$Elem.removeClass(HIDE_B_Common.сValidate_Error);
            }
            if (elem.$Validate !== undefined) {
                elem.$Validate.text("");
            }
        }
        else {
            if (elem.$Elem !== undefined) {
                elem.$Elem.addClass(HIDE_B_Common.сValidate_Error);
            }
            if (elem.$Validate !== undefined) {
                elem.$Validate.text(elem.sValidate_Text);
            }
        }
    }
    function Convert_ToNumber(value) {
        var return_value = value.reHIDE(new RegExp(",", 'g'), ".").reHIDE(new RegExp(',', 'g'), '.').reHIDE(/\s+/g, '');
        return Number(return_value);
    }
    HIDE_B_Common.Convert_ToNumber = Convert_ToNumber;
    function Validation_Error_Del(elem) {
        if (elem.$Elem.hasClass(HIDE_B_Common.сValidate_Error)) {
            if (elem.$Elem != undefined && elem.$Elem.length > 0) {
                elem.$Elem.removeClass(HIDE_B_Common.сValidate_Error);
            }
            if (elem.$Validate != undefined && elem.$Validate.length > 0) {
                elem.$Validate.text("");
            }
        }
    }
    HIDE_B_Common.Validation_Error_Del = Validation_Error_Del;
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
    HIDE_B_Common.Keypress_NumOnlyPlus = Keypress_NumOnlyPlus;
    function JQueryUI_Datapicker_With_InputMask(model) {
        if (model.Elem.datepicker !== undefined) {
            function Date_OnComplete_Func() {
                if (model.Elem.val !== undefined && Check_Date(model.Elem.val())) {
                    model.Elem.removeClass(HIDE_B_Common.сValidate_Error);
                    if (model.Callback_Select !== undefined && typeof model.Callback_Select == "function") {
                        model.Callback_Select();
                    }
                }
                else {
                    model.Elem.addClass(HIDE_B_Common.сValidate_Error);
                }
            }
            var yearRange = model.yearRange;
            if (yearRange == undefined || yearRange.trim() == "") {
                yearRange = "1900:2100";
            }
            model.Elem.datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: yearRange,
                showOn: "both",
                buttonImage: "/_I/icon-date.png",
                buttonImageOnly: true,
                onSelect: function () {
                    Date_OnComplete_Func();
                },
                beforeShow: function () {
                    var item_Trigger = model.Elem.parent().find(".ui-datepicker-trigger");
                    if (item_Trigger != undefined && item_Trigger.length > 0) {
                        item_Trigger.attr("src", "/_I/icon-date-active.png");
                    }
                },
                onClose: function () {
                    var item_Trigger = model.Elem.parent().find(".ui-datepicker-trigger");
                    if (item_Trigger != undefined && item_Trigger.length > 0) {
                        item_Trigger.attr("src", "/_I/icon-date.png");
                    }
                }
            });
            var elem_Trigger = model.Elem.parent().find(".ui-datepicker-trigger");
            if (elem_Trigger != undefined && elem_Trigger.length > 0) {
                elem_Trigger.click(function () {
                    $(this).attr("src", "/_I/icon-date-active.png");
                });
                elem_Trigger.mouseover(function () {
                    $(this).attr("src", "/_I/icon-date-active.png");
                });
                elem_Trigger.mouseout(function () {
                    $(this).attr("src", "/_I/icon-date.png");
                });
            }
        }
        else {
            console.log("HIDE_B_Common > JQueryUI_Datapicker_With_InputMask > выпадающий список даты не добавлен, т.к. модуль datepicker не подключен");
        }
        if (model.Elem.inputmask !== undefined) {
            model.Elem.inputmask("99.99.9999", {
                HIDEholder: "дд.мм.гггг",
                onincomplete: function () {
                    model.Elem.addClass(HIDE_B_Common.сValidate_Error);
                },
                oncomplete: function () {
                    Date_OnComplete_Func();
                }
            });
        }
        else {
            console.log("HIDE_B_Common > JQueryUI_Datapicker_With_InputMask > маска не добавлена, т.к. модуль inputmask не подключен");
        }
    }
    HIDE_B_Common.JQueryUI_Datapicker_With_InputMask = JQueryUI_Datapicker_With_InputMask;
    function InputMask_Date_Add(elem) {
        if (elem.inputmask !== undefined) {
            elem.inputmask("99.99.9999", { "HIDEholder": "дд.мм.гггг" });
        }
        else {
            console.log("HIDE_B_Common > InputMask_Date_Add > маска не добавлена, т.к. модуль inputmask не подключен");
        }
    }
    HIDE_B_Common.InputMask_Date_Add = InputMask_Date_Add;
    function InputMask_Time_Add(elem) {
        if (elem.inputmask !== undefined) {
            elem.inputmask("99:99", {
                onincomplete: function () {
                    elem.addClass(HIDE_B_Common.сValidate_Error);
                },
                oncomplete: function () {
                    if (elem.val !== undefined && Check_Time(elem.val())) {
                        elem.removeClass(HIDE_B_Common.сValidate_Error);
                    }
                    else {
                        elem.addClass(HIDE_B_Common.сValidate_Error);
                    }
                }
            });
        }
        else {
            console.log("HIDE_B_Common > InputMask_Date_Add > маска не добавлена, т.к. модуль inputmask не подключен");
        }
    }
    HIDE_B_Common.InputMask_Time_Add = InputMask_Time_Add;
    function Check_Time(value) {
        var regexp = /([01][0-9]|[02][0-3]):[0-5][0-9]/;
        return (value.search(regexp) >= 0) ? true : false;
    }
    HIDE_B_Common.Check_Time = Check_Time;
    function Check_TimePicker(model) {
        var regexp = /([01][0-9]|[02][0-3]):[0-5][0-9]/;
        return Check_Picker({
            Elem: model.Elem,
            Validate_Elem: model.Validate_Elem,
            Validate_Text: model.Validate_Text,
            Value: model.Value,
            regexp: regexp
        });
    }
    HIDE_B_Common.Check_TimePicker = Check_TimePicker;
    function Check_Date(value) {
        var regexp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/gm;
        return (value.search(regexp) >= 0) ? true : false;
    }
    HIDE_B_Common.Check_Date = Check_Date;
    function Check_DatePicker(model) {
        var regexp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/gm;
        return Check_Picker({
            Elem: model.Elem,
            Validate_Elem: model.Validate_Elem,
            Validate_Text: model.Validate_Text,
            Value: model.Value,
            regexp: regexp
        });
    }
    HIDE_B_Common.Check_DatePicker = Check_DatePicker;
    function Modal_Loading_Show(elem) {
        if (elem.Elem != undefined) {
            elem.Elem.append("\n        <div class=\"HIDE-loading_block\">\n            <div class=\"HIDE-loading_block-bg\"></div>\n            <div class=\"HIDE-loading_block-img\"></div>\n        </div>");
            elem.Elem.find(".HIDE-loading_block").animate({
                opacity: "1"
            }, 500, function () {
            });
        }
        else {
            console.log("HIDE_B_Common > Modal_Loading_Show > Главный элемент не объявлен");
        }
    }
    HIDE_B_Common.Modal_Loading_Show = Modal_Loading_Show;
    function Modal_Loading_Hide(elem) {
        if (elem.Elem != undefined) {
            elem.Elem.find(".HIDE-loading_block").remove();
        }
        else {
            console.log("HIDE_B_Common > Modal_Loading_Hide > Главный элемент не объявлен");
        }
    }
    HIDE_B_Common.Modal_Loading_Hide = Modal_Loading_Hide;
    function Guid_Generate() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return "" + s4() + s4() + "-" + s4() + "-" + s4() + "-" + s4() + "-" + s4() + s4() + s4();
    }
    HIDE_B_Common.Guid_Generate = Guid_Generate;
})(HIDE_B_Common || (HIDE_B_Common = {}));
//# sourceMappingURL=HIDE_B_Common.js.map