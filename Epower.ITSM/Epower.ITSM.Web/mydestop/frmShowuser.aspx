<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmShowuser" Codebehind="frmShowuser.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>查看用户</title>
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
									用户名称：
								</TD>
								<TD style="HEIGHT: 16px" class="list" >
                                    <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label></TD>
							</TR>	
							<TR>
								<TD class="listTitle" align="right">
									职位：
								</TD>
								<TD class="list"><asp:Label id="lblJob" runat="server" Width="159px"></asp:Label></TD>
							</TR>
							<TR>
								<TD class="listTitle" align="right">
									电话：
								</TD>
								<TD class="list"><asp:Label id="lblTelNo" runat="server" Width="159px"></asp:Label></TD>
							</TR>
							<TR>
								<TD class="listTitle" align="right">
									手机号码：
								</TD>
								<TD class="list"><asp:Label id="lblMobile" runat="server" Width="159px"></asp:Label></TD>
							</TR>
							<TR>
								<TD  class="listTitle" align="right">
									电子邮件：
								</TD>
								<TD  class="list"><asp:Label id="lblEmail" runat="server" Width="159px"></asp:Label></TD>
							</TR>
							<TR>
								<TD  class="listTitle" align="right">
										QQ号码：
								</TD>
								<TD  class="list">
									<asp:Label id="lblQQ" runat="server" Width="159px"></asp:Label></TD>
							</TR>
							<TR>
								<TD class="listTitle" align="right">
									学历：
								</TD>
								<TD style="HEIGHT: 16px" class="list"><asp:Label id="lblEdu" runat="server" Width="97px"></asp:Label>
								<asp:dropdownlist id="dropEdu" runat="server" Width="97px" Visible="false">
										<asp:ListItem Value="" Selected="True">请选择...</asp:ListItem>
										<asp:ListItem Value="博士">博士</asp:ListItem>
										<asp:ListItem Value="硕士">硕士</asp:ListItem>
										<asp:ListItem Value="大学">大学</asp:ListItem>
										<asp:ListItem Value="大专">大专</asp:ListItem>
										<asp:ListItem Value="中专">中专</asp:ListItem>
										<asp:ListItem Value="高中">高中</asp:ListItem>
										<asp:ListItem Value="初中">初中</asp:ListItem>
										<asp:ListItem Value="小学">小学</asp:ListItem>
									</asp:dropdownlist>
								</TD>
							</TR>
							<TR>
								<TD  class="listTitle" align="right">
                                        角色：
								</TD>
								<TD  class="list"><asp:Label id="lblRole" runat="server" Width="159px"></asp:Label></TD>
							</TR>
							<TR>
								<TD class="listTitle" align="right">
										所属部门：
								</TD>
								<TD class="list"><asp:Label id="lblDeptName" runat="server" Width="160"></asp:Label></TD>
							</TR>
							<TR>
								<TD class="listTitle" align="right">
									部门中排序ID：
								</TD>
								<TD class="list"><asp:Label id="lblSortID" runat="server" Width="160px">-1</asp:Label></TD>
							</TR>
						</TABLE>
						<INPUT id="hidUserID" type="hidden" runat="server">
					</TD>
				</TR>
				<TR>
					<TD align="center" class="listTitle">
						<INPUT style="WIDTH: 56px;" onclick="javascript:window.close()" type="button" value="关闭" class="btnClass"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
