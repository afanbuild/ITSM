<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.FrmSMSSend" Codebehind="FrmSMSSend.aspx.cs" %>

<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>发送短消息</title>
		<base target="_self"></base>
		<META HTTP-EQUIV="pragma" CONTENT="no-cache">
		<META HTTP-EQUIV="Cache-Control" CONTENT="no-cache, must-revalidate">
		<META HTTP-EQUIV="expires" CONTENT="Mon, 23 Jan 1978 20:52:30 GMT">
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript" type="text/javascript" src="../Js/App_Common.js"> </script>
		<script language="javascript" type="text/javascript">
		    function GetRandom() {
                return Math.floor(Math.random() * 1000 + 1);
            }
		</script>
		
	</HEAD>
	<body onkeydown="if (event.keyCode==116){reload.click()}">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" width="90%" class="listContent" align="center" >
				<TR>
					<TD align="center" style="HEIGHT: 30px" class="listTitle">
							<uc1:CtrTitle id="CtrTitle1" runat="server"></uc1:CtrTitle></TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" width="90%" align="center" class="listContent">
				<TR>
					<TD width="30%" class="listTitle" noWrap>接收人</TD>
					<TD colSpan="1" rowSpan="1" class="list" style="width: 92%">
                        <uc2:UserPicker ID="UserPicker1" runat="server" />
                    </TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 72px" width="30%" colSpan="1" rowSpan="1" class="listTitle" noWrap>信息内容</TD>
					<TD style="HEIGHT: 72px; width: 92%;" colSpan="1" rowSpan="1" class="list">
						<asp:TextBox id="txtContent" runat="server" Height="153px" TextMode="MultiLine"
							 Width="100%" onblur="MaxLength(this,500,'信息内容长度超出限定长度:');"></asp:TextBox></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2" class="listTitle">
						<asp:button id="cmdSend" runat="server" Text="发送" onclick="cmdSend_Click" CssClass="btnClass"></asp:button></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
