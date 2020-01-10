
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

    $('select#dia').val(d-1);
    $('input#hora').val(String(h) + ":" + String(mi));
    $('#dia_text').text($('select#dia option:selected').text() );
    $('#hora_text').text(String(h) + ":" + String(mi));
});

function cargar(id) {
    //$('#'+nombre+'-tab').click(function() {
    //	spinner = ''
    //	if (!$('#pills-'+nombre).children().length) {
    //		spinner = '<div class="col-12" style="text-align: center;"><tr id="spinner'+nombre+'" class="odd"><td height="140"  valign="middle" colspan="11" style="text-align: center;"><div class="btn icofont-spinner fa-spin" style="font-size: 2em"></div></td></tr></div>';
    //	}
    //	try {
    //		$('#pills-'+nombre).prepend(spinner);
    //		$('#pills-'+nombre).load(url);
    //	}
    //	catch(err) {
    //		$(spinner).remove();
    //		message('danger', err.message);
    //	}
    //});
    $('#pills-' + id).load(id);
}

function cargar_mapa() {

    var testData = {
        max: 8,
        data: [{ lat: -2.1462113, lng: -79.9656774, count: 16 },
            { lat: -2.1467718400398264, lng: -79.96335100903025, count: 10 },
            { lat: -2.146819196740002, lng: -79.96319801589986, count: 20 },
            { lat: -2.1482729055959218, lng: -79.96846949358775, count: 10 },
            { lat: -2.148198263064966, lng: -79.96871155237764, count: 20 },
            { lat: -2.148713340510601, lng: -79.96792160705635, count: 30},
            { lat: -2.1487748134517846, lng: -79.96748121743613, count: 10 },
            { lat: -2.1487586530115554, lng: -79.96755366529352 , count: 10 },
            { lat: -2.1472254781195748, lng: -79.96690148984044, count: 5 },
            { lat: -2.1472292492026703, lng: -79.96792236237579, count: 12 },
            { lat: -2.147258012885626, lng: -79.96782861760725, count: 15 },
            { lat: -2.146965220511311, lng: -79.96670928382672, count: 10 },
            { lat: -2.146624609602815, lng: -79.96671491582661, count: 22 },
            { lat: -2.1470145571484243, lng: -79.96683711517376, count: 15 },
            { lat: -2.1468719826737455, lng: -79.96668029080266, count: 13 },
            { lat: -2.146118001696026, lng: -79.96567975250287, count: 19 },
            { lat: -2.1459732109943186, lng: -79.9656042982829, count: 15 },
            { lat: -2.1460698704232812, lng: -79.9656546713161, count: 25},
            { lat: 39.4209, lng: -74.4977, count: 1 }, { lat: 39.7437, lng: -104.979, count: 1 },
            { lat: 39.5593, lng: -105.006, count: 1 }, { lat: 45.2673, lng: -93.0196, count: 1 },
            { lat: 41.1215, lng: -89.4635, count: 1 }, { lat: 43.4314, lng: -83.9784, count: 1 },
            { lat: 43.7279, lng: -86.284, count: 1 }, { lat: 40.7168, lng: -73.9861, count: 1 },
            { lat: 47.7294, lng: -116.757, count: 1 }, { lat: 47.7294, lng: -116.757, count: 2 },
            { lat: 35.5498, lng: -118.917, count: 1 }, { lat: 34.1568, lng: -118.523, count: 1 },
            { lat: 39.501, lng: -87.3919, count: 3 }, { lat: 33.5586, lng: -112.095, count: 1 },
            { lat: 38.757, lng: -77.1487, count: 1 }, { lat: 33.223, lng: -117.107, count: 1 },
            { lat: 30.2316, lng: -85.502, count: 1 }, { lat: 39.1703, lng: -75.5456, count: 8 },
            { lat: 30.0041, lng: -95.2984, count: 2 }, { lat: 29.7755, lng: -95.4152, count: 1 },
            { lat: 41.8014, lng: -87.6005, count: 1 }, { lat: 37.8754, lng: -121.687, count: 7 }, { lat: 38.4493, lng: -122.709, count: 1 }, { lat: 40.5494, lng: -89.6252, count: 1 }, { lat: 42.6105, lng: -71.2306, count: 1 }, { lat: 40.0973, lng: -85.671, count: 1 }, { lat: 40.3987, lng: -86.8642, count: 1 }, { lat: 40.4224, lng: -86.8031, count: 4 }, { lat: 47.2166, lng: -122.451, count: 1 }, { lat: 32.2369, lng: -110.956, count: 1 }, { lat: 41.3969, lng: -87.3274, count: 2 }, { lat: 41.7364, lng: -89.7043, count: 2 }, { lat: 42.3425, lng: -71.0677, count: 1 }, { lat: 33.8042, lng: -83.8893, count: 1 }, { lat: 36.6859, lng: -121.629, count: 2 }, { lat: 41.0957, lng: -80.5052, count: 1 }, { lat: 46.8841, lng: -123.995, count: 1 }, { lat: 40.2851, lng: -75.9523, count: 2 }, { lat: 42.4235, lng: -85.3992, count: 1 }, { lat: 39.7437, lng: -104.979, count: 2 }, { lat: 25.6586, lng: -80.3568, count: 7 }, { lat: 33.0975, lng: -80.1753, count: 1 }, { lat: 25.7615, lng: -80.2939, count: 1 }, { lat: 26.3739, lng: -80.1468, count: 1 }, { lat: 37.6454, lng: -84.8171, count: 1 }, { lat: 34.2321, lng: -77.8835, count: 1 }, { lat: 34.6774, lng: -82.928, count: 1 }, { lat: 39.9744, lng: -86.0779, count: 1 }, { lat: 35.6784, lng: -97.4944, count: 2 }, { lat: 33.5547, lng: -84.1872, count: 1 }, { lat: 27.2498, lng: -80.3797, count: 1 }, { lat: 41.4789, lng: -81.6473, count: 1 }, { lat: 41.813, lng: -87.7134, count: 1 }, { lat: 41.8917, lng: -87.9359, count: 1 }, { lat: 35.0911, lng: -89.651, count: 1 }, { lat: 32.6102, lng: -117.03, count: 1 }, { lat: 41.758, lng: -72.7444, count: 1 }, { lat: 39.8062, lng: -86.1407, count: 1 }, { lat: 41.872, lng: -88.1662, count: 1 }, { lat: 34.1404, lng: -81.3369, count: 1 }, { lat: 46.15, lng: -60.1667, count: 1 }, { lat: 36.0679, lng: -86.7194, count: 1 }, { lat: 43.45, lng: -80.5, count: 1 }, { lat: 44.3833, lng: -79.7, count: 1 }, { lat: 45.4167, lng: -75.7, count: 2 }, { lat: 43.75, lng: -79.2, count: 2 }, { lat: 45.2667, lng: -66.0667, count: 3 }, { lat: 42.9833, lng: -81.25, count: 2 }, { lat: 44.25, lng: -79.4667, count: 3 }, { lat: 45.2667, lng: -66.0667, count: 2 }, { lat: 34.3667, lng: -118.478, count: 3 }, { lat: 42.734, lng: -87.8211, count: 1 }, { lat: 39.9738, lng: -86.1765, count: 1 }, { lat: 33.7438, lng: -117.866, count: 1 }, { lat: 37.5741, lng: -122.321, count: 1 }, { lat: 42.2843, lng: -85.2293, count: 1 }, { lat: 34.6574, lng: -92.5295, count: 1 }, { lat: 41.4881, lng: -87.4424, count: 1 }, { lat: 25.72, lng: -80.2707, count: 1 }, { lat: 34.5873, lng: -118.245, count: 1 }, { lat: 35.8278, lng: -78.6421, count: 1 }]
    };

    var baseLayer = L.tileLayer(
        'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://cloudmade.com">CloudMade</a>',
        maxZoom: 18
    }
    );

    var cfg = {
        // radius should be small ONLY if scaleRadius is true (or small radius is intended)
        "radius": 0.00015,
        "maxOpacity": .8,
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
        valueField: 'count'
    };

    var heatmapLayer = new HeatmapOverlay(cfg);

    //if (document.getElementById('map-canvas') == null) {
    //    alert("hola")
    //    return;
    //}

    var map = new L.Map('map-canvas', {
        center: new L.LatLng(-2.1462113, -79.9656774),
        zoom: 17,
        layers: [baseLayer, heatmapLayer]
    });

    heatmapLayer.setData(testData);

}