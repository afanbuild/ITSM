<%@ Page language="c#" Codebehind="frmCustMenu.aspx.cs" AutoEventWireup="True" Inherits="Epower.ITSM.Web.CustManager.frmCustMenu" %>
<HTML xmlns="http://www.w3.org/1999/xhtml">
	<HEAD runat="server">
				<style>
		    body
{
	margin: 0px;
}
		</style>
	</HEAD>
	<body>
	<form runat="server">
		<table border="0" cellspacing="0" cellpadding="0" align="right" height="100%">
          <tr>
            <td width="5">&nbsp;</td>
            <td width="100%" height="100%" align="center" valign="top" id="tableNavArea">
              <table width="95%"  border="0" cellpadding="0" cellspacing="0" >     
              <tr>
                <td style="height:6px;" colspan="2">
                </td>
              </tr>
              <tr>
                <td colspan="2" align="center" valign="top">
                    <table width="98%" border="0" cellspacing="0" cellpadding="0" bgcolor="#EEF5FB">
                  
                    <tr>
                      <td colspan="3" align="center" valign="top" class="bian">
                        <table width="100%">
                           <tr >
                            <td style="height:4px"></td>
                          </tr>
                          <tr>
                            <td>搜&nbsp;&nbsp;&nbsp;&nbsp;索&nbsp;<asp:DropDownList ID="ddlObject" runat="server" Width="124px" 
                                    Class="bian" Height="16px">
                                <asp:ListItem Value="0">事件单</asp:ListItem>
                                <asp:ListItem Value="1">投诉单</asp:ListItem>
                                <asp:ListItem Value="2">问题单</asp:ListItem>
                                <asp:ListItem Value="3">变更单</asp:ListItem>
                                <asp:ListItem Value="4">知识库</asp:ListItem>
                            </asp:DropDownList></td>
                          </tr>
                          <tr >
                            <td  style="height:6px"></td>
                          </tr>
                          <tr>
                            <td width="73%">关键字<asp:TextBox ID="txtValue" runat="server" Width="80px" 
                                    CssClass="bian" onkeydown="if (event.keyCode==13){DoSearch()}"></asp:TextBox>
                                  <asp:Button ID="btnSearch" runat="server" OnClientClick="DoSearch();"  Text="搜索" CssClass="btnClass" />
                              </td>
                          </tr>
                          </table>
                      </td>
                    </tr>
                </table></td>
              </tr>
              
              <tr>
                <td style="height:6px" colspan="2">
                </td>
              </tr>
              
               <tr>
                <td colspan="2" align="center" valign="top">
                    <table width="98%" border="0" cellspacing="0" cellpadding="0" bgcolor="#EEF5FB" style="height:470;">
                    <tr>
                      <td colspan="3" align="center" valign="top" class="bian">
                        <table width="100%">
                          <tr>
                            <td valign="top" align="left">
                            <iframe  src='frmCustNar.aspx' width='100%' height="460" frameborder='no' scrolling="no"></iframe>
                            </td>
                          </tr>
                          
                      </table>
                      </td>
                    </tr>
                </table></td>
              </tr>
                    
            </table>
            </td>
            
            <td width="8" valign="top">
                <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="8" id="tdblank">&nbsp;</td>
                    <td width="8" height="100%" valign="middle" background="../images/bg_middle_main.gif">
                    
                   
                    
                    </td>
                  </tr>
                </table>
            </td>
	     </tr>
	     </table>
	   </form>		
	</body>
	<script type="text/javascript">
	   function swapNav()
        {
	        if(typeof(top.document.all.frameMain)=="object")
	        {
		        if(top.document.all.frameMain.cols != "10,*")
		        {
			        document.all.tableNavArea.style.display="none";
			        top.document.all.frameMain.cols="10,*";
			        document.all.imgMiddle.src="../images/shensuo-a.gif";
			        document.all.tdblank.style.display="none";
		        }
		        else
		        {
			        document.all.tableNavArea.style.display="";
			        top.document.all.frameMain.cols="210,*";
			        document.all.imgMiddle.src="../images/shensuo-b.gif";
			        document.all.tdblank.style.display="";
		        }
	        }
        }
        function DoSearch() {
          var templateid = document.all.<%=ddlObject.ClientID%>.value;
           var svalue = document.all.<%=txtValue.ClientID%>.value;
           var sUrl = "";
           if(templateid =="0")  //事件单
           {
                sUrl = "../AppForms/CST_Issue_List.aspx";
           }
           else if(templateid =="1")  //投诉单
           {
                sUrl = "../AppForms/frm_BYTS_Query.aspx";
           }
           else if(templateid =="2")  //问题单
           {
                sUrl = "../ProbleForms/frmProblemMain.aspx";
           }
           else if(templateid =="3")  //变更单
           {
                sUrl = "../EquipmentManager/frm_ChangeQuery.aspx";
           }
           else if(templateid =="4")  //知识库
           {
                sUrl = "../InformationManager/frmInfSearch.aspx";
           }
           sUrl = sUrl + "?svalue=" + escape(svalue);
           window.open(sUrl,"MainFrame","scrollbars=no,status=yes ,resizable=yes");
           event.returnValue = false;
        }
	</script>
	

</HTML>
