$(document).ready(function () {

    $('.nav-link').attr('class', 'nav-link')
    $('#reunion-tab').attr('class', 'nav-link active')
    $('.tab-pane').attr('class', 'tab-pane fade')
    $('#pills-reunion').attr('class', 'tab-pane fade show active')

    var columnList = [{
        "targets": 5,
        "searchable": false,
        "orderable": false,
        "className": "dt-body-center"
    }];

    personsTable = initializeSimpleTable('#reunionesTable', 10, columnList, 1, 'desc');
    $("#reunionesTable").attr("hidden", false)

});

function cargarReuniones() {
    $.getJSON('/Lista/Reuniones', function (reuniones) {
        if (reuniones != null && !jQuery.isEmptyObject(reuniones)) {
            console.log(reuniones)
            //$.each(reuniones, function (index, reunion) {
            //    result.push([reunion., documentos[i]['tipo_documento'], documentos[i]['fecha_emision'], documentos[i]['cliente'], documentos[i]['vendedor'], "$ " + documentos[i]['total'], "$ " + documentos[i]['saldo'], documentos[i]['estado'], documentos[i]['estado_electronico'], acciones]);
            //    regionsSelect.append($('<option/>', {
            //        value: region.Value,
            //        text: region.Text
            //    }));
            //});
        };
    });
}
