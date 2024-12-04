<%@ Page language="c#" Inherits="Epower.ITSM.Web.Ajax.frmpopCatalog" Codebehind="frmpopCatalog.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>常用类别</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		
	</HEAD>
	<script language="javascript">
			function Click_OK()
			{			
				window.returnValue=document.all.hidCatalog.value;
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
	       <div style="" >
	        <iframe id="Myiframe" frameborder="no" width="100%" height="90%"></iframe>
	        </div>
		    <div style=" padding-left:20px;">
		        <span id="selectDeptTips"></span>
		    </div>
			 <div style=" text-align:center;">
			    <INPUT type="hidden" id="hidCatalog" value=""> <INPUT id=currRootID type=hidden value="<%=lngRootID%>">
			    <INPUT id="hidIsPoint" type=hidden value="<%=IsPoint%>">
			</div>
			<script language="javascript">
			    document.all.Myiframe.src = "frmSelectCalalog.aspx?RootID=" + document.all.currRootID.value+"&IsPoint="+document.all.hidIsPoint.value;
			</script>
		</form>
	</body>
</HTML>
