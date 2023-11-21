window.onloadTurnstileCallback = function () {
    turnstile.render('.cfturnstile', {
        sitekey: '0x4AAAAAAANJgZydX09IuVou',
        theme: "light",
        callback: function (token) {
            $.ajax({
                url: '/api/verify/turnstile',
                method: 'POST',
                data: {
                    token
                },
                success: function (result) {
                    const turnstile = JSON.parse(result).success;
                    if (turnstile == null || turnstile == false) {
                        Page_IsValid = false;
                        alert("User appears to be invalid or suspicious.");
                        window.location.reload();
                    } else {
                        $(".signup-btn").toggleClass("disabled", !turnstile);
                        $(".signup-btn").prop("disabled", !turnstile);
                    }
                },
                error: function (error) {
                    alert('Error:', error);
                },
            });
        },

        'expired-callback': () => {
            alert("The token has expired, please revalidate.");
            window.location.reload();
        },
    });
};