
//按自己的统一格式进行客户端的日期操作 2003年04月01日 14:01 或 2003年04月01日

//   重要提示： 问题悬而未决  ---》 正则表达式 中带有汉字，放在js 中就不匹配不了，WHY？？？？？
// 操作之前，将日期格式转为 yyyy-MM-dd hh:mm
// maxDate: new Date(new Date().getFullYear(), 11, 30)
var selectDatepickerConfig = {
    monthNames: new Array('O1', 'O2', 'O3', 'O4', 'O5', 'O6', 'O7', 'O8', 'O9 ', '10', '11', '12'),
    dayNamesMin: new Array('日', '一', '二', '三', '四', '五', '六'),
    dateFormat: 'yy-mm-dd',
    yearRange: '-1:+1',
    currentText: "Today"
   
};
var popupAlertMsg = '1';
function parseDate(dateStr) {
    var year;
    var month;
    var date;



    //if (dateStr.match(/\d\d\d\d\/|年[01]*\d\/|月[0123]*\d/|/日/ig)==null) {
    if (dateStr.match(/\d\d\d\d\/|-[01]*\d\/|-[0123]*\d/ig) == null) {
        return null;
    }





    var dates = dateStr.split(/\/|-|:|\s/);
    //var dates = dateStr.split(/\/|-|:|\s/);
    //alert(dates[0] + '  ' + dates[1] + ' ' + dates[2]);
    var year = parseInt(dates[0], 10);
    if (year <= 0 || year > 10000) {
        return null;
    }
    var month = (parseInt(dates[1], 10) + 11) % 12;
    var day = parseInt(dates[2], 10);
    var lastDay = getLastDayInMonth(year, month);
    if (day <= 0 || day > lastDay) {
        return null;
    }

    var hour = 0;
    var minute = 0;
    var second = 0;
    if (dates.length > 3) {
        hour = (parseInt(dates[3], 10) + 24) % 24;
        if (hour >= 24 || hour < 0) {
            return null;
        }
    }
    if (dates.length > 4) {
        minute = (parseInt(dates[4], 10) + 60) % 60;
        if (minute >= 60 || minute < 0) {
            return null;
        }
    }
    if (dates.length > 5) {
        second = (parseInt(dates[5], 10) + 60) % 60;
        if (second >= 60 || second < 0) {
            return null;
        }
    }
    return new Date(year, month, day, hour, minute, second);
}

function parseString(date) {
    if (typeof (date) != "string") {
        var dateStr = "";
        dateStr = "000" + date.getFullYear() + "-";
        dateStr = dateStr.substr(dateStr.length - 5);
        var tmp = date.getMonth() + 1;
        if (tmp > 9) {
            dateStr += tmp + "-";
        } else {
            dateStr += "0" + tmp + "-";
        }
        tmp = date.getDate();
        if (tmp > 9) {
            dateStr += tmp + " ";
        } else {
            dateStr += "0" + tmp + " ";
        }
        tmp = date.getHours();
        if (tmp > 9) {
            dateStr += tmp + ":";
        } else {
            dateStr += "0" + tmp + ":";
        }
        tmp = date.getMinutes();
        if (tmp > 9) {
            dateStr += tmp + ":";
        } else {
            dateStr += "0" + tmp + ":";
        }
        tmp = date.getSeconds();
        if (tmp > 9) {
            dateStr += tmp;
        } else {
            dateStr += "0" + tmp;
        }
        return dateStr;
    } else {
        return "";
    }
}

function compareDates(date1, date2) {
    if (typeof (date1) == "string") {
        date1 = parseDate(date1);
    }
    if (typeof (date2) == "string") {
        date2 = parseDate(date2);
    }
    if (date1 != null && date2 != null) {
        date1 = date1.valueOf();
        date2 = date2.valueOf();
        if (date1 > date2) {
            return 1; //大于
        } else if (date1 == date2) {
            return 0;  //等于
        } else {
            return -1;  //小于
        }
    }
    else {
        return -2;  //不能比较
    }
}

//获取某年某月的最后一天
function getLastDayInMonth(fullYear, month) {
    var lastDay;
    if (month == 0 || month == 2 || month == 4 || month == 6 || month == 7 || month == 9 || month == 11) {
        lastDay = 31;
    }
    else if (month == 3 || month == 5 || month == 8 || month == 10) {
        lastDay = 30;
    }
    else {
        if (fullYear % 4) {
            lastDay = 28;
        } else {
            if (fullYear % 400) {
                lastDay = 29;
            } else {
                lastDay = 28;
            }
        }
    }
    return lastDay;
}

function isDate(dateStr) {
    var year;
    var month;
    var date;
    var isDate;
    if (dateStr.length == 0) {
        return true;
    }
    if (dateStr.length > 10) {
        return false;
    }
    if (dateStr.match(/[12][09]\d\d-[01]*\d-[0123]*\d/ig) == null) {
        return false;
    }
    try {
        var dates = dateStr.split("-");
        year = parseInt(dates[0], 10);
        month = parseInt(dates[1], 10);
        date = parseInt(dates[2], 10);
    } catch (e) {
        return false;
    }
    if (month > 12 || month <= 0) {
        return false;
    }
    if (date == 0) {
        return false;
    }
    var lastDay;
    if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) {
        lastDay = 31;
    }
    else if (month == 4 || month == 6 || month == 9 || month == 11) {
        lastDay = 30;
    }
    else {
        if (year % 4) {
            lastDay = 28;
        } else if (year % 100) {
            lastDay = 29;
        } else if (year % 400) {
            lastDay = 28;
        } else {
            lastDay = 29;
        }
    }
    if (date > lastDay) {
        return false;
    }
    return true;
}

//判断字符串长度
function MaxLength(txttemp, maxlength, strmsg) {
    var l = txttemp.value.length;
    if (maxlength < l) {
        alert(strmsg + maxlength);
        txttemp.focus();
        event.cancelBubble = true;
        event.returnValue = false;
        return false;
    }
    else return true;
}

