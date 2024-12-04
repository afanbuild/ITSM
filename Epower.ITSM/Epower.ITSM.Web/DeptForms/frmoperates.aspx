<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmOperates" Codebehind="frmOperates.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<TITLE>操作项维护</TITLE>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script src="../Js/common.js" language="javascript"></script>
		<script language="javascript">
			function Show_ModifyWindow(obj)
			{
				reg=/cmdModify/g;
				var idtag=obj.id.replace(reg,"");
				var rid=document.all(idtag+"labOperateID").innerText;
				OpenNoBarWindow("frmeditoperate.aspx?operateid="+rid,500,300);
			}
			
			
		</script>
	</HEAD>
	<body bgcolor="#ffffff">
		<FORM id="Form1" method="post" runat="server">
			<TABLE id="tbMain" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR vAlign="top" height="40">
					<TD width="20"></TD>
					<TD>
						<uc1:CtrTitle id="uTitle" runat="server"></uc1:CtrTitle></TD>
					<TD></TD>
				</TR>
				<TR height="30">
					<TD></TD>
					<TD><INPUT onclick="OpenNoBarWindow('frmeditoperate.aspx?operateid=',500,300);" type="button"
							value="新增">
						<asp:Button id="cmdDelete" runat="server" Text="删除" onclick="cmdDelete_Click"></asp:Button></TD>
					<TD></TD>
				</TR>
				<TR>
					<TD></TD>
					<TD>请选择操作项类型:
						<asp:DropDownList id="dpdOpType" runat="server"></asp:DropDownList>操作项ID:
						<asp:TextBox id="txtOpID" runat="server"></asp:TextBox>
						<asp:Button id="cmdFind" runat="server" Text="查询" onclick="cmdFind_Click"></asp:Button></TD>
					<TD></TD>
				</TR>
				<TR height="10">
					<TD></TD>
					<TD></TD>
					<TD></TD>
				</TR>
				<TR>
					<TD></TD>
					<TD>
						<asp:DataGrid CssClass="table" id="dgOperate" runat="server" AutoGenerateColumns="False"  CssClass="Gridtable"  AllowPaging="True"
							PageSize="20">
							<ItemStyle CssClass="tablebody"></ItemStyle>
							<HeaderStyle Height="25px" CssClass="tableheader"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:CheckBox id="chkSelect" runat="server" Visible='<%#GetVisible((int)DataBinder.Eval(Container.DataItem, "OperateID"))%>'>
										</asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn>
									<ItemTemplate>
										<a href="#" id="cmdModify" runat="server" onclick="Show_ModifyWindow(this)">编辑</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="操作项ID">
									<ItemTemplate>
										<asp:Label ID="labOperateID" Runat=server Text='<%#DataBinder.Eval(Container.DataItem, "OperateID")%>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="OpName" HeaderText="操作名称"></asp:BoundColumn>
								<asp:BoundColumn DataField="OpTypeName" HeaderText="操作类别"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="SqlStatement" HeaderText="SQL语句"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Paramaters" HeaderText="参数表"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="ConnectSystem" HeaderText="关联系统"></asp:BoundColumn>
								<asp:BoundColumn DataField="OpDesc" HeaderText="描述"></asp:BoundColumn>
							</Columns>
							<PagerStyle Visible="False"></PagerStyle>
						</asp:DataGrid>
						<uc1:ControlPage id="ControlPageOperate" runat="server"></uc1:ControlPage></TD>
					<TD></TD>
				</TR>
			</TABLE>
		</FORM>
	</body>
</HTML>
