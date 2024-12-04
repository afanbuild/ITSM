<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmSelectStaffLeft" Codebehind="frmSelectStaffLeft.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="CtrDeptTree" Src="../DeptControls/CtrDeptTree.ascx" %>


<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD  runat="server">
		<title>frmSelectDept</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">
			var blnClicked = false;
			function Tree_Click()
			{
				if ((CtrDeptTree1_tvDept.selectedNodeIndex !=null) & (CtrDeptTree1_tvDept.clickedNodeIndex !=null))
				{
					var lngPreDeptID = CtrDeptTree1_tvDept.getTreeNode(CtrDeptTree1_tvDept.selectedNodeIndex).getAttribute("ID");
					var lngDeptID = CtrDeptTree1_tvDept.getTreeNode(CtrDeptTree1_tvDept.clickedNodeIndex).getAttribute("ID");
					if(lngPreDeptID != lngDeptID || blnClicked == false)
					{
						window.parent.userinfo.location="frmSelectStaffRight.aspx?DeptID="+lngDeptID;
						blnClicked = true;
					}
				
				}


			}
		</script>
	</HEAD>
	<body leftmargin="0" topmargin="0">
		<form id="Form1" method="post" runat="server">
			<table>
				<tr>
					<td><uc1:CtrDeptTree id="CtrDeptTree1" runat="server"></uc1:CtrDeptTree>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
