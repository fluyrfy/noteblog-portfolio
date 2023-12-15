var contentEditor;

const watchdog = new CKSource.EditorWatchdog();

window.watchdog = watchdog;

watchdog.setCreator((element, config) => {
	return CKSource.Editor.create(element, config).then((editor) => {
		editor.model.document.on("change:data", () => {});

		contentEditor = editor;

		return editor;
	});
});

watchdog.setDestructor((editor) => {
	return editor.destroy();
});

watchdog.on("error", handleError);

watchdog
	.create(document.querySelector(".ck-editor"), {
		codeBlock: {
			languages: [
				{ language: "plaintext", label: "Plain text" }, // The default language.
				{ language: "c", label: "C" },
				{ language: "cs", label: "C#" },
				{ language: "cpp", label: "C++" },
				{ language: "css", label: "CSS" },
				{ language: "diff", label: "Diff" },
				{ language: "html", label: "HTML" },
				{ language: "java", label: "Java" },
				{ language: "javascript", label: "JavaScript" },
				{ language: "php", label: "PHP" },
				{ language: "python", label: "Python" },
				{ language: "r", label: "R" },
				{ language: "sql", label: "SQL" },
				{ language: "ruby", label: "Ruby" },
				{ language: "typescript", label: "TypeScript" },
				{ language: "xml", label: "XML" },
			],
		},
		extraAllowedContent: "pre[data-language]",
	})
	.catch(handleError);

function handleError(error) {
	console.error("Oops, something went wrong!");
	console.error(
		"Please, report the following error on https://github.com/ckeditor/ckeditor5/issues with the build id and the error stack trace:"
	);
	console.warn("Build id: 31o3b59jzmiv-rq35etlei2s");
	console.error(error);
}

const addDataLanguage = () => {
	const data = contentEditor.getData();
	const tempDiv = document.createElement("div");
	tempDiv.innerHTML = data;
	const renderedPres = tempDiv.querySelectorAll("pre");
	const originalPres = document.querySelectorAll("div.ck-editor__main pre");
	renderedPres.forEach((pre, idx) => {
		pre.setAttribute(
			"data-language",
			originalPres[idx].attributes["data-language"].textContent
		);
	});
	$("#hdnContent").val(tempDiv["innerHTML"]);
};
