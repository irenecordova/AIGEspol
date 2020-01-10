/*function inicializartable(tabla,pageLength,column,ordenColumn,grupo,pieTotal){
    return $(tabla).DataTable({    
            language: {
                "decimal": "",
                "emptyTable": "No se encontraron resultados",
                "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
                "infoEmpty": "Mostrando 0 de 0 registros",
                "infoFiltered": "(Filtrado de _MAX_ registros en total)",
                "infoPostFix": "",
                "thousands": ",",
                "lengthMenu": "Mostrar _MENU_ registros",
                "loadingRecords": "Cargando...",
                "processing": "Procesando...",
                "search": "",
                "zeroRecords": "Sin resultados encontrados",
                "paginate": {
                    "first": "Primero",
                    "last": "Ultimo",
                    "next": "Siguiente",
                    "previous": ""
                }
            },
            columnDefs: column,
            "pageLength": pageLength,
            "drawCallback": function (settings) {
                if(grupo!=null){
                    var api = this.api();
                    var rows = api.rows({page: 'current'}).nodes();
                    var last = null;

                    api.column(grupo[0], {page: 'current'}).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                    '<tr style="height:30px !important;"><td colspan="'+grupo[1]+'" style="text-align: left;height:30px !important;padding: 6px 9px !important;background-color: #50BFE6 !important;color: white;">' + group + '</td></tr>'
                                    );
                            last = group;
                        }
                        
                    });}
                },
                "footerCallback": function (row, data, start, end, display) {
                    if(pieTotal!=null){
                var api = this.api(), data;
                // Remove the formatting to get integer data for summation
                var intVal = function (i) {
                    return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                };
                // Total over all pages
                total = api
                        .column(pieTotal)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                // Update footer
                $(api.column(pieTotal).footer()).html('Total de todas las pensiones vencidas ( $' + total + ' )');}
            },
            "order": [[ordenColumn, 'asc']],
            lengthChange: false,
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'excel',
                    className: 'btn btn-success',
                    text: '<i class="fa fa-file-excel-o"></i>',
                    exportOptions: {columns: "thead th:not(.noExport)"}},
                {
                    extend: 'pdf',
                    className: 'btn btn-primary',
                    text: '<i class="fa  fa-file-pdf-o"></i>',
                    exportOptions: {columns: "thead th:not(.noExport)"}},
                {
                    extend: 'print',
                    className: 'btn btn-warning',
                    text: '<i class="fa fa-print"></i>',
//                    title: '<h4>Nombre de Institución</h4>',
//                    message: '<h5>Reporte de pagina</h5>',
                    exportOptions: {columns: "thead th:not(.noExport)"},
                    footer: true,
                    customize: function (win) {
                        $(win.document.body)
                                .css('font-size', '10pt')
                                .prepend(
                                        '<img src="http://localhost/Jardin/assets/img/pdf.png" style="position:absolute; left: 50%;top: 50%;transform: translate(-50%, -50%);-webkit-transform: translate(-50%, -50%);" />'
                                        );

                        $(win.document.body).find('table')
//                                .addClass('')
                                .css('width', '100%');
                        var footer = $('#footer');
                        if (footer.length > 0) {
                            var exportFooter = $(win.document.body).find('tfoot');
                            exportFooter.after(footer.clone());
                            exportFooter.remove();
                        }
                    }
                }
//                ,
//                {
//                    extend: 'colvis',
//                    className: 'btn btn-default',
//                    text: '<i class="fa fa-columns"></i> <i class="fa fa-plus"></i>',
//                    columns: ':visible :not(:last-child)'
//                }
            ]
        });

}
*/


function initializeSimpleTable(tabla, pageLength=10, column, ordenColumn=0, ordenTipo='asc', lengthCh=true){
    var t = $(tabla).DataTable({
            language: {
                "decimal": "",
                "info": "Página _PAGE_ de _PAGES_ (Mostrando _START_ a _END_ de _TOTAL_ registros)",
                "infoEmpty": "",
                "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                "infoPostFix": "",
                "thousands": ",",
                "lengthMenu": "Mostrar _MENU_ registros por página",
                "loadingRecords": "Cargando...",
                "processing": "Procesando...",
                "search": "",
                "searchPlaceholder": "Buscar registro...",
                "zeroRecords": "No se encontraron resultados",
                "paginate": {
                    "first": '<a class="btn icofont-circled-left" title="Primera página"></a>',
                    "last": '<a class="btn icofont-circled-right" title="Última página"></a>',
                    "next": '<a class="btn icofont-rounded-right" title="Siguiente"></a>',
                    "previous": '<a class="btn icofont-rounded-left" title="Anterior"></a>'
                }
            },
            columnDefs: column,
            "order": [[ordenColumn, ordenTipo]],
            lengthChange: lengthCh,
            bFilter:true,
            "pageLength": pageLength,
            "paging":   true,
            "ordering": true,
            "info":     true,
            "processing": true,
        });

    if (!$('input[aria-controls="'+tabla.slice(1)+'"]').next().is("i")) {
        $('input[aria-controls="'+tabla.slice(1)+'"]').after('<i id="iconSearch" class="icofont-search-2" style="color: lightgray"></i>');
    }

    $('input[aria-controls="'+tabla.slice(1)+'"]').parent().attr('style', 'min-width: 120%;')
    
    return t;
}

function initializeNoPaginationTable(tabla, pageLength=10, column, ordenColumn=0, ordenTipo='asc', lengthCh=true){
    var t = $(tabla).DataTable({
            language: {
                "decimal": "",
                "info": "",
                "infoEmpty": "",
                "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                "infoPostFix": "",
                "thousands": ",",
                "lengthMenu": "Mostrar _MENU_ registros por página",
                "loadingRecords": "Cargando...",
                "processing": "Procesando...",
                "search": "",
                "searchPlaceholder": "Buscar registro...",
                "zeroRecords": "No se encontraron resultados",
                "paginate": {
                    "first": '<a class="btn icofont-circled-left" title="Primera página"></a>',
                    "last": '<a class="btn icofont-circled-right" title="Última página"></a>',
                    "next": '<a class="btn icofont-rounded-right" title="Siguiente"></a>',
                    "previous": '<a class="btn icofont-rounded-left" title="Anterior"></a>'
                }
            },
            columnDefs: column,
            "order": [[ordenColumn, ordenTipo]],
            lengthChange: lengthCh,
            bFilter:false,
            "pageLength": pageLength,
            "paging":   false,
            "ordering": false,
            "info":     true,
            "processing": true,
        });

    if (!$('input[aria-controls="'+tabla.slice(1)+'"]').next().is("i")) {
        $('input[aria-controls="'+tabla.slice(1)+'"]').after('<i id="iconSearch" class="icofont-search-2" style="color: lightgray"></i>');
    }

    $('input[aria-controls="'+tabla.slice(1)+'"]').parent().attr('style', 'min-width: 120%;')
    
    return t;
}

function initializeFilterTable(tabla, pageLength=10, column, ordenColumn=0, ordenTipo='asc', lengthCh=true){
    var t = $(tabla).DataTable({
            language: {
                "decimal": "",
                "info": "Página _PAGE_ de _PAGES_ (Mostrando _START_ a _END_ de _TOTAL_ registros)",
                "infoEmpty": "",
                "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                "infoPostFix": "",
                "thousands": ",",
                "lengthMenu": "Mostrar _MENU_ registros por página",
                "loadingRecords": "Cargando...",
                "processing": "Procesando...",
                "search": "",
                "searchPlaceholder": "Buscar registro...",
                "zeroRecords": "No se encontraron resultados",
                "paginate": {
                    "first": '<a class="btn icofont-circled-left" title="Primera página"></a>',
                    "last": '<a class="btn icofont-circled-right" title="Última página"></a>',
                    "next": '<a class="btn icofont-rounded-right" title="Siguiente"></a>',
                    "previous": '<a class="btn icofont-rounded-left" title="Anterior"></a>'
                }
            },
            columnDefs: column,
            "order": [[ordenColumn, ordenTipo]],
            lengthChange: lengthCh,
            bFilter:true,
            "pageLength": pageLength,
            "paging":   true,
            "ordering": true,
            "info":     true,
            "processing": true
        });

    if (!$('input[aria-controls="'+tabla.slice(1)+'"]').next().is("i")) {
        $('input[aria-controls="'+tabla.slice(1)+'"]').after('<i id="iconSearch" class="icofont-search-2" style="color: lightgray"></i>');
    }

    $('input[aria-controls="'+tabla.slice(1)+'"]').parent().attr('style', 'min-width: 120%;')

    $(tabla+' thead th').each( function () {
        var title = $(this).text();
        if(title!=='Acciones'){
            $(this).html( '<input type="text" class="form-control" placeholder="'+title+'" name="'+title+'" />' );
        }
    } );

    // Apply the search
    t.columns().every( function () {
        var that = this;

        $( 'input', this.header() ).on( 'keyup change', function () {
            console.log(that.search());
            console.log(this.value);
            if ( that.search() !== this.value ) {
                that.search(this.value).draw();
            }
        } );
    } );
    
    return t;
}

function initializePagerTable(tabla, pageLength=10, lengthCh=true){
    var t = $(tabla).DataTable({
            language: {
                "decimal": "",
                "info": "Página _PAGE_ de _PAGES_ (Mostrando _START_ a _END_ de _TOTAL_ registros)",
                "infoEmpty": "",
                "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                "infoPostFix": "",
                "thousands": ",",
                "lengthMenu": "Mostrar _MENU_ registros por página",
                "loadingRecords": "Cargando...",
                "processing": "Procesando...",
                "search": "",
                "searchPlaceholder": "Buscar registro...",
                "zeroRecords": "No se encontraron resultados",
                "paginate": {
                    "first": '<a class="btn icofont-circled-left" title="Primera página"></a>',
                    "last": '<a class="btn icofont-circled-right" title="Última página"></a>',
                    "next": '<a class="btn icofont-rounded-right" title="Siguiente"></a>',
                    "previous": '<a class="btn icofont-rounded-left" title="Anterior"></a>'
                }
            },
            columnDefs: [{
                "searchable": false,
                "orderable": false,
                "targets": 0
            }],            
            lengthChange: lengthCh,
            bFilter:false,
            "pageLength": pageLength,
            "paging":   true,
            "ordering": false,
            "info":     true,
            "processing": true,
        });    
    $(tabla).removeClass('no-footer');
    //$('input[aria-controls="'+tabla.slice(1)+'"]').parent().attr('style', 'min-width: 120%;')
    
    return t;
}