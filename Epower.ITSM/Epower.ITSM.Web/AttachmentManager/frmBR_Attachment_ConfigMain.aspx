<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="frmBR_Attachment_ConfigMain.aspx.cs" Inherits="Epower.ITSM.Web.AttachmentManager.frmBR_Attachment_ConfigMain" Title="必填附件配置信息查询" %>

<%@ Register TagPrefix="uc1" TagName="ControlPageFoot" Src="../Controls/ControlPageFoot.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<table cellpadding='2' cellspacing='0' width='98%' class="listContent GridTable">
<tr>
	<td class="listTitleRight" style='width:12%;'>
	    应用名称				
    </td>		
    <td class='list' style='width:35%;'>
	   <asp:DropDownList ID="ddlApp" runat="server" Width="152px" AutoPostBack="true"            
            onselectedindexchanged="ddlApp_SelectedIndexChanged">
       </asp:DropDownList>
    </td>		
    <td class="listTitleRight" style='width:12%;'>
	    流程名称
    </td>		
    <td class='list'>
	   <asp:DropDownList ID="ddlFlowModel" runat="server" Width="152px">
       </asp:DropDownList> 
    </td>
</tr>
</table>
<br />
<table cellpadding="0"  cellspacing="0" width="98%" align="center" border="0" class="">
	<tr>
		<td>
			<asp:datagrid id="dgBR_Attachment_Config" runat="server" Width="100%" CssClass="GridTable" OnItemDataBound="dgBR_Attachment_Config_ItemDataBound" 
			 OnItemCommand="dgBR_Attachment_Config_ItemCommand" OnItemCreated="dgBR_Attachment_Config_ItemCreated">
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
					<asp:BoundColumn DataField='AppName' HeaderText='应用名称'></asp:BoundColumn>
					<asp:BoundColumn DataField='FlowName' HeaderText='流程名称'></asp:BoundColumn>
					<asp:BoundColumn DataField='AppID' HeaderText='AppID' Visible="false"></asp:BoundColumn>
					<asp:BoundColumn DataField='OFLOWMODELID' HeaderText='OFLOWMODELID' Visible="false"></asp:BoundColumn>
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
	    <td align="right"><uc1:ControlPageFoot id="cpBR_Attachment_Config" runat="server" /></td>
	</tr>
</table>
<script type="text/javascript" language="javascript">
	function checkAll(checkAll) {
		var len = document.forms[0].elements.length;
		var cbCount = 0;
		for (i = 0; i < len; i++) {
			if (document.forms[0].elements[i].type == "checkbox") {
				if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgBR_Attachment_Config") != -1 &&
				document.forms[0].elements[i].disabled == false) {
					document.forms[0].elements[i].checked = checkAll.checked;
					cbCount += 1;
				}
			}
		}
	} 
</script>
</asp:Content>
