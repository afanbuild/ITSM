<%@ Page Language="c#" Inherits="Epower.ITSM.Web.AppForms.Allfrmselectstaffleft" EnableEventValidation="false"
    CodeBehind="Allfrmselectstaffleft.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>frmSelectDept</title>
     <style>
        .TreeViewSelectedNode
        {
            background-color: Yellow;
            color: Red;
        }
      </style>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <script  type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>

    <script language="javascript">
			var blnClicked = false;
			//=================================
			function Tree_Click() {			    			    
			    var selectedNodeID = Form1.elements["tvDept_SelectedNode"].value;		
			    var gourl;	    
			    if (selectedNodeID != "") {
			        var selectedNode = document.getElementById(selectedNodeID);
			        var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
			        value = value.substring(value.lastIndexOf('\\') + 1);
			        var text = selectedNode.innerText;
                    
                   
			        gourl = "Allfrmselectstaffright.aspx?DeptID="+value;			        
			    }
			    else
			    {
			       gourl ="";
			    }
			    
			    if (gourl != "") {
			        window.parent.userinfo.location = gourl;
			    }
			 }
    </script>

</head>
<body leftmargin="0" topmargin="0">
    <form id="Form1" method="post" runat="server">
    <table>
        <tr>
            <td>
                <asp:TreeView ID="tvDept" runat="server" EnableClientScript="true" >
                    <SelectedNodeStyle CssClass="TreeViewSelectedNode" />
                </asp:TreeView>
               
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
