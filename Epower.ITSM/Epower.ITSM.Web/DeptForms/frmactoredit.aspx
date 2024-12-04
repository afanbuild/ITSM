<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmActorEdit" Codebehind="frmActorEdit.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register src="../Controls/DeptPicker.ascx" tagname="DeptPicker" tagprefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>用户组维护</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript" src="../Js/App_Common.js"></script>
		<script language="javascript">
			function SelectPActor()
			{
				var value=window.showModalDialog("frmpopactor.aspx");
				if(value != null)
				{
			        if(value.length>1)
			        {
				        arr=value.split("@");
				        document.all.txtPActorName.value=arr[1];
				        document.all.hidPActorID.value=arr[0];
			        }
				}
}

function SaveConfirm() {
    //alert(document.all.txtActorName.value);
        if(document.all.txtActorName.value=="")
        {
            document.all.txtActorName.focus();
			alert("用户组名称不能为空！");
            event.returnValue = false;
        }
}
		</script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<table id="tbmain" align="left" class="listContent" width="100%">
				<tr>
					<td colSpan="2" class="list">
						<uc1:CtrTitle id="CtrTitle" runat="server"></uc1:CtrTitle></td>
				</tr>
				<tr height="40">
					<td colspan="2" class="listTitle">
						<asp:Button id="cmdAdd" runat="server" Text="新增" onclick="cmdAdd_Click" CssClass="btnClass"></asp:Button>&nbsp;&nbsp;
						<asp:button id="cmdSave" runat="server" Text="保存" onclick="cmdSave_Click" CssClass="btnClass" OnClientClick="SaveConfirm();"></asp:button>&nbsp;&nbsp;
						<asp:button id="cmdDelete" runat="server" Text="删除" onclick="cmdDelete_Click" CssClass="btnClass"></asp:button>
					</td>
				</tr>
				<tr>
					<td class="listTitle">用户组编号:
					</td>
					<td class="list">
                        <asp:Label ID="txtNo" runat="server" Text=""></asp:Label></td>
				</tr>
				<tr>
					<td class="listTitle">用户组所属部门:
					</td>
					<td class="list">
                        
                        <uc2:DeptPicker ID="DeptPicker1" runat="server" />
                        
                    </td>
				</tr>
				<tr>
					<td class="listTitle">用户组名称:
					</td>
					<td class="list"><asp:textbox id="txtActorName" runat="server" MaxLength="25"></asp:textbox>
					<label style="color:Red;">*</label>
					</td>
				</tr>
				<tr>
					<td  class="listTitle">用户组说明:</td>
					<td class="list"><asp:textbox id="txtActorDesc" runat="server" TextMode="MultiLine" Height="72px" Width="232px" onblur="MaxLength(this,250,'用户组说明长度超出限定长度:');"></asp:textbox></td>
				</tr>
			</table>
			<asp:Label id="labMsg" runat="server" ForeColor="Red" Font-Bold="True" Visible="false"></asp:Label>
			<asp:TextBox id="txtPActorName" runat="server" Visible="false"></asp:TextBox>
			<INPUT id="hidPActorID" type="hidden" runat="server" NAME="hidPActorID">
			
			
			<INPUT id="hidActorID" type="hidden" runat="server" NAME="hidActorID">
		</form>
	</body>
</HTML>
