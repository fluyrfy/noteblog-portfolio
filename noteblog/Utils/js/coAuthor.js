let selectedCoAuthorUserIds = [];
$(function () {
	$("#input-co-author").on("input", function (e) {
		let keyword = $(this)
			.clone() //clone the element
			.children() //select all the children
			.remove() //remove all the children
			.end() //again go back to selected element
			.text();
		keyword = keyword.trim();
		if (keyword.length > 0) {
			$.ajax({
				url: `/api/users/getByKeyword/${keyword}?selectedUsers=${selectedCoAuthorUserIds.join(
					","
				)}`,
				method: "GET",
			}).done(function (res) {
				if (res !== null && res !== undefined && res.length > 0) {
					var containerElement = $(
						'<div style="display: flex; gap: 1px;"></div>'
					);
					for (var i = 0; i < res.length; i++) {
						let author = res[i];
						let avatarSrc = author.avatar
							? `data:image/png;base64,${author.avatar}`
							: "Images/ico/user.png";
						let figureElement = $(
							'<figure contenteditable="false" class="fir-image-figure fig-co-author" id="' +
								author.id +
								'">' +
								'<img class="fir-author-image fir-clickcircle" src="' +
								avatarSrc +
								'" alt="' +
								author.name +
								' - Author">' +
								"<figcaption>" +
								'<div class="fig-author-figure-title">' +
								author.name +
								"</div>" +
								"</figcaption>" +
								"</figure>"
						);
						figureElement.on("click", function () {
							$(this).append('<i class="fa fa-times" aria-hidden="true"></i>');
							$("#coAuthorContainer").empty();
							$("#input-co-author")
								.contents()
								.each(function () {
									if (this.nodeType === Node.TEXT_NODE) this.remove();
								});
							$("#input-co-author").append($(this));
							$("#input-co-author").append("&nbsp;");
							selectedCoAuthorUserIds.push(author.id);
							$(this)
								.find(".fa-times")
								.on("click", function () {
									figureElement.remove();
									let userId = parseInt($(this).parent().attr("id"));
									selectedCoAuthorUserIds = selectedCoAuthorUserIds.filter(
										(existingUserId) => existingUserId !== userId
									);
								});
						});
						containerElement.append(figureElement);
					}
					$("#coAuthorContainer").html(containerElement);
				} else {
					$("#coAuthorContainer").empty();
				}
			});
		} else {
			$("#coAuthorContainer").empty();
		}
		if ($(this).children().length) {
			selectedCoAuthorUserIds = [];
		}
	});
	$(".submit-btn").on("click", function () {
		console.log(selectedCoAuthorUserIds);
		$("#hdnSelectedCoAuthorUserIds").val(selectedCoAuthorUserIds.join(","));
	});
});
