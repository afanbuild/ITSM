<%@ Page Language="c#" Inherits="Epower.ITSM.Web.MyDestop.frmpopdeptBranch" CodeBehind="frmpopdeptBranch.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>����</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">

    <script language="javascript">
			function Click_OK()
			{
			var value=document.all.hidDeptID.value;
			if(value  !=null){
			    if(value.length > 1)
			    {
			      var arr =value.split("@");
			      window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptBranch_txtDept").value=arr[1];
			      window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptBranch_hidDeptName").value=arr[1];
			      window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptBranch_hidDept").value=arr[0];
			        
			    }
			}
			else
			{
			  window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptBranch_txtDept").value="";
			  window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptBranch_hidDeptName").value="";
			  window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptBranch_hidDept").value=0;
			    
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
			   document.all.Myiframe.location = "frmselectdeptBranch.aspx?LimitCurr=true&CurrDeptID=" + document.all.currDeptID.value;
			}
			
    </script>

</head>
<body leftmargin="0" topmargin="0" rightmargin="0" bottommargin="0">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <iframe id="Myiframe" frameborder="no" width="100%" height="90%"></iframe>
            </div>
            <div style="padding-left: 20px;">
                <span id="selectDeptTips"></span>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="text-align: center;">
        <input id="cmdOK" type="button" value="ȷ��" onclick="Click_OK()" class="btnClass">
        <input id="cmdCancel" type="button" value="ȡ��" onclick="Click_Cancel()" class="btnClass">
    </div>

    <input type="hidden" id="hidDeptID">
    <input id="currDeptID" type="hidden" value="<%=lngCurrDeptID%>">

    <script language="javascript">
<!--
			document.all.Myiframe.src = "frmselectdeptBranch.aspx?LimitCurr=true&CurrDeptID=" + document.all.currDeptID.value;
//-->
    </script>

    </form>
</body>
</html>
