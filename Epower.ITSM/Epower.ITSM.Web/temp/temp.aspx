<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="temp.aspx.cs" Inherits="Epower.ITSM.Web.temp.temp" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table cellpadding='2' cellspacing='0' width='98%' class='listContent'>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                导入部门
            </td>
            <td class='list'>
               
                <asp:FileUpload ID="fuDept" runat="server" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnImportDept" runat="server" Text="导入" 
                    onclick="btnImportDept_Click" />
               
            </td>
        </tr>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                导入用户
            </td>
            <td class='list'>
                
                <asp:FileUpload ID="fuUser" runat="server" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnImportUser" runat="server" Text="导入" 
                    onclick="btnImportUser_Click" />
                
            </td>        
        </tr>
    </table>
</asp:Content>
