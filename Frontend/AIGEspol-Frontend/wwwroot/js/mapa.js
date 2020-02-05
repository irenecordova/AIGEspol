var baseLayer, cfg, heatmapLayer, map

$(document).ready(function () {
    $('.nav-link').attr('class', 'nav-link')
    $('#mapa-tab').attr('class', 'nav-link active')
    $('.tab-pane').attr('class', 'tab-pane fade')
    $('#pills-mapa').attr('class', 'tab-pane fade show active')

    n = new Date();
    y = n.getFullYear();
    m = n.getMonth() + 1;
    d = n.getDate();

    h = n.getHours()
    mi = n.getMinutes()

    console.log(h)
    console.log(mi)

    if (mi < 30 && mi > 0) {
        mi = 30;
    }
    else if (mi > 30) {
        mi = "00";
        h += 1;
    }

    if (h < 10)
        h = '0' + h

    if (d < 10)
        d = '0' + d;
    if (m < 10)
        m = '0' + m
    $('input[type=date]').val(y + "-" + m + "-" + d);

    //$('select#dia').val(d-1);
    $('input#hora').val(String(h) + ":" + String(mi));
    $('#dia_text').text($('input[type=date]').val());
    $('#hora_text').text(String(h) + ":" + String(mi));

    getData();

    baseLayer = L.tileLayer(
        'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://cloudmade.com">CloudMade</a>',
        maxZoom: 18
    }
    );

    cfg = {
        // radius should be small ONLY if scaleRadius is true (or small radius is intended)
        "radius": 0.00015,
        "maxOpacity": .8,
        "blur": .75,
        // scales the radius based on map zoom
        "scaleRadius": true,
        // if set to false the heatmap uses the global maximum for colorization
        // if activated: uses the data maximum within the current map boundaries
        //   (there will always be a red spot with useLocalExtremas true)
        "useLocalExtrema": true,
        // which field name in your data represents the latitude - default "lat"
        latField: 'lat',
        // which field name in your data represents the longitude - default "lng"
        lngField: 'lng',
        // which field name in your data represents the data value - default "value"
        valueField: 'count',
        //"gradient": {
        //    // enter n keys between 0 and 1 here
        //    // for gradient color customization
        //    '.01': 'blue',
        //    '.8': 'blue',
        //    '.95': 'blue'
        //}
    };

    heatmapLayer = new HeatmapOverlay(cfg);

    map = new L.Map('map-canvas', {
        center: new L.LatLng(-2.1462113, -79.9656774),
        zoom: 17,
        layers: [baseLayer, heatmapLayer]
    });

});

function actualizar() {
    $('#dia_text').text($('input[type=date]').val());
    $('#hora_text').text(String(h) + ":" + String(mi));
    getData();
}

function getData() {
    var fecha = new Date($('#date').val());
    var hora = $('#hora').val()

    fecha.setDate(fecha.getDate() + 1);
    fecha.setHours(hora.split(':')[0], hora.split(':')[1])
    fecha = new Date(fecha.getTime() - (fecha.getTimezoneOffset() * 60000)).toJSON()
   
    $.get("/Mapa/Estadisticas",
        { fecha: fecha },
        function (data) {
            var json = JSON.parse(data)
            $('#cant_estudiantes').text(json['totalPersonasMomento'] + "/" + json['numRegistrados']);
            $('#cant_boques').text(json['cantBloquesUsados'] + "/" + json['cantBloquesTotales']);
            $('#cant_lugares').text(json['cantLugaresUsados'] + "/" + json['cantLugares']);
            $('#prom_boques').text(json['promPersonasPorLugar'].toFixed(2));
            $('#prom_lugares').text(json['promPersonasPorBloque'].toFixed(2));
            if (json['top3Bloques'].length != 0) {
                $('#top_bloques_1').text(json['top3Bloques'][0]['nombre'] + " - " + json['top3Bloques'][0]['numPersonas'] + " personas");
                $('#top_bloques_2').text(json['top3Bloques'][1]['nombre'] + " - " + json['top3Bloques'][1]['numPersonas'] + " personas");
                $('#top_bloques_3').text(json['top3Bloques'][2]['nombre'] + " - " + json['top3Bloques'][2]['numPersonas'] + " personas");
            }

            $.get("/Mapa/Generar",
                { fecha: fecha },
                function (data) {
                    var data = JSON.parse(data)
                    cargar_mapa(data, json['numRegistrados']);
                });
            
        });
}

function cargar_mapa(dicc, personas) {
    console.log(dicc)
    console.log(personas)
    var testData = {
        //max: personas,
        //max: 300,
        //min: 0,
        data: dicc
    };
  
    //var testData = {
    //    max: 100,
    //    data: [{ lat: -2.1462113, lng: -79.9656774, count: 25 },
    //        { lat: -2.1446227, lng: -79.9669499, count: 10 },
    //        { lat: -2.1449607, lng: 79.9668688, count: 10 },
    //        { lat: -2.1457835, lng: -79.966984, count: 15 },
    //        { lat: -2.145354, lng: -79.9660794, count: 10},
    //        { lat: -2.1487748134517846, lng: -79.96748121743613, count: 10 },
    //        { lat: -2.1487586530115554, lng: -79.96755366529352 , count: 10 },
    //        { lat: -2.1472254781195748, lng: -79.96690148984044, count: 5 },
    //        { lat: -2.1472292492026703, lng: -79.96792236237579, count: 12 },
    //        { lat: -2.147258012885626, lng: -79.96782861760725, count: 15 },
    //        { lat: -2.146965220511311, lng: -79.96670928382672, count: 10 },
    //        { lat: -2.146624609602815, lng: -79.96671491582661, count: 22 },
    //        { lat: -2.1470145571484243, lng: -79.96683711517376, count: 15 },
    //        { lat: -2.1468719826737455, lng: -79.96668029080266, count: 13 },
    //        { lat: -2.146118001696026, lng: -79.96567975250287, count: 19 },
    //        { lat: -2.1459732109943186, lng: -79.9656042982829, count: 15 },
    //        { lat: -2.1460698704232812, lng: -79.9656546713161, count: 25},
    //        { lat: -2.1465902, lng: -79.9673152, count: 12 },
    //        { lat: -2.1445699, lng: -79.9676742, count: 13 },]
    //};

    heatmapLayer.setData(testData);

}