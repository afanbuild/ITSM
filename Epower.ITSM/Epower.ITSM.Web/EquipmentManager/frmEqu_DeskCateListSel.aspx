<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmEqu_DeskCateListSel.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_DeskCateListSel" Title="资产目录选择列表" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="uc1" TagName="ControlPageFoot" Src="../Controls/ControlPageFoot.ascx" %>
<%@ Register src="ctrFlowEquCategory.ascx" tagname="ctrFlowEquCategory" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" language="javascript">
        function ServerOndblclick(value0,value1)
        {
            var arr = new Array();
            arr[0] = value0;
            arr[1] = value1;
            window.parent.returnValue = arr;
            top.close();
        }
    </script>
    <table cellpadding='2' cellspacing='0' width='98%' border='0' class='listContent'>
        <tr>
            <td class="listTitleRight" style='width: 12%;'>
                资产分类
            </td>
            <td class="list">                
                <uc2:ctrFlowEquCategory ID="ctrFlowEquCategory1" RootID="1" runat="server" />                
            </td>
            <td class="listTitleRight" align='right' style='width: 12%;'>
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
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='ListName' HeaderText='资产目录'  ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="选择" >
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1"  runat="server" Text="选择" CommandName="Sel" />
                            </ItemTemplate>
                            <HeaderStyle Width="60"></HeaderStyle>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc1:ControlPageFoot ID="cpEqu_CateLists" runat="server" />
            </td>
        </tr>
    </table>

</asp:Content>
