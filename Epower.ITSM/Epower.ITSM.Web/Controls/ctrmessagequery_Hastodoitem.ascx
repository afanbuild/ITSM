<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrmessagequery_Hastodoitem.ascx.cs" Inherits="Epower.ITSM.Web.Controls.ctrmessagequery_Hastodoitem" %>
<script language="javascript" src="../Controls/Calendar/Popup.js"></script>
<TABLE class="listContent" id="Table1" width="100%"
	align="center">
		<TR>
			<TD noWrap class="listTitle">状态</TD>
			<TD noWrap  align=left class="list"><FONT face="宋体">
					<asp:DropDownList id="cboStatus" Width="120px" runat="server"></asp:DropDownList></FONT></TD>
			<TD noWrap class="listTitle"><FONT face="宋体">标题</FONT></TD>
			<TD noWrap  align=left class="list">
				<asp:TextBox id="txtSubject" Width="236px" runat="server"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD noWrap class="listTitle">应用范围</TD>
			<TD noWrap  align=left class="list">
				<asp:DropDownList id="cboApp" Width="120px" runat="server"></asp:DropDownList></TD>
			<TD noWrap class="listTitle">事项范围</TD>
			<TD noWrap align=left class="list"><FONT face="宋体">
					<asp:DropDownList id="cboMsgRange" Width="120px" runat="server" Enabled="False"></asp:DropDownList></FONT></TD>
		</TR>
		<TR>
			<TD noWrap class="listTitle">事项日期</TD>
			<TD colspan="3" noWrap  align=left class="list">
				<asp:TextBox id="txtMsgDateBegin" runat="server" Width="108px" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" style="background-image:url(../Controls/time/My97DatePicker/skin/datePicker.gif);background-position:right;background-repeat:no-repeat;"></asp:TextBox>
				<IMG id="imgSBegin" runat="server" src="../Controls/Calendar/calendar.gif" style="CURSOR: hand; display:none;">~
				<asp:TextBox id="txtMsgDateEnd" runat="server" Width="108px" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" style="background-image:url(../Controls/time/My97DatePicker/skin/datePicker.gif);background-position:right;background-repeat:no-repeat;"></asp:TextBox>
				<IMG id="imgEEnd" runat="server" src="../Controls/Calendar/calendar.gif" style="CURSOR: hand;display:none;">
			</TD>
		</TR>
		<TR>
			<TD noWrap class="listTitle">处理时间</TD>
			<TD colspan="3" noWrap  align=left class="list">
				<asp:TextBox id="txtProcessBegin" runat="server" Width="108px" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" style="background-image:url(../Controls/time/My97DatePicker/skin/datePicker.gif);background-position:right;background-repeat:no-repeat;"></asp:TextBox>
				<IMG id="imgBegin" runat="server" src="../Controls/Calendar/calendar.gif" style="CURSOR: hand;display:none;">~
				<asp:TextBox id="txtProcessEnd" runat="server" Width="108px" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" style="background-image:url(../Controls/time/My97DatePicker/skin/datePicker.gif);background-position:right;background-repeat:no-repeat;"></asp:TextBox>
				<IMG id="imgEnd" runat="server" src="../Controls/Calendar/calendar.gif" style="CURSOR: hand;display:none;">
			</TD>
		</TR>
</TABLE>