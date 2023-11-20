$(document).ready(function () {
    $(".loading-btn").on('click', function () {
        $(this).addClass("btn-loading")
    });

});

function pageLoad() {
    removeLoading();
}



window.onload = function () {
    removeLoading();
}



function addLoading(btn) {
    btn.classList.add('btn-loading');
}
function removeLoading() {
    $(".loading-btn").removeClass("btn-loading")
}