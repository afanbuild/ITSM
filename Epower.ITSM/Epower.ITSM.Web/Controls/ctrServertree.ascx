<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.ctrServertree" Codebehind="ctrServertree.ascx.cs" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<asp:TreeView 
    ID="tvSubject" runat="server"
    OnSelectedNodeChanged="tvSubject_SelectedNodeChanged" 
    OnTreeNodeExpanded="tvSubject_TreeNodeExpanded"
      >
    <SelectedNodeStyle BackColor="Yellow" ForeColor="Red"  />
</asp:TreeView>


<!--Begin: 单击部门时可以自动收缩或展开下级部门节点 - 2013-06-05 @孙绍棕 -->
<script type="text/javascript" language="javascript">
    
$('#<%=tvSubject.ClientID %> .<%=tvSubject.ClientID %>_0').each(function(){	
	var a = $(this).parent().prevAll('td:eq(1)').find('a');		
	var href = a.attr('href');	
	if (href) {
	    var href = $(this).attr('href');
	    $(this).attr('href', href.replace('s-1','t-1'));
	}	
});

</script>
<!--End.-->


