<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmEditOperate" Codebehind="frmEditOperate.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>操作项编辑</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<table id="tbMain" border="0" cellpadding="0" cellspacing="0">
				<tr height="30">
					<td width="20">
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td>
					</td>
					<td width="70">操作ID:
					</td>
					<td>
						<asp:Label id="labID" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td>
					</td>
					<td>操作名称:
					</td>
					<td>
						<asp:TextBox id="txtOpName" runat="server" Width="184px"></asp:TextBox>
						<asp:RequiredFieldValidator id="OpNameRequired" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtOpName">*</asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr>
					<td>
					</td>
					<td>操作类别:
					</td>
					<td>
						<asp:DropDownList id="dpdOpType" runat="server" Width="184px"></asp:DropDownList>
						<asp:RequiredFieldValidator id="OpTypeRequired" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="dpdOpType">*</asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr>
					<td>
					</td>
					<td><!--SQL语句:-->
					</td>
					<td>
						<asp:TextBox id="txtSql" runat="server" TextMode="MultiLine" Width="184" Visible="False"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td>
					</td>
					<td><!--参数表:-->
					</td>
					<td>
						<asp:TextBox id="txtParam" runat="server" TextMode="MultiLine" Visible="False"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td>
					</td>
					<td><!--关联系统:-->
					</td>
					<td>
						<asp:TextBox id="txtConnectSystem" runat="server" Width="184px" Visible="False"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td>
					</td>
					<td>描述:
					</td>
					<td>
						<asp:TextBox id="txtDesc" runat="server" TextMode="MultiLine"></asp:TextBox>
					</td>
				</tr>
				<tr height="20">
					<td>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td>
					</td>
					<td><asp:Button id="cmdSave" runat="server" Text="保存" onclick="cmdSave_Click"></asp:Button>
					</td>
					<td><INPUT id="cmdCancel" type="button" value="取消" onclick="javascript:window.close()">
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
