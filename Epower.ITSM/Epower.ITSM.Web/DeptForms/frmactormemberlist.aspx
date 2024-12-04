<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmActorMemberList" Codebehind="frmActorMemberList.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>frmActorMemberList</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="../Js/Common.js"></script>
		<script language="javascript">
			
			function AddActorMember(actorid)
			{
			    if(document.all.hidActorID.value=="0")
			    {
			        alert("请选择用户组！");
			        return false;
			    }
				OpenNoBarWindow("frmActorMemberEdit.aspx?actormemberid=0"+"&actorid="+document.all.hidActorID.value,350,250);
			}
			
			function EditActorMember(id,actorid)
			{
				OpenNoBarWindow("frmActorMemberEdit.aspx?actormemberid="+id+"&actorid="+actorid,350,250);
			}
			
			function ShowMulUser(userordept)
			{
			    if(document.all.hidActorID.value=="0")
			    {
			        alert("请选择用户组！");
			        return false;
			    }
			    OpenNoBarWindow("frmMulAddUser.aspx?actormemberid=0"+"&actorid="+document.all.hidActorID.value + "&type=" + userordept,350,250);
			    event.returnValue = true;
			}
			
			//全选复选框
		function checkAll(checkAll)
		{			  
			var len = document.forms[0].elements.length;
			var cbCount = 0;
			for (i=0;i < len;i++)
			{
				if (document.forms[0].elements[i].type == "checkbox")
				{
					if (document.forms[0].elements[i].name.indexOf("Chk") != -1 && 
						document.forms[0].elements[i].disabled == false)
					{
						document.forms[0].elements[i].checked = checkAll.checked;

						cbCount += 1;
					}
				}
			}		
		} 
		</script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" width="100%" height="20" class="listContent">
				<tr>
					<td class="list" colspan=2><uc1:ctrtitle id="CtrTitle" runat="server"></uc1:ctrtitle>
						<INPUT id="hidActorID" type="hidden" runat="server"></td>
				</tr>
				<TR >
					<TD vAlign="middle" align="left" class="listTitle">描述
					</TD>
					<td class="list"><DIV id="divDESC" style="DISPLAY: inline; WIDTH: 360px; HEIGHT: 15px" runat="server">Label</DIV></td>
				</TR>
				<tr>
				    <td class="listTitle">
				    用户组成员查询
				    </td>
				    <td class="list"> 
                        <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
				        <asp:Button ID="cmdQuery" runat="server" Text="查询" onclick="cmdQuery_Click"/>
				    </td>
				</tr>
				<TR>
					<TD style="HEIGHT: 3px" vAlign="middle" align="left" colSpan="2" class="listTitle">
					<asp:button id="btnDel" runat="server" Text="删除" onclick="btnDel_Click" CssClass="btnClass"></asp:button>
					<INPUT id="cmdAdd" onclick="AddActorMember()" type="button" value="添加成员" class="btnClass">
					<asp:Button ID="btnMulAddUser" runat="server" Text="批量添加用户成员" OnClientClick="ShowMulUser(0);" SkinID="btnClass3" />
					<asp:Button ID="btnMulAddDept" runat="server" Text="批量添加部门成员" OnClientClick="ShowMulUser(1);" SkinID="btnClass3" />
				    <asp:button id="cmdLoad" runat="server" Text="刷新" onclick="cmdLoad_Click" CssClass="btnClass"></asp:button>
                        </TD>
				</TR>
</table>
<br />
<TABLE id="Table2" width="100%" height="20" class="listContent" cellpadding=0>
				<TR>
					<TD vAlign="top" align="left" class="listContent">
					<asp:datagrid id="dgUserInfo" runat="server" AutoGenerateColumns="False"  
                            CssClass="Gridtable"  AllowPaging="True"
							PageSize="20" Width="100%" cellpadding="1" cellspacing="2" BorderWidth="0px" 
                            onitemdatabound="dgUserInfo_ItemDataBound" >
							<AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                            <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                            <HeaderStyle CssClass="listTitle"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn >
									<HeaderStyle Width="40px"></HeaderStyle>
									<ItemStyle Wrap="False"></ItemStyle>
									<HeaderTemplate>
						                <asp:CheckBox id="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
						            </HeaderTemplate>
									<ItemTemplate>
										<INPUT id="Checkbox1" type="checkbox" name='Chk<%#DataBinder.Eval(Container.DataItem, "id")%>' style='<%#GetVisible(DataBinder.Eval(Container.DataItem, "IsInRange").ToString())%>'>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="编辑">
									<HeaderStyle Width="40px"></HeaderStyle>
									<ItemStyle Wrap="False"></ItemStyle>
									<ItemTemplate>
										<!--<asp:HyperLink id=HyperLink1  runat="server" NavigateUrl='<%#"javascript:EditActorMember("+DataBinder.Eval(Container.DataItem, "id")+","+DataBinder.Eval(Container.DataItem, "actorid")+")"%>'>编辑</asp:HyperLink>-->
										<A onclick='<%#"javascript:EditActorMember("+DataBinder.Eval(Container.DataItem, "id")+","+DataBinder.Eval(Container.DataItem, "actorid")+")"%>' href="#" 
										  style='<%#GetVisible(DataBinder.Eval(Container.DataItem, "IsInRange").ToString())%>'>编辑</A>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="Id">
									<ItemStyle Wrap="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="actortypename" ReadOnly="True" HeaderText="类别">
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<%--<asp:BoundColumn DataField="objectname" ReadOnly="True" HeaderText="名称">
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>--%>
								<asp:TemplateColumn HeaderText="名称">
								    <ItemStyle Wrap="False"></ItemStyle>
								    <ItemTemplate>
								        <asp:Label ID="lblName" runat="server"></asp:Label>
								        <asp:HiddenField ID="hidactorName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"actorname") %>' />
								        <asp:HiddenField ID="hidobjectName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"objectName") %>' />
								    </ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="actortype">
									<ItemStyle Wrap="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="objectid">
									<ItemStyle Wrap="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="IsInRange"></asp:BoundColumn>
							</Columns>
							<PagerStyle Visible="False"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<tr>
					<td class="listTitle" align="right"><uc1:controlpage id="ControlActorMembers" runat="server"></uc1:controlpage></td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
