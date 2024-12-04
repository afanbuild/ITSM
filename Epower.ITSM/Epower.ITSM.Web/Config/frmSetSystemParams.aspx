<%@ Page language="c#" Codebehind="frmSetSystemParams.aspx.cs" AutoEventWireup="True" Inherits="Epower.ITSM.Web.Config.frmSetSystemParams" %>
<HTML xmlns="http://www.w3.org/1999/xhtml">
	<HEAD id="HEAD1" runat="server">
<style type="text/css">
<!--
body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}
.STYLE8 {
	color: #336666;
	font-size: 16px;
	font-style: italic;
}
.STYLE14 {
	color: #3192BE;
	font-size: 20px;
	font-family: "黑体";
}
-->
</style>
	</HEAD>
	<body>
	<form id="Form1" runat="server">	
<table width="99%" border="0" cellpadding="0" cellspacing="0" class="bian1" align="center">
          <tr>
            <td align="center" valign="top"><table width="99%" height="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                  <td align="left" valign="bottom"><table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="49%"><span class="STYLE5 STYLE8"><span class="STYLE14">系统参数配置</span></span></td>
                        <td width="51%" height="23" align="right"><a href="#" target="_parent" class="td_4"></a></td>
                      </tr>
                      <tr>
                        <td style="height: 5px"><table width="100%" border="0" cellspacing="0" cellpadding="0" class="bian3">
                            <tr>
                              <td></td>
                            </tr>
                        </table>
                        </td>
                        <td style="height: 5px"><table width="100%" border="0" cellspacing="0" cellpadding="0" class="bian4">
                            <tr>
                              <td></td>
                            </tr>
                        </table></td>
                      </tr>
                  </table></td>
                </tr>
                <tr>
                  <td align="left" valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="30%" height="3"></td>
                      </tr>
                      <tr>
                        <td colspan="3" align="center" valign="top"><table width="100%" height="25" border="0" cellpadding="0" cellspacing="0" bgcolor="#EEF5FB" class="bian">
                            <tr>
                              <td width="50%" align="left"><img src="images/jiantou.gif" width="9" height="7"><span class="STYLE5"><asp:Literal ID="ltlTitle" Text="系统名称" runat="server"></asp:Literal></span></td>
                              <td width="50%" align="right" class="td_4"><asp:Button ID="Button1" runat="server" Text="保  存" OnClick="Button1_Click"/></td>
                            </tr>
                        </table></td>
                      </tr>
                      <tr>
                        <td height="5">  
                            </td>
                      </tr>
                      <tr>
                        <td colspan="3" align="center" valign="top" id="tdDyTable" runat=server>
                        </td>
                      </tr>
                      <tr>
                        <td colspan="3" align="center" valign="top">&nbsp;</td>
                      </tr>
                      <tr>
                        <td colspan="3" align="center" valign="top"><table width="100%" height="100%" class="bian">
                            <tr>
                              <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
                                  <tr>
                                    <td width="35%" height="23"><img src="images/bangzhu.gif" width="14" height="15"><span class="STYLE5"> 帮助卡</span></td>
                                    <td width="15%" align="right"></td>
                                  </tr>
                                  <tr>
                                    <td height="5" colspan="2"><table width="100%" border="0" cellspacing="0" cellpadding="0" class="bian3">
                                        <tr>
                                          <td></td>
                                        </tr>
                                    </table></td>
                                  </tr>
                              </table></td>
                            </tr>
                            <tr>
                              <td align="left" valign="middle" bgcolor="#EEF5FB">
                              <asp:Literal ID="ltlHelp" Text="<br> &nbsp;&nbsp;&nbsp;&nbsp;有多种方式/方法可以将服务请求提交您的.有多种方式/方法可以将服务请求提交您的.有多种方式/方法可以将服务请求提交您的.有多种</br><br>&nbsp;&nbsp;&nbsp;&nbsp;fsgfsgfsgfgsgfsgf </br> <br>&nbsp;&nbsp;&nbsp;&nbsp;fgshgdjfdhjdfhkdfjhjuykukuy </br></td>" runat="server">
                              </asp:Literal>
                            </tr>
                        </table></td>
                      </tr>
                  </table></td>
                </tr>
            </table></td>
          </tr>
        </table>
    </form>
    </body>
</HTML>