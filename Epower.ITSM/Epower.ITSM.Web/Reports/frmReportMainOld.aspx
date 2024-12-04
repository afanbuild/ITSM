<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportMainOld.aspx.cs" Inherits="Epower.ITSM.Web.Reports.frmReportMainOld" %>
<%@ Register Assembly="ReportViewer" Namespace="Microsoft.Samples.ReportingServices"
    TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>±¨±í</title>
</head>
    <script language=javascript>
<!--
var req;
		function SubmitExchange()
		{	
			var versionStr = new String(window.clientInformation.appVersion);
			var oHttpReq;
			if((versionStr.indexOf("MSIE 5.0")!=-1)&&(versionStr.indexOf("Windows 98")!=-1))
			{
				oHttpReq = new ActiveXObject("Microsoft.XMLHTTP");
				
			}
			else
			{						
				oHttpReq = new ActiveXObject("Msxml2.XMLHTTP");	
				
			}		
			oHttpReq.open("POST", "DMUser.aspx", false);
			oHttpReq.send();
			var result = oHttpReq.responseText;	
			var arrayabc= result.split(';');  
			var username=arrayabc[0];
			var userpsw=arrayabc[1];
			BasicLoginAs(arrayabc[2], username,userpsw);
			
		}
		function BasicLoginAs(hostURL,id, pw)	 
		{ 
			req = null;
			req = new ActiveXObject("MSXML2.XMLHTTP"); 
			req.open("GET" ,hostURL , false, id, pw);      
			req.send();  
		}

		function state_Change()
		{
			if (req.readyState==4)
			{
				if (req.status==200)
				{
					alert(req.getAllResponseHeaders());
				}
				else
				{
					alert("Problem retrieving data:" + req.statusText)
				}
			}
		}
		var sVisitReportMode = '<%=VisitReportMode%>';
        if(sVisitReportMode=="1")
        {
            SubmitExchange();
        }
//-->
</script>
<body>
    <form id="form1" runat="server">
       <table width="100%">
            <cc1:ReportViewer id="rptViewer" runat="server" Width="1100" Height="800px"  Toolbar="False" Parameters="False" />
       </table>
    </form>
</body>
</html>
