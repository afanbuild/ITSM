<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmActorCondList" Codebehind="frmActorCondList.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>������Ա�б�</title>
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
				if (event.srcElement.value =="ɾ��" )
					event.returnValue =confirm("�Ƿ����Ҫɾ����");
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
				        ��ţ�
				    </td>
				    <td class="list">
				        <asp:TextBox ID="txtCondID" runat="server"></asp:TextBox>
				    </td>
				    <td class="listTitle">
				        ���ƣ�
				    </td>
				    <td class="list">
				        <asp:TextBox ID="txtCondName" runat="server"></asp:TextBox>
				        <asp:Button ID="cmdQuery" runat="server" Text="��ѯ" onclick="cmdQuery_Click"/>
				    </td>
				    
				</tr>
				<tr height="40">
					<td class="listTitle" colspan="4">
					    <INPUT id="cmdAdd" type="button" value="���" onclick="AddCond()" class="btnClass">
						<asp:Button id="cmdDelete" runat="server" Text="ɾ��" onclick="cmdDelete_Click" CssClass="btnClass"></asp:Button>
						<asp:Button id="cmdLoad" runat="server" Text="ˢ��" onclick="cmdLoad_Click" CssClass="btnClass"></asp:Button>
					</td>
				</tr>
	      </table>
	      <table id="Table1" width="100%" cellpadding=0>
				<tr>
					<td class="listContent">
						<asp:DataGrid  id="dgActorConds" runat="server" AutoGenerateColumns="False"  CssClass="Gridtable"  Width="100%">
							<Columns>
								<asp:TemplateColumn HeaderText="ѡ��">
									<HeaderStyle Wrap="False" Width="30px"></HeaderStyle>
									<ItemTemplate>
										<asp:CheckBox id="chkSelect" runat="server"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CondID" HeaderText="���">
									<HeaderStyle Wrap="False" Width="60px"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CondName" HeaderText="����">
									<HeaderStyle Wrap="False" Width="400px"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="�༭">
									<HeaderStyle Width="30px"></HeaderStyle>
									<ItemStyle Wrap="False" Width="25px"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink id="hl1" runat="server" NavigateUrl='<%#"javascript:EditCond("+DataBinder.Eval(Container.DataItem, "CondID")+")"%>'>�༭</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="�û���">
									<ItemTemplate>
										<a href="#" onclick='JoinActor(<%#DataBinder.Eval(Container.DataItem, "CondID")%>)'>
											����</a>
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
