<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_ExtensionsEdit.aspx.cs" 
    Inherits="Epower.ITSM.Web.EquipmentManager.frm_ExtensionsEdit"
    Title="扩展项编辑" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
     function CheckSchemaValuesStatus()
            {
                    var id= document.all.<%=ddltitemType.ClientID%>.id;
                    var ddl=document.getElementById(id);      
                    var trValues = document.getElementById("SchemaItemsValues");
                
                     if(ddl.selectedIndex == 0)
                     {
                         trValues.style.display = "none";
                         document.getElementById("drCatalog").style.display = "none";
                     }
                     else if(ddl.selectedIndex == 3)
                     {
                        document.getElementById("drCatalog").style.display = "";
                     }
                     else
                     {
                        document.getElementById("drCatalog").style.display = "none";
                        trValues.style.display = "none";
                     } 	            
            }
        
    </script>

<input id="hidID" runat="server" type="hidden" />
    <table style='width: 98%' class='listContent'>
        <tr style="display: none;">
            <td class='listTitle' align='right' style='width: 30%;'>
                属性代码
            </td>
            <td class='list'>
                <asp:TextBox ID='txtFieldID' runat='server' MaxLength="50" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class='listTitle' align='right' style='width: 30%;'>
                属性名称
            </td>
            <td class='list'>
                <uc1:CtrFlowFormText ID="CtrFTCHName" runat="server" MustInput="true" TextToolTip="属性名称" />
            </td>
        </tr>
        <tr>
            <td class='listTitle' align='right' style='width: 30%;'>
                所属分组
            </td>
            <td class='list'>
                <asp:DropDownList ID="ddlType" runat="server">
                    <asp:ListItem Value="1026">事件单</asp:ListItem>
                    <asp:ListItem Value="210">问题单</asp:ListItem>
                    <asp:ListItem Value="420">变更单</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class='listTitle' align='right' style='width: 30%;'>
                类型
            </td>
            <td class='list'>
                <asp:DropDownList ID="ddltitemType" runat="server">
                    <asp:ListItem Value="0">基础信息</asp:ListItem>
                    <asp:ListItem Value="1">关联配置</asp:ListItem>
                    <asp:ListItem Value="2">备注信息</asp:ListItem>
                    <asp:ListItem Value="3">下拉选择</asp:ListItem>
                    <asp:ListItem Value="4">部门信息</asp:ListItem>
                    <asp:ListItem Value="5">用户信息</asp:ListItem>
                    <asp:ListItem Value="6">日期类型</asp:ListItem>
                    <asp:ListItem Value="7">数值类型</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="drCatalog" style="display: none;">
            <td class='listTitle' align='right' style='width: 30%;'>
                分类
            </td>
            <td class='list'>
                <uc2:ctrFlowCataDropList ID="ctrFlowCataDropDefault" runat="server" RootID="1" />
            </td>
        </tr>
        <tr id="SchemaItemsValues" style="display: none">
            <td align="right" class="listTitle" style="width: 30%">
                属性值
            </td>
            <td class="list">
                <select id="lstValues" multiple="multiple" name="lstSelect" ondblclick="RemoveItem();"
                    style="width: 50%; height: 208px">
                </select>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
</asp:Content>
