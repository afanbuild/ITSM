<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmDeptQuery" Codebehind="frmDeptQuery.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<TITLE>frmDeptQuery</TITLE>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript">
		<!--
			//只允许输入数字
			function NumberKey()
			{
				if(!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode==8 || event.keyCode==46))
				{
					//alert(event.keyCode);
					event.returnValue = false;
				}
			}
			
			function EditDept(lngDeptID,lngDeptName)
			{
				var features =
				'dialogWidth:500px;' +
				'dialogHeight:500px;' +
				'directories:no; localtion:no; menubar:no; status=no; toolbar=no;scroll:no;resizable=yes';
				var result;
				result = window.showModalDialog('frmDeptEdit_Container.aspx?deptid='+lngDeptID+'&DeptText='+lngDeptName,'',features);
				if(result == "refresh") window.location.reload();

			}

		-->
		</script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体">
				<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 0px" height="100%"
					cellSpacing="1" cellPadding="1" width="100%" border="0">
					<TR>
						<TD vAlign="top" align="center" colSpan="3">
							<TABLE id="Table2" style="WIDTH: 694px; HEIGHT: 184px" cellSpacing="1" cellPadding="1"
								width="694" border="0">
								<TR>
									<TD style="HEIGHT: 31px; BACKGROUND-COLOR: #99ccff" colSpan="3">
										<TABLE id="Table3" style="WIDTH: 504px; HEIGHT: 23px" cellSpacing="1" cellPadding="1" width="504"
											border="0">
											<TR>
												<TD style="WIDTH: 53px" noWrap>部门ID：</TD>
												<TD style="WIDTH: 126px" noWrap><asp:textbox id="txtDeptID" onkeydown="return NumberKey();" runat="server" MaxLength="20" Width="100px"></asp:textbox></TD>
												<TD style="WIDTH: 52px" noWrap>部门名：</TD>
												<TD style="WIDTH: 209px"><asp:textbox id="txtDeptName" runat="server" Width="184px"></asp:textbox></TD>
												<TD><asp:button id="btnQuery" runat="server" Width="51px" Text="查 询" onclick="btnQuery_Click"></asp:button></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD vAlign="top" align="left" colSpan="3" height="10"><asp:datagrid id="dgDeptInfo" runat="server" Width="100%" PageSize="50" BorderColor="#E7E7FF"
											BorderStyle="None" BorderWidth="1px" BackColor="Black" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False"  CssClass="Gridtable" >
											<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
											<AlternatingItemStyle Wrap="False" BackColor="#F7F7F7"></AlternatingItemStyle>
											<ItemStyle Wrap="False" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
											<HeaderStyle Font-Bold="True" HorizontalAlign="Left" ForeColor="Black" BackColor="#FFFFC0"></HeaderStyle>
											<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
											<Columns>
												<asp:BoundColumn DataField="DeptID" HeaderText="部门ID"></asp:BoundColumn>
												<asp:TemplateColumn HeaderText="部门名">
													<ItemTemplate>
														<%#GetDeptName(DataBinder.Eval(Container.DataItem, "DeptID").ToString(),DataBinder.Eval(Container.DataItem, "DeptName").ToString(),DataBinder.Eval(Container.DataItem, "DeptKind").ToString(),DataBinder.Eval(Container.DataItem, "IsTemp").ToString())%>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:BoundColumn DataField="FullDeptName" HeaderText="部门路径">
													<HeaderStyle Wrap="False"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="DeptName" HeaderText="DeptName">
													<HeaderStyle Wrap="False"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="DeptKind" HeaderText="DeptKind"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="IsTemp" HeaderText="IsTemp"></asp:BoundColumn>
											</Columns>
											<PagerStyle Visible="False" HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF"
												Mode="NumericPages"></PagerStyle>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD colSpan="3"><uc1:controlpage id="ControlPage1" runat="server" Visible="False"></uc1:controlpage></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
		</SCRIPT>
	</body>
</HTML>
