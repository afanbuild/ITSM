/*     
 * Date: 2012-8-6 18:16
 * summary: 以指定间隔检测页面三个文本框的输入状态. 当本次输入与前次输入
 * 不同时, 即认为改变了输入状态, 即根据输入重新加载页面数据.
 * modified: sunshaozong@gmail.com     
 */           
function check_input_state() {        
    var _deptname = $('#Table1 input:text:first').val(),
        _begindate = $('#Table1 input:text:eq(1)').val();
        _enddate = $('#Table1 input:text:last').val();
        
    if (!window.deptName && !window.beginDate && !window.endDate){
        window.deptName = _deptname;    
        window.beginDate = _begindate;
        window.endDate = _enddate;                
    }
    
    var isChanged = false;                
    if (window.deptName != _deptname){ isChanged = true; }
    if (window.beginDate != _begindate){ isChanged = true; }
    if (window.endDate != _enddate){ isChanged = true; }
    
    if (isChanged) {
        document.getElementById(epower.analsysforms.submit_id).click();        
    }               
}
    
setInterval('check_input_state();', 100);