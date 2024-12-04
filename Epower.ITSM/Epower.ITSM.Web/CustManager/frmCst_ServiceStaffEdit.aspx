<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="frmCst_ServiceStaffEdit.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmCst_ServiceStaffEdit"
    Title="工程师编辑" %>

<%@ Register Src="../Controls/CtrFlowRemark.ascx" TagName="CtrFlowRemark" TagPrefix="uc3" %>
<%@ Register Src="../Controls/CtrFlowNumeric.ascx" TagName="CtrFlowNumeric" TagPrefix="uc4" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc1" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table style='width: 98%' class='listContent Gridtable' cellpadding="2" cellspacing="0">
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                服务单位
            </td>
            <td class='list'>
                <asp:DropDownList ID="ddltMastCustID" runat="server" Width="152px">
                </asp:DropDownList>
                <%--<font color="#ff6666">*</font>--%>
                <span style="color: Red; font-size: Small">*</span>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddltMastCustID"
                    ErrorMessage="服务单位不能为空！" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr id="ShowRefUser" runat="server">
            <td class="listTitleRight">
                对应用户
            </td>
            <td class="list">
                <uc1:UserPicker ID="RefUser" runat="server" MustInput="false" TextToolTip="对应用户">
                </uc1:UserPicker>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                工程师姓名
            </td>
            <td class='list'>

                <uc5:CtrFlowFormText ID="CtrFlowtxtName" runat="server" MustInput="true" TextToolTip="工程师姓名" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                入职时间
            </td>
            <td class='list'>
                <uc2:ctrdateandtime ID="CtrdateJoin" runat="server" ShowTime="false" />
            </td>
        </tr>
        <tr style="display: none;">
            <td class='listTitleRight'>
                排序
            </td>
            <td class='list'>
                <uc4:CtrFlowNumeric ID="CtrNumOrder" runat="server" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                休息类型
            </td>
            <td class='list'>
                <asp:DropDownList ID="ddRestType" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                擅长能力
            </td>
            <td class='list'>
                <ftb:FreeTextBox ID="ftxtFaculty" runat="server" ButtonPath="../Forms/images/epower/officexp/"
                    Height="300px" ImageGalleryPath="Attfiles\\Photos" Width="100%">
                </ftb:FreeTextBox>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                备注
            </td>
            <td class='list'>
                &nbsp;<uc3:CtrFlowRemark ID="CtrRemark" runat="server" />
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
</asp:Content>
