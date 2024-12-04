<%@ Page language="c#" Inherits="Epower.ITSM.Web.AppForms.Allfrmselectstaffright" Codebehind="Allfrmselectstaffright.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>frmSelectStaffRight</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript">
			function chkSelect_Click(obj)
			{
				reg=/chkSelect/g;
				var idtag=obj.id.replace(reg,"");
				if (obj.checked == true) {
				    var userid = document.all(idtag + "hidUserID").value;
				    var username = document.all(idtag + "hidUserName").value;
				    if (window.parent.parent.document.all.hidUserID_Name.value != "") {
				        window.parent.parent.document.all.hidUserID_Name.value = window.parent.parent.document.all.hidUserID_Name.value + "|" + userid + "@" + username
				    }
				    else {
				        window.parent.parent.document.all.hidUserID_Name.value =  userid + "@" + username
				    }
				}
				else {
				    var userid = document.all(idtag + "hidUserID").value;
				    var username = document.all(idtag + "hidUserName").value;
				    var Values = window.parent.parent.document.all.hidUserID_Name.value;
				    var strValues2 = "";
				    if (Values != "") {
				        var listValues = Values.split("|");
				        if (listValues.length > 0) {
				            for (var i = 0; i < listValues.length; i++) {
				                var vlue = listValues[i].split("@")[0];
				                if (vlue != userid) {
				                    if (strValues2 == "") {
				                        strValues2 = listValues[i];
				                    }
				                    else {
				                        strValues2 = strValues2+"|"+ listValues[i];
				                    }        
				                }
				            }
				            window.parent.parent.document.all.hidUserID_Name.value = strValues2;
				        }
				        else {
				            window.parent.parent.document.all.hidUserID_Name.value = "";
				        }
				    }
				    
				}
			}
			
		</script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<asp:DataList id="dlUsers" runat="server" RepeatColumns="2" BorderColor="#999999" BorderStyle="None"
				BackColor="White" BorderWidth="1px" RepeatDirection="Horizontal">
				<ItemTemplate>
					<asp:CheckBox id=chkSelect runat="server" onclick="chkSelect_Click(this)" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'>
					</asp:CheckBox><INPUT id=hidUserID type=hidden value='<%# DataBinder.Eval(Container.DataItem, "UserID")%>' runat="server">
					<INPUT type="hidden" id=hidUserName value='<%# DataBinder.Eval(Container.DataItem, "Name")%>' runat=server>
				</ItemTemplate>
			</asp:DataList></form>
	</body>
</HTML>
