export default async function draft(element, callback = () => {}) {
	$("div.spanner, div.overlay").toggleClass("show");
	let draftData;
	try {
		draftData = await getDraft(element.noteId);
		$("div.spanner, div.overlay").toggleClass("show");
		if (draftData) {
			const confirmed = confirm(
				"You have an unsaved draft. Would you like to restore your edits?"
			);
			if (confirmed) {
				try {
					fillElementWithDraft(element, draftData);
				} catch (error) {
					console.error(error);
					alert("Failed to restore draft, please refresh the page");
				}
			} else {
				const confirmDeleteDraft = confirm(
					"Since you don't need the draft now, would you like to delete it or keep it for later?"
				);
				if (confirmDeleteDraft) {
					await deleteDraft(element.noteId);
				}
			}
		}
		await callback();
		await autoSaveDraft(element);
	} catch (error) {
		console.error(error);
		alert("Fetch draft data failed, check your network connection");
	}
}

function fillElementWithDraft(
	{ category, title, content, keyword, pic, coAuthor, preImg, hdnImg },
	draft
) {
	if (category) {
		category.find(`input[value="${draft.categoryId}"]`).prop("checked", true);
	}
	title.val(draft.title);
	keyword.val(draft.keyword);
	content.setData(draft.content);
	coAuthor.html(draft.coAuthor);
	if (draft.pic) {
		const blob = new Blob([new Uint8Array(atob(draft.pic).length)], {
			type: "image/png",
		});
		// Create a file from the Blob
		const file = new File([blob], "cover.png", { type: "image/png" });
		if (file.size != 0) {
			var reader = new FileReader();
			reader.onload = function (e) {
				// 將讀取的圖片數據設置為 <img> 的 src
				preImg.attr("src", `data:image/png;base64,${draft.pic}`);
				$(".cover-container").show(); // 顯示圖片
				hdnImg.val(draft.pic);
			};
			// 讀取選擇的文件
			reader.readAsDataURL(file);
		}
	} else {
		preImg.attr("src", "");
		$(".cover-container").hide();
		hdnImg.val("");
	}
}

function autoSaveDraft(element) {
	let timeoutDraft;
	async function setSaveTimeout() {
		const res = await fetch("/api/users/auth-state");
		const authState = await res.json();
		if (authState.status === "active") {
			if (timeoutDraft) {
				clearTimeout(timeoutDraft);
			}
			timeoutDraft = setTimeout(() => {
				saveDraft(element);
			}, 3000);
		} else {
			location.href = "/Sign";
		}
	}
	window.addEventListener("keydown", setSaveTimeout);
	$("main input, div#input-co-author, #fuCoverPhoto").on(
		"change",
		setSaveTimeout
	);
	window.addEventListener("beforeunload", () => {
		clearTimeout(timeoutDraft);
	});
}

async function getDraft(noteId) {
	return await callGetApi(noteId);
}

async function deleteDraft(noteId) {
	await callDeleteApi(noteId);
}

function saveDraft(element) {
	return new Promise((resolve, reject) => {
		var reader = new FileReader();
		var fileByteArray = [];
		var fileEntity = element.pic.get(0).files[0];

		reader.onloadend = function (evt) {
			if (evt.target.readyState == FileReader.DONE) {
				var arrayBuffer = evt.target.result,
					array = new Uint8Array(arrayBuffer);
				for (var i = 0; i < array.length; i++) {
					fileByteArray.push(array[i]);
				}
				callSaveApi(element, fileByteArray).then(resolve).catch(reject);
			}
		};

		reader.onerror = function (error) {
			console.error(error);
			alert("Error reading file");
			reject(error);
		};

		try {
			if (fileEntity) {
				reader.readAsArrayBuffer(fileEntity);
			} else {
				fileByteArray = base64ToByteArray(element.hdnImg.val());
				callSaveApi(element, fileByteArray).then(resolve).catch(reject);
			}
		} catch (error) {
			console.error(error);
			alert("Error saving draft");
			reject(error);
		}
	});
}

function callSaveApi(
	{ noteId, category, title, content, keyword, coAuthor },
	fileByteArray
) {
	let draftData = {
		noteId,
		categoryId: category.find(":checked").val(),
		title: title.val(),
		keyword: keyword.val(),
		content: content.getData(),
		coAuthor: coAuthor.html(),
	};
	draftData.pic = fileByteArray.length === 0 ? null : fileByteArray;
	if (navigator.onLine) {
		return fetch("/api/drafts/save", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify(draftData),
		})
			.then((response) => response.json())
			.then((data) => {
				//console.log('draft auto saved：', data);
			})
			.catch((error) => {
				console.error("auto save draft error：", error);
			});
	} else {
		alert("Your internet connection is down. Please try again later.");
		console.log("user is offline.");
	}
}

async function callGetApi(noteId) {
	if (navigator.onLine) {
		try {
			const response = await fetch(`/api/drafts/get/${noteId}`);
			if (response.status == 200) {
				const data = await response.json();
				return data;
			} else {
				return null;
			}
		} catch (error) {
			console.error("get draft error：", error);
			return null;
		}
	} else {
		alert("Your internet connection is down. Please try again later.");
		console.log("user is offline.");
	}
}

function callDeleteApi(noteId) {
	if (navigator.onLine) {
		return fetch(`/api/drafts/delete/${noteId}`, {
			method: "DELETE",
		})
			.then((response) => {
				if (response.ok) {
				} else {
					console.error("Delete draft error:", response.statusText);
				}
			})
			.catch((error) => {
				console.error("Delete draft error:", error);
			});
	} else {
		alert("Your internet connection is down. Please try again later.");
		console.log("user is offline.");
	}
}

function base64ToByteArray(base64) {
	const byteCharacters = atob(base64);
	const byteArray = new Uint8Array(byteCharacters.length);

	for (let i = 0; i < byteCharacters.length; i++) {
		byteArray[i] = byteCharacters.charCodeAt(i);
	}

	let bytes = [];

	for (let i = 0; i < byteArray.length; i++) {
		bytes[i] = byteArray[i];
	}

	return bytes;
}
