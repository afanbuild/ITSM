<%@ Reference Control="~/controls/ctrdepttree.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrDeptTree" Src="~/Controls/CtrDeptTree.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.MyDestop.frmselectdeptRight" Codebehind="frmselectdeptRight.aspx.cs" %>
<%@ Register src="../Controls/ctrdepttreeRight.ascx" tagname="ctrdepttreeRight" tagprefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD  runat="server">
		<title>部门</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<script language="javascript">
		    
		    function Tree_Click()
		    {
		        var selectedNodeID = Form1.elements["CtrDeptTree1_tvDept_SelectedNode"].value;
			    if (selectedNodeID != "") {
			        var selectedNode = document.getElementById(selectedNodeID);
			        var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
			        value = value.substring(value.lastIndexOf('\\') + 1);
			        var text = selectedNode.innerText;
			        
			        //return value.substring(value.lastIndexOf('\\') + 1);

			        window.parent.document.all.hidDeptID.value = value + "@" + text;
			        window.parent.document.all.selectDeptTips.innerHTML = "您选择的部门是：<span style='color:red;'>" + text +"</span>";
			      }
			      else
			      {
			          window.parent.document.all.selectDeptTips.value = "@";
			          window.parent.document.all.selectDeptTips.innerHTML = "";
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
		<FONT face="宋体">
			<FORM id="Form1" method="post" runat="server">
				<TABLE id="Table1">
					<TR>
						<TD>
							<uc2:ctrdepttreeRight ID="CtrDeptTree1" runat="server" />
                        </TD>
					</TR>
				</TABLE>
			</FORM>
		</FONT>
	</body>
</HTML>
