<%@ Page Language="c#" Inherits="Epower.ITSM.Web.MyDestop.frmPopDept" CodeBehind="frmPopDept.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>部门</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">

    <script language="javascript">
		
		    function GetQueryString(name) {
                var url = window.location.search.substr(1)
                var pattern = /(\w+)=(\w+)/ig;
                var parames = {};
                url.replace(pattern, function(a, b, c){
                parames[b] = c;
                });
                return parames[name];
            }

			
			function Click_Cancel()
			{
				window.returnValue="@";
				window.close();
			}
    </script>

</head>

<script type="text/javascript">
		    function Click_OK()
			{
			   
			  //  value=document.all.hidDeptID.value;
			        var arr=document.all.hidDeptID.value.split("@");
			        			        
			        var type  ="<%=TypeFrm %>";
			        
			        if(type =="frmuserqueryMult")
			        {
			            window.opener.document.getElementById("ctl00_ContentPlaceHolder1_txtDeptName").value=arr[1];
			            window.opener.document.getElementById("ctl00_ContentPlaceHolder1_hidQueryDeptID").value=arr[0];
			            window.close();
    			    
			        }
			        else if(type=="frmeditright")
			        {
			            window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1]+"(" +arr[0] + ")";
			            window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectId").value=arr[0];
			            window.close();    
			        }
			        else if(type=="DePicker")
			        {
			             var openerClientId = "<%=Opener_ClientId %>";
			             window.opener.document.getElementById(openerClientId.replace("cmdPopParentDept","txtDept")).value=arr[1];
                         window.opener.document.getElementById(openerClientId.replace("cmdPopParentDept","hidDeptName")).value=arr[1];
                         window.opener.document.getElementById(openerClientId.replace("cmdPopParentDept","hidDept")).value=arr[0];
                         window.opener.document.getElementById(openerClientId.replace("cmdPopParentDept","txtChange")).value=arr[1];

                         window.close();
			        }
			        else if(type=="frmuserquery")
			        {
			        
				
						//arr=value.split("@");
						window.opener.document.getElementById("<%=Opener_ClientId %>txtDeptName").value=arr[1];
						window.opener.document.getElementById("<%=Opener_ClientId %>hidQueryDeptID").value=arr[0];
						
						//document.all.txtDeptName.value=arr[1];
						//document.all.hidQueryDeptID.value=arr[0];
			        }
			        else{		    
                         window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptPicker1_txtDept").value=arr[1];
                         window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptPicker1_hidDeptName").value=arr[1];
                         window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptPicker1_hidDept").value=arr[0];
                         window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptPicker1_txtChange").value=arr[1];
                         window.close();
                    }
               
                
			    }
			    
		
</script>

<body leftmargin="0" topmargin="0" rightmargin="0" bottommargin="0">
    <form id="Form1" method="post" runat="server">
    <div style="text-align:center;">
        <iframe id="Myiframe" frameborder="no" width="50%"  style="border:1px solid #EEEEE0;height:300px;"></iframe>
    </div>
    <div style="padding-left: 20px;">
        <span id="selectDeptTips"></span>
    </div>
    <div style="text-align: center;padding-top:10px;">
        <input id="cmdOK" type="button" value="确定" onclick="Click_OK()" class="btnClass">
        <input id="cmdCancel" type="button" value="取消" onclick="Click_Cancel()" class="btnClass">
    </div>
    <input type="hidden" id="hidDeptID">
    <input id="currDeptID" type="hidden" value="<%=lngCurrDeptID%>">

    <script language="javascript">
<!--
			document.all.Myiframe.src = "frmSelectDept.aspx?LimitCurr=<%=IsLimit%>&CurrDeptID=" + document.all.currDeptID.value;
//-->
    </script>

    </form>
</body>
</html>
