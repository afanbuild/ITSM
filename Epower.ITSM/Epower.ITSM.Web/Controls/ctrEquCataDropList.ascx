<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrEquCataDropList.ascx.cs" Inherits="Epower.ITSM.Web.Controls.ctrEquCataDropList" %>

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
<input id="hidCateID" runat="server" type="hidden" />
<asp:Label ID="labCate1" runat="server" Visible="False"></asp:Label><asp:DropDownList
    ID="ddlCate1" runat="server">
</asp:DropDownList>
