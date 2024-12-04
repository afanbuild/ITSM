<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrdateandtimeNew.ascx.cs" Inherits="Epower.ITSM.Web.Controls.ctrdateandtimeNEW" %>
<script language="javascript" type="text/jscript" src="../Controls/time/My97DatePicker/WdatePicker.js"></script>
<asp:label id="labDate" Visible="False" runat="server"></asp:label>
<asp:textbox id="txtDate" runat="server" MaxLength="20" style="background-image:url(../Controls/time/My97DatePicker/skin/datePicker.gif);background-position:right;background-repeat:no-repeat;"></asp:textbox>
<asp:DropDownList id="ddlHours" runat="server" Width="48px"></asp:DropDownList>
<asp:DropDownList id="ddlMinutes" runat="server" Width="48px"></asp:DropDownList>&nbsp;<asp:Label ID="rWarning" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Red">*</asp:Label>
<input type="hidden" id="hidY" runat="server" />
