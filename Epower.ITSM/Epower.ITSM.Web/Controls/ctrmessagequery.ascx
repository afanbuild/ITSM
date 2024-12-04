<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.CtrMessageQuery" Codebehind="CtrMessageQuery.ascx.cs" %>
	<script type="text/javascript">
//	    $(function() {
//	        $("#<%=txtMsgDateBegin.ClientID %>").datepicker(selectDatepickerConfig);
//	        $("#<%=txtMsgDateEnd.ClientID %>").datepicker(selectDatepickerConfig);
//	        $("#<%=txtProcessBegin.ClientID %>").datepicker(selectDatepickerConfig);
//	        $("#<%=txtProcessEnd.ClientID %>").datepicker(selectDatepickerConfig);
//	    });
		</script>

<style type="text/css">
    #messagequery td 
    {
    	border: 1px solid #CEE3F2;
    }
</style>			
		
		
<TABLE id="messagequery" width="100%"	align="center"  cellpadding="0" cellspacing="0" class="listTitle">
		<TR>
			<TD  class="listTitle">状态</TD>
			<TD   align=left class="list"><FONT face="宋体">
					<asp:DropDownList id="cboStatus" Width="120px" runat="server"></asp:DropDownList></FONT></TD>
			<TD  class="listTitle"><FONT face="宋体">标题</FONT></TD>
			<TD   align=left class="list">
				<asp:TextBox id="txtSubject" Width="236px" runat="server"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD  class="listTitle">应用范围</TD>
			<TD   align=left class="list">
				<asp:DropDownList id="cboApp" Width="120px" runat="server"></asp:DropDownList></TD>
			<TD  class="listTitle">事项范围</TD>
			<TD  align=left class="list"><FONT face="宋体">
					<asp:DropDownList id="cboMsgRange" Width="120px" runat="server" Enabled="False"></asp:DropDownList></FONT></TD>
		</TR>
		<TR>
			<TD  class="listTitle">事项日期</TD>
			<TD colspan="3"   align=left class="list">
				<asp:TextBox id="txtMsgDateBegin" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" runat="server" Width="108px" style="background-image:url(../Controls/time/My97DatePicker/skin/datePicker.gif);background-position:right;background-repeat:no-repeat;"></asp:TextBox>
				<%--<IMG id="imgSBegin" runat="server" src="../Controls/Calendar/calendar.gif" style="CURSOR: hand">~--%>~
				<asp:TextBox id="txtMsgDateEnd" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" runat="server" Width="108px" style="background-image:url(../Controls/time/My97DatePicker/skin/datePicker.gif);background-position:right;background-repeat:no-repeat;"></asp:TextBox>
				<%--<IMG id="imgEEnd" runat="server" src="../Controls/Calendar/calendar.gif" style="CURSOR: hand">--%> 
			</TD>
		</TR>
		<TR>
			<TD  class="listTitle">处理时间</TD>
			<TD colspan="3"   align=left class="list">
				<asp:TextBox id="txtProcessBegin" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" runat="server" Width="108px" style="background-image:url(../Controls/time/My97DatePicker/skin/datePicker.gif);background-position:right;background-repeat:no-repeat;"></asp:TextBox>

				<%--<IMG id="imgBegin" runat="server" src="../Controls/Calendar/calendar.gif" style="CURSOR: hand">--%>~
				<asp:TextBox id="txtProcessEnd" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" runat="server" Width="108px" style="background-image:url(../Controls/time/My97DatePicker/skin/datePicker.gif);background-position:right;background-repeat:no-repeat;"></asp:TextBox>
				<%--<IMG id="imgEnd" runat="server" src="../Controls/Calendar/calendar.gif" style="CURSOR: hand">--%>
			</TD>
		</TR>
</TABLE>

<div></div>
