<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.CtrCatalogTree" Codebehind="CtrCatalogTree.ascx.cs" %>

<asp:TreeView ID="tvCatalog" runat="server" ForeColor="Red"  >
    <SelectedNodeStyle CssClass="TreeViewSelectedNode"  />
</asp:TreeView>
<style>
    .TreeViewSelectedNode{
 background-color:Yellow;
 color:Red;	}
</style>