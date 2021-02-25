var interval = 10000;   // 10秒

$(document).ready(function () {
    // Initialize displaying status of left menu
    InitMenuStatus(1);

    // Dashboard標題列名稱
    $('#function_title').text("狀態監控");

    RenderStatusVC();
});

// Render the View Component of statuses within a time period (10 seconds)
function RenderStatusVC() {
    var res_bpm_status = AJAXGet_Base("../Dashboard/GetCommStatusVC/1");
    var res_airline_status = AJAXGet_Base("../Dashboard/GetCommStatusVC/2");
    var res_brs_status = AJAXGet_Base("../Dashboard/GetCommStatusVC/3");
    if (res_bpm_status !== null) {
        $('#bpm_status').children('div').remove();
        $('#bpm_status').append(res_bpm_status);
    }
    if (res_airline_status !== null) {
        $('#airline_status').children('div').remove();
        $('#airline_status').append(res_airline_status);

    }
    if (res_brs_status !== null) {
        $('#brs_status').children('div').remove();
        $('#brs_status').append(res_brs_status);
    }

    setTimeout(RenderStatusVC, interval);
}