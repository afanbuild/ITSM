<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmChangePassword" Codebehind="frmChangePassword.aspx.cs" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics.WebUI.WebDataInput.v5.1, Version=5.1.20051.37, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>修改密码</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
  </HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr height="20">
					<td style="HEIGHT: 1px"><FONT face="宋体"></FONT></td>
				</tr>
				<TR height="100%">
					<TD style="HEIGHT: 62.79%" vAlign="middle" align="center"><TABLE id="Table2" cellSpacing="0" cellPadding="0" width="500" border="0">
							<TR>
								<TD style="WIDTH: 209px">
									<DIV style="DISPLAY: inline; WIDTH: 176px; HEIGHT: 18px" align="right">登录用户名称：</DIV>
								</TD>
								<TD><asp:textbox id="txtLoginName" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
								<TD></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 209px; HEIGHT: 23px">
									<DIV style="DISPLAY: inline; WIDTH: 176px; HEIGHT: 18px" align="right">用户名称：</DIV>
								</TD>
								<TD style="HEIGHT: 23px"><asp:textbox id="txtName" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
								<TD style="HEIGHT: 23px"></TD>
								<TD style="HEIGHT: 23px"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 209px; HEIGHT: 28px"><FONT face="宋体">
										<DIV style="DISPLAY: inline; WIDTH: 176px; HEIGHT: 18px" align="right">旧密码：</DIV>
									</FONT>
								</TD>
								<TD style="HEIGHT: 28px" vAlign="middle"><FONT face="宋体">
										<asp:TextBox id="txtOldPwd" runat="server" TextMode="Password" Width="135px"></asp:TextBox></FONT></TD>
								<TD style="HEIGHT: 28px"></TD>
								<TD style="HEIGHT: 28px"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 209px; HEIGHT: 28px">
									<DIV style="DISPLAY: inline; WIDTH: 176px; HEIGHT: 18px" align="right">新密码：</DIV>
								</TD>
								<TD style="HEIGHT: 28px" vAlign="middle">
									<asp:TextBox id="txtFistPwd" runat="server" Width="135px" TextMode="Password"></asp:TextBox></TD>
								<TD style="HEIGHT: 28px"></TD>
								<TD style="HEIGHT: 28px"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 209px; HEIGHT: 28px">
									<DIV style="DISPLAY: inline; WIDTH: 176px; HEIGHT: 18px" align="right">确认新密码：</DIV>
								</TD>
								<TD style="HEIGHT: 28px" vAlign="middle">
									<asp:TextBox id="txtLastPwd" runat="server" Width="135px" TextMode="Password"></asp:TextBox>
									<asp:CompareValidator id="CompareValidator1" runat="server" ControlToValidate="txtLastPwd" ErrorMessage="两次输入不一致"
										ControlToCompare="txtFistPwd"></asp:CompareValidator></TD>
								<TD style="HEIGHT: 28px"></TD>
								<TD style="HEIGHT: 28px"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 209px; HEIGHT: 21px"><FONT face="宋体"></FONT></TD>
								<TD style="HEIGHT: 21px"></TD>
								<TD style="HEIGHT: 21px"></TD>
								<TD style="HEIGHT: 21px"></TD>
							</TR>
        <TR>
          <TD style="HEIGHT: 33px" align=center colSpan=4></TD></TR>
							<TR class="StatusBarForPop">
								<TD align="center" colSpan="4"><FONT face="宋体"><asp:button id="cmdSave" runat="server" Text="确认" Width="56px" onclick="cmdSave_Click"></asp:button>&nbsp;&nbsp;
										<INPUT style="WIDTH: 49px; HEIGHT: 20px" onclick="javascript:window.close()" type="button"
											value="取消"></FONT></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 3px">&nbsp;</TD>
				</TR>
			</TABLE>
			<INPUT id="hidUserID" type="hidden" runat="server"> &nbsp; <INPUT id="hidPassWord" type="hidden" runat="server">
		</form>
	</body>
</HTML>
