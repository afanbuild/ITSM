<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.CtrRight" Codebehind="CtrRight.ascx.cs" %>
<table>
	<tr id="trCan">
	    <td id="tdRead"><asp:CheckBox id="chkCanRead" runat="server" Text="�ɶ�" onclick='CanRead_Click(this)'></asp:CheckBox></td>
		<td nowrap id="tdAdd">
			<asp:CheckBox id="chkCanAdd" runat="server" Text="�����"></asp:CheckBox>
			<asp:CheckBox id="chkCanModify" runat="server" Text="���޸�"></asp:CheckBox>
			<asp:CheckBox id="chkCanDelete" runat="server" Text="��ɾ��"></asp:CheckBox>
		</td>
	</tr>
</table>