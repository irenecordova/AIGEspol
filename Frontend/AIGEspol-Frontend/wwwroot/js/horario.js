$(document).ready(function () {

    $('.nav-link').attr('class', 'nav-link')
    $('#horario-tab').attr('class', 'nav-link active')
    $('.tab-pane').attr('class', 'tab-pane fade')
    $('#pills-horario').attr('class', 'tab-pane fade show active')

    n = new Date();
    y = n.getFullYear();
    m = n.getMonth() + 1;
    d = n.getDate();

    if (d < 10)
        d = '0' + d;
    if (m < 10)
        m = '0' + m
    $('input[type=date]').val(y + "-" + m + "-" + d);

    var columnList = [{
        "targets": 0,
        "searchable": false,
        "orderable": false,
        "className": "dt-body-center"
    }];

    var columnTime = [{
        "targets": 0,
        "searchable": false,
        "orderable": false,
        "className": "dt-body-center"
    },
        {
            "targets": [1,2,3,4,5,6],
            "searchable": false,
            "orderable": false,
            "className": "dt-body-center"
        }];

    personsTable = initializeSimpleTable('#personsTable', 10, columnList, 1, 'desc');
    $("#personsTable").attr("hidden", false)

    timeTable = initializeNoPaginationTable('#timeTable', 10, columnTime, 1, 'desc');

    $(document).on('change', '.seleccion', function () {
        if ($(this).is(':checked')) {
            $('input[name=persons]').prop('checked', true);
        }
        else {
            $('input[name=persons]').prop('checked', false);
        }
    });
});

function crear_lista() {
    let idPersons = []
    let namePersons = []
    $('input[name=persons]').each(function () {
        if ($(this)[0].checked) {
            id = $(this).attr('id');
            name = $('td#name_' + id).text();
            idPersons.push(new Number(id))
            namePersons.push(name)
        }
    });

    var data = {
        nombre: $('#name').val(),
        idCreador: 1,
        idPersonas: idPersons,
        nombrePersonas: namePersons
    };

    $.post("/Lista/Create", { lista: data }, function () { alert('Se guardó la ista personalizada.') });
}

function crear_reunion() {
    let idPersons = []
    let fecha_inicio = $('#fecha_reunion').val();
    let fecha_fin = $('#fecha_reunion').val();
    let hora_inicio = $('#hora_inicio').val()
    let hora_fin = $('#hora_fin').val()
    fecha_inicio.setHours(hora_inicio.split(':')[0])
    fecha_inicio.setMinutes(hora_inicio.split(':')[1])
    fecha_fin.setHours(hora_fin.split(':')[0])
    fecha_fin.setMinutes(hora_fin.split(':')[1])

    $('input[name=persons]').each(function () {
        if ($(this)[0].checked) {
            id = $(this).attr('id');
            name = $('td#name_' + id).text();
            idPersons.push(new Number(id))
        }
    });

    var data = {
        idCreador: 1,
        asunto: $('#asunto').val(),
        descripcion: $('#descripcion').val(),
        idLugar: $('#lugar').val(),
        fechaInicio: Date.now(),
        fechaFin: Date.now(),
        idPersonas: idPersons,
    };

    $.post("/Reunion/Create", { lista: data }, function () { alert('Se guardó la reunión.') });

}

function timetableGenerator() {
    $("#timeTable").attr("hidden", false)
    $("#agendar_reunion").attr("style", 'float: right; font-size: 0.9em; margin-top: 15px; display: block;')

    let horas = ['07:00 - 07:30', '07:30 - 08:00', '08:00 - 08:30', '08:30 - 09:00',
                '09:00 - 09:30', '09:30 - 10:00', '10:00 - 10:30', '10:30 - 11:00',
                '11:00 - 11:30', '11:30 - 12:00', '12:00 - 12:30', '12:30 - 13:00',
                '13:00 - 13:30', '13:30 - 14:00', '14:00 - 14:30', '14:30 - 15:00',
                '15:00 - 15:30', '15:30 - 16:00', '16:00 - 16:30', '16:30 - 17:00',
                '17:00 - 17:30', '17:30 - 18:00', '18:00 - 18:30', '18:30 - 19:00',
                '19:00 - 19:30', '19:30 - 20:00', '20:00 - 20:30', '20:30 - 21:00',
                '21:00 - 21:30', '21:30 - 22:00']

    let data_result = {
                    data:
                        [
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0},
                            { 0: 10, 1: 2, 2: 0, 3: 2, 4: 5, 5: 0},
                            { 0: 10, 1: 2, 2: 0, 3: 2, 4: 5, 5: 0 },
                            { 0: 10, 1: 2, 2: 0, 3: 2, 4: 5, 5: 0 },
                            { 0: 10, 1: 2, 2: 0, 3: 2, 4: 5, 5: 0 },
                            { 0: 10, 1: 10, 2: 0, 3: 10, 4: 0, 5: 0 },
                            { 0: 10, 1: 10, 2: 0, 3: 10, 4: 0, 5: 0 },
                            { 0: 10, 1: 10, 2: 0, 3: 10, 4: 0, 5: 0 },
                            { 0: 0, 1: 0, 2: 5, 3: 8, 4: 5, 5: 0 },
                            { 0: 0, 1: 0, 2: 5, 3: 8, 4: 5, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                            { 0: 0, 1: 8, 2: 5, 3: 8, 4: 5, 5: 0 },
                            { 0: 0, 1: 8, 2: 5, 3: 8, 4: 5, 5: 0 },
                            { 0: 0, 1: 8, 2: 5, 3: 8, 4: 5, 5: 0 },
                            { 0: 0, 1: 8, 2: 5, 3: 8, 4: 5, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                            { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
                        ],
	            total: 10
    }

    let data = data_result['data']

    $("#timeTable tbody").empty();
    

    for (var i = 0; i < data.length; i++) {
        array = data[i]
        var newElem = $('<tr>\
                            <td>' + horas[i] + '</td>\
                        </tr>')
        for (var key in array) {
            porcentaje = (array[key] * 100) / data_result['total']
            var clase = ""
            if (0 <= porcentaje && porcentaje <= 20)
            {
                clase = "cero-percent"
            }
            else if (20 < porcentaje && porcentaje <= 40) {
                clase = "twenty-percent"
            }
            else if (40 < porcentaje && porcentaje <= 60) {
                clase = "forty-percent"
            }
            else if (60 < porcentaje && porcentaje <= 80) {
                clase = "sixty-percent"
            }
            else if (80 < porcentaje && porcentaje <= 100) {
                clase = "eighty-percent"
            }
            var td = $('<td porcentaje="' + porcentaje + '" class="' + clase + '">' + array[key] + '</td>')
            newElem.append(td)
        }
        
        $("#timeTable tbody").append(newElem)
    }
    //timeTable.clear().draw();
    //timeTable.rows.add(result).draw();
}