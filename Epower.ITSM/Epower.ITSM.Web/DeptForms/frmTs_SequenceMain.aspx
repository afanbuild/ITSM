<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmTs_SequenceMain.aspx.cs" Inherits="Epower.ITSM.Web.DeptForms.frmTs_SequenceMain"
    Title="系统系列号管理" %>

<%@ Register TagPrefix="uc1" TagName="ControlPageFoot" Src="../Controls/ControlPageFoot.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding='0' cellspacing='2' width='98%' class='listContent'>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                系列号关键字
            </td>
            <td class='list'>
                <asp:TextBox ID='txtName' runat='server'></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgTs_Sequence" runat="server"  cssclass="GridTable" Width="100%" OnItemDataBound="dgTs_Sequence_ItemDataBound"
                    OnItemCommand="dgTs_Sequence_ItemCommand" OnItemCreated="dgTs_Sequence_ItemCreated">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='Name' HeaderText='系列号关键字'></asp:BoundColumn>
                        <asp:BoundColumn DataField='MinValue' HeaderText='最小值'></asp:BoundColumn>
                        <asp:BoundColumn DataField='MaxValue' HeaderText='最大值'></asp:BoundColumn>
                        <asp:BoundColumn DataField='CurrentValue' HeaderText='当前值'></asp:BoundColumn>
                        <asp:BoundColumn DataField='Step' HeaderText='步进'></asp:BoundColumn>
                        <asp:BoundColumn DataField='Recycle' HeaderText='循环版本'></asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="修改">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="修改" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc1:ControlPageFoot ID="cpTs_Sequence" runat="server" />
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
	function checkAll(checkAll) {
		var len = document.forms[0].elements.length;
		var cbCount = 0;
		for (i = 0; i < len; i++) {
			if (document.forms[0].elements[i].type == "checkbox") {
				if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgTs_Sequence") != -1 &&
				document.forms[0].elements[i].disabled == false) {
					document.forms[0].elements[i].checked = checkAll.checked;
					cbCount += 1;
				}
			}
		}
	} 
    </script>

</asp:Content>
