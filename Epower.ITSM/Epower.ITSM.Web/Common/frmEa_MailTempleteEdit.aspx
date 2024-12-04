<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"　 validateRequest="false" CodeBehind="frmEa_MailTempleteEdit.aspx.cs" Inherits="Epower.ITSM.Web.Common.frmEa_MailTempleteEdit" Title="无标题页" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table style='width:100%' class='listContent'>
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
