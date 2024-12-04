<%@ Page language="c#" Inherits="Epower.ITSM.Web.MyDestop.frmdeptcontforuserMult" Codebehind="frmdeptcontforuserMult.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="CtrDeptTree" Src="../Controls/CtrDeptTree.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server" >
		<title>frmDeptContForUser</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		
	</HEAD>
	<body bgcolor="#ffffff">
		<form id="Form1" method="post" runat="server">
			<FONT face="ËÎÌו">
				<uc1:CtrDeptTree id="CtrDeptTree1" runat="server"></uc1:CtrDeptTree></FONT>
				<script language="javascript">
		var blnHasClicked = false;
		function Tree_Click() {

		    var nodeValue = GetSelectedNode();
		    if (nodeValue != undefined && nodeValue != "undefined" && nodeValue != "") {
		  
		        window.parent.document.all.DeptID=nodeValue;
		       
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
		
		
		function Tree_DBClick()
		{
		    
			var lngDeptID = CtrDeptTree1_tvDept.getTreeNode(CtrDeptTree1_tvDept.clickedNodeIndex).getAttribute("ID");
			if(CtrDeptTree1_tvDept.getTreeNode(CtrDeptTree1_tvDept.clickedNodeIndex).getAttribute("expanded") == false)
			{
			    CtrDeptTree1_tvDept.getTreeNode(CtrDeptTree1_tvDept.clickedNodeIndex).setAttribute("expanded", true, 0);
			 }
			 else
			 {
			    CtrDeptTree1_tvDept.getTreeNode(CtrDeptTree1_tvDept.clickedNodeIndex).setAttribute("expanded", false, 0);
			 }
			// window.parent.userinfo.location = "frmUsersMultSelect.aspx?ExtAll=true&DeptID=" + lngDeptID + "&LimitCurr=<%=IsLimit %>";
		}
		
		</script>
		</form>
	</body>
</HTML>
