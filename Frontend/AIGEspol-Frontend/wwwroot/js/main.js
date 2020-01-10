$(document).ready(function () {
    $(document).on('click', '.myTab a', function () {
        alert('hola')
        id = $(this).attr("id").split("-")[0];
        $('.tab-pane').attr('class', 'tab-pane fade')
        $('#pills-' + id).attr('class', 'tab-pane fade show active')
    })
});
