function getCategoryValue(categoryId) {
	try {
		const request = $.ajax({
			url: `/api/categories/get/${categoryId}`,
			method: "GET",
			dataType: "json",
		})
			.done(function (category) {
				$("#editCategoryName").val(category.name);
				$("#editCategoryDescription").val(category.description);
				$("#editCategoryIconClass").val(category.iconClass);
			})
			.fail(function (xhr, status, error) {
				console.error(status, error);
			});
	} catch (error) {
		console.error("error:", error);
	}
}

function getUserValue(userId) {
	try {
		const request = $.ajax({
			url: `/api/users/get/${userId}`,
			method: "GET",
			dataType: "json",
		})
			.done(function (user) {
				$("#editUserName").val(user.name);
				$("#editUserRole").val(user.role);
			})
			.fail(function (xhr, status, error) {
				console.error(status, error);
			});
	} catch (error) {
		console.error("error:", error);
	}
}

function setLogContent(content) {
	$("#logContent").text(decodeURIComponent(content));
}

function setId(type, id) {
	document.getElementById(`hid${type}Id`).value = id;
	switch (type) {
		case "User":
			getUserValue(id);
			break;
	}
}

function setNoteIds(noteIds = 0) {
	document.getElementById("noteId").value = noteIds == 0 ? 0 : noteIds;
}

function convertToBase64() {
	if ($("#MainContent_txtSearch").val() == "") {
		return false;
	}
	var userInput = $("#MainContent_txtSearch").val();
	var encodedInput = btoa(encodeURIComponent(userInput)); // 將用戶輸入的內容進行 Base64 編碼
	$("#MainContent_hdnSearch").val(encodedInput);
	$("#MainContent_txtSearch").val("");
	return true;
}

$(document).ready(function () {
	$(".xp-menubar").on("click", function () {
		$("#sidebar").toggleClass("active");
		$("#content").toggleClass("active");
	});

	$(".xp-menubar,.body-overlay").on("click", function () {
		$("#sidebar,.body-overlay").toggleClass("show-nav");
	});

	$(".cb-note").on("click", function () {
		var selectedCheckBoxes = $(".cb-note:checked");
		var multiDelete = $(".multi-delete");
		if (selectedCheckBoxes.length > 0) {
			multiDelete.removeClass("disabled");
		} else {
			multiDelete.addClass("disabled");
		}
	});

	$("#lbtnStatsSearch").toggleClass(
		"disabled",
		$("#txtStatsStart").val() == "" && $("#txtStatsEnd").val() == ""
	);
});

$(".sidebar-item").on("click", function () {
	$("#hidActiveSidebarItem").val($(this).data("sidebar-item"));
});

$(".sidebar-item").each(function (index) {
	$(this).removeClass("active");
	if ($(this).data("sidebar-item") === $("#hidActiveSidebarItem").val()) {
		$(this).addClass("active");
	}
});
