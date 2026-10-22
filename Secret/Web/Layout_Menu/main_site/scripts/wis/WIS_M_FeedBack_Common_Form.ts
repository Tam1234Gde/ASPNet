/* Компонент wis-feedback_form_ver2 1.20.2.25  */
/* Требуемые модули: wis-checkbox, wis-notification, wis-textbox, wis-file, jquery + необязательный JQuery.InputMask (для маски телефона). */
/* Поддержка браузеров: IE8+ и остальные браузеры. */
/* Автор: Инфографика (Висерв) - https://wiserv.ru */

/**
 * Форма обратной связи.
 */
module WIS_M_FeedBack_Common_Form {

    //#region Инициализация

    /**
     * Элемент
     * */
    let $Elem: JQuery = $('.wis-js-feedback_common_form');

    /**
     * True - Debug режим
     * */
    let flag_Debug: boolean = false;

    if ($Elem != undefined && $Elem.length > 0) {
        let flag_Debug_Str: string = $Elem.data("wis-debug");
        if (flag_Debug_Str != undefined && flag_Debug_Str.toString().toLowerCase() == "true") {
            flag_Debug = true;
        }
    }

    /**
     * Url - часть 1 (для защиты от роботов)
     * */
    let urlSubmit_Part_1: string = "https://srv.wis";

    /**
     * Url - часть 2 (для защиты от роботов)
     * */
    let urlSubmit_Part_2: string = "erv.ru/mails/test-site.ru/api/feedback/send";

    /**
     * Url - для отправки на сервер
     * */
    let urlSubmit: string = `${urlSubmit_Part_1}${urlSubmit_Part_2}`;

    /**
     * Тема письма
     * */
    let sTitle: string = "Тема письма (Отправлено с ProjectBase_wis-feedback_form_ver2)";

    /**
     * Тема письма (для клиента)
     * */
    let sClient_Title = "Тема письма (Отправлено с ProjectBase_wis-feedback_form_ver2) [для клиента]";

    // Изменение настроек для Debug режима (например для верстки настройки Debug, а для CMS настройки стандартные)
    //if (flag_Debug) {
    //    let urlDebug_Submit_Part_1 = "https://srv.wis";
    //    let urlDebug_Submit_Part_2 = "erv.ru/mails/test-site.ru/api/feedback/send";
    //    urlSubmit = `${urlDebug_Submit_Part_1}${urlDebug_Submit_Part_2}`;
    //    sTitle = "Debug - Тема письма (Отправлено с ProjectBase_wis-feedback_form_[ver2])";
    //    sClient_Title = "Debug - Тема письма (Отправлено с ProjectBase_wis-feedback_form_[ver2]) [для клиента]";
    //}

    //#endregion

    /* Инициализация обратной связи */
    new WIS_B_FeedBackCommon.Init({

        /* Обязательно - Элемент формы */
        $Elem: $Elem,

        /* Необязательно - True - Включить отображение ошибок и логов при инициализации */
        flag_isShow_Init_Log: true,

        /* Необязательно - True / undefined - Включить отображение уведомления после успешной отправки */
        flag_isShow_Notification_Success: true,

        /* Необязательно - True / undefined - Включить отображение уведомления после отправки с ошибкой */
        flag_isShow_Notification_Error: true,

        /* Необязательно - тип запроса (берется автоматически из атрибута method элемента формы или устанавливается вручную) */
        sMethod: "POST",

        /* Необязательно - url запроса (берется автоматически из атрибута action элемента формы или устанавливается вручную)  */
        urlSubmit: urlSubmit,

        /* Необязательно - True - вывести всплывающее сообщение перед отправкой */
        flag_isTest: true,

        /* Необязательно - Текст всплывающего сообщения перед отправкой  */
        fValue_Test: function (list: Array<WIS_B_FeedBackCommon.iItem>) {

            //Получение значений элементов (по атрибуту name)
            let elem_Nam = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "nam", aItems: list });
            let elem_Fam = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "fam", aItems: list });
            let elem_Email = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "email", aItems: list });
            let elem_Password = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "password", aItems: list });
            let elem_Phone = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "phone", aItems: list });
            let elem_Text = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "text", aItems: list });
            let elem_City = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "city", aItems: list });
            let elem_File = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "filename", aItems: list });

            return `Данные которые отправляются (тестовое сообщение, при публикации удалить)<br/>` +
                `title: ${sTitle} <br/>` +
                `value: Имя: ${elem_Nam} ~ Фамилия: ${elem_Fam} ~ Эл. почта: ${elem_Email} ~ Пароль: ${elem_Password} ~ Телефон: ${elem_Phone} ~ Текст: ${elem_Text} ~ Город: ${elem_City} ~ Файл: ${elem_File} <br/>`;
        },

        /* Необязательно - Функция в случае успеха (после основного кода)  */
        fCallback_Success_After: function (data: string) {
            if (data != undefined && data.length > 0 && data[0] == WIS_B_FeedBackCommon.sResponse_Success) {
                console.log("Код, например редирект на страницу");
            }
        },

        /* Необязательно - генерирование json данных для отправки (json данные генерируются автоматически из элементов формы или устанавливаются вручную)  */
        fData_Json: function (list: Array<WIS_B_FeedBackCommon.iItem>) {

            //Получение значений элементов (по атрибуту name)
            let elem_Nam = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "nam", aItems: list });
            let elem_Fam = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "fam", aItems: list });
            let elem_Email = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "email", aItems: list });
            let elem_Password = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "password", aItems: list });
            let elem_Phone = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "phone", aItems: list });
            let elem_Text = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "text", aItems: list });
            let elem_City = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "city", aItems: list });
            let elem_File = WIS_B_FeedBackCommon.GetValue({ sAttr_Name: "filename", aItems: list });

            //Получение UTM меток
            let elem_UTM = WIS_B_UTM.Get();

            // Изменение настроек для Debug режима (например для верстки настройки Debug, а для CMS настройки стандартные)
            //if (flag_Debug) {
            //    //Генерация данных
            //    let Debug_data_json = {
            //        /* Тема письма */
            //        "title": sTitle,

            //        /* Тема письма (для клиента). Если для клиента письмо не нужно, то задать пустое значение client_title : ""  */
            //        "client_title": sClient_Title,

            //        /* Тело письма */
            //        "value":
            //            `Имя: ${elem_Nam}\n` +
            //            `Фамилия: ${elem_Fam}\n` +
            //            `Эл. почта: ${elem_Email}\n` +
            //            `Пароль: ${elem_Password}\n` +
            //            `Телефон: ${elem_Phone}\n` +
            //            `Текст: ${elem_Text}\n` +
            //            `Город: ${elem_City}\n` +
            //            `Файл: ${elem_File}`,

            //        /* Тело письма (для клиента). Если для клиента письмо не нужно, то задать пустое значение client_value : ""  */
            //        "client_value":
            //            `Имя: ${elem_Nam}\n` +
            //            `Фамилия: ${elem_Fam}\n` +
            //            `Эл. почта: ${elem_Email}\n` +
            //            `Пароль: ${elem_Password}\n` +
            //            `Телефон: ${elem_Phone}\n` +
            //            `Текст: ${elem_Text}\n` +
            //            `Город: ${elem_City}\n` +
            //            `Файл: ${elem_File}`,

            //        /* Эл. почта клиента. Если для клиента письмо не нужно, то задать пустое значение client_email : ""  */
            //        "client_email": elem_Email,

            //        /* Url страницы */
            //        "pageurl": elem_UTM.wis_page,

            //        /* Utm метки */
            //        "utm_medium": elem_UTM.utm_medium,
            //        "utm_source": elem_UTM.utm_source,
            //        "utm_campaign": elem_UTM.utm_campaign,
            //        "block": elem_UTM.block,
            //        "utm_term": elem_UTM.utm_term,
            //        "utm_content": elem_UTM.utm_content,
            //        "position": elem_UTM.position,
            //        "keyword": elem_UTM.keyword
            //    };
            //    return Debug_data_json;
            //}

            //Генерация данных
            let data_json = {
                /* Тема письма */
                "title": sTitle,

                /* Тема письма (для клиента). Если для клиента письмо не нужно, то задать пустое значение client_title : ""  */
                "client_title": sClient_Title,

                /* Тело письма */
                "value":
                    `Имя: ${elem_Nam}\n` +
                    `Фамилия: ${elem_Fam}\n` +
                    `Эл. почта: ${elem_Email}\n` +
                    `Пароль: ${elem_Password}\n` +
                    `Телефон: ${elem_Phone}\n` +
                    `Текст: ${elem_Text}\n` +
                    `Город: ${elem_City}\n` +
                    `Файл: ${elem_File}`,

                /* Тело письма (для клиента). Если для клиента письмо не нужно, то задать пустое значение client_value : ""  */
                "client_value":
                    `Имя: ${elem_Nam}\n` +
                    `Фамилия: ${elem_Fam}\n` +
                    `Эл. почта: ${elem_Email}\n` +
                    `Пароль: ${elem_Password}\n` +
                    `Телефон: ${elem_Phone}\n` +
                    `Текст: ${elem_Text}\n` +
                    `Город: ${elem_City}\n` +
                    `Файл: ${elem_File}`,

                /* Эл. почта клиента. Если для клиента письмо не нужно, то задать пустое значение client_email : ""  */
                "client_email": elem_Email,

                /* Url страницы */
                "pageurl": elem_UTM.wis_page,

                /* Utm метки */
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
}