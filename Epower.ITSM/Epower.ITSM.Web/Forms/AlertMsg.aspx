<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlertMsg.aspx.cs" Inherits="Epower.ITSM.Web.Forms.AlterMsg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>E8ITSM服务管理系统</title>
    <style type="text/css">
        #mainDiv table td
        {
            border: 1px solid #CEE3F2;
            padding: 2px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="mainDiv" runat="server">
        <asp:Repeater ID="rptAlertMsg" runat="server">
            <HeaderTemplate>
                <table width="90%" align="center" border="0" cellpadding="0" cellspacing="0" class="listContent">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td align="center" class="list" colspan="2">
                        <div style="font-weight: bold;">
                            <%#Eval("Title")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="listTitle" colspan="2">
                        <div style="padding: 5px;">
                            提供者:&nbsp;&nbsp;
                            <%#Eval("Writer")%>&nbsp;&nbsp;（发布时间:&nbsp;&nbsp;
                            <%#Eval("InputDate")%>）
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="listTitle" style="width: 12%;" align="center">
                        内容
                    </td>
                    <td class="list">
                        <div style="padding: 10px; line-height:18px; letter-spacing:1px;">
                            <%#Eval("content")%>
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
