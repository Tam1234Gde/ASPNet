/* Компонент wis-notification 1.19.11.1  */
/* Поддержка браузеров: IE8+ и остальные браузеры. */
/* Автор: Инфографика (Висерв) - https://wiserv.ru */

/**
 * Вывод всплывающих сообщений
 */
module WIS_B_Notification {

    /**
     * Класс - основной
     */
    let cBlock = "wis-jscss-notification";

    /**
     * Класс - класс элемента
     */
    let cBlock_Item = "wis-jscss-notification-elem";

    /**
     * Класс - класс элемента "успех"
     */
    let cBlock_ItemSuccess = "wis-jscss-notification-elem__success";

    /**
     * Класс - класс элемента "предупреждение"
     */
    let cBlock_ItemWarning = "wis-jscss-notification-elem__warning";

    /**
     * Класс - класс элемента "ошибка"
     */
    let cBlock_ItemError = "wis-jscss-notification-elem__error";

    /**
     * Класс - класс кнопки закрытия
     */
    let cBlock_Item_Text = "wis-jscss-notification-elem-text";

    /**
     * Класс - класс кнопки закрытия
     */
    let cBlock_Item_Close = "wis-jscss-notification-elem-close";

    /**
     * Число - Время показа сообщения (5000 = 5 секунд)
     * */
    let nTime = 5000;

    /**
     * Перечисление - типы сообщений
     */
    export enum eType {
        /**
         * Успешно
         * */
        Success = 1,

        /**
         * Предупреждение
         * */
        Warning = 2,

        /**
         * Ошибка
         * */
        Error = 3
    }

    interface iShow {
        /*
         * html значение сообщения 
         */
        Value_html: string;

        /*
         * Тип сообщения
         */
        Type?: eType;
    }

    /**
      * Вывод сообщения об ошибке, предупреждении или успехе
      * @param elem данные
      */
    export function Show(data: iShow) {

        // Класс зависит от типа сообщения
        let elem_class = `${cBlock_Item} ${cBlock_ItemSuccess}`;
        switch (data.Type) {
            case eType.Warning:
                elem_class = `${cBlock_Item} ${cBlock_ItemWarning}`;
                break;
            case eType.Error:
                elem_class = `${cBlock_Item} ${cBlock_ItemError}`;
                break;
            default:
                break;
        }

        //Добавление блока в начало body
        let elem_bar = $(`.${cBlock}`);
        if (elem_bar.length == 0) {
            elem_bar = $('<div>').addClass(cBlock);
            $("body").prepend(elem_bar);
        }
        elem_bar.show();

        // Добавление элемента в блок
        let elem = $('<div>').addClass(elem_class).html(`<div class="${cBlock_Item_Text}">${data.Value_html}</div>`);
        let elem_close = $('<button>').addClass(cBlock_Item_Close);
        elem_close.click(function () {
            elem.hide();
        });
        elem.append(elem_close);
        elem_bar.append(elem);
        elem.slideDown("show");

        // Удаление элемента через заданное время
        setTimeout(function () {
            elem_bar.find(elem).slideUp("slow", function () {
                $(this).remove();

                if (elem_bar.find(`.${cBlock_Item}`).length == 0) {
                    elem_bar.hide();
                }
            });
        }, nTime);
    }
}