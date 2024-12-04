/*     
 * Date: 2012-9-13
 * summary: 资产状态监控 
 * modified: sunshaozong@gmail.com     
 */        
 
$(document).ready(doc_loaded);

function doc_loaded() {    
    $('#ctrtabbuttons').remove();

    $('#btn_add_rule_item').click(add_rule_item_to_panel);
    // setup rule-item remove button event.
    $('.add_rule_list_item:first input:first').click(function(){
        $(this).parent().remove();
    });
    
    $('#btn_save_changes').click(add_rule_to_db);
    
    $('#btn_open_add_rule_panel').click(function(){
        $('#panel_add_new_rule').show();
        $('#panel_rule_list').hide();    
        $('#btn_close_rule_item_view_panel').hide();
        $('.rule-list .hand').css('background-color','white');
    });
    
    $('#btn_close_add_rule_panel').click(function(){
        $('#panel_add_new_rule').hide();
    });
    
    $('.hand').click(show_rule_item_list);
    
    $('#btn_close_rule_item_view_panel').click(function(){
       $('#panel_rule_list').hide();       
       $('#btn_close_rule_item_view_panel').hide();
    });
    
    $('.remove-rule').click(remove_rule);
    
    var add_rule_item_template = $('#panel_add_new_rule .add_rule_item:first').clone(false);
    add_rule_item_template.find('input:button:last').click(add_rule_item_to_panel_for_update);
    $('#panel_rule_list .panel_rule_list_item').prepend(add_rule_item_template);
    
    $('#panel_rule_list_btn_savechanges').click(function(){
        var desc = $('#txt_description_update').val();
        if ( desc == undefined ) desc = "";        
        
        var way = $('#select_logicway_update').val();
        var alert_image = $('#txt_alert_image_update').val();
        if (alert_image == undefined || alert_image == "") {
            alert("必须设置提示图标！"); return;
        }
        
        var priority = $('#txt_priority_update').val();
        if (priority == undefined || priority == "") {
            alert("必须设置优先级！"); return;
        }
        
        var rule = create_rule();
        rule.AlertImage = alert_image;
        rule.Description = desc;
        rule.Priority = priority;
        rule.ResourceId = window.location.href.match(/\?resource_id=(\d{1,})$/i)[1];
        rule.RuleId = $('#panel_rule_list').attr('rule_id');        
        rule.Way = way;                   
        
        $('#panel_rule_list .rule_item_template').each(function(i){
            if (i != 0)  {                
                var op_pair = create_op_pair();
                op_pair.Key = $(this).attr("key");            
                op_pair.Value = $(this).attr("val");
                op_pair.Operator = $(this).attr("operator");
                
                rule.OPPairs.push(op_pair);
            }
        });        
        
        rule.CTIME = new Date().toDateString();
        
        var json_text = JSON.stringify(rule);
        
        var data = { action:"update_rule", "data": json_text };
        
        $.post("../EquipmentManager/resource_rule_manager.ashx", data, 
                    function(data) {
                        var d = eval("(" + data + ")");
                        if (d.status == 'ok') {
                            $('#panel_rule_list').hide();
                            $('.hand').each(function(i){
                                var rule_id = $(this).find('td:eq(3)').text();
                                if (rule_id == rule.RuleId) {
                                    $(this).find('td:eq(0)').text(rule.Description);
                                    $(this).find('td:eq(1)').html('<img src="../images/resource_state_' + rule.AlertImage + '.gif" width="25" height="25">');
                                    $(this).find('td:eq(2)').text(rule.Priority);
                                    $(this).find('td:eq(3)').text(rule.RuleId);
                                    $(this).find('td:eq(4)').text(rule.Way);
                                    $(this).find('td:eq(6) div:first').text(json_text);
                                    
                                    $('#panel_rule_list').hide();
                                    return false;
                                }
                            });
                        } else {
                            alert(d.message);
                        }
                    });                        
    });
}

function remove_rule() {
        var container = $(this);
        var rule_id = container.attr('ruleid');
        var data = { action:"delete_rule", "rule_id": rule_id };
        if (!confirm("真的要删除该规则吗？")) { return; }
        
        $.post("../EquipmentManager/resource_rule_manager.ashx", data, 
                    function(data) {
                        var d = eval("(" + data + ")");
                        if ( d.status == 'error' ) {                            
                            alert('删除规则失败: ' + d.message);
                        } else if ( d.status == 'ok' ) {
                            container.parent().hide(300);
                            container.parent().parent().remove();
                            $('#panel_rule_list').hide();
                            $('#btn_close_rule_item_view_panel').hide();
                        }
                    });
}

function add_rule_to_db() {
        var description = $('#txt_description').val();
        if (description == undefined) {
            description = "";
        }
        
        var way = $('#logic_way').val();
        
        var alert_image = $('#txt_alert_image').val();
        var priority = $('#txt_priority').val();
        
        if (alert_image == undefined || priority == undefined
            || alert_image == "" || priority == "") {
            alert("请设置显示图片和规则优先级"); return;
        }
        
        var rule = create_rule();
        rule.Description = description;
        rule.Way = way;
        rule.AlertImage = alert_image;
        rule.Priority = priority;
        rule.ResourceId = window.location.href.match(/\?resource_id=(\d{1,})$/i)[1];
                
        $('.add_rule_list_item').each(function(i){
            if (i != 0)  {
            
            var op_pair = create_op_pair();
            op_pair.Key = $(this).attr("key");            
            op_pair.Value = $(this).attr("val");
            op_pair.Operator = $(this).attr("operator");
            
            rule.OPPairs.push(op_pair);
            }
        });
        
        rule.CTIME = new Date().toDateString();
        
        var json_text = JSON.stringify(rule);
        
        var data = { action:"add_new_rule", "data": json_text };
        
        $.post("../EquipmentManager/resource_rule_manager.ashx", data, 
                    function(data) {                        
                        d = eval("(" + data + ")");
                        if (d.status == "ok") {                                     
	                        var rule1 = d.data;
	                        var tr_template = $('.rule-list tr:first').clone(false);
	                        
                            tr_template.find('td:first').text(rule1.Description);                               
                            tr_template.find('td:eq(1)').html('<img src="../images/resource_state_' + rule1.AlertImage + '.gif" width="25" height="25">');
                            tr_template.find('td:eq(2)').text(rule1.Priority);   
                            tr_template.find('td:eq(3)').text(rule1.RuleId);   
                            tr_template.find('td:eq(4)').text(rule1.Way == 0 ? "And" : "Or");   
                            tr_template.find('td:eq(5)').text(rule1.CTIME);   
                            tr_template.find('td:eq(6)').html(' ');
                            tr_template.find('td:eq(6)').append('<strong class="remove-rule" style="cursor:pointer;" ruleid="' + rule1.RuleId + '">移除</strong><div style="display:none;">' + JSON.stringify(rule1) + '</div>');
                     
                            
                            tr_template.attr('style', 'cursor:pointer;color:#333333;background-color:White;border-color:#CEE3F2;border-style:Solid;');
                            $('.rule-list tbody').append(tr_template);             
                            
                            tr_template.parent().find('.hand').css('background-color','white');
                            tr_template.css('background-color','yellow');
                            tr_template.attr('class','hand');
                            
                            tr_template.find('.remove-rule').click(remove_rule);
    
                            tr_template.click(show_rule_item_list);                            
                            
                            $('#panel_add_new_rule').hide();
                        } else {
                            if (d.status == "error") {
                                alert(d.message);
                            }
                        }
                    });
}

function show_rule_item_list() {
    var container = $(this);
    container.parent().find('.hand').css('background-color','white');
    container.css('background-color','yellow');
    
    var json_text = container.find('td:eq(6) div:first').text();
    var rule = eval("("+ json_text +")");
    
    $('#panel_rule_list .rule_item_template').each(function(i){
        if (i != 0) {
            $(this).remove();
        }
    });
    
    $('#txt_description_update').val(rule.Description);
    $('#txt_alert_image_update').val(rule.AlertImage);
    $('#txt_priority_update').val(rule.Priority);
             
    if (rule.Way == 'and') rule.Way = 0;
    if (rule.Way == 'or') rule.Way = 1;    
    $("#select_logicway_update option[value='"+ (rule.Way == 0 ? "and" : "or") +"']").attr("selected",true);
    
    // alert(rule.OPPairs.length);
    for(var i = 0; i < rule.OPPairs.length; i++) {
        var op_pair = rule.OPPairs[i];
        var rule_item = $('#panel_rule_list .rule_item_template:first').clone(true);    
        
        rule_item.show(500);        
                
        rule_item.attr("key", op_pair.Key)
                 .attr("val", op_pair.Value)
                 .attr("operator", op_pair.Operator);

        
        var key_text = $('#panel_add_new_rule select:eq(1)').find('option[value='+op_pair.Key+']').text();
        var operator_text = $('#panel_add_new_rule select:last').find('option[value='+op_pair.Operator+']').text();           
        
        rule_item.find("label:eq(1)").text(key_text);
        rule_item.find("label:eq(3)").text(operator_text);
        rule_item.find("label:eq(5)").text(op_pair.Value);                  
        
        rule_item.insertAfter('#panel_rule_list .rule_item_template:last');
        
        // 移除
        rule_item.find('.panel_rule_list_item_btn_remove').click(function(){    
            $(this).parent().remove();
        });
        // 编辑
        rule_item.find('.panel_rule_list_item_btn_edit').click(function(){
            var rule_item = $(this).parent();
            var key_elem = $(this).parent().find('label:eq(1)');
            var operator_elem = $(this).parent().find('label:eq(3)');
            var val_elem = $(this).parent().find('label:eq(5)');
            
            var key = key_elem.text();
            var operator = operator_elem.text();
            var val = val_elem.text();
                        
            var support_key_ele = $('#panel_add_new_rule select:eq(1)');
            var operator_ele = $('#panel_add_new_rule select:last');
            var val_ele = $('#panel_add_new_rule input:text:last');
            
            key_elem[0].outerHTML = support_key_ele[0].outerHTML;
            operator_elem[0].outerHTML = operator_ele[0].outerHTML;
            val_elem[0].outerHTML = val_ele[0].outerHTML;      
            
            rule_item.find('select:first').val(rule_item.attr('key'));
            rule_item.find('select:last').val(rule_item.attr('operator'));
            rule_item.find('input:text:first').val(rule_item.attr('val'));
            
            rule_item.find('.panel_rule_list_item_btn_edit').hide();
            rule_item.find('.panel_rule_list_item_btn_edit')
                .after('<input class="panel_rule_list_item_btn_update" type="button" value="修改" />');                       
                
            rule_item.find('.panel_rule_list_item_btn_update').click(function(){
                   var parent = $(this).parent();
                var key = parent.find('select:first').val();
                var key_text = parent.find("select:first option:selected").text();
                var operator = parent.find('select:last').val();
                var operator_text = parent.find("select:last option:selected").text();
                var val = parent.find('input:text:first').val();                       
                
                if (val == undefined || val == "") {
                    alert('必须设置预设值！'); return;
                }
                
                parent.attr("key", key)
                      .attr("val", val)
                      .attr("operator", operator);                           
                
                parent.find('select:first')[0].outerHTML = '<label>' + key_text + '</label>';
                parent.find('select:last')[0].outerHTML = '<label>' + operator_text + '</label>';
                parent.find('input:text:first')[0].outerHTML = '<label>' + val + '</label>';
                
                parent.find('.panel_rule_list_item_btn_edit').show();
                $(this).remove();                
            });
        });
        
    }       
    
    $('#panel_rule_list').show();
    var _rule_id = container.find('td:eq(3)').text();
    $('#panel_rule_list').attr('rule_id', _rule_id);
    
    $('#btn_close_rule_item_view_panel').attr('value', '关闭 [' + rule.Description + '] 面板');
    $('#btn_close_rule_item_view_panel').show();
    $('#panel_add_new_rule').hide();
}

function add_rule_item_to_panel_for_update() {
        var container = $('.add_rule_item:last');
        var key = container.find("select:first").val();
        var key_text = container.find("select:first option:selected").text();
        var operator = container.find("select:last").val();
        var operator_text = container.find("select:last option:selected").text();
        var val = container.find("#txtWarningValue").val();
        
        if (val == undefined || val == "") {
            alert("请输入规则值"); return;            
        }
        
        var _rule_item_template = $(".rule_item_template:last").clone(true);
                
        _rule_item_template.attr("key", key)
                           .attr("val", val)
                           .attr("operator", operator);
                           
        _rule_item_template.find("label:eq(1)").text(key_text);
        _rule_item_template.find("label:eq(3)").text(operator_text);
        _rule_item_template.find("label:eq(5)").text(val);        
                
        _rule_item_template.insertAfter(".rule_item_template:last");
        _rule_item_template.show();
}

function add_rule_item_to_panel() {
        var container = $('.add_rule_item:first');
        var key = container.find("select:first").val();
        var key_text = container.find("select:first option:selected").text();
        var operator = container.find("select:last").val();
        var operator_text = container.find("select:last option:selected").text();
        var val = container.find("#txtWarningValue").val();
        
        if (val == undefined || val == "") {
            alert("请输入规则值"); return;            
        }
        
        var _rule_item_template = $(".add_rule_list_item:last").clone(true);
                
        _rule_item_template.attr("key", key)
                           .attr("val", val)
                           .attr("operator", operator);
                           
        _rule_item_template.find("label:eq(1)").text(key_text);
        _rule_item_template.find("label:eq(3)").text(operator_text);
        _rule_item_template.find("label:eq(5)").text(val);        
                
        _rule_item_template.insertAfter(".add_rule_list_item:last");
        _rule_item_template.show();
}

// create rule object.
function create_rule() {
    return { RuleId:"",    
             ResourceId:"", 
             Way:"",
             AlertImage:"", 
             Priority:"", 
             Description:"", 
             OPPairs:[],
             CTIME:""};
}

// create op_pair object.
function create_op_pair() {
    return { Key:"", Value:"", Operator:"" };
}