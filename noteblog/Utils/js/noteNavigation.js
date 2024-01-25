$(function () {
	let noteContentTitles = $("#main-text h2");
	let noteNavigationObject = $(".noteNavigation ul");

	$(".toggle-navi").on("click", function () {
		$(this).toggleClass("active");
		var content = $(".noteNavigation");
		// var currentWidth = content.width();
		// content.animate(
		// 	{
		// 		width: currentWidth > 0 ? 0 : 200,
		// 		height: currentWidth > 0 ? "0" : "100%",
		// 		left: currentWidth > 0 ? "100%" : "0%",
		// 	},
		// 	500
		// );
		content.toggle(500, function () {
			// 檢查區塊是否已展開
			if (content.is(":visible")) {
				// 如果是，則重新計算並設置 navigationArrow 的位置
				var activeItem = $(".noteNavigation li.active");
				if (activeItem.length) {
					calculateAndSetMarkerTopPosition(activeItem);
				}
			}
		});
	});

	function removeSpecialCharactersFromTitle(title) {
		// return title.replace(/[^a-zA-Z ]/g, "");
		return title.replace(/[^\w一-龥\d]/g, "");
	}

	function createIdFromTitle(title) {
		let titleId = title.toLowerCase();
		titleId = titleId.replace(/ /g, "_");
		return titleId;
	}

	function generateNavForTitle(title, titleId) {
		return '<li><a href="#' + titleId + '">' + title + "</a></li>";
	}

	let currentTitleIndex = 0;
	noteContentTitles.each(function () {
		let title = $(this).text();
		let listTitle = removeSpecialCharactersFromTitle(title);
		let titleId = createIdFromTitle(listTitle);

		//putting id to titles
		$(this).attr("id", titleId);

		//generating titles nav with id
		noteNavigationObject.append(generateNavForTitle(title, titleId));

		//getIndexOfCurrentTitleAfterLoad
		if ($(window).scrollTop() > $(this).offset().top) {
			currentTitleIndex += 1;
		}
	});

	//setActiveTitleAfterLoad
	$(".noteNavigation li").eq(currentTitleIndex).addClass("active");
	//adding active marker
	$(".noteNavigation").append('<div class="noteNavigationArrow"></div>');

	//update marker position
	function calculateAndSetMarkerTopPosition(targetTitle) {
		let previousTitlesHeight = 0;
		$(".noteNavigation li").each(function (i) {
			if (i < targetTitle.index()) {
				previousTitlesHeight += $(this).height();
			}
		});

		const listItemMargin = 24,
			ulPadding = 36,
			markerAlign = 3;
		let topValue =
			ulPadding +
			targetTitle.index() * listItemMargin +
			previousTitlesHeight +
			(targetTitle.height() - 9) / 2 -
			markerAlign;

		$(".noteNavigationArrow").css("top", topValue + "px");
	}

	calculateAndSetMarkerTopPosition($(".noteNavigation li.active"));

	let scroll = $(this).scrollTop();

	$(window).scroll(function () {
		let currentTitle = $(".noteNavigation li.active");
		let currentTitleObj = currentTitle.children().attr("href");

		if (scroll > $(this).scrollTop()) {
			//if scrolling up
			let prevTitle = $(".noteNavigation li.active").prev(),
				prevTitleObj = prevTitle.children().attr("href");

			if (prevTitle.length) {
				if (
					$(this).scrollTop() < $(prevTitleObj).offset().top ||
					$(currentTitleObj).offset().top >=
						window.innerHeight + $(this).scrollTop()
				) {
					currentTitle.removeClass("active");
					prevTitle.addClass("active");

					calculateAndSetMarkerTopPosition(prevTitle);
				}
			}
		} else {
			//if scrolling down
			let nextTitle = $(".noteNavigation li.active").next(),
				nextTitleObj = nextTitle.children().attr("href");

			if (nextTitle.length) {
				if ($(this).scrollTop() >= $(nextTitleObj).offset().top) {
					currentTitle.removeClass("active");
					nextTitle.addClass("active");

					calculateAndSetMarkerTopPosition(nextTitle);
				}
			}
		}
		scroll = $(this).scrollTop();
	});

	// $(window).scroll(function () {
	// 	let currentScroll = $(this).scrollTop();

	// 	let closestTitle = null;
	// 	let closestDistance = Infinity;

	// 	noteContentTitles.each(function () {
	// 		let titleTop = $(this).offset().top;
	// 		let distance = Math.abs(titleTop - currentScroll);

	// 		if (distance < closestDistance) {
	// 			closestDistance = distance;
	// 			closestTitle = $(this);
	// 		}
	// 	});

	// 	if (closestTitle) {
	// 		let titleId = closestTitle.attr("id");
	// 		$(".noteNavigation li.active").removeClass("active");
	// 		$(".noteNavigation li a[href='#" + titleId + "']")
	// 			.parent()
	// 			.addClass("active");
	// 		calculateAndSetMarkerTopPosition($(".noteNavigation li.active"));
	// 	}
	// });

	$(document).on("click", ".noteNavigation li a", function (e) {
		e.preventDefault();

		$(this).parent().siblings("li.active").removeClass("active");
		$(this).parent().addClass("active");

		calculateAndSetMarkerTopPosition($(this).parent());

		const titleMarginTop = 16;
		$("html, body").animate(
			{
				scrollTop: $($(this).attr("href")).offset().top - titleMarginTop,
			},
			500
		);

		return false;
	});
});
