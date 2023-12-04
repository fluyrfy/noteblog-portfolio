$(function () {
	const path = location.pathname.split("/")[1];
	const pageName = path.length > 0 ? path : "Default";
	const noteId = new URLSearchParams(location.search).get("id");
	$.ajax({
		type: "POST",
		url: "/api/stats/save",
		data: JSON.stringify({ pageName }),
		contentType: "application/json",
	}).fail(function (error) {
		console.error(error);
	});
	if (pageName === "Note") {
		$.ajax({
			type: "POST",
			url: "/api/stats/saveNoteCTR",
			data: JSON.stringify({ noteId }),
			contentType: "application/json",
		}).fail(function (error) {
			console.error(error);
		});
	}
});
