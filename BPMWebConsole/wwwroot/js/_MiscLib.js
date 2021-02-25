/* =============================================== */
/* _MiscLib.js 為此專案所有網頁使用之全域方法        */
/* 共分為以下幾種類別：                              */
/* 1. 字串與字元檢查處理方法                         */
/* 2. 日期與時間取得處理方法                         */
/* 3. 搜尋參數值、頁面導向處理方法                   */
/* 4. 表格處理方法                                  */
/* 5. WebSocket連線處理方法                         */
/* 6. 其他方法                                      */
/*                                                 */
/* 可再增加全域方法並記錄變更者、變更日期             */
/* =============================================== */

/* ======================================== 字串與字元檢查處理方法 ======================================== */

// Do zero padding to a value based on a specified number of digits
function ZeroPadding(value, number) {
    value = (typeof value === "string") ? value : value.toString();
    var diff = number - parseInt(value.length);
    var zero = "";
    for (var i = 0; i < diff; i++) {
        zero = zero + "0";
    }

    return zero + value;
}

// Transfer the format of a number into currency one
function Number2Currency(str, glue) {
    if (isNaN(str)) {
        return NaN;
    }

    glue = (typeof glue === "string") ? glue : ",";
    var digits = str.toString().split('.');

    var IntegerDigits = digits[0].split("");
    var tempArr = [];

    while (IntegerDigits.length > 3) {
        tempArr.unshift(IntegerDigits.splice(IntegerDigits.length - 3, 3).join(""));
    }

    tempArr.unshift(IntegerDigits.join(""));
    digits[0] = tempArr.join(glue);

    return digits.join(".");
}

// Representation of null (empty) data convertion for data field
function CheckDataNULL(data) {
    if (data) {
        if (!data.trim()) {
            data = "-";
        }
        else if (data === "-") {
            data = "";
        }
    }

    return data;
}

// Try to parse a string as a number
function TryParseNo(str) {
    var ret = false;

    if (str !== null) {
        if (str.length > 0) {
            if (!isNaN(str)) {
                ret = true;
            }
        }
    }

    return ret;
}

/* ======================================== 日期與時間取得處理方法 ======================================== */

// Convert date and time into string format [yyyy/MM/dd HH:mm:ss.fff]
function Time2String_Format(date) {
    var Year = date.getFullYear().toString();
    var Month = ZeroPadding((date.getMonth() + 1).toString(), 2);
    var Day = ZeroPadding(date.getDate().toString(), 2);
    var Hour = ZeroPadding(date.getHours().toString(), 2);
    var Minute = ZeroPadding(date.getMinutes().toString(), 2);
    var Second = ZeroPadding(date.getSeconds().toString(), 2);
    var Millisecond = ZeroPadding(date.getMilliseconds().toString(), 3);

    if (Millisecond > 0) {
        return Year + "/" + Month + "/" + Day + " " + Hour + ":" + Minute + ":" + Second + "." + Millisecond;
    }
    else {
        return Year + "/" + Month + "/" + Day + " " + Hour + ":" + Minute + ":" + Second
    }
}

// Convert date and time into string format [yyyyMMddHHmmss.fff]
function Time2String_Concat(date) {
    var Year = date.getFullYear().toString();
    var Month = ZeroPadding((date.getMonth() + 1).toString(), 2);
    var Day = ZeroPadding(date.getDate().toString(), 2);
    var Hour = ZeroPadding(date.getHours().toString(), 2);
    var Minute = ZeroPadding(date.getMinutes().toString(), 2);
    var Second = ZeroPadding(date.getSeconds().toString(), 2);
    var Millisecond = ZeroPadding(date.getMilliseconds().toString(), 3);

    if (Millisecond > 0) {
        return Year + Month + Day + Hour + Minute + Second + "." + Millisecond;
    }
    else {
        return Year + Month + Day + Hour + Minute + Second
    }
}

/* ======================================== 其他方法 ======================================== */

// Make columns in header of a table to have fixed width compared to the ones in table body
function TableHeaderFixed(table_class) {
    var $bodyCells = $(table_class).find('tbody tr:first').children();
    var colWidth;

    // Get the tbody columns width array
    colWidth = $bodyCells.map(function () {
        return $(this).width();
    }).get();

    // Set the width of thead columns
    $(table_class).find('thead tr').children().each(function (i, v) {
        $(v).width(colWidth[i]);
    });
}

// Confirm a string whether if it is a json string that can be converted into json objects
function IsJson(str) {
    if (typeof str === "string") {
        try {
            JSON.parse(str);
        }
        catch (e) {
            return false;
        }
        return true;
    }
    else {
        return false;
    }
}

// Build a AJAX object with required action of http method (GET, POST, ect.)
function AJAXBuild(action) {
    // Initialize AJAXObject
    var res = null;
    this["ajaxObj"] = new ajaxObject(action.toUpperCase());

    // Methods for modifying parameters of AJAX Object(change order: 'url' -> 'data' -> other parameters, success and error methods)
    this["ajaxChangeParam"] = function (key, value) {
        switch (key) {
            case "url":
                if (!value.includes(".ashx")) {
                    this["ajaxObj"]["contentType"] = "application/json; charset=utf-8";
                }
                else {
                    delete this["ajaxObj"]["contentType"];
                }
                this["ajaxObj"][key] = value;
                break;
            case "data":
                if (!("url" in this["ajaxObj"])) {
                    console.log("在'ajaxObj'中沒有'url'的參數值! 請先加入'url'的參數值!");
                }
                else {
                    if (this["ajaxObj"]["url"].includes(".ashx")) {
                        if (typeof value === "object") {
                            this["ajaxObj"][key] = value;
                        }
                        else {
                            console.log("'data'的參數值資料型態不是'object'!");
                        }
                    }
                    else {
                        if (isJson(value)) {
                            this["ajaxObj"][key] = value;
                        }
                        else {
                            console.log("'data'的參數值資料型態不是'JsonString'!");
                        }
                    }
                }
                break;
            case "async":
                if (typeof value === "boolean") {
                    this["ajaxObj"][key] = value;
                }
                else {
                    console.log("'async'的參數值資料型態不是'boolean'!");
                }
                break;
            default:
                if (!("url" in this["ajaxObj"])) {
                    console.log("在'ajaxObj'中沒有'url'的參數值! 請先加入'url'的參數值!");
                }
                else if (!("data" in this["ajaxObj"])) {
                    console.log("在'ajaxObj'中沒有'data'的參數值! 請先加入'data'的參數值!");
                }
                else {
                    this["ajaxObj"][key] = value;
                }
                break;
        }
    }

    // Execute this AJAX object to send a request and get the response(JsonString <WebMethod>; Object <.ashx>; or error)
    this["ajaxExecuteReq"] = function () {
        var exeFlag = false;
        switch (this.type) {
            case "GET":
                exeFlag = "url" in this["ajaxObj"];
                break;
            case "POST":
                exeFlag = "url" in this["ajaxObj"] && "data" in this["ajaxObj"];
                break;
            default:
                exeFlag = "url" in this["ajaxObj"];
                break;
        }

        if (exeFlag) {
            try {
                $.ajax(this["ajaxObj"]);
                if (res) {
                    this["ajaxObj"]["response"] = res;
                }
            }
            catch (e) {
                console.log("在'ajaxObj'中的參數值錯誤或缺少必要的參數!");
            }
        }
        else {
            console.log("在'ajaxObj'中的參數未完成!'!");
        }
    }

    // Private Function
    function ajaxObject(action) {
        this["response"] = null;
        this["type"] = action;
        this["async"] = false;
        this["success"] = function (response) {
            res = response;
        }
        this["error"] = function (xhr) {
            //ShowDialog(xhr.responseText, "error");
            console.log(xhr.responseText);
        }
    }
}

// General form of AJAX GET method
function AJAXGet_Base(url) {
    var AJAXObj = new AJAXBuild("GET");
    AJAXObj.ajaxChangeParam("url", url);
    AJAXObj.ajaxExecuteReq();

    return AJAXObj.ajaxObj.response;
}

// General form of AJAX POST method
function AJAXPost_Base(url, data) {
    var AJAXObj = new AJAXBuild("POST");
    AJAXObj.ajaxChangeParam("url", url);
    if (url.includes(".ashx")) {
        AJAXObj.ajaxChangeParam("data", data);
    }
    else {
        AJAXObj.ajaxChangeParam("data", JSON.stringify(data));
    }
    AJAXObj.ajaxChangeParam("dataType", "json");
    AJAXObj.ajaxExecuteReq();

    return AJAXObj.ajaxObj.response;
}