<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_ExtensionDisplayWayEdit.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frm_ExtensionDisplayWayEdit"
    Title="扩展项显示方式编辑" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding='2' cellspacing='0' width='98%' border='0' class='listContent Gridtable' style="display:none;">
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
                <asp:DropDownList ID="ddlNodeModel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNodeModel_SelectedIndexChanged">
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
                                width="16" align="absbottom" />显示方式设置
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 98%" align="center" id="Table1">
        <tr>
            <td>
                <asp:DataGrid ID="dgExtensionDisplayWay" runat="server" Width="100%" CellPadding="1"
                    CellSpacing="2" BorderWidth="0px" AutoGenerateColumns="False" CssClass="Gridtable"
                    OnItemDataBound="dgExtensionDisplayWay_ItemDataBound">
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="FieldID" Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CHName" HeaderText="扩展项名"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TypeName" HeaderText="类型"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="可见">
                            <HeaderTemplate>
                                可见(<input type="checkbox" class="btnClass1" onclick="check_all(1, this)" value="全选"/>)
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="visible">
                                    <asp:CheckBox ID="chkVisible" runat="server" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="可编辑">
                            <HeaderTemplate>
                                可编辑(<input type="checkbox" class="btnClass1" onclick="check_all(2, this)" value="全选"/>)
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="edit">
                                    <asp:CheckBox ID="chkEdit" runat="server" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">        
        function check_all(coltype, obj) {            
            var container;
            var checked = $(obj).attr('checked'); 
                        
            if (coltype == 1) {                
                container = $('.visible > input');
            } else {
                container = $('.edit > input');
            }
            
            if (checked) {
                container.attr('checked', 'checked');
            } else { container.removeAttr('checked'); }
        }
        
        $(document).ready(function(){
            $('#ctrtabbuttons > tbody > tr:first td:first').hide();
        });
    </script>

</asp:Content>
