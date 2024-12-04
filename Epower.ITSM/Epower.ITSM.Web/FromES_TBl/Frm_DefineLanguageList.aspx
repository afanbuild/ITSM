<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Frm_DefineLanguageList.aspx.cs" Inherits="Epower.ITSM.Web.FromES_TBl.Frm_DefineLanguageList"
    Title="自定义信息项名称" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc4" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="2" cellspacing="0" width="98%" align="center" border="0" class="listContent GridTable">
        <tr>
            <td class="listTitleRight " runat="server" style="width: 12%">
                <asp:Literal ID="litGroup" runat="server" Text="所属分组"></asp:Literal>
            </td>
            <td class="list" style="width: 35%" runat="server">
                <asp:DropDownList ID="ddlGroup" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
            <td id="Td1" class="listTitleRight " runat="server" style="width: 12%">
                <asp:Literal ID="Literal1" runat="server" Text="信息项名称"></asp:Literal>
            </td>
            <td id="Td2" class="list" style="width: 35%" runat="server">
                <asp:TextBox ID="txtKeyVa" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgDefineLanguages" runat="server" Width="100%" CellPadding="1"
                    CellSpacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemDataBound="dgDefineLanguages_ItemDataBound">
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitleRight "></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="ID" Visible="false"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="序号" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <%#Container.ItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="信息项名称">
                            <ItemTemplate>
                                <asp:TextBox ID="txtKeyValue" Text='<%# DataBinder.Eval(Container, "DataItem.KeyValue")%>'
                                    Width="90%" runat="server"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Width="25%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="所属分组">
                            <ItemTemplate>
                                <asp:TextBox ID="txtGroup" Text='<%# DataBinder.Eval(Container, "DataItem.Groups")%>'
                                    Width="95%" runat="server" Enabled="False"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Width="30%" />
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc4:ControlPageFoot ID="cpDefineLanguages" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
