<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmCatalogContent" Codebehind="frmCatalogContent.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="CtrCatalogTree" Src="../Controls/CtrCatalogTree.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>frmCatalogContent</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">
		var blnHasClicked = false;
		function Tree_Click()
		{
			var lngPreCatalogID = 0;
			if(blnHasClicked == true)
			{ 

			}
			else
			{
				blnHasClicked = true;
}

var nodeValue = GetSelectedNode();
if (nodeValue != undefined && nodeValue != "undefined" && nodeValue != "") {
    window.parent.cataloginfo.location = "frmcatalogedit.aspx?catalogid=" + nodeValue; //+"&CatalogText="+lngCatalogText;
}

}

function GetSelectedNode() {
    var selectedNodeID = theForm.elements["CtrCatalogTree1_tvCatalog_SelectedNode"].value;
    if (selectedNodeID != "") {
        var selectedNode = document.getElementById(selectedNodeID);
        var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
        return value.substring(value.lastIndexOf('\\') + 1);
    }
}  

		</script>
	</HEAD>
	<body bgcolor="#ffffff">
		<form id="Form1" method="post" runat="server">
			<uc1:CtrCatalogTree id="CtrCatalogTree1" runat="server"></uc1:CtrCatalogTree>
		</form>
	</body>
</HTML>
