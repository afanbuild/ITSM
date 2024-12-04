<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmCheckUser" Codebehind="frmCheckUser.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>¼ì²âµÇÂ½ÕÊ»§</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<FONT face="ËÎÌå">
				<TABLE id="tbTips" style="Z-INDEX: 101; LEFT: 10px; POSITION: absolute; TOP: 0px" cellSpacing="1"
					cellPadding="1" width="100%" border="0" height="100%" runat="server">
					<TR>
						<TD align="left" id="tdTip">
							<asp:Label id="lblTips" runat="server"></asp:Label></TD>
					</TR>
					<TR>
						<TD align="center" height="20"><INPUT id="btnClose" onclick="self.close();" style="WIDTH: 62px; HEIGHT: 24px" type="button"
								value="·µ»Ø"></TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
