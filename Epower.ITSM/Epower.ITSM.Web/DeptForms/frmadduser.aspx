<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmAddUser" Codebehind="frmAddUser.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>frmAddUser</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<FONT face="����">
				<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px" cellSpacing="0"
					cellPadding="0" width="100%" border="0">
					<TR>
						<TD style="HEIGHT: 106px" align="center">
							<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="75%" border="0">
								<TR>
									<TD>
										<DIV style="DISPLAY: inline; WIDTH: 136px; HEIGHT: 36px" align="right">��¼�û����ƣ�</DIV>
									</TD>
									<TD>
										<asp:TextBox id="txtLoginName" runat="server"></asp:TextBox>
										<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ControlToValidate="txtLoginName" ErrorMessage="����Ϊ��"></asp:RequiredFieldValidator></TD>
									<TD></TD>
									<TD></TD>
								</TR>
								<TR>
									<TD style="HEIGHT: 16px">
										<DIV style="DISPLAY: inline; WIDTH: 136px; HEIGHT: 18px" align="right">�û����ƣ�</DIV>
									</TD>
									<TD style="HEIGHT: 16px">
										<asp:TextBox id="txtName" runat="server"></asp:TextBox>
										<asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ControlToValidate="txtName" ErrorMessage="����Ϊ��"></asp:RequiredFieldValidator></TD>
									<TD style="HEIGHT: 16px"></TD>
									<TD style="HEIGHT: 16px"></TD>
								</TR>
								<TR>
									<TD>
										<DIV style="DISPLAY: inline; WIDTH: 136px; HEIGHT: 18px" align="right">���룺</DIV>
									</TD>
									<TD>
										<asp:TextBox id="txtFistPwd" runat="server" TextMode="Password"></asp:TextBox></TD>
									<TD></TD>
									<TD></TD>
								</TR>
								<TR>
									<TD>
										<DIV style="DISPLAY: inline; WIDTH: 136px; HEIGHT: 18px" align="right">ȷ�����룺</DIV>
									</TD>
									<TD>
										<asp:TextBox id="txtLastPwd" runat="server" TextMode="Password"></asp:TextBox>
										<asp:CompareValidator id="CompareValidator2" runat="server" ControlToValidate="txtFistPwd" ErrorMessage="�������벻һ��"
											ControlToCompare="txtLastPwd"></asp:CompareValidator></TD>
									<TD></TD>
									<TD></TD>
								</TR>
								<TR>
									<TD style="HEIGHT: 4px">
										<DIV style="DISPLAY: inline; WIDTH: 136px; HEIGHT: 18px" align="right">�Ա�</DIV>
									</TD>
									<TD style="HEIGHT: 4px">
										<asp:DropDownList id="dropSex" runat="server" Width="152px">
											<asp:ListItem Value="1">��</asp:ListItem>
											<asp:ListItem Value="2">Ů</asp:ListItem>
										</asp:DropDownList></TD>
									<TD style="HEIGHT: 4px"></TD>
									<TD style="HEIGHT: 4px"></TD>
								</TR>
								<TR>
									<TD>
										<DIV style="DISPLAY: inline; WIDTH: 136px; HEIGHT: 18px" align="right">ְλ��</DIV>
									</TD>
									<TD>
										<asp:TextBox id="txtJob" runat="server"></asp:TextBox></TD>
									<TD></TD>
									<TD></TD>
								</TR>
								<TR>
									<TD>
										<DIV style="DISPLAY: inline; WIDTH: 136px; HEIGHT: 18px" align="right">�绰��</DIV>
									</TD>
									<TD>
										<asp:TextBox id="txtTelNo" runat="server"></asp:TextBox></TD>
									<TD></TD>
									<TD></TD>
								</TR>
								<TR>
									<TD>
										<DIV style="DISPLAY: inline; WIDTH: 136px; HEIGHT: 18px" align="right">�����ʼ���</DIV>
									</TD>
									<TD>
										<asp:TextBox id="txtEmail" runat="server"></asp:TextBox>
										<asp:regularexpressionvalidator id="RegularExpressionValidator2" runat="server" ErrorMessage="�����ʼ�����" ControlToValidate="txtEmail"
											ValidationExpression="^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$"></asp:regularexpressionvalidator></TD>
									<TD></TD>
									<TD></TD>
								</TR>
								<TR>
									<TD>
										<DIV style="DISPLAY: inline; WIDTH: 136px; HEIGHT: 18px" align="right">ѧ����</DIV>
									</TD>
									<TD>
										<asp:DropDownList id="dropEdu" runat="server" Width="152px">
											<asp:ListItem Value="��ʿ">��ʿ</asp:ListItem>
											<asp:ListItem Value="˶ʿ">˶ʿ</asp:ListItem>
											<asp:ListItem Value="��ѧ">��ѧ</asp:ListItem>
											<asp:ListItem Value="��ר">��ר</asp:ListItem>
											<asp:ListItem Value="��ר">��ר</asp:ListItem>
											<asp:ListItem Value="����">����</asp:ListItem>
											<asp:ListItem Value="����">����</asp:ListItem>
											<asp:ListItem Value="Сѧ">Сѧ</asp:ListItem>
										</asp:DropDownList></TD>
									<TD></TD>
									<TD></TD>
								</TR>
								<TR>
									<TD>
										<DIV style="DISPLAY: inline; WIDTH: 136px; HEIGHT: 18px" align="right">��ɫ��</DIV>
									</TD>
									<TD>
										<asp:TextBox id="txtRole" runat="server" Width="157px"></asp:TextBox></TD>
									<TD></TD>
									<TD></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 7px" align="center">
							<asp:Button id="Button1" runat="server" Width="58px" Text="ȷ��" onclick="Button1_Click"></asp:Button><INPUT type="button" onclick="javascript:window.history.back(1)" value="����"></TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
