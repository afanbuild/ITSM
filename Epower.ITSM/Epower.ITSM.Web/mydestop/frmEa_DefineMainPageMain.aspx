<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEa_DefineMainPageMain.aspx.cs" Inherits="Epower.ITSM.Web.mydestop.frmEa_DefineMainPageMain" %>

<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
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
				document.forms[0].elements[i].name.indexOf("dgEa_DefineMainPage") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                        document.forms[0].elements[i].checked = checkAll.checked;

                        cbCount += 1;
                    }
                }
            }
        } 
    </script>

    <table width='98%' class='listContent GridTable' cellpadding="2" cellspacing="0">
        <tr>
            <td class="listTitleRight" style="width: 12%;">
                标题
            </td>
            <td class="list" style="width: 84%" colspan="3">
                <asp:TextBox ID='txtTitle' runat='server'></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" border="0" width="98%">
        <tr>
            <td>
                <asp:DataGrid ID="dgEa_DefineMainPage" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCommand="dgEa_DefineMainPage_ItemCommand" OnItemCreated="dgEa_DefineMainPage_ItemCreated"
                    OnItemDataBound="dgEa_DefineMainPage_ItemDataBound">
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
                        <asp:BoundColumn DataField='iOrder' HeaderText='序号'></asp:BoundColumn>
                        <asp:BoundColumn DataField='LeftOrRight' HeaderText='默认位置'><ItemStyle HorizontalAlign="Left" /></asp:BoundColumn>
                        <asp:BoundColumn DataField='DefaultVisible' HeaderText='默认显示'><ItemStyle HorizontalAlign="Left" /></asp:BoundColumn>
                        <asp:BoundColumn DataField='Title' HeaderText='标题'><ItemStyle HorizontalAlign="Left" /></asp:BoundColumn>
                        <asp:BoundColumn DataField='Url' HeaderText='Url'><ItemStyle HorizontalAlign="Left" /></asp:BoundColumn>
                        <asp:BoundColumn DataField='IframeHeight' HeaderText='高度'></asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="修改">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" runat="server"  SkinID="btnClass1" Text="修改" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="44"></HeaderStyle>
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
