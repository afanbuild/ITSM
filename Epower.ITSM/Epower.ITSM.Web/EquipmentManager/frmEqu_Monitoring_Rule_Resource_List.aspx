<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmEqu_Monitoring_Rule_Resource_List.aspx.cs"
    Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_Monitoring_Rule_Resource_List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../App_Themes/NewOldMainPage/css.css" type="text/css" rel="stylesheet">
    <link href="../App_Themes/NewOldMainPage/jquery.contextmenu.css" type="text/css"
        rel="stylesheet">
    <link href="../App_Themes/NewOldMainPage/ui.all.css" type="text/css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Repeater ID="rptResourceList" runat="server">
            <HeaderTemplate>
                <table class="Gridtable" cellspacing="1" cellpadding="1" rules="cols" border="0"
                    id="ctl00_ContentPlaceHolder1_dgEqu_Desk" style="width: 100%;">
                    <tr class="listTitleNew_1" align="center">
                        <td>
                            资产名称
                        </td>
                        <td>
                            资产编号
                        </td>
                        <td>
                            客户名称
                        </td>
                        <td>
                            资产类别
                        </td>
                        <td style="width: 60px;">
                            操作
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="listTitleNoAlign" align="center" onmouseover="currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'"
                    onmouseout="this.style.backgroundColor=currentcolor" style="background-color: white;
                    height: 25px;">
                    <td align="left">
                        <a onclick="javascript:openobj.open('frmEqu_DeskEdit.aspx?IsTanChu=true&amp;IsSelect=1&amp;id=10138&amp;subjectid=-1&amp;FlowID=0','','scrollbars=yes,resizable=yes');event.returnValue = false;"
                            id="ctl00_ContentPlaceHolder1_dgEqu_Desk_ctl02_lnkEquName" >
                            <%#Eval("NAME")%></a>
                    </td>
                    <td align="left">
                        <%#Eval("CODE")%>
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                    <td align="left">
                        <%#Eval("CATALOGNAME")%>
                    </td>
                    <td align="center">
                        <a href="frmEqu_Monitoring_Rule_Resource_Details.aspx?resource_id=<%#Eval("ID") %>">编辑</a>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="listTitleNoAlign" align="center" onmouseover="currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'"
                    onmouseout="this.style.backgroundColor=currentcolor" style="background-color: rgb(247, 250, 253);
                    height: 25px;">
                    <td align="left">
                        <a onclick="javascript:openobj.open('frmEqu_DeskEdit.aspx?IsTanChu=true&amp;IsSelect=1&amp;id=10137&amp;subjectid=-1&amp;FlowID=0','','scrollbars=yes,resizable=yes');event.returnValue = false;"
                            id="ctl00_ContentPlaceHolder1_dgEqu_Desk_ctl03_lnkEquName">
                            <%#Eval("NAME") %></a>
                    </td>
                    <td align="left">
                        <%#Eval("CODE") %>
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                    <td align="left">
                        <%#Eval("CATALOGNAME")%>
                    </td>
                    <td align="center">
                        <a href="frmEqu_Monitoring_Rule_Resource_Details.aspx?resource_id=<%#Eval("ID") %>">编辑</a>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
