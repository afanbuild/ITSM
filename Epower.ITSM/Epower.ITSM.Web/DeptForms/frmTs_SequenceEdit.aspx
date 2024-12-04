<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmTs_SequenceEdit.aspx.cs" Inherits="Epower.ITSM.Web.DeptForms.frmTs_SequenceEdit"
    Title="系统系列号编辑" %>

<%@ Register TagPrefix="uc2" TagName="ctrdateandtime" Src="../Controls/CtrDateAndTimeV2.ascx" %>
<%@ Register TagPrefix="uc3" TagName="CtrFlowFormText" Src="../Controls/CtrFlowFormText.ascx" %>
<%@ Register TagPrefix="uc4" TagName="ctrFlowCataDropList" Src="../Controls/ctrFlowCataDropList.ascx" %>
<%@ Register TagPrefix="uc5" TagName="CtrFlowNumeric" Src="../Controls/CtrFlowNumeric.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style='width: 98%' class='listContent'>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                系列号关键字
            </td>
            <td class='list'  style='width: 35%;'>
                <uc3:CtrFlowFormText ID="CtrFlowName" runat="server" MaxLength="18" MustInput="true"
                    TextToolTip="系列号关键字" />
            </td>
            <td class='listTitleRight'  style='width: 12%;'>
                最小值
            </td>
            <td class='list'>
                <uc5:CtrFlowNumeric ID='CtrFlowMinValue' runat='server' MustInput="true" TextToolTip="最小值" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                最大值
            </td>
            <td class='list'>
                <uc5:CtrFlowNumeric ID='CtrFlowMaxValue' runat='server' MustInput="true" TextToolTip="最大值" />
            </td>
            <td class='listTitleRight'>
                当前值
            </td>
            <td class='list'>
                <uc5:CtrFlowNumeric ID='CtrFlowCurrentValue' runat='server' MustInput="true" TextToolTip="当前值" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                步进
            </td>
            <td class='list'>
                <uc5:CtrFlowNumeric ID='CtrFlowStep' runat='server' MustInput="true" TextToolTip="步进" />
            </td>
            <td class='listTitleRight'>
                循环版本
            </td>
            <td class='list'>
                <uc5:CtrFlowNumeric ID='CtrFlowRecycle' runat='server' MustInput="true" TextToolTip="循环版本" />
            </td>
        </tr>
    </table>
</asp:Content>
