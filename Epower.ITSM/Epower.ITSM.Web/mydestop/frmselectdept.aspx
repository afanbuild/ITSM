<%@ Page language="c#" Inherits="Epower.ITSM.Web.MyDestop.frmSelectDept" Codebehind="frmSelectDept.aspx.cs" %>
<%@ Reference Control="~/controls/ctrdepttree.ascx" %>
<%--<%@ Register TagPrefix="uc1" TagName="CtrDeptTree" Src="~/Controls/CtrDeptTree.ascx" %>--%>
<%@ Register Src="~/DeptControls/ctrdepttree.ascx" TagName="ctrdepttree" TagPrefix="uc12" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD  runat="server">
		<title>≤ø√≈</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script  type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>
		
		<script language="javascript">			
			
			function Tree_Click() {
			
			    var selectedNodeID = Form1.elements["CtrDeptTree1_tvDept_SelectedNode"].value;
			    if (selectedNodeID != "") {
			        var selectedNode = document.getElementById(selectedNodeID);
			        var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
			        value = value.substring(value.lastIndexOf('\\') + 1);
			        var text = selectedNode.innerText;
			        //return value.substring(value.lastIndexOf('\\') + 1);

			        window.parent.document.all.hidDeptID.value = value + "@" + text;
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
		<FONT face="ÀŒÃÂ">
			<FORM id="Form1" method="post" runat="server" >
			    <div>
			    <%--<uc1:CtrDeptTree id="CtrDeptTree1" runat="server"></uc1:CtrDeptTree>--%>
			    <uc12:ctrdepttree IsRegisterClientEvent="true" ID="CtrDeptTree1" runat="server" />
			    </div>

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
<style type="text/css">
		    .ctrdepttree1_tvdept_0 {
		        font-size:12px;	
		    }
		</style>