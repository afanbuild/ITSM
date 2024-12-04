<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeptPickerRight.ascx.cs" Inherits="Epower.ITSM.Web.Controls.DeptPickerRight" %>
<script language="javascript" type="text/javascript">
function SelectPDept(obj)
			{
			   var right=document.getElementById("<%=hidRight.ClientID%>").value;
			    //=============zxl==
			      var url='<%=sApplicationUrl %>' +"/mydestop/frmpopdeptRight.aspx?Right="+right+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";			      
			        window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=700,height=550px,left=150,top=50");
			        
			    //========zxl==
			}
			</script>
<input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
<asp:Label ID="labDeptName" runat="server" Visible="False"></asp:Label>
<asp:TextBox ID="txtDept" runat="server" MaxLength="50" Width="155px" ReadOnly="True"></asp:TextBox>&nbsp;
<input id="cmdPopParentDept" runat="server" name="cmdPopParentDept" onclick="SelectPDept(this)"
    type="button" value="..." class="btnClass2" /><input id="hidDept" runat="server" name="hidDept" size="4"
        style="width: 56px" type="hidden" />
<input id="hidDeptName" runat="server" name="hidDept" size="4"
        style="width: 56px" type="hidden" />
<asp:Label ID="rWarning" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
<input id="hidRight" runat="server" value="0" type="hidden" />
<input id="Hiddefualt" runat="server" value="0" type="hidden" />
<input id="HidMoRenValue" runat="server" value="0" type="hidden" />
<input id="HidMoRenName" runat="server" value="" type="hidden" />