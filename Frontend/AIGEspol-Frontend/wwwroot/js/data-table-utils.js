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
                "searchPlaceholder": "Filtar personas...",
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