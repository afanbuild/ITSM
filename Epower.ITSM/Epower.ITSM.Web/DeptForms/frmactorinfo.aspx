<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmActorInfo" Codebehind="frmActorInfo.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>frmActorInfo</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="../Js/Common.js"></script>
		<script language="javascript">
			function PopActorCond(condid)
			{
				OpenNoBarWindow("frmModCondActor.aspx?condid="+condid,500,300);
			}
			
			function AddActorMember(actorid)
			{
				OpenNoBarWindow("frmActorMemberEdit.aspx?actormemberid=0"+"&actorid="+document.all.hidActorID.value,500,300);
			}
			
			function EditActorMember(id,actorid)
			{
				OpenNoBarWindow("frmActorMemberEdit.aspx?actormemberid="+id+"&actorid="+actorid,500,300);
			}
		</script>
	</HEAD>
	<body bgcolor="#ffffff">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" borderColor="#000000" cellSpacing="1" borderColorDark="#ffffff" cellPadding="1"
				align="left" borderColorLight="#000000" border="0">
				<tr>
					<td>
						<uc1:CtrTitle id="CtrTitle" runat="server"></uc1:CtrTitle>
					</td>
				</tr>
				<TR>
					<TD style="HEIGHT: 3px" vAlign="middle" align="right" colSpan="2"><FONT face="ËÎÌå"><asp:button id="btnDel" runat="server" Text="É¾³ý" Width="56px" onclick="btnDel_Click"></asp:button>
							<INPUT id="cmdAdd" type="button" value="Ìí¼Ó³ÉÔ±" onclick="AddActorMember()"></FONT></TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left">
						<asp:datagrid id="dgUserInfo" runat="server" Width="604px" PageSize="50" BorderColor="#E7E7FF"
							BorderStyle="None" BorderWidth="1px" BackColor="Black" CellPadding="3" GridLines="Horizontal"
							AutoGenerateColumns="False"  CssClass="Gridtable" >
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" Height="25px" ForeColor="Black" BackColor="#FFFFC0"></HeaderStyle>
							<Columns>
								<asp:BoundColumn Visible="False" DataField="Id"></asp:BoundColumn>
								<asp:BoundColumn DataField="actortypename" ReadOnly="True" HeaderText="Àà±ð"></asp:BoundColumn>
								<asp:BoundColumn DataField="objectname" ReadOnly="True" HeaderText="Ãû³Æ"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="±à¼­">
									<ItemTemplate>
										<asp:HyperLink id=HyperLink1 runat="server" NavigateUrl='<%#"javascript:EditActorMember("+DataBinder.Eval(Container.DataItem, "id")+","+DataBinder.Eval(Container.DataItem, "actorid")+")"%>'>±à¼­</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="É¾³ý">
									<ItemTemplate>
										<INPUT type="checkbox" name='Chk<%#DataBinder.Eval(Container.DataItem, "id")%>'>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="actortype"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="objectid"></asp:BoundColumn>
							</Columns>
							<PagerStyle Visible="False" HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF"
								Mode="NumericPages"></PagerStyle>
						</asp:datagrid>
					</TD>
				</TR>
				<tr>
					<td><uc1:ControlPage id="ControlActorMembers" runat="server"></uc1:ControlPage>
					</td>
				</tr>
			</TABLE>
			<INPUT type="hidden" id="hidActorID" runat="server">
		</form>
	</body>
</HTML>
