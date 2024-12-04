$(document).ready(function(){
     $('#ex3a').jqm({
            modal: true,
            trigger: '#ex3aTrigger',
            overlay: 30, /* 0-100 (int) : 0 is off/transparent, 100 is opaque */
            overlayClass: 'whiteOverlay'});
            
     $('#btnAddRelName').click(function(){
        $('#ex3a').jqmShow();
     });
     
     $('#btnCancel').click(function(){
        $('#ex3a').jqmHide();
     });
     
     $('#btnConfirm').click(function(){
        var val = $('#txtSearchKey').val();        
        if (!val) {
            alert('请输入关联名称!'); return;
        }
        
        $.get('frmEquRelMain.aspx', {action:'addrelname', relname: val}, function(data){
            if (data) {
                window.location.href= switchUrl(data);
            } else {
                alert('服务器内部错误:' + data);
            }
        });               
     });
     
     $('#TablesTitle .switchTab').click(function(){
         var span = $(this).find('span');        
         var relkey_id = span.attr('id');
         
         window.location.href = switchUrl(relkey_id);
     });
     
     (function(){
         var r_relkeyid = /&ddlrelkeyid=(\d{1,})/i;
         var url = window.location.href;
         if (r_relkeyid.test(url)) {
             var array = url.match(r_relkeyid);
             if (array.length > 1) {                 
                 $('#TablesTitle td.td_3').each(function(){                    
                     $(this).removeClass('td_3').css('background-image', 'url(../Images/lm-a.gif)').find('span').removeClass('td_3').addClass('STYLE4');                                          
                 });            
                  
                 var relkeyid = array[1];
                 
                 $('#' + relkeyid).addClass('td_3');                 
                 $('#' + relkeyid).parent().removeClass('STYLE4').addClass('td_3').css('background-image', 'url(../Images/lm-2b.gif)');
             }
         }
     })();
});

/* 切换需要转到的URL, 根据选择显示不同的视角面板. */
function switchUrl(relkey_id) {
    var r_relkeyid = /&ddlrelkeyid=\d{1,}/i;
    var r_relkeyid_default = /&ddlrelkeyid=default/i;
    var url = window.location.href;
    
    if (r_relkeyid.test(url)){
        url = url.replace(r_relkeyid, "&ddlrelkeyid=" + relkey_id);
    } else if (r_relkeyid_default.test(url)) {
        url = url.replace(r_relkeyid_default, "&ddlrelkeyid=" + relkey_id);
    } else { url = url +  "&ddlrelkeyid=" + relkey_id; }           
        
    return url;     
}