function downloadResume(uid = 0) {
	let userId;
	if (uid !== 0) {
		userId = uid.toString();
	} else {
		userId = new URLSearchParams(location.search).get("uid");
		userId = userId ?? "1";
	}
	$.ajax({
		url: "/Files/Resume.ashx?userId=" + userId,
		type: "GET",
		xhrFields: {
			responseType: "blob",
		},
		success: function (data, statusText, xhr) {
			if (xhr.status === 200) {
				window.location.href = "/Files/Resume.ashx?userId=" + userId;
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
