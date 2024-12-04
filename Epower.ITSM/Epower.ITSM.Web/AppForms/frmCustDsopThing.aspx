<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCustDsopThing.aspx.cs"
    Inherits="Epower.ITSM.Web.AppForms.frmCustDsopThing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

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
                window.open('../CustManager/frmBr_SellChanceMain.aspx?Operator=1910&ReturnValue=true', 'MainFrame', 'scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');
            }
        }
    </script>

    <table style="display: none">
        <tr>
            <td align="right">
                <asp:Label ID="lblToDay" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <div id="tabs" style="width: 100%;">
        <ul>
            <table width="100%">
                <tr>
                    <td width="100%">
                        <li><a href="../Forms/frmWaittingContent.aspx?TypeContent=UndoMsg&Params=<%=sparams%>">
                            <asp:Label ID="lblPlan" runat="server" Text="待办事项"></asp:Label>[<font color='red'><%=UndoMsg%></font>]</a></li>
                        <li><a href="../Forms/frmWaittingContent.aspx?TypeContent=ReceiveMsg&Params=<%=sparams%>">
                            <asp:Label ID="lblCareFor" runat="server" Text="待接收事项"></asp:Label>[<font color='red'><%=ReceiveMsg%></font>]</a></li>
                        <li><a href="../Forms/frmWaittingContent.aspx?TypeContent=ReadMsg&Params=<%=sparams%>">
                            <asp:Label ID="lblCalendar" runat="server" Text="阅知事项"></asp:Label>[<font color='red'><%=ReadMsg%></font>]</a></li>
                    </td>
                    <td>
                        <a id="hfMsgQue" href="../Forms/oa_messagequery.aspx?type=1026" target="_parent">
                            <img src='../Images/more-2.gif' border='0' align='absmiddle'></a>
                    </td>
                </tr>
            </table>
        </ul>
    </div>
    <input type="hidden" id="hidIndex" runat="server" value="-1" />
    </form>
</body>
</html>
