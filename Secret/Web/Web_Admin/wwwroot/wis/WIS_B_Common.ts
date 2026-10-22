/* Компонент HIDE_B_Common 1.19.11.11  */
/* Требуемые модули: jquery */
/* Поддержка браузеров: IE8+ и остальные браузеры. */
/* Автор: Инфографика (Висерв) - https://HIDEerv.ru */

/**
 * Общие методы
 */
module HIDE_B_Common {

    //#region Переменные и интерфейсы

    /**
    * Класс ошибки при валидации
    */
    export const сValidate_Error = "HIDE-jscss-validate-error";

    /**
     * Текст ошибки, когда ничего не выбрано
     * */
    export const sError_ComboBoxEmpty = "Не выбрано";

    var sError_Space = "Поле не может содержать пробелы";

    var sError_Incorrectly = "Заполнено неверно";

    var sError_Empty = "Не заполнено";

    /**
     * Проверка количества символов
     * */
    interface iCheck_Length {

        /**
        * Элемент - элемент которого проверяют на длину
        */
        $Elem: JQuery;

        /**
        * Строка - Наименование поля
        */
        sFieldName: string;

        /**
        * Число - минимальная допустимая длина
        */
        nMin: number;

        /**
        * Число - максимальная допустимая длина
        */
        nMax: number;

        /**
        * Элемент - элемент валидации (для вывода сообщения об ошибке)
        */
        $Validate: JQuery;
    }

    /**
     * Проверка телефона
     * */
    interface iCheck_Phone {

        /**
        * Элемент - элемент которого проверяют на телефон
        */
        $Elem: JQuery;

        /**
        * Элемент - элемент валидации (для вывода сообщения об ошибке)
        */
        $Validate: JQuery;
    }

    /**
     * Проверка эл. почты
     * */
    interface iCheck_Email {

        /**
        * Элемент - которого проверяют на корректность эл. почты
        */
        $Elem: JQuery;

        /**
        * Элемент - элемент валидации (для вывода сообщения об ошибке)
        */
        $Validate: JQuery;
    }

    interface iCheck_Common {

        /**
        * элемент которого проверяют
        */
        Elem: JQuery;

        /**
        * Значение
        */
        Value: any;

        /**
        * Текст ошибки
        */
        Validate_Text: string;

        /**
        * элемент валидации (для вывода сообщения об ошибке)
        */
        Validate_Elem: JQuery;
    }

    interface iCheck_Picker {

        /**
        * элемент которого проверяют
        */
        Elem: JQuery;

        /**
        * Значение
        */
        Value: any;

        /**
        * Текст ошибки
        */
        Validate_Text: string;

        /**
        * элемент валидации (для вывода сообщения об ошибке)
        */
        Validate_Elem: JQuery;

        /**
         * Регулярное выражение
         * */
        regexp: RegExp;
    }

    /**
     * Проверка выпадающего списка
     * */
    interface iCheck_ComboBox {

        /**
        * Элемент - элемент которого проверяют
        */
        $Elem: JQuery;

        /**
        * Значение
        */
        anyValue: any;

        /**
        * Текст ошибки
        */
        sValidate_Text: string;

        /**
        * Элемент - элемент валидации (для вывода сообщения об ошибке)
        */
        $Validate: JQuery;
    }

    /**
     * Удаление класса ошибки валидации
     * */
    interface iValidation_Error_Del {

        /**
        * Элемент - на котором будет удален класс ошибки валидации
        */
        $Elem: JQuery;

        /**
        * Элемент - валидации (для вывода сообщения об ошибке) в котором будет удален текст об ошибке
        */
        $Validate: JQuery;
    }

    /**
     * Обработка ввода - позволить ввод только положительных цифр
     * */
    interface iKeypress_NumOnlyPlus {

        /**
        * Элемент
        */
        $Elem: JQuery;

        /**
        * Число - Максимальное количество символов
        */
        nSymbol_Max?: number;

        /**
        * Функция обратного вызова - для события keypress
        */
        fCallback_KeyPress?: Function;

        /**
        * Функция обратного вызова - для события focus
        */
        fCallback_Focus?: Function;
    }

    interface iJQueryUI_Datapicker_With_InputMask {

        /**
        * Элемент
        */
        Elem: JQuery;

        /**
         * Диапазон лет, отображаемый в раскрывающемся списке
         * По умолчанию: "c-10:c+10"
         * */
        yearRange?: string;

        /**
        * Функция обратного вызова - для события select
        */
        Callback_Select?: Function;
    }

    //#endregion

    //#region Общие функции - Проверка элемента на валидность

    /**
     * Проверка элемента на валидность
     * @param model данные
     */
    function Check_Picker(model: iCheck_Picker): boolean {
        if (model.Elem !== undefined) {
            var correct = (model.Value.search(model.regexp) >= 0) ? true : false;

            if (correct) {
                if (model.Validate_Elem !== undefined) {
                    model.Validate_Elem.text("");
                }
                model.Elem.removeClass(сValidate_Error);
            }
            else {
                model.Elem.addClass(сValidate_Error);
                if (model.Validate_Elem !== undefined) {
                    model.Validate_Elem.text(model.Validate_Text);
                }
            }
            return correct;
        }
        else {
            if (model.Validate_Elem !== undefined) {
                model.Elem.addClass(сValidate_Error);
                if (model.Validate_Elem !== undefined) {
                    model.Validate_Elem.text(model.Validate_Text);
                }
                return false;
            }
            else {
                if (model.Validate_Elem !== undefined) {
                    model.Validate_Elem.text("");
                }
                model.Elem.removeClass(сValidate_Error);
                return true;
            }
        }
    }

    //#endregion

    //#region Проверка количества символов, Проверка содержания пробелов

    /**
     * Проверка содержания пробелов
     */
    function _Check_Space(elem_check: JQuery, elem_validate: JQuery) {
        let value: string = elem_check.val();
        let value_new: string = value.reHIDE(/\s+/g, '');

        if (value.length != value_new.length) {
            if (elem_check != undefined && elem_check != null) {
                elem_check.addClass(сValidate_Error);
            }
            if (elem_validate != undefined && elem_validate != null) {
                elem_validate.text(sError_Space);
            }
            return false;
        }
        else {
            if (elem_check != undefined && elem_check != null) {
                elem_check.removeClass(сValidate_Error);
            }
            if (elem_validate != undefined && elem_validate != null) {
                elem_validate.text("");
            }
            return true;
        }
    }

    /**
    * Проверка количества символов
    * @param elem данные
    * @param Flag_Check True - проверка удачна. False - содержит ошибки
    */
    function _Check_Length(elem: iCheck_Length, Flag_Check: boolean): boolean {
        if (!Flag_Check) {
            let value: string = elem.$Elem.val();
            if (elem.$Elem !== undefined) {
                elem.$Elem.addClass(сValidate_Error);
            }

            if (value.length > 0) {
                if (elem.$Validate != undefined && elem.$Validate != null) elem.$Validate.text(`Длина поля '${elem.sFieldName}' должна быть от ${elem.nMin} до ${elem.nMax} символов.`);
            }
            else {
                if (elem.$Validate != undefined && elem.$Validate != null) elem.$Validate.text(sError_Empty);
            }

            return false;
        } else {
            if (elem.$Validate != undefined && elem.$Validate != null) elem.$Validate.text("");
            if (elem.$Elem !== undefined) {
                elem.$Elem.removeClass(сValidate_Error);
            }
            return true;
        }
    }

    /**
    * Проверка количества символов
    * @param elem данные
    */
    export function Check_Length(elem: iCheck_Length): boolean {

        let value: string = elem.$Elem.val();
        let Flag_Check = !(elem.$Elem.val().length > elem.nMax || elem.$Elem.val().length < elem.nMin);
        return _Check_Length(elem, Flag_Check);
    }

    /**
    * Проверка количества символов (без пробелов)
    * @param elem данные
    */
    export function Check_Length_WithoutSpace(elem: iCheck_Length): boolean {

        let value: string = elem.$Elem.val();
        let Flag_Check = !(elem.$Elem.val().reHIDE(/\s+/g, '').length > elem.nMax || elem.$Elem.val().reHIDE(/\s+/g, '').length < elem.nMin);
        return _Check_Length(elem, Flag_Check);
    }


    /**
    * Проверка количества символов (пробелы вначале и в конце не учитываются)
    * @param elem данные
    */
    export function Check_Length_Trim(elem: iCheck_Length): boolean {

        let value: string = elem.$Elem.val();
        let Flag_Check = !(elem.$Elem.val().trim().length > elem.nMax || elem.$Elem.val().trim().length < elem.nMin);
        return _Check_Length(elem, Flag_Check);
    }

    //#endregion

    //#region Проверка количества символов для мобильного телефона, Проверка корректности электронной почты, Проверка корректности пароля

    /**
    * Проверка количества символов для мобильного телефона
    * @param elem данные
    */
    export function Check_Phone(elem: iCheck_Phone): boolean {
        let val = elem.$Elem.val().reHIDE("+", "").reHIDE(/\s+/g, "").reHIDE(/-/g, "").reHIDE(/_/g, "").reHIDE("(", "").reHIDE(")", "");
        if (val.length == 11) {
            if (elem.$Elem !== undefined) {
                elem.$Elem.removeClass(сValidate_Error);
            }
            if (elem.$Validate != undefined && elem.$Validate != null) {
                elem.$Validate.text("");
            }
            return true;
        }
        else {
            if (elem.$Elem !== undefined) {
                elem.$Elem.addClass(сValidate_Error);
            }
            if (elem.$Validate != undefined && elem.$Validate != null) {
                elem.$Validate.text(sError_Incorrectly);
            }
            return false;
        }
    }

    /**
    * Проверка корректности электронной почты
    * @param elem данные
    */
    export function Check_Email(elem: iCheck_Email): boolean {
        let flag: boolean = _Check_Space(elem.$Elem, elem.$Validate);
        if (flag) {
            let val = elem.$Elem.val();
            if (val.reHIDE(/\s+/g, '').length > 0) {
                let re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                if (re.test(val)) {
                    if (elem.$Elem !== undefined) {
                        elem.$Elem.removeClass(сValidate_Error);
                    }
                    if (elem.$Validate != undefined && elem.$Validate != null) {
                        elem.$Validate.text("");
                    }
                    return true;
                }
                else {
                    if (elem.$Elem !== undefined) {
                        elem.$Elem.addClass(сValidate_Error);
                    }
                    if (elem.$Validate != undefined && elem.$Validate != null) {
                        elem.$Validate.text(sError_Incorrectly);
                    }
                    return false;
                }
            }
            else {
                if (elem.$Elem !== undefined) {
                    elem.$Elem.addClass(сValidate_Error);
                }
                if (elem.$Validate != undefined && elem.$Validate != null) {
                    elem.$Validate.text(sError_Empty);
                }
            }
        }

        return false;
    }


    /**
     * Проверка корректности пароля
     * @param elem данные
     */
    export function Check_Password(elem: iCheck_Length) {
        let flag: boolean = _Check_Space(elem.$Elem, elem.$Validate);
        if (flag) {
            return Check_Length_WithoutSpace(elem);
        }
        return false;
    }

    //#endregion

    //#region Проверка выпадающего списка 

    /**
    * Проверка выпадающего списка (выбрано ли значение). В значение хранится string
    * @param elem данные
    */
    export function Check_ComboBox_Str(elem: iCheck_ComboBox) {
        if (elem.anyValue) {
            Check_ComboBox(elem, true);
            return true;
        } else {
            Check_ComboBox(elem, false);
            return false;
        }
    }

    /**
    * Проверка выпадающего списка (выбрано ли значение и больше ли оно нуля). В значение хранится number
    * @param elem данные
    */
    export function Check_ComboBox_Num(elem: iCheck_ComboBox) {
        if (elem.anyValue >= 0) {
            Check_ComboBox(elem, true);
            return true;
        } else {
            Check_ComboBox(elem, false);
            return false;
        }
    }

    /**
     * Проверка выпадающего списка
     * @param elem данные
     * @param Flag_Check True - проверка успешна. False - Содержит ошибки
     */
    function Check_ComboBox(elem: iCheck_ComboBox, Flag_Check: boolean) {
        if (Flag_Check) {
            if (elem.$Elem !== undefined) {
                elem.$Elem.removeClass(сValidate_Error);
            }

            if (elem.$Validate !== undefined) {
                elem.$Validate.text("");
            }
        }
        else {
            if (elem.$Elem !== undefined) {
                elem.$Elem.addClass(сValidate_Error);
            }
            if (elem.$Validate !== undefined) {
                elem.$Validate.text(elem.sValidate_Text);
            }
        }
    }

    //#endregion

    //#region Конвертировать значение в number

    /**
    * Конвертировать значение в number
    * @param value значение
          */
    export function Convert_ToNumber(value: string) {
        var return_value = value.reHIDE(new RegExp(",", 'g'), ".").reHIDE(new RegExp(',', 'g'), '.').reHIDE(/\s+/g, '');
        return Number(return_value);
    }

    //#endregion

    //#region Удаление класса ошибки валидации

    /**
    * Удаление класса ошибки валидации
    * @param elem данные
      */
    export function Validation_Error_Del(elem: iValidation_Error_Del) {
        if (elem.$Elem.hasClass(сValidate_Error)) {

            if (elem.$Elem != undefined && elem.$Elem.length > 0) {
                elem.$Elem.removeClass(сValidate_Error);
            }

            if (elem.$Validate != undefined && elem.$Validate.length > 0) {
                elem.$Validate.text("");
            }
        }
    }

    //#endregion

    //#region Ввод только положительного числа

    /**
     * Обработка ввода - позволить ввод только положительных цифр
     * @param elem
     */
    export function Keypress_NumOnlyPlus(elem: iKeypress_NumOnlyPlus) {
        elem.$Elem.keypress(function (e) {
            let verified: boolean = false;
            let Symbol_Max = 30;
            if (elem.nSymbol_Max !== undefined && elem.nSymbol_Max > 0) {
                Symbol_Max = elem.nSymbol_Max;
            }

            if (elem.$Elem.val().length > Symbol_Max) {
                verified = true;
            }
            else {
                verified = ((e.which === 8 || e.which === undefined || e.which === 0) ? null : String.fromCharCode(e.which).match(/[^0-9,.]/) as any);
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

    //#endregion

    //#region Методы для даты и времени - Добавление макси времени, Проверка значения на время, Проверка значения на дату

    /**
     * Добавление выпадающего списка даты + маска даты
     * @param model Данные
     */
    export function JQueryUI_Datapicker_With_InputMask(model: iJQueryUI_Datapicker_With_InputMask) {
        if (model.Elem.datepicker !== undefined) {

            function Date_OnComplete_Func() {
                if (model.Elem.val !== undefined && Check_Date(model.Elem.val())) {
                    model.Elem.removeClass(сValidate_Error);
                    if (model.Callback_Select !== undefined && typeof model.Callback_Select == "function") {
                        model.Callback_Select();
                    }
                } else {
                    model.Elem.addClass(сValidate_Error);
                }
            }

            let yearRange = model.yearRange;
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
                    let item_Trigger = model.Elem.parent().find(".ui-datepicker-trigger");
                    if (item_Trigger != undefined && item_Trigger.length > 0) {
                        item_Trigger.attr("src", "/_I/icon-date-active.png");
                    }

                },
                onClose: function () {
                    let item_Trigger = model.Elem.parent().find(".ui-datepicker-trigger");
                    if (item_Trigger != undefined && item_Trigger.length > 0) {
                        item_Trigger.attr("src", "/_I/icon-date.png");
                    }
                }
            });

            let elem_Trigger = model.Elem.parent().find(".ui-datepicker-trigger");
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
                    model.Elem.addClass(сValidate_Error);
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

    /**
     * Добавление маски даты
     * @param elem элемент к которому добавляются маски и проверки на дату
     */
    export function InputMask_Date_Add(elem: JQuery) {
        if (elem.inputmask !== undefined) {
            elem.inputmask("99.99.9999", { "HIDEholder": "дд.мм.гггг" });
        }
        else {
            console.log("HIDE_B_Common > InputMask_Date_Add > маска не добавлена, т.к. модуль inputmask не подключен");
        }
    }

    /**
     * Добавление маски времени
     * @param elem элемент к которому добавляются маски и проверки на время
     */
    export function InputMask_Time_Add(elem: JQuery) {
        if (elem.inputmask !== undefined) {
            elem.inputmask("99:99", {
                onincomplete: function () {
                    elem.addClass(сValidate_Error);
                },
                oncomplete: function () {
                    if (elem.val !== undefined && Check_Time(elem.val())) {
                        elem.removeClass(сValidate_Error);
                    } else {
                        elem.addClass(сValidate_Error);
                    }
                }
            });
        }
        else {
            console.log("HIDE_B_Common > InputMask_Date_Add > маска не добавлена, т.к. модуль inputmask не подключен");
        }
    }

    /**
    * Проверка значения на время
    * @param elem значение
    */
    export function Check_Time(value: string): boolean {
        var regexp = /([01][0-9]|[02][0-3]):[0-5][0-9]/;
        return (value.search(regexp) >= 0) ? true : false;
    }


    /**
     * Проверка элемента с временем на валидность
     * @param model
     */
    export function Check_TimePicker(model: iCheck_Common): boolean {
        var regexp = /([01][0-9]|[02][0-3]):[0-5][0-9]/;
        return Check_Picker({
            Elem: model.Elem,
            Validate_Elem: model.Validate_Elem,
            Validate_Text: model.Validate_Text,
            Value: model.Value,
            regexp: regexp
        });
    }

    /**
     * Проверка значения на дату
     * @param value значение
     */
    export function Check_Date(value: string): boolean {
        var regexp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/gm;
        return (value.search(regexp) >= 0) ? true : false;
    }

    /**
     * Проверка элемента с датой на валидность
     * @param model данные
     */
    export function Check_DatePicker(model: iCheck_Common): boolean {
        var regexp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/gm;
        return Check_Picker({
            Elem: model.Elem,
            Validate_Elem: model.Validate_Elem,
            Validate_Text: model.Validate_Text,
            Value: model.Value,
            regexp: regexp
        });
    }

    //#endregion

    //#region Блок ожидания - Добавить блок ожидания подгрузки данных, Удалить блок ожидания подгрузки данных

    interface iModal_Loading {
        /**
      * Элемент
      */
        Elem: JQuery;
    }


    /**
    * Добавить блок ожидания подгрузки данных
    * @param elem Элемент
    */
    export function Modal_Loading_Show(elem: iModal_Loading) {
        if (elem.Elem != undefined) {
            elem.Elem.append(`
        <div class="HIDE-loading_block">
            <div class="HIDE-loading_block-bg"></div>
            <div class="HIDE-loading_block-img"></div>
        </div>`
            );

            elem.Elem.find(".HIDE-loading_block").animate({
                opacity: "1"
            }, 500, function () {
            });
        }
        else {
            console.log("HIDE_B_Common > Modal_Loading_Show > Главный элемент не объявлен");
        }
    }

    /**
     * Удалить блок ожидания подгрузки данных
     * @param elem Элемент
     */
    export function Modal_Loading_Hide(elem: iModal_Loading) {
        if (elem.Elem != undefined) {
            elem.Elem.find(".HIDE-loading_block").remove();
        }
        else {
            console.log("HIDE_B_Common > Modal_Loading_Hide > Главный элемент не объявлен");
        }
    }

    //#endregion 

    //#region Генерация guid

    /**
     * Генерация guid
     */
    export function Guid_Generate(): string {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return `${s4()}${s4()}-${s4()}-${s4()}-${s4()}-${s4()}${s4()}${s4()}`;
    }

    //#endregion
}