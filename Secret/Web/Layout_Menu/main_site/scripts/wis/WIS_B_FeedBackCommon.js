var WIS_B_FeedBackCommon;
(function (WIS_B_FeedBackCommon) {
    WIS_B_FeedBackCommon.sResponse_Success = "+";
    var Init = (function () {
        function Init(model) {
            if (model.$Elem != undefined && model.$Elem.length > 0) {
                var Item_Class = "wis-js-feedback_form-fields-elem";
                if (model.cItem != undefined && model.cItem.length > 0) {
                    Item_Class = model.cItem;
                }
                var Flag_isNoAsync = false;
                if (model.flag_isNoAsync != undefined) {
                    Flag_isNoAsync = model.flag_isNoAsync;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        var Flag_isNoAsync_Str = model.$Elem.data("wis-no_async");
                        if (Flag_isNoAsync_Str != undefined && Flag_isNoAsync_Str.toString().toLowerCase() == "true") {
                            Flag_isNoAsync = true;
                        }
                    }
                }
                var Method_Str = "";
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
                var Submit_Url = "";
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
                var Info_Class = "wis-js-feedback_form-info";
                if (model.cInfo != undefined && model.cInfo.length > 0) {
                    Info_Class = model.cInfo;
                }
                var Submit_Class = "wis-js-feedback_form-submit";
                if (model.cSubmit != undefined && model.cSubmit.length > 0) {
                    Submit_Class = model.cSubmit;
                }
                var Error_Class = "wis-js-error";
                if (model.cError != undefined && model.cError.length > 0) {
                    Error_Class = model.cError;
                }
                var Info_Block_Text_Class = "wis-feedback_form-info-text";
                if (model.cInfo_Block_Text != undefined && model.cInfo_Block_Text.length > 0) {
                    Info_Block_Text_Class = model.cInfo_Block_Text;
                }
                var Info_Block_TextError_Class = "wis-feedback_form-info-text_error";
                if (model.cInfo_Block_TextError != undefined && model.cInfo_Block_TextError.length > 0) {
                    Info_Block_TextError_Class = model.cInfo_Block_TextError;
                }
                var Info_Block_TextLoader_Class = "wis-feedback_form-info-text_loader";
                if (model.cInfo_Block_TextLoader != undefined && model.cInfo_Block_TextLoader.length > 0) {
                    Info_Block_TextLoader_Class = model.cInfo_Block_TextLoader;
                }
                var Info_Block_Text_Image_Class = "wis-feedback_form-info-text-img";
                if (model.cInfo_Block_Text_Image != undefined && model.cInfo_Block_Text_Image.length > 0) {
                    Info_Block_Text_Image_Class = model.cInfo_Block_Text_Image;
                }
                var Info_Sending_Str = "Заявка отправляется...";
                if (model.sInfo_Sending != undefined && model.sInfo_Sending.length > 0) {
                    Info_Sending_Str = model.sInfo_Sending;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        var sData_Info_Sending = model.$Elem.data("wis-info_sending");
                        if (sData_Info_Sending != undefined) {
                            Info_Sending_Str = sData_Info_Sending;
                        }
                    }
                }
                var Info_Success_Str = "Сообщение успешно отправлено.";
                if (model.sInfo_Success != undefined && model.sInfo_Success.length > 0) {
                    Info_Success_Str = model.sInfo_Success;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        var sData_Info_Success = model.$Elem.data("wis-info_success");
                        if (sData_Info_Success != undefined) {
                            Info_Success_Str = sData_Info_Success;
                        }
                    }
                }
                var Info_Error_Str = "Ошибка отправки сообщения, обратитесь к разрабочику или проверьте интернет-соединение.";
                if (model.sInfo_Error != undefined && model.sInfo_Error.length > 0) {
                    Info_Error_Str = model.sInfo_Error;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        var sData_Info_Error = model.$Elem.data("wis-info_error");
                        if (sData_Info_Error != undefined) {
                            Info_Error_Str = sData_Info_Error;
                        }
                    }
                }
                var Info_ErrorData_Str = "Ошибка отправки сообщения, обратитесь к разрабочику или проверьте интернет-соединение. (Ошибка отправки данных).";
                if (model.sInfo_ErrorData != undefined && model.sInfo_ErrorData.length > 0) {
                    Info_ErrorData_Str = model.sInfo_ErrorData;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        var s_Data_Info_ErrorData = model.$Elem.data("wis-info_errordata");
                        if (s_Data_Info_ErrorData != undefined) {
                            Info_ErrorData_Str = s_Data_Info_ErrorData;
                        }
                    }
                }
                var Info_ErrorFieldsAndUserAgreement_Str = "Необходимо принять соглашение и заполнить обязательные поля";
                if (model.sInfo_ErrorFieldsAndUserAgreement != undefined && model.sInfo_ErrorFieldsAndUserAgreement.length > 0) {
                    Info_ErrorFieldsAndUserAgreement_Str = model.sInfo_ErrorFieldsAndUserAgreement;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        var s_Data_Info_ErrorFieldsAndUserAgreement = model.$Elem.data("wis-info_errorfieldsanduseragreement");
                        if (s_Data_Info_ErrorFieldsAndUserAgreement != undefined) {
                            Info_ErrorFieldsAndUserAgreement_Str = s_Data_Info_ErrorFieldsAndUserAgreement;
                        }
                    }
                }
                var Info_ErrorUserAgreement_Str = "Необходимо принять соглашение";
                if (model.sInfo_ErrorUserAgreement != undefined && model.sInfo_ErrorUserAgreement.length > 0) {
                    Info_ErrorUserAgreement_Str = model.sInfo_ErrorUserAgreement;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        var s_Data_Info_ErrorUserAgreement = model.$Elem.data("wis-info_erroruseragreement");
                        if (s_Data_Info_ErrorUserAgreement != undefined) {
                            Info_ErrorUserAgreement_Str = s_Data_Info_ErrorUserAgreement;
                        }
                    }
                }
                var Info_ErrorFields_Str = "Необходимо заполнить обязательные поля";
                if (model.sInfo_ErrorFields != undefined && model.sInfo_ErrorFields.length > 0) {
                    Info_ErrorFields_Str = model.sInfo_ErrorFields;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        var s_Data_Info_ErrorFields = model.$Elem.data("wis-info_errorfields");
                        if (s_Data_Info_ErrorFields != undefined) {
                            Info_ErrorFields_Str = s_Data_Info_ErrorFields;
                        }
                    }
                }
                var str_InfoField_ErrorComboBoxEmpty = WIS_B_Common.sError_ComboBoxEmpty;
                if (model.s_InfoField_ErrorComboBoxEmpty != undefined && model.s_InfoField_ErrorComboBoxEmpty.length > 0) {
                    str_InfoField_ErrorComboBoxEmpty = model.s_InfoField_ErrorComboBoxEmpty;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        var s_Data_InfoField_ErrorComboBoxEmpty = model.$Elem.data("wis-infofield_errorcomboboxempty");
                        if (s_Data_InfoField_ErrorComboBoxEmpty != undefined) {
                            str_InfoField_ErrorComboBoxEmpty = s_Data_InfoField_ErrorComboBoxEmpty;
                        }
                    }
                }
                var str_InfoField_ErrorSpace = undefined;
                if (model.s_InfoField_ErrorSpace != undefined && model.s_InfoField_ErrorSpace.length > 0) {
                    str_InfoField_ErrorSpace = model.s_InfoField_ErrorSpace;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        var s_Data_InfoField_ErrorSpace = model.$Elem.data("wis-infofield_errorspace");
                        if (s_Data_InfoField_ErrorSpace != undefined) {
                            str_InfoField_ErrorSpace = s_Data_InfoField_ErrorSpace;
                        }
                    }
                }
                var str_InfoField_ErrorIncorrectly = undefined;
                if (model.s_InfoField_ErrorIncorrectly != undefined && model.s_InfoField_ErrorIncorrectly.length > 0) {
                    str_InfoField_ErrorIncorrectly = model.s_InfoField_ErrorIncorrectly;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        var s_Data_InfoField_ErrorIncorrectly = model.$Elem.data("wis-infofield_errorincorrectly");
                        if (s_Data_InfoField_ErrorIncorrectly != undefined) {
                            str_InfoField_ErrorIncorrectly = s_Data_InfoField_ErrorIncorrectly;
                        }
                    }
                }
                var str_InfoField_ErrorEmpty = undefined;
                if (model.s_InfoField_ErrorEmpty != undefined && model.s_InfoField_ErrorEmpty.length > 0) {
                    str_InfoField_ErrorEmpty = model.s_InfoField_ErrorEmpty;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        var s_Data_InfoField_ErrorEmpty = model.$Elem.data("wis-infofield_errorempty");
                        if (s_Data_InfoField_ErrorEmpty != undefined) {
                            str_InfoField_ErrorEmpty = s_Data_InfoField_ErrorEmpty;
                        }
                    }
                }
                var str_InfoField_ErrorLength = undefined;
                if (model.s_InfoField_ErrorLength != undefined && model.s_InfoField_ErrorLength.length > 0) {
                    str_InfoField_ErrorLength = model.s_InfoField_ErrorLength;
                }
                else {
                    if (model.$Elem != undefined && model.$Elem.length > 0) {
                        var s_Data_InfoField_ErrorLength = model.$Elem.data("wis-infofield_errorlength");
                        if (s_Data_InfoField_ErrorLength != undefined) {
                            str_InfoField_ErrorLength = s_Data_InfoField_ErrorLength;
                        }
                    }
                }
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
                }
            }
        }
        Init.prototype._Refresh = function () {
            var class_this = this;
            if (class_this._Context.$Elem != undefined && class_this._Context.$Elem.length > 0) {
                class_this._Context.$Items = class_this._Context.$Elem.find(".".concat(class_this._Context.cItem));
                class_this._Context.$Info = class_this._Context.$Elem.find(".".concat(class_this._Context.cInfo));
                ;
                class_this._Context.$Submit = class_this._Context.$Elem.find(".".concat(class_this._Context.cSubmit));
                class_this._Context.$Items.each(function () {
                    var elem_this = $(this);
                    var item_Elem = elem_this.find("input, textarea, select");
                    var error_Elem = elem_this.find(".".concat(class_this._Context.cError));
                    if (item_Elem != undefined && item_Elem.length > 0) {
                        var tagname = item_Elem.get()[0].tagName.toString().toLowerCase();
                        var attr_type = item_Elem.attr("type");
                        if (attr_type != undefined && attr_type.length > 0) {
                            attr_type = attr_type.toString().toLowerCase();
                        }
                        var attr_name = item_Elem.attr("name");
                        var name_1 = item_Elem.data(class_this._Context.data_wis_rule_name);
                        var flag_required = false;
                        var flag_required_Str = item_Elem.data(class_this._Context.data_wis_rule_required);
                        if (flag_required_Str != undefined && flag_required_Str.toString().toLowerCase() == "true") {
                            flag_required = true;
                        }
                        var flag_email = false;
                        var flag_email_Str = item_Elem.data(class_this._Context.data_wis_rule_email);
                        if (flag_email_Str != undefined && flag_email_Str.toString().toLowerCase() == "true") {
                            flag_email = true;
                        }
                        var flag_phone = false;
                        var flag_phone_Str = item_Elem.data(class_this._Context.data_wis_rule_phone);
                        if (flag_phone_Str != undefined && flag_phone_Str.toString().toLowerCase() == "true") {
                            flag_phone = true;
                        }
                        var flag_select_num = false;
                        var flag_select_num_Str = item_Elem.data(class_this._Context.data_wis_rule_select_num);
                        if (flag_select_num_Str != undefined && flag_select_num_Str.toString().toLowerCase() == "true") {
                            flag_select_num = true;
                        }
                        var flag_useragreement = false;
                        var flag_useragreement_Str = item_Elem.data(class_this._Context.data_wis_rule_useragreement);
                        if (flag_useragreement_Str != undefined && flag_useragreement_Str.toString().toLowerCase() == "true") {
                            flag_useragreement = true;
                        }
                        var minlength_1 = item_Elem.data(class_this._Context.data_wis_rule_minlength);
                        if (minlength_1 == undefined || minlength_1 < 0) {
                            minlength_1 = 0;
                        }
                        var maxlength_1 = item_Elem.data(class_this._Context.data_wis_rule_maxlength);
                        if (maxlength_1 == undefined || maxlength_1 < 0) {
                            maxlength_1 = 0;
                        }
                        if (minlength_1 > 0 && maxlength_1 <= 0) {
                            maxlength_1 = 1000;
                        }
                        var flag_Check_Length_Trim = false;
                        var flag_Check_Email = false;
                        var flag_Check_Phone = false;
                        var flag_Check_ComboBox_Str = false;
                        var flag_Check_ComboBox_Num = false;
                        var flag_isCheckbox = false;
                        var flag_isCombobox = false;
                        if (attr_type == "text" ||
                            attr_type == "password" ||
                            attr_type == "email" ||
                            attr_type == "file" ||
                            tagname == "textarea") {
                            if (flag_required) {
                                if (minlength_1 <= 0) {
                                    minlength_1 = 1;
                                }
                                if (maxlength_1 <= 0) {
                                    maxlength_1 = 1000;
                                }
                                flag_Check_Length_Trim = true;
                            }
                            else {
                                if (minlength_1 > 0 || maxlength_1 > 0) {
                                    if (maxlength_1 <= 0) {
                                        maxlength_1 = 1000;
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
                        if (flag_Check_Length_Trim) {
                            item_Elem.focusout(function () {
                                WIS_B_Common.Check_Length_Trim({
                                    $Elem: item_Elem,
                                    sFieldName: name_1,
                                    nMin: minlength_1,
                                    nMax: maxlength_1,
                                    $Validate: error_Elem,
                                    s_Error_Empty: class_this._Context.s_InfoField_ErrorEmpty,
                                    s_Error_Length: class_this._Context.s_InfoField_ErrorLength,
                                    s_Error_Space: class_this._Context.s_InfoField_ErrorSpace,
                                });
                            });
                            item_Elem.keyup(function () {
                                WIS_B_Common.Validation_Error_Del({
                                    $Elem: item_Elem,
                                    $Validate: error_Elem
                                });
                            });
                        }
                        if (flag_Check_Email) {
                            item_Elem.focusout(function () {
                                WIS_B_Common.Check_Email({
                                    $Elem: item_Elem,
                                    $Validate: error_Elem,
                                    s_Error_Empty: class_this._Context.s_InfoField_ErrorEmpty,
                                    s_Error_Incorrectly: class_this._Context.s_InfoField_ErrorIncorrectly,
                                    s_Error_Space: class_this._Context.s_InfoField_ErrorSpace
                                });
                            });
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
                            if (item_Elem.inputmask !== undefined) {
                                item_Elem.inputmask("mask", {
                                    "mask": "+7 (999) 999-99-99",
                                    "oncomplete": function () {
                                        WIS_B_Common.Validation_Error_Del({
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
                                });
                            }
                            item_Elem.keyup(function () {
                                WIS_B_Common.Validation_Error_Del({
                                    $Elem: item_Elem,
                                    $Validate: error_Elem
                                });
                            });
                        }
                        if (flag_Check_ComboBox_Str) {
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
                            sName: name_1,
                            flag_isRequired: flag_required,
                            flag_isEmail: flag_email,
                            flag_isPhone: flag_phone,
                            flag_isSelectNum: flag_select_num,
                            nMinLength: minlength_1,
                            nMaxLength: maxlength_1,
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
                if (class_this._Context.$Elem.get()[0].tagName.toLowerCase() == "form") {
                    class_this._Context.$Elem.submit(function (e) {
                        var check = class_this._Submit();
                        if (class_this._Context.flag_isNoAsync != undefined && class_this._Context.flag_isNoAsync) {
                            if (check) {
                                setTimeout(function () {
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
                    class_this._Context.$Submit.click(function (e) {
                        var check = class_this._Submit();
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
        };
        Init.prototype._Submit = function () {
            var class_this = this;
            var valid = true;
            var valid_Fields = true;
            var valid_UserAgreement = true;
            var err_text = "";
            if (class_this._Context.aItems != undefined && class_this._Context.aItems.length > 0) {
                for (var _i = 0, _a = class_this._Context.aItems; _i < _a.length; _i++) {
                    var elem = _a[_i];
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
                class_this._Context.$Info.html("<div class=\"".concat(class_this._Context.cInfo_Block_Text, " ").concat(class_this._Context.cInfo_Block_TextLoader, "\">\n                                <span class=\"").concat(class_this._Context.cInfo_Block_Text_Image, "\"></span>\n                                ").concat(class_this._Context.sInfo_Sending, "\n                            </div>"));
                if (!(this._Context.flag_isNoAsync != undefined && this._Context.flag_isNoAsync)) {
                    class_this._Context.$Submit.prop("disabled", true);
                    var data_json = {};
                    if (class_this._Context.fData_Json != undefined && typeof class_this._Context.fData_Json == "function") {
                        data_json = class_this._Context.fData_Json(class_this._Context.aItems);
                        if (class_this._Context.flag_isTest) {
                            var data_send_str = "";
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
                        var data_send_str = "";
                        for (var _b = 0, _c = class_this._Context.aItems; _b < _c.length; _b++) {
                            var elem = _c[_b];
                            if (elem.sAttr_Name != undefined) {
                                if (elem.flag_isCheckbox) {
                                    var elem_value = false;
                                    if (elem.$Elem.prop("checked")) {
                                        elem_value = true;
                                    }
                                    data_send_str += "".concat(elem.sAttr_Name, ": ").concat(elem_value, " ");
                                    data_json["".concat(elem.sAttr_Name)] = elem_value;
                                }
                                else {
                                    data_send_str += "".concat(elem.sAttr_Name, ": ").concat(elem.$Elem.val(), " ");
                                    data_json["".concat(elem.sAttr_Name)] = elem.$Elem.val();
                                }
                            }
                        }
                        var elem_UTM = WIS_B_UTM.Get();
                        data_json["wis_page"] = elem_UTM.wis_page;
                        data_json["utm_medium"] = elem_UTM.utm_medium;
                        data_json["utm_source"] = elem_UTM.utm_source;
                        data_json["utm_campaign"] = elem_UTM.utm_campaign;
                        data_json["block"] = elem_UTM.block;
                        data_json["utm_term"] = elem_UTM.utm_term;
                        data_json["utm_content"] = elem_UTM.utm_content;
                        data_json["position"] = elem_UTM.position;
                        data_json["keyword"] = elem_UTM.keyword;
                        if (class_this._Context.flag_isTest) {
                            WIS_B_Notification.Show({
                                Value_html: "\u0414\u0430\u043D\u043D\u044B\u0435 \u043A\u043E\u0442\u043E\u0440\u044B\u0435 \u043E\u0442\u043F\u0440\u0430\u0432\u043B\u044F\u044E\u0442\u0441\u044F (\u0442\u0435\u0441\u0442\u043E\u0432\u043E\u0435 \u0441\u043E\u043E\u0431\u0449\u0435\u043D\u0438\u0435, \u043F\u0440\u0438 \u043F\u0443\u0431\u043B\u0438\u043A\u0430\u0446\u0438\u0438 \u0443\u0434\u0430\u043B\u0438\u0442\u044C). \n                                 ".concat(data_send_str, "\n                                 \u0421\u0442\u0440\u0430\u043D\u0438\u0446\u0430 \u043E\u0442\u043F\u0440\u0430\u0432\u043A\u0438: ").concat(elem_UTM.wis_page),
                                Type: WIS_B_Notification.eType.Warning
                            });
                        }
                    }
                    $.ajax({
                        type: class_this._Context.sMethod,
                        cache: false,
                        url: class_this._Context.urlSubmit,
                        data: data_json,
                        success: function (data) {
                            if (class_this._Context.fCallback_Success != undefined && typeof class_this._Context.fCallback_Success == "function") {
                                class_this._Context.fCallback_Success(data);
                            }
                            else {
                                class_this._Context.$Submit.prop("disabled", false);
                                if (data != undefined && data.length > 0 && data[0] == WIS_B_FeedBackCommon.sResponse_Success) {
                                    if (class_this._Context.flag_isShow_Notification_Success === undefined || class_this._Context.flag_isShow_Notification_Success) {
                                        WIS_B_Notification.Show({
                                            Value_html: class_this._Context.sInfo_Success
                                        });
                                    }
                                    err_text =
                                        "<div class=\"".concat(class_this._Context.cInfo_Block_Text, "\">\n                                            <span class=\"").concat(class_this._Context.cInfo_Block_Text_Image, "\"></span>\n                                            ").concat(class_this._Context.sInfo_Success, "\n                                        </div>");
                                    class_this._Context.$Info.html(err_text);
                                }
                                else {
                                    if (class_this._Context.flag_isShow_Notification_Error === undefined || class_this._Context.flag_isShow_Notification_Error) {
                                        WIS_B_Notification.Show({
                                            Value_html: class_this._Context.sInfo_Error,
                                            Type: WIS_B_Notification.eType.Error
                                        });
                                    }
                                    err_text =
                                        "<div class=\"".concat(class_this._Context.cInfo_Block_Text, " ").concat(class_this._Context.cInfo_Block_TextError, "\">\n                                            <span class=\"").concat(class_this._Context.cInfo_Block_Text_Image, "\"></span>\n                                            ").concat(class_this._Context.sInfo_Error, "\n                                        </div>");
                                    class_this._Context.$Info.html(err_text);
                                }
                            }
                            if (class_this._Context.fCallback_Success_After != undefined && typeof class_this._Context.fCallback_Success_After == "function") {
                                class_this._Context.fCallback_Success_After(data);
                            }
                        },
                        error: function () {
                            class_this._Context.$Submit.prop("disabled", false);
                            if (class_this._Context.flag_isShow_Notification_Error === undefined || class_this._Context.flag_isShow_Notification_Error) {
                                WIS_B_Notification.Show({
                                    Value_html: class_this._Context.sInfo_ErrorData,
                                    Type: WIS_B_Notification.eType.Error
                                });
                            }
                            err_text =
                                "<div class=\"".concat(class_this._Context.cInfo_Block_Text, " ").concat(class_this._Context.cInfo_Block_TextError, "\">\n                                        <span class=\"").concat(class_this._Context.cInfo_Block_Text_Image, "\"></span>\n                                        ").concat(class_this._Context.sInfo_ErrorData, "\n                                    </div>");
                            class_this._Context.$Info.html(err_text);
                        }
                    });
                }
            }
            else {
                if (!valid_UserAgreement) {
                    if (!valid_Fields) {
                        err_text =
                            "<div class=\"".concat(class_this._Context.cInfo_Block_Text, " ").concat(class_this._Context.cInfo_Block_TextError, "\">\n                                        <span class=\"").concat(class_this._Context.cInfo_Block_Text_Image, "\"></span>\n                                        ").concat(class_this._Context.sInfo_ErrorFieldsAndUserAgreement, "\n                                    </div>");
                    }
                    else {
                        err_text =
                            "<div class=\"".concat(class_this._Context.cInfo_Block_Text, " ").concat(class_this._Context.cInfo_Block_TextError, "\">\n                                        <span class=\"").concat(class_this._Context.cInfo_Block_Text_Image, "\"></span>\n                                        ").concat(class_this._Context.sInfo_ErrorUserAgreement, "\n                                    </div>");
                    }
                }
                else {
                    err_text =
                        "<div class=\"".concat(class_this._Context.cInfo_Block_Text, " ").concat(class_this._Context.cInfo_Block_TextError, "\">\n                                    <span class=\"").concat(class_this._Context.cInfo_Block_Text_Image, "\"></span>\n                                    ").concat(class_this._Context.sInfo_ErrorFields, "\n                                </div>");
                }
                class_this._Context.$Info.html(err_text);
            }
            return valid;
        };
        return Init;
    }());
    WIS_B_FeedBackCommon.Init = Init;
    function GetValue(model) {
        for (var _i = 0, _a = model.aItems; _i < _a.length; _i++) {
            var elem = _a[_i];
            if (elem.sAttr_Name != undefined && elem.sAttr_Name == model.sAttr_Name) {
                if (elem.flag_isCheckbox) {
                    var elem_value = false;
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
    WIS_B_FeedBackCommon.GetValue = GetValue;
    $(".wis-js-feedback_common_form_init").each(function () {
        new WIS_B_FeedBackCommon.Init({
            $Elem: $(this)
        });
    });
})(WIS_B_FeedBackCommon || (WIS_B_FeedBackCommon = {}));
//# sourceMappingURL=WIS_B_FeedBackCommon.js.map