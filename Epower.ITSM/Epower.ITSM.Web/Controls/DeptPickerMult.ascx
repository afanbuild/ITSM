<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeptPickerMult.ascx.cs" Inherits="Epower.ITSM.Web.Controls.DeptPickerMult" %>
<script language="javascript" type="text/javascript">
function SelectPDeptMult(obj)
			{
			    //zxl=========
			    var url='<%=sApplicationUrl %>' +"/mydestop/frmpopdeptMult.aspx?LimitCurr=<%=IsLimit %>&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
			     window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=100");
			}
			</script>
<asp:Label ID="labDeptName" runat="server" Visible="False"></asp:Label>
<asp:TextBox ID="txtDept" runat="server" MaxLength="50"  ReadOnly="True"></asp:TextBox><input id="cmdPopParentDept" runat="server" name="cmdPopParentDept" onclick="SelectPDeptMult(this)"
    type="button" value="..." class="btnClass2" /><input id="hidDept" runat="server" name="hidDept" size="4"
        style="width: 56px" type="hidden" />
<input id="hidDeptName" runat="server" name="hidDept" size="4"
        style="width: 56px" type="hidden" />
<asp:Label ID="rWarning" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Red">*</asp:Label>
<input id="hidClientId_ForOpenerPage" type="hidden" value="0" runat="server" />