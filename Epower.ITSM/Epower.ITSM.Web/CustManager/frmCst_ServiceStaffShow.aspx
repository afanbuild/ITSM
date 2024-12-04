<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmCst_ServiceStaffShow.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmCst_ServiceStaffShow"
    Title="工程师" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style='width: 98%' class='listContent' cellpadding="2" cellspacing="0">
        <tr>
            <td class="listTitleRight" style='width: 12%;'>
                服务单位
            </td>
            <td class='list'>
                <asp:Label ID="lblBlongDeptName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                对应用户
            </td>
            <td class="list">
                <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                工程师姓名
            </td>
            <td class='list'>
                <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                入职时间
            </td>
            <td class='list'>
                <asp:Label ID="lblJoinDate" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                擅长能力
            </td>
            <td class='list'>
                <asp:Label ID="lblFaculty" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                备注
            </td>
            <td class='list'>
                <asp:Label ID="lblRemark" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
