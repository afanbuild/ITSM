<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.ControlPage" Codebehind="ControlPage.ascx.cs" %>
<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0">
	<TR height="15">
		<TD noWrap width="14"><asp:imagebutton id="btnFirstPage" SkinID="btnFirstPage" runat="server" CausesValidation="False"></asp:imagebutton></TD>
		<TD noWrap width="14"><asp:imagebutton id="btnPrevPage" SkinID="btnPrevPage" runat="server" CausesValidation="False"></asp:imagebutton></TD>
		<TD noWrap width="68" align="center" style="WIDTH: 68px">
			<asp:Label id="lblPageIndex" runat="server" Width="56px">1 / 3</asp:Label></TD>
		<TD noWrap width="14"><asp:imagebutton id="btnNextPage" SkinID="btnNextPage" runat="server" CausesValidation="False"></asp:imagebutton></TD>
		<TD noWrap width="14"><asp:imagebutton id="btnLastPage" SkinID="btnLastPage" runat="server"  CausesValidation="False"></asp:imagebutton></TD>
		<TD noWrap>��<asp:Label id="lblRowCount" runat="server"></asp:Label>��&nbsp;&nbsp;ÿҳ��ʾ
			<asp:DropDownList id="dplPageButtonCount" runat="server" AutoPostBack="True" onselectedindexchanged="dplPageButtonCount_SelectedIndexChanged">
				<asp:ListItem Value="5">5��</asp:ListItem>
				<asp:ListItem Value="10">10��</asp:ListItem>
				<asp:ListItem Value="20" Selected="True">20��</asp:ListItem>
				<asp:ListItem Value="30">30��</asp:ListItem>
				<asp:ListItem Value="50">50��</asp:ListItem>
				<asp:ListItem Value="100">100��</asp:ListItem>
				<asp:ListItem Value="200">200��</asp:ListItem>
				<asp:ListItem Value="0">ȫ��</asp:ListItem>
			</asp:DropDownList></TD>
		<TD noWrap><FONT face="����"></FONT></TD>
	</TR>
</TABLE>
