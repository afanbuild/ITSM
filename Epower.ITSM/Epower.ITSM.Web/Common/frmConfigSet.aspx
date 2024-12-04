<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmConfigSet.aspx.cs" Inherits="Epower.ITSM.Web.Common.frmConfigSet" Title="无标题页" %>

<%@ Register Src="../Controls/ctrtitle.ascx" TagName="ctrtitle" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 100%">
        <tr>
            <td align="center" style="width: 100%">
                <uc1:ctrtitle ID="Ctrtitle1" runat="server" />
            </td>
        </tr>
        <tr>
            <td id="tdDyTable" align="center" runat=server style="width: 100%">
            </td>
        </tr>
    </table>
</asp:Content>
