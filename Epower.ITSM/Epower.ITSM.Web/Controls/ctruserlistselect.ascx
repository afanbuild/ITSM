<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.CtrUserListSelect" Codebehind="CtrUserListSelect.ascx.cs" %>
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td vAlign="top" rowspan="4" width="50%"><asp:listbox id="lstUsers" Rows="6" Width="100%" runat="server" SelectionMode="Multiple"></asp:listbox></td>
		<TD vAlign="middle" align="center" width="5%">
			<INPUT class="FLOWBUTTON" id="btnAdd" title="->" style="WIDTH: 35px; HEIGHT: 24px" type="button"
				size="20" value="选择" name="btnAdd" runat="server">
		</TD>
		<td vAlign="top" rowspan="4" width="50%"><asp:listbox id="lstSelected" Rows="6" Width="100%" runat="server" SelectionMode="Multiple"></asp:listbox></td>
	</tr>
	<tr>
		<TD vAlign="middle" align="center" width="5%"><font face="宋体"> <input class="FLOWBUTTON" id="btnRemove" title="<-" style="WIDTH: 35px" type="button" size="20"
					value="移除" name="btnRemove" runat="server"> </font>
		</TD>
	</tr>
	<tr>
		<TD vAlign="middle" align="center" width="5%"><input class="FLOWBUTTON" id="btnAddAll" title=">>" style="WIDTH: 35px" type="button" size="30"
				value="全选" name="btnAddAll" runat="server"></TD>
	</tr>
	<tr>
		<TD vAlign="middle" align="center" width="5%"><input class="FLOWBUTTON" id="btnClear" title="<<" style="WIDTH: 35px" type="button" size="30"
				value="清除" name="cmdSelect" runat="server"></TD>
	</tr>
</table>
<input id="hid_IDS" type="hidden" runat="server" NAME="hid_IDS"> <input id="hid_Names" type="hidden" runat="server" NAME="hid_Names">
<INPUT id="_hid_lstuser" type="hidden" runat="server" NAME="_hid_lstdept">
