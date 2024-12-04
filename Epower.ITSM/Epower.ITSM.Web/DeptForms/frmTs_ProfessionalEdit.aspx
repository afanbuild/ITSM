<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmTs_ProfessionalEdit.aspx.cs" Inherits="Epower.ITSM.Web.DeptForms.frmTs_ProfessionalEdit" Title="无标题页" %>
<%@ Register TagPrefix="uc2" TagName="ctrdateandtime" Src="../Controls/CtrDateAndTimeV2.ascx" %>
<%@ Register TagPrefix="uc3" TagName="CtrFlowFormText" Src="../Controls/CtrFlowFormText.ascx" %>
<%@ Register TagPrefix="uc4" TagName="ctrFlowCataDropList" Src="../Controls/ctrFlowCataDropList.ascx" %>
<%@ Register TagPrefix="uc5" TagName="CtrFlowNumeric" Src="../Controls/CtrFlowNumeric.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table style='width:100%' class='listContent'>
<tr>
	<td class='listTitle'  align='right' style='width:30%;'>
	专业编号		
</td>		
<td class='list'>	
    <uc5:CtrFlowNumeric ID="CtrFlowPID" runat="server" TextToolTip='专业编号' MustInput='true' MaxLength="6" />
</td>		
</tr>
	<tr>
	<td class='listTitle'  align='right' style='width:30%;'>
	专业名称		
</td>		
<td class='list'>
	<uc3:CtrFlowFormText ID='CtrFlowPName' runat='server' TextToolTip='专业名称' MustInput='true' MaxLength=50 />		
</td>		
</tr>
</table>
</asp:Content>