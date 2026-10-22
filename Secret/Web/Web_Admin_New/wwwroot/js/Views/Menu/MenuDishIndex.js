// File: wwwroot/js/Views/Menu/MenuDishIndex.js
$(document).ready(function () {
    var table = $('#HIDE-dishes').DataTable({
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
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            }
        ]
    });

    $('.HIDE-js-search-avtomats_right').on('keyup', function () {
        table.search(this.value).draw();
    });

    $('#HIDE-show-title').on('change', function () {
        if ($(this).is(':checked')) {
            $('#HIDE-dishes thead').show();
        } else {
            $('#HIDE-dishes thead').hide();
        }
    });

    $('#HIDE-dishes thead').hide();
});