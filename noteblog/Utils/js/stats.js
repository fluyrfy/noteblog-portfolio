$(function () {
    const path = location.pathname.split("/")[1];
    const pageName = path.length > 0 ? path : "Default";
    $.ajax({
        type: "POST",
        url: "/api/stats/save",
        data: JSON.stringify({ pageName }),
        contentType: "application/json",
    }).fail(function (error) {
        console.error(error);
    });
});
