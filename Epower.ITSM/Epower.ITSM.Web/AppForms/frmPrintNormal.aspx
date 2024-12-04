<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPrintNormal.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmPrintNormal" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"  TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="CtrlProcess" Src="~/Controls/CtrlProcess.ascx" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
		<style type="text/css"> 
		<!-- #head { height: 30px; width: 440px; font-size: 20px; line-height: 30px; word-break:break-all; margin-right: auto; margin-left: auto; text-align: center; font-weight: bold; }
	#head_line { height: 24px; width: 440px; font-size: 12px; line-height: 24px; word-break:break-all; margin-right: auto; margin-left: auto; text-align: center; font-weight: normal; vertical-align: bottom; margin-top: 2px; }
	#main{ width: 440px; margin-right: auto; margin-left: auto; border-top-width: 1px; border-left-width: 1px; border-top-style: solid; border-left-style: solid; border-top-color: #000000; border-left-color: #000000; padding: 0px; margin-top: 2px; border-right-width: 1px; border-bottom-width: 1px; border-right-style: solid; border-right-color: #000000; border-bottom-color: #000000; }
	#line1{ height: 35px; position: relative; width: 100%; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: #000000; }
	#line1 div{ FLOAT: left; font-size: 12px; word-break:break-all; text-align: center; }
	#line2{
	width: 100%;
	margin:0px;
	border-bottom-width: 1px;
	border-bottom-style:solid;
	border-bottom-color: #000000;
	position: relative;
}
	#line2 div{ FLOAT: left; font-size: 12px; word-break:break-all; vertical-align: middle; }
	.unit_normal{ border-right-width: 1px; border-right-style: solid; border-right-color: #000000; }
	#line3{ height: 20px; position: relative; width: 100%; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: #000000; }
	#line3 div{ FLOAT: left; font-size: 12px; word-break:break-all; line-height: 20px; text-align: center; }
	#line4{ height: 40px; position: relative; width: 100%; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: #000000; }
	#line4 div{ FLOAT: left; font-size: 12px; word-break:break-all; line-height: 16px; text-align: center; }
	body { margin-left: 0px; margin-top: 0px; margin-right: 0px; margin-bottom: 0px;}
	--> 
		</style>
    
</head>
	<script language="javascript" type="text/javascript" src="../JS/ows.js"></script>
	<script language="javascript" type="text/javascript" src="../JS/OWSBROWS.JS"></script>
<body>
    <form id="form1" runat="server">
    <script language="javascript" type="text/javascript">
        //打印
        function printdiv()
        {
            var headstr = "<html><head><title></title></head><body>";
            var footstr = "</body></html>";
            var newstr = document.all.item("PrintArea").innerHTML;
            var oldstr = document.body.innerHTML;
            document.body.innerHTML = headstr+newstr+footstr;
            window.print();
            document.body.innerHTML = oldstr;
            return false;
        }        
    </script>
            <table class="listContent" width="95%" align="center">
	            <tr>
	                <td align="center" class="list">
	                    <input id="btnPrint" type="button" value="打  印" style="width:60px" onclick="printdiv();"/>
                        <input id="btnClose" type="button" value="关  闭" style="width:60px" onclick="window.close();"/>
	                </td>
	            </tr>
	           </table>
    
      <div id="PrintArea">
			<table class="listContent" width="95%" align="center" cellpadding="0" cellspacing="0">
		        <tr id="showTitle" runat="server">
			        <td noWrap align="left" colspan="4" class="list"><asp:label id="lblPrintTitle" runat="server"></asp:label></td>
		        </tr>
		    </table>
		    <table width="95%" align="center" border="1" cellpadding="1" cellspacing="0">
		        <tr style="display:none">
			        <td noWrap align="left" width="9%" colSpan="1" rowSpan="1" class="listTitle">标题</td>
			        <td colSpan="3" class="list"><asp:label id="labFlowName" runat="server" Visible="False"></asp:label><asp:textbox id="txtFlowName" runat="server" Width="70%" MaxLength="100"></asp:textbox>
				        <asp:label id="lblScript" runat="server"></asp:label>
				        <asp:Label ID="rFlowName" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Red">*</asp:Label>
			        </td>
		        </tr>
		        <tr>
			        <td noWrap align="left" class="listTitle" style="width: 140px">申请人</td>
			        <td class="list"><asp:label id="labApplyName" runat="server"></asp:label></td>
			        <td noWrap align="left" class="listTitle" style="width: 140px">所属部门</td>
			        <td class="list"><asp:label id="labDeptName" runat="server"></asp:label></td>
		        </tr>
		        <tr>
			        <td style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle">开始时间</td>
			        <td style="HEIGHT: 17px" class="list"><asp:label id="labStartDate" runat="server"></asp:label></td>
			        <td style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle">结束时间</td>
			        <td style="HEIGHT: 17px" class="list"><asp:label id="labEndDate" runat="server"></asp:label></td>
		        </tr>
		        <TR id="ShowDate1" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subDate1" runat="server" >日期1</asp:label></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><asp:label id="labDate1" runat="server" Visible="False"></asp:label><asp:textbox id="txtDate1" runat="server" Width="160px" MaxLength="20"></asp:textbox><asp:image id="imgDate1" style="CURSOR: hand" runat="server" ImageUrl="../Controls/Calendar/calendar.gif"></asp:image></TD>
		        </TR>
		        <TR id="ShowDate2" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subDate2" runat="server">日期2</asp:label></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><asp:label id="labDate2" runat="server" Visible="False"></asp:label><asp:textbox id="txtDate2" runat="server" Width="160px" MaxLength="20"></asp:textbox><asp:image id="imgDate2" style="CURSOR: hand" runat="server" ImageUrl="../Controls/Calendar/calendar.gif"></asp:image></TD>
		        </TR>
		        <TR id="ShowCate1" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subCate1" runat="server">分类1</asp:label></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><FONT face="宋体"><uc2:ctrFlowCataDropList ID="CtrFCate1" runat="server" /></FONT></TD>
		        </TR>
		        <TR id="ShowCate2" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subCate2" runat="server">分类1</asp:label></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><FONT face="宋体"><uc2:ctrFlowCataDropList ID="CtrFCate2" runat="server" /></FONT></TD>
		        </TR>
		        <TR id="ShowCate3" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subCate3" runat="server">分类1</asp:label></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><FONT face="宋体"><uc2:ctrFlowCataDropList ID="CtrFCate3" runat="server"  /></FONT></TD>
		        </TR>
		        <TR id="ShowCate4" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subCate4" runat="server">分类1</asp:label></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><FONT face="宋体"><uc2:ctrFlowCataDropList ID="CtrFCate4" runat="server"/></FONT></TD>
		        </TR>
		        <TR id="ShowCate5" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subCate5" runat="server">分类1</asp:label></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><FONT face="宋体"><uc2:ctrFlowCataDropList ID="CtrFCate5" runat="server" /></FONT></TD>
		        </TR>
		        <TR id="ShowString1" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subString1" runat="server">字符1</asp:label></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><FONT face="宋体"><asp:label id="labString1" runat="server" Visible="False"></asp:label><asp:textbox id="txtString1" runat="server" Width="70%" MaxLength="100"></asp:textbox></FONT></TD>
		        </TR>
		        <TR id="ShowString2" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subString2" runat="server">字符2</asp:label></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><asp:label id="labString2" runat="server" Visible="False"></asp:label><asp:textbox id="txtString2" runat="server" Width="70%" MaxLength="100"></asp:textbox></TD>
		        </TR>
		        <TR id="ShowString3" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subString3" runat="server">字符3</asp:label></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><asp:label id="labString3" runat="server" Visible="False"></asp:label><asp:textbox id="txtString3" runat="server" Width="70%" MaxLength="100"></asp:textbox></TD>
		        </TR>
		        <TR id="ShowString4" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subString4" runat="server">字符4</asp:label></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><asp:label id="labString4" runat="server" Visible="False"></asp:label><asp:textbox id="txtString4" runat="server" Width="70%" MaxLength="100"></asp:textbox></TD>
		        </TR>
		        <TR id="ShowString5" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subString5" runat="server">字符1</asp:label></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><FONT face="宋体"><asp:label id="labString5" runat="server" Visible="False"></asp:label><asp:textbox id="txtString5" runat="server" Width="70%" MaxLength="100"></asp:textbox></FONT></TD>
		        </TR>
		        <TR id="ShowString6" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subString6" runat="server">字符2</asp:label></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><asp:label id="labString6" runat="server" Visible="False"></asp:label><asp:textbox id="txtString6" runat="server" Width="70%" MaxLength="100"></asp:textbox></TD>
		        </TR>
		        <TR id="ShowString7" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subString7" runat="server">字符3</asp:label></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><asp:label id="labString7" runat="server" Visible="False"></asp:label><asp:textbox id="txtString7" runat="server" Width="70%" MaxLength="100"></asp:textbox></TD>
		        </TR>
		        <TR id="ShowString8" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subString8" runat="server">字符4</asp:label></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><asp:label id="labString8" runat="server" Visible="False"></asp:label><asp:textbox id="txtString8" runat="server" Width="70%" MaxLength="100"></asp:textbox></TD>
		        </TR>
		        <TR id="ShowNumber1" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><FONT face="宋体"><asp:label id="subNumber1" runat="server">数值1</asp:label></FONT></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><FONT face="宋体"><asp:label id="labNumber1" runat="server" Visible="False"></asp:label><asp:textbox id="txtNumber1" runat="server" Width="70%" MaxLength="100" onblur="CheckIsnum(this,'必须为数值！');" style="ime-mode:Disabled" onkeydown="NumberInput('1');"></asp:textbox></FONT></TD>
		        </TR>
		        <TR id="ShowNumber2" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><FONT face="宋体"><asp:label id="subNumber2" runat="server">数值1</asp:label></FONT></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><FONT face="宋体"><asp:label id="labNumber2" runat="server" Visible="False"></asp:label><asp:textbox id="txtNumber2" runat="server" Width="70%" MaxLength="100" onblur="CheckIsnum(this,'必须为数值！');" style="ime-mode:Disabled" onkeydown="NumberInput('1');"></asp:textbox></FONT></TD>
		        </TR>
		        <TR id="ShowNumber3" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><FONT face="宋体"><asp:label id="subNumber3" runat="server">数值1</asp:label></FONT></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><FONT face="宋体"><asp:label id="labNumber3" runat="server" Visible="False"></asp:label><asp:textbox id="txtNumber3" runat="server" Width="70%" MaxLength="100" onblur="CheckIsnum(this,'必须为数值！');" style="ime-mode:Disabled" onkeydown="NumberInput('1');"></asp:textbox></FONT></TD>
		        </TR>
		        <TR id="ShowNumber4" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><FONT face="宋体"><asp:label id="subNumber4" runat="server">数值1</asp:label></FONT></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><FONT face="宋体"><asp:label id="labNumber4" runat="server" Visible="False"></asp:label><asp:textbox id="txtNumber4" runat="server" Width="70%" MaxLength="100" onblur="CheckIsnum(this,'必须为数值！');" style="ime-mode:Disabled" onkeydown="NumberInput('1');"></asp:textbox></FONT></TD>
		        </TR>
		        <TR id="ShowNumber5" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><FONT face="宋体"><asp:label id="subNumber5" runat="server">数值5</asp:label></FONT></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><FONT face="宋体"><asp:label id="labNumber5" runat="server" Visible="False"></asp:label><asp:textbox id="txtNumber5" runat="server" Width="70%" MaxLength="100" onblur="CheckIsnum(this,'必须为数值！');" style="ime-mode:Disabled" onkeydown="NumberInput('1');"></asp:textbox></FONT></TD>
		        </TR>
		        <TR id="ShowBool1" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><FONT face="宋体"><asp:label id="subBool1" runat="server">判断1</asp:label></FONT></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><FONT face="宋体"><asp:checkbox id="chkBool1" runat="server"></asp:checkbox></FONT></TD>
		        </TR>
		        <TR id="ShowBool2" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><FONT face="宋体"><asp:label id="subBool2" runat="server">判断2</asp:label></FONT></TD>
			        <TD style="HEIGHT: 17px" colSpan="3" class="list"><FONT face="宋体"><asp:checkbox id="chkBool2" runat="server"></asp:checkbox></FONT></TD>
		        </TR>
		        <TR id="ShowRemark1" runat="server">
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="subRemark1" runat="server">备注1</asp:label></TD>
			        <TD style="HEIGHT: 17px;word-break:break-all" colSpan="3" class="list"><asp:label id="labRemark1" runat="server" Visible="False"></asp:label><asp:textbox id="txtRemark1" runat="server" Height="64px" Width="100%" MaxLength="500" TextMode="MultiLine"></asp:textbox></TD>
		        </TR>
		         <TR id="ShowDesc" runat="server">
                    <TD colspan="4" class="list">
                        <asp:label id="lblDesc" runat="server" ></asp:label>
                    </TD>
                </TR>
		        <TR >
			        <TD style="HEIGHT: 17px; width: 140px;" noWrap align="left" class="listTitle"><asp:label id="Label1" runat="server">处理过程</asp:label></TD>
			        <TD style="HEIGHT: 17px;word-break:break-all" colSpan="3" class="list"><uc1:ctrlprocess id="CtrlProcess1" runat="server"></uc1:ctrlprocess></TD>
		        </TR>
		        </table>
		        <table class="listContent" width="95%" align="center">
		       <tr id="showBottom" runat="server">
			        <td noWrap align="left" colspan="4" class="list"><asp:label id="lblPrintBottom" runat="server"></asp:label></td>
		        </tr>
	        </table>
       </div>
    </form>
</body>
</html>