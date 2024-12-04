<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frm_Hastodoitems.aspx.cs" Inherits="Epower.ITSM.Web.Forms.frm_Hastodoitems" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>待办事项</title>
    <script src="../Js/jquery-1.3.2.js" type="text/javascript"></script>
    <script src="../Js/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>
    <style>
        #tooltip
        {
            position: absolute;
            z-index: 3000;
            border: 1px solid #111;
            background-color: #eee;
            padding: 5px;
            opacity: 0.85;
        }
        #tooltip h3, #tooltip div
        {
            margin: 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">
        $.ajaxSetup({ cache: false });
        $(function() {
            var i = document.getElementById("<%=hidIndex.ClientID %>").value;
            if (i == "-1") {
                //否则取原来的       
                    document.all.hidIndex.value = '0';
                    i = document.all.hidIndex.value;
            }
           
            $("#tabs").tabs({ selected: i,
                select: function(e, ui) {
                    var t = $(e.target);
                    document.all.hidIndex.value = ui.index;
                }
            });
        });

    </script>
    <div id="tabs" style="width: 100%;">
        <ul>
            <table width="100%">
                <tr>
                    <td width="100%">
                        <li id="liIssues" runat="server"><a href="../Forms/frm_HastodoitemsContent_Process_2.aspx?TypeContent=Issue&Params=<%=sparams%>">
                            <asp:Label ID="lblIssues" runat="server" Text="事件"></asp:Label>[<font color='red'><%=Issue%></font>]</a>
                        </li>
                        <li id="liChange" runat="server"><a href="../Forms/frm_HastodoitemsContent_Process_2.aspx?TypeContent=Change&Params=<%=sparams%>">
                            <asp:Label ID="lblChange" runat="server" Text="变更"></asp:Label>[<font color='red'><%=Change%></font>]</a></li>
                        <li id="liByts" runat="server"><a href="../Forms/frm_HastodoitemsContent_Process_2.aspx?TypeContent=Byts&Params=<%=sparams%>">
                            <asp:Label ID="lblByts" runat="server" Text="问题"></asp:Label>[<font color='red'><%=Byts%></font>]</a></li>                        
                        <li id="liKb" runat="server"><a href="../Forms/frm_HastodoitemsContent_Process_2.aspx?TypeContent=Kb&Params=<%=sparams%>">
                            <asp:Label ID="Label1" runat="server" Text="知识"></asp:Label>[<font color='red'><%=Kb%></font>]</a></li>
                        <li id="liOther" runat="server"><a href="../Forms/frm_HastodoitemsContent_Process_2.aspx?TypeContent=Other&Params=<%=sparams%>">
                            <asp:Label ID="lblOther" runat="server" Text="其它"></asp:Label>[<font color='red'><%=Other%></font>]</a></li>
                    </td>
                    <td>
                        <a id="hfMsgQue" href="oa_messagequery_Hastodoitem.aspx?type=1026"  target="_self">
                            <img src='../Images/more-2.gif' border='0' align='absmiddle'></a>
                    </td>
                </tr>
            </table>
        </ul>
    </div>
    <input type="hidden" id="hidType" runat="server" value="-1" />
    <input type="hidden" id="hidIndex" runat="server" value="-1" />
    </form>
</body>
</html>