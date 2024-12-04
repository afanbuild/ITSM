<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="frmBr_MastCustomerMain.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmBr_MastCustomerMain" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc2" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
        //全选复选框
        function checkAll(checkAll) {
            var len = document.forms[0].elements.length;
            var cbCount = 0;
            for (i = 0; i < len; i++) {
                if (document.forms[0].elements[i].type == "checkbox") {
                    if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgECustomer") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                        document.forms[0].elements[i].checked = checkAll.checked;

                        cbCount += 1;
                    }
                }
            }
        } 
    </script>

    <table width='98%' class="listContent">
        <tr>
            <td class="listTitleRight" style="width: 12%">
                单位简称
            </td>
            <td class="list" style="width: 35%">
                <asp:TextBox ID="txtShortName" runat="server"></asp:TextBox>
            </td>
            <td class="listTitleRight" style="width: 12%">
                单位全称
            </td>
            <td class="list" style="width: 35%">
                <asp:TextBox ID="txtFullName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitleRight" style="width: 12%">
                单位性质
            </td>
            <td class="list" style="width: 35%">
                <uc2:ctrFlowCataDropList ID="ctrFCDServiceType" runat="server" ContralState="eNormal"
                    RootID="1016" Visible="true" />
            </td>
            <td class="listTitleRight" style="width: 12%">
                传 真
            </td>
            <td class="list" style="width: 35%">
                <asp:TextBox ID="txtFax1" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitleRight" style="width: 12%">
                联 系 人
            </td>
            <td class="list" style="width: 35%">
                <asp:TextBox ID="txtLinkMan1" runat="server"></asp:TextBox>
            </td>
            <td class="listTitleRight" style="width: 12%">
                联系电话
            </td>
            <td class="list" style="width: 35%">
                <asp:TextBox ID="txtTel1" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgECustomer" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCommand="dgECustomer_ItemCommand" OnItemCreated="dgECustomer_ItemCreated"
                    OnItemDataBound="dgECustomer_ItemDataBound">
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ShortName" HeaderText="单位简称">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                单位全称</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblfullname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FullName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="EnterpriseTypeName" HeaderText="单位性质">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fax1" HeaderText="传  真">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="linkman1" HeaderText="联系人">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="tel1" HeaderText="联系电话">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="修改">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="修改" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="44" HorizontalAlign="center" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc2:ControlPageFoot ID="cpServiceStaff" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
