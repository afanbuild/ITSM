<%@ Control Language="c#" AutoEventWireup="true"  Inherits="Epower.ITSM.Web.DeptControls.CtrDeptTree" Codebehind="CtrDeptTree.ascx.cs" %>


<asp:TreeView ID="tvDept" runat="server"  
    EnableViewState="true" onselectednodechanged="tvDept_SelectedNodeChanged" 
    ontreenodeexpanded="tvDept_TreeNodeExpanded">
    <SelectedNodeStyle CssClass="TreeViewSelectedNode"  />
</asp:TreeView>

<style type="text/css">
    .TreeViewSelectedNode{
 background-color:Yellow;
 color:Red;	}
</style>


<!--Begin: 单击部门时可以自动收缩或展开下级部门节点 - 2013-06-04 @孙绍棕 -->
<script type="text/javascript" language="javascript">
    
$('#<%=tvDept.ClientID %> .<%=tvDept.ClientID %>_0').each(function(){	
	var a = $(this).parent().prevAll('td:eq(1)').find('a');		
	var href = a.attr('href');	
	if (href) {
	    var href = $(this).attr('href');
	    $(this).attr('href', href.replace('s1','t1'));
	}	
});

</script>
<!--End.-->

