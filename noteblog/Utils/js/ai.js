$(window).on("load", function () {
	$("#apiScript").remove();
	$(".bar-item-chat, .btn-close-chat, .block_openai_chat::before").on(
		"click",
		function () {
			$(".block_openai_chat").toggle(function () {
				$("body").toggleClass("noscroll", $(this).is(":visible"));
			});
		}
	);
});

let messages = [];
var questionString = "Ask a question...";
var errorString = "An error occurred! Please try again later.";
var codeBlockRegex = /```(\w+)\n([\s\S]*?)```/g;

document.querySelector("#openai_input").addEventListener("keyup", (e) => {
	if (e.which === 13 && !e.shiftKey && e.target.value.trim().length > 0) {
		addToChatLog("user", e.target.value);
		createCompletion(e.target.value);
		e.target.value = "";
	}
});

const addToChatLog = (type, message) => {
	let messageContainer = document.querySelector("#openai_chat_log");

	const messageElem = document.createElement("div");
	messageElem.classList.add("openai_message");
	for (let className of type.split(" ")) {
		messageElem.classList.add(className);
	}

	const messageText = document.createElement("span");
	messageText.innerHTML = message;
	messageElem.append(messageText);

	messageContainer.append(messageElem);
	messageElem.style.width = messageText.offsetWidth + 40 + "px";
	messageContainer.scrollTop = messageContainer.scrollHeight;
};

const createCompletion = (message) => {
	// const history = buildTranscript();
	document.querySelector("#openai_input").classList.add("disabled");
	document.querySelector("#openai_input").classList.remove("error");
	document.querySelector("#openai_input").placeholder = questionString;
	document.querySelector("#openai_input").blur();
	addToChatLog("bot loading", "...");

	let newMessages = {
		role: "user",
		content: message,
	};

	messages.push(newMessages);

	fetch("https://api.openai.com/v1/chat/completions", {
		method: "POST",
		headers: {
			"Content-Type": "application/json",
			Authorization: "Bearer " + apiKey,
		},
		body: JSON.stringify({
			model: "gpt-3.5-turbo",
			messages,
		}),
	})
		.then((response) => {
			let messageContainer = document.querySelector("#openai_chat_log");
			messageContainer.removeChild(messageContainer.lastElementChild);
			document.querySelector("#openai_input").classList.remove("disabled");

			if (!response.ok) {
				throw Error(response.statusText);
			} else {
				return response.json();
			}
		})
		.then((data) => {
			let ans = data.choices[0];

			try {
				let ansContent = "";
				if (ans.text) {
					ansContent = ans.text;
				} else {
					ansContent = ans.message.content;
				}
				messages.push({
					role: "assistant",
					content: ansContent,
				});
				var processedText = ansContent.replace(
					codeBlockRegex,
					function (match, language, code) {
						return `<pre><code class="language-${language}">${code.trim()}</code></pre>`;
					}
				);
				addToChatLog("bot", processedText);
				hljs.highlightAll();
			} catch (error) {
				addToChatLog("bot", data.error.message);
			}
			document.querySelector("#openai_input").focus();
		})
		.catch((error) => {
			document.querySelector("#openai_input").classList.add("error");
			document.querySelector("#openai_input").placeholder = errorString;
		});
};
