<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    CodeBehind="frmEngineerWork.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmEngineerWork"
    Title="工程师工作情况分析" %>
<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc5" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ctrdateandtime.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript" src="../Controls/Calendar/Popup.js"></script>

    <script language="javascript" type="text/javascript" src="../Js/App_Common.js"> </script>

    <script type="Text/javascript" language="Javascript">
	 var selectobj = document.getElementById("name0");
    
    function chang(obj,img){
            if(obj!=selectobj)	    
		        obj.background="../Images/"+img;
    }
	
        function chang_Class(name, num, my) {
            for (i = 0; i < num; i++) {
                if (i != my) {
					document.getElementById(name+i).className="STYLE4";
					//document.getElementById(name+i).background="../Images/lm-a.gif";
					$("#" + name + i).css("background-image", "url(../Images/lm-a.gif)");
					document.getElementById("a"+i).className="STYLE4";
                }
            }
			selectobj = document.getElementById(name+my);
			document.getElementById(name+my).className="td_3";
			//document.getElementById(name+my).background="../Images/lm-2b.gif";
			$("#" + name + my).css("background-image", "url(../Images/lm-2b.gif)");
			document.getElementById("a"+my).className="td_3";

            switch (my) {
                case 0:
                    document.getElementById("Table1").style.display = "";
                    document.getElementById("Table2").style.display = "";
                    document.getElementById("Table3").style.display = "none";
                    break;
                case 1:
                    document.getElementById("Table1").style.display = "none";
                    document.getElementById("Table2").style.display = "none";
                    document.getElementById("Table3").style.display = "";
                    Contract.location = 'frmEngineerAnalysis.aspx';
                    break;
            }
        }
    </script>

    <script language="javascript" type="text/javascript">
        function btnClick()
        {
            document.getElementById("<%=btnSelect.ClientID %>").click();
        }    
    </script>
    
    <div style="display:none;">
        <asp:Button ID="btnSelect" Width="0px" runat="server" OnClick="btnSelect_Click" />
    </div>

    <table border="0" cellspacing="0" cellpadding="0" width="98%">
        <tr>
            <td>
                <table height="29" border="0" cellpadding="0" cellspacing="0">
                    <tr style="cursor:hand">
                            <td width="7"><img src="../Images/lm-left.gif" width="7" height="29" /></td>
                            <td width="115" height="29" align="center" valign="middle" id="name0" class="td_3" onclick="chang_Class('name',2,0)" background="../Images/lm-2b.gif" onmouseover="chang(this,'lm-2c.gif')" onmouseout="chang(this,'lm-a.gif')"><span id="a0" class="td_3">处理情况分析</span></td>
                            <td width="115" height="29" align="center" valign="middle" id="name1" class="STYLE4" onclick="chang_Class('name',2,1)" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-2c.gif')" onmouseout="chang(this,'lm-a.gif')"><span id="a1" class="STYLE4">工程师工作情况</span></td>
                            <td width="7"><img src="../Images/lm-right.gif" width="7" height="29" /></td>
                    </tr>
                </table> 
            </td>
        </tr>        
        <tr>
           <td>                
                    <table width="100%" cellpadding="2" cellspacing="0" id="Table1" class="listContent">     
                    <tr>
                         <td class="listTitleRight" style="width:12%">
                                            时间范围:
                          </td>
                          <td class="list">
                                            <uc5:ctrdateselecttime id="ctrDateTime" runat="server" OnChangeScript="btnClick()"  />
                          </td>
                                        <td class="listTitleRight" style="width:12%; display:none;">
                                            <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
                                        </td>
                                        <td class="list" style="display:none;">
                                            <asp:DropDownList ID="dpdMastShortName" runat="server" AutoPostBack="True" Width="152px"
                                                OnSelectedIndexChanged="dpdMastShortName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                        </tr>
                    </table>
                </td>
        </tr>
    </table>
    <table width="98%" class="listContent" id="Table2">
        <tr id="tr2" runat="server">
            <td id="tdResultChart" runat="server" class="list">
                <div id="ReportDiv" runat="server">
                </div>
            </td>
        </tr>
    </table>
    <table id="Table3" width="98%" align="center" class="listContent" style="display: none;" border="0">
        <tr>
            <td>
                <iframe id='Contract' src="" width='100%' height='400' scrolling='no' frameborder='no'>
                </iframe>
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">
        selectobj = document.getElementById("name0");
    </script>
</asp:Content>
