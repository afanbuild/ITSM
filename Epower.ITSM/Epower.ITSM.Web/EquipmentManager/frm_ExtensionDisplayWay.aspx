<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" 
AutoEventWireup="true" CodeBehind="frm_ExtensionDisplayWay.aspx.cs" 
Inherits="Epower.ITSM.Web.EquipmentManager.frm_ExtensionDisplayWay" Title="扩展项显示方式管理" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table cellpadding='2' cellspacing='0' width='98%' border='0' class='listContent Gridtable'>
        <tr>
            <td class='listTitleRight' style="width: 12%">
                应用
            </td>
            <td class='list' style="width: 35%">
                <asp:DropDownList ID="ddlApp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlApp_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class='listTitleRight' style="width: 12%">
                流程模型
            </td>
            <td class='list' style="width: 35%">
                <asp:DropDownList ID="ddlFlowModel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFlowModel_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight' style="width: 12%">
                环节模型
            </td>
            <td class='list' style="width: 35%" colspan="3">
                <asp:DropDownList ID="ddlNodeModel" runat="server">
                </asp:DropDownList>
                
                <asp:TextBox ID="txtSearchKey" runat="server" Visible="false"></asp:TextBox>
            </td>        
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0" class="Gridtable">
        <tr>
            <td>
                <asp:DataGrid ID="dgExNodeModelList" runat="server" Width="100%" CellPadding="1" CellSpacing="2"
                    BorderWidth="0px" AutoGenerateColumns="False"
                    OnItemCommand="dgExNodeModelList_ItemCommand"
                    >
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitleRight"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" Visible="false">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%" HorizontalAlign="Center"></HeaderStyle>
                        </asp:TemplateColumn>

                        <asp:BoundColumn DataField='flowmodelid' Visible="false">                            
                        </asp:BoundColumn>                        
                        <asp:BoundColumn DataField='nodemodelid' Visible="false">                            
                        </asp:BoundColumn>
                                                                    
                        <asp:BoundColumn DataField='appname' HeaderText='应用名' ItemStyle-Width="15%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='flowmodelname' HeaderText='流程模型名' ItemStyle-Width="15%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='nodename' HeaderText='环节模型名' ItemStyle-Width="15%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="查看" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Button ID="lnkSelect" SkinID="btnClass1" runat="server" Text="查看" CommandName="view"  CommandArgument='<%#Eval("AppID") + "|" + Eval("FlowModelID") + "|" + Eval("NodeModelID")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="修改" Visible="false" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Button ID="lnkModify" SkinID="btnClass1" runat="server" Text="修改" CommandName="modify" CommandArgument='<%#Eval("AppID") + "|" + Eval("FlowModelID") + "|" + Eval("NodeModelID")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="删除" Visible="false" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Button ID="lnkDelete" SkinID="btnClass1" runat="server" Text="删除" CommandName="delete" CommandArgument='<%#Eval("AppID") + "|" + Eval("FlowModelID") + "|" + Eval("NodeModelID")%>' OnClientClick="return confirm('确认删除吗?');" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    
    
    <script type="text/javascript">
        //全选复选框
        function checkAll(checkAll) {
            var len = document.forms[0].elements.length;
            var cbCount = 0;
            for (i = 0; i < len; i++) {
                if (document.forms[0].elements[i].type == "checkbox") {
                    if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgExNodeModelList") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                        document.forms[0].elements[i].checked = checkAll.checked;

                        cbCount += 1;
                    }
                }
            }
        }    
    </script>

</asp:Content>
