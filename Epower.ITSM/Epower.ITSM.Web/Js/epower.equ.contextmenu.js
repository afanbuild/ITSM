/*     
 * Date: 2012-8-6 18:34
 * summary: 资产关联图的右键菜单. 有[编辑]和[删除]两个菜单项. 操作之前
 * 检测是否有权限. 否则, 提示拒绝.
 * modified: sunshaozong@gmail.com     
 */        
$(document).ready(function(){
    /* 绑定右键菜单 */     
   if (!epower.equ.read_only) {     
       $(function(){
             $.contextMenu({
                selector: '.context-menu-one', 
                callback: function(key, options) {                                
                    var isOk = context_checkright(this, options);                                
                    if (!isOk) return;
                    
                    if (key == 'edit') {                                    
                        context_edit_func(this, options);
                    } else if (key == 'delete') {
                        context_delete_func(this, options);
                    }
                },
                items: {
                    "edit": {name: "编辑关联资产", icon: "edit"},
                    "delete": {name: "删除此关联", icon: "cut"}
                }
            });
            
            $('.context-menu-one').on('click', function(e){
//                console.log('clicked', this);
            })
         });     
     }
});

// 右键编辑资产
function context_edit_func(obj, options) {
    var node_id = $(obj).attr('id');
        
    //获取资产ID
    var objEquID = node_id.replace("EquTD","").substring(0,node_id.replace("EquTD","").indexOf("-")); 
        
    /* 若点击图像. */    
    if (!objEquID) {
        objEquID = node_id.substring(5);
    }                
    
    var url = "frmEquRelMain.aspx?IsChartAdd=true&ID=" + objEquID;        
        
    epower.tools.open(url);                                                           
}
// 右键删除资产
function context_delete_func(obj, options) {
    var node_rel_id = $(obj).attr('RelID');                                          
    if (node_rel_id == '0') {
        alert('根资产不能删除！');
        return;
    }
    
    if (confirm('您确认删除吗?')) {
        var url = "../Ajax/HttpAjax.ashx?Type=DelEquRel";
        $.get(url, {relid: node_rel_id}, function(data){
            window.location.reload();
        });        
    }
}
// 检查右键菜单操作权限.
function context_checkright(obj, options) {
    var result = false;
    var node_id = $(obj).attr('id');
    
    var objEquID = node_id.replace("EquTD","").substring(0,node_id.replace("EquTD","").indexOf("-")); //获取资产ID
    if (!objEquID) {
        objEquID = node_id.substring(5);
    }                
        
    //判断是否有关联权限                
    var pars = "act=EquRel&EquID="+objEquID;
    $.ajax({
            type: "post",
            data:pars,
            async:false,
            url: "../Forms/Handler.ashx",
            success: function(data, textStatus){                        
                //alert(data);                        
                var json = eval("(" + data + ")");                 
                result = json.record;                      
            }
         });
        
    if(!result)
    {
        alert("您没有修改此资产的关联权限!");
        return false;
    }
    
    return true;
}