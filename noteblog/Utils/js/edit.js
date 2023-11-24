$(document).ready(function () {
    if ($("#hdnImgData").val() == "") {
        $(".cover-container").hide();
    }

    $("#fuCoverPhoto").on("change", function () {
        var fileInput = $(this)[0];
        if (fileInput.files && fileInput.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                // 將讀取的圖片數據設置為 <img> 的 src
                $("#MainContent_imgCover").attr("src", e.target.result);
                $(".cover-container").show(); // 顯示圖片
                $("#hdnImgData.ClientID").val(e.target.result.split(',')[1])
            };
            // 讀取選擇的文件
            reader.readAsDataURL(fileInput.files[0]);
        } else {
            $(".cover-container").hide();
        }
    })

})

$(".remove-img-btn").on("click", function () {
    $("#hdnImgData, #fuCoverPhoto").val("");
    $(".cover-container").hide();
})