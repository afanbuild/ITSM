<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportMain.aspx.cs" Inherits="Epower.ITSM.Web.Reports.frmReportMain" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>报表</title>
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
            //SubmitExchange();
        }
//-->
</script>
<body>
    <form id="form1" runat="server">
       <table width="100%">
            <tr>
                <td><rsweb:ReportViewer ID="rptViewer" Width="1100px" Height="800px"   runat="server" Visible="true" ProcessingMode="Remote" ShowBackButton="True" SizeToReportContent="True" DocumentMapCollapsed="True" > 
                    <LocalReport EnableExternalImages="True" EnableHyperlinks="True">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="" />
                        </DataSources>
                    </LocalReport>
                    <ServerReport ReportServerUrl=""
                        Timeout="30000" />
                </rsweb:ReportViewer></td>
            </tr>
       </table>
    </form>
</body>
</html>
