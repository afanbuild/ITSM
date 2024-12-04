var urls = ['../ajax/httpajax.ashx'];

$(document).ready(function(){
    load_field();
    
    /* 以下代码专门针对 ie8及以下版本的ie样式进行优化 */
    try {
        if ( $.browser.msie ) {
                    
            $('.uploadify').hide();
            $('#divZong').hide();
            if ($.browser.version == '8.0') {                                                                
                $('.uploadify').css({'top':'10px'});                
                $('#divZong').css({'position':'inherit'}); 
                $('#divZong').css({'z-index':'1'});                                             
            }
            else if ($.browser.version == '7.0') {                
                $('.uploadify').css({'top':'5px'});                
                $('#divZong').css({'top':'0px'});                
                $('#divZong').css({'position':'inherit'}); 
                $('#divZong').css({'z-index':'1'});
            }
            else if ($.browser.version == '6.0') {                
                $('.uploadify').css({'top':'-70px'});
                $('#divZong').css({'position':'inherit'}); 
                $('#divZong').css({'z-index':'1'});                                             
            }
            
            $('.uploadify').show(10);
            $('#divZong').show(10);            
        }    
    } catch(e) {alert(e.message)}
    //$('#divZong').css({'top':'-180px'});
});

function load_field() {
    
    $.get(urls[0],'Type=normal_app_get_orderbyfieldandmenu&flowmodelid='+window.flowModelID,function(data){
        if (data) {
            try {
            
                var tbl_baseinfo = $('#' + window.ctrl_prefix + 'PrintArea').find('table:eq(1)');
                var tbl_list = null;
                var field_list = eval('('+ data +')');
                
                if (field_list.length <= 0) { return; }
                var groupid = 0;
                
                for (var idx in field_list) {
                
                    
                    var field = field_list[idx];
                    var ofield = field_list[idx == 0 ? 0 : idx - 1];
                    
                    var isRemarkRow = field.FIELDNAME.indexOf('remark') > -1 || ofield.FIELDNAME.indexOf('remark') > -1;
                    var isRemark = field.FIELDNAME.indexOf('remark') > -1;
                    
                    var fieldname = field.FIELDNAME;
                    
                    if (fieldname.indexOf('cata') > -1) {
                        fieldname = fieldname.replace('cata','cate');
                    } else if (fieldname.indexOf('num') > -1) {
                        fieldname = fieldname.replace('num','number');
                    } else if (fieldname == 'remark') {
                        fieldname = 'remark1';
                    }                    
                                        
                    var div_element = window.ctrl_prefix + 'Show' + fieldname.substr(0,1).toUpperCase() + fieldname.substring(1);                    
                    
                    
                    if (field.CATALOGNAME == '基础资料') {
   
                        var row = $('#head_00001').next().find(' > tbody > tr:last');
                        
                        if (row.find(' > td').size() >= 4 || isRemarkRow) {    // 一行两列
                        
                            if ( isRemarkRow && isRemark ) {
                            
                                if ( row.find(' > td').size() < 4 && ofield.FIELDNAME.indexOf('remark') <= -1) {
                                    row.find(' > td:last').attr('colspan', '3');                                                                            
                                }                                  
                                
                            }
                                                    
                            var list = $('#head_00001').next();
                            list.append('<tr></tr>');    // add new row.                            
                            
                            row = $('#head_00001').next().find(' > tbody > tr:last');
                        }
                        
                        
                        row.append('<td width="11%"></td>');
                        var td = row.find('td:last');
                        
                        var div_lbl = $("#" + div_element).find(' > div:first').detach();                                                               
                        td.append(div_lbl);                        
                        
                        var _align = div_lbl.attr('align');
                        var _class = div_lbl.attr('class');
                        
                        div_lbl.removeAttr('align');
                        div_lbl.removeAttr('class');
                        
                        td.attr('align', _align);
                        td.attr('class', _class);


                        if ( isRemark ) {                        
                            row.append('<td colspan="3" width="89%"></td>');
                        } else {
                            row.append('<td width="35%"></td>');
                        }                                                
                                                
                        var td = row.find('td:last');
                        
                        var div_content = $("#" + div_element).find(' > div:last').detach();                                                      
                        td.append(div_content);
                        
                        var _class = div_content.attr('class');
                                                
                        div_content.removeAttr('class');
                        
                        td.attr('class', _class);                                                                                                                                                                        
                    } else {
                        if (field.GROUPID != groupid) {                        
                            if (row && row.find('> td').size() < 4) {
                                row.find('> td:last').attr('colspan',3);                    
                            }                        
                        
                            var __head = __head_tpl.replace(/#0#/ig, 'head_' + field.GROUPID).replace(/#1#/ig, field.CATALOGNAME);    
                            $('#' + window.ctrl_prefix + 'PrintArea').find(' > table:last').after(__head);

                            var head = $('#head_' + field.GROUPID);
                            head.after('<table class="listContent" id="list_' + field.GROUPID + '" width="100%" align="center"><tbody></tbody></table>');                                                        
                            
                            var list = $('#list_' + field.GROUPID);
                            list.after(__hr);
                            
                            list.append('<tr></tr>');    // add new row.                            
                            
                            groupid = field.GROUPID;                                                        
                        }                                                
                        
                        var row = $('#list_' + field.GROUPID + ' > tbody > tr:last');

                        
                        if (row.find(' > td').size() >= 4 || isRemarkRow   ) {    // 一行两列
                            
                            if ( isRemarkRow && isRemark ) {
                            
                                if ( row.find(' > td').size() < 4 && ofield.FIELDNAME.indexOf('remark') <= -1) {
                                    row.find(' > td:last').attr('colspan', '3');                                                                            
                                }                                  
                                
                            }
                            
                            
                            var list = $('#list_' + field.GROUPID);
                            list.append('<tr></tr>');    // add new row.                            
                            
                            row = $('#list_' + field.GROUPID + ' > tbody > tr:last');
                        }
                        
                        
                        row.append('<td width="11%"></td>');
                        var td = row.find('td:last');
                        
                        var div_lbl = $("#" + div_element).find(' > div:first').detach();                                                               
                        td.append(div_lbl);                        
                        
                        var _align = div_lbl.attr('align');
                        var _class = div_lbl.attr('class');
                        
                        div_lbl.removeAttr('align');
                        div_lbl.removeAttr('class');
                        
                        td.attr('align', _align);
                        td.attr('class', _class);

                        
                        if ( isRemark ) {                        
                            row.append('<td colspan="3" width="89%"></td>');
                        } else {
                            row.append('<td width="35%"></td>');
                        }                                                
                        
                        var td = row.find('td:last');
                        
                        var div_content = $("#" + div_element).find(' > div:last').detach();                                                      
                        td.append(div_content);
                        
                        var _class = div_content.attr('class');
                                                
                        div_content.removeAttr('class');
                        
                        td.attr('class', _class);
                                                                      
                        
                    }                    
                }
                
                if (row.find('> td').size() < 4) {
                    row.find('> td:last').attr('colspan',3);                    
                }
                
            } catch(e) {
                alert(e.message);
            }            
        }                
    });
}


function display(head_groupid) {
    var list_table;
    if (head_groupid == 'head_00001') {
        list_table = $('#' + head_groupid).next();            
    } else {
        list_table = $('#list_'+head_groupid.substring(5));
    }

    var ImgPlusScr = "../Images/icon_expandall.gif";
    var ImgMinusScr = "../Images/icon_collapseall.gif";    
    var displayMode = list_table.css('display');
    
    if (displayMode == 'none') {
        $('#'+head_groupid).find('img:first').attr('src', ImgMinusScr);
        list_table.show(); 
    } else { list_table.hide(); $('#'+head_groupid).find('img:first').attr('src', ImgPlusScr);}
}


var __head_tpl = "<table width='100%' id='#0#' align='center' class='listNewContent'> " +
           " <tbody> " +
                " <tr class='listTitleNew'> " +
                    " <td align='left' valign='top' class='bt_di'> " +
                        " <img src='../Images/icon_collapseall.gif' class='icon' " +
                           " onclick=\"display('#0#')\" " +
                           "  height='16' width='16' align='absbottom'> " +
                           "  <span>#1#</span> " +
                    " </td> " +
                " </tr> " +
            " </tbody> " +
        " </table>";
        
var __hr = "<table class='listContent' id='tabList' width='100%' align='center'> " +
           " <tbody></tbody> " +
        " </table>";        
        