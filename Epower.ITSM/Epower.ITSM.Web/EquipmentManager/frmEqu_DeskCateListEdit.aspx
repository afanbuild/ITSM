<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEqu_DeskCateListEdit.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_DeskCateListEdit"
    Title="资产目录编辑" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hidCataLogId" runat="server" Value="0" />
    <table cellpadding='1' cellspacing='2' width='98%' border='0' class='listContent'>
        <tr>
            <td class='listTitle' align='right' style='width: 15%;'>
                资产目录
            </td>
            <td class='list'>
                <uc1:CtrFlowFormText ID="CtrFlowListName" runat="server" MustInput="true" TextToolTip="资产目录" />
            </td>
        </tr>
    </table>
</asp:Content>
