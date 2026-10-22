// File: wwwroot/js/Views/Menu/MenuIndex.js
$(document).ready(function () {
    // Инициализация DataTable
    var table = $('#HIDE-menu').DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        "language": {
            "url": "/lib/datatables/ru.json"
        },
        "dom": 'Bfrtip',
        "buttons": [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2]
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2]
                }
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: [0, 1, 2]
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2]
                }
            }
        ]
    });

    // Поиск по таблице
    $('.HIDE-js-search-avtomats_right').on('keyup', function () {
        table.search(this.value).draw();
    });

    // Показать/скрыть заголовки
    $('#HIDE-show-title').on('change', function () {
        if ($(this).is(':checked')) {
            $('#HIDE-menu thead').show();
        } else {
            $('#HIDE-menu thead').hide();
        }
    });

    // Скрыть заголовки по умолчанию
    $('#HIDE-menu thead').hide();
});