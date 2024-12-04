<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmPopActor" Codebehind="frmPopActor.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="CtrActorTree" Src="../Controls/ctractortreeV52.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>用户组</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<script language="javascript">
	        
			function Click_OK()
			{
			    
			//debugger;
			   // var arr=document.all.hidActorID_Name.value.split("@");
			   var arr=document.getElementById("hidActorID_Name").value.split("@");
			  // var arr=strs;
			    if(arr[0]=="0")
			    {
			        alert("请选择用户组！");
			        return;
			    }
			     var type  ="<%=TypeFrm %>";
			    if(type =="frmeditrightbatch")
		        {
		            window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1]+"("+arr[0]+")";
		            window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectId").value=arr[0];
		            window.opener.document.getElementById("<%=Opener_ClientId %>hidObjectName").value=arr[1]+"("+arr[0]+")";
		            window.close();
		        }
		        else if(type=="frmright")
		        {
		            frmright(arr);
		            window.close();
		        }
		        else if(type =="frmeditright")
		        {
		            frmeditright(arr);
		            window.close();
		        }
			    else  if(type=="frmrightSeach")
			    {
			        if(typeof(arr)!="undefined" && arr.length>1)
			        {
			           // arr=value.split("@");
			           window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1]+"(" +arr[0] + ")";
			            window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectID").value=arr[0];
			        }
			        else
			        {
			            window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectID").value="";  
			        }
			        window.close();		    
			     
			    }
			     else{
			   
			    window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1]+"("+arr[0]+")";
			    window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectId").value=arr[0];
			//	window.returnValue=document.all.hidActorID_Name.value;
			    
				window.close();
				}
			}
			function Click_Cancel()
			{
				window.returnValue="@";
				window.close();
			}
        
			function Tree_DBClick()
			{
				if (document.all.CtrActorTree1_tvActor.clickedNodeIndex !=null)
				{
					document.all.cmdOK.click();
					//QueryAllChillNode(treeNode);//有子节点则递归所有子节点
				}
			}
			function frmeditright(arr)
			{
			   
				     window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1]+"(" +arr[0] +")";
				     window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectId").value =arr[0];
			    
			}
			function frmright(arr)
		    {
				    window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectID").value =arr[0];    			
		    }
		</script>
	<body leftmargin="0" topmargin="0" rightmargin="0" bottommargin="0">
		<form id="Form1" method="post" runat="server">
		    <div style="" >
		        <iframe id="Myiframe" frameborder="no" width="100%" height="80%"></iframe>
		    </div>
		    <div style=" padding-left:20px;">
		        <span id="selectDeptTips"></span>
		    </div>
		     <div style=" text-align:center;">
		        <INPUT id="cmdOK" type="button" value="确定" onclick="Click_OK()" class="btnClass"> 
				<INPUT id="cmdCancel" type="button" value="取消" onclick="Click_Cancel()" class="btnClass">
		    </div>
	
			<INPUT type="hidden" id="hidActorID_Name">
		</form>
		<script type="text/javascript">
		    document.all.Myiframe.src = "frmpopactorSelect.aspx";

		</script>
	</body>
</HTML>
