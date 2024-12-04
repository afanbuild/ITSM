<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmBr_SchemaItemsEdit.aspx.cs" Inherits="Epower.ITSM.Web.CustManager.frmBr_SchemaItemsEdit"
    Title="客户扩展项编辑" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
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
                     }
                     else
                     {
                        trValues.style.display = "none";
                     }
                    //alert(ddl.options[ddl.selectedIndex].text.trim());
	                
	            
            }
    </script>

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
                类型
            </td>
            <td class='list'>
                <asp:DropDownList ID="ddltitemType" runat="server">
                    <asp:ListItem Value="0">基础属性</asp:ListItem>
                    <asp:ListItem Value="1">关联属性</asp:ListItem>
                    <asp:ListItem Value="2">备注属性</asp:ListItem>
                </asp:DropDownList>
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
