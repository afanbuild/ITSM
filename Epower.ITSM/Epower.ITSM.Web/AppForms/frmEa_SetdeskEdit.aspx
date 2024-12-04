<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmEa_SetdeskEdit.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmEa_SetdeskEdit" Title="服务台编辑" %>

<%@ Register src="../Controls/UserPicker.ascx" tagname="UserPicker" tagprefix="uc2" %>
<%@ Register TagPrefix="uc3" TagName="CtrFlowFormText" Src="../Controls/CtrFlowFormText.ascx" %>
<%@ Register TagPrefix="uc4" TagName="ctrFlowCataDropList" Src="../Controls/ctrFlowCataDropList.ascx" %>
<%@ Register TagPrefix="uc5" TagName="CtrFlowNumeric" Src="../Controls/CtrFlowNumeric.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table cellpadding='1' cellspacing='2' width='98%' border='0' class='listContent'>
<tr>
    <td class='listTitleRight' style='width:12%;'>
	    用户名称				
    </td>		
    <td class='list' style='width:35%;'>	    			
        <uc2:UserPicker ID="UserPicker1" runat="server" MustInput="true" TextToolTip="用户名称" />	    			
    </td>		
	<td class='listTitleRight' style='width:15%;'>
	    座席（对应呼叫中心座席）				
    </td>		
    <td class='list'  style='width:35%;'>
	    <uc3:CtrFlowFormText ID="CtrFlowBlockRoom" MustInput="true" runat="server" TextToolTip="座席（对应呼叫中心座席）" />	    			
    </td>				
</tr>
</table>
</asp:Content>
