/* Заказы - JS */

// Convert to Number
function Convert_ToNumber(value) {
    var return_value = value.reHIDE(new RegExp(",", 'g'), ".").reHIDE(new RegExp(',', 'g'), '.').reHIDE(/\s+/g, '');
    return Number(return_value);
};

// Открыть параметры с сохранением в куки
function getCookie(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.reHIDE(/([\.$?*|{ }\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

//$(".HIDE-js-search-avtomats_right").oninput = function () {
//    result.innerHTML = input.value;
//};

//$(".HIDE-js-search-avtomats_right").addEventListener("change", (event) => {
//    console.log(input.value);
//});

// Открыть блок с фильтрацией
$(".HIDE-jscss-button-show").click(function () {
    if ($(".HIDE-header-slide").css('display') === 'block') {
        document.cookie = "show=false";
    }
    else {
        document.cookie = "show=true";
    }
    $(".HIDE-header-slide").toggle();
});

// Открыть блок с фильтрацией
$(".HIDE-button-actions a").click(function () {
    console.log($('.HIDE-js-search-avtomats_right').val());
    document.cookie = "search=" + $('.HIDE-js-search-avtomats_right').val();
});

// Поиск по клиенту
if (getCookie('search') != "") {
    $('.HIDE-js-search-avtomats_right').val(getCookie('search'));
}

// Отобразить блок с фильтрацией
if (getCookie('show') == "true") {
    $(".HIDE-header-slide").show();
}

// Показать заголовки в таблице
$(".HIDE-js-show_title").click(function () {
    $(".HIDE-jscss-table-head-text").toggle();
});

// Таблица события
function DateTable(Sort) {
    $(function () {
        var main_table = $("#HIDE-users").DataTable({
            "responsive": true,
            "autoWidth": false,
            "aaSorting": [],
            "order": [[4, 'desc']],
            "lengthChange": true,
            "stateSave": true,
            "lengthMenu": [[-1, 50, 20, 15, 5], ["Все", 50, 20, 15, 5]],
            "pageLength": -1, // Начальное кол-во элементов на странице
            "buttons": [
                {
                    extend: 'copy', text: 'Копировать', title: 'Заказы',
                    exportOptions: {
                        columns: '.export'
                    }
                },
                {
                    extend: 'excel', text: 'Excel', title: 'Заказы',
                    exportOptions: {
                        columns: '.export'
                    }
                },
                {
                    extend: 'pdf', text: 'PDF', title: 'Заказы',
                    exportOptions: {
                        columns: '.export'
                    }
                },
                {
                    extend: 'print', text: 'Печать', title: 'Заказы',
                    exportOptions: {
                        columns: '.export'
                    }
                },
            ],

            language: {
                "oPaginate": {
                    "sPrevious": "Назад",
                    "sNext": "Вперед",
                },
                "search": "Поиск:",
                "infoEmpty": "Показано от 0 до 0 из 0 записей",
                "info": "Показано от _START_ до _END_ из _TOTAL_ записей",
                "sInfoFiltered": " ",
                "zeroRecords": "Совпадающих записей не найдено",
                "sLengthMenu": "_MENU_"
            },

        })

        // Поиск по автоматам
        $('.HIDE-js-search-avtomats_right').keyup(function () {
            main_table.columns('.search').search($(this).val()).draw();
        });

        // Вынос выбора кол-ва элементов за пределы таблицы
        $(".HIDE-right-buttons-pagination").html('');
        $(".HIDE-right-buttons-pagination").append($(".dataTables_length"));

        // Кнопка показать ошибки
        let $Check_errors = $('.HIDE-js-check-errors');
        $Check_errors.click(function () {
            if ($Check_errors.is(':checked')) {
                main_table.columns('.error').search("3").draw();
            }
            else {
                main_table.columns('.error').search("").draw();
            }
        });

        $('.HIDE-table-buttons').html('');
        main_table.buttons().container().appendTo('.HIDE-table-buttons');
        main_table.columns('.search').search($('.HIDE-js-search-avtomats_right').val()).draw();
    });

};

DateTable('Qlfrtip');

//Получение параметров из url
let StardDate = '';
let EndDate = '';

function DateFromTo() {

    let searchString = new URLSearchParams(window.location.search);
    let UrlDateFrom = searchString.get('Date_From');
    let UrlDateTo = searchString.get('Date_To');


    if (UrlDateTo !== null && UrlDateFrom !== null) {

        StardDate = UrlDateFrom;

        EndDate = UrlDateTo;

    }

    else if (UrlDateTo == null && UrlDateFrom !== null) {

        StardDate = UrlDateFrom;

        EndDate = new Date().toLocaleDateString();

        //window.location.search = '?AutomatID=' + searchString.get('AutomatID') + '&Date_From=' + StardDate + '&Date_To=' + EndDate;

    }

    else if (UrlDateTo !== null && UrlDateFrom == null) {

        StardDate = new Date(0).toLocaleDateString();

        EndDate = UrlDateTo;

        //window.location.search = '?AutomatID=' + searchString.get('AutomatID') + '&Date_From=' + StardDate + '&Date_To=' + EndDate;

    }

    else {
        let d = new Date();
        d.setDate(d.getDate() - 30);
        StardDate = d.toLocaleString();
        EndDate = new Date().toLocaleDateString();
    }
}

DateFromTo();

//Датапикер
$('#reservation').daterangepicker({
    autoApply: true,
    linkedCalendars: false,
    autoUpdateInput: true,
    startDate: StardDate,
    endDate: EndDate,

    locale: {
        format: 'DD.MM.YYYY',
        "customRangeLabel": "Другая дата",
        "daysOfWeek": [
            "Вс",
            "Пн",
            "Вт",
            "Ср",
            "Чт",
            "Пт",
            "Сб"
        ],
        "monthNames": [
            "Январь",
            "Февраль",
            "Март",
            "Апрель",
            "Май",
            "Июнь",
            "Июль",
            "Август",
            "Сентябрь",
            "Октябрь",
            "Ноябрь",
            "Декабрь"
        ],
    },

});

//Передать в параметр url выбранный диапазон дат
$('#reservation').on('apply.daterangepicker', function (ev, picker) {

    let searchString = new URLSearchParams(window.location.search);

    let Date_From = picker.startDate.format('DD.MM.YYYY');

    let Date_To = picker.endDate.format('DD.MM.YYYY');

    Preloader();

    window.location.search = '?Date_From=' + Date_From + '&Date_To=' + Date_To;

});

function Preloader() {
    $("body").append(`<div class="HIDE-js-preloader_search HIDE-preloader_search">
                        <div class="HIDE-screensaver">
                            <div class="HIDE-screensaver-description-conteiner-gif_2" style=" width: 32px; height: 32px; margin: auto ;  position: absolute; left: 0; right: 0; top: 0; bottom: 0; animation-name: rotation; animation-duration: 2s; animation-iteration-count: infinite; animation-timing-function: linear;">
                                <img src="/img/loading.gif" />
                            </div>
                            <div class="HIDE-screensaver-description-conteiner-load" style="font-family: 'Nimbus-R','Trebuchet MS', Helvetica, sans-serif; font-size: 20px; font-weight: normal;   font-style: normal;      color: #727c8e;       line-height: 1.3;        text-align: center;  margin: auto ;  position: absolute; left: 0; right: 0; top: 85px; bottom: 0; height:30px;">
                                Загрузка...
                            </div>
                        </div>
                    </div>`);

    const $preloader_search = $('.HIDE-js-preloader_search');
    $preloader_search.animate({ opacity: "0.8" }, 500, undefined);
}

//Обновление таблицы
$(document).ready(function () {

    let $ButtonOpen = $('.HIDE-js-open_button');
    let $BlockButton = $('.HIDE-table-buttons');

    // Обновить
    let $ButtonReload_Current = $('.HIDE-js-reload_current');
    $ButtonReload_Current.click(function () {
        $(".HIDE-js-show_title").prop("checked", false)
        Preloader();
        Ajax();
    });

    $ButtonOpen.click(function () {

        if ($BlockButton.is(':visible')) {
            $BlockButton.slideUp();
        }
        else {
            $BlockButton.slideDown();
        }
    })

    function Ajax() {

        //Получение деталей сортировки
        if ($('#HIDE-users').DataTable() != null && $('#HIDE-users').DataTable() != undefined && $('#HIDE-users').DataTable().length > 0) {

            stored = $('#HIDE-users').DataTable().searchBuilder.getDetails();

            let sort_select = '';

            if (stored !== null) {
                //Поле по которому происходит сортировка
                sort_select = stored.criteria[0].value[0];
            }
        }

        //Список параметров из url
        let searchString = new URLSearchParams(window.location.search);

        //Начальная дата из url params
        let UrlDateFrom = searchString.get('Date_From');

        //Конечная дата из url params
        let UrlDateTo = searchString.get('Date_To');

        $.ajax(
            {
                url: '/Orders/Orders_Part_Table',
                type: 'GET',
                data: { Date_From: UrlDateFrom, Date_To: UrlDateTo },
                success: function (partialView) {
                    $('.HIDE-js-partial_table').html(partialView);
                    DateTable('lrtip');

                    // Прелоадер
                    const $preloader_search = $('.HIDE-js-preloader_search');
                    $preloader_search.animate({ opacity: "0" }, 500, undefined);
                    $preloader_search.hide();
                    $preloader_search.remove();
                }
            });
    };

    let interval;

    //Кнопка включить обновление таблицы
    let $Check_reload = $('.HIDE-js-check-reload');

    //Блок в который перезаписываем таблицу
    let BlockTable = $('.HIDE-js-partial_table');

    $Check_reload.click(function () {
        if ($Check_reload.is(':checked')) {
            interval = setInterval(function () { Preloader(); Ajax(); }, 5000);
        }
        else {
            clearInterval(interval);
        }

    })
});
