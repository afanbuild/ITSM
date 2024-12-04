<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmDelayFlow" Codebehind="frmDelayFlow.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>处理时限延长</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="80%" border="0" style="Z-INDEX: 101; LEFT: 18px; POSITION: absolute; TOP: 36px">
				<TR>
					<TD align="center" width="40%">延长时间（分钟）</TD>
					<TD align="left" width="60%" colSpan="1" rowSpan="1">
						<asp:TextBox id="txtMinute" runat="server">60</asp:TextBox>
						<asp:RangeValidator id="RangeValidator1" runat="server" ErrorMessage="请输入正确的延长时间" MaximumValue="10000"
							MinimumValue="0" Type="Integer" ControlToValidate="txtMinute"></asp:RangeValidator></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2">
						<asp:Button id="cmdDelay" runat="server" Text="确定" CssClass="flowbutton" onclick="cmdDelay_Click"></asp:Button></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
