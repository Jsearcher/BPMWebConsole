$(document).ready(function () {
    // Validate account and password input in the login form
    $('#login_form').validate({
        rules: {
            Account: {
                required: true,
                email: true
            },
            Password: {
                required: true,
                minlength: 7
            },
            CaptchaString: {
                required: true,
                rangelength: [4, 4]
            }
        }
    });

    $('#submit_btn').click(function (e) {
        if ($('#login_form').valid()) {
            $('#login_form').submit();
        }
        return false;
    });

    $('#captcha_img').click(function (e) {
        let url = $(this).attr("src");
        let urlSplit = url.split('?');
        let extraVar = new Date().getTime();

        if (urlSplit.length === 2) {
            urlSplit[1] = "timestamp=" + extraVar
            url = urlSplit.join('?');
        }
        else if (urlSplit.length === 1) {
            url = url + "?timestamp=" + extraVar;
        }
        
        $(this).attr("src", url);
    });

    // Press Enter key to login
    $(document).keypress(function (e) {
        if (e.keyCode === 13 || e.which === 13 || e.key === "Enter") {
            $('#submit_btn').click();
        }
    });
});