function autoSaveDraft(element) {

    const interval = setInterval(function () { saveDraft(element) }, 30000);

    window.addEventListener("beforeunload", () => {
        clearInterval(interval);
    });
}

function saveDraft({ development, title, content, keyword, pic }) {
    var draftData = {
        development: development.find(":checked").val(),
        pic: pic.get(0).files[0],
        title: title.val(),
        keyword: keyword.val(),
        content: content.getData(),
    }
    if (navigator.onLine) {
        fetch('/api/drafts/save', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(draftData),
        })
            .then(response => response.json())
            .then(data => {
                console.log('draft auto saved：', data);
            })
            .catch(error => {
                console.error('auto save draft error：', error);
            });
    } else {
        alert("Your internet connection is down. Please try again later.")
        console.log("user is offline.");
    }
}