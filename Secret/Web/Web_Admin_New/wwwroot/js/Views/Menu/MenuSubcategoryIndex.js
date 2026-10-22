// File: wwwroot/js/Views/Menu/MenuSubcategoryIndex.js
$(document).ready(function () {
    var table = $('#HIDE-subcategories').DataTable({
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

    $('.HIDE-js-search-avtomats_right').on('keyup', function () {
        table.search(this.value).draw();
    });

    $('#HIDE-show-title').on('change', function () {
        if ($(this).is(':checked')) {
            $('#HIDE-subcategories thead').show();
        } else {
            $('#HIDE-subcategories thead').hide();
        }
    });

    $('#HIDE-subcategories thead').hide();
});