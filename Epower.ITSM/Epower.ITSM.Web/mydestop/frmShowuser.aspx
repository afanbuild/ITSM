<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmShowuser" Codebehind="frmShowuser.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>�鿴�û�</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" borderColor="#000000" cellSpacing="1" borderColorDark="#ffffff" cellPadding="1"	width="100%"  borderColorLight="#000000" border="0">
				<tr height="20">
					<td></td>
				</tr>
				<TR height="100%">
					<TD vAlign="top" align="left">
						<TABLE id="Table5" cellSpacing="1" cellPadding="1" width="100%"  class="listContent" border="0">
							<TR>
								<TD style="WIDTH: 30%; HEIGHT: 16px" class="listTitle" align="right">
									�û����ƣ�
								</TD>
								<TD style="HEIGHT: 16px" class="list" >
                                    <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label></TD>
							</TR>	
							<TR>
								<TD class="listTitle" align="right">
									ְλ��
								</TD>
								<TD class="list"><asp:Label id="lblJob" runat="server" Width="159px"></asp:Label></TD>
							</TR>
							<TR>
								<TD class="listTitle" align="right">
									�绰��
								</TD>
								<TD class="list"><asp:Label id="lblTelNo" runat="server" Width="159px"></asp:Label></TD>
							</TR>
							<TR>
								<TD class="listTitle" align="right">
									�ֻ����룺
								</TD>
								<TD class="list"><asp:Label id="lblMobile" runat="server" Width="159px"></asp:Label></TD>
							</TR>
							<TR>
								<TD  class="listTitle" align="right">
									�����ʼ���
								</TD>
								<TD  class="list"><asp:Label id="lblEmail" runat="server" Width="159px"></asp:Label></TD>
							</TR>
							<TR>
								<TD  class="listTitle" align="right">
										QQ���룺
								</TD>
								<TD  class="list">
									<asp:Label id="lblQQ" runat="server" Width="159px"></asp:Label></TD>
							</TR>
							<TR>
								<TD class="listTitle" align="right">
									ѧ����
								</TD>
								<TD style="HEIGHT: 16px" class="list"><asp:Label id="lblEdu" runat="server" Width="97px"></asp:Label>
								<asp:dropdownlist id="dropEdu" runat="server" Width="97px" Visible="false">
										<asp:ListItem Value="" Selected="True">��ѡ��...</asp:ListItem>
										<asp:ListItem Value="��ʿ">��ʿ</asp:ListItem>
										<asp:ListItem Value="˶ʿ">˶ʿ</asp:ListItem>
										<asp:ListItem Value="��ѧ">��ѧ</asp:ListItem>
										<asp:ListItem Value="��ר">��ר</asp:ListItem>
										<asp:ListItem Value="��ר">��ר</asp:ListItem>
										<asp:ListItem Value="����">����</asp:ListItem>
										<asp:ListItem Value="����">����</asp:ListItem>
										<asp:ListItem Value="Сѧ">Сѧ</asp:ListItem>
									</asp:dropdownlist>
								</TD>
							</TR>
							<TR>
								<TD  class="listTitle" align="right">
                                        ��ɫ��
								</TD>
								<TD  class="list"><asp:Label id="lblRole" runat="server" Width="159px"></asp:Label></TD>
							</TR>
							<TR>
								<TD class="listTitle" align="right">
										�������ţ�
								</TD>
								<TD class="list"><asp:Label id="lblDeptName" runat="server" Width="160"></asp:Label></TD>
							</TR>
							<TR>
								<TD class="listTitle" align="right">
									����������ID��
								</TD>
								<TD class="list"><asp:Label id="lblSortID" runat="server" Width="160px">-1</asp:Label></TD>
							</TR>
						</TABLE>
						<INPUT id="hidUserID" type="hidden" runat="server">
					</TD>
				</TR>
				<TR>
					<TD align="center" class="listTitle">
						<INPUT style="WIDTH: 56px;" onclick="javascript:window.close()" type="button" value="�ر�" class="btnClass"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
