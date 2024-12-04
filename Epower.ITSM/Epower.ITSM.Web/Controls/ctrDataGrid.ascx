<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrDataGrid.ascx.cs"
    Inherits="Epower.ITSM.Web.Controls.ctrDataGrid" %>
<!--Title: 查询列表控件-->
<asp:DataGrid ID="dgDataList" runat="server" Width="100%" AutoGenerateColumns="False"
    CssClass="Gridtable" OnItemCreated="dgDataList_ItemCreated" OnDeleteCommand="dgDataList_DeleteCommand"
    OnItemDataBound="dgDataList_ItemDataBound">
    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
    <ItemStyle CssClass="tablebody" BackColor="White" HorizontalAlign="Left"></ItemStyle>
    <HeaderStyle CssClass="listTitle" HorizontalAlign="Left"></HeaderStyle>
</asp:DataGrid>
