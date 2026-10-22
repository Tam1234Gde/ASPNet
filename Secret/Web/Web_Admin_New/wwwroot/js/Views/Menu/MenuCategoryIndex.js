// File: wwwroot/js/Views/Menu/MenuCategoryIndex.js
$(document).ready(function () {
    var table = $('#HIDE-categories').DataTable({
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
                    columns: [0, 1]
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1]
                }
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: [0, 1]
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1]
                }
            }
        ]
    });

    $('.HIDE-js-search-avtomats_right').on('keyup', function () {
        table.search(this.value).draw();
    });

    $('#HIDE-show-title').on('change', function () {
        if ($(this).is(':checked')) {
            $('#HIDE-categories thead').show();
        } else {
            $('#HIDE-categories thead').hide();
        }
    });

    $('#HIDE-categories thead').hide();
});