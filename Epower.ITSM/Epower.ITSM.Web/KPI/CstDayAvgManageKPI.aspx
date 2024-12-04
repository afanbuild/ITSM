<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CstDayAvgManageKPI.aspx.cs" Inherits="Epower.ITSM.Web.KPI.CstDayAvgManageKPI" Title="事件日均产率KPI" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="Table1" width="98%" cellpadding="2" cellspacing="0" class="listContent">
        <tr style="display:none">
            <td class="listTitleRight" width="12%">
                统计类别:
            </td>
            <td class="list">
                <asp:DropDownList ID="drType" runat="server" Width="152px">
                    <asp:ListItem Value="0">按月</asp:ListItem>
                    <asp:ListItem Value="1">按年</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td id="tdResultChart" runat="server" class="list" colspan="2">
                <div id="ReportDiv" runat="server">
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
