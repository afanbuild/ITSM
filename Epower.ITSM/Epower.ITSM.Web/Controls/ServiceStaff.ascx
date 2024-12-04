<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceStaff.ascx.cs" Inherits="Epower.ITSM.Web.Controls.ServiceStaff" %>
<script>
    function SelectServiceStaff(obj)
			{
			    var value = window.showModalDialog("../mydestop/frmSelectServiceStaff.htm","","dialogWidth=400px; dialogHeight=300px;status=no; help=no;scroll=auto;resizable=no") ;
				if(value != null)
				{
					if(value.length>1)
					{	
					    arr=value.split("@");
						document.getElementById(obj.id.replace("cmdPopUser","txtUser")).value = arr[1];
						document.getElementById(obj.id.replace("cmdPopUser","hidUserName")).value = arr[1];
						document.getElementById(obj.id.replace("cmdPopUser","hidUser")).value = arr[0];
					}
					else
					{
					    document.getElementById(obj.id.replace("cmdPopUser","txtUser")).value = "";
						document.getElementById(obj.id.replace("cmdPopUser","hidUserName")).value = "";
						document.getElementById(obj.id.replace("cmdPopUser","hidUser")).value = 0;
					}
				}
				else
				{
				    document.getElementById(obj.id.replace("cmdPopUser","txtUser")).value = "";
					document.getElementById(obj.id.replace("cmdPopUser","hidUserName")).value = "";
					document.getElementById(obj.id.replace("cmdPopUser","hidUser")).value = 0;
				}
			}
</script>
<asp:Label ID="labUser" runat="server" Visible="False"></asp:Label><asp:TextBox
    ID="txtUser" runat="server" MaxLength="80" Width="128px" ReadOnly="True"></asp:TextBox><input id="cmdPopUser" runat="server" onclick="SelectServiceStaff(this)" type="button" value="..." class="btnClass" /><input
            id="hidUser" runat="server" name="hidUser" size="4" style="width: 56px; height: 19px"
            type="hidden" />
<input id="hidUserName" runat="server" name="hidUser" size="4" style="width: 56px; height: 19px"
            type="hidden" />