<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmpopactorSelect.aspx.cs" Inherits="Epower.ITSM.Web.DeptForms.frmpopactorSelect" %>
<%@ Register TagPrefix="uc1" TagName="CtrActorTree" Src="../Controls/ctractortreeV52.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    
</head>
<script type="text/javascript">
     function Tree_Click()
	        {
	            
	              var selectedNodeID=Form1.elements["CtrActorTree1_TreeView1_SelectedNode"].value;
	              if(selectedNodeID !=""){
	                var selectedNode=document.getElementById(selectedNodeID);
	                var value=selectedNode.href.substring(selectedNode.href.indexOf(",")+3,selectedNode.href.length-2);
	                var text=selectedNode.innerHTML;
	                value=value.substring(value.lastIndexOf('\\')+1);
	               
	              window.parent.document.all.hidActorID_Name.value=value +"@"+text;	              
	              window.parent.document.all.selectDeptTips.innerHTML = "您选择的用户组是：<span style='color:red;'>" + text +"</span>";
	             //  alert("sdfsdf");
	                }
	                else
	                {
	                    window.parent.document.all.hidActorID_Name.value="@";
	                    window.parent.document.all.selectDeptTips.innerHTML ="";
	                   // document.all.hidActorID_Name.value="@";
	                }
	        
	        }
	    function Tree_DBClick()
		{
			if (document.all.CtrActorTree1_tvActor.clickedNodeIndex !=null)
			{
				document.all.cmdOK.click();
					//QueryAllChillNode(treeNode);//有子节点则递归所有子节点
			}
		}
</script>
<body leftMargin="0" topMargin="0">
		<FONT face="宋体" style="color:Black;">
			<FORM id="Form1" method="post" runat="server">
				<TABLE id="Table1">
					<TR>
						<TD>
							 <uc1:CtrActorTree id="CtrActorTree1" runat="server" ></uc1:CtrActorTree>
                        </TD>
					</TR>
				</TABLE>
			</FORM>
		</FONT>
	</body>
</html>
