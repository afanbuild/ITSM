<%@ Register TagPrefix="uc1" TagName="CtrCatalogTree" Src="../Controls/CtrCatalogTree.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.InformationManager.frmselectSubject" Codebehind="frmselectSubject.aspx.cs" %>

<%@ Register Src="ctrSubjecttree.ascx" TagName="ctrSubjecttree" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>知识库类别</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">
		
function Tree_Click(){
              var selectedNodeID = Form1.elements["CtrSubjecttree1_TreeView1_SelectedNode"].value;
			    if (selectedNodeID != "") {
			        var selectedNode = document.getElementById(selectedNodeID);
			        var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
			        value = value.substring(value.lastIndexOf('\\') + 1);
			        var text = selectedNode.innerText;
			        //return value.substring(value.lastIndexOf('\\') + 1);
			        window.parent.document.all.hidCatalogID.value=value+"@"+text;

			        //window.parent.document.all.hidDeptID.value = value + "@" + text;
			    }
			    else
			    {
			       // window.parent.document.all.hidDeptID.value="@";
			       window.parent.document.all.hidCatalogID.value="@";
			    }
			 }
		</script>
	</HEAD>
	<body leftmargin="0" topmargin="0">
		<form id="Form1" method="post" runat="server">
			<table>
				<tr>
					<td>
                        &nbsp;<uc2:ctrSubjecttree ID="CtrSubjecttree1" runat="server" />
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
