window.onloadTurnstileCallback = function () {
    turnstile.render('.cfturnstile', {
        sitekey: '0x4AAAAAAANJgZydX09IuVou',
        theme: "light",
        callback: async function (token) {
            const result = await $.ajax({
                url: '/api/verify/turnstile',
                method: 'POST',
                data: {
                    token
                }
            });
            const turnstile = JSON.parse(result).success
            $("#btnSignUp").toggleClass('disabled', !turnstile)
            $("#myButton").prop("disabled", !turnstile);
            if (turnstile == null || turnstile == false) {
                Page_IsValid = false;
                alert("User appears to be invalid or suspicious.");
                window.location.reload();
            }
        },

        'expired-callback': () => {
            alert("The token has expired, please revalidate.");
            window.location.reload();
        },
    });
};