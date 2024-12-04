<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmEqu_DeskShot.aspx.cs"
    EnableViewState="false" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_DeskShot" %>

<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/DymSchemeCtr.ascx" TagName="DymSchemeCtr" TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrEquCataDropList.ascx" TagName="ctrEquCataDropList"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<%@ Register Src="../Controls/BussinessControls/CustomCtr.ascx" TagName="CustomCtr"
    TagPrefix="uc5" %>
<%@ Register Src="../Controls/DymSchemeCtrList.ascx" TagName="DymSchemeCtrList" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <table style="width: 100%" class="listContent" runat="server" id="tabRender">
        <tr id="ShowVersion" runat="server">
            <td align="right" class="listTitle" style="width: 12%;">
                版本
            </td>
            <td class="list" colspan="3" style="">
                <asp:Label ID="lblVersion" runat="server"></asp:Label>
            </td>
        </tr>
        <tr style="display: none;">
            <td class='listTitle' align='right' >
                <asp:Literal ID="LitListName" runat="server" Text="资产目录"></asp:Literal>
            </td>
            <td class="list"  colspan="3">
                <asp:Label ID="lblListName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class='listTitle' align='right' style="width: 12%;">
                <asp:Literal ID="LitEquDeskName" runat="server" Text="名称"></asp:Literal>
            </td>
            <td class="list" style="width: 35%;">
                <asp:Label ID="lblEquDeskName" runat="server"></asp:Label>
            </td>
            <td class='listTitle' align='right' style="width: 12%;">
                <asp:Literal ID="LitEquDeskCode" runat="server" Text="资产编号"></asp:Literal>
            </td>
            <td class='list'>
                <asp:Label ID="lblCode" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class='listTitle' align='right' style='width: 12%;'>
                <asp:Literal ID="LitMastCust" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class='list' style='width: 32%;'>
                <asp:Label ID="lblMastCust" runat="server" Text=""></asp:Label>
            </td>
            <td class='listTitle' align='right' style='width: 12%;'>
                <asp:Literal ID="LitCustom" runat="server" Text="所属客户"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCustom" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class='listTitle' align='right' style='width: 12%;'>
                <asp:Literal ID="LitEquDeskEquStatus" runat="server" Text="资产状态"></asp:Literal>
            </td>
            <td class="list" style='width: 32%;'>
                <asp:Label ID="lblEquStatus" runat="server"></asp:Label>
            </td>
            <td align="right" class="listTitle" style="width: 12%">
                <asp:Literal ID="LitPartBankName" runat="server" Text="维护机构"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblPartBankName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class='listTitle' align='right' style='width: 12%;'>
                <asp:Literal ID="LitPartBranchName" Text="维护部门" runat="server"></asp:Literal>
            </td>
            <td class='list'>
                <asp:Label ID="lblPartBranchName" runat="server" Text=""></asp:Label>
            </td>
            <td class='listTitle' align='right' style='width: 12%;'>
                <asp:Literal ID="LitEquDeskServiceTime" runat="server" Text="保修期"></asp:Literal>
            </td>
            <td class='list'>
                <asp:Label ID="lblServiceBeginTime" runat="server" Text=""></asp:Label>~
                <asp:Label ID="lblServiceEndTime" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" class="listTitle" colspan="6">
                <uc1:DymSchemeCtrList ID="DymSchemeCtrList1" runat="server" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
