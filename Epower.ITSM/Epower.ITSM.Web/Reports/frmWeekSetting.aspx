<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="frmWeekSetting.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmWeekSetting"
    Title="报表周区间设置" %>

<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc1" %>
<%@ Register Src="../Controls/CtrFlowNumeric.ascx" TagName="CtrFlowNumeric" TagPrefix="uc4" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style='width: 98%' class='listContent'>
        <tr runat="server" visible="false">
            <td class='listTitleRight' style='width: 12%;'>
               区间有效时间
            </td>
            <td class='list'>
                <uc1:ctrdateandtime ID="CtrDateBegin" runat="server" ShowTime="false" TextToolTip="开始时间"
                    MustInput="false" />
                ~
                <uc1:ctrdateandtime ID="CtrDateEnd" runat="server" ShowTime="false" TextToolTip="结束时间"
                    MustInput="false" />
            </td>
        </tr>
        <tr id="ShowRefUser" runat="server">
            <td class="listTitleRight" style='width: 12%;'>
                周区间
            </td>
            <td class="list">
                <asp:DropDownList ID="drbDay" runat="server" Width="152px">
                    <asp:ListItem Value="1">星期一</asp:ListItem>
                    <asp:ListItem Value="2">星期二</asp:ListItem>
                    <asp:ListItem Value="3">星期三</asp:ListItem>
                    <asp:ListItem Value="4">星期四</asp:ListItem>
                    <asp:ListItem Value="5">星期五</asp:ListItem>
                    <asp:ListItem Value="6">星期六</asp:ListItem>
                    <asp:ListItem Value="7">星期日</asp:ListItem>
                </asp:DropDownList>
                <uc4:CtrFlowNumeric ID="CtrBegin" TextToolTip="周区间" MustInput="true" runat="server"
                    MaxLength="1" Visible="false" />
                <uc4:CtrFlowNumeric ID="CtrEnd" runat="server" Visible="false" MaxLength="1" />
            </td>
        </tr>
    </table>
    <table border="0" width="98%" cellpadding="0" cellspacing="0">
        <tr>
            <td align="left">
                <asp:Label ID="lblSetting" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
</asp:Content>
