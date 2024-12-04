<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="frm_EmailIssueEdit.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frm_EmailIssueEdit"
    Title="模板信息编辑" %>
   
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/CustSchemeCtr.ascx" TagName="CustSchemeCtr" TagPrefix="uc3" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc4" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc5" %>
<%@ Register src="../Controls/CtrFlowNumeric.ascx" tagname="CtrFlowNumeric" tagprefix="uc7" %>
<%@ Register src="../Controls/UserPickerMult.ascx" tagname="UserPickerMult" tagprefix="uc9" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 98%" class="listContent" cellpadding="2" cellspacing="0" runat="server" id="Table2">
        <tr id="ShowRefUser" runat="server">
            <td class="listTitle" align="right" style="width: 12%; height: 24px;">
                <asp:Literal ID="LitRuleName" runat="server" Text="保障邮件"></asp:Literal>
            </td>
            <td class="list" align="left" style="height: 24px" colspan="3">
                <asp:Label ID="Lb_FromEmail" runat="server" Text=""></asp:Label>
            </td>            
        </tr>        
        <tr>
            <td class="listTitle" align="right">
                <asp:Literal ID="Literal2" runat="server" Text="邮件标题"></asp:Literal>
            </td>
            <td class="list" align="left" colspan="3">
                <asp:Label ID="lb_EmailTitle" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitle" align="right">
                <asp:Literal ID="Literal1" runat="server" Text="邮件内容"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:Label ID="lb_Emailcontent" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="list" align="center" colspan=4> 
                <asp:RadioButtonList ID="radioFlowModel" runat="server" RepeatColumns="3">
                </asp:RadioButtonList>    
            </td>
            
        </tr>
        <tr>
            <td class="list" align="center" colspan=4> 
                <asp:Button ID="Btn_baoZhang" runat="server" Text="确认报障" 
                    onclick="Btn_baoZhang_Click" />
            </td>
        </tr>
        
        
        
    </table>
</asp:Content>
