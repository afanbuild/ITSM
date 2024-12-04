<%@ Page Language="c#" CodeBehind="Menu.aspx.cs" AutoEventWireup="True" Inherits="Epower.ITSM.Web.NewMainPage.Menu" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link type="text/css" rel="stylesheet" href="../App_Themes/NewOldMainPage/css.css" />
    <script  type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>
    <script language="javascript" type="text/javascript">
	        var selectObj;

            function chang(obj,img){
	            if(obj!=selectObj){
		            obj.background="../Images/"+img;
	            }
            }

            function chang_Class(name,num,my){

	            for(i=0;i<num;i++){
		            if(i!=my){
			            document.getElementById(name+i).className="STYLE2";
			            document.getElementById(name+i).background="../Images/lanmu-c.gif";
		            }
	            }
	            document.getElementById(name+my).className="STYLE1";
	            document.getElementById(name+my).background="../Images/lanmu-a.gif";
            	
            }
            function changImg(obj,changeImage){
              
                  if(changeImage!=''&&changeImage!=null){
	                 obj.src = changeImage;
	                return ;
	              }
                 
	            if(document.getElementById("left_").style.display!="none"){
		            document.getElementById("left_").style.display="none";
	            }else{
		            document.getElementById("left_").style.display="";
	            }
            }
			function changImg(obj){
				if(obj.src.indexOf("Images/shensuo-b.gif")>-1){
					obj.src="../Images/shensuo-a.gif";
					document.getElementById("left_").style.display="none";
					top.document.all.frameMain.cols="8,*";
				}else{
					obj.src="../Images/shensuo-b.gif";
					document.getElementById("left_").style.display="";
					top.document.all.frameMain.cols="187,*";
				}
			}
            //下拉菜单
            function mOvr(src,clrOver){
	            if (!src.contains(event.fromElement)) {
		            src.style.cursor = 'hand';
		            src.bgColor = clrOver;
	            }
            }
            function mOut(src,clrIn)  {
	            if (!src.contains(event.toElement)) {
		            src.style.cursor = 'default';
		            src.bgColor = clrIn;
	            }
            }
            
            function onSelect(id){          
	            for(var i=0;i<30;i++){
	                var stemp = document.getElementById("menuID_"+i);
		            if(i!=id){		             
		                 if(stemp!=null&&document.getElementById("menuID_"+i)!="undefined")
		                    stemp.style.display="none";
		            }
		            else
		            {
		                if(stemp!=null&&document.getElementById("menuID_"+i)!="undefined")
		                {
		                    if(stemp.style.display=="none")
		                        stemp.style.display="";
		                    else
		                        stemp.style.display="none";
		                }
		            }
		        }
	        }
            function selectMenu(oThis)
			{
				 var oTags = document.getElementById("tab1");
				 var oMenu = oTags.getElementsByTagName("dt");
				 for(i = 0; i < oMenu.length; i++)
				 {
					  oMenu[i].className = "title_close";
				 }
				 oThis.className = "title_open";
			} 
          //返回主页
          function GoUrl(sUrl) {
              parent.MainFrame.location = sUrl;                           

          }
            
    </script>

</head>
<body scroll="no">
    <table width="180" height="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" valign="top" id="left_">
                <table width="175" height="100%" border="0" cellpadding="0" cellspacing="0" class="bian1">
                    <tr>
                        <td>
                            <table width="100%" height="100%" valign="top" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td height="50" align="center" valign="top">
                                        <table width="100%" height="100%" valign="top" border="0" cellpadding="0" cellspacing="0"
                                            name="tableNavArea">
                                            <tr>
                                                <td class="dh_bt">
                                                    自助服务
                                                </td>
                                            </tr>
                                            <tr>          
                                                <td align="center" valign="top">
                                                    <div>
                                                        <dl id="tab1">
                                                            <!--一级分类-->
                                                            <asp:Panel ID="Panel_pdup" runat="server">
                                                                <dt class="title_open" id="firstMenu" onclick="selectMenu(this);GoUrl('../AppForms/frm_Services_List.aspx')"
                                                                    style="width: 175px">&nbsp;&nbsp;&nbsp;&nbsp;服务目录</dt>
                                                            </asp:Panel>
                                                            <asp:Panel ID="Panel_uup" runat="server">
                                                                <dt class="title_close" id="secondMenu" onclick="selectMenu(this);GoUrl('frmRegEventMain.aspx')"
                                                                    style="width: 175px">&nbsp;&nbsp;&nbsp;&nbsp;我登记事项</dt>
                                                            </asp:Panel>
                                                            <asp:Panel ID="Panel1" runat="server">
                                                            <dt class="title_close" id="Dt1" onclick="selectMenu(this);GoUrl('../NewMainPage/NewmainDefine.aspx')"
                                                                    style="width: 175px">&nbsp;&nbsp;&nbsp;&nbsp;待处理事项</dt>
                                                            </asp:Panel>
                                                            <asp:Panel ID="Panel_cgm" runat="server">
                                                                <dt class="title_close" id="fourthMenu" onclick="selectMenu(this);GoUrl('../Forms/frmmodSelf.aspx')"
                                                                    style="width: 175px">&nbsp;&nbsp;&nbsp;&nbsp;个人信息设置</dt>
                                                            </asp:Panel>
                                                            <asp:Panel ID="Panel_cp" runat="server">
                                                                <dt class="title_close" id="fifthMenu" onclick="selectMenu(this);GoUrl('../Forms/FrmModuser.aspx?user')"
                                                                    style="width: 175px">&nbsp;&nbsp;&nbsp;&nbsp;修改密码</dt>
                                                            </asp:Panel>
                                                        </dl>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
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
                <img src="../Images/shensuo-b.gif" width="8" height="126" onclick="changImg(this)">
            </td>
        </tr>
    </table>

    <script language="javascript">
		    selectObj=document.getElementById("home");
    </script>
    
    <asp:Literal ID="literalPopupWindow"   runat="server" Visible="false">
    <script language='javascript' type="text/javascript">
        $(document).ready(function(){                        
            window.showModalDialog('../Forms/AlertMsg.aspx');
        });
    </script>
    </asp:Literal>

</body>
</html>
