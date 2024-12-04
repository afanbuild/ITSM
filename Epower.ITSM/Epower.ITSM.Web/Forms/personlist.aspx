<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.PersonList" Codebehind="PersonList.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>PersonList</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 19px; WIDTH: 162px; POSITION: absolute; TOP: 8px; HEIGHT: 285px"
				cellSpacing="1" cellPadding="1" width="162" border="0">
				<TR>
					<TD vAlign="top" colSpan="1" rowSpan="1">
						<asp:ListBox id="lstPerson" runat="server" Height="281px" Width="158px"></asp:ListBox>
					</TD>
				</TR>
			</TABLE>
			<INPUT style="Z-INDEX: 103; LEFT: 123px; POSITION: absolute; TOP: 297px"
				onclick="window.parent.close();" type="button" value=" 取  消" class="btnClass" >
			<asp:Button id="cmdOK" style="Z-INDEX: 104; LEFT: 48px; POSITION: absolute; TOP: 297px" runat="server"
				Text=" 确  定" onclick="Button1_Click"></asp:Button>
		</form>
	</body>
</HTML>
