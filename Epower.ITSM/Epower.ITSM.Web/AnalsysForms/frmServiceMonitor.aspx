<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmServiceMonitor.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmServiceMonitor" Title="服务监控" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register src="../Controls/DeptPicker.ascx" tagname="DeptPicker" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="98%" class="listContent" cellpadding="2" cellspacing="0">
        <tr>
            <td class='listTitleRight' style="width:12%">
                <asp:Literal ID="Literal1" runat="server" Text="时间范围"></asp:Literal>
            </td>
            <td class="list" style="width:35%">
                <asp:Literal ID="Literal2" runat="server" Text="当月"></asp:Literal>                
            </td>
            <td class='listTitleRight' style="width:12%">
                <asp:Literal ID="Literal3" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="ddltMastCustID" Width="152px" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="ddltMastCustID_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="98%" class="listContent" cellpadding="2">
        <tr id="tr2" runat="server" align="left">
            <td id="tdResultChart" runat="server" align="center" class="list" style="width:50%">
                <div id="IssueDiv" runat="server"></div>
            </td>
            <td id="td1" runat="server" align="center" class="list">
                <div id="ChangeDiv" runat="server"></div>
            </td>
        </tr>
        <tr id="tr1" runat="server" align="left">
            <td id="td2" runat="server" align="center" class="list" style="width:50%">
                <div id="ProblemDiv" runat="server"></div>
            </td>
            <td id="td3" runat="server" align="center" class="list">
                <div id="EquDiv" runat="server"></div>
            </td>
        </tr>
        <tr id="tr3" runat="server" align="left">
            <td id="td4" runat="server" align="center" class="list" style="width:50%">
                <div id="InfDiv" runat="server"></div>
            </td>
            <td id="td5" runat="server" align="center" class="list">
                <div id="FeedBackDiv" runat="server"></div>
            </td>
        </tr>
    </table>
</asp:Content>
