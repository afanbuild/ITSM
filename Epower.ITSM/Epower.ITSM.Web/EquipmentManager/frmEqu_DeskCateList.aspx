<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEqu_DeskCateList.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_DeskCateList"
    Title="资产目录管理" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="uc1" TagName="ControlPageFoot" Src="../Controls/ControlPageFoot.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding='1' cellspacing='2' width='98%' border='0' class='listContent'>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                资产目录
            </td>
            <td class='list'>
                <asp:TextBox ID='txtListName' runat='server'></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgEqu_CateLists" runat="server" Width="100%" OnItemDataBound="dgEqu_CateLists_ItemDataBound"
                    OnItemCommand="dgEqu_CateLists_ItemCommand" OnItemCreated="dgEqu_CateLists_ItemCreated">
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
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='CatalogID' HeaderText='资产ID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='CatalogName' HeaderText='资产类别'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='ListName' HeaderText='资产目录'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
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
            <td align="right" class="listTitle">
                <uc1:ControlPageFoot ID="cpEqu_CateLists" runat="server" />
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
				document.forms[0].elements[i].name.indexOf("dgEqu_CateLists") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                        document.forms[0].elements[i].checked = checkAll.checked;
                        cbCount += 1;
                    }
                }
            }
        } 
    </script>

</asp:Content>
