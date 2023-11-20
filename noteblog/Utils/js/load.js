$(document).ready(function () {
    $(".loading-btn").on('click', function () {
        $(this).addClass("btn-loading")
    });
});

function addLoading(btn) {
    btn.classList.add('btn-loading');
}

window.onload = function () {
    $(".loading-btn").removeClass("btn-loading")
}
