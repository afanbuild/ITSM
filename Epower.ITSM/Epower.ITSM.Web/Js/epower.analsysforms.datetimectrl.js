/*     
 * Date: 2012-8-6 18:31
 * summary: 日期时间选择控件. 特别之处在于: 当选择日期时, 隐藏嵌入的
 * Flash 对象. 选择结束时, 显示 Flash 对象. 
 * reason: div层无法覆盖到 Flash 对象之上.
 * modified: sunshaozong@gmail.com     
 */          
$(document).ready(function(){         
    var _selecttimeconfig = {
        monthNames: new Array('O1', 'O2', 'O3', 'O4', 'O5', 'O6', 'O7', 'O8', 'O9 ', '10', '11', '12'),
        dayNamesMin: new Array('日', '一', '二', '三', '四', '五', '六'),
        dateFormat: 'yy-mm-dd',
        yearRange: '-1:+1',
        currentText: "Today",
        beforeShow: function(input, inst) { $('#myNextDiv').hide(); },
        onClose: function(dateText, inst) { $('#myNextDiv').show(); }
    }
    
    $('#' + epower.date_select_time.begindate).datepicker(_selecttimeconfig);
    $('#' + epower.date_select_time.enddate).datepicker(_selecttimeconfig);
});
