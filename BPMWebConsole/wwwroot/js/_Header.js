$(document).ready(function () {

    // ===== [Initialization] =====


    // ===== [Event Handlers of Elements] =====


});

// ===== [Private] Function =====

// Display current computer time (per second)
function DispComTime() {
    // Repeat this function every second
    setTimeout(DispComTime, 1000);
    var today = new Date();
    var day_list = ["日", "一", "二", "三", "四", "五", "六"];
    $('#time_count')[0].innerHTML = Time2String_Format(today).substring(11, 19);
    $('#date_count')[0].innerHTML = Time2String_Format(today).substring(0, 10) + " 星期" + day_list[today.getDay()];
}