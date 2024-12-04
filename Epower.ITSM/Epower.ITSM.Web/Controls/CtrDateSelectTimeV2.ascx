<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrDateSelectTimeV2.ascx.cs" 
    Inherits="Epower.ITSM.Web.Controls.ctrDateSelectTimeV2" %>
	
<div id="Div1" runat="server" style="position:relative;">	
<asp:textbox id="txtBeginDate" runat="server" MaxLength="20"  Width="100"  style="background-image:url(../Controls/time/My97DatePicker/skin/datePicker.gif);background-position:right;background-repeat:no-repeat;"></asp:textbox>
~
<asp:textbox id="txtEndDate" runat="server" MaxLength="20" Width="100"  style="background-image:url(../Controls/time/My97DatePicker/skin/datePicker.gif);background-position:right;background-repeat:no-repeat;"></asp:textbox>
</div>

<%--
<!--Begin: 初始化基础脚本-->
<script src="../js/epower.base.js" type="text/javascript"></script>
<script type="text/javascript">       
    epower.date_select_time = {};           
    epower.date_select_time.begindate = '<%=txtBeginDate.ClientID %>';
    epower.date_select_time.enddate = '<%=txtEndDate.ClientID %>';
</script>
<!--Begin: 初始化基础脚本-->

<!--Begin: 日期选择处理脚本-->
<script language="javascript" src="../js/epower.analsysforms.datetimectrl.js" type="text/javascript"></script>
<!--End: 日期选择处理脚本-->
--%>