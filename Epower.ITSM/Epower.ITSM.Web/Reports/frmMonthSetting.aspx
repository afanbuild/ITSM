<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmMonthSetting.aspx.cs" Inherits="Epower.ITSM.Web.Reports.frmMonthSetting"
    Title="月区间设置" %>
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
            <td class="listTitleRight" style="width:12%">
                月区间
            </td>
            <td class="list">
                <asp:DropDownList ID="drbDay" runat="server" Width="152px">
                    <asp:ListItem Value="1">1</asp:ListItem>
                    <asp:ListItem Value="2">2</asp:ListItem>
                    <asp:ListItem Value="3">3</asp:ListItem>
                    <asp:ListItem Value="4">4</asp:ListItem>
                    <asp:ListItem Value="5">5</asp:ListItem>
                    <asp:ListItem Value="6">6</asp:ListItem>
                    <asp:ListItem Value="7">7</asp:ListItem>
                    <asp:ListItem Value="8">8</asp:ListItem>
                    <asp:ListItem Value="9">9</asp:ListItem>
                    <asp:ListItem Value="10">10</asp:ListItem>
                    <asp:ListItem Value="11">11</asp:ListItem>                    
                    <asp:ListItem Value="12">12</asp:ListItem>
                    <asp:ListItem Value="13">13</asp:ListItem>
                    <asp:ListItem Value="14">14</asp:ListItem>
                    <asp:ListItem Value="15">15</asp:ListItem>
                    <asp:ListItem Value="16">16</asp:ListItem>
                    <asp:ListItem Value="17">17</asp:ListItem>
                    <asp:ListItem Value="18">18</asp:ListItem>
                    <asp:ListItem Value="19">19</asp:ListItem>
                    <asp:ListItem Value="20">20</asp:ListItem>
                    <asp:ListItem Value="21">21</asp:ListItem>
                    <asp:ListItem Value="22">22</asp:ListItem>
                    <asp:ListItem Value="23">23</asp:ListItem>
                    <asp:ListItem Value="24">24</asp:ListItem>
                    <asp:ListItem Value="25">25</asp:ListItem>
                    <asp:ListItem Value="26">26</asp:ListItem>
                    <asp:ListItem Value="27">27</asp:ListItem>
                    <asp:ListItem Value="28">28</asp:ListItem>
                    <asp:ListItem Value="29">29</asp:ListItem>
                    <asp:ListItem Value="30">30</asp:ListItem>
                    <asp:ListItem Value="31">31</asp:ListItem>
                </asp:DropDownList>
                <uc4:CtrFlowNumeric ID="CtrBegin" runat="server" MaxLength="2" Visible="false" />
                <uc4:CtrFlowNumeric ID="CtrEnd" runat="server" MaxLength="2" Visible="false" />
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
