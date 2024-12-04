﻿<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ChangeTimeAvgKPI.aspx.cs" Inherits="Epower.ITSM.Web.KPI.ChangeTimeAvgKPI" Title="变更处理平均时间KPI" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="Table1" width="98%" cellpadding="2" cellspacing="0" class="listContent">
        <tr>
            <td class="listTitleRight" width="12%">
                统计类别:
            </td>
            <td class="list">
                <asp:DropDownList ID="drType" runat="server" Width="152px" 
                    onselectedindexchanged="drType_SelectedIndexChanged" AutoPostBack="true">
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