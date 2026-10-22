/**
 * Пример настройки типизации для API Яндекс.Карт (можно описывать столько свойств, сколько удобно)
 * */
declare module ymaps {

    /**
     * Класс карта 
     * */
    class Map {
        /*
         * Конструктор
         */
        constructor(...params: any);

        /**
         * ГЕО объекты
         */ 
        public geoObjects: any;

        /**
         * События
         */
        public events: any;

        /**
         * Поведения
         */
        public behaviors: any;

        /**
         * Получить увеличение
         * @param params
         */
        public getZoom(...params: any): any;
    }

    /**
     * Класс метка
     * */
    class Placemark {
        constructor(...params: any);
    }

    /**
     * Инициализация
     * @param params
     */
    export function ready(...params: any) : any;
}