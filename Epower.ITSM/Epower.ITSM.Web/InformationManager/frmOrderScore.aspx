<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmOrderScore.aspx.cs" Inherits="Epower.ITSM.Web.InformationManager.frmOrderScore"
    Title="无标题页" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register TagPrefix="uc2" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding='1' cellspacing='2' width="98%" align="center" border="0">
        <tr>
            <td class="Title" align="center">
                <font size="5">
                    <uc2:CtrTitle ID="CtrTitle1" runat="server" Title="知识评分排行榜"></uc2:CtrTitle>
                </font>
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgInf_Information" runat="server" Width="100%" 
                    CellPadding="1" CellSpacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCreated="dgInf_Information_ItemCreated" OnItemDataBound="dgInf_Information_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="名次" HeaderStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:Label ID="lnklink" runat="server" Text='<%#Container.ItemIndex+1%>' />
                            </ItemTemplate>
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='RegUserName' HeaderText='提供者' HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='AllScore' HeaderText='总分数' HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='iCount' HeaderText='发表知识数' HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc1:ControlPage ID="ControlPage1" runat="server" Visible="false"></uc1:ControlPage>
            </td>
        </tr>
    </table>
</asp:Content>
