<%@ Page Language="c#" CodeBehind="Menu.aspx.cs" AutoEventWireup="True" Inherits="Epower.ITSM.Web.NewOldMainPage.Menu" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link type="text/css" rel="stylesheet" href="../App_Themes/NewOldMainPage/css.css" />

    <script type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>

    <script language="javascript" type="text/javascript">
        var selectObj;

        function chang(obj, img) {
            if (obj != selectObj) {
                obj.background = "../Images/" + img;
            }
        }

        function chang_Class(name, num, my) {

            for (i = 0; i < num; i++) {
                if (i != my) {
                    document.getElementById(name + i).className = "STYLE2";
                    document.getElementById(name + i).background = "../Images/lanmu-c.gif";
                }
            }
            document.getElementById(name + my).className = "STYLE1";
            document.getElementById(name + my).background = "../Images/lanmu-a.gif";

        }
        function changImg(obj, changeImage) {

            if (changeImage != '' && changeImage != null) {
                obj.src = changeImage;
                return;
            }

            if (document.getElementById("left_").style.display != "none") {
                document.getElementById("left_").style.display = "none";
            } else {
                document.getElementById("left_").style.display = "";
            }
        }
        function changImg(obj) {
            if (obj.src.indexOf("Images/shensuo-b.gif") > -1) {
                obj.src = "../Images/shensuo-a.gif";
                document.getElementById("left_").style.display = "none";
                top.document.all.frameMain.cols = "8,*";
                //document.getElementById("body1").scroll="no";
            } else {
                obj.src = "../Images/shensuo-b.gif";
                document.getElementById("left_").style.display = "";
                top.document.all.frameMain.cols = "187,*";
                //document.getElementById("body1").scroll="no";
            }
        }
        //下拉菜单
        function mOvr(src, clrOver) {

            if (!src.contains(event.fromElement)) {
                src.style.cursor = 'hand';
                src.bgColor = clrOver;

            }
        }
        function mOut(src, clrIn) {

            if (!src.contains(event.toElement)) {
                src.style.cursor = 'default';
                src.bgColor = clrIn;
            }
        }

        function onSelect(id) {

            for (var i = 0; i < 30; i++) {
                var stemp = document.getElementById("menuID_" + i);
                if (i != id) {
                    if (stemp != null && document.getElementById("menuID_" + i) != "undefined")
                        stemp.style.display = "none";
                    if (document.getElementById("imgID_" + i) != null && document.getElementById("imgID_" + i) != "undefined") {
                        document.getElementById("imgID_" + i).src = "../Images/left_yj.gif";

                        //  document.getElementById("imgID_"+i).background-color="yellow";
                    }

                }
                else {
                    if (stemp != null && document.getElementById("menuID_" + i) != "undefined") {
                        if (stemp.style.display == "none") {
                            stemp.style.display = "";
                            document.getElementById("imgID_" + i).src = "../Images/left_yj1.gif";
                        }
                        else {
                            stemp.style.display = "none";
                            document.getElementById("imgID_" + i).src = "../Images/left_yj.gif";
                        }

                    }

                }
            }
        }

        /* 上次点击的链接 */
        var last_click_node;
        //返回主页
        function GoUrl(sUrl) {
            var source_elem = event.target || event.srcElement;
            var len = $(source_elem).prev().size();

            if (len == 0) {
                try {
                    last_click_node.style.color = "#004175";
                } catch (e) {
                };

                last_click_node = source_elem;
                last_click_node.style.color = "red";
            }

            parent.MainFrame.location = sUrl;

            // parent.MainFrame.document.bgColor="#FCAFBF";                 

        }


        var m_CurrentIndex = 1;

        function setNavMenuBG(index) {
            var obj;
            m_CurrentIndex = index;
            for (i = 1; i <= 10; i++) {
                obj = eval("document.all.navMenu" + i);
                if (typeof (obj) == "object") {
                    if (i == index && obj.className != "navColumnLight") {
                        obj.className = "navColumnLight";
                    }
                    else if (i != index && obj.className != "navColumnDark") {
                        obj.className = "navColumnDark";
                    }
                }
            }
        }

        function setNavMenuBGOut(index) {

            var obj;

            obj = eval("document.all.navMenu" + index);
            if (typeof (obj) == "object") {

                if (index != m_CurrentIndex && obj.className == "navColumnMouseOver") {
                    obj.className = "navColumnDark";
                }


            }

        }

        function setNavMenuBGMove(index) {

            var obj;
            if (index != m_CurrentIndex) {
                for (i = 1; i <= 10; i++) {
                    obj = eval("document.all.navMenu" + i);
                    if (typeof (obj) == "object") {

                        if (i == index && obj.className != "navColumnMouseOver") {
                            obj.className = "navColumnMouseOver";
                        }
                        else if (i != index && i != m_CurrentIndex && obj.className != "navColumnDark") {
                            //alert(m_CurrentIndex);
                            obj.className = "navColumnDark";
                        }
                    }

                }
            }
        } 

    </script>

</head>
<body id="body1">

    <script type="text/javascript">

        $(document).ready(function() {
            //邮件直接审批,upt by wxh
            var backUrl = '<%=BackUrl %>';
            if (backUrl != "") {
                var obj = document.getElementById("imgChange");
                changImg(obj);
                window.open(backUrl, 'MainFrame', 'scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');
            }
        });

    </script>

    <table width="180" height="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" valign="top" id="left_">
                <table width="175" height="100%" border="0" cellpadding="0" cellspacing="0" class="bian1">
                    <tr>
                        <td height="7" align="left">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" height="100%" valign="top" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td height="100%" align="center" valign="top">
                                        <div id="elSixParent" class="parent" style="margin-left: 0.1px; min-height: 500;"
                                            runat="server">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="7" align="left">
                        </td>
                    </tr>
                </table>
            </td>
            <td width="8" height="100%" valign="middle" class="bian_ss2">
                <img id="imgChange" src="../Images/shensuo-b.gif" width="8" height="126" onclick="changImg(this)">
            </td>
        </tr>
    </table>

    <script language="javascript">
        selectObj = document.getElementById("home");
    </script>

    <!--Begin: 引入基础脚本库-->

    <script type="text/javascript" language="javascript" src="../js/epower.base.js"></script>

    <!--End: 引入基础脚本库-->
    <asp:Literal ID="literalPopupWindow" runat="server" Visible="false">
        <script language='javascript' type="text/javascript">
            $(document).ready(function() {
                var _xy = epower.tools.computeXY('c', window, 800, 400);
                epower.tools.open('../Forms/AlertMsg.aspx', null,
                    { width: '800px', height: '400px', left: _xy.x, top: _xy.y });
            });
        </script>
    </asp:Literal>
</body>
</html>
