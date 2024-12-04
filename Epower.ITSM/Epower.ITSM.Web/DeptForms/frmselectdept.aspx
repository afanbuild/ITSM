<%@ Page Language="c#" Inherits="Epower.ITSM.Web.Forms.frmSelectDept" CodeBehind="frmSelectDept.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="CtrDeptTree" Src="~/DeptControls/CtrDeptTree.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>≤ø√≈</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <script  type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>
    <script language="javascript">
			
			function Tree_Click(mEvent,text)
			{		   			      
				  var selectedNodeID=Form1.elements["CtrDeptTree1_tvDept_SelectedNode"].value;
	              if(selectedNodeID !=""){	              
	                //var selectedNode=document.getElementById(selectedNodeID);
	                var node = $('#' + selectedNodeID);
	                //var value=selectedNode.href.substring(selectedNode.href.indexOf(",")+3,selectedNode.href.length-2);	                
	               
	                var value = node.attr('deptid');
	                if(value =="s1")
	                {
	                    value="1";
	                }
	                
	                var text = node.text();
	                //value=value.substring(value.lastIndexOf('\\')+1);	                
	                window.parent.document.all.hidDeptID.value=value+"@"+text;
	              }
	              else
	              {
	                 window.parent.document.all.hidDeptID.value="@";
	              }
	              
	              //window.parent.isSelected = true;
			
			}
			function Tree_DBClick(mEvent, text)
			{			
			    try{
			        
			        window.parent.Click_OK();
			    			        
			        window.parent.close();
			    }catch(e){
			        alert(e.message);
			    }
			    return true;
                
			}
    </script>
    <style type="text/css">
        #tvContainer a
        {
        	font-size: 12px;
        	color: #004175;        
        }
    </style>
</head>
<body leftmargin="0" topmargin="0">
    <form id="Form1" method="post" runat="server">
    <div id="tvContainer" >
        <uc1:CtrDeptTree ID="CtrDeptTree1" runat="server"></uc1:CtrDeptTree>
    </div>
       
    </form>
</body>
</html>

<script type="text/javascript" language="javascript">    
    $(document).ready(function(){                       
        //alert($('#tvContainer table:first td:last a:last').html());     
        $('#tvContainer table').each(function(){
            var link = $(this).find('td:last').find('a:first');                 
           
            //link.click=' ';
            var deptId = link.attr('href');            
            var rDeptId = /'(s1.*?)\'/i;
            var rDeptId2 = /s\d{1}/i;
            var str="";
                        
            try{
                str = deptId.match(rDeptId)[1];    
                if(str=="s1")            
                {
                   deptId = "s1";
                }
                else
                {
                    var arr = str.split("\\");
                    deptId = arr[arr.length-1];
                }
            }catch(e){
                try{
                    deptId = deptId.match(rDeptId2)[0];                
                }catch(e) {
                    return;   
                }                
            }                                   
            
            link.attr('deptid', deptId);
            link.attr('href',"javascript:###");
            link.dblclick(function(){
                 try{			        
                    Tree_Click(undefined, undefined);
			        window.parent.Click_OK();			    			        
			        window.parent.close();			        
			    }catch(e){
			        alert(e.message);
			    }
            });
            
            link.unbind('click');            
            link.click(function(){
                try{                                    
                    if (window.select_node != null) {                        
                        window.select_node.css('color','#004175');
                    }
                    window.select_node = $(this);              
                    window.select_node.css('color','red');
                }catch(e){  
                    alert(e.message);
                }
            });
        });
    });
</script>
