<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.CtrDeptTree" Codebehind="CtrDeptTree.ascx.cs" %>
	<asp:TreeView ID="tvDept"  	 
	runat="server" onclick="TreeNode_Click()" ForeColor="Black" Font-Size="8pt"  >
	    <SelectedNodeStyle CssClass="TreeViewSelectedNode"  />
</asp:TreeView>

	<style type="text/css">
    .TreeViewSelectedNode{
 background-color:Yellow;
 color:Red;	}
</style>

	

