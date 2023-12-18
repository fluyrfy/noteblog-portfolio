$(window).on("load", function () {
	$("#apiScript").remove();
	$(".bar-item-chat, .btn-close-chat, .block_openai_chat::before").on(
		"click",
		function () {
			$(".block_openai_chat").toggle(function () {
				$("html").toggleClass("noscroll", $(this).is(":visible"));
				$("#openai_input").focus();
			});
		}
	);
});

let messages = [];
var questionString = "Ask a question...";
var errorString = "An error occurred! Please try again later.";
var codeBlockRegex = /```(\w+)\n([\s\S]*?)```/g;
var htmlRegex = /<!DOCTYPE html>([\s\S]*)<\/html>/;
var combinedRegex = new RegExp(codeBlockRegex.source + "|" + htmlRegex.source);

document.querySelector("#openai_input").addEventListener("keyup", (e) => {
	if (!e.shiftKey && e.which === 13 && e.target.value.trim().length > 0) {
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
	hljs.highlightAll();
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
				var codeBlocks = ansContent.match(codeBlockRegex);
				var htmlBlocks = ansContent.match(htmlRegex);
				var processedText = ansContent;
				if (codeBlocks) {
					processedText = processedText.replace(
						codeBlockRegex,
						(match, language, code) => {
							return `<pre data-language="${language}">
                       <code class="language-${language}">${code.trim()}</code>
                   </pre>`;
						}
					);
				}

				if (htmlBlocks) {
					processedText = processedText.replace(htmlRegex, (html) => {
						const codeBlock = document.createElement("pre");
						codeBlock.setAttribute("data-language", "html");
						const codeElement = document.createElement("code");
						codeElement.classList.add("hljs", "language-html");
						codeElement.innerText = html;
						codeBlock.appendChild(codeElement);
						return codeBlock.outerHTML;
					});
					console.log(processedText);
				}
				addToChatLog("bot", processedText);
			} catch (error) {
				console.error(error);
				addToChatLog("bot", data.error.message);
			}
			document.querySelector("#openai_input").focus();
		})
		.catch((error) => {
			document.querySelector("#openai_input").classList.add("error");
			document.querySelector("#openai_input").placeholder = errorString;
		});
};
