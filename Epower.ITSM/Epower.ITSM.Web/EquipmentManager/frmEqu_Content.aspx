<%@ Page language="c#" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_Content" Codebehind="frmEqu_Content.aspx.cs" EnableEventValidation="false"  %>
<%@ Register TagPrefix="uc1" TagName="ctrSubjecttree" Src="ctrSubjecttree.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>frmSubjectContent</title>
		
	</HEAD>
		
		<script type="text/javascript">		    
		    var blnHasClicked = false;
		    function Tree_Click() {
		        var source_from = '<%=sType%>';
                //debugger;
		        var selectedNodeID = Form1.elements["CtrSubjecttree1_tvSubject_SelectedNode"].value;
		        if (selectedNodeID != "") {
		            var selectedNode = document.getElementById(selectedNodeID);
		            var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
		            var text = selectedNode.innerHTML;
		            //alert("Text = " + text + "\r\n" + "Value = " + value)
		            value = value.substring(value.lastIndexOf('\\') + 1)
		            
		            if (source_from == "3") {
		            window.parent.list.location = "<%=sUrl%>?subjectid=" + value; //+"&DeptText="+lngDeptText;
		            } else {
		            window.parent.subjectinfo.location = "<%=sUrl%>?subjectid=" + value; //+"&DeptText="+lngDeptText;    
		            }
		        }
		        return;		       
		    }
		   
		</script>
	<body >
		<form id="Form1" method="post" runat="server">
		    <uc1:ctrSubjecttree id="CtrSubjecttree1" runat="server"></uc1:ctrSubjecttree>
		</form>
	</body>
</HTML>
