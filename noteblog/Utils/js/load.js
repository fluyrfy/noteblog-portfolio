window.addEventListener('load', function () {
    $(".loading-btn").removeClass("btn-loading");
    $(".loading-btn").on('click', function () {
        $(this).addClass("btn-loading")
    });
});