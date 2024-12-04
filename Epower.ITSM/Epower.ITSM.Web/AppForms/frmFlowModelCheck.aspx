<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmFlowModelCheck.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmFlowModelCheck" Title="流程模型检查" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table cellpadding="0" width="100%" align="center">
	<tr>
		<td align="center" class="list">
			<asp:datagrid id="dgFlowModelCheck" runat="server" cssclass="GridTable" Width="100%" OnItemCreated="dgFlowModelCheck_ItemCreated" OnItemDataBound="dgFlowModelCheck_ItemDataBound">
				<Columns>
					<asp:BoundColumn DataField='FlowModelID' HeaderText='流程模型编号'></asp:BoundColumn>
					<asp:BoundColumn DataField='FlowName' HeaderText='流程模型名称'></asp:BoundColumn>
					<asp:BoundColumn DataField='CheckResult' HeaderText='检查内容'></asp:BoundColumn>
					<asp:BoundColumn DataField='CheckResult' HeaderText='检查结果'></asp:BoundColumn>
				</Columns>
				</asp:datagrid>
			</td>
	</tr>
</table>
</asp:Content>
