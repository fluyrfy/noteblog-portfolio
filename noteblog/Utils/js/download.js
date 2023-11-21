function downloadResume() {
    $.ajax({
        url: "/Files/Resume.ashx",
        type: "GET",
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data, statusText, xhr) {
            if (xhr.status === 200) {
                window.location.href = "/Files/Resume.ashx"
            } else {
                console.error('Error: download fail');
            }
        },
        error: function (error) {
            console.error('Error:', error);
        },
        complete: function (data) {
            removeLoading();
        }
    });
};