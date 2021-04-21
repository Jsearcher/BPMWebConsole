$(document).ready(function () {
    // Validate account and password input in the login form
    $('#forgot_form').validate({
        rules: {
            Email: {
                required: true,
                email: true
            }
        }
    });

    $('#submit_btn').click(function (e) {
        if ($('#forgot_form').valid()) {
            $('#forgot_form').submit();
        }
        return false;
    });

    $('#back_btn').click(function (e) {
        history.go(-1);
        return false;
    });

    // Press Enter key to login
    $(document).keypress(function (e) {
        if (e.keyCode === 13 || e.which === 13 || e.key === "Enter") {
            $('#submit_btn').click();
        }
    });
});