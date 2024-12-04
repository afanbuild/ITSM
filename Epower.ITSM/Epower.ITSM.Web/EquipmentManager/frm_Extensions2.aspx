<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_Extensions2.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frm_Extensions2"
    Title="流程自定义扩展项配置管理" %>

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
                <asp:DropDownList ID="ddlFlowModel" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight' style="width: 12%">
                关键词
            </td>
            <td class='list' style="width: 35%" colspan="3">
                <asp:TextBox ID="txtSearchKey" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0" class="Gridtable">
        <tr>
            <td>
                <asp:DataGrid ID="dgExFlowModelList" runat="server" Width="100%" CellPadding="1" CellSpacing="2"
                    BorderWidth="0px" AutoGenerateColumns="False"
                    OnItemCommand="dgExFlowModelList_ItemCommand"
                    >
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitleRight"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField='appname' HeaderText='应用名' ItemStyle-Width="15%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='flowmodelname' HeaderText='流程模型名' ItemStyle-Width="15%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='extcount' HeaderText='扩展项数' ItemStyle-Width="5%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="可见可编辑" ItemStyle-Width="15%">
                            <ItemTemplate>                                                                
                                <input type="button"  value="设置" onclick='open_view_chart(<%#Eval("AppID")%>,<%#Eval("FlowModelID") %>)' />
                                <input type="button"  value="初始化" onclick='open_view_displaystatus(<%#Eval("AppID")%>,<%#Eval("FlowModelID") %>)' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="查看" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Button ID="lnkSelect" SkinID="btnClass1" runat="server" Text="查看" CommandName="view"  CommandArgument='<%#Eval("AppID") + "|" + Eval("FlowModelID") %>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="修改" Visible="false" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Button ID="lnkModify" SkinID="btnClass1" runat="server" Text="修改" CommandName="modify" CommandArgument='<%#Eval("AppID") + "|" + Eval("FlowModelID") %>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="删除" Visible="false" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Button ID="lnkDelete" SkinID="btnClass1" runat="server" Text="删除" CommandName="delete" CommandArgument='<%#Eval("FlowModelID") %>' OnClientClick="return confirm('确认删除吗?');" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    

<script type="text/javascript" src="../js/epower.base.js"></script>

<script type="text/javascript" language="javascript">

    function open_view_chart(appid, flowmodelid) {                
        var urlparam = '?appid='+appid+'&flowmodelid=' + flowmodelid;
        var url;
        
        var pos = epower.tools.computeXY('center', window, 650,470);        
        
        if (!$.browser.msie) {
            url = '../forms/flow_view_exitem_chart_SVG.aspx' + urlparam;            
        } else {
            url = '../forms/flow_view_exitem_chart.aspx' + urlparam;            
        }
        
        epower.tools.open(url,null,{width:650,height:470,top:pos.y,left:pos.x});
    }
    
    function open_view_displaystatus(appid, flowmodelid) {
        var pos = epower.tools.computeXY('center', window, 650,470);        
        
        url = "../EquipmentManager/frm_ExtensionDisplayWayEdit.aspx?appid="+ appid +"&flowmodelid="+flowmodelid+"&nodemodelid=2&action=init_all";
        epower.tools.open(url,null,{width:650,height:470,top:pos.y,left:pos.x});
    }

</script>
    
</asp:Content>
