<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmPlanDetail.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmPlanDetail" Title="计划设置" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table  class="listContent"  style="width: 100%">
        <tr>
            <td  class="listTitle" align="right" style="width: 40%">
                计划类别:</td>
            <td  class="list" align="center">
                <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="10">持续运行</asp:ListItem>
                    <asp:ListItem Value="20" Selected="True">每日执行</asp:ListItem>
                    <asp:ListItem Value="30">每周执行</asp:ListItem>
                    <asp:ListItem Value="40">每月执行</asp:ListItem>
                </asp:RadioButtonList></td>
        </tr>
        <tr>
            <td style="width: 40%" align="right" class="listTitle">
                指定执行时间:</td>
            <td  class="list" align="center">
                <asp:DropDownList ID="ddlHours" runat="server" Width="48px">
                </asp:DropDownList>时<asp:DropDownList ID="ddlMinutes" runat="server" Width="48px">
                </asp:DropDownList>分</td>
        </tr>
        <tr>
            <td align="right" class="listTitle" style="width: 40%">
                指定日期(每月执行):</td>
            <td align="center" class="list">
                第<asp:DropDownList ID="ddlDays" runat="server" Width="48px">
                </asp:DropDownList>天</td>
        </tr>
        <tr>
            <td align="right" class="listTitle" style="width: 40%">
                指定日期(每周执行):</td>
            <td align="center" class="list">
                <asp:DropDownList ID="ddlWeeks" runat="server" Width="64px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right" class="listTitle" style="width: 40%">
                运行时间间隔:</td>
            <td align="center" class="list">
                <asp:TextBox ID="txtInterval" runat="server"  MaxLength="9" style="ime-mode:Disabled" onblur="CheckIsnum(this,'运行时间间隔必须为数值！');" onkeydown="NumberInput('');"></asp:TextBox>小时</td>
        </tr>
        <tr>
            <td style="width: 40%" align="right" class="listTitle">
                运行时间范围:</td>
            <td  class="list" align="center">
                <asp:DropDownList ID="ddlHoursBeg" runat="server" Width="48px">
                </asp:DropDownList>时<asp:DropDownList ID="ddlMinutesBeg" runat="server" Width="48px">
                </asp:DropDownList>分&nbsp; ---- &nbsp;
                <asp:DropDownList ID="ddlHoursEnd" runat="server" Width="48px">
                </asp:DropDownList>时<asp:DropDownList ID="ddlMinutesEnd" runat="server" Width="48px">
                </asp:DropDownList>分</td>
        </tr>
        <tr>
            <td style="width: 40%" align="right" class="listTitle">
                每周运行设置:</td>
            <td  class="list" align="center">
                <asp:CheckBoxList ID="cblWeeks" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0">周日</asp:ListItem>
                    <asp:ListItem Selected="True" Value="1">周一</asp:ListItem>
                    <asp:ListItem Selected="True" Value="2">周二</asp:ListItem>
                    <asp:ListItem Selected="True" Value="3">周三</asp:ListItem>
                    <asp:ListItem Selected="True" Value="4">周四</asp:ListItem>
                    <asp:ListItem Selected="True" Value="5">周五</asp:ListItem>
                    <asp:ListItem Value="6">周六</asp:ListItem>
                </asp:CheckBoxList></td>
        </tr>
    </table>
</asp:Content>
