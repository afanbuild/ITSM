<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewmainDefine.aspx.cs" Inherits="Epower.ITSM.Web.NewMainPage.NewmainDefine" %>

<html>
	<HEAD id="HEAD1" runat="server">
		<TITLE>待办或待接收事项</TITLE>
		<base onmouseover="window.status='';return true"/>
		<script src="../Js/DefineMainPage.js" type="text/javascript" language="javascript"></script>
	</HEAD>
	<body class="body" background="" topmargin="5" leftmargin="5"  scroll="yes">
		<FORM id="Form1" name="postForm" method="post" runat="server">
			<div id="divMain" style="BORDER-RIGHT: 0px; BORDER-TOP: 0px; OVERFLOW: auto; BORDER-LEFT: 0px; WIDTH: 100%; BORDER-BOTTOM: 0px; HEIGHT: 90%"
				align="center" name="divMain">
				<table cellSpacing="0" cellPadding="5" width="100%" border="0" >
					<tr>
                      <td valign="top">                            
                            <div id="div1" style='position: relative;padding-bottom:10px;'>
                                <table cellspacing='0' width='98%' cellpadding='1' border='0' class='bian' >
                                    <tr class='sy_lmbg'>
                                        <td id='waiteProcess' width='58%'> 
                                         待办事项-按流程分类</td>
                                    </tr>
                                    <tr>
                                        <td  height='10' class='list'>
                                            <div id='divShowControl1'>                                         
                                                <iframe id='iframe1' src='../Forms/frmWaitProcessThing.aspx' width='100%' height='280' frameborder='no' scrolling='no'></iframe>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                      </td>
                     </tr>
				</table>
				<table cellSpacing="0" cellPadding="5" width="100%" border="0" >
				   <tr>
                        <td  valign="top">
                             <div id="div2" style='position: relative;padding-bottom:10px;'>
                                <table cellspacing='0' width='98%' cellpadding='1' border='0' class='bian' >
                                    <tr class='sy_lmbg'>
                                        <td id='Td1' width='58%'> 
                                         待接收事项-按流程分类</td>
                                    </tr>
                                    <tr>
                                        <td height='10' class='list'>
                                            <div id='divShowControl2'>                                        
                                                <iframe id='iframe2' src='../Forms/frmWaitRecieveThing.aspx' width='100%' height='280' frameborder='no' scrolling='no'></iframe>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                     </tr>
				</table>
            </div>
            <input type="hidden" id="userdefined" runat="server" value="false" />
            <input type="hidden" id="hidTabIndex" runat="server" value="false" />            
		</FORM>
		<script language="javascript" type="text/javascript">
		    if (document.getElementById("userdefined").value == "true")
		        _upc();
		</script>
	</body>
</html>

