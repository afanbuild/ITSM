<%@ Register TagPrefix="uc1" TagName="ctrSubjecttree" Src="ctrSubjecttree.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.InformationManager.frmInformationContent" Codebehind="frmInformationContent.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD  runat="server">
		<title>frmSubjectContent</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script  type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>
	</HEAD>
	<script language="javascript">
	
      
	
		var blnHasClicked = false;
		function Tree_Click() {

		    var nodeValue = GetSelectedNode();
		    
		    //var hid=document.getElementById("hidCtrSubJectTree").value;

		    if (nodeValue != undefined && nodeValue != "undefined" && nodeValue != "") {
		    
		        window.parent.subjectinfo.location = "<%=sUrl%>&subjectid=" + nodeValue; //+"&DeptText="+lngDeptText;
		    }
		}

		function GetSelectedNode() {
		
		    var selectedNodeID = theForm.elements["CtrSubjecttree1_TreeView1_SelectedNode"].value;
		    
		    if (selectedNodeID != "") {
		        var selectedNode = document.getElementById(selectedNodeID);
		       
		        var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
		        return value.substring(value.lastIndexOf('\\') + 1);
		    }
		}  
		
function getSelectNodeId( nodeValue) 
{ 
return nodeValue.substring(nodeValue.lastIndexOf('//')+2)
}
		</script>
	<body  bgcolor=#ffffff>
		<form id="Form1" method="post" runat="server">
		    <uc1:ctrSubjecttree id="CtrSubjecttree1" runat="server"></uc1:ctrSubjecttree>
		  
		      <input id="hidCtrSubJectTree" type="hidden" runat="server" value="0" />
		    
		</form>
	</body>
</HTML>
<script type="text/javascript">
    $(document).ready(function(){
        var node = $('.TreeViewSelectedNode');
        node.css('color','red');
        node.css('background-color','white');
    });
</script>