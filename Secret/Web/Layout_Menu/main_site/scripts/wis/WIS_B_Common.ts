/* Компонент WIS_B_Common 1.22.1.6  */
/* Требуемые модули: jquery */
/* Поддержка браузеров: IE8+ и остальные браузеры. */
/* Автор: Инфографика (Висерв) - https://wiserv.ru */

/**
 * Общие методы
 */
module WIS_B_Common {

    //#region Переменные и интерфейсы

    /**
    * Класс ошибки при валидации
    */
    export const cValidate_Error = "wis-jscss-validate-error";

    /**
     * Текст ошибки, когда ничего не выбрано
     * */
    export const sError_ComboBoxEmpty = "Не выбрано";

    const sError_Space = "Поле не может содержать пробелы";

    const sError_Incorrectly = "Заполнено неверно";

    const sError_Empty = "Не заполнено";

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

        /**
         * Сообщение об ошибке (недопустимое количество символов)
         * */
        s_Error_Length?: string;

        /**
         * Сообщение об ошибке (пусто)
         * */
        s_Error_Empty?: string;

        /**
         * Сообщение об ошибке (не может содержать пробелы)
         * */
        s_Error_Space?: string;
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

        /**
         * Сообщение об ошибке (неверное значение)
         * */
        s_Error_Incorrectly?: string;
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

        /**
         * Сообщение об ошибке (неверное значение)
         * */
        s_Error_Incorrectly?: string;

        /**
         * Сообщение об ошибке (пусто)
         * */
        s_Error_Empty?: string;

        /**
         * Сообщение об ошибке (не может содержать пробелы)
         * */
        s_Error_Space?: string;
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

    /**
     * Проверка содержания пробелов
     * */
    interface I_Check_Space {

        /**
        * Элемент - элемент которого проверяют
        */
        $Elem: JQuery;

        /**
        * Элемент - элемент валидации (для вывода сообщения об ошибке)
        */
        $Validate: JQuery;

        /**
         * Сообщение об ошибке (не может содержать пробелы)
         * */
        s_Error_Space?: string;
    }

    //#endregion

    //#region Проверка количества символов, Проверка содержания пробелов


    /**
     * Проверка содержания пробелов
     * @param data Данные
     */
    function _Check_Space(data: I_Check_Space) {
        if (data.$Elem != undefined && data.$Elem != null) {
            let value: string = data.$Elem.val();
            let value_new: string = value.replace(/\s+/g, '');

            if (value.length != value_new.length) {
                data.$Elem.addClass(cValidate_Error);
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
                data.$Elem.removeClass(cValidate_Error);
                if (data.$Validate != undefined && data.$Validate != null) {
                    data.$Validate.text("");
                }
                return true;
            }
        }

        return false;
    }

    /**
    * Проверка количества символов
    * @param data данные
    * @param Flag_Check True - проверка удачна. False - содержит ошибки
    */
    function _Check_Length(data: iCheck_Length, Flag_Check: boolean): boolean {
        if (!Flag_Check) {
            let value: string = data.$Elem.val();
            if (data.$Elem !== undefined) {
                data.$Elem.addClass(cValidate_Error);
            }

            if (value.length > 0) {
                if (data.$Validate != undefined && data.$Validate != null) {
                    if (data.s_Error_Length != undefined) {
                        data.$Validate.text(data.s_Error_Length);
                    }
                    else {
                        data.$Validate.text(`Длина поля '${data.sFieldName}' должна быть от ${data.nMin} до ${data.nMax} символов.`);
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
        } else {
            if (data.$Validate != undefined && data.$Validate != null) data.$Validate.text("");
            if (data.$Elem !== undefined) {
                data.$Elem.removeClass(cValidate_Error);
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
        let Flag_Check = !(elem.$Elem.val().replace(/\s+/g, '').length > elem.nMax || elem.$Elem.val().replace(/\s+/g, '').length < elem.nMin);
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
    * @param data данные
    */
    export function Check_Phone(data: iCheck_Phone): boolean {
        let val = data.$Elem.val().replace("+", "").replace(/\s+/g, "").replace(/-/g, "").replace(/_/g, "").replace("(", "").replace(")", "");
        if (val.length == 11) {
            if (data.$Elem !== undefined) {
                data.$Elem.removeClass(cValidate_Error);
            }
            if (data.$Validate != undefined && data.$Validate != null) {
                data.$Validate.text("");
            }
            return true;
        }
        else {
            if (data.$Elem !== undefined) {
                data.$Elem.addClass(cValidate_Error);
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

    /**
    * Проверка корректности электронной почты
    * @param data данные
    */
    export function Check_Email(data: iCheck_Email): boolean {
        let flag: boolean = _Check_Space({
            $Elem: data.$Elem,
            $Validate: data.$Validate,
            s_Error_Space : data.s_Error_Space
        });
        if (flag) {
            let val = data.$Elem.val();
            if (val.replace(/\s+/g, '').length > 0) {
                let re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                if (re.test(val)) {
                    if (data.$Elem !== undefined) {
                        data.$Elem.removeClass(cValidate_Error);
                    }
                    if (data.$Validate != undefined && data.$Validate != null) {
                        data.$Validate.text("");
                    }
                    return true;
                }
                else {
                    if (data.$Elem !== undefined) {
                        data.$Elem.addClass(cValidate_Error);
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
                    data.$Elem.addClass(cValidate_Error);
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


    /**
     * Проверка корректности пароля
     * @param data данные
     */
    export function Check_Password(data: iCheck_Length) {
        let flag: boolean = _Check_Space({
            $Elem: data.$Elem,
            $Validate: data.$Validate,
            s_Error_Space: data.s_Error_Space
        });
        if (flag) {
            return Check_Length_WithoutSpace(data);
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
                elem.$Elem.removeClass(cValidate_Error);
            }

            if (elem.$Validate !== undefined) {
                elem.$Validate.text("");
            }
        }
        else {
            if (elem.$Elem !== undefined) {
                elem.$Elem.addClass(cValidate_Error);
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
        var return_value = value.replace(new RegExp(",", 'g'), ".").replace(new RegExp(',', 'g'), '.').replace(/\s+/g, '');
        return Number(return_value);
    }

    //#endregion

    //#region Удаление класса ошибки валидации

    /**
    * Удаление класса ошибки валидации
    * @param elem данные
      */
    export function Validation_Error_Del(elem: iValidation_Error_Del) {
        if (elem.$Elem.hasClass(cValidate_Error)) {

            if (elem.$Elem != undefined && elem.$Elem.length > 0) {
                elem.$Elem.removeClass(cValidate_Error);
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
}