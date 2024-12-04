<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmPopOperates" Codebehind="frmPopOperates.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="CtrActorTree" Src="../Controls/ctractortreeV52.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD  runat="server">
		<title>操作项</title>
		<base target="_self" />
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">
			
			
			function Click_Cancel()
			{
				window.returnValue="@";
				window.close();
			}
			
			function List_OnClick()
			{
				if(document.all.lstOperates.selectedIndex>-1)
				{
					var lngID =document.all.lstOperates.value;
					//var sText =document.all.lstOperates.options(document.all.lstOperates.selectedIndex).text;
					document.all.hidOperateID_Name.value= lngID;
				}
				else
				{
					document.all.hidOperateID_Name.value="@@";
				}
			}
			
			
			function On_ListDblClick()
			{
				Click_OK();
			}
			
		</script>
	</HEAD>
	<script language="javascript">
	    function Click_OK()
			{
			var value=document.all.hidOperateID_Name.value;
			var TypeFrm="<%=TypeFrm %>";
			if(TypeFrm =="frmright")
			{
			    frmright(value);
			}
			else{
			//==zxl===
				//window.returnValue=document.all.hidOperateID_Name.value;
				
				if(typeof(value)!="undefined" && value.length>1)
				{
				   var arr=value.split("@");
				   window.opener.document.getElementById("<%=Opener_ClientId %>txtOperateName").value=arr[1]+"(" +arr[0] + ")";
				   window.opener.document.getElementById("<%=Opener_ClientId %>txtOperateId").value =arr[0];
				   window.opener.document.getElementById("<%=Opener_ClientId %>hidTypeID").value=arr[2];
				   switch(arr[2])
				   {
				     case "10": //功能类
				        window.opener.document.getElementById("<%=Opener_ClientId %>trRightRange").style.display="none";
				        window.opener.document.getElementById("<%=Opener_ClientId %>tdRead").style.display="none";
				        break;
				    case "20": //分析、查询类
				        window.opener.document.getElementById("<%=Opener_ClientId %>trRightRange").style.display="";
				        window.opener.document.getElementById("<%=Opener_ClientId %>tdRead").style.display="";
				        window.opener.document.getElementById("<%=Opener_ClientId %>tdAdd").style.display="none";
				        window.opener.document.getElementById("<%=Opener_ClientId %>uRight_chkCanRead").checked=true;
				        break;
				    case "30"://它类，如控制普通查询页时
				        window.opener.document.getElementById("<%=Opener_ClientId %>trRightRange").style.display="";
				        window.opener.document.getElementById("<%=Opener_ClientId %>tdRead").style.display="";
				        window.opener.document.getElementById("<%=Opener_ClientId %>tdAdd").style.display="";
				        window.opener.document.getElementById("<%=Opener_ClientId %>uRight_chkCanRead").checked=false;
					    break;
					default:
					break;
				        
				   }
				
			    }
			  }
			window.close();
			}
			function frmright(value)
			{
			   if(typeof(value) != "undefined" && value.length>1)
			    {
				    arr=value.split("@");
				    window.opener.document.getElementById("<%=Opener_ClientId %>txtOpName").value=arr[1];
				    window.opener.document.getElementById("<%=Opener_ClientId %>txtOpID").value =arr[0];
    				
			    }
			    else
			    {
			         window.opener.document.getElementById("<%=Opener_ClientId %>txtOpName").value="";
			         window.opener.document.getElementById("<%=Opener_ClientId %>txtOpID").value ="";
			    }
			}
	</script>
	<body>
		<form id="Form1" method="post" runat="server">
			<table style="WIDTH: 100%; HEIGHT: 468px" class="listContent">
			    <tr>
					<td class="listTitle" width="80">权限类别:
					</td>
					<td class="list">
                        <asp:DropDownList ID="ddltRightType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddltRightType_SelectedIndexChanged"></asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td style="HEIGHT: 422px" colspan="2" class="list">
							<asp:ListBox id="lstOperates" runat="server" Width="100%" Height="100%" onclick="List_OnClick()"></asp:ListBox>
					</td>
				</tr>
				<tr>
					<td colspan="2" class="list" align="center">
						<INPUT id="cmdOK" type="button" value="确定" class="btnCLass" onclick="Click_OK()"> 
						<INPUT id="cmdCancel" type="button" value="取消" class="btnClass" onclick="Click_Cancel()">
						<INPUT type="hidden" id="hidOperateID_Name">
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
