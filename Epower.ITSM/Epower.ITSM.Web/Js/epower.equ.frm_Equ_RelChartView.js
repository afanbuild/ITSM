/*     
 * Date: 2012-8-6 18:38
 * summary: 资产关联图的用户视角和偏好. 可以跳转到选择的视角, 可以设置自己的
 * 视角偏好. 
 * modified: sunshaozong@gmail.com     
 
 * Date: 2012-9-17 18:57
 * summary: 添加资产预警监控状态. 针对 IE , Chrome 等浏览器做了专门处理. 实现逻辑: 以指定间隔
 * 在服务器下载最新资源状态, 然后更新图上各资产的提示图标和预警详情
 */        
 
 var EPOWER_EQU_RESOURCE_MONITORING_IMAGE_TAG = "IMGEQ";
 
 /* 页面上的资源编号集 */
 var ID_LIST = new Array();
 
$(document).ready(function(){
    // begin: 加载资产状态    
    if ( $.browser.msie ) {
        load_resource_state_ie();
    } else {
        load_resource_state_webkit();
    }   
    // end.

    $('.div-tabs-content').click(function(){
         var span = $(this).find('span');        
         var relkey = span.text();
         if (relkey == '默认视角') { relkey = 'default'; }
         if (relkey == '偏好设置') { relkey = 'prefer'; }
         
         window.location.href = switchUrl(relkey);
    });
    
    (function(){
         var r_relkeyid = /&search_key=(.*?)$/i;
         var url = window.location.href;
         if (r_relkeyid.test(url)) {
             var array = url.match(r_relkeyid);
             if (array.length > 1) {                                 
                 
                 $('.div-tabs-opend2').each(function(){                    
                     $(this).removeClass('div-tabs-opend2').addClass('STYLE4');
                 });            
                 $('.div-tabs-opend').css('background-image', 'url(../Images/lm-a.gif)');
                 $('.div-tabs-opend').removeClass('div-tabs-opend').addClass('STYLE4');
                  
                 var relkey = decodeURIComponent(array[1].toLowerCase());
                 $('#div-tabs-container span').each(function(){
                     var val = $(this).text().toLowerCase();                     
                     if (val == relkey || ((relkey == 'default' && val == '默认视角'))
                                       || ((relkey == 'prefer' && val == '偏好设置'))) {                                                 
                         $(this).addClass('div-tabs-opend2').removeClass('STYLE4')
                         .parent().removeClass('STYLE4').addClass('div-tabs-opend2')
                         .addClass('div-tabs-opend');
                                     
                         $(this).parent().css('background-image', 'url(../Images/lm-2b.gif)');            
                         return false;
                     } 
                 });                                     
             }
         }
     })();
});

/* 切换需要转到的URL, 根据选择显示不同的视角面板. */
function switchUrl(relkey) {
    relkey = encodeURIComponent(relkey);
    
    var r_relkeyid = /&search_key=(.*?)$/i, r_typeid = /Type=\d{1,}&/i;
    var r_relkeyid_default = /&search_key=default/i;
    var url = window.location.href;
    
    /*     
     * Date: 2012-8-7 16:48
     * summary: 针对通过 [资产影响分析] 路径进来时的处理.
     * 需要移除掉 typeid, 才能进入到资产关联图视角.
     * modified: sunshaozong@gmail.com     
     */        
    if (r_typeid.test(url)) {
        url = url.replace(r_typeid,'');
    }
    
    if (r_relkeyid.test(url)){        
        url = url.replace(r_relkeyid, "&search_key=" + relkey);
    } else if (r_relkeyid_default.test(url)) {
        url = url.replace(r_relkeyid_default, "&search_key=" + relkey);
    } else { url = url +  "&search_key=" + relkey; }           
        
    return url;     
}

/* IE 系列浏览器专门处理 */
function load_resource_state_ie() {
    var tpl_img_container = '<div style="position: relative;"></div>';
                    
    var id_list = new Array();     
        
    $('.equ_object').each(function(i){        
        
        var id = $(this).attr('id'); 
        
        id = id.substring(3); 
        id_list.push(id)                     
        
        // 放入资源编号集
        window.ID_LIST.push(id);  
                
        var container = $(tpl_img_container);
        container.css('top', -70);
        container.css('left', 70);
        
        container.attr('top', -70);
        container.attr('left', 70);        
        
        //container.text(container.text() + id);
    
        container.mouseover(function(){
            // $(this).find('img:first').attr('src','../images/resource_state_error.gif');                                   
            
            $('#alert-float-panel-tpl-1').remove();
            
            var container1 = $('#alert-float-panel-tpl').clone(true);
            var y = $(this).attr('top');
            var x = $(this).attr('left');            
            
            $('#' + $(this).attr('equ_id')).append(container1);
            
            container1.css('top', parseInt(y) + 10);
            container1.css('left', x - 30);
            container1.css('z-index', 700);
            
            container1.attr('id','alert-float-panel-tpl-1');            
            // container1.show();
            //alert(container1[0].outerHTML);
                        
            // container1.attr('id','alert-float-panel-tpl-1');            
            
            view_details_of_resource_state(this);
            
            container1.show();
        }); 
             
        // alert("Rect[id=RECT_" + id + "]");
        //alert( $("Rect[id=RECT_" + id + "]").html());
        $("#RECT_" + id).append(container);             
        container.attr('equ_id', "RECT_" + id);           
        container.attr('id', 'alert_image_' + id);
    });
    
    if (id_list.length == 0) return;    
    setInterval('get_latest_resource_state();', epower.equ.resource_state_interval);
}

/* chrome 等浏览器专门处理 */
function load_resource_state_webkit() {
    var tpl_img_container = '<div style="position: absolute;"><img /><div style="display:none;" /></div>';    
    var id_list = new Array();     
    $('.equ_object').each(function(i){        
        var id = $(this).attr('id'); 
        id = id.substring(5); 
        id_list.push(id);
        
        // 放入资源编号集
        window.ID_LIST.push(id);
        
        var x = $(this).attr('x');
        var y = $(this).attr('y');
        
        var container = $(tpl_img_container);
        container.css('top', parseInt(y) + 20);
        container.css('left', parseInt(x) + 30);
        
        container.attr('top', parseInt(y) + 20);
        container.attr('left', parseInt(x) + 30);
        
        //container.text(container.text() + id);
        container.mouseover(function(){
            //$(this).find('img:first').attr('src','../images/resource_state_error.gif');
            
            $('#alert-float-panel-tpl-1').remove();
            
            var container1 = $('#alert-float-panel-tpl').clone(true);
            var y = $(this).attr('top');
            var x = $(this).attr('left');            
            
            $(document.body).append(container1);
            
            container1.css('top', parseInt(y) + 10);
            container1.css('left', x + "px");
            container1.attr('id','alert-float-panel-tpl-1');            
            
            view_details_of_resource_state(this);
            
            container1.show();
        }); 
        
        container.attr('id', 'alert_image_' + id);
        $(document.body).append(container);                        
    });
    
    if (id_list.length == 0) return;
    
    setInterval('get_latest_resource_state();', epower.equ.resource_state_interval);
}

/* 显示资产监控预警详情 */
function view_details_of_resource_state(container) {
    // begin: 清除上次内容
    $('#alert-float-panel-tpl-1 tr').each(function(i){
        if (i < 3) { return true; }
        
        $(container).remove();
    });
    // end.
               
    
    // begin: 填充内容
    
    var json_text = $(container).find('div:last').text();                        
    
    // alert(json_text);
    var resource_item = eval("(" + json_text + ")");            
    if ( resource_item.MessageList.length < 1 ) { return; }
    
    for ( var rule_idx = 0; 
              rule_idx < resource_item.MessageList.length; rule_idx++ ) {
        var rule_item =  resource_item.MessageList[rule_idx];
        var tpl_rule_head = $('.alert-float-panel-tpl-head:first').clone(true);
        $('#alert-float-panel-tpl-1 table:first').append(tpl_rule_head);
        tpl_rule_head.show();
        
        tpl_rule_head.find('td:first').text(rule_item.Description.substring(0,7));    // rule description
        tpl_rule_head.find('td:eq(1)').html(' <img src="../images/resource_state_' + 
            rule_item.AlertImage + '.gif" />');    // rule alert image.
        
        for ( var rule_oppair_idx = 0; 
                  rule_oppair_idx < rule_item.Messages.length; rule_oppair_idx++ ) {
            var rule_oppair_item = rule_item.Messages[rule_oppair_idx];
            var tpl_rule_item = $('.alert-float-panel-tpl-item:first').clone(true);                
            
            tpl_rule_item.find('td:eq(0)').text(rule_oppair_item.Key);    // key
            tpl_rule_item.find('td:eq(1)').text(rule_oppair_item.Operator);    // operator
            tpl_rule_item.find('td:eq(2)').text(rule_oppair_item.Preset + " " + rule_oppair_item.Symbol);    // preset
            tpl_rule_item.find('td:eq(4)').text(rule_oppair_item.Value + " " + rule_oppair_item.Symbol);    // current value.
            tpl_rule_item.find('td:eq(6)').text(rule_oppair_item.NormalValue + " " + rule_oppair_item.Symbol);    // normal value.               
                                            
                                
            
            $('#alert-float-panel-tpl-1 table:first').append(tpl_rule_item);
            tpl_rule_item.show();
        }
    }
    // end.
}

/* 下载最新资源状态 */
function get_latest_resource_state() {
    var id_list = window.ID_LIST;    
    var json_text = $.toJSON(id_list);
   
    var data = { action: 'get-resource-state', resource_id_list:  json_text};    
    
    $.post('../EquipmentManager/get-resource-state.ashx', data, 
        function(data) {        
            var d = eval("(" + data + ")");            
            if (d.status == 'ok') {        
                
                var resource_list = d.data;    
                for(var i = 0; i < resource_list.length; i++) {
                    
                    var resource_item = resource_list[i];
                    var resource_obj = $('#' + 
                        EPOWER_EQU_RESOURCE_MONITORING_IMAGE_TAG + resource_item.ResourceId);                                        
                    var alert_image = $('#alert_image_' + resource_item.ResourceId);
                    // 清空上次提示图片
                    alert_image.html('');                                                            
                    for ( var k = 0; k < resource_item.MessageList.length; k++ ) {
                        var rule_item = resource_item.MessageList[k];
                    
                        var img = $('<img style="display:none;" src="../images/resource_state_'+ rule_item.AlertImage +'.gif">');                        
                        alert_image.append(img);                                 
                        img.show();
                    }               
                                             
                    // 缓存获取到的资源状态                    
                    alert_image.append('<div style="display:none;">' + $.toJSON(resource_item) + '</div>');
                    // alert_image.find('div:last').text(JSON.stringify(resource_item));                    
                }
            } else { alert( d.message ); }
        });    
}