﻿var idUsuario;

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

    reunionesTable = initializeSimpleTable('#reunionesTable', 10, columnList, 0, 'desc');
    $("#reunionesTable").attr("hidden", false)

    getId();

});

function actualizar() {
    if ($('#filtro').val() == 'R') {
        cargarReuniones(idUsuario);
    }
    else {
        cargarInvitaciones(idUsuario);
    }
}

function getId() {
    $.get("/Filtros/GetId",
        function (data) {
            console.log(data)
            idUsuario = data
            cargarReuniones(data);
        });
}

function cargarReuniones(idPersona) {
    $.get('/Reunion/Reuniones',
        { idPersona: idPersona },
        function (data) {
            var reuniones = JSON.parse(data);
            console.log(reuniones)
            reunionesTable.clear().draw();
            result = []
            if (reuniones.length) {
                for (var i = 0; i < reuniones.length; i++) {

                    let  fechaInicio = new Date(reuniones[i]['fechaInicio'])
                    let fechaFin = new Date(reuniones[i]['fechaFin'])
                    let dia = fechaInicio.getDate()
                    let mes = parseInt(fechaInicio.getMonth()) + 1
                    let anio = fechaInicio.getFullYear()

                    acciones = "<a class='btn btn-primary btm-sm m-2' name='cancelar' " + (reuniones[i]['cancelada'] == "F" ? "onclick='cancelar(" + reuniones[i]['id'] + ", this)' title='Cancelar reunión'" : "title='No se puede cancelar' style='color: lightgray'") +">CANCELAR</a>";
                    result.push([dia + "/" + mes + "/" + anio, reuniones[i]['asunto'], reuniones[i]['descripcion'], reuniones[i]['nombreLugar'], fechaInicio.getHours() + ":" + fechaInicio.getMinutes() + " - " + fechaFin.getHours() + ":" + fechaFin.getMinutes(), acciones]);
                }
                reunionesTable.rows.add(result).draw();
            }
        });
}

function cargarInvitaciones(idPersona) {
    $.get('/Reunion/Invitaciones',
        { idPersona: idPersona },
        function (data) {
            var invitaciones = JSON.parse(data);
            console.log(invitaciones)
            reunionesTable.clear().draw();
            result = []
            if (invitaciones.length) {
                for (var i = 0; i < invitaciones.length; i++) {
                    
                    let fechaInicio = new Date(invitaciones[i]['reunion']['fechaInicio'])
                    let fechaFin = new Date(invitaciones[i]['reunion']['fechaFin'])
                    let dia = fechaInicio.getDate()
                    let mes = parseInt(fechaInicio.getMonth()) + 1
                    let anio = fechaInicio.getFullYear()

                    let fechaActual = new Date();

                    let aceptable = false
                    let rechazable = true

                    if (invitaciones[i]['estado'] != 'A') {
                        aceptable = true
                    }

                    if (invitaciones[i]['estado'] != 'R' && fechaInicio > fechaActual) {
                        rechazable = true
                    }

                    acciones = "<a class='btn btn-primary btm-sm m-2' name='aceptar' " + (aceptable ? "onclick='aceptar(" + invitaciones[i]['idInvitacion'] + ", this)' title='Aceptar invitación'" : "title='No se puede aceptar' style='color: lightgray'") + ">ACEPTAR</a>\
                                <a class='btn btn-primary btm-sm m-2' name='rechazar' " + (rechazable ? "onclick='rechazar(" + invitaciones[i]['idInvitacion'] + ", this)' title='Rechazar invitación'" : "title='No se puede rechazar' style='color: lightgray'") + ">CANCELAR</a>"
                    result.push([dia + "/" + mes + "/" + anio, invitaciones[i]['reunion']['asunto'], invitaciones[i]['reunion']['descripcion'], invitaciones[i]['nombreLugar'], fechaInicio.getHours() + ":" + fechaInicio.getMinutes() + " - " + fechaFin.getHours() + ":" + fechaFin.getMinutes(), acciones]);
                }
                reunionesTable.rows.add(result).draw();
            }
        });
}

function cancelar(idReunion, element) {
    $.get('/Reunion/Cancelar',
        { idReunion: idReunion },
        function (data) {
            console.log(data)
            var data = JSON.parse(data);
            if (data['error'] == 0) {
                $(element).removeAttr('onclick')
                $(element).attr('style', 'color: lightgray;')
                $(element).attr('title', 'No se puede cancelar')
                alert(data['mensaje']);
            }
            else {
                alert("Ocurrió un error: " + data['mensaje']);
            }
            
        });
}

function aceptar(idInvitacion, element) {
    $.get('/Reunion/Aceptar',
        { IdInvitacion: idInvitacion },
        function (data) {
            console.log(data)
            var data = JSON.parse(data);
            if (data['error'] == 0) {
                $(element).removeAttr('onclick')
                $(element).attr('style', 'color: lightgray;')
                $(element).attr('title', 'No se puede rechazar')
                alert(data['mensaje']);
            }
            else {
                alert("Ocurrió un error: " + data['mensaje']);
            }

        });
}

function rechazar(idInvitacion, element) {
    $.get('/Reunion/Rechazar',
        { idInvitacion: idInvitacion },
        function (data) {
            console.log(data)
            var data = JSON.parse(data);
            if (data['error'] == 0) {
                $(element).removeAttr('onclick')
                $(element).attr('style', 'color: lightgray;')
                $(element).attr('title', 'No se puede rechazar')
                alert(data['mensaje']);
            }
            else {
                alert("Ocurrió un error: " + data['mensaje']);
            }

        });
}
