<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEqu_SchemaItemsMain.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_SchemaItemsMain"
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
				document.forms[0].elements[i].name.indexOf("dgEqu_SchemaItems") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                        document.forms[0].elements[i].checked = checkAll.checked;

                        cbCount += 1;
                    }
                }
            }
        }

        function ServerOndblclick(value1) {
            var arr;
            arr = value1;
            window.parent.returnValue = arr;
            top.close();
        }
    </script>

    <style type="text/css">
        .listtd
        {
            background-color: #ffffff;
        }
    </style>
    <table cellpadding='2' cellspacing='0' width='98%' border='0' class='listContent GridTable'>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                配置项名称
            </td>
            <td class='list'>
                <asp:TextBox ID='txtCHName' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight' style='width: 12%;'>
                类型
            </td>
            <td class='list' colspan="3">
                <asp:DropDownList ID="ddltitemType" runat="server" Width="152px">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="0">基础信息</asp:ListItem>
                    <asp:ListItem Value="1">关联配置</asp:ListItem>
                    <asp:ListItem Value="2">备注信息</asp:ListItem>
                    <asp:ListItem Value="3">下拉选择</asp:ListItem>
                    <asp:ListItem Value="4">部门信息</asp:ListItem>
                    <asp:ListItem Value="5">用户信息</asp:ListItem>
                    <asp:ListItem Value="6">日期类型</asp:ListItem>
                    <asp:ListItem Value="7">数值类型</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="display: none;">
            <td class='listTitleRight' align='right' style='width: 12%;'>
                配置项代码
            </td>
            <td class='list' visible="false" colspan="5">
                <asp:TextBox ID='txtFieldID' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr id="trChecked" runat="server">
            <td align="center" colspan="6" class='listtd'>
                <asp:Button ID="BtnChecked" Text="选择" runat="server" OnClick="BtnChecked_Click" />
                <asp:Button ID="btnCancel" Text="取消" runat="server" OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0" class="Gridtable">
        <tr>
            <td align="center" class="listContent">
                <asp:DataGrid ID="dgEqu_SchemaItems" runat="server" Width="100%" CellPadding="1"
                    CellSpacing="2" BorderWidth="0px" AutoGenerateColumns="False"  OnItemCommand="dgEqu_SchemaItems_ItemCommand"
                    OnItemCreated="dgEqu_SchemaItems_ItemCreated" OnItemDataBound="dgEqu_SchemaItems_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
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
                        <asp:BoundColumn DataField='FieldID' HeaderText='配置项代码' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='CHName' HeaderText='配置项名称' ItemStyle-Width="30%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='itemType' HeaderText='类型' ItemStyle-Width="30%">
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="修改">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="修改" CommandName="edit" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="10%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="False">
                            <ItemTemplate>
                                <asp:Button ID="lnkSelect" SkinID="btnClass1" runat="server" Text="选择" CommandName="edit" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="资产配置">
                            <ItemTemplate>
                                <asp:Button Width="60" ID="lnkDesk" SkinID="btnClass1" runat="server" Text="资产配置"
                                    CommandName="look" />
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitle">
                <uc2:ControlPageFoot ID="cpEqu_SchemaItems" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
