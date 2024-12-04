<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="uc1" TagName="CtrDeptTree" Src="../Controls/CtrDeptTree.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.DeptTree" Codebehind="DeptTree.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>DeptTree</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">
<!--
       
		function Tree_Click() {

		    var nodeValue = GetSelectedNode();
		    if (nodeValue != undefined && nodeValue != "undefined" && nodeValue != "") {
		        window.parent.right.location = "PersonList.aspx?DeptID=" + nodeValue + "&AppID=" + document.all.AppID.value;
		    }
		}
		
		function Tree_DBClick()
		{
		    return true;
		}

		function GetSelectedNode() {
		    var selectedNodeID = theForm.elements["CtrDeptTree1_tvDept_SelectedNode"].value;
		    if (selectedNodeID != "") {
		        var selectedNode = document.getElementById(selectedNodeID);
		        var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
		        return value.substring(value.lastIndexOf('\\') + 1);
		    }
		}  
//-->
		</script>
	</HEAD>
	<body>
		<form method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 33px; WIDTH: 198px; POSITION: absolute; TOP: 8px; HEIGHT: 281px"
				cellSpacing="1" cellPadding="1" width="198" border="1">
				<TR>
					<TD vAlign="top" colSpan="1" rowSpan="1">
						<uc1:CtrDeptTree id="CtrDeptTree1" runat="server"></uc1:CtrDeptTree>
						<INPUT 
id=AppID type=hidden value="<%=lngAppID.ToString()%>"></TD>
				</TR>
			</TABLE>
			

		</form>
	</body>
</HTML>
