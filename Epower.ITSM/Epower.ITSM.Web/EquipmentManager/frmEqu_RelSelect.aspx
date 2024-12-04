<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True"
    CodeBehind="frmEqu_RelSelect.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_RelSelect"
    Title="资产选择" %>

<%@ Register Src="../Controls/ctrEquCataDropList.ascx" TagName="ctrEquCataDropList"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrTextDropList.ascx" TagName="CtrTextDropList" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc4" %>
<%@ Register Src="../Controls/BussinessControls/CustomCtr.ascx" TagName="CustomCtr"
    TagPrefix="uc5" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
function ServerOndblclick(value1)
{
        var arr = new Array();        
        arr[0] =value1;      

        window.parent.returnValue = arr;
        top.close();
    }
    
    function ChangeSel()
    {
        //资产类别改变时，更改
        document.all.<%=hidBtnOK.ClientID %>.click();
    }
    
    </script>
    <div style="display:none">
    <asp:Button ID="hidBtnOK" runat="server" OnClick="BtnOK_Click"/>
    </div>
    <table cellpadding='1' cellspacing='2' width='98%' border='0' class="listContent">
        <tr>
            <td class='listTitle' align='right' style='width: 12%;'>
                <asp:Literal ID="LitEquDeskName" runat="server" Text="名称"></asp:Literal>
            </td>
            <td class='list' style="width: 35%">
                <asp:TextBox ID='txtName' runat='server'></asp:TextBox>
            </td>
            <td class='listTitle' align='right' style='width: 12%;'>
                <asp:Literal ID="LitEquDeskCode" runat="server" Text="资产编号"></asp:Literal>
            </td>
            <td class='list'>
                <asp:TextBox ID='txtCode' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr>            
            <td class='listTitle' align='right'>
                <asp:Literal ID="LitEquDeskEquStatus" runat="server" Text="资产状态"></asp:Literal>
            </td>
            <td class='list'>
                <uc6:ctrFlowCataDropList ID="ctrCataEquStatus" runat="server" RootID="1018" />
            </td>
            <td align="right" class="listTitle">
                所属客户
            </td>
            <td class="list">
                <asp:TextBox ID='txtCust' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr>            
            <td class='listTitle' align='right'>
                <asp:Literal ID="LitEquDeskType" runat="server" Text="资产类别"></asp:Literal>
            </td>
            <td class='list'>
                <uc2:ctrEquCataDropList ID="CtrEquCataDropList1" runat="server" RootID="1" OnChangeScript="ChangeSel();"/>
                <asp:CheckBox ID="chkIncludeSub" Text="包含子类" Checked="true" runat="server" Visible="false"/>
            </td>
            <td align="right" class="listTitle">
                基本配置项
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlSchemaItemJB" runat="server" Width="52%">
                </asp:DropDownList>
                <asp:TextBox ID="txtItemValue" runat="server"></asp:TextBox>
            </td>            
        </tr>
        <tr style="display:none">
            <td align="right" class="listTitle" style="width: 15%">
                关联配置项
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlSchemaItemGL" runat="server" Width="50%">
                </asp:DropDownList>
                <asp:CheckBox ID="chkItemValue" Text="是否配置" Checked="true" runat="server" />
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding='1' cellspacing='2' width='98%' border='0' class="listContent">
        <tr>
            <td align="center" class="listTitleNoAlign">
                <asp:Button ID="btnConfirm" runat="server" Text="确定" OnClick="btnConfirm_Click"
                    CssClass="btnClass" />
                &nbsp;
                <asp:Button ID="btnClose" runat="server" Text="取消"  OnClick="btnClose_Click"
                    CssClass="btnClass" />
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td align="center" class="listContent">
                <asp:DataGrid ID="dgEqu_Desk" runat="server" Width="100%" CellPadding="1" CellSpacing="2"
                    BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCreated="dgEqu_Desk_ItemCreated"
                    OnItemDataBound="dgEqu_Desk_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn>
                        <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Name' HeaderText='资产名称'><ItemStyle HorizontalAlign="Left" /></asp:BoundColumn>
                        <asp:BoundColumn DataField='Code' HeaderText='资产编号'><ItemStyle HorizontalAlign="Left" /></asp:BoundColumn>
                        <asp:BoundColumn DataField='costomname' HeaderText='所属客户'><ItemStyle HorizontalAlign="Left" /></asp:BoundColumn>
                        <asp:BoundColumn DataField='SerialNumber' HeaderText='SN' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='CatalogName' HeaderText='分类' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Positions' HeaderText='位置' Visible="false"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitle">
                <uc4:ControlPageFoot ID="cpfECustomerInfo" runat="server" />
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">
        function checkAll(checkAll) {
            var len = document.forms[0].elements.length;
            var cbCount = 0;
            for (i = 0; i < len; i++) {
                if (document.forms[0].elements[i].type == "checkbox") {
                    if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgEqu_Desk") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                        document.forms[0].elements[i].checked = checkAll.checked;
                        cbCount += 1;
                    }
                }
            }
        } 
    </script>
</asp:Content>
