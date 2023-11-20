function downloadResume() {
    window.location.href = "/Files/Resume.ashx"
    $.ajax({
        url: "/Files/Resume.ashx",
        type: "GET",
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data, statusText, xhr) {
            if (xhr.status === 200) {
                removeLoading();
            } else {
                console.error('Error: download fail');
                removeLoading();
            }
        },
        error: function (error) {
            console.error('Error:', error);
            removeLoading();
        }
    });
}