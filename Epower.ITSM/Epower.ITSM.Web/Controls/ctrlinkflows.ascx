<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.ctrLinkFlows" Codebehind="ctrLinkFlows.ascx.cs" %>
<TABLE id="Table1" height="100%" width="100%">
	<TR>
		<TD vAlign="top" align="left"><asp:datagrid id="dgLinkFlow" runat="server" Width="100%" CellPadding="3" BorderWidth="1px" PageSize="16"
				BorderColor="#CCCCCC" BackColor="White" BorderStyle="None" AutoGenerateColumns="False"  CssClass="Gridtable"  Visible="False">
				<FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>
				<SelectedItemStyle Font-Size="X-Small" Font-Bold="True" ForeColor="White" BackColor="#669999"></SelectedItemStyle>
				<AlternatingItemStyle Font-Size="X-Small"></AlternatingItemStyle>
				<ItemStyle Font-Size="X-Small" ForeColor="#000066"></ItemStyle>
				<HeaderStyle ForeColor="#000033" BackColor="#F1F5FF"></HeaderStyle>
				<Columns>
					<asp:BoundColumn DataField="FlowName" HeaderText="流程名称">
						<HeaderStyle Width="220px"></HeaderStyle>
					</asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="IsLink" HeaderText="类别">
						<HeaderStyle Width="0px"></HeaderStyle>
					</asp:BoundColumn>
					<asp:TemplateColumn>
						<HeaderStyle Width="60px"></HeaderStyle>
					</asp:TemplateColumn>
					<asp:BoundColumn Visible="False" DataField="LinkFlowModelID"></asp:BoundColumn>
				</Columns>
				<PagerStyle NextPageText="下一页" PrevPageText="上一页" HorizontalAlign="Left" ForeColor="#000066"
					BackColor="White"></PagerStyle>
			</asp:datagrid></TD>
	</TR>
</TABLE>
