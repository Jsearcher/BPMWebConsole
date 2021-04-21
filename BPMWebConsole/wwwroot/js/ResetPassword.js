$(document).ready(function () {
    // Validate account and password input in the login form
    $('#reset_form').validate({
        rules: {
            Password: {
                required: true,
                minlength: 7
            },
            ConfirmPassword: {
                required: true,
                equalTo: "#Password"
            }
        }
    });

    $('#submit_btn').click(function (e) {
        if ($('#reset_form').valid()) {
            $('#reset_form').submit();
        }
        return false;
    });

    // Press Enter key to login
    $(document).keypress(function (e) {
        if (e.keyCode === 13 || e.which === 13 || e.key === "Enter") {
            $('#submit_btn').click();
        }
    });
});