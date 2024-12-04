<%@ Page Language="C#" CodeBehind="frmselectSubject.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmselectSubject" %>

<%@ Register Src="~/Controls/ctrServertree.ascx" TagName="ctrSubjecttree" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="CtrCatalogTree" Src="../Controls/CtrCatalogTree.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<HTML>
	<HEAD id="HEAD1" runat="server">
		<title></title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">
		
         function Tree_Click()
	     {
	            
	              var selectedNodeID=Form1.elements["CtrSubjecttree1_tvSubject_SelectedNode"].value;
	              if(selectedNodeID !=""){
	                var selectedNode=document.getElementById(selectedNodeID);
	                var value=selectedNode.href.substring(selectedNode.href.indexOf(",")+3,selectedNode.href.length-2);
	                var text=selectedNode.innerHTML;
	                value=value.substring(value.lastIndexOf('\\')+1);
	              
	               window.parent.document.all.hidCatalogID.value=value+"@"+text;
	           
	                }
	                else
	                {
	                    window.parent.document.all.hidCatalogID.value="@";
	                }
	        
	        }           
			
		</script>
	</HEAD>
	<body leftmargin="0" topmargin="0">
		<form id="Form1" method="post" runat="server">
			<table width="100%" height="100%">
				<tr width="100%">
					<td>
					
                        &nbsp;<uc2:ctrSubjecttree ID="CtrSubjecttree1" runat="server" />
                       
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
