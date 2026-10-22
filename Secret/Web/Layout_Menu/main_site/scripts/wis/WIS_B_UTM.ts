/* Компонент wis-utm 1.18.1.31  */
/* Требуемые модули: jquery */
/* Поддержка браузеров: IE8+ и остальные браузеры. */
/* Автор: ВебИнформСервис (Висерв) - https://wiserv.ru/ */

/**
 * UTM метки 
 */
module WIS_B_UTM {

    /**
     * Url - Отправка UTM
     */
    var UTM_Url: string = "/FeedBack/UTM";

    /**
     * Структура - UTM
     */
    interface iUTM {

        /**
         * Url страницы с которой отправлена заявка
         */
        wis_page: string;

        /**
         * Тип трафика
         */
        utm_medium: string;

        /**
         * Источник перехода
         */
        utm_source: string;

        /**
         * Название рекламной кампании
         */
        utm_campaign: string;

        /**
         * Тип блока
         */
        block: string;

        /**
         * Ключевая фраза
         */
        utm_term: string;

        /**
         * Дополнительная информация, которая помогает различать объявления
         */
        utm_content: string;

        /**
         * Точная позиция объявления в блоке
         */
        position: string;

        /**
         * Ключевая фраза, по которой было показано объявление
         */
        keyword: string;
    }


    interface iUTM_Request {

        check: boolean;
    }

    export function Get_UrlParameter(elem: string) {
        let sPageURL = decodeURIComponent(window.location.search.substring(1)),

            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === elem) {
                return sParameterName[1] === undefined ? true : sParameterName[1];
            }
        }
    }

    export function Get(): iUTM {

        // Url страницы с которой отправлена заявка
        let wis_page: string = document.location.href;

        // Тип трафика
        let utm_medium: string = (Get_UrlParameter("utm_medium") as any);

        // Источник перехода
        let utm_source: string = (Get_UrlParameter("utm_source") as any);

        // Название рекламной кампании
        let utm_campaign: string = (Get_UrlParameter("utm_campaign") as any);

        // Тип блока
        let block: string = (Get_UrlParameter("block") as any);

        // Ключевая фраза
        let utm_term: string = (Get_UrlParameter("utm_term") as any);

        // Дополнительная информация, которая помогает различать объявления
        let utm_content: string = (Get_UrlParameter("utm_content") as any);

        // Точная позиция объявления в блоке.
        let position: string = (Get_UrlParameter("position") as any);

        // Ключевая фраза, по которой было показано объявление
        let keyword: string = (Get_UrlParameter("keyword") as any);

        let elem: iUTM = {
            wis_page: wis_page,
            utm_medium: utm_medium,
            utm_source: utm_source,
            utm_campaign: utm_campaign,
            block: block,
            utm_term: utm_term,
            utm_content: utm_content,
            position: position,
            keyword: keyword
        };
        return elem;
    }

    function Send() {
        let valid: boolean = false;
        let elem_UTM = Get();

        if (elem_UTM.utm_medium !== undefined) {
            valid = true;
        }

        if (elem_UTM.utm_source !== undefined) {
            valid = true;
        }

        if (elem_UTM.utm_campaign !== undefined) {
            valid = true;
        }

        if (elem_UTM.block !== undefined) {
            valid = true;
        }

        if (elem_UTM.utm_term !== undefined) {
            valid = true;
        }

        if (elem_UTM.utm_content !== undefined) {
            valid = true;
        }

        if (elem_UTM.position !== undefined) {
            valid = true;
        }

        if (elem_UTM.keyword !== undefined) {
            valid = true;
        }


        if (valid) {
            var data_json = {
                "wis_page": elem_UTM.wis_page,
                "utm_medium": elem_UTM.utm_medium,
                "utm_source": elem_UTM.utm_source,
                "utm_campaign": elem_UTM.utm_campaign,
                "block": elem_UTM.block,
                "utm_term": elem_UTM.utm_term,
                "utm_content": elem_UTM.utm_content,
                "position": elem_UTM.position,
                "keyword": elem_UTM.keyword
            };

            $.ajax({
                type: 'POST',
                cache: false,
                url: UTM_Url,
                data: data_json,
                success: function (data: iUTM_Request) {
                    if (!data.check) {
                        console.log("UTM - Ошибка отправки сообщения.");
                    }
                },
                error: function () {
                    console.log("UTM - Ошибка отправки сообщения, обратитесь к разрабочику или проверьте интернет-соединение.");
                }
            });
        }
    }

    //Send(); Раскомментировать, если требуется автоматическая отправка UTM меток при переходе на страницу
}