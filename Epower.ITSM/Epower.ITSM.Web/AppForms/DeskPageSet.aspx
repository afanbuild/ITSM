<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="DeskPageSet.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.DeskPageSet" Title="所有功能设置" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="Table2" style="width: 100%" align="center" class="listContent" runat="server">
        <tr>
            <td align="left" class="listTitle" nowrap="nowrap" style="width: 33%">
                功能名称
            </td>
            <td class="list" colspan="3" style="width: 66%">
                <asp:Label ID="PageName" runat="server" Text=""></asp:Label>
            </td>            
        </tr>
        <tr>
            <td class="list" align="center"  nowrap="nowrap" colspan="4" >
                <asp:Button ID="btnPageAll" runat="server" Text="设为全局" 
                    onclick="btnPageAll_Click"  />&nbsp;       
                <asp:Button ID="btnOftenPage" runat="server" Text="设为个人" 
                    onclick="btnOftenPage_Click"  />&nbsp;         
                <asp:Button ID="btnConent" runat="server" Text="取消设置" 
                    onclick="btnConent_Click"/>
            </td>                                    
        </tr>
</table>
</asp:Content>
