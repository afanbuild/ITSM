<%@ Page Language="c#" Inherits="Epower.ITSM.Web.AppForms.Allfrmpopstaff"  CodeBehind="Allfrmpopstaff.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>人员</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
</head>

<script language="javascript">
			function Click_OK()
			{			    
				var sReturnValue=document.all.hidUserID_Name.value;
				if(sReturnValue=="")
				{
				    sReturnValue="@";
				}
				
				//window.returnValue=sReturnValue;
				//==== zxl=============
				var value=sReturnValue;
				if(typeof(value)!="undefined" && value.length>1)
				{
				    
				    var TypeFrm ="<%=TypeFrm %>";
				    if(TypeFrm=="AddINJFpeople")
				    {
				       if(window.opener.document.getElementById("<%=Opener_ClientId %>UserValues").value=='')
				       {
				         window.opener.document.getElementById("<%=Opener_ClientId %>UserValues").value=value;   
				       }
				       else
				       {
				        window.opener.document.getElementById("<%=Opener_ClientId %>UserValues").value =window.opener.document.getElementById("<%=Opener_ClientId %>UserValues").value+"|"+value;
				       }
				       var listvalue =value.split("|");
				       var strValue="";
				       for(var x=0;x<listvalue.length;x++)
				       {
				         if(strValue =="")
				         {
				            strValue=listvalue[x].split("@")[1];
				         }
				         else
				         {
				            strValue=strValue+","+listvalue[x].split("@")[1];
				         }
				       }
				       if(window.opener.document.getElementById("<%=Opener_ClientId %>Lb_people").innerText=='')
				       {
				         window.opener.document.getElementById("<%=Opener_ClientId %>Lb_people").innerText =strValue;
				         window.opener.document.getElementById("<%=Opener_ClientId %>HidPeople").value =strValue; 
				       }
				       else
				       {
				        window.opener.document.getElementById("<%=Opener_ClientId %>Lb_people").innerText=window.opener.document.getElementById("<%=Opener_ClientId %>Lb_people").innerText+","+strValue;
				        window.opener.document.getElementById("<%=Opener_ClientId %>HidPeople").value=window.opener.document.getElementById("<%=Opener_ClientId %>HidPeople").value+","+strValue;
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
			
</script>

<body leftmargin="0" topmargin="0" rightmargin="0" bottommargin="0">
    <form id="Form1" method="post" runat="server">
    <div>
         <iframe src="AllfrmSelectStaff.aspx" runat="server" frameborder="no" width="100%" height="60%">
         </iframe>
    </div>
      <div style="padding-left: 20px;">
        <span id="selectDeptTips"></span>
    </div>
    <div style="text-align: center;">
         <input id="cmdOK" type="button" value="确定" onclick="Click_OK()" class="btnClass">
         <input id="cmdCancel" type="button" value="取消" onclick="Click_Cancel()" class="btnClass">
    </div>
    <input type="hidden" id="hidUserID_Name">
    </form>
</body>
</html>
