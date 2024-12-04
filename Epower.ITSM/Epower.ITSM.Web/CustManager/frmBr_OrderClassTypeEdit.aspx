<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmBr_OrderClassTypeEdit.aspx.cs" Inherits="Epower.ITSM.Web.CustManager.frmBr_OrderClassTypeEdit"
    Title="班次编辑" %>

<%@ Register TagPrefix="uc2" TagName="ctrdateandtime" Src="../Controls/CtrDateAndTimeV2.ascx" %>
<%@ Register TagPrefix="uc3" TagName="CtrFlowFormText" Src="../Controls/CtrFlowFormText.ascx" %>
<%@ Register TagPrefix="uc4" TagName="ctrFlowCataDropList" Src="../Controls/ctrFlowCataDropList.ascx" %>
<%@ Register TagPrefix="uc5" TagName="CtrFlowNumeric" Src="../Controls/CtrFlowNumeric.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/CtrFlowRemark.ascx" TagName="CtrFlowRemark" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style='width: 98%' class='listContent'>
        <tr>
            <td class='listTitleRight' align='right' style='width: 12%;'>
                班次名称
            </td>
            <td class='list'>
                <uc3:CtrFlowFormText ID='CtrFlowClassTypeName' runat='server' MustInput='true' MaxLength="50"
                    TextToolTip="班次名称" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight' align='right' style='width: 12%;'>
                班次说明
            </td>
            <td class='list'>
                <uc1:CtrFlowRemark ID="CtrFlowRemark" runat="server" MustInput="true" MaxLength="1000"
                    TextToolTip="班次说明" />
            </td>
        </tr>
    </table>
</asp:Content>
