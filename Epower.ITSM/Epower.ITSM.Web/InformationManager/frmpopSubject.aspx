<%@ Page language="c#" Inherits="Epower.ITSM.Web.InformationManager.frmpopSubject" Codebehind="frmpopSubject.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>知识库类别</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		
	</HEAD>
	<script language="javascript">
			function Click_OK()
			{
			//===========zxl ===
			   // debugger;
			     var value=document.all.hidCatalogID.value;
				//window.returnValue=document.all.hidCatalogID.value;
		        if (value != null) {
                    if (value.length > 1) {
                        arr = value.split("@");
                        window.opener.document.getElementById("<%=Opener_ClientId %>txtPCatalogName").value=arr[1];
                        window.opener.document.getElementById("<%=Opener_ClientId %>hidPCatalogID").value=arr[0];
                        }
                          
                  }
				
				window.close();
			}
			function Click_Cancel()
			{
				window.returnValue="@";
				window.close();
			}			
		</script>
	<body leftmargin="0" topmargin="0" rightmargin="0" bottommargin="0">
		<form id="Form1" method="post" runat="server">
			<table width="100%" height="100%">
				<tr height="100%">
					<td><iframe id="Myiframe" frameborder="no" width="100%" height="100%"></iframe>
					</td>
				</tr>
				<tr>
					<td  align="center">
						<INPUT id="cmdOK" type="button" value="确定" onclick="Click_OK()" class="btnClass"> <INPUT id="cmdCancel" type="button" value="取消" onclick="Click_Cancel()" class="btnClass">
					</td>
				</tr>
			</table>
			<INPUT type="hidden" id="hidCatalogID"> <INPUT id=currCatalogID type=hidden value="<%=lngCurrSubjectID%>" 
name=currCatalogID>
			<script language="javascript">
<!--
			document.all.Myiframe.src = "frmselectSubject.aspx?LimitCurr=true&CurrSubjectID=" + document.all.currCatalogID.value;
//-->
			</script>
		</form>
	</body>
</HTML>
