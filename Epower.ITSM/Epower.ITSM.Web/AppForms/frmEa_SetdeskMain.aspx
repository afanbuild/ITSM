<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEa_SetdeskMain.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmEa_SetdeskMain"
    Title="服务台查询" %>

<%@ Register TagPrefix="uc1" TagName="ControlPageFoot" Src="../Controls/ControlPageFoot.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" src="../Js/jquery-1.7.2.min.js.js"></script>
    <table cellpadding='1' cellspacing='2' width='98%' border='0' class='listContent'>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                用户名称
            </td>
            <td class='list'>
                <uc2:UserPicker ID="UserPicker1" runat="server" />
            </td>
            <td class='listTitleRight' style='width: 15%;'>
                座席（对应呼叫中心座席）
            </td>
            <td class='list'>
                <asp:TextBox ID='txtBlockRoom' runat='server'></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgEa_Setdesk" runat="server" CssClass="GridTable" Width="100%" OnItemDataBound="dgEa_Setdesk_ItemDataBound"
                    OnItemCommand="dgEa_Setdesk_ItemCommand" OnItemCreated="dgEa_Setdesk_ItemCreated">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitleRight"></HeaderStyle>
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
                        <asp:BoundColumn DataField='UserID' HeaderText='用户ID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='UserName' HeaderText='用户名称'></asp:BoundColumn>
                        <asp:BoundColumn DataField='BlockRoom' HeaderText='座席（对应呼叫中心座席）'></asp:BoundColumn>
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
            <td align="right">
                <uc1:ControlPageFoot ID="cpEa_Setdesk" runat="server" />
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
        function checkAll(objectCheck) {

            var demo = document.getElementById('<%=dgEa_Setdesk.ClientID%>');
            var gg = demo.getElementsByTagName('INPUT');
            for (i = 0; i < gg.length; i++) {
                if (gg[i].type == "checkbox") {
                    gg[i].checked = objectCheck.checked;
                }
            }
        }
    
    </script>

</asp:Content>
