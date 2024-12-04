<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEqu_Insert.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_Insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="listContent" width="98%">
        <tr>
            <td class="listTitleRight" style="width: 12%">
                导入资产
            </td>
            <td class="list">
                <asp:FileUpload ID="FileUpload1" runat="server" />
                <asp:Button ID="btnImportEqu" runat="server" Text="确认导入" OnClick="btnImportEqu_Click" />
            </td>
        </tr>
        <tr id="tdEquErrorList" runat="server" visible="false">
            <td class="listTitleRight" style="width: 12%">
                失败记录
            </td>
            <td class="list">                <asp:Label
                    ID="labEqu" runat="server" Text="Label" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table class="listContent" width="98%">
        <tr>
            <td class="listTitleRight" style="width: 12%">
                导入客户
            </td>
            <td class="list">
                <asp:FileUpload ID="FileUpload2" runat="server" />
                <asp:Button ID="btnImportCust" runat="server" Text="确认导入" OnClick="btnImportCust_Click" />
            </td>
        </tr>
        
        <tr id="trCustomerErrorList" runat="server" visible="false">
            <td class="listTitleRight" style="width: 12%">
                失败记录
            </td>
            <td class="list" style="color:Red;">
                <asp:Literal ID="literalCustomerErrorList" runat="server">
                    
                </asp:Literal>
            </td>
        </tr>        
    </table>
    <br />
    <table>
        <tr>
            <td align="right" class="listTitle" style="height: 21px">
                <asp:Label ID="Label5" runat="server" Text="模板下载："></asp:Label>
            </td>
            <td class="list" style="height: 21px">
                <asp:LinkButton ID="lnkCust" runat="server" OnClick="lnkCust_Click" CausesValidation="false">客户资料导入模板下载</asp:LinkButton>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                <asp:LinkButton ID="lnkEqu" runat="server" OnClick="lnkEqu_Click" CausesValidation="false">资产资料导入模板下载</asp:LinkButton>
            </td>
        </tr>
    </table>
</asp:Content>
