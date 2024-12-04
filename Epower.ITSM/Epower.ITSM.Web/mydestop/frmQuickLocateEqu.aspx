<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmQuickLocateEqu.aspx.cs" Inherits="Epower.ITSM.Web.MyDestop.frmQuickLocateEqu"
    Title="快速查询设备\产品" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
        function ServerOndblclick(value1, value2, value3,value4) {
            var arr = new Array();
            arr[1] = value1;
            arr[2] = value2;
            arr[3] = value3;
            arr[4] = value4;
            window.parent.returnValue = arr;
            top.close();
        }
        
        function OnClientClick(selectcommandId)
        {
            $("#" + selectcommandId ).click();
        }
    </script>

    <table id="Table1" bordercolor="#000000" cellspacing="1" bordercolordark="#ffffff"
        cellpadding="1" width="100%" bordercolorlight="#000000" border="0">
        <tr>
            <td valign="top" align="center" colspan="2" class="listContent">
                <asp:DataGrid ID="dgUserInfo" runat="server" Width="100%" PageSize="50" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    GridLines="Horizontal" CellPadding="1" CellSpacing="2" BorderWidth="0px" OnItemCommand="dgUserInfo_ItemCommand"
                    OnItemDataBound="dgUserInfo_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="id" HeaderText="id"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="ListID" HeaderText="资产目录ID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CatalogName" HeaderText="资产分类">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ListName" HeaderText="资产目录">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Name" HeaderText="资产名称">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Code" HeaderText="资产编号">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CostomName" HeaderText="所属客户">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="选择">
                            <ItemTemplate>
                                <asp:Button runat="server" ID="lnkselect" CommandName="Select" Text="选择" SkinID="btnClass1" />
                            </ItemTemplate>
                            <HeaderStyle Width="5%" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle Visible="False" HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF"
                        Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td class="listTitle" align="right">
                <input id="hidQueryDeptID" style="width: 72px; height: 19px" type="hidden" size="6"
                    runat="server">
                <input id="hidDeptID" style="width: 72px; height: 19px" type="hidden" size="6" runat="server">
            </td>
        </tr>
    </table>
</asp:Content>
