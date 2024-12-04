<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmBr_OrderClassTypeMain.aspx.cs" Inherits="Epower.ITSM.Web.CustManager.frmBr_OrderClassTypeMain" Title="班次查询" %>

<%@ Register TagPrefix="uc1" TagName="ControlPageFoot" Src="../Controls/ControlPageFoot.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table cellpadding='2' cellspacing='0' width='98%' class='listContent'>
<tr>
	<td class='listTitleRight' style='width:12%;'>
	    班次名称				
    </td>		
    <td class='list'>
	    <asp:TextBox ID='txtClassTypeName' runat='server'></asp:TextBox>			
    </td>		
</tr>
</table>
<br />
<table cellpadding="0"  cellspacing="0" width="98%" align="center" border="0">
	<tr>
		<td>
			<asp:datagrid id="dgBr_OrderClassType" runat="server" Width="100%" OnItemDataBound="dgBr_OrderClassType_ItemDataBound" 
			 OnItemCommand="dgBr_OrderClassType_ItemCommand" OnItemCreated="dgBr_OrderClassType_ItemCreated">
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
					<asp:BoundColumn DataField='ID' HeaderText='ID' Visible="false"></asp:BoundColumn>
					<asp:BoundColumn DataField='ClassTypeName' HeaderText='班次名称'></asp:BoundColumn>
					<asp:BoundColumn DataField='Remark' HeaderText='班次说明'></asp:BoundColumn>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="center"  HeaderText="修改">
						<ItemTemplate>
							<asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="修改" CommandName="edit" />
						</ItemTemplate>
						<HeaderStyle Width="5%"></HeaderStyle>
					</asp:TemplateColumn>
				</Columns>
				</asp:datagrid>
			</td>
	</tr>
	<tr>
	    <td align="right"><uc1:ControlPageFoot id="cpBr_OrderClassType" runat="server" /></td>
	</tr>
</table>
<script type="text/javascript" language="javascript">
	function checkAll(checkAll) {
		var len = document.forms[0].elements.length;
		var cbCount = 0;
		for (i = 0; i < len; i++) {
			if (document.forms[0].elements[i].type == "checkbox") {
				if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgBr_OrderClassType") != -1 &&
				document.forms[0].elements[i].disabled == false) {
					document.forms[0].elements[i].checked = checkAll.checked;
					cbCount += 1;
				}
			}
		}
	} 
</script>
</asp:Content>