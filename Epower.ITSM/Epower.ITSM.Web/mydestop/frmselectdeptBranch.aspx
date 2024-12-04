<%@ Reference Control="~/controls/ctrdepttreeBranch.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrDeptTree" Src="~/Controls/ctrdepttreeBranch.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.MyDestop.frmselectdeptBranch" Codebehind="frmselectdeptBranch.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD  runat="server">
		<title>Ö§ÐÐ</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script  type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>
		<script language="javascript">
		
		
		 function Tree_Click()
	        {
	              var selectedNodeID=Form1.elements["CtrDeptTree1_tvDept_SelectedNode"].value;
	              if(selectedNodeID !=""){
	                var selectedNode=document.getElementById(selectedNodeID);
	                var value=selectedNode.href.substring(selectedNode.href.indexOf(",")+3,selectedNode.href.length-2);
	                var text=selectedNode.innerHTML;
	                value=value.substring(value.lastIndexOf('\\')+1);
	                window.parent.document.all.hidDeptID.value=value+"@"+text;
	             
	                }
	                else
	                {
	                   window.parent.document.all.hidDeptID.value="@";
	                
	                }
	        
	        }

			
			function Tree_DBClick()
			{
				if (document.all.CtrDeptTree1_tvDept.clickedNodeIndex !=null)
				{
					var lngID =document.all.CtrDeptTree1_tvDept.getTreeNode(document.all.CtrDeptTree1_tvDept.clickedNodeIndex).getAttribute("ID");
					var sText =document.all.CtrDeptTree1_tvDept.getTreeNode(document.all.CtrDeptTree1_tvDept.clickedNodeIndex).getAttribute("Text");
					window.parent.document.all.hidDeptID.value=lngID+"@"+sText;
					window.parent.returnValue=window.parent.document.all.hidDeptID.value;
				    window.parent.close();
				}
				else
				{
					window.parent.document.all.hidDeptID.value="@";
				}
			}
			
		</script>
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<FONT face="ËÎÌå">
			<FORM id="Form1" method="post" runat="server"  style="border:1px solid #EEEEE0;">
				<TABLE id="Table1">
					<TR>
						<TD>
							<uc1:CtrDeptTree id="CtrDeptTree1" runat="server"></uc1:CtrDeptTree></TD>
					</TR>
				</TABLE>
			</FORM>
		</FONT>
	</body>
</HTML>
<script type="text/javascript">
    $(document).ready(function(){
        var node = $('.CtrDeptTree1_tvDept_1');
        node.css('color','red');
    });
</script>
