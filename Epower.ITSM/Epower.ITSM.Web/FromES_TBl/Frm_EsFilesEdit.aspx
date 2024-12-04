<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Frm_EsFilesEdit.aspx.cs" Inherits="Epower.ITSM.Web.FromES_TBl.Frm_EsFilesEdit" Title="无标题页" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register src="../Controls/CtrFlowFormText.ascx" tagname="CtrFlowFormText" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table class="listContent" width="100%" align="center" runat="server" id="Table2">
       <tr>        
            <td  align="right" class="listTitle" style="width: 50%">配置项名称：</td>
            <td  class="list">
                <asp:DropDownList ID="Dop_tblName" runat="server">
                </asp:DropDownList>
                <asp:Label ID="lab_tblName" runat="server" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>        
            <td  align="right" class="listTitle" style="width: 50%">配置项名称：</td>
            <td  class="list">
                <uc1:CtrFlowFormText ID="txt_TblFeildName" runat="server" MustInput="true" TextToolTip="配置项目名称" />
            </td>           
        </tr>        
        <tr>            
            <td  class="list" colspan="2" align=center>                
                <asp:Button ID="btn_Save" runat="server" Text="保存" onclick="btn_Save_Click" />&nbsp;
                <asp:Button ID="Btn_return" runat="server" Text="返回" 
                    onclick="Btn_return_Click"  />
            </td>           
        </tr>    
 </table>
</asp:Content>
