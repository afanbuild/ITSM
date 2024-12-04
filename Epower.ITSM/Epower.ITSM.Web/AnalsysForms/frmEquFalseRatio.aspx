<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    CodeBehind="frmEquFalseRatio.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmEquFalseRatio"
    Title="资产故障率" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript" src="../Controls/Calendar/Popup.js"></script>

    <script language="javascript" type="text/javascript" src="../Js/App_Common.js"> </script>
    <script language="javascript" type="text/javascript">
        function btnClick()
        {
            document.getElementById("<%=btnSelect.ClientID %>").click();
        }    
    </script>
    
    <div style="display:none;">
        <asp:Button ID="btnSelect" Width="0px" runat="server" OnClick="btnSelect_Click" />
    </div>
    
    <table id="tblTitle" width="98%" cellpadding="2" cellspacing="0" class="listContent" runat="server">
        <tr>
            <td width="12%" class='listTitleRight'>
                时间范围:
            </td>
            <td class="list">
                <uc5:ctrDateSelectTime ID="ctrDateTime" runat="server" OnChangeScript="btnClick()" />
            </td>
            <td width="12%" class='listTitleRight'>
                <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="dpdMastShortName" runat="server" AutoPostBack="True" Width="152px"
                    OnSelectedIndexChanged="dpdMastShortName_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="98%" class="listContent">
        <tr id="tr2" runat="server">
            <td id="tdResultChart" runat="server" align="left" class="list">
                <div id="ReportDiv" runat="server">
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
