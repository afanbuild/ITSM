<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrFlowFormText.ascx.cs" Inherits="Epower.ITSM.Web.Controls.CtrFlowFormText" %>
<asp:Label ID="labCaption" runat="server"  Visible="False"></asp:Label>
<asp:TextBox ID="txtText" runat="server"  MaxLength="100"></asp:TextBox>
<asp:Label ID="rWarning" runat="server" Style="margin-left:7px;" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>

<script type="text/javascript">
    window._id = document.getElementById("<%=txtText.ClientID %>");    
    </script>
