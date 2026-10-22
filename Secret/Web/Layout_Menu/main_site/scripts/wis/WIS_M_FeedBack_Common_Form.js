var WIS_M_FeedBack_Common_Form;
(function (WIS_M_FeedBack_Common_Form) {
    var $Elem = $('.wis-js-feedback_common_form');
    var flag_Debug = false;
    if ($Elem != undefined && $Elem.length > 0) {
        var flag_Debug_Str = $Elem.data("wis-debug");
        if (flag_Debug_Str != undefined && flag_Debug_Str.toString().toLowerCase() == "true") {
            flag_Debug = true;
        }
    }
    var urlSubmit_Part_1 = "https://srv.wis";
    var urlSubmit_Part_2 = "erv.ru/mails/test-site.ru/api/feedback/send";
    var urlSubmit = "".concat(urlSubmit_Part_1).concat(urlSubmit_Part_2);
    var sTitle = "Тема письма (Отправлено с ProjectBase_wis-feedback_form_ver2)";
    var sClient_Title = "Тема письма (Отправлено с ProjectBase_wis-feedback_form_ver2) [для клиента]";
    new WIS_B_FeedBackCommon.Init({
        $Elem: $Elem,
        flag_isShow_Init_Log: true,
        flag_isShow_Notification_Success: true,
        flag_isShow_Notification_Error: true,
        sMethod: "POST",
        urlSubmit: urlSubmit,
        flag_isTest: true,
        fValue_Test: function (list) {
            var elem_Nam = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "nam", aItems: list });
            var elem_Fam = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "fam", aItems: list });
            var elem_Email = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "email", aItems: list });
            var elem_Password = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "password", aItems: list });
            var elem_Phone = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "phone", aItems: list });
            var elem_Text = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "text", aItems: list });
            var elem_City = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "city", aItems: list });
            var elem_File = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "filename", aItems: list });
            return "\u0414\u0430\u043D\u043D\u044B\u0435 \u043A\u043E\u0442\u043E\u0440\u044B\u0435 \u043E\u0442\u043F\u0440\u0430\u0432\u043B\u044F\u044E\u0442\u0441\u044F (\u0442\u0435\u0441\u0442\u043E\u0432\u043E\u0435 \u0441\u043E\u043E\u0431\u0449\u0435\u043D\u0438\u0435, \u043F\u0440\u0438 \u043F\u0443\u0431\u043B\u0438\u043A\u0430\u0446\u0438\u0438 \u0443\u0434\u0430\u043B\u0438\u0442\u044C)<br/>" +
                "title: ".concat(sTitle, " <br/>") +
                "value: \u0418\u043C\u044F: ".concat(elem_Nam, " ~ \u0424\u0430\u043C\u0438\u043B\u0438\u044F: ").concat(elem_Fam, " ~ \u042D\u043B. \u043F\u043E\u0447\u0442\u0430: ").concat(elem_Email, " ~ \u041F\u0430\u0440\u043E\u043B\u044C: ").concat(elem_Password, " ~ \u0422\u0435\u043B\u0435\u0444\u043E\u043D: ").concat(elem_Phone, " ~ \u0422\u0435\u043A\u0441\u0442: ").concat(elem_Text, " ~ \u0413\u043E\u0440\u043E\u0434: ").concat(elem_City, " ~ \u0424\u0430\u0439\u043B: ").concat(elem_File, " <br/>");
        },
        fCallback_Success_After: function (data) {
            if (data != undefined && data.length > 0 && data[0] == WIS_B_FeedBackCommon.sResponse_Success) {
                console.log("Код, например редирект на страницу");
            }
        },
        fData_Json: function (list) {
            var elem_Nam = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "nam", aItems: list });
            var elem_Fam = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "fam", aItems: list });
            var elem_Email = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "email", aItems: list });
            var elem_Password = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "password", aItems: list });
            var elem_Phone = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "phone", aItems: list });
            var elem_Text = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "text", aItems: list });
            var elem_City = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "city", aItems: list });
            var elem_File = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "filename", aItems: list });
            var elem_UTM = WIS_B_UTM.Get();
            var data_json = {
                "title": sTitle,
                "client_title": sClient_Title,
                "value": "\u0418\u043C\u044F: ".concat(elem_Nam, "\n") +
                    "\u0424\u0430\u043C\u0438\u043B\u0438\u044F: ".concat(elem_Fam, "\n") +
                    "\u042D\u043B. \u043F\u043E\u0447\u0442\u0430: ".concat(elem_Email, "\n") +
                    "\u041F\u0430\u0440\u043E\u043B\u044C: ".concat(elem_Password, "\n") +
                    "\u0422\u0435\u043B\u0435\u0444\u043E\u043D: ".concat(elem_Phone, "\n") +
                    "\u0422\u0435\u043A\u0441\u0442: ".concat(elem_Text, "\n") +
                    "\u0413\u043E\u0440\u043E\u0434: ".concat(elem_City, "\n") +
                    "\u0424\u0430\u0439\u043B: ".concat(elem_File),
                "client_value": "\u0418\u043C\u044F: ".concat(elem_Nam, "\n") +
                    "\u0424\u0430\u043C\u0438\u043B\u0438\u044F: ".concat(elem_Fam, "\n") +
                    "\u042D\u043B. \u043F\u043E\u0447\u0442\u0430: ".concat(elem_Email, "\n") +
                    "\u041F\u0430\u0440\u043E\u043B\u044C: ".concat(elem_Password, "\n") +
                    "\u0422\u0435\u043B\u0435\u0444\u043E\u043D: ".concat(elem_Phone, "\n") +
                    "\u0422\u0435\u043A\u0441\u0442: ".concat(elem_Text, "\n") +
                    "\u0413\u043E\u0440\u043E\u0434: ".concat(elem_City, "\n") +
                    "\u0424\u0430\u0439\u043B: ".concat(elem_File),
                "client_email": elem_Email,
                "pageurl": elem_UTM.wis_page,
                "utm_medium": elem_UTM.utm_medium,
                "utm_source": elem_UTM.utm_source,
                "utm_campaign": elem_UTM.utm_campaign,
                "block": elem_UTM.block,
                "utm_term": elem_UTM.utm_term,
                "utm_content": elem_UTM.utm_content,
                "position": elem_UTM.position,
                "keyword": elem_UTM.keyword
            };
            return data_json;
        }
    });
})(WIS_M_FeedBack_Common_Form || (WIS_M_FeedBack_Common_Form = {}));
//# sourceMappingURL=WIS_M_FeedBack_Common_Form.js.map