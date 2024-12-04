<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmDeptContForUser" Codebehind="frmDeptContForUser.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="CtrDeptTree" Src="../DeptControls/CtrDeptTree.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD id="HEAD1" runat="server">
		<title>frmDeptContForUser</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script  type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>
		
		<script language="javascript">
		var blnHasClicked = false;
		function Tree_Click()
		{
			var lngPreDeptID = 0;
			if (blnHasClicked == true)
            {
			    var selectedNodeID = theForm.elements["CtrDeptTree1_tvDept_SelectedNode"].value;
                if (selectedNodeID != "") 
                {
                    var selectedNode = document.getElementById(selectedNodeID);
                    var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
                    lngPreDeptID= value.substring(value.lastIndexOf('\\') + 1);
                }
			}
			else
			{
				blnHasClicked = true;
            }
            if (blnHasClicked) 
            {
                var nodeValue = GetSelectedNode();
                if (nodeValue != undefined && nodeValue != "undefined" && nodeValue != "") 
                {
                    window.parent.userinfo.location = "frmUserQuery.aspx?DeptID=" + nodeValue;
                }
            }
        }



        function GetSelectedNode() 
        {
            var selectedNodeID = theForm.elements["CtrDeptTree1_tvDept_SelectedNode"].value;
            if (selectedNodeID != "") 
            {
                var selectedNode = document.getElementById(selectedNodeID);
                var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
                return value.substring(value.lastIndexOf('\\') + 1);
            }
        }  


        function redirect(deptid)
        {
             window.parent.userinfo.location = "frmUserQuery.aspx?DeptID=" + deptid; //+"&DeptText="+lngDeptText;
        }

		</script>
	</HEAD>
	<body bgcolor="#ffffff">
		<form id="Form1" method="post" runat="server">
				<uc1:CtrDeptTree id="CtrDeptTree1" runat="server"></uc1:CtrDeptTree>
		</form>
	</body>
</HTML>
