<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrMonitor.ascx.cs" Inherits="Epower.ITSM.Web.Controls.CtrMonitor" %>
<%@ Register Src="ctrdateandtime.ascx" TagName="ctrdateandtime" TagPrefix="uc1" %>
<asp:Literal ID="ltlHFList" runat="server"></asp:Literal>
<asp:textbox id="txtMonitor" runat="server" Rows="5" Width="90%" onblur="MaxLength(this,250,'�������ݳ��ȳ����޶�����:');" TextMode="MultiLine"></asp:textbox>
<br />
<asp:button id="cmdMonitor" runat="server" Text="ȷ��" onclick="cmdMonitor_Click" CssClass="btnClass"></asp:button>