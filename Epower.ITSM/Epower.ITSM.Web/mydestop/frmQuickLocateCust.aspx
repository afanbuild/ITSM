<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmQuickLocateCust.aspx.cs" Inherits="Epower.ITSM.Web.MyDestop.frmQuickLocateCust"
    Title="快速查询用户" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
        function ServerOndblclick(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16, value17) {
            var arr = new Array();
            arr[1] = value1;
            arr[2] = value2;
            arr[3] = value3;
            arr[4] = value4;
            arr[5] = value5;
            arr[11] = value6;
            arr[12] = value7;
            arr[13] = value8;
            arr[6] = value9;
            arr[7] = value10;
            arr[8] = value11;
            arr[9] = value12;
            arr[10] = value13;
            arr[14] = value14;
            arr[15] = value15;
            arr[16] = value16;
            arr[17] = value17;

            window.parent.returnValue = arr;
            top.close();
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
                        <asp:BoundColumn DataField="ShortName" HeaderText="客户名称">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Address" HeaderText="客户地址">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="LinkMan1" HeaderText="联系人">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Tel1" HeaderText="联系人电话">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustomCode" HeaderText="客户代码">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Email" HeaderText="电子邮件">
                            <HeaderStyle Width="8%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="MastCust" HeaderText="服务单位">
                            <HeaderStyle Width="8%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:Button runat="server" ID="lnkselect" CommandName="Select" Text="选择"  SkinID="btnClass1" />
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
