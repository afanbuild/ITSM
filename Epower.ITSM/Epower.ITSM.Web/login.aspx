<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Epower.ITSM.Web.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>系统提示</title>
    <script type="text/javascript">
//<!--
function toHome(){ window.top.location.href='<%=sUrl%>';}
window.setTimeout("toHome()",5000);
//-->
</script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" height="100%" border="0">
		<tr>
			<td><p>&nbsp;</p>
				<p></p>
			</td>
		</tr>
		<tr>
			<td align="center" valign="top">
				<P><font color="#ff0000" size="5"><STRONG><EM>系统超时! <br />5秒钟之后将会带您进入登录页!</EM></STRONG></font></P>
				<P><font color="#ff0000" size="6"><STRONG><EM></EM></STRONG></font>&nbsp;</P>
			</td>
		</tr>
		<tr>
			<td></td>
		</tr>
	</table>
    </form>
    <p>
&nbsp;</p>
</body>
</html>
