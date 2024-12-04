<%@ Page Language="c#" Inherits="Epower.ITSM.Web.MyDestop.frmpopdeptSubBank" CodeBehind="frmpopdeptSubBank.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>机构</title>

    <script language="javascript">
			function Click_OK()
			{
			
			  var value=document.all.hidDeptID.value;
			  
			   if (value != null) {
                if (value.length > 1) {
                    arr = value.split("@");
			        window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptPickerbank2_txtDept").value=arr[1];
			        window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptPickerbank2_hidDeptName").value=arr[1];
			        window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptPickerbank2_hidDept").value=arr[0];
			        }
			    }
			    else
			    {
			     window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptPickerbank2_txtDept").value="";
			     window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptPickerbank2_hidDeptName").value="";
			     window.opener.document.getElementById("ctl00_ContentPlaceHolder1_DeptPickerbank2_hidDept").value=0;
			    }

                window.close();
			
			
			    return;
				window.returnValue=document.all.hidDeptID.value;
				window.close();
			}
			function Click_Cancel()
			{
				window.returnValue="@";
				window.close();
			}
			
			function SetSelectUrl()
			{
			   document.all.Myiframe.location = "frmselectdeptSubBank.aspx?LimitCurr=false&CurrDeptID=" + document.all.currDeptID.value;
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
        <input id="cmdOK" type="button" value="确定" onclick="Click_OK()" class="btnClass">
        <input id="cmdCancel" type="button" value="取消" onclick="Click_Cancel()" class="btnClass">
    </div>

    <input type="hidden" id="hidDeptID">
    <input id="currDeptID" type="hidden" value="<%=lngCurrDeptID%>">

    <script language="javascript">
<!--
			document.all.Myiframe.src = "frmselectdeptSubBank.aspx?LimitCurr=true&CurrDeptID=" + document.all.currDeptID.value;
//-->
    </script>

    </form>
</body>
</html>
