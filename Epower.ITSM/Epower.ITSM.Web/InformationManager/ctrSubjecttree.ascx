<%@ Control Language="c#" Inherits="Epower.ITSM.Web.InformationManager.ctrSubjecttree" Codebehind="ctrSubjecttree.ascx.cs" %>

<asp:TreeView ID="TreeView1" runat="server"   >
<SelectedNodeStyle  CssClass="TreeViewSelectedNode"  />
</asp:TreeView>
<style type="text/css">
    .TreeViewSelectedNode{
 background-color:Yellow;
 color:Red;	}
</style>




