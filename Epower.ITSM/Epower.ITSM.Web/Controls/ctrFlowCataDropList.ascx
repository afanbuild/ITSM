<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrFlowCataDropList.ascx.cs"
    Inherits="Epower.ITSM.Web.Controls.ctrFlowCataDropList" %>

<script type="text/javascript" language="javascript">
    function CateSelecteChanged(obj) {
        for (i = 0; i < obj.options.length; i++) {
            if (obj.options(i).selected) {
                document.getElementById(obj.id.replace("ddlCate1", "hidCateID")).value = obj.options(i).value;
                break;
            }
        }
    }
</script>

<input id="hidCateID" runat="server" name="hidCateID" type="hidden" />
<asp:Label ID="labCate1" runat="server" Visible="False"></asp:Label>
<asp:DropDownList ID="ddlCate1" runat="server" Width="152px">
</asp:DropDownList>
<asp:RadioButtonList ID="RadioCate1" CellPadding="0" CellSpacing="0" runat="server"
    RepeatColumns="4" RepeatDirection="Horizontal"  Width="100%" >
</asp:RadioButtonList>
<asp:Label ID="lblMessage" runat="server" Visible="False"></asp:Label><asp:Label
    ID="rWarning" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
