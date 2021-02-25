$(document).ready(function () {
    // Initialize displaying status of left menu
    InitMenuStatus(2);

    // Dashboard標題列名稱
    $('#function_title').text("統計數量查詢");

    // 調整BPM處理數量統計結果之顯示樣式
    var query_status = $('#FormsModel_QueryStatus').val();
    if (query_status !== null) {
        if (parseInt(query_status) === -1) {
            // Show warning dialog
        }
        else if (parseInt(query_status) === 1) {
            TableHeaderFixed('.result_container .table_container .grid_area .table_setting table');
            var result_col_count = $('.result_container .grid_area .table_setting table tbody tr td:nth-child(3)');
            result_col_count.each(function () {
                var td_val = parseInt($(this).text());
                $(this).text(Number2Currency(td_val));
            });
        }
    }

    $('#graphic_disp_chk').change(function () {
        if (this.checked) {
            $('.result_container .graphic_container .bar_chart_area').css('display', 'block');
        }
        else {
            $('.result_container .graphic_container .bar_chart_area').css('display', 'none');
        }
    });
});

