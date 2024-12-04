<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeptPicker.ascx.cs"
    Inherits="Epower.ITSM.Web.Controls.DeptPicker" %>

<script language="javascript" type="text/javascript">
    function SelectPDept(obj) {
    

            window.open('<%=sApplicationUrl %>' + "/mydestop/frmpopdept.aspx?LimitCurr=<%=IsLimit %>&Opener_ClientId="+obj.id+"&TypeFrm=DePicker", "",  'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=500,height=400,left=150,top=50');  
            
}
</script>

<input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
<asp:Label ID="labDeptName" runat="server" Visible="False"></asp:Label>
<asp:TextBox ID="txtDept" runat="server" MaxLength="50"></asp:TextBox>&nbsp;<input id="cmdPopParentDept" runat="server" name="cmdPopParentDept" onclick="SelectPDept(this)"
    type="button" value="..." class="btnClass2" /><input id="hidDept" runat="server"
        name="hidDept" size="4" style="width: 56px" type="hidden" />
<input id="hidDeptName" runat="server" name="hidDept" size="4" style="width: 56px"
    type="hidden" />
<asp:Label ID="rWarning" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
<asp:TextBox ID="txtChange" runat="server" style="display:none;" Width="0px" Height="0px" AutoPostBack="true"
    OnTextChanged="txtChange_TextChanged"></asp:TextBox>