<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="PrintRuleList.aspx.cs" Inherits="Epower.ITSM.Web.Print.PrintRuleList" %>

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
    <table width="98%" class="listContent GridTable" cellspacing="0" cellpadding="2">
        <tr>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitRuleName" runat="server" Text="规则名称"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:TextBox ID="txtRuleName" runat="server" MaxLength="50"></asp:TextBox>
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitStatus" runat="server" Text="是否有效"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="ddlStatus" runat="server" Width="152px">
                    <asp:ListItem Text="" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="禁用" Value="0"></asp:ListItem>
                    <asp:ListItem Text="启用" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitSystemName" runat="server" Text="应用名称"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="cboApp" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboApp_SelectedIndexChanged"
                    Width="152px">
                </asp:DropDownList>
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitModelName" runat="server" Text="流程模型名称"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="cboFlowModel" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgMailMessageTem" runat="server" Width="100%" CellPadding="1" CellSpacing="2"
                    BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgMailMessageTem_ItemCommand"
                    OnItemDataBound="dgMailMessageTem_ItemDataBound" OnItemCreated="dgMailMessageTem_ItemCreated">
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
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="False"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="规则名称">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lbRuleName" Text='<%#DataBinder.Eval(Container, "DataItem.PrintRuleName")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='AppNames' HeaderText='应用名称'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='FlowModelName' HeaderText='流程模型名称'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="是否有效">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lbStatus" Text='<%#DataBinder.Eval(Container, "DataItem.IsOpen")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Remark" HeaderText="备注">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="修改">
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
                <uc4:ControlPageFoot ID="CtrcpfMailMessage" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
