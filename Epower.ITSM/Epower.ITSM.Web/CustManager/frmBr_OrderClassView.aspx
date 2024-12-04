<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmBr_OrderClassView.aspx.cs" Inherits="Epower.ITSM.Web.CustManager.frmBr_OrderClassView"
    Title="排班表查询" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="uc4" TagName="ControlPageFoot" Src="../Controls/ControlPageFoot.ascx" %>
<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc1" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc2" %>
<%@ Register Src="../Controls/UserPickerMult.ascx" TagName="UserPickerMult" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding='2' cellspacing='0' width='98%' class='listContent'>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                值班人
            </td>
            <td class='list'>
                <uc3:UserPickerMult ID="UserPickerMult1" runat="server" />
            </td>
            <td class='listTitle' style='width: 12%;'>
                值班日期
            </td>
            <td class='list'>
                <uc1:ctrDateSelectTime ID="ctrDateSelectTime1" runat="server" />                
            </td>            
        </tr>
    </table>
    <br />
    <div id="mainDiv" runat="server" style="width: 98%;">
    </div>
    <br /><br />
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgBr_OrderClassType" runat="server" CssClass="GridTable" Width="100%" OnItemDataBound="dgBr_OrderClassType_ItemDataBound"
                    OnItemCreated="dgBr_OrderClassType_ItemCreated">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField='ClassTypeName' HeaderText='班次名称'></asp:BoundColumn>
                        <asp:BoundColumn DataField='Remark' HeaderText='班次说明'></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc4:ControlPageFoot ID="cpBr_OrderClassType" runat="server" />
            </td>
        </tr>
    </table>
    
<script language="javascript" type="text/javascript">

function btnClassManager_Click() {
    window.open("frmBr_OrderClassMain.aspx","MainFrame");
}

</script>

</asp:Content>
