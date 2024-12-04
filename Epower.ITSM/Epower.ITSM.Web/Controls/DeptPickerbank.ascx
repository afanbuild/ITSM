<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeptPickerbank.ascx.cs"
    Inherits="Epower.ITSM.Web.Controls.DeptPickerbank" %>

<script language="javascript" type="text/javascript">
    function SelectDeptName(obj) {
    
    //zxl=======
    window.open('<%=sApplicationUrl %>' + "/mydestop/frmpopdeptSubBank.aspx","","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=400,height=300px,left=350,top=180");
    //zxl==========
    }
</script>

<asp:Label ID="labDeptName" runat="server" Visible="False"></asp:Label>
<asp:TextBox ID="txtDept" runat="server" MaxLength="50" AutoPostBack="True"
    OnTextChanged="txtDept_TextChanged"></asp:TextBox>&nbsp;<input id="cmdPopParentDept"
        runat="server" name="cmdPopParentDept" onclick="SelectDeptName(this)" type="button"
        value="..." class="btnClass2" /><input id="hidDept" runat="server" name="hidDept"
            size="4" style="width: 56px" type="hidden" />
<input id="hidDeptName" runat="server" name="hidDept" size="4" style="width: 56px"
    type="hidden" />
<asp:Label ID="rWarning" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>