<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_EmailIssue.aspx.cs" Inherits="Epower.ITSM.Web.Print.frm_EmailIssue" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc4" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc1" %>
<%@ Register Src="../Controls/DeptPicker.ascx" TagName="DeptPicker" TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>

    <script type="text/javascript">
        function checkAll(checkAll) {
            var len = document.forms[0].elements.length;
            var cbCount = 0;
            for (i = 0; i < len; i++) {
                if (document.forms[0].elements[i].type == "checkbox") {
                    if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgMailMessageTem") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                        document.forms[0].elements[i].checked = checkAll.checked;

                        cbCount += 1;
                    }
                }
            }
        }       
        function ShowDetailsInfo(obj, id) {
            //$("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "frmBr_CustomXmlHttp.aspx?ZHServiceDP=" + id }).responseText; } });
        }
    </script>

    <style>
        #tooltip
        {
            position: absolute;
            z-index: 3000;
            border: 1px solid #111;
            background-color: #eee;
            padding: 5px;
            opacity: 0.85;
        }
        #tooltip h3, #tooltip div
        {
            margin: 0;
        }
        .style1
        {
            background-color: #ffffff;
            text-align: left;
            width: 512px;
        }
    </style>
    <input id="hidTable" value="" runat="server" type="hidden" />
    <table width="98%" class="listContent" cellpadding="2" cellspacing="0">
        <tr>
            <td class="listTitle" style="width: 10%; text-align: right;">
                <asp:Literal ID="LitRuleName" runat="server" Text="保障邮件"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtFromEmail" runat="server" MaxLength="50"></asp:TextBox>
            </td>
            <td class="listTitle" style="width: 10%; text-align: right;">
                <asp:Literal ID="LitStatus" runat="server" Text="邮件标题"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtEmailTitle" runat="server" MaxLength="50"></asp:TextBox>
            </td>
            <td class="listTitle" style="width: 10%; text-align: right;">
                <asp:Literal ID="Literal1" runat="server" Text="保障状态"></asp:Literal>
            </td>
            <td class="list">
                  <asp:DropDownList ID="ddlStatus" runat="server" Width="50px">                    
                    <asp:ListItem Text="未报障" Value="0"></asp:ListItem>
                    <asp:ListItem Text="已报障" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </td>
            
        </tr>        
    </table>
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td align="center">
                <asp:DataGrid ID="dgMailMessageTem" runat="server" Width="100%" CellPadding="1" CellSpacing="2"
                    BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgMailMessageTem_ItemCommand"
                    OnItemDataBound="dgMailMessageTem_ItemDataBound" OnItemCreated="dgMailMessageTem_ItemCreated">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="False"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="保障邮件">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lbRuleName" Text='<%#DataBinder.Eval(Container, "DataItem.FromEmail")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='EmailTitle' HeaderText='邮件标题'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='EmailContant' HeaderText='邮件内容'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn> 
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" CssClass="btnClass" runat="server" Text="查看" CommandName="edit" />
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
                <uc4:ControlPageFoot ID="CtrcpfMailMessage" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
