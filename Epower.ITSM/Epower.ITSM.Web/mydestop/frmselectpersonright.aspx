<%@ Page language="c#" Inherits="Epower.ITSM.Web.mydestop.frmSelectPersonRight" Codebehind="frmSelectPersonRight.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>frmSelectPersonRight</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">
			function chkSelect_Click(obj)
			{
				reg=/chkSelect/g;
				var idtag=obj.id.replace(reg,"");
				if (obj.checked==true) {
					var userid=document.all(idtag+"hidUserID").value;
					var username=document.all(idtag+"hidUserName").value;
					window.parent.parent.document.all.hidUserID_Name.value=userid+"@"+username
				}
				
				OnlySelectOne(obj);
			}
			
			
			function OnlySelectOne(obj)
			{
				for(i=0;i<document.all.length;i++)
				{
					var sid=document.all(i).id;
					if(sid!="")
					{
						if(sid.substr(sid.length-9,9)=="chkSelect")
						{
							document.all(i).checked=false;
						}
					}
				}
				obj.checked=true;				
			}
			
			
			
			function Add_Staff()
			{
			  
			    		
				var i
				var j
				var el = document.all.lsbStaff;
											     
				i=document.all.lsbStaff.selectedIndex;
				
				if(i==-1)
				{
				    return;
				}
				
			    window.opener.document.getElementById("txtObjectName").value=el.options(i).text;
			    window.opener.document.getElementById("txtObjectId").value=el.options(i).value;
			
				window.parent.close();
				
			}   


		

		</script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<asp:listbox id="lsbStaff" ondblclick="Add_Staff();" runat="server" Width="152px" Height="100%"></asp:listbox></form>
	</body>
</HTML>
