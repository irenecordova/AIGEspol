
$(document).ready(function () {
    cargar_mapa()
    $('.nav-link').attr('class', 'nav-link')
    $('#mapa-tab').attr('class', 'nav-link active')
    $('.tab-pane').attr('class', 'tab-pane fade')
    $('#pills-mapa').attr('class', 'tab-pane fade show active')

    n = new Date();
    d = n.getDay();

    h = n.getHours()
    mi = n.getMinutes()

    if (d == '0') {}

    $('select#dia').val(d-1);
    $('input#hora').val(String(h) + ":" + String(mi));
    $('#dia_text').text($('select#dia option:selected').text() );
    $('#hora_text').text(String(h) + ":" + String(mi));
});

function cargar_mapa() {

    var heatMapData = [
        { location: new google.maps.LatLng(-2.1462113, -79.9656774), weight: 0.5 },
        new google.maps.LatLng(37.782, -122.445),
        { location: new google.maps.LatLng(37.782, -122.443), weight: 2 },
        { location: new google.maps.LatLng(37.782, -122.441), weight: 3 },
        { location: new google.maps.LatLng(37.782, -122.439), weight: 2 },
        new google.maps.LatLng(37.782, -122.437),
        { location: new google.maps.LatLng(37.782, -122.435), weight: 0.5 },

        { location: new google.maps.LatLng(37.785, -122.447), weight: 3 },
        { location: new google.maps.LatLng(37.785, -122.445), weight: 2 },
        new google.maps.LatLng(37.785, -122.443),
        { location: new google.maps.LatLng(37.785, -122.441), weight: 0.5 },
        new google.maps.LatLng(37.785, -122.439),
        { location: new google.maps.LatLng(37.785, -122.437), weight: 2 },
        { location: new google.maps.LatLng(37.785, -122.435), weight: 3 }
    ];

    var sanFrancisco = new google.maps.LatLng(37.774546, -122.433523);

    map = new google.maps.Map(document.getElementById('map-canvas'), {
        center: sanFrancisco,
        zoom: 13,
        mapTypeId: 'satellite'
    });

    var heatmap = new google.maps.visualization.HeatmapLayer({
        data: heatMapData
    });
    heatmap.setMap(map);

    //var testData = {
    //    max: 8,
    //    data: [{ lat: -2.1462113, lng: -79.9656774, count: 25 },
    //    { lat: -2.1446227, lng: -79.9669499, count: 10 },
    //    { lat: -2.1449607, lng: 79.9668688, count: 10 },
    //    { lat: -2.1457835, lng: -79.966984, count: 15 },
    //    { lat: -2.145354, lng: -79.9660794, count: 10 },
    //    { lat: -2.1487748134517846, lng: -79.96748121743613, count: 10 },
    //    { lat: -2.1487586530115554, lng: -79.96755366529352, count: 10 },
    //    { lat: -2.1472254781195748, lng: -79.96690148984044, count: 5 },
    //    { lat: -2.1472292492026703, lng: -79.96792236237579, count: 12 },
    //    { lat: -2.147258012885626, lng: -79.96782861760725, count: 15 },
    //    { lat: -2.146965220511311, lng: -79.96670928382672, count: 10 },
    //    { lat: -2.146624609602815, lng: -79.96671491582661, count: 22 },
    //    { lat: -2.1470145571484243, lng: -79.96683711517376, count: 15 },
    //    { lat: -2.1468719826737455, lng: -79.96668029080266, count: 13 },
    //    { lat: -2.146118001696026, lng: -79.96567975250287, count: 19 },
    //    { lat: -2.1459732109943186, lng: -79.9656042982829, count: 15 },
    //    { lat: -2.1460698704232812, lng: -79.9656546713161, count: 25 },
    //    { lat: -2.1465902, lng: -79.9673152, count: 12 },
    //    { lat: -2.1445699, lng: -79.9676742, count: 13 },]
    //};

    //var baseLayer = L.tileLayer(
    //    'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    //    attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://cloudmade.com">CloudMade</a>',
    //    maxZoom: 18
    //}
    //);

    //var cfg = {
    //    // radius should be small ONLY if scaleRadius is true (or small radius is intended)
    //    "radius": 0.00015,
    //    "maxOpacity": .8,
    //    // scales the radius based on map zoom
    //    "scaleRadius": true,
    //    // if set to false the heatmap uses the global maximum for colorization
    //    // if activated: uses the data maximum within the current map boundaries
    //    //   (there will always be a red spot with useLocalExtremas true)
    //    "useLocalExtrema": true,
    //    // which field name in your data represents the latitude - default "lat"
    //    latField: 'lat',
    //    // which field name in your data represents the longitude - default "lng"
    //    lngField: 'lng',
    //    // which field name in your data represents the data value - default "value"
    //    valueField: 'count'
    //};

    //var heatmapLayer = new HeatmapOverlay(cfg);

    //var map = new L.Map('map-canvas', {
    //    center: new L.LatLng(-2.1462113, -79.9656774),
    //    zoom: 17,
    //    layers: [baseLayer, heatmapLayer]
    //});

    //heatmapLayer.setData(testData);

}

//function cargar_mapa() {

//    var testData = {
//        max: 8,
//        data: [{ lat: -2.1462113, lng: -79.9656774, count: 25 },
//            { lat: -2.1446227, lng: -79.9669499, count: 10 },
//            { lat: -2.1449607, lng: 79.9668688, count: 10 },
//            { lat: -2.1457835, lng: -79.966984, count: 15 },
//            { lat: -2.145354, lng: -79.9660794, count: 10},
//            { lat: -2.1487748134517846, lng: -79.96748121743613, count: 10 },
//            { lat: -2.1487586530115554, lng: -79.96755366529352 , count: 10 },
//            { lat: -2.1472254781195748, lng: -79.96690148984044, count: 5 },
//            { lat: -2.1472292492026703, lng: -79.96792236237579, count: 12 },
//            { lat: -2.147258012885626, lng: -79.96782861760725, count: 15 },
//            { lat: -2.146965220511311, lng: -79.96670928382672, count: 10 },
//            { lat: -2.146624609602815, lng: -79.96671491582661, count: 22 },
//            { lat: -2.1470145571484243, lng: -79.96683711517376, count: 15 },
//            { lat: -2.1468719826737455, lng: -79.96668029080266, count: 13 },
//            { lat: -2.146118001696026, lng: -79.96567975250287, count: 19 },
//            { lat: -2.1459732109943186, lng: -79.9656042982829, count: 15 },
//            { lat: -2.1460698704232812, lng: -79.9656546713161, count: 25},
//            { lat: -2.1465902, lng: -79.9673152, count: 12 },
//            { lat: -2.1445699, lng: -79.9676742, count: 13 },]
//    };

//    var baseLayer = L.tileLayer(
//        'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
//        attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://cloudmade.com">CloudMade</a>',
//        maxZoom: 18
//    }
//    );

//    var cfg = {
//        // radius should be small ONLY if scaleRadius is true (or small radius is intended)
//        "radius": 0.00015,
//        "maxOpacity": .8,
//        // scales the radius based on map zoom
//        "scaleRadius": true,
//        // if set to false the heatmap uses the global maximum for colorization
//        // if activated: uses the data maximum within the current map boundaries
//        //   (there will always be a red spot with useLocalExtremas true)
//        "useLocalExtrema": true,
//        // which field name in your data represents the latitude - default "lat"
//        latField: 'lat',
//        // which field name in your data represents the longitude - default "lng"
//        lngField: 'lng',
//        // which field name in your data represents the data value - default "value"
//        valueField: 'count'
//    };

//    var heatmapLayer = new HeatmapOverlay(cfg);

//    //if (document.getElementById('map-canvas') == null) {
//    //    alert("hola")
//    //    return;
//    //}

//    var map = new L.Map('map-canvas', {
//        center: new L.LatLng(-2.1462113, -79.9656774),
//        zoom: 17,
//        layers: [baseLayer, heatmapLayer]
//    });

//    heatmapLayer.setData(testData);

//}