var idUsuario;
var data_dicc;
var tr = 0;
var timeTable;
var horas = ['07:00 - 07:30', '07:30 - 08:00', '08:00 - 08:30', '08:30 - 09:00',
    '09:00 - 09:30', '09:30 - 10:00', '10:00 - 10:30', '10:30 - 11:00',
    '11:00 - 11:30', '11:30 - 12:00', '12:00 - 12:30', '12:30 - 13:00',
    '13:00 - 13:30', '13:30 - 14:00', '14:00 - 14:30', '14:30 - 15:00',
    '15:00 - 15:30', '15:30 - 16:00', '16:00 - 16:30', '16:30 - 17:00',
    '17:00 - 17:30', '17:30 - 18:00', '18:00 - 18:30', '18:30 - 19:00',
    '19:00 - 19:30', '19:30 - 20:00', '20:00 - 20:30', '20:30 - 21:00',
    '21:00 - 21:30', '21:30 - 22:00']

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

    var columnList2 = [{
        "targets": 0,
        "searchable": true,
        "orderable": true,
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

    personasOcupadasTable = initializeSimpleTable('#personasOcupadasTable', 10, columnList2, 0, 'desc');
    
    if ($('#timeTable').length) {
        timeTable = initializeNoPaginationTable('#timeTable', 10, columnTime, 1, 'desc');
        
        $('#timeTable tbody').on('click', 'td', function () {
            var seleccionado = $(this)
            var seleccionados = $('td.selected')
            
            if (seleccionados.length == 0) {
                tr = seleccionado.attr('id').split('_')[1];
                seleccionado.toggleClass('selected');
            }
            else {
         
                if (seleccionado.attr('id').split('_')[1] == tr) {
                    seleccionado.toggleClass('selected');
                }
                else {
                    tr = seleccionado.attr('id').split('_')[1];
                    seleccionados.removeClass('selected')
                    seleccionado.toggleClass('selected');
                }                
            }
            
        });
    }

    getId();
    cargar_facultades("filtro_2_docente");
    cargar_facultades("filtro_2_estudiante");
    cargar_materias_usuario();

    $(document).on('change', '.seleccion', function () {
        if ($(this).is(':checked')) {
            personsTable.$('input[name=persons]').prop('checked', true);
        }
        else {
            personsTable.$('input[name=persons]').prop('checked', false);
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

    $('#resultados_busqueda').change(function () {
        var id = $(this).val()
        if ($('input#' + id).length == 0) {
            let nombres = $("#resultados_busqueda option:selected").text();
            let check = '<input type="checkbox" name="persons" id="' + id + '" checked />';
            personsTable.row.add([check, nombres]).draw()
        }
    });

    $('#zona').change(function () {
        cargar_bloques_zona($(this).val());
    });

    $('#bloque').change(function () {
        cargar_lugares_bloque($(this).val());
    });

    $('#agendar_reunion').click(function () {
        var seleccionados = $('td.selected')
        if (seleccionados.length != 0) {
            let hora_inicio = horas[$(seleccionados[0]).attr('id').split('_')[0]]
            let hora_fin = horas[$(seleccionados[seleccionados.length - 1]).attr('id').split('_')[0]]
            var fecha = new Date($('#date').val())

            if ((fecha.getDay() + 1) == tr) {
                fecha.setDate(fecha.getDate() + 1);
            }
            else {
                if (tr > (fecha.getDay() + 1)) {
                    let diferencia = tr - (fecha.getDay() + 1);
                    fecha.setDate(fecha.getDate() + diferencia + 1);
                }
                else {
                    let diferencia = (fecha.getDay() + 1) - tr;
                    fecha.setDate(fecha.getDate() - diferencia + 1);
                }

            }

            y = fecha.getFullYear();
            m = fecha.getMonth() + 1;
            d = fecha.getDate();
            if (d < 10)
                d = '0' + d;
            if (m < 10)
                m = '0' + m
            $('#fecha_reunion').val(y + "-" + m + "-" + d);
            $('#hora_inicio').val(hora_inicio.split(' - ')[0]);
            $('#hora_fin').val(hora_fin.split(' - ')[1]);
        }
                
        cargar_zonas();
        $('#lugar').empty();
        $('#lugar').append($('<option value="">Seleccionar lugar...</option>'));
        $('#bloque').empty();
        $('#bloque').append($('<option value="">Seleccionar bloque...</option>'));
    });

    $('#fecha_reunion').change(function () {
        cargar_lugares_bloque($('#bloque').val())
    });

    $('#hora_inicio').change(function () {
        cargar_lugares_bloque($('#bloque').val())
    });

    $('#hora_fin').change(function () {
        cargar_lugares_bloque($('#bloque').val())
    });
});

function crear_lista() {
    let idPersons = []
    let namePersons = []
    personsTable.$('input[name=persons]').each(function () {
        if ($(this)[0].checked) {
            id = $(this).attr('id');
            name = $(this).parent().next().text();
            idPersons.push(new Number(id))
            namePersons.push(name)
        }
    });

    var lista = {
        nombre: $('#name').val(),
        idCreador: idUsuario,
        idPersonas: idPersons,
        nombrePersonas: namePersons
    };

    $.post("/Lista/Create",
        { lista: lista },
        function (data)
        {
            var data = JSON.parse(data);
            $('#filtro_personalizada').append($('<option value="' + data['listaInsertada']['id'] + '">' + data['listaInsertada']['nombre'] + '</option>'));
            $('#modalRegistrarLista').modal('toggle');
            alert('Se guardó la lista personalizada.')
        });
}

function crear_reunion() {

    if (!validar_form()) {
        alert("Por favor complete todos los datos.")
        return -1;
    }

    let idPersons = []
    let fecha_inicio = new Date($('#fecha_reunion').val());
    let fecha_fin = new Date($('#fecha_reunion').val());
    fecha_inicio.setDate(fecha_inicio.getDate() + 1);
    fecha_fin.setDate(fecha_fin.getDate() + 1);
    let hora_inicio = $('#hora_inicio').val()
    let hora_fin = $('#hora_fin').val()
    fecha_inicio.setHours(hora_inicio.split(':')[0], hora_inicio.split(':')[1])
    //fecha_inicio = new Date(fecha_inicio.getTime() - (fecha_inicio.getTimezoneOffset() * 60000)).toJSON()
    fecha_fin.setHours(hora_fin.split(':')[0], hora_fin.split(':')[1])
    //fecha_fin = new Date(fecha_fin.getTime() - (fecha_fin.getTimezoneOffset() * 60000)).toJSON()

    console.log('fecha_inicio')
    console.log(fecha_inicio.toJSON())
    console.log('fecha_fin')
    console.log(fecha_fin.toJSON())

    personsTable.$('input[name=persons]').each(function () {
        if ($(this)[0].checked) {
            id = $(this).attr('id');
            name = $('td#name_' + id).text();
            idPersons.push(new Number(id))
        }
    });

    var reunion = {
        idCreador: idUsuario,
        asunto: $('#asunto').val(),
        descripcion: $('#descripcion').val(),
        idLugar: $('#lugar').val(),
        fechaInicio: fecha_inicio.toJSON(),
        fechaFin: fecha_fin.toJSON(),
        idPersonas: idPersons,
    };

    $.post("/Reunion/Create",
        { reunion: reunion },
        function (data) 
        {
            $('#modalRegistrarReunion').modal('toggle');
            alert('Se guardó la reunión.')
        });

}

function timetableGenerator() {
    $("#timeTable tbody").empty();
    spinner = '<tr id="spinnerTimeTable" class="odd"><td valign="top" colspan="11" class="dataTables_empty"><div style="text-align: center;"><i disabled class="btn icofont-spinner fa-spin" id="loading" style="font-size: 2em">Cargando...</div></td></tr>'
    $('#timeTable tbody').prepend(spinner);
    $('#spinnerTimeTable').siblings().remove();

    let idPersons = []
    let numeroPersonas = 0
    personsTable.$('input[name=persons]').each(function () {
        if ($(this)[0].checked) {
            id = $(this).attr('id');
            idPersons.push(new Number(id))
            numeroPersonas += 1;
        }
    });

    if (numeroPersonas < 3) {
        alert("Seleccione más de 2 personas");
        return 0;
    }

    let total = idPersons.length;
    $.post("/Horario/Generar",
        {
            idsPersonas: idPersons,
            fecha: $('#date').val()
        },
        function (data) {
            $("#timeTable tbody").empty();
            data_dicc = JSON.parse(data);
            
            for (var i = 0; i < data_dicc.length; i++) {
                array = data_dicc[i]
                var newElem = $('<tr>\
                                    <td style="text-align: center">' + horas[i] + '</td>\
                                </tr>')

                for (var key in array) {
                    porcentaje = (array[key]['numOcupados'] * 100) / total
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
                    var td = $('<td porcentaje="' + porcentaje + '" class="' + clase + '" id="' + i + '_' + key + '" style="text-align: center">\
                                    <a class="pt-2 mb-0" role="button" onclick="refreshPersonasOcupadasTable(' + i + ', ' + key + ')" style="font-size: 1.2em;">' + array[key]['numOcupados'] + '</a >\
                                </td>')
                    newElem.append(td)
                }

                $("#timeTable tbody").append(newElem)
            }
        });

    $("#timeTable").attr("hidden", false)
    $("#codigo_colores").attr("hidden", false)
    $("#agendar_reunion").attr("style", 'float: right; font-size: 0.9em; margin-top: 15px; display: block;')

}

function cargar_facultades(input) {
    $.get("/Filtros/Facultades",
        function (data) {
            var facultades = JSON.parse(data);
            if (facultades.length) {
                $('#' + input).empty();
                if (input != 'filtro_2_estudiante') {
                    $('#' + input).append($('<option value="T">Todos</option>'));
                }
                for (var i = 0; i < facultades.length; i++) {
                    if (facultades[i]['intIdUnidad'] == 15249 || facultades[i]['intIdUnidad'] == 15266 || facultades[i]['intIdUnidad'] == 15007 || facultades[i]['intIdUnidad'] == 15310 || facultades[i]['intIdUnidad'] == 15259 || facultades[i]['intIdUnidad'] == 15257 || facultades[i]['intIdUnidad'] == 15006) {
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
            cargar_listas(data);
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
                    $('#filtro_1_estudiante').append($('<option value="' + materias[i]['idCurso'] + '">' + materias[i]['nombreMateria'] + " - " + materias[i]['numeroParalelo'] + '</option>'));
                }
            }
        });
}

function cargar_materias_facultad(idFacultad) {
    $.get("/Filtros/MateriasFacultad",
        { idFacultad: idFacultad },
        function (data) {
            var materias = JSON.parse(data);
            if (materias.length) {
                $('#filtro_3_estudiante').empty();
                for (var i = 0; i < materias.length; i++) {
                    $('#filtro_3_estudiante').append($('<option value="' + materias[i]['idMateria'] + '">' + materias[i]['nombreMateria'] + '</option>'));
                }
            }
        });
}

function cargar_estudiantes() {
    if ($('#filtro_1_estudiante').val() != 'F' && $('#filtro_1_estudiante').val() != 'M') {
        $.get("/Filtros/EstudiantesCurso",
            { idCurso: $('#filtro_1_estudiante').val() },
            function (data) {
                console.log(data)
                var estudiantes = JSON.parse(data);
                result = []
                if (estudiantes.length) {
                    for (var i = 0; i < estudiantes.length; i++) {
                        if ($('input#' + estudiantes[i]['idPersona']).length == 0) {
                            var check = '<input type="checkbox" name="persons" id="' + estudiantes[i]['idPersona'] + '" checked />';
                            result.push([check, estudiantes[i]['nombres'] + ' ' + estudiantes[i]['apellidos']]);
                        }
                    }
                    personsTable.rows.add(result).draw();
                }
            });
    }
    else if ($('#filtro_1_estudiante').val() == 'F') {
        $.get("/Filtros/EstudiantesFacultad",
            { IdFacultad: $('#filtro_2_estudiante').val() },
            function (data) {
                var estudiantes = JSON.parse(data);
                result = []
                if (estudiantes.length) {
                    for (var i = 0; i < estudiantes.length; i++) {
                        if ($('input#' + estudiantes[i]['idPersona']).length == 0) {
                            var check = '<input type="checkbox" name="persons" id="' + estudiantes[i]['idPersona'] + '" checked />';
                            result.push([check, estudiantes[i]['nombres'] + ' ' + estudiantes[i]['apellidos']]);
                        }
                    }
                    personsTable.rows.add(result).draw();
                }
            });
    }
    else if ($('#filtro_1_estudiante').val() == 'M') {
        $.get("/Filtros/EstudiantesMateria",
            { IdMateria: $('#filtro_3_estudiante').val() },
            function (data) {
                var estudiantes = JSON.parse(data);
                result = []
                if (estudiantes.length) {
                    for (var i = 0; i < estudiantes.length; i++) {
                        if ($('input#' + estudiantes[i]['idPersona']).length == 0) {
                            var check = '<input type="checkbox" name="persons" id="' + estudiantes[i]['idPersona'] + '" checked />';
                            result.push([check, estudiantes[i]['nombres'] + ' ' + estudiantes[i]['apellidos']]);

                        }
                    }
                    personsTable.rows.add(result).draw();
                }
            });
    }
    
}

function cargar_docentes() {
    if ($('#filtro_1_docente').val() == 'D') {
        if ($('#filtro_2_docente').val() == "T") {
            $.get("/Filtros/Decanos",
                { IdFacultad: $('#filtro_2_docente').val() },
                function (data) {
                    var docentes = JSON.parse(data);
                    result = []
                    if (docentes.length) {
                        for (var i = 0; i < docentes.length; i++) {
                            if ($('input#' + docentes[i]['idPersona']).length == 0) {
                                var check = '<input type="checkbox" name="persons" id="' + docentes[i]['idPersona'] + '" checked />';
                                result.push([check, docentes[i]['nombres'] + ' ' + docentes[i]['apellidos']]);
                            }
                        }
                        personsTable.rows.add(result).draw();
                    }
                });
        }
        else {
            $.get("/Filtros/DecanosFacultad",
                { IdFacultad: $('#filtro_2_docente').val() },
                function (data) {
                    var docentes = JSON.parse(data);
                    result = []
                    if (docentes.length) {
                        for (var i = 0; i < docentes.length; i++) {
                            if ($('input#' + docentes[i]['idPersona']).length == 0) {
                                var check = '<input type="checkbox" name="persons" id="' + docentes[i]['idPersona'] + '" checked />';
                                result.push([check, docentes[i]['nombres'] + ' ' + docentes[i]['apellidos']]);
                            }
                        }
                        personsTable.rows.add(result).draw();
                    }
                });
        }
        
    }
    else {
        if ($('#filtro_2_docente').val() == "T") {
            $.get("/Filtros/Docentes",
                { IdFacultad: $('#filtro_2_docente').val() },
                function (data) {
                    var docentes = JSON.parse(data);
                    result = []
                    if (docentes.length) {
                        for (var i = 0; i < docentes.length; i++) {
                            if ($('input#' + docentes[i]['idPersona']).length == 0) {
                                var check = '<input type="checkbox" name="persons" id="' + docentes[i]['idPersona'] + '" checked />';
                                result.push([check, docentes[i]['nombres'] + ' ' + docentes[i]['apellidos']]);
                            }
                        }
                        personsTable.rows.add(result).draw();
                    }
                });
        }
        else {
            $.get("/Filtros/DocentesFacultad",
                { IdFacultad: $('#filtro_2_docente').val() },
                function (data) {
                    var docentes = JSON.parse(data);                    
                    result = []
                    if (docentes.length) {
                        for (var i = 0; i < docentes.length; i++) {
                            if ($('input#' + docentes[i]['idPersona']).length == 0) {
                                var check = '<input type="checkbox" name="persons" id="' + docentes[i]['idPersona'] + '" checked />';
                                result.push([check, docentes[i]['nombres'] + ' ' + docentes[i]['apellidos']]);
                            }
                        }
                        personsTable.rows.add(result).draw();
                    }
                });
        }
        
    }

}

function cargar_listas(idPersona) {
    $.get("/Lista/Listas",
        { idPersona: idPersona },
        function (data) {
            var data = JSON.parse(data);
            if (data.length) {
                $('#filtro_personalizada').empty();
                for (var i = 0; i < data.length; i++) {
                    $('#filtro_personalizada').append($('<option value="' + data[i]['id'] + '">' + data[i]['nombre'] + '</option>'));
                }
            }
        });
}

function cargar_personas() {
    $.get("/Lista/PersonasLista",
        { idLista: $('#filtro_personalizada').val() },
        function (data) {
            console.log(data)
            var personas = JSON.parse(data);
            result = []
            if (personas.length) {
                for (var i = 0; i < personas.length; i++) {
                    if ($('input#' + personas[i]['idPersona']).length == 0) {
                        var check = '<input type="checkbox" name="persons" id="' + personas[i]['idPersona'] + '" checked />';
                        result.push([check, personas[i]['nombre']]);
                    }
                    
                }
                personsTable.rows.add(result).draw();
            }
        });

}

function buscar() {
    $.get("/Filtros/Buscar",
        { nombrePersona: $('#busqueda').val() },
        function (data) {
            var personas = JSON.parse(data);
            if (personas.length) {
                $('#resultados_busqueda').empty();
                $('#resultados_busqueda').append($('<option selected>Seleccionar persona...</option>'));
                for (var i = 0; i < personas.length; i++) {
                    $('#resultados_busqueda').append($('<option value="' + personas[i]['intIdPersona'] + '">' + personas[i]['strNombres'] + " " + personas[i]['strApellidos'] + '</option>'));
                }
            }
            //if (personas.length == 1) {
            //    let check = '<input type="checkbox" name="persons" id="' + personas[0]['intIdPersona'] + '" checked />';
            //    personsTable.row.add([check, personas[0]['strNombres'] + " " + personas[0]['strApellidos']]).draw()
            //}
        });

}

function refreshPersonasOcupadasTable(fila, columna) {
    let personas = data_dicc[fila][columna]['nombresPersonas']
    spinner = '<tr id="spinnerProv" class="odd"><td valign="top" colspan="11" class="dataTables_empty"><div style="text-align: center;"><i disabled class="btn icofont-spinner fa-spin" id="loading" style="font-size: 2em"></div></td></tr>'
    $('#personasOcupadasTable tbody').prepend(spinner);
    $('#spinnerProv').siblings().remove();
    result = [];
    for (var i = 0; i < personas.length; i++) {
        result.push([personas[i]]);
    }
    personasOcupadasTable.clear().draw();
    personasOcupadasTable.rows.add(result).draw();
    $("#personasOcupadasTable").attr("hidden", false)
    $('#modalPersonasOcupadas').modal({ show: true });

}

function limpiar_tabla() {
    personsTable.clear().draw();
    $("#timeTable tbody").empty();
    $("#timeTable").attr("hidden", true)
    $("#codigo_colores").attr("hidden", true)
    $("#agendar_reunion").attr("style", 'float: right; font-size: 0.9em; margin-top: 15px; display: none;')
}

function cargar_zonas() {
    $.get("/Filtros/Zonas",
        function (data) {
            console.log(data)
            var zonas = JSON.parse(data);
            if (zonas.length) {
                $('#zona').empty();
                $('#zona').append($('<option selected>Seleccionar zona...</option>'));
                for (var i = 0; i < zonas.length; i++) {
                    $('#zona').append($('<option value="' + zonas[i] + '">Zona ' + zonas[i] + '</option>'));
                }
            }
        });
}

function cargar_bloques_zona(zona) {
    $.get("/Filtros/BloquesZona",
        { zona: zona },        
        function (data) {
            console.log(data)
            var bloques = JSON.parse(data);
            if (bloques.length) {
                $('#bloque').empty();
                $('#bloque').append($('<option selected>Seleccionar bloque...</option>'));
                for (var i = 0; i < bloques.length; i++) {
                    $('#bloque').append($('<option value="' + bloques[i]['idLugarBaseEspol'] + '">Bloque ' + bloques[i]['descripcion'] + '</option>'));
                }
            }
        });
}

function cargar_lugares_bloque(idBloque) {

    let fecha = $('#fecha_reunion').val();
    let hora_inicio = $('#hora_inicio').val();
    let hora_fin = $('#hora_fin').val();
    
    if (!fecha || !hora_inicio || !hora_fin) {
        alert('Por favor escoja fecha, hora de inicio y hora de fin para cargar los lugares disponibles')
        return -1;
    }

    let fecha_inicio = new Date($('#fecha_reunion').val());
    let fecha_fin = new Date($('#fecha_reunion').val());
    fecha_inicio.setDate(fecha_inicio.getDate() + 1);
    fecha_inicio.setHours(hora_inicio.split(':')[0], hora_inicio.split(':')[1])
    fecha_inicio = new Date(fecha_inicio.getTime() - (fecha_inicio.getTimezoneOffset() * 60000)).toJSON()

    fecha_fin.setDate(fecha_fin.getDate() + 1);
    fecha_fin.setHours(hora_fin.split(':')[0], hora_fin.split(':')[1])
    fecha_fin = new Date(fecha_fin.getTime() - (fecha_fin.getTimezoneOffset() * 60000)).toJSON()


    $.get("/Filtros/LugaresBloque",
        {
            idBloque: idBloque,
            fechaInicio: fecha_inicio,
            fechaFin: fecha_fin,

        },
        function (data) {
            console.log(data)
            var lugares = JSON.parse(data);
            if (lugares.length) {
                $('#lugar').empty();
                $('#lugar').append($('<option value="">Seleccionar lugar...</option>'));
                for (var i = 0; i < lugares.length; i++) {
                    $('#lugar').append($('<option value="' + lugares[i]['idLugar'] + '">' + lugares[i]['nombreLugar'] + '</option>'));
                }
            }
        });
}

function validar_form() {
    let fecha = $('#fecha_reunion').val();
    let hora_inicio = $('#hora_inicio').val();
    let hora_fin = $('#hora_fin').val();
    let descripcion = $('#descripcion').val();
    let lugar = $('#lugar').val();
    console.log(lugar)

    if (!fecha || !lugar || !hora_inicio || !hora_fin || !descripcion) {
        return false;
    }

    return true;

}