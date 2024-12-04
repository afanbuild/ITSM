<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrDateSelectTime.ascx.cs" Inherits="Epower.ITSM.Web.Controls.ctrDateSelectTime" %>

<script language="javascript" src="../Controls/TimeSelect/JsDiv.js" type="text/javascript"></script> 

<div id="DivTxtTime" runat="server" style="position:relative;">
<asp:TextBox ID="TxtTime" runat="server" style="background-image:url(../Controls/time/My97DatePicker/skin/datePicker.gif);background-position:right;background-repeat:no-repeat;"></asp:TextBox>
</div>
<input id="HidBeginTime" value="" runat="server" type="hidden" />
<input id="HidBeginEnd" value="" runat="server" type="hidden" />
<link type="text/css" href="../Controls/TimeSelect/showdiv.css"  rel="Stylesheet"/>

<!--Begin: 初始化基础脚本-->
<script src="../js/epower.base.js" type="text/javascript"></script>
<script type="text/javascript">       
    epower.date_select_time = {};    
    epower.date_select_time.container = '<%=DivTxtTime.ClientID %>';     
</script>
<!--Begin: 初始化基础脚本-->

<!--Begin: 日期选择处理脚本-->
<script language="javascript" src="../js/epower.analsysforms.datetimectrl.js" type="text/javascript"></script>
<!--End: 日期选择处理脚本-->
