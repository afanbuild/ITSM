<%@ Register TagPrefix="uc1" TagName="CtrCatalogTree" Src="../Controls/CtrCatalogTree.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmSelectCatalog" Codebehind="frmSelectCatalog.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>≤ø√≈</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">		
					
        function Tree_Click()
	        {
	       // debugger;
	            
	              var selectedNodeID=Form1.elements["CtrCatalogTree1_tvCatalog_SelectedNode"].value;
	              if(selectedNodeID !=""){
	                var selectedNode=document.getElementById(selectedNodeID);
	                var value=selectedNode.href.substring(selectedNode.href.indexOf(",")+3,selectedNode.href.length-2);
	                var text=selectedNode.innerHTML;
	                value=value.substring(value.lastIndexOf('\\')+1);
	               
	              window.parent.document.all.hidCatalogID.value=value +"@"+text;
	             //  alert("sdfsdf");
	                }
	                else
	                {
	                    //window.parent.document.all.hidCatalogID.value="@";
	                   window.parent.document.all.hidCatalogID.value="@";
	                }
	        
	        }
			
		</script>
	</HEAD>
	<body leftmargin="0" topmargin="0">
		<form id="Form1" method="post" runat="server">
			<table>
				<tr>
					<td><uc1:CtrCatalogTree id="CtrCatalogTree1" runat="server"></uc1:CtrCatalogTree>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
