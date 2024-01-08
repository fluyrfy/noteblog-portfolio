function downloadResume(uid = "") {
	if (uid.length === 0) {
		uid = new URLSearchParams(location.search).get("uid") ?? uid;
	}
	$.ajax({
		url: "/Files/Resume.ashx?userId=" + uid,
		type: "GET",
		xhrFields: {
			responseType: "blob",
		},
		success: function (data, statusText, xhr) {
			if (xhr.status === 200) {
				window.location.href = "/Files/Resume.ashx?userId=" + uid;
			} else {
				console.error("Error: download fail");
			}
		},
		error: function (error) {
			console.error("Error:", error);
		},
		complete: function (data) {
			removeLoading();
		},
	});
}
