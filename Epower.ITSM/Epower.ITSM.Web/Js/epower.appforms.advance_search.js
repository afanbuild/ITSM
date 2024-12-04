/*     
 * Date: 2012-8-6 18:28
 * summary: 事件, 问题, 变更的高级搜索的页面脚本. **需要注意的是: 在选择显示字段时,
 * 还将显示字段的值放入到了表单隐藏域中. 而服务器正是通过后者获取选择的显示字段的.
 * modified: sunshaozong@gmail.com     
 */          
$(document).ready(function(){
    var container_div = $('#_DisplayColumnDiv');
    /* 打开字段列表 */
    $('#btn_columns').click(function(){           
           var _display = container_div.css('display'),
               _width = '700', _height = '500';
           if (_display == 'block') {
               this.value = '显示字段';               
               container_div.hide();               
           } else if (_display == 'none') {
               this.value = '隐藏字段';               
               container_div.show();
               _width = '1500';
           }
                       
           epower.tools.resize(_width, _height, window);           
           setTimeout('epower.tools.reposition("c", window)',100);
    });

    // ########################
    // #### 左右列表框互选 ####
    // ########################
    
    // ## 选择字段
    container_div.find('input:first').click(function(){        
        var _left_select = container_div.find('select:first');
        if (!_left_select.val()){ alert('请选择需要的字段名！'); return;}
        
        var _left_select_opt = _left_select.find('option:selected'),
            _right_select = container_div.find('select:last');
            _left_len = _left_select_opt.size(),
            _right_len = _right_select.find('option').size();
            
        if (_left_len > 7 
        || _right_len > 7
        || (_left_len + _right_len) > 7) { alert("最多能选择显示7个字段,你选择超过7个字段"); return;}        
        
        var _node, _len;        
        for(var i=0; i < _left_select_opt.size(); i++){
            _node = _left_select_opt.eq(i);
            _len = _right_select.find('option[value="' + _node.val().trim() + '"]').size();            
            if (_len > 0) { continue; }            
            _right_select.append(_node.clone());
        }
        
        change_hidden_values(_right_select);
    });
    // ## 移除选择的字段
    container_div.find('input:eq(1)').click(function(){
        var _right_select = container_div.find('select:last');
        _right_select.find('option:selected').remove();
        
        change_hidden_values(_right_select);
    });
    // ## 移除所有字段
    container_div.find('input:last').click(function(){
        var yes = confirm('确定要清除所有字段?');
        if (!yes) return;
        
        var _right_select = container_div.find('select:last');
        _right_select.find('option').remove();      
        
        change_hidden_values(_right_select);  
    });
    
  
}); 


//获取相关字段ID串
function change_hidden_values(selectctrl){
    var _options = selectctrl.find('option'), _idarray = '', _namearray ='';    
    
    for(var i = 0; i < _options.size(); i++) {
        _idarray = _idarray + _options.eq(i).val().trim() + ',';
        _namearray = _namearray + _options.eq(i).text().trim() + ',';        
    }
    
    if (_idarray.length == 1) _idarray = '';
    
    try {
        $('#' + epower.advance_search.deptIdCtrlID).val(_idarray);
        $('#' + epower.advance_search.deptNameCtrlID).val(_namearray);
        $('#' + epower.advance_search.hidValueCtrlID).val(_idarray + '@' + _namearray);    
    }catch(e){    
    }
}

                