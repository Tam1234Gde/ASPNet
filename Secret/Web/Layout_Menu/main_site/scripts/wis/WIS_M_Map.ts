/**
 * Карта
 */
module WIS_M_Map {

    if (typeof ymaps !== "undefined" && ymaps !== null) {
        var myMap;
        function InitMap() {
            myMap = new ymaps.Map("wis-map", {
                center: [43.318333, 45.690265],
                zoom: 18,
                controls: ['zoomControl', 'typeSelector', 'fullscreenControl', 'geolocationControl']
            }, {
                    yandexMapDisablePoiInteractivity: true
                }); 
            // #region 800m
            // #endregion
            var Size_over = new ymaps.Placemark([43.318333, 45.690265], {
                hintContent: '"Нормальное место", проспект В.В. Путина, 34',
                balloonContent: '"Нормальное место", проспект В.В. Путина, 34'
            }, {
                preset: 'islands#redIcon',
                //iconLayout: 'default#imageWithContent',
                //iconImageHref: '_I/img-map.png',
                //iconImageSize: [160, 90],
                //iconImageOffset: [0, 0],
            });
            myMap.behaviors.disable('scrollZoom');
            myMap.behaviors
                .disable('scrollZoom')
                .disable('ruler');
            //myMap.controls.remove('searchControl').add(null);
            myMap.geoObjects.add(Size_over);
        }
        ymaps.ready(InitMap);
    }
}