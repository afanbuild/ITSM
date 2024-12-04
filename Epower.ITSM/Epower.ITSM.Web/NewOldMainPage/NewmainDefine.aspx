<%@ Page language="c#" Codebehind="NewmainDefine.aspx.cs"  EnableViewState="false" AutoEventWireup="True" Inherits="Epower.ITSM.Web.NewOldMainPage.NewmainDefine"  %>

<html>
	<HEAD runat="server">
		<TITLE>个人工作区首页</TITLE>
		<base onmouseover="window.status='';return true"/>
		<script src="../Js/DefineMainPage.js" type="text/javascript" language="javascript"></script>
	</HEAD>
	<body class="body" background="" topmargin="5" leftmargin="5"  scroll="yes">
		<FORM id="Form1" name="postForm" method="post" runat="server">
			<div id="divMain" style="BORDER-RIGHT: 0px; BORDER-TOP: 0px; OVERFLOW: auto; BORDER-LEFT: 0px; WIDTH: 100%; BORDER-BOTTOM: 0px; HEIGHT: 90%"
				align="center" name="divMain" DESIGNTIMEDRAGDROP="549">
				<table cellSpacing="0" cellPadding="5" width="100%" border="0" >
					<tr>
                      <td id="col_l" width="66%" valign="top">
                            <asp:Literal ID="ltlLeft" runat="server"></asp:Literal>
                            <div></div>
                      </td>
                      <td id="col_r" align="center" valign="top">
                            <asp:Literal ID="ltlRight" runat="server"></asp:Literal>
                            <div></div>
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
