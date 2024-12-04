<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeptPickerBranch.ascx.cs"
    Inherits="Epower.ITSM.Web.Controls.DeptPickerBranch" %>

<script language="javascript" type="text/javascript">
    function SelectPDeptBranch(obj) {
        //==zxl==
        window.open('<%=sApplicationUrl %>' + "/mydestop/frmpopdeptBranch.aspx?SubBankID=" + '<%=ParentID%>',"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=400,height=300px,left=350,top=180");
    }
</script>

<asp:Label ID="labDeptName" runat="server" Visible="False"></asp:Label>
<asp:TextBox ID="txtDept" runat="server" MaxLength="50"  ReadOnly="true" OnTextChanged="txtDept_TextChanged"></asp:TextBox>&nbsp;<input id="cmdPopParentDept" runat="server" name="cmdPopParentDept" onclick="SelectPDeptBranch(this)"
    type="button" value="..." class="btnClass2" /><input id="hidDept" runat="server" name="hidDept"
        size="4" type="hidden" />
<input id="hidDeptName" runat="server" name="hidDept" size="4" type="hidden" />
<input id="hidParentId" runat="server" name="hidUser" size="4" type="hidden" />

<asp:Label ID="rWarning" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>