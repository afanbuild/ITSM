<%@ Register TagPrefix="uc1" TagName="CtrRight" Src="../Controls/CtrRight.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmEditRight" Codebehind="frmEditRight.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
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
		
		
		function SelectOperates()
		{
		    var url="frmPopOperates.aspx?OperateID="+document.all.txtOperateId.value+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
		    window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=501,height=517,left=250,top=100");		    		    
		}
		
		
		
		function PopSelectDialog()
		{
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
		
		
		function AddExtDeptList()
		{
		   var value=window.showModalDialog("frmpopdept.aspx");
			if(typeof(value) != "undefined" && value.length>1)
			{
				arr=value.split("@");
				if(document.all.txtDeptNames.value == "")
				{
					document.all.txtDeptNames.value=arr[1];
				}
				else
				{
				    document.all.txtDeptNames.value=document.all.txtDeptNames.value + ";" + arr[1];
				}
				if(document.all.hidDeptList.value == "")
				{
				   document.all.hidDeptList.value=arr[0];
				}
				else
				{
				    document.all.hidDeptList.value=document.all.hidDeptList.value + "," + arr[0];
				}
			}
		}
		
		function SelectPDept()
		{
		    var url="frmpopdept.aspx?Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmeditright";
		    window.open(url,"","toolbar=no,menu=no,width=400,height=300,left=150,top=100");
		}
		
		function SelectStaff()
		{	   
		     var url="frmSelectStaffRight.aspx?Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmeditright";
		    window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=450,height=500,left=250,top=100");
		}
		
		function SelectActor()
		{
		//==zxl==
            var url="frmPopActor.aspx?Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmeditright";
             window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=500,height=520,left=250,top=80");		  
		}
		
		function clearDeptList()
		{
		  document.all.txtDeptNames.value = "";
		  document.all.hidDeptList.value = "";
		}


		</script>
	<body>
		<form id="Form1" method="post" runat="server">
		    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
		    <br />
			<table id="tbMain" class="listContent">
				<tr>
					<td width="70" class="listTitle">权限ID:
					</td>
					<td class="list"><asp:label id="labID" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="listTitle">操作项:</td>
					<td class="list">
						<asp:textbox id="txtOperateName" runat="server" Width="179px" ReadOnly="True"></asp:textbox><input id="cmdPopOperates" onclick="SelectOperates()" type="button" value="..." name="Button1" class="btnClass2" /><asp:requiredfieldvalidator id="OpIDRequired" runat="server" ControlToValidate="txtOperateId" ErrorMessage="RequiredFieldValidator">*</asp:requiredfieldvalidator><asp:textbox id="txtOperateId" runat="server" style="display:none;" Width="0px"></asp:textbox></td>
				</tr>
				<tr>
					<td class="listTitle">权限对象类别:
					</td>
					<td class="list"><asp:dropdownlist id="dpdObjectType" runat="server"></asp:dropdownlist><asp:requiredfieldvalidator id="ObjectTypeRequired" runat="server" ControlToValidate="dpdObjectType" ErrorMessage="RequiredFieldValidator">*</asp:requiredfieldvalidator></td>
				</tr>
				<tr>
					<td class="listTitle">对象ID:
					</td>
					<td class="list"><asp:textbox id="txtObjectId" runat="server" style="display:none;" Width="0px"></asp:textbox><asp:textbox id="txtObjectName" runat="server" Width="179px" ReadOnly="True"></asp:textbox><INPUT id="cmdPop" onclick="PopSelectDialog()" type="button" value="..." class="btnClass2">
					</td>
				</tr>
				<tr id="trRightRange">
					<td style="HEIGHT: 1px" class="listTitle">权限范围:
					</td>
					<td style="HEIGHT: 1px" class="list"><asp:dropdownlist id="dpdRightRange" runat="server"></asp:dropdownlist></td>
				</tr>
				<TR style="display:none;">
					<TD class="listTitle"><FONT face="宋体" >扩展范围</FONT></TD>
					<TD class="list">
						<asp:textbox id="txtDeptNames" runat="server" Width="268px" ReadOnly="True"></asp:textbox>
						<INPUT id="Button1" onclick="AddExtDeptList()" type="button" value="添加"  class="btnClass">
						<INPUT id="Button2" onclick="clearDeptList()" type="button" value="清除" class="btnClass"></TD>
				</TR>
				<tr>
					<td class="listTitle">权限值:
					</td>
					<td class="list"><uc1:ctrright id="uRight" runat="server"></uc1:ctrright></td>
				</tr>
				<tr height="20">
					<td colSpan="2" class="listTitle"><font color="#006600">!注:对分析类操作,在权限设定中,只对"可读"选项有效,"可读"=允许操作,反之不允许操作</font>
					</td>
				</tr>
				<tr>
					<td class="list" colspan=2 align="center"><asp:button id="cmdSave" runat="server" Text="保存" onclick="cmdSave_Click" CssClass="btnClass"></asp:button>
					<INPUT id="cmdExit" onclick="javascript:window.close()" type="button" value="取消" class="btnClass">
				</tr>
			</table>
			<input id="hidDeptList" runat="server" type="hidden" />
			<input id="hidTypeID" runat="server" type="hidden" />
		</form>
	</body>
</HTML>
