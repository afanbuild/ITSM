<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmCst_ServiceStaffShow.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmCst_ServiceStaffShow"
    Title="����ʦ" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style='width: 98%' class='listContent' cellpadding="2" cellspacing="0">
        <tr>
            <td class="listTitleRight" style='width: 12%;'>
                ����λ
            </td>
            <td class='list'>
                <asp:Label ID="lblBlongDeptName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                ��Ӧ�û�
            </td>
            <td class="list">
                <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                ����ʦ����
            </td>
            <td class='list'>
                <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                ��ְʱ��
            </td>
            <td class='list'>
                <asp:Label ID="lblJoinDate" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                �ó�����
            </td>
            <td class='list'>
                <asp:Label ID="lblFaculty" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                ��ע
            </td>
            <td class='list'>
                <asp:Label ID="lblRemark" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
