<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrDateAndTimeV2.ascx.cs" Inherits="Epower.ITSM.Web.Controls.CtrDateAndTimeV2" %>

		
<asp:label id="labDate" Visible="False" runat="server"></asp:label>
<asp:textbox id="txtDate" runat="server" MaxLength="20" 
    style="background-image:url(../Controls/time/My97DatePicker/skin/datePicker.gif);background-position:right;background-repeat:no-repeat;"></asp:textbox>
<asp:Label ID="rWarning" runat="server" Style="margin-left:7px;" Font-Bold="false" Font-Size="Small" ForeColor="Red">*</asp:Label>
<input type="hidden" id="hidY" runat="server" />