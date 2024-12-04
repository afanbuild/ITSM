/*     
 * Date: 2012-8-13 11:00
 * summary: 后台管理页, 左侧边栏[submenu.aspx].
 * modified: sunshaozong@gmail.com     
 */        
$(document).ready(function(){
    var _td = $('a[title="所属专业管理"]:first')
        .parent()
        .prev();
    /* 消除[所属专业管理]节点项左边的 "+" 图标.  */
    _td.find('img:first')
       .attr('src', "../images/fix_sa_space.gif");
});