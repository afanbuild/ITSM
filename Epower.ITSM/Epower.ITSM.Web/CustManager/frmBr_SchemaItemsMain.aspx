<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmBr_SchemaItemsMain.aspx.cs" Inherits="Epower.ITSM.Web.CustManager.frmBr_SchemaItemsMain"
    Title="配置项" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
        //全选复选框
        function checkAll(checkAll) {
            var len = document.forms[0].elements.length;
            var cbCount = 0;
            for (i = 0; i < len; i++) {
                if (document.forms[0].elements[i].type == "checkbox") {
                    if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgBr_SchemaItems") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                        document.forms[0].elements[i].checked = checkAll.checked;

                        cbCount += 1;
                    }
                }
            }
        }
        function doubleSelect(obj) {
            obj.click();
        }
    </script>

    <style type="text/css">
        .listtd
        {
            background-color: #ffffff;
        }
    </style>
    <table cellpadding='2' cellspacing='0' width='98%' border='0' class='listContent Gridtable'>
        <tr style="display: none;">
            <td class='listTitleRight' style='width: 12%;'>
                属性代码
            </td>
            <td class='list' style="width: 35%">
                <asp:TextBox ID='txtFieldID' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight' style="width: 12%">
                属性名称
            </td>
            <td class='list' style="width: 35%">
                <asp:TextBox ID='txtCHName' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight' style="width: 12%">
                类型
            </td>
            <td class='list' style="width: 35%">
                <asp:DropDownList ID="ddltitemType" runat="server" Width="152px">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="0">基础属性</asp:ListItem>
                    <asp:ListItem Value="1">关联属性</asp:ListItem>
                    <asp:ListItem Value="2">备注属性</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="trChecked" runat="server">
            <td colspan="4" class='listtd' align="center">
                <asp:Button ID="BtnChecked" Text="选择" runat="server" OnClick="BtnChecked_Click" />
                <asp:Button ID="btnCancel" Text="取消" runat="server" OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0" class="Gridtable">
        <tr>
            <td>
                <asp:DataGrid ID="dgBr_SchemaItems" runat="server" Width="100%" CellPadding="1" CellSpacing="2"
                    BorderWidth="0px" AutoGenerateColumns="False"   OnItemCommand="dgBr_SchemaItems_ItemCommand"
                    OnItemCreated="dgBr_SchemaItems_ItemCreated" OnItemDataBound="dgBr_SchemaItems_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitleRight"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField='FieldID' HeaderText='属性代码' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='CHName' HeaderText='属性名称' ItemStyle-Width="35%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='itemType' HeaderText='类型' ItemStyle-Width="35%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="修改" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="修改" CommandName="edit" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="False" HeaderText="选择" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Button ID="lnkSelect" SkinID="btnClass1" runat="server" Text="选择" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc2:ControlPageFoot ID="cpBr_SchemaItems" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
