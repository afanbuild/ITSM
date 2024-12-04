<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmTs_OperatesEdit.aspx.cs" Inherits="Epower.ITSM.Web.DeptForms.frmTs_OperatesEdit"
    Title="无标题页" %>

<%@ Register Src="../Controls/CtrFlowRemark.ascx" TagName="CtrFlowRemark" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="ctrdateandtime" Src="../Controls/CtrDateAndTimeV2.ascx" %>
<%@ Register TagPrefix="uc3" TagName="CtrFlowFormText" Src="../Controls/CtrFlowFormText.ascx" %>
<%@ Register TagPrefix="uc4" TagName="ctrFlowCataDropList" Src="../Controls/ctrFlowCataDropList.ascx" %>
<%@ Register TagPrefix="uc5" TagName="CtrFlowNumeric" Src="../Controls/CtrFlowNumeric.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style='width: 98%' cellpadding="2" cellspacing="0" class='listContent'>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                操作项编号
            </td>
            <td class='list'>
                <uc5:CtrFlowNumeric ID='CtrFlowOperateID' runat='server' MustInput="true" MaxLength="18"
                    TextToolTip="操作项编号" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                操作项名称
            </td>
            <td class='list'>
                <uc3:CtrFlowFormText ID='CtrFlowOpName' runat='server' TextToolTip='操作项名称' MustInput='true'
                    MaxLength="50" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                操作项类别
            </td>
            <td class='list'>
                <uc3:CtrFlowFormText ID='CtrFlowOpCatalog' runat='server' TextToolTip='操作项类别' MustInput='true'
                    MaxLength="50" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                操作项描述
            </td>
            <td class='list' style="height: 91px">
                <uc1:CtrFlowRemark ID="CtrFlowOpDesc" runat="server" MaxLength="250" />
            </td>
        </tr>
    </table>
</asp:Content>
