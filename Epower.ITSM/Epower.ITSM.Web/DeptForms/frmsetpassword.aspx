<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmSetPassword" Codebehind="frmSetPassword.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD runat="server">
		<title>重设密码</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script>
		<!--
			function SetMsg()
			{			
				document.all.lblMsg.text ="正在联系AD服务器,请稍后......";
			}
		-->
		</script>
</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
		    <br />
			<TABLE id="Table2" width="95%" align="center" class="listContent">
			    <TR>
				    <TD style="WIDTH: 209px" class="listTitle">
					    <DIV style="DISPLAY: inline; WIDTH: 176px; HEIGHT: 18px" align="right" >登录用户名称：</DIV>
				    </TD>
				    <TD class="list"><asp:textbox id="txtLoginName" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
			    </TR>
			    <TR>
				    <TD style="WIDTH: 209px; HEIGHT: 23px" class="listTitle">
					    <DIV style="DISPLAY: inline; WIDTH: 176px; HEIGHT: 18px" align="right" >用户名称：</DIV>
				    </TD>
				    <TD style="HEIGHT: 23px" class="list"><asp:textbox id="txtName" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
			    </TR>
			    <TR>
				    <TD style="WIDTH: 209px; HEIGHT: 28px" class="listTitle">
					    <DIV style="DISPLAY: inline; WIDTH: 176px; HEIGHT: 18px" align="right">新密码：</DIV>
				    </TD>
				    <TD style="HEIGHT: 28px" vAlign="middle" class="list">
					    <asp:TextBox id="txtFistPwd" runat="server" Width="135px" TextMode="Password"></asp:TextBox></TD>
			    </TR>
			    <TR>
				    <TD style="WIDTH: 209px; HEIGHT: 28px" class="listTitle">
					    <DIV style="DISPLAY: inline; WIDTH: 176px; HEIGHT: 18px" align="right" >确认新密码：</DIV>
				    </TD>
				    <TD style="HEIGHT: 28px" vAlign="middle" class="list">
					    <asp:TextBox id="txtLastPwd" runat="server" Width="135px" TextMode="Password"></asp:TextBox>
					    <asp:CompareValidator id="CompareValidator1" runat="server" ControlToValidate="txtLastPwd" ErrorMessage="两次输入不一致"
						    ControlToCompare="txtFistPwd"></asp:CompareValidator></TD>
			    </TR>
			    <TR class="listTitle">
				    <TD align="center" colSpan="2" class="listTitle"><FONT face="宋体"><asp:button id="cmdSave" runat="server" Text="确认" onclick="cmdSave_Click" ></asp:button>&nbsp;&nbsp; 
							    <INPUT onclick="javascript:window.close()" type="button" value="取消" class="btnClass"></FONT></TD>
			    </TR>
		    </TABLE>
		    <asp:Label id="lblMsg" runat="server"></asp:Label>
		    <INPUT id="hidUserID" type="hidden" runat="server"> <INPUT id="hidPassWord" type="hidden" runat="server">
		</form>
	</body>
</HTML>
