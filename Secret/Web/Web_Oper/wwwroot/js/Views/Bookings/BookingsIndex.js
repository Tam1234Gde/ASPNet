// Таблица события
function DateTable() {
    $(function () {
        var main_table = $("#HIDE-users").DataTable({
            "responsive": true,
            searching: false,
            "autoWidth": false,
            "lengthChange": false,
            "stateSave": false,
            "order": [[1, 'desc']],
            "lengthMenu": [],
            "pageLength": 15,

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

        // Поиск
        //$('.HIDE-js-search-avtomats_right').keyup(function () {
        //    console.log(1111);
        //    main_table.columns('.search').search($(this).val()).draw();
        //});
        //main_table.columns('.search').search($('.HIDE-js-search-avtomats_right').val()).draw();
    });

};

DateTable();
