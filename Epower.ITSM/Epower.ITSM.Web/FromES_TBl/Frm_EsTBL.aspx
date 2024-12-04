<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Frm_EsTBL.aspx.cs" Inherits="Epower.ITSM.Web.FromES_TBl.Frm_EsTBL" Title="无标题页" %>
<%@ Register src="../Controls/CtrFlowFormText.ascx" tagname="CtrFlowFormText" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table class="listContent" width="100%" align="center" runat="server" id="Table2">
        <tr>
            <td  align="right" class="listTitle" style="width: 50%">配置名称：</td>
            <td  class="list">
                <uc1:CtrFlowFormText ID="txt_TblName" runat="server" MustInput="true" TextToolTip="配置名称" />
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
