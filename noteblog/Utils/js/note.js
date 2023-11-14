
async function startPolling() {
    await getLastUpdateTime();

    var timer = setInterval(async function () {
        // 發送 Ajax 請求到 API，檢查是否有更新
        await getLastUpdateTime();
    }, 60000);

    window.addEventListener('beforeunload', function () {
        // 在卸載事件中停止計時器
        clearInterval(timer);
    });
}

async function getLastUpdateTime() {
    try {
        const note = await $.ajax({
            url: '/api/notes/getLatestNote',
            method: 'GET',
            dataType: 'json',
        });
        $("#hidLastUpdateTime").val(note.updatedAt);
    } catch (error) {
        console.error('error:', error);
    }
}

function clearCache() {
    location.reload(true);
}

