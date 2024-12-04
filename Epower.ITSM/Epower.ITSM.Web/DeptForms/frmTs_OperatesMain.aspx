<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmTs_OperatesMain.aspx.cs" Inherits="Epower.ITSM.Web.DeptForms.frmTs_OperatesMain"
    Title="无标题页" %>

<%@ Register TagPrefix="uc1" TagName="ControlPageFoot" Src="../Controls/ControlPageFoot.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding='2' cellspacing='0' width='98%' class='listContent'>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                操作项名称
            </td>
            <td class='list'>
                <asp:TextBox ID='txtOpName' runat='server'></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgTs_Operates" runat="server" cssclass="GridTable" Width="100%" OnItemDataBound="dgTs_Operates_ItemDataBound"
                    OnItemCommand="dgTs_Operates_ItemCommand" OnItemCreated="dgTs_Operates_ItemCreated">
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
                        <asp:BoundColumn DataField='OperateID' HeaderText='操作项编号'></asp:BoundColumn>
                        <asp:BoundColumn DataField='OpName' HeaderText='操作项名称'></asp:BoundColumn>
                        <asp:BoundColumn DataField='OpCatalog' HeaderText='类别'></asp:BoundColumn>
                        <asp:BoundColumn DataField='OpDesc' HeaderText='描述'></asp:BoundColumn>
                        <asp:BoundColumn DataField='SysID' HeaderText='系统编号' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='OpType' HeaderText='OpType' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='SqlStatement' HeaderText='SqlStatement' Visible="false">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='Paramaters' HeaderText='Paramaters' Visible="false">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='ConnectSystem' HeaderText='ConnectSystem' Visible="false">
                        </asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="修改">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="修改" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="5%" ></HeaderStyle>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc1:ControlPageFoot ID="cpTs_Operates" runat="server" />
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
				document.forms[0].elements[i].name.indexOf("dgTs_Operates") != -1 &&
				document.forms[0].elements[i].disabled == false) {
					document.forms[0].elements[i].checked = checkAll.checked;
					cbCount += 1;
				}
			}
		}
	} 
    </script>

</asp:Content>
