<%@ Page Language="c#" CodeBehind="Header.aspx.cs" AutoEventWireup="True" Inherits=" Epower.ITSM.Web.NewMainPage.Header" %>

<html>
<head id="Head1" runat="server">
    <title></title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link type="text/css" rel="stylesheet" href="../App_Themes/NewOldMainPage/css.css" />
</head>

<script language="javascript">
	    //返回主页
        function home()
        {
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
        function CreateXmlHttpObject()
        {
			try  
			{  
				xmlhttp = new ActiveXObject("MSXML2.XMLHTTP");  
			}  
			catch(e)  
			{  
				try  
				{  
					xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");  
				}  
				catch(e2){}  
			}
			return xmlhttp;
        }
        
        function RecoverDesk()
        {
            if(!confirm("确认恢复默认桌面项吗？"))
            {
                return;
            }
            if(xmlhttp == null)
                xmlhttp = CreateXmlHttpObject();   
            if(xmlhttp != null)
            {
                try
                {	
					var newDateObj = new Date()
	                var sparamvalue =  newDateObj.getFullYear().toString() + newDateObj.getHours().toString() + newDateObj.getMinutes().toString() + newDateObj.getSeconds().toString();
					xmlhttp.open("GET", "../MyDestop/frmXmlHttp.aspx?Desk=1&param=" + sparamvalue, true); 
                    xmlhttp.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
					xmlhttp.onreadystatechange = function() 
												{ 
													if ( xmlhttp.readyState==4 ) 
													{
													    if(xmlhttp.responseText=="0")  //没有
														{
														    alert("成功恢复默认桌面项，如需立即生效请刷新主页！"); 
														}
													}
				                                }									 
				xmlhttp.send(null); 
				}catch(e3){}
            }
        }
        //返回工程师工作量
    function cstWork()
    {
        parent.MainFrame.location='../AnalsysForms/frmzhCstWork.aspx';
    }
        
        function logout() {
            if (confirm('你确定要退出系统吗？')) {
                //清除Exchange登录用户凭证缓存  IE6 SP1以上有效
                document.execCommand("ClearAuthenticationCache");
                parent.document.location = '<%=sExitUrl%>';

            }
        }
        function txtKeyNameClear()
        {
            if(document.getElementById('<%=txtKeyName.ClientID%>').value=='知识搜索')
            {
                document.getElementById('<%=txtKeyName.ClientID%>').value="";
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
        function SearchInfo()
        {
            var KeyWord = document.getElementById('<%=txtKeyName.ClientID%>').value;
            parent.MainFrame.location = "../InformationManager/frmInfSearch.aspx?KeyWord="+escape(KeyWord);
        }
        function txtKeyNameBack() {
            if (document.getElementById('<%=txtKeyName.ClientID%>').value == '') {
                document.getElementById('<%=txtKeyName.ClientID%>').value = "知识搜索";
            }
        }
        //返回管理模式
        function self() {
            parent.document.location = '../NewOldMainPage/Index.aspx';        
        }
</script>

<body scroll="no">
    <form id="Form1" name="PostForm" action="" method="post" runat="server">
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
                                        <table width="90%" border="0" cellspacing="0" cellpadding="0">
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
                                                <td align="left">
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
                                                    <div style="FONT-SIZE: 16px; COLOR: #478ABC" >
                                                    <asp:datalist id="RptNewsShow" runat="server" Height="16px" RepeatDirection="Horizontal">
								                        <ItemTemplate>
									                        <div nowrap>
										                        <asp:HyperLink id="Hyperlink2" ForeColor="#B2DFFF" style="display:inline-block;" Font-Size="14px" text='<%#GetShowTitle((string)DataBinder.Eval(Container.DataItem, "Title"),(string)DataBinder.Eval(Container.DataItem, "photo"))%>' NavigateUrl='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "NewsId"))%>' runat="server" Target="_blank">
										                        </asp:HyperLink>
									                        </div>
								                        </ItemTemplate>
							                        </asp:datalist>                                                                                                        
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
                                    <td valign="top"  align="center">
                                        <input type="text" id="txtKeyName" runat="server" style="width: 195px" value="知识搜索"
                                            onclick="txtKeyNameClear();" onblur="txtKeyNameBack();" onkeydown="keyDown();" />&nbsp;&nbsp;<a href="#" id="hfSearch" onclick="SearchInfo();"><img
                                                src="../Images/ssan.gif" width="40" height="20" border="0" align="absbottom"></a>
                                    </td>
                                    <td align="right">
                                         <a href="#" style="display:none;" onclick="cstWork();">
                                        工程师工作量情况</a>
                                        <a href="#" class="td_2" onclick="home();">
                                            <img src="../Images/fh.gif" width="16" height="16" align="absmiddle" border="0">
                                            主页</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#" class="td_2" onclick="window.open('../Forms/FrmSMSSend.aspx','','resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=400,height=300px,left=150,top=100')"><img
                                                src="../Images/suser.gif" width="16" height="16" align="absmiddle" border="0">
                                                消息</a>&nbsp;&nbsp;<a href="#" class="td_2" onclick="self();"><img src="../Images/ROOT.gif"
                                                    width="16" height="16" align="absmiddle" border="0">
                                                    管理模式</a>&nbsp;&nbsp;<a href="#" class="td_2" onclick="logout();"><img src="../Images/tc.gif"
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
    <input type="hidden" id="userdefined" runat="server" value="false" />

    <script language="JavaScript" type="text/javascript">
               top.MainFrame.location = '<%=StartPage%>';
               if ('<%=sCallTel%>' == "0")  //呼叫中心启用
               {
                   if ('<%=sBlockRoom%>' != "") {
                         //Iframe3.location = "../AppForms/frmCallCenter.aspx?BlockRoom=" + '<%=sBlockRoom%>';  
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
