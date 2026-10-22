/* Компонент wis-menu_mobile 1.19.10.31  */
/* Требуемые модули css: wis-menu_mobile. */
/* Поддержка браузеров: IE8+ и остальные браузеры. */
/* Автор: Инфографика (Висерв) - https://wiserv.ru */

/**
 * Мобильное меню (как на iphone)
 */
module WIS_M_MenuMobile {

    /**
    * Элемент - Главный
    */
    let $Elem: JQuery = $(".wis-js-menu_mobile");

    if ($Elem != undefined && $Elem.length > 0) {

        /**
        * Элемент - кнопка меню
        */
        let $Menu_Button: JQuery = $(".wis-js-menu_mobile-open");

        /**
        * Элемент - кнопка меню закрыть
        */
        let $Menu_Button_Close: JQuery = $(".wis-menu_mobile-box-title-close");

        /**
          * Элемент - Главный мобильное меню
          */
        let $Menu_Mobile: JQuery = $(".wis-js-menu_mobile-box");

        /**
          * Элемент - Активный класс
          */
        let cMenu_Mobile_Active: string = "wis-jscss-menu_mobile-box__active";

        /**
        * Элемент - Фон
        */
        let $Bg: JQuery = $(".wis-js-menu_mobile-bg");

        /**
        * Класс - активный класс для меню
        */
        let cBg_Active: string = "wis-jscss-menu_mobile-bg__active";

        /**
        * Элементы - Кнопки для открытия меню
        */
        let $Mobile_Content_Buttons: JQuery = $Menu_Mobile.find(".wis-js-menu_mobile-box-nav-elem-button");

        /**
        * Класс - Для открытой кнопки
        */
        let cMobile_Content_Button_Active: string = "wis-jscss-menu_mobile-box-nav-elem-button__open";

        /**
        * Элементы - Кнопки (подменю) для открытия меню
        */
        let $Mobile_Content_Buttons_Child: JQuery = $Menu_Mobile.find(".wis-js-menu_mobile-box-nav-elem-child-button");

        /**
        * Класс - Для открытой кнопки подменю
        */
        let cMobile_Content_Button_Child_Active: string = "wis-jscss-menu_mobile-box-nav-elem-child-button__open";

        /**
         * Класс - Для дочернего элемента
         * */
        let cMobile_Content_Nav_Elem_Child: string = "wis-js-menu_mobile-box-nav-elem-child";

        /**
         * Класс - Для поддочернего элемента
         * */
        let cMobile_Content_Nav_Elem_Child_Menu: string = "wis-js-menu_mobile-box-nav-elem-child-menu";

        //Закрыть меню  -  закрытие по клику на ссылку прописал в script.js
        $Bg.click(function () {
            $Menu_Mobile.removeClass(cMenu_Mobile_Active);
            $Bg.removeClass(cBg_Active);
        });

        $Menu_Button_Close.click(function () {
            $Menu_Mobile.removeClass(cMenu_Mobile_Active);
            $Bg.removeClass(cBg_Active);
        });

        //Открыть меню
        $Menu_Button.click(function () {
            $Menu_Mobile.addClass(cMenu_Mobile_Active);
            $Bg.addClass(cBg_Active);
        });
    }
}