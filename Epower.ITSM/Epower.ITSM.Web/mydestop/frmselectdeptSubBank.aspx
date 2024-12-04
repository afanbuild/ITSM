<%@ Page Language="c#" Inherits="Epower.ITSM.Web.MyDestop.frmselectdeptSubBank" CodeBehind="frmselectdeptSubBank.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="CtrdepttreeSubBank" Src="~/Controls/CtrdepttreeSubBank.ascx" %> 
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>·ÖÐÐ</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script language="javascript">
			
			function Tree_Click() {

			    var selectedNodeID = theForm.elements["CtrdepttreeSubBank1_tvDept_SelectedNode"].value;
			    if (selectedNodeID != "") {
			        var selectedNode = document.getElementById(selectedNodeID);
			        var sValue = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
			        sValue = sValue.substring(sValue.lastIndexOf('\\') + 1);
			        var sText = selectedNode.innerHTML;
			        window.parent.document.all.hidDeptID.value = sValue + "@" + sText;
        
			        window.parent.document.all.cmdOK.click();
			    }

			}


			function Tree_DBClick(event) {

			    var selectedNodeID = theForm.elements["CtrdepttreeSubBank1_tvDept_SelectedNode"].value;
			    if (selectedNodeID != "") {
			        var selectedNode = document.getElementById(selectedNodeID);
			        var sValue = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
			        sValue = sValue.substring(sValue.lastIndexOf('\\') + 1);
			        var sText = selectedNode.innerHTML;
			        window.parent.document.all.hidDeptID.value = sValue + "@" + sText;
			        alert(sValue + "@" + sText);
			        //window.parent.document.all.cmdOK.click();
			    }
			}
			
    </script>

</head>
<body leftmargin="0" topmargin="0">
    <font face="ËÎÌå">
        <form id="Form1" method="post" runat="server">
        <table id="Table1">
            <tr>
                <td> 
                       <uc1:CtrdepttreeSubBank id="CtrdepttreeSubBank1" runat="server"></uc1:CtrdepttreeSubBank>  
                </td>
            </tr>
        </table>
        </form>
    </font>
</body>
</html>
