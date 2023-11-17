$(document).ready(function () {
    $("a, button, input[type = 'submit']").on('click', function () {
        $(".loading").show();
    });
});

window.onload = function () {
    $(".loading").hide();
}
