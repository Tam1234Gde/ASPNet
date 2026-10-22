// Таблица события
function DateTable() {
    $(function () {
        var main_table = $("#HIDE-users").DataTable({
            "responsive": true,
            searching: true,
            "autoWidth": false,
            "lengthChange": false,
            "stateSave": false,
            "order": [[0, 'desc']],
            "lengthMenu": [],
            "paging": false,
            "ordering": false,
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

        // Поиск по коду заказа
        $('.HIDE-js-search-code').keyup(function () {
            main_table.columns('.search_code').search($(this).val()).draw();
        });

        main_table.columns('.search_code').search($('.HIDE-js-search-code').val()).draw();

        // Поиск по клиенту
        $('.HIDE-js-search-client').keyup(function () {
            main_table.columns('.search').search($(this).val()).draw();
        });

        main_table.columns('.search').search($('.HIDE-js-search-client').val()).draw();

    });

};

DateTable();

// Функция Ajax
function Ajax() {
    $.ajax(
        {
            url: '/Orders/Orders_Part_Table',
            type: 'GET',
            success: function (partialView) {
                $('.HIDE-js-partial_table').html(partialView);
                DateTable('lrtip');

                Modal_Delivery();
                Modal_OrderMore();
                Modal_Comment();
            }
        });
};

// Функция Ajax
function Ajax_Check() {
    $.ajax(
        {
            url: '/Orders/Check_Date',
            type: 'GET',
            success: function (data) {
                if (data == true) {
                    Ajax();
                    // Звуковой сигнал при новом заказе
                    function playSound(soundfile) {
                        var audioElement = document.createElement('audio');
                        audioElement.setAttribute('src', soundfile + '.mp3'); // Для современных браузеров
                        audioElement.setAttribute('ogg', soundfile + '.ogg'); // Для старых версий
                        const promise = audioElement.play();
                        if (promise !== undefined) {
                            promise.then(() => {
                                // Autoplay started
                            }).catch(error => {
                                // Autoplay was prevented.
                                audioElement.muted = true;
                                audioElement.play();
                            });
                        }
                    }
                    playSound("/HIDE/audio_new_order");
                }
            }
        });
};

let interval = setInterval(function () { Ajax_Check(); }, 30000);