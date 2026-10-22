var WIS_M_Map;
(function (WIS_M_Map) {
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
            var Size_over = new ymaps.Placemark([43.318333, 45.690265], {
                hintContent: '"Нормальное место", проспект В.В. Путина, 34',
                balloonContent: '"Нормальное место", проспект В.В. Путина, 34'
            }, {
                preset: 'islands#redIcon',
            });
            myMap.behaviors.disable('scrollZoom');
            myMap.behaviors
                .disable('scrollZoom')
                .disable('ruler');
            myMap.geoObjects.add(Size_over);
        }
        ymaps.ready(InitMap);
    }
})(WIS_M_Map || (WIS_M_Map = {}));
//# sourceMappingURL=WIS_M_Map.js.map