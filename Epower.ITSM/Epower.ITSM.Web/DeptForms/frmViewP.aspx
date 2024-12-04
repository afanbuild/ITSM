<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmViewP.aspx.cs" Inherits="Epower.ITSM.Web.DeptForms.frmViewP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <TABLE id="Table1" height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr height="20">
					<td style="HEIGHT: 1px"><FONT face="宋体"></FONT></td>
				</tr>
				<TR height="100%">
					<TD style="HEIGHT: 62.79%" vAlign="middle" align="center"><TABLE id="Table2" cellSpacing="0" cellPadding="0" width="500" border="0">
							<TR>
								<TD style="HEIGHT: 29px" align="center" colSpan="4"><FONT face="宋体">
										<TABLE id="Table3" style="WIDTH: 283px; HEIGHT: 25px" cellSpacing="0" cellPadding="0" width="283"
											border="0">
											<TR class="StatusBarForPop">
												<TD>用户登陆ID:</TD>
												<TD style="WIDTH: 142px">
													<asp:TextBox id="LoginName" runat="server"></asp:TextBox></TD>
												<TD>
													<asp:Button id="btnShow" runat="server" Width="49px" Text="查看" OnClick="btnShow_Click"></asp:Button></TD>
											</TR>
										</TABLE>
									</FONT>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 209px; HEIGHT: 19px"><FONT face="宋体"></FONT></TD>
								<TD style="HEIGHT: 19px"><FONT face="宋体"></FONT></TD>
								<TD style="HEIGHT: 19px"></TD>
								<TD style="HEIGHT: 19px"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 209px; HEIGHT: 23px">
									<DIV style="DISPLAY: inline; WIDTH: 192px; HEIGHT: 18px" align="right" ms_positioning="FlowLayout">用户名称：</DIV>
								</TD>
								<TD style="HEIGHT: 23px"><asp:textbox id="txtName" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
								<TD style="HEIGHT: 23px"></TD>
								<TD style="HEIGHT: 23px"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 209px; HEIGHT: 28px"><FONT face="宋体">
										<DIV style="DISPLAY: inline; WIDTH: 192px; HEIGHT: 18px" align="right" ms_positioning="FlowLayout">用户密码：</DIV>
									</FONT>
								</TD>
								<TD style="HEIGHT: 28px" vAlign="middle"><FONT face="宋体">
										<asp:textbox id="txtPSW" runat="server" ReadOnly="True" BorderStyle="None"></asp:textbox></FONT></TD>
								<TD style="HEIGHT: 28px"></TD>
								<TD style="HEIGHT: 28px"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 209px; HEIGHT: 28px"><FONT face="宋体"></FONT>
								</TD>
								<TD style="HEIGHT: 28px" vAlign="middle"><FONT face="宋体"></FONT></TD>
								<TD style="HEIGHT: 28px"></TD>
								<TD style="HEIGHT: 28px"></TD>
							</TR>
							<TR>
								<TD style="HEIGHT: 33px" align="center" colSpan="4"><FONT face="宋体"></FONT></TD>
							</TR>
							<TR class="StatusBarForPop">
								<TD align="center" colSpan="4"><FONT face="宋体"> <INPUT style="WIDTH: 49px; HEIGHT: 20px" onclick="javascript:window.close()" type="button"
											value="关闭"></FONT></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 3px">&nbsp;</TD>
				</TR>
			</TABLE>
    </div>
    </form>
</body>
</html>
