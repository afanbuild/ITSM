<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.flowReader" Codebehind="flowReader.ascx.cs" %>
<FONT face="����">
	
	<TABLE id="Table2" style="Z-INDEX: 107; LEFT: 8px; POSITION: absolute; TOP: 48px" cellSpacing="1"
		cellPadding="1" width="100" border="0">
		<TR>
			<TD><FONT face="����"><FONT face="����"><INPUT class="FLOWBUTTON" id="cmdReaded"  style="WIDTH: 68px; HEIGHT: 24px" type="button"
							value=" ��  �� " name="cmdReaded" Height="24" Width="82" onclick="ReaderOver()"></FONT></FONT></TD>
		</TR>
		
		<TR>
			<TD><FONT face="����">&nbsp; </FONT>
			</TD>
		</TR>
		<TR>
			<TD><FONT face="����"><INPUT class="FLOWBUTTON" id="cmdViewFlow" style="WIDTH: 68px; HEIGHT: 24px" type="button"
						value="���̲鿴" name="cmdViewFlow" onclick="ViewFlowChart()" Height="24" Width="82"></FONT></TD>
		</TR>
		<TR>
			<TD height="24"></TD>
		</TR>
		<TR>
			<TD colSpan="1" height="24" rowSpan="1"><FONT face="����"></FONT></TD>
		</TR>
		<TR>
			<TD colSpan="1" height="24" rowSpan="1"></TD>
		</TR>
		<TR>
			<TD colSpan="1" height="24" rowSpan="1"><FONT face="����"></FONT></TD>
		</TR>
		<TR>
			<TD colSpan="1" height="24" rowSpan="1"><FONT face="����"></FONT></TD>
		</TR>
		<TR>
			<TD height="24"></TD>
		</TR>
		<TR>
			<TD colSpan="1" height="24" rowSpan="1"></TD>
		</TR>
		<TR>
			<TD><FONT face="����"><FONT face="����"><INPUT class="FLOWBUTTON" id="cmdExit" style="WIDTH: 68px; HEIGHT: 24px" onclick="window.parent.close();"
							type="button" value=" ��  �� " name="cmdExit" Height="24" Width="82"></FONT></FONT></TD>
		</TR>
	</TABLE>
	<INPUT type=hidden value="<%=_SendType%>" id="txtVisabled"> </FONT>
	
	<script language="javascript">
<!--
		if(document.all.txtVisabled.value != "Handle")
		{
			document.all.cmdReaded.style.display="none";
		}
	
//-->
	</script>
