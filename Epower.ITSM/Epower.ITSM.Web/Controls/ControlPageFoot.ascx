<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlPageFoot.ascx.cs" Inherits="Epower.ITSM.Web.Controls.ControlPageFoot" %>
<TABLE cellSpacing="0" cellPadding="0" border="0">
	<TR height="15">
		<TD noWrap width="14"><asp:imagebutton id="btnFirst" runat="server" SkinID="btnFirstPage" CausesValidation="False"
				OnClick="btnFirst_Click"></asp:imagebutton></TD>
		<TD noWrap width="14"><asp:imagebutton id="btnPre" runat="server" SkinID="btnPrevPage" CausesValidation="False"
				OnClick="btnPre_Click"></asp:imagebutton></TD>
		<TD noWrap width="68" align="center">
			<asp:label id="LabCurrentPage" runat="server"></asp:label>/<asp:label id="LabPageCount" runat="server"></asp:label></TD>
		<TD noWrap width="14"><asp:imagebutton id="btnNext" runat="server" SkinID="btnNextPage" CausesValidation="False"
				OnClick="btnNext_Click"></asp:imagebutton></TD>
		<TD noWrap width="14"><asp:imagebutton id="btnLast" runat="server" SkinID="btnLastPage" CausesValidation="False"
				OnClick="btnLast_Click"></asp:imagebutton></TD>
		<TD noWrap>共<asp:label id="LabRecordCount" runat="server"></asp:label>条&nbsp;&nbsp;每页显示
			<asp:DropDownList id="drpPageSize" runat="server" AutoPostBack="True" onselectedindexchanged="drpPageSize_SelectedIndexChanged" Width="60px">
				<asp:ListItem Value="5">5条</asp:ListItem>
				<asp:ListItem Value="10">10条</asp:ListItem>
				<asp:ListItem Value="20" Selected="True">20条</asp:ListItem>
				<asp:ListItem Value="30">30条</asp:ListItem>
				<asp:ListItem Value="50">50条</asp:ListItem>
				<asp:ListItem Value="100">100条</asp:ListItem>
				<asp:ListItem Value="200">200条</asp:ListItem>
				<asp:ListItem Value="100000000">全部</asp:ListItem>
			</asp:DropDownList></TD>
		<TD noWrap><FONT face="宋体"></FONT></TD>
	</TR>
</TABLE>