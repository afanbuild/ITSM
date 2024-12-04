<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmPopCatalog" Codebehind="frmPopCatalog.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>部门</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		
	</HEAD>
	<script language="javascript">
			function Click_OK()
			{
				//window.returnValue=document.all.hidCatalogID.value;
				var value=document.all.hidCatalogID.value;
				 if (value != null) {
                if (value.length > 1) {
                    arr = value.split("@");
                    if (arr[0] == "1") {
                        alert("不能选择根分类!");
                    }
                    else {
                       //  window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1]+"(" +arr[0] + ")";
                         window.opener.document.getElementById("<%=Opener_ClientId %>txtPCatalogName").value=arr[1];
                         window.opener.document.getElementById("<%=Opener_ClientId %>hidPCatalogID").value=arr[0];
                         
                        //document.all.txtPCatalogName.value = arr[1];
                        //document.all.hidPCatalogID.value = arr[0];
                    }
                }
            }
				
				window.close();
			}
			function Click_Cancel()
			{
				window.returnValue="@";
				window.close();
			}
			
			function SetSelectUrl()
			{
			   document.all.Myiframe.location = "frmSelectCatalog.aspx?LimitCurr=true&CurrCatalogID=" + document.all.currCatalogID.value;
			}
			
		</script>
	<body leftmargin="0" topmargin="0" rightmargin="0" bottommargin="0">
		<form id="Form1" method="post" runat="server">
		    <div style="" >
		        <iframe id="Myiframe" frameborder="no" width="100%" height="80%"></iframe>
		    </div>
		    <div style=" padding-left:20px;">
		        <span id="selectDeptTips"></span>
		    </div>
		    <div style=" text-align:center;">
		        <INPUT id="cmdOK" type="button" value="确定" onclick="Click_OK()" class="btnClass"> 
				<INPUT id="cmdCancel" type="button" value="取消" onclick="Click_Cancel()" class="btnClass">
		    </div>
			<INPUT type="hidden" id="hidCatalogID"> <INPUT id=currCatalogID type=hidden value="<%=lngCurrCatalogID%>" 
name=currCatalogID>
			<script language="javascript">
<!--
			document.all.Myiframe.src = "frmSelectCatalog.aspx?LimitCurr=true&CurrCatalogID=" + document.all.currCatalogID.value;
//-->
			</script>
		</form>
	</body>
</HTML>
