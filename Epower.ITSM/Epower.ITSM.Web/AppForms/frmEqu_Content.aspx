<%@ Page language="c#" Inherits="Epower.ITSM.Web.AppForms.frmEqu_Content" Codebehind="frmEqu_Content.aspx.cs"  EnableEventValidation="false"   %>
<%@ Register TagPrefix="uc1" TagName="ctrSubjecttree"  Src="../Controls/ctrServertree.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>frmSubjectContent</title>
        <link href="../Js/css/ui-lightness/jquery-ui-1.8.20.custom.css" rel="stylesheet" type="text/css" />
		<script type="text/javascript" src="../Js/jquery-1.7.2.min.js"></script>
	</HEAD>
	
		<script language="javascript">
		var blnHasClicked = false;
		
		    function Tree_Click()
	        {
	            var lngPreDeptID = 0;
	            if(blnHasClicked ==true)
	            {
	                var selectedNodeID=Form1.elements["CtrSubjecttree1_tvSubject_SelectedNode"].value;
	               
	                if(selectedNodeID !=""){
	                var selectedNode=document.getElementById(selectedNodeID);
	                var value=selectedNode.href.substring(selectedNode.href.indexOf(",")+3,selectedNode.href.length-2);
	               // var text=selectedNode.innerHTML;
	                var  value=value.substring(value.lastIndexOf('\\')+1);
	                lngPreDeptID=value;
	                }       
	            }
	            else
	            {
	                blnHasClicked=true;
	                
	            }
	            //=====================zxl====
	            
	            
	              var selectedNodeID_2=Form1.elements["CtrSubjecttree1_tvSubject_SelectedNode"].value;
	              if(selectedNodeID_2 !=""){
	                var selectedNode_2=document.getElementById(selectedNodeID_2);
	                var value=selectedNode_2.href.substring(selectedNode_2.href.indexOf(",")+3,selectedNode_2.href.length-2);
	                var text=selectedNode_2.innerHTML;
	                var  value=value.substring(value.lastIndexOf('\\')+1);
	                var lngDeptID=value;
	                
	                window.parent.subjectinfo.location="<%=sUrl%>?subjectid="+lngDeptID;

	                }
	        
	        }
		
		
		
		
		</script>
	<body >
		<form id="Form1" method="post" runat="server">
		    <uc1:ctrSubjecttree id="CtrSubjecttree1" runat="server"></uc1:ctrSubjecttree>
		</form>
	</body>
</HTML>
