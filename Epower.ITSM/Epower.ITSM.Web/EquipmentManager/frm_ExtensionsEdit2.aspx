<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_ExtensionsEdit2.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frm_ExtensionsEdit2"
    Title="流程自定义扩展项配置" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrFlowNumeric.ascx" TagName="CtrFlowNumeric" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ctrSetUserOtherRight.ascx" TagName="ctrSetUserOtherRight"
    TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc5" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="table1" cellpadding='2' cellspacing='0' width='98%' border='0' class='listContent GridTable'>
        <tr>
            <td class='listTitleRight' style="width: 8%">
                应用
            </td>
            <td class='list' style="width: 15%">
                <asp:DropDownList ID="ddlApp" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlApp_SelectedIndexChanged"> 
                </asp:DropDownList>
            </td>
            <td class='listTitleRight' style="width: 8%">
                流程模型
            </td>
            <td class='list' style="width: 35%">
                <asp:DropDownList ID="ddlFlowModel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFlowModel_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table id="table2" width="98%" align="center">
        <tr id="tr1" runat="server">
            <td valign="top" align="left" class="listTitleNew" colspan="2">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />配置项设置
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 98%" align="center" id="Table2">
        <tr>
            <td>
                <asp:DataGrid ID="dgExtensionItem" runat="server" Width="100%" CellPadding="1" CellSpacing="2"
                    BorderWidth="0px" AutoGenerateColumns="False" CssClass="Gridtable" OnItemCommand="dgExtensionItem_ItemCommand"
                    OnItemDataBound="dgExtensionItem_ItemDataBound">
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn HeaderText="ID" Visible="false">
                            <ItemTemplate>

                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="类别">
                            <ItemTemplate>
                            
                        <asp:TextBox ID="txtID" Text='' onblur="CheckDoubleID(this,'txtID');" Width="85%"
                                    runat="server" Style="display:none;"></asp:TextBox>
                            
                                &nbsp;<asp:DropDownList ID="ddlTypeName" runat="server" Width="95%" onchange="SwitchDefaultValueContainer(this);">
                                    <asp:ListItem>基础信息</asp:ListItem>
                                    <asp:ListItem>关联配置</asp:ListItem>
                                    <asp:ListItem>备注信息</asp:ListItem>
                                    <asp:ListItem>下拉选择</asp:ListItem>
                                    <asp:ListItem>部门信息</asp:ListItem>
                                    <asp:ListItem>用户信息</asp:ListItem>
                                    <asp:ListItem>日期类型</asp:ListItem>
                                    <asp:ListItem>数值类型</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="配置项名称">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCHName" Text='' Width="90%" runat="server"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Width="25%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="初值">
                            <ItemTemplate>
                                <asp:Panel ID="PanDefault" Style="display: none;" runat="server" Height="16px" Width="100px">
                                    <asp:CheckBox ID="chkDefault" Checked='' runat="server" /></asp:Panel>
                                <asp:Panel ID="PantxtDefault" Style="display: inherit;" runat="server" Height="24px"
                                    Width="100px">
                                    <asp:TextBox ID="txtDefault" Text='' Width="100%" runat="server"></asp:TextBox></asp:Panel>
                                <asp:Panel ID="PantxtMDefault" Style="display: none;" runat="server" Height="24px"
                                    Width="100px">
                                    <asp:TextBox ID="txtMDefault" Text='' Width="100%" runat="server" TextMode="MultiLine"></asp:TextBox></asp:Panel>
                                <asp:Panel ID="panDropDownList" Style="display: none;" runat="server" Height="24px"
                                    Width="100px">
                                    <uc1:ctrFlowCataDropList ID="ctrFlowCataDropDefault" runat="server" RootID="1" />
                                </asp:Panel>
                                <asp:Panel ID="panDept" Style="display: none;" runat="server" Height="24px" Width="100px">
                                    <asp:CheckBox ID="CheckDept" Checked='' runat="server" />所在部门</asp:Panel>
                                <asp:Panel ID="panUser" Style="display: none;" runat="server" Height="24px" Width="100px">
                                    <asp:CheckBox ID="CheckUser" Checked='' runat="server" />登录人</asp:Panel>
                                <asp:Panel ID="PanTime" Style="display: none;" runat="server" Height="24px" Width="100px">
                                    <asp:CheckBox ID="CheckTime" Checked='' runat="server" />当前日期
                                    <asp:CheckBox ID="CheckIsTime" Checked='' runat="server" />是否时间</asp:Panel>
                                <asp:Panel ID="PanNumber" Style="display: none;" runat="server" Height="24px" Width="100px">
                                    <uc3:CtrFlowNumeric ID="TextNumber" runat="server" Value='' />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="必填">
                            <HeaderStyle Width="65px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsMust" Checked='' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="查询">
                            <HeaderStyle Width="65px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsSelect" Checked='' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="组">
                            <ItemTemplate>
                                <asp:TextBox ID="txtGroup" Text='' Width="95%" runat="server"></asp:TextBox>
                                <asp:TextBox ID="TxtGroupID" Visible="false" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="排序">
                            <ItemTemplate>
                                <asp:TextBox ID="TxtOrderBy" Text='' Width="95%" runat="server"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="操作">
                            <ItemTemplate>
                                <asp:Button ID="lnkdelete" SkinID="btnClass1" runat="server" Text="删除" CommandName="delete"
                                    Visible="false" CausesValidation="false" />
                                <asp:Button ID="btnAddNew" SkinID="btnClass1" runat="server" Text="添加" CommandName="add"
                                    CausesValidation="false" />
                            </ItemTemplate>
                            <HeaderStyle Width="5%" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Default" HeaderText="初值" Visible="false"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">

    $(document).ready(function(){
        $('#<%=dgExtensionItem.ClientID %> > tbody > tr').each(function(idx){
            if (idx == 0) return true;
                        
            var selectObj = $(this).find('td:first > select')[0];            
            
            SwitchDefaultValueContainer(selectObj);
        });
    });
    
    function SwitchDefaultValueContainer(obj)
	{
        var defaultValContainer = $(obj).parent().parent().find('td:eq(2)');	    
        var selectedVal = obj.value;    // 选中的扩展项类型           
                
                
        var divlist = defaultValContainer.find('div');
        divlist.hide();
                   
       
        if (selectedVal == '基础信息') {
            divlist.eq(1).show();
        } else if (selectedVal == '关联配置') {
            divlist.eq(0).show();
        } else if (selectedVal == '备注信息') {
            divlist.eq(2).show();
        } else if (selectedVal == '下拉选择') {
            divlist.eq(3).show();
        } else if (selectedVal == '部门信息') {
            divlist.eq(4).show();
        } else if (selectedVal == '用户信息') {
            divlist.eq(5).show();
        } else if (selectedVal == '日期类型') {
            divlist.eq(6).show();
        } else if (selectedVal == '数值类型') {
            divlist.eq(7).show();
        }	
	}

    </script>

</asp:Content>
