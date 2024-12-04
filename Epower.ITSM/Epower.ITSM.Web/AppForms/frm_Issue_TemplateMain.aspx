<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_Issue_TemplateMain.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frm_Issue_TemplateMain"
    Title="无标题页" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">

        function checkAll(objectCheck) {

            var demo = document.getElementById('<%=dgECustomer.ClientID%>');
            var gg = demo.getElementsByTagName('INPUT');
            for (i = 0; i < gg.length; i++) {
                if (gg[i].type == "checkbox") {
                    gg[i].checked = objectCheck.checked;
                }
            }
        }
    </script>
    <table width="98%" border="0" cellpadding="0" cellspacing="0" class="listContent GridTable">
        <tr>
            <td class='listTitleRight' style="width: 5%">
                模板名称
            </td>
            <td class='list' style="width: 45%" colspan="3">
                <asp:TextBox ID='txtTemplateName' runat='server'></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table width="98%" border="0" cellpadding="0" cellspacing="0"> 
        <tr>
            <td>
                <asp:DataGrid ID="dgECustomer" runat="server" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgECustomer_ItemCommand"
                    OnItemCreated="dgECustomer_ItemCreated" Width="100%" OnItemDataBound="dgECustomer_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure" />
                    <ItemStyle BackColor="White" CssClass="tablebody" />
                    <HeaderStyle CssClass="listTitle" />
                    <Columns>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="5%" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="TemplateID" HeaderText="ID" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Owner" HeaderText="模版性质">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="25%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TemplateName" HeaderText="模版名称">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Wrap="False" Width="65%" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="修改">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" runat="server" CommandName="edit" SkinID="btnClass1" Text="修改" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="60" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc2:ControlPageFoot ID="cpTemplate" runat="server" />
            </td>
        </tr>
    </table>
    <asp:Label ID="labMsg" runat="server" Visible="False" ForeColor="Red"></asp:Label>
</asp:Content>
