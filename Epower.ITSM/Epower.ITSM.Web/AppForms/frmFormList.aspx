<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmFormList.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmFormList" Title="无标题页" %>
<%@ Register TagPrefix="uc1" TagName="ControlPageFoot" Src="../Controls/ControlPageFoot.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table cellpadding='1' cellspacing='2' width='100%' border='0' class='listContent'>
<tr>
	<td class='listTitle'  align='right' style='width:15%;'>
	表单名称				
</td>		
<td class='list'>
	<asp:TextBox ID='txtdj_name' runat='server'></asp:TextBox>			
    </td>	<td class='listTitle'  align='right' style='width:15%;'>
	表单分类				
</td>		
<td class='list'>
	<asp:DropDownList ID="ddltdjlx" runat="server"></asp:DropDownList>	
</td>		
</tr>
<tr>
	<td class='listTitle'  align='right' style='width:15%;'>
	表单SN				
</td>		
<td class='list'>
	<asp:TextBox ID='txtFormSN' runat='server'></asp:TextBox>			
</td>
	<td class='listTitle'  align='right' style='width:15%;'>
	流程名称				
</td>		
<td class='list'>
	<asp:TextBox ID='txtFlowName' runat='server'></asp:TextBox>			
</td>		
</tr>
<tr style="display:none;">
    <td class="list" colspan="4" align="center"><asp:Button ID="btnSync" runat="server" Text="同步流程模型" OnClick="btnSync_Click" /></td>
</tr>
</table>
<br />
<table cellpadding="0"  cellspacing="0" width="100%" align="center" border="0">
	<tr>
		<td align="center"  class="listContent">
			<asp:datagrid id="dgFC_BILLZL" runat="server" Width="100%" OnItemDataBound="dgFC_BILLZL_ItemDataBound" 
			 OnItemCommand="dgFC_BILLZL_ItemCommand" OnItemCreated="dgFC_BILLZL_ItemCreated">
				<AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			    <HeaderStyle CssClass="listTitle"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="center">
						<HeaderTemplate>
                             <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                        </HeaderTemplate>
						<ItemTemplate>
							<asp:CheckBox id="chkDel" runat="server"></asp:CheckBox>
						</ItemTemplate>
						<HeaderStyle Width="5%"></HeaderStyle>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField='djid' HeaderText='djid' Visible="false"></asp:BoundColumn>
					<asp:BoundColumn DataField='dj_name' HeaderText='表单名称'></asp:BoundColumn>
					<asp:BoundColumn DataField='djlx' HeaderText='表单分类'></asp:BoundColumn>
					<asp:BoundColumn DataField='djsn' HeaderText='表单SN'></asp:BoundColumn>
					<asp:BoundColumn DataField='userType' HeaderText='生成路径分类'></asp:BoundColumn>
					<asp:BoundColumn DataField='FlowName' HeaderText='流程名称'></asp:BoundColumn>
					<asp:BoundColumn DataField='oFlowModelID' HeaderText='oFlowModelID' Visible="false"></asp:BoundColumn>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="修改">
						<ItemTemplate>
							<asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="修改" CommandName="edit"  />
						</ItemTemplate>
						<HeaderStyle Width="60px"></HeaderStyle>
					</asp:TemplateColumn>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="运行">
						<ItemTemplate>
							<asp:Button ID="lnkrun" SkinID="btnClass1" runat="server" Text="运行" CommandName="run"  />
						</ItemTemplate>
						<HeaderStyle Width="60px"></HeaderStyle>
					</asp:TemplateColumn>
				</Columns>
				</asp:datagrid>
			</td>
	</tr>
	<tr>
	    <td align="right" class="listTitle"><uc1:ControlPageFoot id="cpFC_BILLZL" runat="server" /></td>
	</tr>
</table>
<script type="text/javascript" language="javascript">
	function checkAll(checkAll) {
		var len = document.forms[0].elements.length;
		var cbCount = 0;
		for (i = 0; i < len; i++) {
			if (document.forms[0].elements[i].type == "checkbox") {
				if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgFC_BILLZL") != -1 &&
				document.forms[0].elements[i].disabled == false) {
					document.forms[0].elements[i].checked = checkAll.checked;
					cbCount += 1;
				}
			}
		}
	} 
</script>
</asp:Content>