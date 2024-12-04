<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmFeedBack.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmFeedBack" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>客户邮件回访</title>
    <link type="text/css" rel="stylesheet" href="../App_Themes/StandardThemes/css.css"/>
    <script type="text/javascript" language="javascript">
        function logout() {
            if (confirm('你确定要退出吗？')) {
                //清除Exchange登录用户凭证缓存  IE6 SP1以上有效
                document.execCommand("ClearAuthenticationCache");
                window.close();

            }
        }
    </script>
</head>
    <script language="javascript" type="text/javascript" src="<%=sApplicationUrl %>/Js/App_Common.js"> </script>
<body>
    <form id="form1" runat="server">
    <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="3" valign="top">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="398" align="left" background="../NewOldMainPage/images/top.gif" height="50">
                            <table width="398" height="50" border="0" cellpadding="0" cellspacing="0" background="../NewOldMainPage/images/top_a.gif">
                                <tr>
                                    <td width="200">
                                        <img src="../NewOldMainPage/images/logo-a.gif" width="200" height="40">
                                    </td>
                                    <td width="198" valign="bottom">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td height="22" align="right" colspan="2">
                                                    <span class="STYLE4">&nbsp;</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblWarning"  runat="server" Text="客户邮件回访"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td height="50" align="right" background="../NewOldMainPage/images/top.gif">
                            <table width="98%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td valign="top" align="center">
                                                                                &nbsp;&nbsp;
                                    </td>
                                    <td align="right">
                                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="#" class="td_2" onclick="logout();"><img src="../NewOldMainPage/images/tc.gif"
                                                    width="16" height="16" align="absmiddle" border="0">
                                                    退出</a>
                                    </td>
                                    <td width="15">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
    <table width="100%" class="listContent" >
        <tr>
           <td class="list">
               <asp:Literal ID="ltlHFList" runat="server"></asp:Literal></td>
        </tr>
    </table>
    <TABLE id="tblFeed" width="100%" class="listContent" runat="server">
		    <TR>
			    <TD style="WIDTH: 70px" align="center"  class="listTitle">满意程度</TD>
			    <TD  class="list" colspan="3"><asp:radiobuttonlist id="rblFeedBack" runat="server" Width="280px" RepeatDirection="Horizontal">
						    <asp:ListItem Value="1" Selected="True">满意</asp:ListItem>
						    <asp:ListItem Value="2">基本满意</asp:ListItem>
						    <asp:ListItem Value="3">不满意</asp:ListItem>
					    </asp:radiobuttonlist></TD>
		    </TR>
            <tr>
                <td align="center" class="listTitle" style="width: 70px">
                    回访人</td>
                <td class="list" >
                    <asp:TextBox ID="txtFeedPerson" runat="server" Enabled="false" ></asp:TextBox></td>
                <td align="center" class="listTitle" style="width: 70px">
                    回访类别</td>
                <td class="list" >
                    <asp:RadioButtonList ID="rboFeedType" runat="server" RepeatDirection="Horizontal" Enabled="false" >
                        <asp:ListItem Selected="True" Value="4">邮件</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td class="listTitle" align="center"  style="width: 70px">
                    被回访人</td>
                <td class="list">
                    <asp:TextBox ID="txtCustName" runat="server"></asp:TextBox></td>
                <td align="center" class="listTitle" style="width: 70px">
                    回访时间</td>
                <td class="list">
                    <uc1:ctrdateandtime ID="CtrDTFBTime" runat="server" />
                </td>
            </tr>
		    <TR>
			    <TD style="WIDTH: 70px" align="center"  class="listTitle">备&nbsp;&nbsp;&nbsp; 注</TD>
			    <TD  class="list" colspan="3">
			    <asp:textbox id="txtFeedBack" runat="server" Height="104px" Width="90%" MaxLength="1500" TextMode="MultiLine"></asp:textbox>
			    </TD>
		    </TR>
		    <tr>
		        <td class="listTitle" colspan="4" align="center">
		            <asp:button id="cmdFeedBack" runat="server" Text="提交" onclick="cmdFeedBack_Click" CssClass="btnClass"></asp:button>
		        </td>
		    </tr>
    </TABLE>
    </form>
</body>
</html>
