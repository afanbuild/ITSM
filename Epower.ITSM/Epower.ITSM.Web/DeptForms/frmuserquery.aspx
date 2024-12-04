<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmUserQuery" Codebehind="frmUserQuery.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>用户查询</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="../Js/App_Common.js"></script>
		<!--#include file="../Js/tableSort.js" -->
		
	</HEAD>
	<SCRIPT language="javascript">
		<!--
			function SetPassword(userid)
			{
				window.open("frmSetPassword.aspx?userId="+userid,"设置密码" ,"scrollbars=no,resizable=no,status=yes,top=100,left=250,width=350,height=200");

			}

			function EditUser(userid,DeptID) //top="+(screen.availheight-400)/2 + ",left=" +(screen.availwidth-500)/2 + "
			{
				window.open("frmModUserDetail.aspx?userId="+userid +"&deptId="+DeptID,"" ,"scrollbars=no,resizable=no,status=no,top=100,left=250,width=500,height=450");

			}
			
			
			function AddUser() // top="+(screen.availheight-400)/2 + ",left=" +(screen.availwidth-500)/2 +
			{
				window.open("frmModUserDetail.aspx?deptId="+document.all.hidDeptID.value,"" ,"scrollbars=no,resizable=no,status=no, top=100,left=250,width=500,height=450");

			}		
			
			function JoinActor(ID)
			{
			   //debugger;
				window.open("frmJoinActor2.aspx?ActorType=20&ObjectID="+ID,"JoinActor" ,"scrollbars=no,resizable=yes,status=no,top=100,left=250,width=480,height=500");
			}
			
			function ViewActor(ID)
			{
			//debugger; top="+(screen.availheight-330)/2 + ",left=" +(screen.availwidth-270)/2 + "
			    
				window.open("frmViewBelongActors.aspx?UserID="+ID,"ViewActors" ,"scrollbars=no,resizable=no,status=no,top=100,left=250,width=270,height=330");
			}
			
			function ViewRights(ID)
			{
			   //top="+(screen.availheight-430)/2 + ",left=" +(screen.availwidth-400)/2 + " 
				window.open("frmViewRights.aspx?UserID="+ID,"ViewActors" ,"scrollbars=yes,resizable=no,status=no,top=100,left=250,width=400,height=430");
			}
			
			
			function SelectDept()
			{
				var features =
				'dialogWidth:380px;' +
				'dialogHeight:500px;' +
				'directories:no; localtion:no; menubar:no; status=no; toolbar=no;scroll:no;resizable=yes';
			    //=============zxl==
			    var url='frmpopdept.aspx?CurrDeptID='+ document.all.hidDeptID.value+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmuserquery";
			    window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=380,height=380,top=100,left=250");
                //top="+(screen.availheight-400)/2 + ",left=" +(screen.availwidth-600)/2			       
			}
						
			
			function ClearQueryDept()
			{
				document.all.txtDeptName.value="";
				document.all.hidQueryDeptID.value="";
			}
			
			
			function addtomailto(strname,stremail)
			{
				var j
				var el1 = window.parent.mail.document.fmSubmit.List1;
				var el2 = window.parent.mail.document.fmSubmit.List2;
				var el3 = window.parent.mail.document.fmSubmit.List3;
				var oOption = top.frames(1).document.createElement("OPTION");
				oOption.text=strname;
				oOption.value=stremail;
				for (j=0; j<el1.options.length; j++) 
				{
					if (el1.options(j).value==stremail)
					{
						alert("该邮件地址选择重复！");
						return;
					}
				}
					
				for (j=0; j<el2.options.length; j++) 
				{
					if (el2.options(j).value==stremail)
					{
						alert("该邮件地址选择重复！");
						return;
					}
				}
				
				for (j=0; j<el3.options.length; j++) 
				{
					if (el3.options(j).value==stremail)
					{
						alert("该邮件地址选择重复！");
						return;
					}
				}   

				el1.add(oOption);
				Show_EmailPanel();	
			}
			
			
			function addtocc(strname,stremail)
			{
				var j
				var el1 = window.parent.mail.document.fmSubmit.List1;
				var el2 = window.parent.mail.document.fmSubmit.List2;
				var el3 = window.parent.mail.document.fmSubmit.List3;
				var oOption = top.frames(1).document.createElement("OPTION");
				oOption.text=strname;
				oOption.value=stremail;
				for (j=0; j<el1.options.length; j++) 
				{
					if (el1.options(j).value==stremail)
					{
						alert("该邮件地址选择重复！");
						return;
					}
				}
					
				for (j=0; j<el2.options.length; j++) 
				{
					if (el2.options(j).value==stremail)
					{
						alert("该邮件地址选择重复！");
						return;
					}
				}
				
				for (j=0; j<el3.options.length; j++) 
				{
					if (el3.options(j).value==stremail)
					{
						alert("该邮件地址选择重复！");
						return;
					}
				}   

				el2.add(oOption);
				Show_EmailPanel();
			}


			function addtoBcc(strname,stremail)
			{
				var j
				var el1 = window.parent.mail.document.fmSubmit.List1;
				var el2 = window.parent.mail.document.fmSubmit.List2;
				var el3 = window.parent.mail.document.fmSubmit.List3;
				var oOption = top.frames(1).document.createElement("OPTION");
				oOption.text=strname;
				oOption.value=stremail;
				for (j=0; j<el1.options.length; j++) 
				{
					if (el1.options(j).value==stremail)
					{
						alert("该邮件地址选择重复！");
						return;
					}
				}
					
				for (j=0; j<el2.options.length; j++) 
				{
					if (el2.options(j).value==stremail)
					{
						alert("该邮件地址选择重复！");
						return;
					}
				}
				
				for (j=0; j<el3.options.length; j++) 
				{
					if (el3.options(j).value==stremail)
					{
						alert("该邮件地址选择重复！");
						return;
					}
				}   
				el3.add(oOption);
				Show_EmailPanel();
			}
			
			
			//显示邮件面板
			function Show_EmailPanel()
			{
				parent.frameContent.rows="*,115";
			}
		-->
		</SCRIPT>
	<body >
		<form id="Form1" method="post" runat="server">
		<TABLE id="Table5" width="100%" class="listContent">
			<TR>
				<TD style="WIDTH: 40px" noWrap align="right" class="listTitle">部门:</TD>
				<TD style="WIDTH: 160px" noWrap class="list">
				<asp:textbox id="txtDeptName" runat="server" Width="120px" ReadOnly="True"></asp:textbox>
				<input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"  type="hidden" />
				<INPUT id="cmdPopDept" title="为查询条件选择部门" onclick="SelectDept();" type="button" value="..." Class="btnClass2"
							name="cmdPopParentDept"><!--<INPUT id="btnClearDept" onclick="ClearQueryDept();" type="button" size="18" value="空" name="cmdPopParentDept" title="移除部门查询条件"></TD>-->
				<TD style="WIDTH: 39px" noWrap align="right" class="listTitle">帐号:</TD>
				<TD style="WIDTH: 105px" noWrap class="list"><asp:textbox id="txtLoginName" runat="server" Width="101"></asp:textbox></TD>
				<TD style="WIDTH: 40px" noWrap align="right" class="listTitle">学历:</TD>
				<TD style="WIDTH: 91px" noWrap class="list"><asp:dropdownlist id="ddlEdu" runat="server" Width="85px">
							<asp:ListItem Value="%" Selected="True">所有</asp:ListItem>
							<asp:ListItem Value="博士">博士</asp:ListItem>
							<asp:ListItem Value="硕士">硕士</asp:ListItem>
							<asp:ListItem Value="大学">大学</asp:ListItem>
							<asp:ListItem Value="大专">大专</asp:ListItem>
							<asp:ListItem Value="中专">中专</asp:ListItem>
							<asp:ListItem Value="高中">高中</asp:ListItem>
							<asp:ListItem Value="初中">初中</asp:ListItem>
							<asp:ListItem Value="小学">小学</asp:ListItem>
							<asp:ListItem Value="">空</asp:ListItem>
						</asp:dropdownlist></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 40px" noWrap align="right" class="listTitle">职位:</TD>
				<TD style="WIDTH: 160px" noWrap class="list"><asp:textbox id="txtPosition" runat="server" Width="150px"></asp:textbox></TD>
				<TD style="WIDTH: 39px" noWrap align="right" class="listTitle">姓名:</TD>
				<TD style="WIDTH: 105px" noWrap class="list"><asp:textbox id="txtName" runat="server" Width="101px"></asp:textbox></TD>
				<TD style="WIDTH: 40px" align="right" class="listTitle">电话:</TD>
				<TD style="WIDTH: 91px" class="list"><asp:textbox id="txtTEL" runat="server" Width="85"></asp:textbox></TD>
			</TR>
			<tr>
			    <TD style="WIDTH: 38px" noWrap align="right" class="listTitle">EMail:</TD>
				<TD style="WIDTH: 128px" noWrap class="list"><asp:textbox id="txtEmail" runat="server" Width="122px"></asp:textbox></TD>
				<TD style="WIDTH: 38px" noWrap align="right" class="listTitle">排序:</TD>
				<TD style="WIDTH: 128px" noWrap align="left" class="list" colspan="3"><asp:dropdownlist id="ddlSort" runat="server" Width="68px">
								<asp:ListItem Value="Dept" Selected="True">部门</asp:ListItem>
								<asp:ListItem Value="Name">姓名</asp:ListItem>
								 </asp:dropdownlist>
				</TD>
			</tr>
			<tr>
			    <TD noWrap class="listTitle" align="center" colspan="6"><asp:button id="btnQuery" runat="server" Text="查 询" CssClass="btnClass" ></asp:button>
			    <INPUT id="cmdAdd" onclick="AddUser()" tabIndex="1" type="button"
								value="添 加" name="cmdAdd" class="btnClass">
			    <asp:button id="btnDel" tabIndex="2" runat="server" Text="删 除" CssClass="btnClass" ></asp:button></TD>
			</tr>
		</TABLE>
		<br />
		<TABLE id="Table1" width="100%" align="center" cellpadding=0 class="listContent">
				<TR>
					<TD vAlign="top" align="center" colSpan="2" class="listContent">
					<asp:datagrid id="dgUserInfo" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"  GridLines="Horizontal"
							PageSize="50">
							 <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                            <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                            <HeaderStyle CssClass="listTitle"></HeaderStyle>
							<Columns>
								<asp:BoundColumn Visible="False" DataField="UserId"></asp:BoundColumn>
								<asp:TemplateColumn>
									<HeaderStyle Width="12px"></HeaderStyle>
									<ItemTemplate>
										<asp:ImageButton id="imgbtnStatus" runat="server" ImageUrl="../Images/PERSON.GIF"></asp:ImageButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="用户名称">
									<HeaderStyle Wrap="False"></HeaderStyle>
									<ItemStyle Wrap="False"></ItemStyle>
									<ItemTemplate>
										<%#DataBinder.Eval(Container.DataItem, "name")%>
									</ItemTemplate>
									<FooterStyle Wrap="False"></FooterStyle>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="FullDeptName" HeaderText="部 门">
									<HeaderStyle Wrap="False"></HeaderStyle>
									<ItemStyle Wrap="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="job" HeaderText="职位">
									<HeaderStyle Wrap="False"></HeaderStyle>
									<ItemStyle Wrap="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="TelNo" HeaderText="电话">
									<HeaderStyle Wrap="False"></HeaderStyle>
									<ItemStyle Wrap="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="电子邮件">
									<HeaderStyle Wrap="False"></HeaderStyle>
									<ItemStyle Wrap="False"></ItemStyle>
									<ItemTemplate>
										<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD noWrap><A href='mailto:<%#DataBinder.Eval(Container.DataItem, "email")%>'><%#DataBinder.Eval(Container.DataItem, "email")%></A></TD>
											</TR>
										</TABLE>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="EduLevel" HeaderText="学历">
									<HeaderStyle Wrap="False"></HeaderStyle>
									<ItemStyle Wrap="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="修改">
									<HeaderStyle Wrap="False"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink id=HyperLink1 runat="server" NavigateUrl='<%#"javascript:EditUser(" + DataBinder.Eval(Container.DataItem, "userId")+","+DataBinder.Eval(Container.DataItem, "DeptID")+")"%>'>修改</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="False" HeaderText="密码">
									<HeaderStyle Wrap="False"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink id=hlnkResetPSW runat="server" NavigateUrl='<%#"javascript:SetPassword(" + DataBinder.Eval(Container.DataItem, "userId")+")"%>'>重设</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="用户组">
									<HeaderStyle Wrap="False"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<A onclick='JoinActor(<%#DataBinder.Eval(Container.DataItem, "userid")%>)' href="#">
											加入</A> <A onclick='ViewActor(<%#DataBinder.Eval(Container.DataItem, "userid")%>)' href="#">
											查看</A>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="权限">
									<HeaderStyle Wrap="False"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<A onclick='ViewRights(<%#DataBinder.Eval(Container.DataItem, "userid")%>)' href="#">
											查看</A>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="删除">
									<HeaderStyle Wrap="False"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<INPUT type="checkbox" name='Chk<%#DataBinder.Eval(Container.DataItem, "userid")%>'  style='<%#GetVisible(DataBinder.Eval(Container.DataItem, "userid").ToString())%>'>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="DeptID" HeaderText="部门ID"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Deleted" HeaderText="状态"></asp:BoundColumn>
							</Columns>
							<PagerStyle Visible="False" HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF"
								Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<tr><td align=right class="listTitle"><uc1:controlpage id="ControlPageUserInfo" runat="server"></uc1:controlpage></td></tr>
			</TABLE>											

		<INPUT id="hidDeptID" style="WIDTH: 72px; HEIGHT: 19px" type="hidden" size="6" runat="server">
		<INPUT id="hidDeptName" style="WIDTH: 72px; HEIGHT: 19px" type="hidden" size="6" runat="server">
		<INPUT id="hidQueryDeptID" style="WIDTH: 72px; HEIGHT: 19px" type="hidden" size="6" runat="server">
		</form>
	</body>
</HTML>
