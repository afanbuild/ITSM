<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True"
    CodeBehind="form_RptMenu.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.form_RptMenu"
    Title="年度工作绩效分析" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="Table1" width="98%" cellpadding="2" cellspacing="0" class="listContent">
        <tr>
            <td class="listTitleRight" width="12%">
                选择年度:
            </td>
            <td class="list" colspan="3">
                <asp:DropDownList ID="dpdYear" runat="server" Width="152px" AutoPostBack="True" OnSelectedIndexChanged="dpdYear_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td id="tdResultChart" runat="server" class="list" colspan="4">
                <div id="ReportDiv" runat="server">
                </div>
                <div id="ReportDiv2" runat="server">
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
