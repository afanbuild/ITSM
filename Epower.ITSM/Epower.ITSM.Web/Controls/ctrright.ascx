<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.CtrRight" Codebehind="CtrRight.ascx.cs" %>
<table>
	<tr id="trCan">
	    <td id="tdRead"><asp:CheckBox id="chkCanRead" runat="server" Text="可读" onclick='CanRead_Click(this)'></asp:CheckBox></td>
		<td nowrap id="tdAdd">
			<asp:CheckBox id="chkCanAdd" runat="server" Text="可添加"></asp:CheckBox>
			<asp:CheckBox id="chkCanModify" runat="server" Text="可修改"></asp:CheckBox>
			<asp:CheckBox id="chkCanDelete" runat="server" Text="可删除"></asp:CheckBox>
		</td>
	</tr>
</table>