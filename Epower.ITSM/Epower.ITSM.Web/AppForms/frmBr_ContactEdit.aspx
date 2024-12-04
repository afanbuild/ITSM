<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmBr_ContactEdit.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmBr_ContactEdit" %>

<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <style type="text/css">
   
        
        #table1 td
        {
        	border: 1px solid #CEE3F2;
        }
    </style>

    <table id="table1" style='width: 98%' cellpadding="0" cellspacing="0" class='listContent'>
        <tr style="display: none;">
            <td class="listTitleRight" style='width: 12%;'>
                单位简称
            </td>
            <td class='list' colspan="3">
                <input runat="server" id="hidServic" type="hidden" />
                <input runat="server" id="hidServicName" type="hidden" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                联系人
            </td>
            <td class='list'>
                <asp:TextBox ID='txtContactName' runat='server' MaxLength="50"></asp:TextBox><font
                    color="#ff6666">*</font>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtContactName"
                    ErrorMessage="联系人不能为空！" SetFocusOnError="true">*</asp:RequiredFieldValidator>
            </td>
            <td class="listTitleRight">
                职位
            </td>
            <td class='list'>
                <asp:TextBox ID='txtCPositionName' runat='server' MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                性别
            </td>
            <td class='list'>
                <asp:DropDownList ID="ddltSex" runat="server" Width="152px">
                    <asp:ListItem Value="0" Selected="true">男</asp:ListItem>
                    <asp:ListItem Value="1">女</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="listTitleRight">
                生日
            </td>
            <td class='list'>
                <uc1:ctrdateandtime ID="dtBirthday" runat="server" ShowTime="false" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                兴趣爱好
            </td>
            <td class='list' colspan="3">
                <asp:TextBox ID='txtInterest' runat='server' MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                办公电话
            </td>
            <td class='list'>
                <asp:TextBox ID='txtOfficeTel' runat='server' MaxLength="50"></asp:TextBox>
            </td>
            <td class="listTitleRight">
                家庭电话
            </td>
            <td class='list'>
                <asp:TextBox ID='txtFamilyTel' runat='server' MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                手机
            </td>
            <td class='list'>
                <asp:TextBox ID='txtMobil' runat='server' MaxLength="50"></asp:TextBox>
            </td>
            <td class="listTitleRight">
                EMAIL
            </td>
            <td class='list'>
                <asp:TextBox ID='txtEMail' runat='server' MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                FAX
            </td>
            <td class='list'>
                <asp:TextBox ID='txtFax' runat='server' MaxLength="50"></asp:TextBox>
            </td>
            <td class="listTitleRight">
                QQ
            </td>
            <td class='list'>
                <asp:TextBox ID='txtQQ' runat='server' MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                MSN
            </td>
            <td class='list'>
                <asp:TextBox ID='txtMsn' runat='server' MaxLength="50"></asp:TextBox>
            </td>
            <td class="listTitleRight">
                邮政编码
            </td>
            <td class='list'>
                <asp:TextBox ID='txtPostNo' runat='server' MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                地址
            </td>
            <td class='list' colspan="3">
                <asp:TextBox ID='txtAddress' runat='server' Width="85%" MaxLength="200"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                备注
            </td>
            <td class='list' colspan="3">
                <asp:TextBox ID='txtRemark' runat='server' Width="85%" Rows="3" TextMode="MultiLine"
                    onblur="MaxLength(this,200,'备注长度超出限定长度:');"></asp:TextBox>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
</asp:Content>
