<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" validateRequest="false" CodeBehind="frmEa_MailTempleteSend.aspx.cs" Inherits="Epower.ITSM.Web.Common.frmEa_MailTempleteSend" Title="无标题页" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table style='width:100%' class='listContent'>
    <tr>
        <td align="center" class="list" colspan="2">
            <asp:Button ID="btnSend" runat="server" CssClass="btnClass" OnClick="btnSend_Click"
                Text="发  送" />&nbsp;
            <asp:Button ID="Button3" runat="server" CssClass="btnClass" OnClick="btnSend_Click"
                Text="后台发送" />&nbsp;
            <asp:Button ID="Button2" runat="server" CssClass="btnClass" OnClick="btnSend_Click"
                Text="退  出" /></td>
    </tr>
	<tr>
	<td class='listTitle'  align='right' style="width:80; height: 90px;">
        收件人&nbsp;</td>		
<td class='list' style="height: 90px">
            &nbsp;<table style="width: 100%">
                <tr>
                    <td style="width: 572px">
	<asp:TextBox ID='txtRecs' runat='server' Width="572px" Height="112px" TextMode="MultiLine"></asp:TextBox></td>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 100px">
                                    <asp:Button ID="Button1" runat="server" CssClass="btnClass" OnClick="btnSend_Click"
                Text="导入客户邮箱" /></td>
                            </tr>
                           <tr>
                                <td style="width: 100px">
                                    <asp:Button ID="Button5" runat="server" CssClass="btnClass" OnClick="btnSend_Click"
                Text="导入邮址库邮箱" Width="117px" /></td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Button ID="Button4" runat="server" CssClass="btnClass" OnClick="btnSend_Click"
                Text="导入文本文件" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
</td>		
</tr>
    <tr>
        <td align="right" class="listTitle" style="width: 80px">
            账户</td>
        <td class="list">
            <asp:DropDownList ID="DropDownList1" runat="server" Width="347px">
            </asp:DropDownList></td>
    </tr>
	<tr>
	<td class='listTitle'  align='right' style='width:80;'>
        标题&nbsp;</td>		
<td class='list'>
	<asp:TextBox ID='txtMailTitle' runat='server' Width="572px"></asp:TextBox>		
</td>		
</tr>
	<tr>
	<td class='listTitle'  align='right' style='width:80;'>
        内容&nbsp;</td>		
<td class='list'>
	<ftb:freetextbox
        id="txtMailBody" runat="server" buttonpath="../Forms/images/epower/officexp/"
        height="600px" imagegallerypath="Attfiles\\Photos" width="100%"> </ftb:freetextbox></td>		
</tr>
</table>
</asp:Content>
