
$(function() {
    getLastUpdateTime();

    var timer = setInterval(() => {
        getLastUpdateTime();
    }, 60000);

    window.addEventListener('beforeunload', function () {
        clearInterval(timer);
    });
})

function getLastUpdateTime() {
    $.ajax({
        url: '/api/notes/getLatestNoteTime',
        method: 'GET',
        dataType: 'json',
    }).done(updatedAt => $("#hidLastUpdateTime").val(updatedAt)).fail(error => {
        console.error('Error:', error);
    });;
}