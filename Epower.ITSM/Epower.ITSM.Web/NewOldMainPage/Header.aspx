<%@ Page Language="c#" CodeBehind="Header.aspx.cs" AutoEventWireup="True" Inherits=" Epower.ITSM.Web.NewOldMainPage.Header" %>

<html>
<head runat="server">
    <title></title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link type="text/css" rel="stylesheet" href="../App_Themes/NewOldMainPage/css.css" />

    <script type="text/javascript" src="../Js/jquery-1.7.2.min.js"></script>

    <script type="text/javascript" src="../Js/jquery.cookie.js"></script>

    <script type="text/javascript" src="../Js/App_Common.js"></script>

</head>

<script type="text/javascript">

    $(document).ready(function() {
        if ($.browser.msie && $.browser.version == "6.0") {
            return;
        }
        try {
            if (popupAlertMsg == "1") {

                CheckTodo();
            }
        } catch (e) {
        }
    });
    var titleTime;

    function CheckTodo() {
        var userId = 'userId=<%=Session["UserID"].ToString() %>';
        var msgCount = GetNeedToDoMessageCount(userId);
        var step = 0;
        if (msgCount == 0) {
            flash_title(msgCount, 0, true);
        }
        else {
            flash_title(msgCount, 0, false);
        }
    }

    function flash_title(msgCount, step, isNoEvent) {
        step++;
        if (isNoEvent == true) {
            window.top.document.title = "<%=InitSystemName %>";
            if (step % 100 == 0) {
                clearTimeout(titleTime)
                CheckTodo()
                return;
            }
        }
        else {
            if (step % 2 == 1) {
                window.top.document.title = '◆◇您有' + msgCount + '条新事项◇◆';
            }
            if (step % 2 == 0) {
                window.top.document.title = '◇◆您有' + msgCount + '条新事项◆◇';
            }
            if (step % 100 == 0) {
                clearTimeout(titleTime)
                CheckTodo()
                return;
            }
        }
        titleTime = setTimeout("flash_title(" + msgCount + "," + step + "," + isNoEvent + ")", 50000);
    }


    function GetNeedToDoMessageCount(userId) {
        var msgCount = 0;

        $.ajax({
            type: "post",
            async: false,
            data: userId,
            url: "../AshxHandler/MessageHandler.ashx",
            error: function() {
                msgCount = 0;
            },
            success: function(data, textStatus) {
                msgCount = data;
            }
        });
        $("#hidMsgCount").val(msgCount);
        //        $.cookie('NeedToDoEventChangeFlag', "0");
        return msgCount;
    }
    //返回工程师工作量
    function cstWork() {
        parent.MainFrame.location = '../AnalsysForms/frmzhCstWork.aspx';
    }
</script>

<script type="text/javascript">
    //返回主页
    function home() {
        parent.MainFrame.location = '<%=StartPage%>';

    }
    //返回主页
    function GoUrl(sUrl) {
        parent.MainFrame.location = sUrl;

    }
    function GoUrl2(sUrl) {
        parent.MainFrame.location = sUrl;
        // window.location = sUrl;
    }

    var xmlhttp = null;
    function CreateXmlHttpObject() {
        try {
            xmlhttp = new ActiveXObject("MSXML2.XMLHTTP");
        }
        catch (e) {
            try {
                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e2) { }
        }
        return xmlhttp;
    }

    function logout() {
        if (confirm('你确定要退出系统吗？')) {
            parent.document.location = '<%=sExitUrl%>';

        }
    }

    function txtKeyNameClear() {
        if (document.getElementById('<%=txtKeyName.ClientID%>').value == '知识搜索') {
            document.getElementById('<%=txtKeyName.ClientID%>').value = "";
        }
    }
    function txtKeyNameBack() {
        if (document.getElementById('<%=txtKeyName.ClientID%>').value == '') {
            document.getElementById('<%=txtKeyName.ClientID%>').value = "知识搜索";
        }
    }
    function keyDown() {
        var currKey = event.keyCode;
        var hfSearch = document.getElementById("hfSearch"); //搜索
        if (currKey == 13) {
            hfSearch.focus();
            hfSearch.onclick(); //执行搜索
        }
    }
    function SearchInfo() {
        var KeyWord = document.getElementById('<%=txtKeyName.ClientID%>').value;
        parent.MainFrame.location = "../InformationManager/frmInfSearch.aspx?KeyWord=" + escape(KeyWord);
    }
    //返回自助模式
    function self() {
        parent.document.location = '../NewMainPage/Index.aspx';
    }
</script>

<body scroll="no">
    <form id="Form1" name="PostForm" action="" method="post" runat="server">
    <input type="hidden" id="hidUserId" runat="server" />
    <input type="hidden" id="hidTitle" runat="server" />
    <input type="hidden" id="hidMsgCount" runat="server" />
    <iframe id='Iframe3' name="Iframe3" src="" width='100%' height='0' scrolling='auto'
        frameborder='no'></iframe>
    <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="3" valign="top">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="398" align="left" background="../Images/top.gif" height="50">
                            <table width="398" height="50" border="0" cellpadding="0" cellspacing="0" background="../Images/top_a.gif">
                                <tr>
                                    <td width="200">
                                        <img src="../Images/logo-a.gif" width="200" height="40">
                                    </td>
                                    <td width="198">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td height="22" align="right" colspan="2">
                                                    <span class="STYLE4">&nbsp;</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblWarning" runat="server" ForeColor="#FF8080" Text="软件许可期限已到,请联系供应商"
                                                        Visible="false"></asp:Label>
                                                    <asp:HyperLink ID="lklLicense" runat="server" Target="MainFrame" NavigateUrl="../Common/frmSetLicense.aspx"
                                                        Visible="false">许可注册</asp:HyperLink>
                                                </td>
                                                <td align="left" style="word-break: break-all">
                                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td height="50" align="right" background="../Images/top.gif">
                            <table width="98%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <table width="76%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="353" height="25">
                                                    <span style="font-size: 16px; color: #478ABC"><span style="height: 25px">
                                                        <marquee id="autoscroll" onmouseover="this.stop()" style="width: 100%; height: 16px"
                                                            onmouseout="this.start()" scrollamount="1" scrolldelay="30" direction="left"
                                                            height="16">
                                                            <div style="font-size: 16px; color: #478ABC">
                                                                <asp:DataList ID="RptNewsShow" runat="server" Height="16px" RepeatDirection="Horizontal">
                                                                    <ItemTemplate>
                                                                        <div nowrap>
                                                                            <asp:HyperLink ID="Hyperlink2" ForeColor="#B2DFFF" Style="display: inline-block;"
                                                                                Font-Size="14px" Text='<%#GetShowTitle((string)DataBinder.Eval(Container.DataItem, "Title"),(string)DataBinder.Eval(Container.DataItem, "photo"))%>'
                                                                                NavigateUrl='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "NewsId"))%>'
                                                                                runat="server" Target="_blank">
                                                                            </asp:HyperLink>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:DataList>
                                                            </div>
                                                        </marquee>
                                                    </span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right">
                                        &nbsp;&nbsp;<span class="STYLE4"><span id="nowDateDiv"></span><span id="nowTimeDiv"></span></span>
                                    </td>
                                    <td width="15">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="center">
                                        <input type="text" id="txtKeyName" runat="server" class="bian" value="知识搜索" onclick="txtKeyNameClear();"
                                            onblur="txtKeyNameBack();" onkeydown="keyDown();" />&nbsp;&nbsp;<a href="#" id="hfSearch"
                                                onclick="SearchInfo();"><img src="../Images/ssan.gif" width="40" height="20" border="0"
                                                    align="absbottom"></a>
                                    </td>
                                    <td align="right">
                                        <span  runat="server" id="spanTool">
                                            <%--zxl--只是做了点点隐藏--%>
                                            <a href="#" style="display: none;" onclick="cstWork();">工程师工作量情况</a> <a href="#"
                                                class="td_2" onclick="home();">
                                                <img src="../Images/fh.gif" width="16" height="16" align="absmiddle" border="0">
                                                主页</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#" class="td_2" onclick="window.open('../Forms/FrmSMSSend.aspx','','resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=400,height=300px,left=150,top=100')"><img
                                                    src="../Images/suser.gif" width="16" height="16" align="absmiddle" border="0">
                                                    消息</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#" class="td_2" onclick="self();"><img src="../Images/ROOT.gif"
                                                        width="16" height="16" align="absmiddle" border="0">
                                                        自助模式</a>&nbsp;&nbsp;&nbsp;
                                        </span>
                                        <a href="#" class="td_2" onclick="logout();">
                                            <img src="../Images/tc.gif" width="16" height="16" align="absmiddle" border="0">
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
    <input type="hidden" id="userdefined" runat="server" value="false" />

    <script language="JavaScript" type="text/javascript">
        if ('<%=sCallTel%>' == "0")  //呼叫中心启用
        {
            if ('<%=sBlockRoom%>' != "") {
                Iframe3.location = "../AppForms/frmCallCenter.aspx?BlockRoom=" + '<%=sBlockRoom%>';
            }
        }   
    </script>

    <script language="JavaScript" type="text/javascript">

        parent.MainFrame.location = '<%=StartPage%>';
        setInterval("timeview()", 1000);

        var nowDay = new Date();
        function dateview() {
            var WEEK = new Array(7);
            WEEK[0] = "日"; WEEK[1] = "一"; WEEK[2] = "二"; WEEK[3] = "三"; WEEK[4] = "四"; WEEK[5] = "五"; WEEK[6] = "六";

            var temp = "";

            temp += " ";
            temp += "";

            temp += "" + nowDay.getFullYear() + "年" + (nowDay.getMonth() + 1) + "月" + nowDay.getDate() + "日";
            temp += " 星期" + WEEK[nowDay.getDay()] + "";
            temp += " ";

            nowDateDiv.innerHTML = temp;
        }

        dateview();

        function timeview() {
            var timestr = nowDay.toLocaleString();
            timestr = timestr.substring(timestr.lastIndexOf(" "));
            nowTimeDiv.innerHTML = timestr;
            nowDay.setSeconds(nowDay.getSeconds() + 1);
        }

        timeview();
        if (document.getElementById("userdefined").value == "true")
            _upc();
    </script>

    </form>
</body>
</html>
