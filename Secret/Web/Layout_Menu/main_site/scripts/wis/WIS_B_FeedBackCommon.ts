/* Компонент wis-feedbackcommon  1.22.1.6  */
/* Требуемые модули: jquery */
/* Поддержка браузеров: IE9+ и остальные браузеры. */
/* Автор: ВебИнформСервис (Висерв) - https://wiserv.ru/ */

/**
 * Форма обратной связи (общая)
 */
module WIS_B_FeedBackCommon {

    //#region Интерфейсы и константы

    /**
     * Ответ сервера - Успех
     * */
    export const sResponse_Success: string = "+";

    /**
     * Структура данных для входных параметров конструктора класса Init
     * */
    export interface iInit {

        /**
         * Элемент - Главный элемент
         * */
        $Elem: JQuery;

        /**
         * True - Включить отображение ошибок и логов при инициализации
         * */
        flag_isShow_Init_Log?: boolean;

        /**
         * True / undefined - Включить отображение уведомления после успешной отправки
         * */
        flag_isShow_Notification_Success?: boolean;

        /**
         * True / undefined - Включить отображение уведомления после отправки с ошибкой
         * */
        flag_isShow_Notification_Error?: boolean;

        /**
         * True - выключить асинхронную отправку
         * */
        flag_isNoAsync?: boolean;

        /**
         * Функция - генерирование json данных для отправки
         * */
        fData_Json?(list: Array<iItem>): any;

        /**
         * Функция в случае успеха (основной код)
         * @param data Ответ с сервера
         */
        fCallback_Success?(data: string): void;

        /**
         * Функция в случае успеха (после основного кода)
         * @param data Ответ с сервера
         */
        fCallback_Success_After?(data: string): void;

        /**
         * True - Вывести сообщение, где будут показаны переданные значения в запрос
         */
        flag_isTest?: boolean;

        /**
         * Данные которые отправляются (тестовое сообщение, при публикации удалить)
         * */
        fValue_Test?(list: Array<iItem>): string;

        /**
         * Тип запроса (GET/POST)
         */
        sMethod?: string;

        /**
         * Url
         */
        urlSubmit?: string;

        /**
         * Класс - элемента
         * */
        cItem?: string;

        /**
         * Класс - информационного блока
         * */
        cInfo?: string;

        /**
         * Заявка отправляется...
         * */
        sInfo_Sending?: string;

        /**
         * Сообщение успешно отправлено.
         * */
        sInfo_Success?: string;

        /**
         * Ошибка отправки сообщения, обратитесь к разрабочику или проверьте интернет-соединение.
         * */
        sInfo_Error?: string;

        /**
         * Ошибка отправки сообщения, обратитесь к разрабочику или проверьте интернет-соединение. (Ошибка отправки данных).
         * */
        sInfo_ErrorData?: string;

        /**
         * Необходимо принять соглашение и заполнить обязательные поля
         * */
        sInfo_ErrorFieldsAndUserAgreement?: string;

        /**
         * Необходимо заполнить обязательные поля
         * */
        sInfo_ErrorUserAgreement?: string;

        /**
         * Необходимо принять соглашение
         * */
        sInfo_ErrorFields?: string;

        /**
         * Значение выпадающего списка не выбрано
         * */
        s_InfoField_ErrorComboBoxEmpty?: string;

        /**
         * Поле не может содержать пробелы
         * */
        s_InfoField_ErrorSpace?: string;

        /**
         * Поле заполнено неверно
         * */
        s_InfoField_ErrorIncorrectly?: string;

        /**
         * Поле не заполнено
         * */
        s_InfoField_ErrorEmpty?: string;

        /**
         * Поле неверное количество символов
         * */
        s_InfoField_ErrorLength?: string;

        /**
         * Класс - информационного блока > текст
         * */
        cInfo_Block_Text?: string;

        /**
         * Класс - информационного блока > текст ошибка
         * */
        cInfo_Block_TextError?: string;

        /**
         * Класс - информационного блока > текст прелоадер
         * */
        cInfo_Block_TextLoader?: string;

        /**
         * Класс - информационного блока > Текст > Изображение
         * */
        cInfo_Block_Text_Image?: string;

        /**
         * Класс - кнопки отправки данных
         * */
        cSubmit?: string;

        /**
         * Класс - блока для вывода ошибки
         * */
        cError?: string;

    }

    /**
    * Структура данных для конструктора контекста Init
    * */
    interface iContext extends iInit {

        /**
         * Наименование поля (для вывода сообщений, когда требуется количество символов)
         * */
        data_wis_rule_name: string;

        /**
         * Обязательное поле
         * */
        data_wis_rule_required: string;

        /**
         * Проверка email
         * */
        data_wis_rule_email: string;

        /**
         * Проверка телефона
         * */
        data_wis_rule_phone: string;

        /**
         * Проверка выпадающего списка по номеру (Если value меньше нуля, то считается элемент не выбран)
         * */
        data_wis_rule_select_num: string;

        /**
         * Минимальное количество символов
         * */
        data_wis_rule_minlength: string;

        /**
         * Максимальное количество символов
         * */
        data_wis_rule_maxlength: string;

        /**
         * True - флажок пользовательского соглашения
         * */
        data_wis_rule_useragreement: string;

        /**
         * Список элементов формы
         * */
        aItems: Array<iItem>;

        /**
        * Элементы
        */
        $Items: JQuery;

        /**
          * Элемент - информация
          */
        $Info: JQuery;


        /**
         * Элемент - кнопка "Отправить заявку"
         */
        $Submit: JQuery;
    }

    /**
     * Элемент
     * */
    export interface iItem {

        /**
         * Элемент
         * */
        $Elem: JQuery;

        /**
         * Элемент для ошибки
         * */
        $Error: JQuery;

        /**
         * Наименование тега
         * */
        sTagName: string;

        /**
         * Атрибут - type
         * */
        sAttr_Type: string;

        /**
         * Атрибут - name
         * */
        sAttr_Name: string;

        /**
         * Наименование поля
         * */
        sName: string;

        /**
         * True - Обязательное поле
         * */
        flag_isRequired: boolean;

        /**
         * True - Проверка email
         * */
        flag_isEmail: boolean;

        /**
         * True - Проверка телефон
         * */
        flag_isPhone: boolean;

        /**
         * True - Проверка выпадающего списка по номеру (Если value меньше нуля, то считается элемент не выбран)
         * */
        flag_isSelectNum: boolean;

        /**
         * Минимальное количество символов
         * */
        nMinLength: number;

        /**
         * Максимальное количество символов
         * */
        nMaxLength: number;

        /**
        * True - проверка на вызов функции Check_Length_Trim
        * */
        flag_Check_Length_Trim: boolean;

        /**
         * True - проверка на вызов функции Check_Email
         * */
        flag_Check_Email: boolean;

        /**
         * True - проверка на вызов функции Check_Phone
         * */
        flag_Check_Phone: boolean;

        /**
         * True - проверка на вызов функции Check_ComboBox_Str
         * */
        flag_Check_ComboBox_Str: boolean;

        /**
         * True - проверка на вызов функции Check_ComboBox_Num
         * */
        flag_Check_ComboBox_Num: boolean;

        /**
         * True - это флажок
         * */
        flag_isCheckbox: boolean;

        /**
         * True - это флажок пользовательского соглашения
         * */
        flag_isCheckboxUserAgreement: boolean;

        /**
         * True - это выпадающий список
         * */
        flag_isCombobox: boolean;
    }

    /**
     * Получение элемента по значению атрибута name
     * */
    interface iGet {

        /**
         * Значение атрибута name
         * */
        sAttr_Name: string;

        /**
         * Список элементов
         * */
        aItems: Array<iItem>;
    }

    //#endregion

    /**
     * Класс
     * */
    export class Init {

        //#region Инициализация

        /**
         * Конструктор
         * @param model данные для инициализации
         */
        constructor(model: iInit) {
            if (model.$Elem != undefined && model.$Elem.length > 0) {

                //#region Значения по умолчанию

                let Item_Class = "wis-js-feedback_form-fields-elem";
                if (model.cItem != undefined && model.cItem.length > 0) {
                    Item_Class = model.cItem;
                }

                let Flag_isNoAsync = false;
                if (model.flag_isNoAsync != undefined) {
                    Flag_isNoAsync = model.flag_isNoAsync;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        let Flag_isNoAsync_Str: string = model.$Elem.data("wis-no_async");
                        if (Flag_isNoAsync_Str != undefined && Flag_isNoAsync_Str.toString().toLowerCase() == "true") {
                            Flag_isNoAsync = true;
                        }
                    }
                }

                let Method_Str = "";
                if (model.sMethod != undefined && model.sMethod.length > 0) {
                    Method_Str = model.sMethod;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        Method_Str = model.$Elem.attr("method");
                        if (Method_Str != undefined) {
                            Method_Str = Method_Str.toString().toUpperCase();
                        }
                    }
                }

                let Submit_Url = "";
                if (model.urlSubmit != undefined && model.urlSubmit.length > 0) {
                    Submit_Url = model.urlSubmit;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        Submit_Url = model.$Elem.attr("action");
                        if (Submit_Url != undefined) {
                            Submit_Url = Submit_Url.toString().toUpperCase();
                        }
                    }
                }

                let Info_Class = "wis-js-feedback_form-info";
                if (model.cInfo != undefined && model.cInfo.length > 0) {
                    Info_Class = model.cInfo;
                }

                let Submit_Class = "wis-js-feedback_form-submit";
                if (model.cSubmit != undefined && model.cSubmit.length > 0) {
                    Submit_Class = model.cSubmit;
                }

                let Error_Class = "wis-js-error";
                if (model.cError != undefined && model.cError.length > 0) {
                    Error_Class = model.cError;
                }

                let Info_Block_Text_Class = "wis-feedback_form-info-text";
                if (model.cInfo_Block_Text != undefined && model.cInfo_Block_Text.length > 0) {
                    Info_Block_Text_Class = model.cInfo_Block_Text;
                }

                let Info_Block_TextError_Class = "wis-feedback_form-info-text_error";
                if (model.cInfo_Block_TextError != undefined && model.cInfo_Block_TextError.length > 0) {
                    Info_Block_TextError_Class = model.cInfo_Block_TextError;
                }

                let Info_Block_TextLoader_Class = "wis-feedback_form-info-text_loader";
                if (model.cInfo_Block_TextLoader != undefined && model.cInfo_Block_TextLoader.length > 0) {
                    Info_Block_TextLoader_Class = model.cInfo_Block_TextLoader;
                }

                let Info_Block_Text_Image_Class = "wis-feedback_form-info-text-img";
                if (model.cInfo_Block_Text_Image != undefined && model.cInfo_Block_Text_Image.length > 0) {
                    Info_Block_Text_Image_Class = model.cInfo_Block_Text_Image;
                }

                let Info_Sending_Str = "Заявка отправляется...";
                if (model.sInfo_Sending != undefined && model.sInfo_Sending.length > 0) {
                    Info_Sending_Str = model.sInfo_Sending;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        let sData_Info_Sending: string = model.$Elem.data("wis-info_sending");
                        if (sData_Info_Sending != undefined) {
                            Info_Sending_Str = sData_Info_Sending;
                        }
                    }
                }

                let Info_Success_Str = "Сообщение успешно отправлено.";
                if (model.sInfo_Success != undefined && model.sInfo_Success.length > 0) {
                    Info_Success_Str = model.sInfo_Success;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        let sData_Info_Success: string = model.$Elem.data("wis-info_success");
                        if (sData_Info_Success != undefined) {
                            Info_Success_Str = sData_Info_Success;
                        }
                    }
                }


                let Info_Error_Str = "Ошибка отправки сообщения, обратитесь к разрабочику или проверьте интернет-соединение.";
                if (model.sInfo_Error != undefined && model.sInfo_Error.length > 0) {
                    Info_Error_Str = model.sInfo_Error;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        let sData_Info_Error: string = model.$Elem.data("wis-info_error");
                        if (sData_Info_Error != undefined) {
                            Info_Error_Str = sData_Info_Error;
                        }
                    }
                }

                let Info_ErrorData_Str = "Ошибка отправки сообщения, обратитесь к разрабочику или проверьте интернет-соединение. (Ошибка отправки данных).";
                if (model.sInfo_ErrorData != undefined && model.sInfo_ErrorData.length > 0) {
                    Info_ErrorData_Str = model.sInfo_ErrorData;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        let s_Data_Info_ErrorData: string = model.$Elem.data("wis-info_errordata");
                        if (s_Data_Info_ErrorData != undefined) {
                            Info_ErrorData_Str = s_Data_Info_ErrorData;
                        }
                    }
                }

                let Info_ErrorFieldsAndUserAgreement_Str = "Необходимо принять соглашение и заполнить обязательные поля";
                if (model.sInfo_ErrorFieldsAndUserAgreement != undefined && model.sInfo_ErrorFieldsAndUserAgreement.length > 0) {
                    Info_ErrorFieldsAndUserAgreement_Str = model.sInfo_ErrorFieldsAndUserAgreement;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        let s_Data_Info_ErrorFieldsAndUserAgreement: string = model.$Elem.data("wis-info_errorfieldsanduseragreement");
                        if (s_Data_Info_ErrorFieldsAndUserAgreement != undefined) {
                            Info_ErrorFieldsAndUserAgreement_Str = s_Data_Info_ErrorFieldsAndUserAgreement;
                        }
                    }
                }

                let Info_ErrorUserAgreement_Str = "Необходимо принять соглашение";
                if (model.sInfo_ErrorUserAgreement != undefined && model.sInfo_ErrorUserAgreement.length > 0) {
                    Info_ErrorUserAgreement_Str = model.sInfo_ErrorUserAgreement;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        let s_Data_Info_ErrorUserAgreement: string = model.$Elem.data("wis-info_erroruseragreement");
                        if (s_Data_Info_ErrorUserAgreement != undefined) {
                            Info_ErrorUserAgreement_Str = s_Data_Info_ErrorUserAgreement;
                        }
                    }
                }

                let Info_ErrorFields_Str = "Необходимо заполнить обязательные поля";
                if (model.sInfo_ErrorFields != undefined && model.sInfo_ErrorFields.length > 0) {
                    Info_ErrorFields_Str = model.sInfo_ErrorFields;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        let s_Data_Info_ErrorFields: string = model.$Elem.data("wis-info_errorfields");
                        if (s_Data_Info_ErrorFields != undefined) {
                            Info_ErrorFields_Str = s_Data_Info_ErrorFields;
                        }
                    }
                }

                let str_InfoField_ErrorComboBoxEmpty = WIS_B_Common.sError_ComboBoxEmpty;
                if (model.s_InfoField_ErrorComboBoxEmpty != undefined && model.s_InfoField_ErrorComboBoxEmpty.length > 0) {
                    str_InfoField_ErrorComboBoxEmpty = model.s_InfoField_ErrorComboBoxEmpty;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        let s_Data_InfoField_ErrorComboBoxEmpty: string = model.$Elem.data("wis-infofield_errorcomboboxempty");
                        if (s_Data_InfoField_ErrorComboBoxEmpty != undefined) {
                            str_InfoField_ErrorComboBoxEmpty = s_Data_InfoField_ErrorComboBoxEmpty;
                        }
                    }
                }

                let str_InfoField_ErrorSpace = undefined;
                if (model.s_InfoField_ErrorSpace != undefined && model.s_InfoField_ErrorSpace.length > 0) {
                    str_InfoField_ErrorSpace = model.s_InfoField_ErrorSpace;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        let s_Data_InfoField_ErrorSpace: string = model.$Elem.data("wis-infofield_errorspace");
                        if (s_Data_InfoField_ErrorSpace != undefined) {
                            str_InfoField_ErrorSpace = s_Data_InfoField_ErrorSpace;
                        }
                    }
                }

                let str_InfoField_ErrorIncorrectly = undefined;
                if (model.s_InfoField_ErrorIncorrectly != undefined && model.s_InfoField_ErrorIncorrectly.length > 0) {
                    str_InfoField_ErrorIncorrectly = model.s_InfoField_ErrorIncorrectly;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        let s_Data_InfoField_ErrorIncorrectly: string = model.$Elem.data("wis-infofield_errorincorrectly");
                        if (s_Data_InfoField_ErrorIncorrectly != undefined) {
                            str_InfoField_ErrorIncorrectly = s_Data_InfoField_ErrorIncorrectly;
                        }
                    }
                }

                let str_InfoField_ErrorEmpty = undefined;
                if (model.s_InfoField_ErrorEmpty != undefined && model.s_InfoField_ErrorEmpty.length > 0) {
                    str_InfoField_ErrorEmpty = model.s_InfoField_ErrorEmpty;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        let s_Data_InfoField_ErrorEmpty: string = model.$Elem.data("wis-infofield_errorempty");
                        if (s_Data_InfoField_ErrorEmpty != undefined) {
                            str_InfoField_ErrorEmpty = s_Data_InfoField_ErrorEmpty;
                        }
                    }
                }

                let str_InfoField_ErrorLength = undefined;
                if (model.s_InfoField_ErrorLength != undefined && model.s_InfoField_ErrorLength.length > 0) {
                    str_InfoField_ErrorLength = model.s_InfoField_ErrorLength;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        let s_Data_InfoField_ErrorLength: string = model.$Elem.data("wis-infofield_errorlength");
                        if (s_Data_InfoField_ErrorLength != undefined) {
                            str_InfoField_ErrorLength = s_Data_InfoField_ErrorLength;
                        }
                    }
                }

                //#endregion Значения по умолчанию

                this._Context = {
                    $Elem: model.$Elem,
                    flag_isShow_Notification_Success: model.flag_isShow_Notification_Success,
                    flag_isShow_Notification_Error: model.flag_isShow_Notification_Error,
                    flag_isNoAsync: Flag_isNoAsync,
                    flag_isTest: model.flag_isTest,
                    fValue_Test: model.fValue_Test,
                    fData_Json: model.fData_Json,
                    sMethod: Method_Str,
                    urlSubmit: Submit_Url,
                    fCallback_Success: model.fCallback_Success,
                    fCallback_Success_After: model.fCallback_Success_After,
                    cItem: Item_Class,
                    cInfo: Info_Class,
                    cInfo_Block_Text: Info_Block_Text_Class,
                    cInfo_Block_TextError: Info_Block_TextError_Class,
                    cInfo_Block_TextLoader: Info_Block_TextLoader_Class,
                    cInfo_Block_Text_Image: Info_Block_Text_Image_Class,
                    sInfo_Sending: Info_Sending_Str,
                    sInfo_Success: Info_Success_Str,
                    sInfo_Error: Info_Error_Str,
                    sInfo_ErrorData: Info_ErrorData_Str,
                    sInfo_ErrorFieldsAndUserAgreement: Info_ErrorFieldsAndUserAgreement_Str,
                    sInfo_ErrorUserAgreement: Info_ErrorUserAgreement_Str,
                    sInfo_ErrorFields: Info_ErrorFields_Str,

                    s_InfoField_ErrorComboBoxEmpty: str_InfoField_ErrorComboBoxEmpty,
                    s_InfoField_ErrorSpace: str_InfoField_ErrorSpace,
                    s_InfoField_ErrorIncorrectly: str_InfoField_ErrorIncorrectly,
                    s_InfoField_ErrorEmpty: str_InfoField_ErrorEmpty,
                    s_InfoField_ErrorLength: str_InfoField_ErrorLength,

                    cSubmit: Submit_Class,
                    cError: Error_Class,
                    data_wis_rule_name: "wis-rule-name",
                    data_wis_rule_required: "wis-rule-required",
                    data_wis_rule_email: "wis-rule-email",
                    data_wis_rule_phone: "wis-rule-phone",
                    data_wis_rule_select_num: "wis-rule-select_num",
                    data_wis_rule_minlength: "wis-rule-minlength",
                    data_wis_rule_maxlength: "wis-rule-maxlength",
                    data_wis_rule_useragreement: "wis-rule-useragreement",
                    aItems: [],
                    $Items: undefined,
                    $Info: undefined,
                    $Submit: undefined

                };

                if (model.flag_isShow_Init_Log) {
                    console.log("WIS_B_FeedBackCommon init");
                }
                this._Refresh();
            }
            else {
                if (model.flag_isShow_Init_Log) {
                    console.log("WIS_B_FeedBackCommon no init. Главный элемент не определен");
                    //WIS_B_Notification.Show({ Value_html: "Обратная связь - Главный элемент не определен", Type: WIS_B_Notification.eType.Warning });
                }
            }
        }

        /**
         * Данные инициализации
         * */
        private _Context: iContext;

        //#endregion Инициализация

        /**
         * Стартовая инициализация, Добавление обработчиков
         * */
        private _Refresh() {
            let class_this = this;

            if (class_this._Context.$Elem != undefined && class_this._Context.$Elem.length > 0) {

                //#region Инициализация

                /**
                * Элементы
                */
                class_this._Context.$Items = class_this._Context.$Elem.find(`.${class_this._Context.cItem}`);

                /**
                  * Элемент - информация
                  */
                class_this._Context.$Info = class_this._Context.$Elem.find(`.${class_this._Context.cInfo}`);;

                /**
                 * Элемент - кнопка "Отправить заявку"
                 */
                class_this._Context.$Submit = class_this._Context.$Elem.find(`.${class_this._Context.cSubmit}`);

                //#endregion

                //#region Обработка обязательных полей

                class_this._Context.$Items.each(function () {

                    //TODO: WIS - Добавить возможность указывать собственное сообщение (для каждого поля)

                    //Инициализация
                    let elem_this = $(this);
                    let item_Elem = elem_this.find("input, textarea, select");
                    let error_Elem = elem_this.find(`.${class_this._Context.cError}`);

                    if (item_Elem != undefined && item_Elem.length > 0) {

                        //Получение значений
                        let tagname: string = item_Elem.get()[0].tagName.toString().toLowerCase();
                        let attr_type: string = item_Elem.attr("type");
                        if (attr_type != undefined && attr_type.length > 0) {
                            attr_type = attr_type.toString().toLowerCase();
                        }
                        let attr_name: string = item_Elem.attr("name");

                        let name: string = item_Elem.data(class_this._Context.data_wis_rule_name);

                        let flag_required = false;
                        let flag_required_Str: string = item_Elem.data(class_this._Context.data_wis_rule_required);
                        if (flag_required_Str != undefined && flag_required_Str.toString().toLowerCase() == "true") {
                            flag_required = true;
                        }

                        let flag_email = false;
                        let flag_email_Str: string = item_Elem.data(class_this._Context.data_wis_rule_email);
                        if (flag_email_Str != undefined && flag_email_Str.toString().toLowerCase() == "true") {
                            flag_email = true;
                        }

                        let flag_phone = false;
                        let flag_phone_Str: string = item_Elem.data(class_this._Context.data_wis_rule_phone);
                        if (flag_phone_Str != undefined && flag_phone_Str.toString().toLowerCase() == "true") {
                            flag_phone = true;
                        }

                        let flag_select_num = false;
                        let flag_select_num_Str: string = item_Elem.data(class_this._Context.data_wis_rule_select_num);
                        if (flag_select_num_Str != undefined && flag_select_num_Str.toString().toLowerCase() == "true") {
                            flag_select_num = true;
                        }

                        let flag_useragreement = false;
                        let flag_useragreement_Str: string = item_Elem.data(class_this._Context.data_wis_rule_useragreement);
                        if (flag_useragreement_Str != undefined && flag_useragreement_Str.toString().toLowerCase() == "true") {
                            flag_useragreement = true;
                        }

                        let minlength: number = item_Elem.data(class_this._Context.data_wis_rule_minlength);
                        if (minlength == undefined || minlength < 0) {
                            minlength = 0;
                        }

                        let maxlength: number = item_Elem.data(class_this._Context.data_wis_rule_maxlength);
                        if (maxlength == undefined || maxlength < 0) {
                            maxlength = 0;
                        }

                        if (minlength > 0 && maxlength <= 0) {
                            maxlength = 1000;
                        }

                        // Проверка для добавления функций
                        let flag_Check_Length_Trim = false;
                        let flag_Check_Email = false;
                        let flag_Check_Phone = false;
                        let flag_Check_ComboBox_Str = false;
                        let flag_Check_ComboBox_Num = false;
                        let flag_isCheckbox = false;
                        let flag_isCombobox = false;

                        if (attr_type == "text" ||
                            attr_type == "password" ||
                            attr_type == "email" ||
                            attr_type == "file" ||
                            tagname == "textarea") {
                            if (flag_required) {
                                if (minlength <= 0) {
                                    minlength = 1;
                                }
                                if (maxlength <= 0) {
                                    maxlength = 1000;
                                }

                                flag_Check_Length_Trim = true;
                            }
                            else {
                                if (minlength > 0 || maxlength > 0) {
                                    if (maxlength <= 0) {
                                        maxlength = 1000;
                                    }

                                    flag_Check_Length_Trim = true;
                                }
                            }
                        }

                        if (flag_email && (attr_type == "text" || attr_type == "email" || tagname == "textarea")) {
                            flag_Check_Email = true;
                        }

                        if (flag_phone && (attr_type == "text" || tagname == "textarea")) {
                            flag_Check_Phone = true;
                        }

                        if (flag_required && (!flag_select_num && tagname == "select")) {
                            flag_Check_ComboBox_Str = true;
                        }

                        if (flag_required && (flag_select_num && tagname == "select")) {
                            flag_Check_ComboBox_Num = true;
                        }

                        if (tagname == "input" && attr_type == "checkbox") {
                            flag_isCheckbox = true;
                        }

                        if (tagname == "select") {
                            flag_isCombobox = true;
                        }


                        //Добавление функций
                        if (flag_Check_Length_Trim) {
                            //Текст - Покидание фокуса
                            item_Elem.focusout(function () {
                                WIS_B_Common.Check_Length_Trim(
                                    {
                                        $Elem: item_Elem,
                                        sFieldName: name,
                                        nMin: minlength,
                                        nMax: maxlength,
                                        $Validate: error_Elem,
                                        s_Error_Empty: class_this._Context.s_InfoField_ErrorEmpty,
                                        s_Error_Length: class_this._Context.s_InfoField_ErrorLength,
                                        s_Error_Space: class_this._Context.s_InfoField_ErrorSpace,
                                    });
                            });

                            //Текст - убрать ошибку, когда идет ввод
                            item_Elem.keyup(function () {
                                WIS_B_Common.Validation_Error_Del(
                                    {
                                        $Elem: item_Elem,
                                        $Validate: error_Elem
                                    });
                            });
                        }

                        if (flag_Check_Email) {

                            //Покидание фокуса с "Эл. почта"
                            item_Elem.focusout(function () {
                                WIS_B_Common.Check_Email({
                                    $Elem: item_Elem,
                                    $Validate: error_Elem,
                                    s_Error_Empty: class_this._Context.s_InfoField_ErrorEmpty,
                                    s_Error_Incorrectly: class_this._Context.s_InfoField_ErrorIncorrectly,
                                    s_Error_Space: class_this._Context.s_InfoField_ErrorSpace
                                });
                            });

                            // Эл. почта - убрать ошибку, когда идет ввод
                            item_Elem.keyup(function () {
                                if ($(this).hasClass(WIS_B_Common.cValidate_Error)) {
                                    WIS_B_Common.Check_Email({
                                        $Elem: item_Elem,
                                        $Validate: error_Elem,
                                        s_Error_Empty: class_this._Context.s_InfoField_ErrorEmpty,
                                        s_Error_Incorrectly: class_this._Context.s_InfoField_ErrorIncorrectly,
                                        s_Error_Space: class_this._Context.s_InfoField_ErrorSpace
                                    });
                                }
                            });
                        }

                        if (flag_Check_Phone) {
                            // Телефон - добавить маску телефона (требуется плагин jquery.inputmask)
                            if (item_Elem.inputmask !== undefined) {
                                item_Elem.inputmask("mask",
                                    {
                                        "mask": "+7 (999) 999-99-99",
                                        "oncomplete": function () {
                                            WIS_B_Common.Validation_Error_Del(
                                                {
                                                    $Elem: item_Elem,
                                                    $Validate: error_Elem
                                                });
                                        },
                                        "onincomplete": function () {
                                            WIS_B_Common.Check_Phone({
                                                $Elem: item_Elem,
                                                $Validate: error_Elem,
                                                s_Error_Incorrectly: class_this._Context.s_InfoField_ErrorIncorrectly
                                            });
                                        }
                                    }
                                );
                            }

                            // Телефон - убрать ошибку, когда идет ввод
                            item_Elem.keyup(function () {
                                WIS_B_Common.Validation_Error_Del(
                                    {
                                        $Elem: item_Elem,
                                        $Validate: error_Elem
                                    });
                            });
                        }

                        if (flag_Check_ComboBox_Str) {

                            /**
                              * Город - Выбор значения
                              */
                            item_Elem.change(function () {
                                WIS_B_Common.Check_ComboBox_Str({
                                    $Elem: item_Elem,
                                    anyValue: item_Elem.find("option:selected").val(),
                                    sValidate_Text: class_this._Context.s_InfoField_ErrorComboBoxEmpty,
                                    $Validate: error_Elem,
                                });
                            });
                        }

                        if (flag_Check_ComboBox_Num) {

                            /**
                              * Город - Выбор значения
                              */
                            item_Elem.change(function () {
                                WIS_B_Common.Check_ComboBox_Num({
                                    $Elem: item_Elem,
                                    anyValue: item_Elem.find("option:selected").val(),
                                    sValidate_Text: class_this._Context.s_InfoField_ErrorComboBoxEmpty,
                                    $Validate: error_Elem
                                });
                            });
                        }


                        class_this._Context.aItems.push({
                            $Elem: item_Elem,
                            $Error: error_Elem,
                            sTagName: tagname,
                            sAttr_Type: attr_type,
                            sAttr_Name: attr_name,
                            sName: name,
                            flag_isRequired: flag_required,
                            flag_isEmail: flag_email,
                            flag_isPhone: flag_phone,
                            flag_isSelectNum: flag_select_num,
                            nMinLength: minlength,
                            nMaxLength: maxlength,
                            flag_Check_Length_Trim: flag_Check_Length_Trim,
                            flag_Check_Email: flag_Check_Email,
                            flag_Check_Phone: flag_Check_Phone,
                            flag_Check_ComboBox_Str: flag_Check_ComboBox_Str,
                            flag_Check_ComboBox_Num: flag_Check_ComboBox_Num,
                            flag_isCheckbox: flag_isCheckbox,
                            flag_isCheckboxUserAgreement: flag_useragreement,
                            flag_isCombobox: flag_isCombobox
                        });
                    }
                });

                //#endregion

                if (class_this._Context.$Elem.get()[0].tagName.toLowerCase() == "form") {

                    // Отправить заявку по событию submit
                    class_this._Context.$Elem.submit(function (e: JQueryEventObject) {
                        let check = class_this._Submit();

                        if (class_this._Context.flag_isNoAsync != undefined && class_this._Context.flag_isNoAsync) {
                            if (check) {
                                setTimeout(function () {
                                    //Когда отключена асинхронная отправка, то необходимо отключать кнопку только после отправки (иначе отправки не будет)
                                    //Отключение кнопки отправки (для исключения множественной отправки одинакового сообщения)
                                    class_this._Context.$Submit.prop("disabled", true);
                                }, 100);
                            }
                            else {
                                e.preventDefault();
                                return false;
                            }

                        }
                        else {
                            e.preventDefault();
                            return false;
                        }
                    });
                }
                else {

                    // Отправить заявку по событию click
                    class_this._Context.$Submit.click(function (e: JQueryEventObject) {
                        let check = class_this._Submit();

                        if (class_this._Context.flag_isNoAsync != undefined && class_this._Context.flag_isNoAsync) {
                            if (!check) {
                                e.preventDefault();
                                return false;
                            }
                        }
                        else {
                            e.preventDefault();
                            return false;
                        }
                    });
                }
            }
        }

        /** 
         * Отправить заявку 
         * */
        private _Submit(): boolean {
            let class_this = this;

            let valid = true;
            let valid_Fields = true;
            let valid_UserAgreement = true;
            let err_text: string = "";

            if (class_this._Context.aItems != undefined && class_this._Context.aItems.length > 0) {
                for (let elem of class_this._Context.aItems) {

                    //Проверка текста
                    if (elem.flag_Check_Length_Trim) {
                        if (!WIS_B_Common.Check_Length_Trim({
                            $Elem: elem.$Elem,
                            sFieldName: elem.sName,
                            nMin: elem.nMinLength,
                            nMax: elem.nMaxLength,
                            $Validate: elem.$Error,
                            s_Error_Empty: class_this._Context.s_InfoField_ErrorEmpty,
                            s_Error_Length: class_this._Context.s_InfoField_ErrorLength,
                            s_Error_Space: class_this._Context.s_InfoField_ErrorSpace
                        })) {
                            valid_Fields = false;
                            valid = false;
                        }
                    }

                    //Проверка эл. почты
                    if (elem.flag_isEmail) {
                        if (!WIS_B_Common.Check_Email({
                            $Elem: elem.$Elem,
                            $Validate: elem.$Error,
                            s_Error_Empty: class_this._Context.s_InfoField_ErrorEmpty,
                            s_Error_Incorrectly: class_this._Context.s_InfoField_ErrorIncorrectly,
                            s_Error_Space: class_this._Context.s_InfoField_ErrorSpace

                        })) {
                            valid_Fields = false;
                            valid = false;
                        }
                    }


                    //Проверка телефона
                    if (elem.flag_isPhone) {
                        if (!WIS_B_Common.Check_Phone({
                            $Elem: elem.$Elem,
                            $Validate: elem.$Error,
                            s_Error_Incorrectly: class_this._Context.s_InfoField_ErrorIncorrectly
                        })) {
                            valid_Fields = false;
                            valid = false;
                        }
                    }

                    //Проверка выпадающего списка (строка)
                    if (elem.flag_Check_ComboBox_Str) {

                        if (!WIS_B_Common.Check_ComboBox_Str({
                            $Elem: elem.$Elem,
                            anyValue: elem.$Elem.find("option:selected").val(),
                            sValidate_Text: class_this._Context.s_InfoField_ErrorComboBoxEmpty,
                            $Validate: elem.$Error
                        })) {
                            valid_Fields = false;
                            valid = false;
                        }
                    }

                    //Проверка выпадающего списка (цифра)
                    if (elem.flag_Check_ComboBox_Num) {

                        if (!WIS_B_Common.Check_ComboBox_Num({
                            $Elem: elem.$Elem,
                            anyValue: elem.$Elem.find("option:selected").val(),
                            sValidate_Text: class_this._Context.s_InfoField_ErrorComboBoxEmpty,
                            $Validate: elem.$Error
                        })) {
                            valid_Fields = false;
                            valid = false;
                        }
                    }

                    if (elem.flag_isCheckbox && elem.flag_isRequired) {
                        if (elem.flag_isCheckboxUserAgreement) {
                            if (!elem.$Elem.prop("checked")) {
                                valid_UserAgreement = false;
                                valid = false;
                            }
                        }
                        else {
                            if (!elem.$Elem.prop("checked")) {
                                valid_Fields = false;
                                valid = false;
                            }
                        }
                    }
                }
            }
            if (valid) {
                //Информирование пользователя о процессе отправки сообщения
                class_this._Context.$Info.html(
                    `<div class="${class_this._Context.cInfo_Block_Text} ${class_this._Context.cInfo_Block_TextLoader}">
                                <span class="${class_this._Context.cInfo_Block_Text_Image}"></span>
                                ${class_this._Context.sInfo_Sending}
                            </div>`);

                if (!(this._Context.flag_isNoAsync != undefined && this._Context.flag_isNoAsync)) {

                    //Отключение кнопки отправки (для исключения множественной отправки одинакового сообщения)
                    class_this._Context.$Submit.prop("disabled", true);

                    //Формирование данных, которые будут посланы на сервер
                    let data_json: any = {};

                    if (class_this._Context.fData_Json != undefined && typeof class_this._Context.fData_Json == "function") {
                        // Получение ручного data_json

                        data_json = class_this._Context.fData_Json(class_this._Context.aItems);

                        if (class_this._Context.flag_isTest) {

                            let data_send_str = "";
                            if (class_this._Context.fValue_Test != undefined && typeof class_this._Context.fValue_Test == "function") {
                                data_send_str = class_this._Context.fValue_Test(class_this._Context.aItems);
                            }

                            WIS_B_Notification.Show({
                                Value_html: data_send_str,
                                Type: WIS_B_Notification.eType.Warning
                            });
                        }
                    }
                    else {
                        //автоматическое преобразование в data_json

                        let data_send_str = "";

                        for (let elem of class_this._Context.aItems) {
                            if (elem.sAttr_Name != undefined) {
                                if (elem.flag_isCheckbox) {
                                    let elem_value = false;
                                    if (elem.$Elem.prop("checked")) {
                                        elem_value = true;
                                    }

                                    data_send_str += `${elem.sAttr_Name}: ${elem_value} `;
                                    data_json[`${elem.sAttr_Name}`] = elem_value;
                                }
                                else {
                                    data_send_str += `${elem.sAttr_Name}: ${elem.$Elem.val()} `;
                                    data_json[`${elem.sAttr_Name}`] = elem.$Elem.val();
                                }
                            }
                        }

                        //Получение UTM меток
                        let elem_UTM = WIS_B_UTM.Get();

                        data_json["wis_page"] = elem_UTM.wis_page;
                        data_json["utm_medium"] = elem_UTM.utm_medium;
                        data_json["utm_source"] = elem_UTM.utm_source;
                        data_json["utm_campaign"] = elem_UTM.utm_campaign;
                        data_json["block"] = elem_UTM.block;
                        data_json["utm_term"] = elem_UTM.utm_term;
                        data_json["utm_content"] = elem_UTM.utm_content;
                        data_json["position"] = elem_UTM.position;
                        data_json["keyword"] = elem_UTM.keyword;

                        //Данные которые отправляются (тестовое сообщение, при публикации удалить)
                        if (class_this._Context.flag_isTest) {
                            WIS_B_Notification.Show({
                                Value_html:
                                    `Данные которые отправляются (тестовое сообщение, при публикации удалить). 
                                 ${data_send_str}
                                 Страница отправки: ${elem_UTM.wis_page}`,
                                Type: WIS_B_Notification.eType.Warning
                            });
                        }
                    }

                    //Отправка данных на сервер
                    $.ajax({
                        type: class_this._Context.sMethod,
                        cache: false,
                        url: class_this._Context.urlSubmit,
                        data: data_json,
                        success: function (data: string) {
                            if (class_this._Context.fCallback_Success != undefined && typeof class_this._Context.fCallback_Success == "function") {
                                class_this._Context.fCallback_Success(data);
                            }
                            else {
                                //Включение кнопки отправки
                                class_this._Context.$Submit.prop("disabled", false);

                                if (data != undefined && data.length > 0 && data[0] == WIS_B_FeedBackCommon.sResponse_Success) {
                                    //Вывод сообщения об успешности отправки сообщения
                                    if (class_this._Context.flag_isShow_Notification_Success === undefined || class_this._Context.flag_isShow_Notification_Success) {
                                        WIS_B_Notification.Show({
                                            Value_html: class_this._Context.sInfo_Success
                                        });
                                    }

                                    //Формирование текста об успешности отправки сообщения
                                    err_text =
                                        `<div class="${class_this._Context.cInfo_Block_Text}">
                                            <span class="${class_this._Context.cInfo_Block_Text_Image}"></span>
                                            ${class_this._Context.sInfo_Success}
                                        </div>`;

                                    //Информирование пользователя об успешности отправки сообщения
                                    class_this._Context.$Info.html(err_text);
                                }
                                else {
                                    //Вывод сообщения об ошибке отправки сообщения (либо о непройденных проверках)
                                    if (class_this._Context.flag_isShow_Notification_Error === undefined || class_this._Context.flag_isShow_Notification_Error) {
                                        WIS_B_Notification.Show({
                                            Value_html: class_this._Context.sInfo_Error,
                                            Type: WIS_B_Notification.eType.Error
                                        });
                                    }

                                    //Формирование текста ошибки
                                    err_text =
                                        `<div class="${class_this._Context.cInfo_Block_Text} ${class_this._Context.cInfo_Block_TextError}">
                                            <span class="${class_this._Context.cInfo_Block_Text_Image}"></span>
                                            ${class_this._Context.sInfo_Error}
                                        </div>`;

                                    //Информирование пользователя об ошибке
                                    class_this._Context.$Info.html(err_text);
                                }
                            }

                            if (class_this._Context.fCallback_Success_After != undefined && typeof class_this._Context.fCallback_Success_After == "function") {
                                class_this._Context.fCallback_Success_After(data);
                            }
                        },
                        error: function () {
                            //Включение кнопки отправки
                            class_this._Context.$Submit.prop("disabled", false);

                            //Вывод сообщения об ошибке отправки сообщения
                            if (class_this._Context.flag_isShow_Notification_Error === undefined || class_this._Context.flag_isShow_Notification_Error) {
                                WIS_B_Notification.Show(
                                    {
                                        Value_html: class_this._Context.sInfo_ErrorData,
                                        Type: WIS_B_Notification.eType.Error
                                    });
                            }

                            //Формирование текста ошибки
                            err_text =
                                `<div class="${class_this._Context.cInfo_Block_Text} ${class_this._Context.cInfo_Block_TextError}">
                                        <span class="${class_this._Context.cInfo_Block_Text_Image}"></span>
                                        ${class_this._Context.sInfo_ErrorData}
                                    </div>`;

                            //Информирование пользователя об ошибке
                            class_this._Context.$Info.html(err_text);
                        }
                    });
                }
            }
            else {
                //Формирование текста ошибки (в зависимости от флагов валидации)
                if (!valid_UserAgreement) {
                    if (!valid_Fields) {
                        err_text =
                            `<div class="${class_this._Context.cInfo_Block_Text} ${class_this._Context.cInfo_Block_TextError}">
                                        <span class="${class_this._Context.cInfo_Block_Text_Image}"></span>
                                        ${class_this._Context.sInfo_ErrorFieldsAndUserAgreement}
                                    </div>`;
                    }
                    else {
                        err_text =
                            `<div class="${class_this._Context.cInfo_Block_Text} ${class_this._Context.cInfo_Block_TextError}">
                                        <span class="${class_this._Context.cInfo_Block_Text_Image}"></span>
                                        ${class_this._Context.sInfo_ErrorUserAgreement}
                                    </div>`;
                    }
                }
                else {
                    err_text =
                        `<div class="${class_this._Context.cInfo_Block_Text} ${class_this._Context.cInfo_Block_TextError}">
                                    <span class="${class_this._Context.cInfo_Block_Text_Image}"></span>
                                    ${class_this._Context.sInfo_ErrorFields}
                                </div>`;
                }

                //Информирование пользователя об ошибке
                class_this._Context.$Info.html(err_text);
            }

            return valid;
        }
    }

    /**
     * Получение значения элемента по значению атрибута name
     */
    export function GetValue(model: iGet): any {

        //TODO: WIS - Одинаковый код (можно объединить)

        for (let elem of model.aItems) {
            if (elem.sAttr_Name != undefined && elem.sAttr_Name == model.sAttr_Name) {

                if (elem.flag_isCheckbox) {
                    let elem_value = false;
                    if (elem.$Elem.prop("checked")) {
                        elem_value = true;
                    }

                    return elem_value;
                }
                else {
                    return elem.$Elem.val();
                }
            }
        }
        return undefined;
    }

    //Автоматический вызов формы
    $(".wis-js-feedback_common_form_init").each(function () {
        new WIS_B_FeedBackCommon.Init({
            $Elem: $(this)
        });
    });
}