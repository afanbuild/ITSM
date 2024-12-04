<%@ Register TagPrefix="uc1" TagName="CtrActorTree" Src="../Controls/ctractortreeV52.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmActorTreeForMember" Codebehind="frmActorTreeForMember.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<meta name="vs_snapToGrid" content="True">
		<title>frmActorTreeForMember</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">
		function Tree_Click() {
		    var nodeValue = GetSelectedNode();
		    if (nodeValue != undefined && nodeValue != "undefined" && nodeValue != "") {
		        window.parent.ActorMember.location = "frmActorMemberList.aspx?actorId=" + nodeValue;
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

		</script>
	</HEAD>
	<body bgcolor=#ffffff>
		<form id="Form1" method="post" runat="server">
		<FONT face="ËÎÌו">
			<uc1:CtrActorTree id="CtrActorTree1" runat="server"></uc1:CtrActorTree></FONT>
		</form>
	</body>
</HTML>
