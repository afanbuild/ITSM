<%@ Reference Control="~/controls/ctrdepttree.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrDeptTree" Src="~/Controls/CtrDeptTree.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Ajax.frmSelectCalalog" Codebehind="frmSelectCalalog.aspx.cs" %>
<%@ Register src="../Controls/ctrcatalogtreeNew.ascx" tagname="ctrcatalogtreeNew" tagprefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD  runat="server">
		<title>常用类别</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<script language="javascript">
						
			function Tree_Click()
			{
			    if(document.all.hidispoint.value=="1")
			    {
			    	if (document.all.ctrcatalogtreeNew1_tvCatalog.clickedNodeIndex !=null&&document.all.ctrcatalogtreeNew1_tvCatalog.getTreeNode(document.all.ctrcatalogtreeNew1_tvCatalog.clickedNodeIndex).getChildren().length<=0)
				    {
					    var lngID =document.all.ctrcatalogtreeNew1_tvCatalog.getTreeNode(document.all.ctrcatalogtreeNew1_tvCatalog.clickedNodeIndex).getAttribute("ID");
					    var sText =document.all.ctrcatalogtreeNew1_tvCatalog.getTreeNode(document.all.ctrcatalogtreeNew1_tvCatalog.clickedNodeIndex).getAttribute("Text");
					    window.parent.document.all.hidCatalog.value=lngID+"@"+sText;
				    }
				    else
				    {
					    window.parent.document.all.hidCatalog.value="@";
				    }
			    }
			    else
			    {			    
				    if (document.all.ctrcatalogtreeNew1_tvCatalog.clickedNodeIndex !=null)
				    {
					    var lngID =document.all.ctrcatalogtreeNew1_tvCatalog.getTreeNode(document.all.ctrcatalogtreeNew1_tvCatalog.clickedNodeIndex).getAttribute("ID");
					    var sText =document.all.ctrcatalogtreeNew1_tvCatalog.getTreeNode(document.all.ctrcatalogtreeNew1_tvCatalog.clickedNodeIndex).getAttribute("Text");
					    window.parent.document.all.hidCatalog.value=lngID+"@"+sText;
				    }
				    else
				    {
				        alert("请选择叶子节点!");
					    window.parent.document.all.hidCatalog.value="@";
				    }
				}
			}
			
			
			function Tree_DBClick()
			{			    
				if(document.all.hidispoint.value=="1")
			    {			    
				    if (document.all.ctrcatalogtreeNew1_tvCatalog.clickedNodeIndex !=null&&document.all.ctrcatalogtreeNew1_tvCatalog.getTreeNode(document.all.ctrcatalogtreeNew1_tvCatalog.clickedNodeIndex).getChildren().length<=0)
				    {
					    var lngID =document.all.ctrcatalogtreeNew1_tvCatalog.getTreeNode(document.all.ctrcatalogtreeNew1_tvCatalog.clickedNodeIndex).getAttribute("ID");
					    var sText =document.all.ctrcatalogtreeNew1_tvCatalog.getTreeNode(document.all.ctrcatalogtreeNew1_tvCatalog.clickedNodeIndex).getAttribute("Text");
					    window.parent.document.all.hidCatalog.value=lngID+"@"+sText;
					    window.parent.returnValue=window.parent.document.all.hidCatalog.value;
				        window.parent.close();
				    }
				    else
				    {
				        alert("请选择叶子节点!");
					    window.parent.document.all.hidCatalog.value="@";
				    }
				}
				else
				{
				    if (document.all.ctrcatalogtreeNew1_tvCatalog.clickedNodeIndex !=null)
				    {
					    var lngID =document.all.ctrcatalogtreeNew1_tvCatalog.getTreeNode(document.all.ctrcatalogtreeNew1_tvCatalog.clickedNodeIndex).getAttribute("ID");
					    var sText =document.all.ctrcatalogtreeNew1_tvCatalog.getTreeNode(document.all.ctrcatalogtreeNew1_tvCatalog.clickedNodeIndex).getAttribute("Text");
					    window.parent.document.all.hidCatalog.value=lngID+"@"+sText;
					    window.parent.returnValue=window.parent.document.all.hidCatalog.value;
				        window.parent.close();
				    }
				    else
				    {
					    window.parent.document.all.hidCatalog.value="@";
				    }
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
						    <uc2:ctrcatalogtreeNew ID="ctrcatalogtreeNew1" runat="server" />						    
						    <input id="hidispoint" type="hidden" value="<%=IsPoint%>" />
						</TD>
					</TR>
				</TABLE>
			</FORM>
		</FONT>
	</body>
</HTML>
