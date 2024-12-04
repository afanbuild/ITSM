<!-- #include file ="chdate.asp" -->
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
		<LINK rel="stylesheet" type="text/css" href="main.css">
	</head>
	<body BGCOLOR="#CCCCFF" TEXT="#0000FF" LEFTMARGIN="3" LINK="#000000" VLINK="#000000">
		<table ALIGN="CENTER" BORDER="1" CELLSPACING="0" CELLPADDING="2" BGCOLOR="White" BORDERCOLOR="Gray">
			<tr>
				<td>
					<table WIDTH="140" BORDER="0" CELLPADDING="1" CELLSPACING="0" BGCOLOR="#FFFFFF">
						<tr HEIGHT="18" BGCOLOR="Silver">
							<td WIDTH="20" HEIGHT="18" ALIGN="LEFT" VALIGN="MIDDLE"><a HREF="<%=sScript%>?month=<%=IntPrevMonth %>&amp;year=<%=IntPrevYear %>"><img SRC="../images/prev.gif" WIDTH="10" HEIGHT="18" BORDER="0" ALT="Previous Month"></a></td>
							<td WIDTH="120" COLSPAN="5" ALIGN="CENTER" VALIGN="MIDDLE" CLASS="SOME"><%= strMonthName & " " & intThisYear %></td>
							<td WIDTH="20" HEIGHT="18" ALIGN="RIGHT" VALIGN="MIDDLE"><a HREF="<%=sScript %>?month=<%=IntNextMonth %>&amp;year=<%=IntNextYear %>"><img SRC="../images/next.gif" WIDTH="10" HEIGHT="18" BORDER="0" ALT="Next Month"></a></td>
						</tr>
						<tr>
							<td ALIGN="RIGHT" CLASS="SOME" WIDTH="20" HEIGHT="15" VALIGN="BOTTOM">日</td>
							<td ALIGN="RIGHT" CLASS="SOME" WIDTH="20" HEIGHT="15" VALIGN="BOTTOM">一</td>
							<td ALIGN="RIGHT" CLASS="SOME" WIDTH="20" HEIGHT="15" VALIGN="BOTTOM">二</td>
							<td ALIGN="RIGHT" CLASS="SOME" WIDTH="20" HEIGHT="15" VALIGN="BOTTOM">三</td>
							<td ALIGN="RIGHT" CLASS="SOME" WIDTH="20" HEIGHT="15" VALIGN="BOTTOM">四</td>
							<td ALIGN="RIGHT" CLASS="SOME" WIDTH="20" HEIGHT="15" VALIGN="BOTTOM">五</td>
							<td ALIGN="RIGHT" CLASS="SOME" WIDTH="20" HEIGHT="15" VALIGN="BOTTOM">六</td>
						</tr>
						<tr>
							<td HEIGHT="1" ALIGN="MIDDLE" COLSPAN="7"><img SRC="../images/line.gif" HEIGHT="1" WIDTH="140" BORDER="0"></td>
						</tr>
						<!--  #include file ="showcal.asp" -->
					</table>
				</td>
			</tr>
		</table>
		<!-- #include file="form.inc" -->
	</body>
</html>
