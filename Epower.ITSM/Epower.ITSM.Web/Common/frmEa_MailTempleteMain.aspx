<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmEa_MailTempleteMain.aspx.cs" Inherits="Epower.ITSM.Web.Common.frmEa_MailTempleteMain" Title="无标题页" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table cellpadding='1' cellspacing='2' width='100%' border='0' class='listContent'>
<tr>
	<td class='listTitle'  align='right' style='width:15%;'>
        邮件标题&nbsp;</td>		
    <td class="list" colspan="3">
	<asp:TextBox ID='txtMailTitle' runat='server' Width="496px"></asp:TextBox>			&nbsp;</td>
</tr>
</table>
<br />
<table cellpadding="0"  cellspacing="0" width="100%" align="center" border="0">
	<tr>
		<td align="center"  class="listContent">
			<asp:datagrid id="dgEa_MailTemplete" runat="server" Width="100%" cellpadding="1" cellspacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgEa_MailTemplete_ItemCommand" OnItemCreated="dgEa_MailTemplete_ItemCreated">
				<AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			    <HeaderStyle CssClass="listTitle"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn>
						<ItemTemplate>
							<asp:CheckBox id="chkDel" runat="server"></asp:CheckBox>
						</ItemTemplate>
						<HeaderStyle Width="5%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
					</asp:TemplateColumn>
					<asp:BoundColumn DataField='id' HeaderText='id' Visible="False"></asp:BoundColumn>
					<asp:BoundColumn DataField='MailTitle' HeaderText='标题'></asp:BoundColumn>
					<asp:TemplateColumn>
						<ItemTemplate>
							<asp:Button ID="lnkedit" CssClass="btnClass" runat="server" Text="修改" CommandName="edit" />
						</ItemTemplate>
						<HeaderStyle Width="5%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<ItemTemplate>
							<asp:Button ID="lnkSendMail" CssClass="btnClass" runat="server" Text="发送邮件" CommandName="SendMail" />
						</ItemTemplate>
						<HeaderStyle Width="5%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
					</asp:TemplateColumn>
				   </Columns>
				</asp:datagrid>
			</td>
	</tr>
	<tr>
	    <td align="right" class="listTitle"><uc1:ControlPage id="ControlPage1" runat="server"></uc1:ControlPage></td>
	</tr>
</table>
</asp:Content>