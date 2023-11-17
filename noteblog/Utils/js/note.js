
async function startPolling() {
    getLastUpdateTime();

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
        const updatedAt = await $.ajax({
            url: '/api/notes/getLatestNoteTime',
            method: 'GET',
            dataType: 'json',
        });
        $("#hidLastUpdateTime").val(updatedAt);
    } catch (error) {
        console.error('error:', error);
    }
}

function clearCache() {
    location.reload(true);
}

