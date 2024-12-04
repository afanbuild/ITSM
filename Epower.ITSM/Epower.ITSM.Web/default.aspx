<%@ Page Language="c#" EnableViewStateMac="false" Inherits="Epower.ITSM.Web.default1"
    CodeBehind="default.aspx.cs" ValidateRequest="false" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=Title%></title>
    <meta name="GENERATOR" content="Microsoft Visual Studio 7.0" />
    <meta name="CODE_LANGUAGE" content="C#" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link type="text/css" rel="stylesheet" href="App_Themes/NewOldMainPage/css.css" />

    <script language="javascript" type="text/javascript">
		    /*窗体最大化*/
            window.moveTo(0,0);
	        window.resizeTo(window.screen.availWidth,window.screen.availHeight);
			function SubmitExchange()
			{
				document.all.cmdOK.click();
}

function objsetFoucse(obj) {
    obj.focus();
}
    </script>

</head>
<body>
    <form id="From1" method="post" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td align="center" valign="middle" bgcolor="#5FA8DF">
                <table width="842" height="563" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" valign="top" background="Images/dldi.jpg">
                            <table width="78%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td height="80" colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <img src="Images/logo-a.gif" width="180" height="40">
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30" colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td height="40" colspan="2" align="center">
                                        <span class="bt_bai">Welcome To E8ITSM System</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="44%">
                                        &nbsp;
                                    </td>
                                    <td width="56%" align="left">
                                        <table class="tableempty" width="260" border="0" cellspacing="0" cellpadding="0">
                                            <logic:notempty name="GLOBAL_ERROR_MSG">
                                                <tr>
                                                    <th height="40" colspan="5" scope="row">&nbsp; </th>
                                                </tr>
                                              </logic:notempty>
                                            <tr>
                                                <th width="21" height="32" scope="row">
                                                    &nbsp;
                                                </th>
                                                <td colspan="2" align="left">
                                                    用 户 名：
                                                </td>
                                                <td width="178" colspan="2">
                                                    <asp:TextBox ID="txtUserName" runat="server" Height="22px" Width="128" onKeyUp="if (event.keyCode==13) txtPassword.focus()"
                                                        CssClass="bian"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th height="30" scope="row">
                                                    &nbsp;
                                                </th>
                                                <td colspan="2" align="left">
                                                    密&nbsp;&nbsp;&nbsp;&nbsp;码：
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="password" Height="22px" Width="128"
                                                        onKeyUp="if (event.keyCode==13) cmdOK.onclick()" CssClass="bian"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th height="30" scope="row">
                                                    &nbsp;
                                                </th>
                                                <td colspan="2" align="left">
                                                    身&nbsp;&nbsp;&nbsp;&nbsp;份：
                                                </td>
                                                <td colspan="2">
                                                    <asp:RadioButtonList ID="drIdentity" runat="server" RepeatDirection="Horizontal"
                                                        Height="40px" Width="154px">
                                                        <asp:ListItem Selected="True" Value="0">管理模式</asp:ListItem>
                                                        <asp:ListItem Value="1">自助模式</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th height="10" colspan="5" scope="row">
                                                </th>
                                            </tr>
                                            <tr>
                                                <th scope="row">
                                                    &nbsp;
                                                </th>
                                                <td colspan="4" align="center">
                                                    <table class="tableempty" width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td width="100%" align="center">
                                                                <table class="tableempty" width="100%" align="center" border="0" cellspacing="0"
                                                                    cellpadding="0">
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:Button ID="cmdOK" runat="server" Text="登录" CssClass="btnClass" OnClick="cmdOK_Click" />
                                                                        </td>
                                                                        <td align="center">
                                                                            <input type="button" class="btnClass" value="重设" onclick="document.forms[0].reset()" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="180" colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        Copyright (c) 2011-,深圳市非凡信息技术有限公司 All rights reserved
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input runat="server" id="hidFlag" type="hidden" />

    <script language="javascript" type="text/javascript">
			window.status = "建议使用：1024×768 分辨率";
			if(document.all.txtUserName.value!="")
			{
			    document.all.txtPassword.focus();
			}
			else
			{
			    document.all.txtUserName.focus();
			}
			//用户不存在
			if (document.all.hidFlag.value == "-1") {
			    document.all.txtUserName.focus();
			}
			else if (document.all.hidFlag.value == "-2") {
			    document.all.txtPassword.focus();
			}
    </script>

    </form>
</body>
</html>
