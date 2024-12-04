<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmBr_ContactMain.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmBr_ContactMain"
    Title="无标题页" %>

<%@ Register Src="../Controls/BussinessControls/CustomCtr.ascx" TagName="CustomCtr"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width='98%' cellpadding="2" cellspacing="0" class='listContent' id="tbCustom" runat="server">
        <tr>
            <td class='listTitleRight ' style="width: 12%;">
                客户简称
            </td>
            <td class='list' style="width: 35%;">
                <asp:TextBox ID='txtCustomName' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight ' style="width: 12%;">
                联系人
            </td>
            <td class="list">
                <asp:TextBox ID='txtContactName' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight '>
                性别
            </td>
            <td class='list'>
                <asp:DropDownList ID="ddltSex" runat="server" Width="152px">
                    <asp:ListItem Selected="True" Value="-1">--性别--</asp:ListItem>
                    <asp:ListItem Value="0">男</asp:ListItem>
                    <asp:ListItem Value="1">女</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class='listTitleRight '>
                地址
            </td>
            <td class="list">
                <asp:TextBox ID='txtAddress' runat='server' Width="85%"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table cellpadding="0" width="98%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgBr_Contact" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgBr_Contact_ItemCommand"
                    OnItemCreated="dgBr_Contact_ItemCreated" OnItemDataBound="dgBr_Contact_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitleRight "></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='CustomID' HeaderText='CustomID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='CustomName' HeaderText='单位简称'></asp:BoundColumn>
                        <asp:BoundColumn DataField='ContactName' HeaderText='联系人'></asp:BoundColumn>
                        <asp:BoundColumn DataField='Sex' HeaderText='性别'></asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText='QQ/MSN'>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "QQ") %>/<%#DataBinder.Eval(Container.DataItem, "MSN") %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='OfficeTel' HeaderText='办公电话'></asp:BoundColumn>
                        <asp:BoundColumn DataField='Mobil' HeaderText='手机'></asp:BoundColumn>
                        <asp:BoundColumn DataField='EMail' HeaderText='邮箱'></asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="修改" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc1:ControlPage ID="ControlPage1" runat="server"></uc1:ControlPage>
            </td>
        </tr>
    </table>
</asp:Content>
