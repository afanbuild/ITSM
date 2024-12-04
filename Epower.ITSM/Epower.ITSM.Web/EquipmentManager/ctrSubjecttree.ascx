<%@ Control Language="c#" Inherits="Epower.ITSM.Web.EquipmentManager.ctrSubjecttree" Codebehind="ctrSubjecttree.ascx.cs" %>

<asp:TreeView ID="tvSubject" runat="server" onclick="TreeNode_Click()" ExpandDepth ="3"   ForeColor="Red"  >
<SelectedNodeStyle CssClass="TreeViewSelectedNode"  />

</asp:TreeView>
<style type="text/css">
    .TreeViewSelectedNode
    {
    	color:Red;
    	background-color:Yellow;
    	}
</style>

