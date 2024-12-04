<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmPopStaff" Codebehind="frmPopStaff.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>人员</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		
	</HEAD>
	<script language="javascript">
			function Click_OK()
			{
			    var type  ="<%=TypeFrm %>";
			    
			    var value=document.all.hidUserID_Name.value;
			        
			    if(type=="frmactormemberedit")
                {
                    if (typeof (value) != "undefined" && value.length > 1)
			        {
                         arr = value.split("@");
                         window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1];
			             window.opener.document.getElementById("<%=Opener_ClientId %>hidObjectID").value=arr[0];
                    }
                    else {
                         window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value="";
			             window.opener.document.getElementById("<%=Opener_ClientId %>hidObjectID").value="0";
                    }
                }
                if(type =="frmrightSeach")
                {
                    if(typeof(value) !="undefined" && value.length>1)
                    {
                        arr=value.split("@");
                        window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectID").value=arr[0];
                    }
                    else
                    {
                        window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectID").value="";
                    }                                                             
                }
				
				window.close();						
			}
			function Click_Cancel()
			{
				//window.returnValue="@";
				window.close();
			}
			
		</script>
	<body leftmargin="0" topmargin="0" rightmargin="0" bottommargin="0">
		<form id="Form1" method="post" runat="server">
		         <div>
		        <iframe id="Myiframe" frameborder="no" width="100%" height="90%"></iframe>
		    </div>
		    <div style=" padding-left:20px;">
		        <span id="selectDeptTips"></span>
		    </div>
		    <div style=" text-align:center;">
		        <INPUT id="cmdOK" type="button" value="确定" onclick="Click_OK()" class="btnClass"> 
				<INPUT id="cmdCancel" type="button" value="取消" onclick="Click_Cancel()" class="btnClass">
		    </div>

			<INPUT type="hidden" id="hidUserID_Name">
		</form>
		
		    <script language="javascript">
			    document.all.Myiframe.src = "frmSelectStaff.htm";
            </script>
    
	</body>
</HTML>
