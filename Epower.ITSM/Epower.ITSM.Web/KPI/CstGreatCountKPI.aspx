﻿<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="CstGreatCountKPI.aspx.cs" Inherits="Epower.ITSM.Web.KPI.CstGreatCountKPI"
    Title="重大事件次数KPI" %>

<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc6" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="Table1" width="98%" cellpadding="2" cellspacing="0" class="listContent">
        <tr>
            <td class="listTitleRight" width="12%">
                统计类别:
            </td>
            <td class="list" width="35%">
                <asp:DropDownList ID="drType" runat="server" Width="152px" onselectedindexchanged="drType_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">按月</asp:ListItem>
                    <asp:ListItem Value="1">按年</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="listTitleRight" width="12%">
                <asp:Literal ID="LitEffectName" runat="server" Text="影响度"></asp:Literal>
            </td>
            <td class="list">
                <uc6:ctrflowcatadroplistnew id="CtrFCDEffect" runat="server" rootid="1023" ShowType="2" />
            </td>
        </tr>
        <tr>
            <td id="tdResultChart" runat="server" class="list" colspan="4">
                <div id="ReportDiv" runat="server">
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
