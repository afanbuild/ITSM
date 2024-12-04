<%@ Register TagPrefix="uc1" TagName="CtrRight" Src="../Controls/CtrRight.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.mydestop.frmEditRight" Codebehind="frmEditRight.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
	    <base target="_self" />
		<title>权限编辑</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	
	</HEAD>
		<script language="javascript">
		function CanRead_Click(ctl)
		{
			reg=/chkCanRead/g;
			var idtag=ctl.id.replace(reg,"");
			if (ctl.checked==false) {
				document.all(idtag+"chkCanAdd").checked=false;
				document.all(idtag+"chkCanModify").checked=false;
				document.all(idtag+"chkCanDelete").checked=false;
			}
		}
		
		function PopSelectDialog()
		{
		//debugger;
			switch(document.all.dpdObjectType.value)
			{
				case "10":
					SelectPDept();
					break;
					
				case "20":
					SelectStaff();
					break;
				
				case "30":
					SelectActor();
					break;
				default:
				    alert("请选择授权对象类型！");
				    break;
			}
		}
		function SelectPDept()
		{
		   var  url="frmpopdept.aspx?Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmeditright";
		   window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=400,height=300px,left=150,top=100");
			}
		
		function SelectStaff()
		{
		    //var url="frmSelectPerson.htm";
		    var url="frmperson_zxl.aspx";
		    window.open(url,"E8","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=400,height=300px,left=150,top=100");
	
		}
		
		function SelectActor()
		{
		   
		    var url="../DeptForms/frmPopActor.aspx?Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
		    window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=400,height=300px,left=150,top=100");

		}
		
		function clearDeptList()
		{
		  document.all.txtDeptNames.value = "";
		  document.all.hidDeptList.value = "";
		}


		</script>
	<body>
		<form id="Form1" method="post" runat="server">
		    <input id="hidClientId_ForOpenerPage" type="hidden" value="0" runat="server" />
		    <br />
			<table id="tbMain" class="listContent">
				<tr>
					<td width="70" class="listTitle">权限ID:
					</td>
					<td class="list"><asp:label id="labID" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="listTitle">权限对象类别:
					</td>
					<td class="list"><asp:dropdownlist id="dpdObjectType" runat="server"></asp:dropdownlist><asp:requiredfieldvalidator id="ObjectTypeRequired" runat="server" ControlToValidate="dpdObjectType" ErrorMessage="RequiredFieldValidator">*</asp:requiredfieldvalidator></td>
				</tr>
				<tr>
					<td class="listTitle">对象ID:
					</td>
					<td class="list"><asp:textbox id="txtObjectId" runat="server" Width="0px"></asp:textbox><asp:textbox id="txtObjectName" runat="server" Width="179px" ReadOnly="True"></asp:textbox><INPUT id="cmdPop" onclick="PopSelectDialog()" type="button" value="..." class="btnClass2">
					</td>
					
				</tr>
				<tr>
					<td class="list" colspan=2 align="center"><asp:button id="cmdSave" runat="server" Text="保存" onclick="cmdSave_Click" CssClass="btnClass"></asp:button>
					<INPUT id="cmdExit" onclick="javascript:window.close()" type="button" value="取消" class="btnClass">
				</tr>
			</table>
		</form>
	</body>
</HTML>
