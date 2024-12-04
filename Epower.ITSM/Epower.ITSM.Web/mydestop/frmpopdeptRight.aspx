<%@ Page language="c#" Inherits="Epower.ITSM.Web.MyDestop.frmpopdeptRight" Codebehind="frmpopdeptRight.aspx.cs" %>
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
			   
			    //=============zxl==
			    //window.returnValue=document.all.hidDeptID.value;
			        var value=document.all.hidDeptID.value;
			    if(value != null)
				{
					if(value.length>1)
					{
						arr=value.split("@");
						window.opener.document.getElementById("<%=Opener_ClientId %>txtDept").value=arr[1];
						window.opener.document.getElementById("<%=Opener_ClientId %>hidDeptName").value=arr[1];
						window.opener.document.getElementById("<%=Opener_ClientId %>hidDept").value=arr[0];
					}
					else
					{
					    if(window.opener.document.getElementById("<%=Opener_ClientId %>Hiddefualt").value=="1")
					    {
					        var value=window.opener.document.getElementById("<%=Opener_ClientId %>HidMoRenValue").value;
		                    var name=window.opener.document.getElementById("<%=Opener_ClientId %>HidMoRenName").value;
		                    window.opener.document.getElementById("<%=Opener_ClientId %>txtDept").value=name;
						    window.opener.document.getElementById("<%=Opener_ClientId %>hidDeptName").value=name;
						    window.opener.document.getElementById("<%=Opener_ClientId %>hidDept").value=value;
    				       
					    }
					    else
					    {
					        window.opener.document.getElementById("<%=Opener_ClientId %>txtDept").value="";
						    window.opener.document.getElementById("<%=Opener_ClientId %>hidDeptName").value="";
						    window.opener.document.getElementById("<%=Opener_ClientId %>hidDept").value=0;
					        
					    }
					}
			    }
			    else{
			    
			        if(window.opener.document.getElementById("<%=Opener_ClientId %>Hiddefualt").value=="1")
				    {
				        
				        var value=window.opener.document.getElementById("<%=Opener_ClientId %>HidMoRenValue").value;
			            var name=window.opener.document.getElementById("<%=Opener_ClientId %>HidMoRenName").value;
				        
				        window.opener.document.getElementById("<%=Opener_ClientId %>txtDept").value = name;
					    window.opener.document.getElementById("<%=Opener_ClientId %>hidDeptName").value = name;
					    window.opener.document.getElementById("<%=Opener_ClientId %>hidDept").value = value;
				    }
				    else
				    {
				        window.opener.document.getElementById("<%=Opener_ClientId %>txtDept").value="";
				        window.opener.document.getElementById("<%=Opener_ClientId %>hidDeptName").value="";
				        window.opener.document.getElementById("<%=Opener_ClientId %>hidDept").value=0;
				    }
			    
			    }
			
			//==============				
				window.close();
			}
			function Click_Cancel()
			{
				window.returnValue="@";
				window.close();
			}
			
			function SetSelectUrl()
			{
			   document.all.Myiframe.location = "frmselectdeptRight.aspx?LimitCurr=true&CurrDeptID=" + document.all.currDeptID.value+"&Right="+document.all.Right.value;
			}
			
		</script>
	<body leftmargin="0" topmargin="0" rightmargin="0" bottommargin="0">
		<form id="Form1" method="post" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
          
		    <div style="" >
		        <iframe id="Myiframe" frameborder="no" width="100%" height="90%"></iframe>
		    </div>
		    <div style=" padding-left:20px;">
		        <span id="selectDeptTips"></span>
		    </div>
		      </ContentTemplate>
        </asp:UpdatePanel>
		    <div style=" text-align:center;">
		        <INPUT id="cmdOK" type="button" value="确定" onclick="Click_OK()" class="btnClass"> 
				<INPUT id="cmdCancel" type="button" value="取消" onclick="Click_Cancel()" class="btnClass">
		    </div>

			<INPUT type="hidden" id="hidDeptID"> <INPUT id=currDeptID type=hidden value="<%=lngCurrDeptID%>">
			<input id="Right" type="hidden" value="<%=Right%>" />
			<script language="javascript">
<!--
			document.all.Myiframe.src = "frmselectdeptRight.aspx?LimitCurr=true&CurrDeptID=" + document.all.currDeptID.value+"&Right="+document.all.Right.value;
//-->
			</script>
		</form>
	</body>
</HTML>
