<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrFlowEquCategory.ascx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.ctrFlowEquCategory" %>

<script type="text/javascript" language="javascript">
        function CateSelecteChanged(obj)
        {
            for (i = 0; i < obj.options.length; i++) {
                if (obj.options(i).selected) {
                    document.getElementById(obj.id.replace("ddlCate1", "hidCateID")).value = obj.options(i).value;
                    break;
                }
            }
        }
</script>

<asp:Label ID="labCate1" runat="server" Visible="False"></asp:Label><asp:DropDownList
    ID="ddlCate1" runat="server" style="z-index:1">
</asp:DropDownList>
<asp:Label ID="lblMessage" runat="server" Visible="False"></asp:Label>&nbsp;<asp:Label
    ID="rWarning" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label><input
        id="hidCateID" runat="server" name="hidCateID" type="hidden" />