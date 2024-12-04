<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="CstDownRateKPI.aspx.cs" Inherits="Epower.ITSM.Web.KPI.CstDownRateKPI"
    Title="事件降低率KPI" %>

<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc6" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="Table1" width="98%" cellpadding="2" cellspacing="0" class="listContent">        
        <tr>
            <td id="tdResultChart" runat="server" class="list">
                <div id="ReportDiv" runat="server">
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
