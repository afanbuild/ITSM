<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEa_DefineMainPageEdit.aspx.cs" Inherits="Epower.ITSM.Web.mydestop.frmEa_DefineMainPageEdit"
    Title="" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style='width: 98%' class='listContent'>
        <tr>
            <td class='listTitleRight ' style="width: 12%">
                标题
            </td>
            <td class='list'>
                <asp:TextBox ID='txtTitle' runat='server' Width="380px" MaxLength="200"></asp:TextBox><font
                    color="#ff6666">*</font>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle"
                    ErrorMessage="标题不能为空！" SetFocusOnError="True">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight ' style="width: 12%">
                序号
            </td>
            <td class='list'>
                <asp:TextBox ID='txtiOrder' runat='server' MaxLength="9" onkeydown="NumberInput('');"
                    Style="ime-mode: Disabled" onblur="NumberInput('');"></asp:TextBox><font color="#ff6666">*</font>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtiOrder"
                    ErrorMessage="序号不能为空！" SetFocusOnError="True">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight ' style="width: 12%">
                显示高度
            </td>
            <td class='list'>
                <asp:TextBox ID='txtIframeHeight' runat='server' MaxLength="50">200</asp:TextBox><font
                    color="#ff6666">*</font>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtIframeHeight"
                    ErrorMessage="显示高度不能为空！" SetFocusOnError="True">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight ' style="width: 12%">
                默认位置
            </td>
            <td class='list'>
                <asp:RadioButtonList ID="rdbtLeftOrRight" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="0">左边</asp:ListItem>
                    <asp:ListItem Value="1">右边</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight ' style="width: 12%">
                默认显示
            </td>
            <td class='list'>
                <asp:RadioButtonList ID="rdbtShow" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0">显示</asp:ListItem>
                    <asp:ListItem Selected="True" Value="1">不显示</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight ' style="width: 12%">
                标题图标
            </td>
            <td class='list'>
                <asp:TextBox ID='txtImageUrl' runat='server' Width="380px" MaxLength="200">Images/dot3.gif</asp:TextBox><font
                    color="#ff6666">*</font>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtImageUrl"
                    ErrorMessage="标题图标不能为空！" SetFocusOnError="True">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight ' style="width: 12%">
                显示Url
            </td>
            <td class='list'>
                <asp:TextBox ID='txtUrl' runat='server' Width="380px" MaxLength="200"></asp:TextBox><font
                    color="#ff6666">*</font>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtUrl"
                    ErrorMessage="显示Url不能为空！" SetFocusOnError="True">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight ' style="width: 12%">
                更多Url
            </td>
            <td class='list'>
                <asp:TextBox ID='txtMoreUrl' runat='server' Width="380px" MaxLength="200"></asp:TextBox>默认为#
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
</asp:Content>
