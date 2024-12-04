<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmWaitProcessThing.aspx.cs" Inherits="Epower.ITSM.Web.Forms.frmWaitProcessThing" Title="待办事项-按流程分类"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
                if (window.parent.document.all.hidTabIndex.value != "false") {
                    document.all.hidIndex.value = window.parent.document.all.hidTabIndex.value;
                    i = document.all.hidIndex.value;
                }
                else {
                    document.all.hidIndex.value = '0';
                    i = document.all.hidIndex.value;
                }
            }
            else {
                setOldIndex(i);

            }


            $("#tabs").tabs({ selected: i,
                select: function(e, ui) {
                    var t = $(e.target);
                    window.parent.document.all.hidTabIndex.value = ui.index;
                    document.all.hidIndex.value = ui.index;
                    var hfMsgQue = document.all.hfMsgQue; //设置连接路径
                    var url = "oa_messagequery.aspx";
                    switch (ui.index) {
                        case 0: url = url + "?type=1026"; break;                        
                        case 1: url = url + "?type=420"; break;
                        case 2: url = url + "?type=210"; break;
                        case 3: url = url + "?type=1028"; break;
                        case 4: url = url + "?type=199"; break;
                    }
                    hfMsgQue.href = url;
                }
            });
        });

        function setOldIndex(val) {
            window.parent.document.all.hidTabIndex.value = val;
        }

        function getOldIndex() {
            var value = document.all.hidIndex.value;
            if (value == "0") {
                window.open('../Forms/oa_messagequery.aspx', 'MainFrame', 'scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');
            }
            else if (value == "1") {
                window.open('../Forms/oa_messagequery.aspx', 'MainFrame', 'scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');
            }
            else if (value == "2") {
                window.open('../Forms/oa_messagequery.aspx', 'MainFrame', 'scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');

            }
            else if (value == "3") {
                window.open('../Forms/oa_messagequery.aspx', 'MainFrame', 'scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');
            }
            else if (value == "4") {
                window.open('../Forms/oa_messagequery.aspx', 'MainFrame', 'scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');
            }
        }

        function xx(obj) {

            obj.location.href = ''
        }
    </script>
    <div id="tabs" style="width: 100%;">
        <ul>
            <table width="100%">
                <tr>
                    <td width="100%">
                        <li id="liIssues" runat="server"><a href="../Forms/frmWaittingContent_Process.aspx?TypeContent=Issue&Params=<%=sparams%>">
                            <asp:Label ID="lblIssues" runat="server" Text="事件"></asp:Label>[<font color='red'><%=Issue%></font>]</a>
                        </li>
                        <li id="liChange" runat="server"><a href="../Forms/frmWaittingContent_Process.aspx?TypeContent=Change&Params=<%=sparams%>">
                            <asp:Label ID="lblChange" runat="server" Text="变更"></asp:Label>[<font color='red'><%=Change%></font>]</a></li>
                        <li id="liByts" runat="server"><a href="../Forms/frmWaittingContent_Process.aspx?TypeContent=Byts&Params=<%=sparams%>">
                            <asp:Label ID="lblByts" runat="server" Text="问题"></asp:Label>[<font color='red'><%=Byts%></font>]</a></li>                        
                        <li id="liOther" runat="server"><a href="../Forms/frmWaittingContent_Process.aspx?TypeContent=Other&Params=<%=sparams%>">
                            <asp:Label ID="lblOther" runat="server" Text="其它"></asp:Label>[<font color='red'><%=Other%></font>]</a></li>
                    </td>
                    <td>
                        <a id="hfMsgQue" href="oa_messagequery.aspx?type=1026" target="_parent">
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
