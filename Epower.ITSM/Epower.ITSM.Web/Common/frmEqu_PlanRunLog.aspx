<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmEqu_PlanRunLog.aspx.cs" Inherits="Epower.ITSM.Web.Common.frmEqu_PlanRunLog" Title="无标题页" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table cellpadding="0"  cellspacing="0" width="100%" align="center" border="0">
	<tr>
		<td align="center"  class="listContent">
			<asp:datagrid id="dgEA_PlanDetail" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"  cellpadding="1" cellspacing="2" BorderWidth="0px" OnItemCreated="dgEA_PlanDetail_ItemCreated" OnItemDataBound="dgEA_PlanDetail_ItemDataBound" >
				<AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			    <HeaderStyle CssClass="listTitle"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="center" Visible="false">
						<ItemTemplate>
							<asp:CheckBox id="chkDel" runat="server"></asp:CheckBox>
						</ItemTemplate>
						<HeaderStyle Width="5%"></HeaderStyle>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField='ID' HeaderText='ID' Visible="false"></asp:BoundColumn>
					<asp:BoundColumn DataField='RunStatus' HeaderText='RunStatus' Visible="false"></asp:BoundColumn>
					<asp:BoundColumn DataField='PlanName' HeaderText='计划名称'></asp:BoundColumn>
					<asp:BoundColumn DataField='Name' HeaderText='用户名称'></asp:BoundColumn>
					<asp:BoundColumn DataField='RunRemark' HeaderText='运行备注'></asp:BoundColumn>
					<asp:BoundColumn DataField='LastTime' HeaderText='运行时间'></asp:BoundColumn>
					<asp:BoundColumn DataField='RunStatus' HeaderText='运行状态'></asp:BoundColumn>
				</Columns>
				</asp:datagrid>
			</td>
	</tr>
	<tr>
	    <td align="right" class="listTitle"><uc1:ControlPage id="ControlPage1" runat="server"></uc1:ControlPage></td>
	</tr>
</table>
</asp:Content>
