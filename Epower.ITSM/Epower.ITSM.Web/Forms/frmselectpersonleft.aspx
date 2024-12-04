<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmSelectPersonLeft" Codebehind="frmSelectPersonLeft.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="CtrDeptTree" Src="../Controls/CtrDeptTree.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>frmSelectPersonLeft</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">
			var blnHasClicked = false;
			function Tree_Click()
			{
			    var nodeValue = GetSelectedNode();
			    if (nodeValue != undefined && nodeValue != "undefined" && nodeValue != "") {
			        window.parent.userinfo.location = "frmSelectPersonRight.aspx?DeptID=" + nodeValue;
			    }			   

}

function GetSelectedNode() {
    var selectedNodeID = theForm.elements["CtrDeptTree1_tvDept_SelectedNode"].value;
    if (selectedNodeID != "") {
        var selectedNode = document.getElementById(selectedNodeID);
        var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
        return value.substring(value.lastIndexOf('\\') + 1);
    }
}  
		</script>
	</HEAD>
	<body leftmargin="0" topmargin="0">
		<form id="Form1" method="post" runat="server">
			<table>
				<tr>
					<td>
						<uc1:CtrDeptTree id="CtrDeptTree1" runat="server"></uc1:CtrDeptTree>
					</td>
				</tr>
			</table>
			

		</form>
	</body>
</HTML>
