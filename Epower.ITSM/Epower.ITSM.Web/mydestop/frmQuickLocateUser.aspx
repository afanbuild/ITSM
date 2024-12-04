<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmQuickLocateUser.aspx.cs" Inherits="Epower.ITSM.Web.MyDestop.frmQuickLocateUser" Title="快速查询用户" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<TABLE id="Table1" borderColor="#000000" cellSpacing="1" borderColorDark="#ffffff" cellPadding="1"
	width="100%"  borderColorLight="#000000" border="0">
    <TR>
			<TD vAlign="top" align="center" colSpan="2" class="listContent">
			<asp:datagrid id="dgUserInfo" runat="server" Width="100%" PageSize="50" AutoGenerateColumns="False"  CssClass="Gridtable" 
					GridLines="Horizontal"  cellpadding="1" cellspacing="2" BorderWidth="0px" OnItemCommand="dgUserInfo_ItemCommand" >
					<AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
					<Columns>
						<asp:BoundColumn Visible="False" DataField="UserId"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="姓名" ItemStyle-Wrap="true">
						    <ItemTemplate>
						        <asp:LinkButton ID="lnklink" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "name")%>' CommandName="Select"  />
						    </ItemTemplate>
						    <HeaderStyle Width="20%" ></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
					    </asp:TemplateColumn>
						<asp:BoundColumn DataField="FullDeptName" HeaderText="部 门">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn Visible="False" DataField="DeptID" HeaderText="部门ID"></asp:BoundColumn>
					</Columns>
					<PagerStyle Visible="False" HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF"
						Mode="NumericPages"></PagerStyle>
				</asp:datagrid>
			</TD>
		</TR> 
        <TR>
			<TD class="listTitle" align="right"><INPUT id="hidQueryDeptID" style="WIDTH: 72px; HEIGHT: 19px" type="hidden" size="6" runat="server">
			<INPUT id="hidDeptID" style="WIDTH: 72px; HEIGHT: 19px" type="hidden" size="6" runat="server"></TD>
		</TR>
</TABLE>
</asp:Content>
