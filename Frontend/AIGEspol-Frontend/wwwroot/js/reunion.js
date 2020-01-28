var idUsuario;

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

function getId() {
    $.get("/Filtros/GetId",
        function (data) {
            console.log(data)
            idUsuario = data
            cargarReuniones(data);
        });
}

function cargarReuniones(idPersona) {
    $.getJSON('/Reunion/Reuniones',
        { idPersona: idPersona },
        function (data) {
            console.log(data)
    });
}
