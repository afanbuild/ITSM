<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.CtrImportance" Codebehind="CtrImportance.ascx.cs" %>


<asp:radiobuttonlist id=rdoImportance Font-Size="Smaller" Width="184px" runat="server" RepeatDirection="Horizontal">
<asp:ListItem Value="2"> 重要</asp:ListItem>
<asp:ListItem Value="1" Selected="True"> 一般</asp:ListItem>
<asp:ListItem Value="0"> 较低</asp:ListItem>
</asp:radiobuttonlist>
