<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.ctrcatalogtreeNew" Codebehind="ctrcatalogtreeNew.ascx.cs" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<iewc:TreeView id="tvCatalog" runat="server" onclick="TreeNode_Click()" SelectExpands="True" EnableViewState="False"></iewc:TreeView>
