<%@ Register TagPrefix="uc1" TagName="CtrDeptTree" Src="../Controls/CtrDeptTree.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.AccountSelect.frmDeptContForUser" Codebehind="frmDeptContForUser.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>frmDeptContForUser</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">
		var blnHasClicked = false;
		function Tree_Click()
		{
			var lngPreDeptID = 0;
			if (blnHasClicked == true)
			{ 
				lngPreDeptID = CtrDeptTree1_tvDept.getTreeNode(CtrDeptTree1_tvDept.selectedNodeIndex).getAttribute("ID");
			}
			else
			{
				blnHasClicked = true;
			}
			if(CtrDeptTree1_tvDept.clickedNodeIndex!=null)
			{
			    var lngDeptID = CtrDeptTree1_tvDept.getTreeNode(CtrDeptTree1_tvDept.clickedNodeIndex).getAttribute("ID");
			    if(lngPreDeptID != lngDeptID)
			    {
    			   
				    window.parent.userinfo.location="frmUserQuery.aspx?DeptID="+lngDeptID;
			    }
			}
		}
		
		
		function Tree_DBClick()
		{
		    
            if(CtrDeptTree1_tvDept.clickedNodeIndex!=null)
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
			     window.parent.userinfo.location="frmUserQuery.aspx?ExtAll=true&DeptID="+lngDeptID;
			 }
		}
		
		</script>
	</HEAD>
	<body bgcolor="#ffffff">
		<form id="Form1" method="post" runat="server">
			<FONT face="ËÎÌו">
				<uc1:CtrDeptTree id="CtrDeptTree1" runat="server"></uc1:CtrDeptTree></FONT>
		</form>
	</body>
</HTML>
