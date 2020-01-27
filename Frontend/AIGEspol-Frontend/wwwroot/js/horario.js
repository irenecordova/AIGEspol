var idUsuario;

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

    getId();
    cargar_facultades("filtro_2_docente");
    cargar_facultades("filtro_2_estudiante");
    cargar_materias_usuario();

    $(document).on('change', '.seleccion', function () {
        if ($(this).is(':checked')) {
            $('input[name=persons]').prop('checked', true);
        }
        else {
            $('input[name=persons]').prop('checked', false);
        }
    });

    $('#filtro_1_estudiante').change(function () {
        if ($(this).val() == 'F' || $(this).val() == 'M') {
            $('#bloque_filtro_2').show();
            //cargar_materias_usuario(idUsuario);
        }
        else {
            $('#bloque_filtro_2').hide();
        }

        if ($(this).val() == 'M') {
            $('#bloque_filtro_3').show();
            if ($('#filtro_2_estudiante').val() == 'T') {
                $('#agregar_estudiante').attr('disabled', true);
            }
            else {
                cargar_materias_facultad($('#filtro_2_estudiante').val())
                $('#agregar_estudiante').attr('disabled', false);
            }
        }
        else {
            $('#bloque_filtro_3').hide();
            $('#agregar_estudiante').attr('disabled', false);
        }
 
    });

    $('#filtro_2_estudiante').change(function () {
        if ($('#filtro_1_estudiante').val() == 'M') {
            if ($(this).val() == 'T') {
                $('#filtro_3_estudiante').empty();
                $('#agregar_estudiante').attr('disabled', true);
            }
            else {
                cargar_materias_facultad($(this).val())
                $('#bloque_filtro_3').show();
                $('#agregar_estudiante').attr('disabled', false);
            }
        }
    });

});

function crear_lista() {
    let idPersons = []
    let namePersons = []
    $('input[name=persons]').each(function () {
        if ($(this)[0].checked) {
            id = $(this).attr('id');
            name = $(this).parent().next().text();
            idPersons.push(new Number(id))
            namePersons.push(name)
        }
    });

    var data = {
        nombre: $('#name').val(),
        idCreador: idUsuario,
        idPersonas: idPersons,
        nombrePersonas: namePersons
    };

    $.post("/Lista/Create",
        { lista: data },
        function ()
        {
            $('#modalRegistrarLista').modal('toggle');
            alert('Se guardó la lista personalizada.')
        });
}

function crear_reunion() {
    let idPersons = []
    let fecha_inicio = new Date();
    let fecha_fin = new Date();
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
        idCreador: idUsuario,
        asunto: $('#asunto').val(),
        descripcion: $('#descripcion').val(),
        idLugar: $('#lugar').val(),
        fechaInicio: fecha_inicio,
        fechaFin: fecha_fin,
        idPersonas: idPersons,
    };

    $.post("/Reunion/Create",
        { reunion: data },
        function () 
        {
            $('#modalRegistrarReunion').modal('toggle');
            alert('Se guardó la reunión.')
        });

}

function timetableGenerator() {
    let idPersons = []
    console.log(personsTable)
    console.log(personsTable.data())
   

    personsTable.$('input[name=persons]').each(function () {
        if ($(this)[0].checked) {
            id = $(this).attr('id');
            idPersons.push(new Number(id))
        }
    });

    let total = idPersons.length;
    console.log(total)

    $.get("/Horario/Generar",
        {
            idsPersonas: idPersons,
            fecha: $('#date').val()
        },
        function (data) {
            $("#timeTable tbody").empty();
            data = JSON.parse(data);
            let horas = ['07:00 - 07:30', '07:30 - 08:00', '08:00 - 08:30', '08:30 - 09:00',
                '09:00 - 09:30', '09:30 - 10:00', '10:00 - 10:30', '10:30 - 11:00',
                '11:00 - 11:30', '11:30 - 12:00', '12:00 - 12:30', '12:30 - 13:00',
                '13:00 - 13:30', '13:30 - 14:00', '14:00 - 14:30', '14:30 - 15:00',
                '15:00 - 15:30', '15:30 - 16:00', '16:00 - 16:30', '16:30 - 17:00',
                '17:00 - 17:30', '17:30 - 18:00', '18:00 - 18:30', '18:30 - 19:00',
                '19:00 - 19:30', '19:30 - 20:00', '20:00 - 20:30', '20:30 - 21:00',
                '21:00 - 21:30', '21:30 - 22:00']
            for (var i = 0; i < data.length; i++) {
                array = data[i]
                var newElem = $('<tr>\
                                    <td>' + horas[i] + '</td>\
                                </tr>')
                for (var key in array) {
                    porcentaje = (array[key] * 100) / total
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
        });

    $("#timeTable").attr("hidden", false)
    $("#agendar_reunion").attr("style", 'float: right; font-size: 0.9em; margin-top: 15px; display: block;')

   
    //let data_result = {
    //                data:
    //                    [
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0},
    //                        { 0: 10, 1: 2, 2: 0, 3: 2, 4: 5, 5: 0},
    //                        { 0: 10, 1: 2, 2: 0, 3: 2, 4: 5, 5: 0 },
    //                        { 0: 10, 1: 2, 2: 0, 3: 2, 4: 5, 5: 0 },
    //                        { 0: 10, 1: 2, 2: 0, 3: 2, 4: 5, 5: 0 },
    //                        { 0: 10, 1: 10, 2: 0, 3: 10, 4: 0, 5: 0 },
    //                        { 0: 10, 1: 10, 2: 0, 3: 10, 4: 0, 5: 0 },
    //                        { 0: 10, 1: 10, 2: 0, 3: 10, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 5, 3: 8, 4: 5, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 5, 3: 8, 4: 5, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 8, 2: 5, 3: 8, 4: 5, 5: 0 },
    //                        { 0: 0, 1: 8, 2: 5, 3: 8, 4: 5, 5: 0 },
    //                        { 0: 0, 1: 8, 2: 5, 3: 8, 4: 5, 5: 0 },
    //                        { 0: 0, 1: 8, 2: 5, 3: 8, 4: 5, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                        { 0: 0, 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 },
    //                    ],
	   //         total: 10
    //}

    //let data = data_result['data']

    //$("#timeTable tbody").empty();
    

    //for (var i = 0; i < data.length; i++) {
    //    array = data[i]
    //    var newElem = $('<tr>\
    //                        <td>' + horas[i] + '</td>\
    //                    </tr>')
    //    for (var key in array) {
    //        porcentaje = (array[key] * 100) / data_result['total']
    //        var clase = ""
    //        if (0 <= porcentaje && porcentaje <= 20)
    //        {
    //            clase = "cero-percent"
    //        }
    //        else if (20 < porcentaje && porcentaje <= 40) {
    //            clase = "twenty-percent"
    //        }
    //        else if (40 < porcentaje && porcentaje <= 60) {
    //            clase = "forty-percent"
    //        }
    //        else if (60 < porcentaje && porcentaje <= 80) {
    //            clase = "sixty-percent"
    //        }
    //        else if (80 < porcentaje && porcentaje <= 100) {
    //            clase = "eighty-percent"
    //        }
    //        var td = $('<td porcentaje="' + porcentaje + '" class="' + clase + '">' + array[key] + '</td>')
    //        newElem.append(td)
    //    }
        
    //    $("#timeTable tbody").append(newElem)
    //}

}

function cargar_facultades(input) {
    $.get("/Filtros/Facultades",
        function (data) {
            var facultades = JSON.parse(data);
            if (facultades.length) {
                $('#' + input).empty();
                $('#' + input).append($('<option value="T">Todos</option>'));
                for (var i = 0; i < facultades.length; i++) {
                    if (facultades[i]['intIdUnidad'] == 15249 || facultades[i]['intIdUnidad'] == 15266 || facultades[i]['intIdUnidad'] == 15007 || facultades[i]['intIdUnidad'] == 15310 || facultades[i]['intIdUnidad'] == 15259 || facultades[i]['intIdUnidad'] == 15257) {
                        continue;
                    }
                    $('#' + input).append($('<option value="' + facultades[i]['intIdUnidad'] + '">' + facultades[i]['strCodFacultad'] + '</option>'));
                }
            }    
        });
}

function getId() {
    $.get("/Filtros/GetId",
        function (data) {
            console.log(data)
            idUsuario = data
            cargar_materias_usuario(data);
        });
}

function cargar_materias_usuario(idPersona) {
    $.get("/Filtros/MateriasUsuario",
        { idPersona: idPersona },
        function (data) {
            var materias = JSON.parse(data);
            if (materias.length) {
                $('#filtro_1_estudiante').empty();
                $('#filtro_1_estudiante').append($('<option value="F">Facultades</option>'));
                $('#filtro_1_estudiante').append($('<option value="M">Materias</option>'));
                for (var i = 0; i < materias.length; i++) {
                    $('#filtro_1_estudiante').append($('<option value="' + materias[i]['idCurso'] + '">' + materias[i]['nombreMateria'] + '</option>'));
                }
            }
        });
}

function cargar_materias_facultad(idFacultad) {
    $.get("/Filtros/MateriasFacultad",
        { idFacultad: idFacultad },
        function (data) {
            var materias = JSON.parse(data);
            console.log(materias)
            if (materias.length) {
                $('#filtro_3_estudiante').empty();
                for (var i = 0; i < materias.length; i++) {
                    $('#filtro_3_estudiante').append($('<option value="' + materias[i]['idCurso'] + '">' + materias[i]['nombreMateria'] + '</option>'));
                }
            }
        });
}

function cargar_estudiantes() {
    if ($('#filtro_1_estudiante').val() != 'F' && $('#filtro_1_estudiante').val() != 'M') {
        $.get("/Filtros/EstudiantesCurso",
            { IdCurso: $('#filtro_1_estudiante').val() },
            function (data) {
                var estudiantes = JSON.parse(data);
                result = []
                if (estudiantes.length) {
                    for (var i = 0; i < estudiantes.length; i++) {
                        var check = '<input type="checkbox" name="persons" id="' + estudiantes[i]['idPersona'] + '" checked />';
                        result.push([check, estudiantes[i]['nombres'] + ' ' + estudiantes[i]['apellidos']]);
                        //result.push([estudiantes[i]['idPersona'], estudiantes[i]['nombres'] + ' ' + estudiantes[i]['apellidos']]);
                    }
                    personsTable.rows.add(result).draw();
                }
            });
    }
    
}