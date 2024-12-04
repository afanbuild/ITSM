<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrFlowCataCheckBox.ascx.cs" Inherits="Epower.ITSM.Web.Controls.ctrFlowCataCheckBox" %>

<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td>            
            <asp:CheckBoxList ID="chkBoxList" runat="server" RepeatColumns="3">
            </asp:CheckBoxList>            
        </td>
    </tr>
</table>
<input id="HidRootID" runat="server" type="hidden" value="0" />
<input id="hidCatelogValue" runat="server" type="hidden" value="" />

