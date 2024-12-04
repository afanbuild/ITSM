<%@ Register TagPrefix="uc1" TagName="CtrActorTree" Src="../Controls/CtrActorTreeV52.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmActorTree" Codebehind="frmActorTree.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD runat="server">
		<title>frmActorTree</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript">
<!--
	function Tree_Click() {

	    var nodeValue = GetSelectedNode();
	    if (nodeValue != undefined && nodeValue != "undefined" && nodeValue != "") {
	        window.parent.ActorInfo.location = "frmActorEdit.aspx?actorId=" + nodeValue;
	    }

}

function GetSelectedNode() {
    var selectedNodeID = theForm.elements["CtrActorTree1_TreeView1_SelectedNode"].value;
    if (selectedNodeID != "") {
        var selectedNode = document.getElementById(selectedNodeID);
        var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
        value = value.substring(value.lastIndexOf('\\') + 1);
        if (value.substring(0, 2) == "D_") {
            return value.substring(2);
        }
        else {
            return value;
        }
    }
}  

//-->
		</script>
</HEAD>
	<body >
		<form id="Form1" method="post" runat="server">
			<uc1:CtrActorTree id="CtrActorTree1" runat="server"></uc1:CtrActorTree></form>
	</body>
</HTML>
