<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmActorCondList" Codebehind="frmActorCondList.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>条件人员列表</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		
	</HEAD>
	<script language="javascript" src="../Js/Common.js"></script>
		<script language="javascript">
			function EditCond(CondID)
			{
				OpenNoBarWindow("frmActorCondEdit.aspx?CondID="+CondID,621,337);
			}
			function AddCond()
			{
				OpenNoBarWindow("frmActorCondEdit.aspx?CondID=",621,337);
			}
			function delete_confirm()
			{
				if (event.srcElement.value =="删除" )
					event.returnValue =confirm("是否真的要删除？");
			}
			
			//===zxl
			
			function JoinActor(ID)
			{
				window.open("frmJoinActor2.aspx?ActorType=30&ObjectID="+ID,"JoinActor" ,"scrollbars=no,status=yes ,resizable=yes,width=480,height=500,left=250,top=100");
			}

			document.onclick=delete_confirm;
		</script>
	<body>
		<form id="Form1" method="post" runat="server">
			<table id="tbmain" width="100%" class="listContent">
				<tr>
					<td class="list" colspan="4">
						<uc1:CtrTitle id="CtrTitle" runat="server"></uc1:CtrTitle>
					</td>
					
				</tr>
				<tr>
				    <td class="listTitle">
				        编号：
				    </td>
				    <td class="list">
				        <asp:TextBox ID="txtCondID" runat="server"></asp:TextBox>
				    </td>
				    <td class="listTitle">
				        名称：
				    </td>
				    <td class="list">
				        <asp:TextBox ID="txtCondName" runat="server"></asp:TextBox>
				        <asp:Button ID="cmdQuery" runat="server" Text="查询" onclick="cmdQuery_Click"/>
				    </td>
				    
				</tr>
				<tr height="40">
					<td class="listTitle" colspan="4">
					    <INPUT id="cmdAdd" type="button" value="添加" onclick="AddCond()" class="btnClass">
						<asp:Button id="cmdDelete" runat="server" Text="删除" onclick="cmdDelete_Click" CssClass="btnClass"></asp:Button>
						<asp:Button id="cmdLoad" runat="server" Text="刷新" onclick="cmdLoad_Click" CssClass="btnClass"></asp:Button>
					</td>
				</tr>
	      </table>
	      <table id="Table1" width="100%" cellpadding=0>
				<tr>
					<td class="listContent">
						<asp:DataGrid  id="dgActorConds" runat="server" AutoGenerateColumns="False"  CssClass="Gridtable"  Width="100%">
							<Columns>
								<asp:TemplateColumn HeaderText="选择">
									<HeaderStyle Wrap="False" Width="30px"></HeaderStyle>
									<ItemTemplate>
										<asp:CheckBox id="chkSelect" runat="server"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CondID" HeaderText="编号">
									<HeaderStyle Wrap="False" Width="60px"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CondName" HeaderText="名称">
									<HeaderStyle Wrap="False" Width="400px"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="编辑">
									<HeaderStyle Width="30px"></HeaderStyle>
									<ItemStyle Wrap="False" Width="25px"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink id="hl1" runat="server" NavigateUrl='<%#"javascript:EditCond("+DataBinder.Eval(Container.DataItem, "CondID")+")"%>'>编辑</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="用户组">
									<ItemTemplate>
										<a href="#" onclick='JoinActor(<%#DataBinder.Eval(Container.DataItem, "CondID")%>)'>
											加入</a>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:DataGrid>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
