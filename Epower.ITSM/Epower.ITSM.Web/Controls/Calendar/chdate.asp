<%
	'------------------------------------------------------------
	' This function finds the last date of the given month
	'------------------------------------------------------------
	Function GetLastDay(intMonthNum, intYearNum)
		Dim dNextStart
		If CInt(intMonthNum) = 12 Then
			dNextStart = CDate( "1/1/" & intYearNum)
		Else
			dNextStart = CDate(intMonthNum + 1 & "/1/" & intYearNum)
		End If
		GetLastDay = Day(dNextStart - 1)
	End Function
	
	'-------------------------------------------------------------------------
	' This routine prints the individual table divisions for days of the month
	'-------------------------------------------------------------------------
	Sub Write_TD(sValue, sClass)
		Response.Write "		<TD ALIGN='RIGHT' WIDTH=20 HEIGHT=15 VALIGN='BOTTOM' CLASS='" & sClass & "'> " & sValue & "</TD>" & vbCrLf
	End Sub


	' Constants for the days of the week
	Const cSUN = 1, cMON = 2, cTUE = 3, cWED = 4, cTHU = 5, cFRI = 6, cSAT = 7
	
	' Get the name of this file
	sScript = Request.ServerVariables("SCRIPT_NAME")
	
	' Check for valid month input
	If IsEmpty(Request("MONTH")) OR NOT IsNumeric(Request("MONTH")) Then
	  datToday = Date()
	  intThisMonth = Month(datToday)
	ElseIf CInt(Request("MONTH")) < 1 OR CInt(Request("MONTH")) > 12 Then
	  datToday = Date()
	  intThisMonth = Month(datToday)
	Else
	  intThisMonth = CInt(Request("MONTH"))
	End If
	
	' Check for valid year input
	If IsEmpty(Request("YEAR")) OR NOT IsNumeric(Request("YEAR")) Then
	  datToday = Date()
	  intThisYear = Year(datToday)
	Else
	  intThisYear = CInt(Request("YEAR"))
	End If

	strMonthName = MonthName(intThisMonth)
	datFirstDay = DateSerial(intThisYear, intThisMonth, 1)
	intFirstWeekDay = WeekDay(datFirstDay, vbSunday)
	intLastDay = GetLastDay(intThisMonth, intThisYear)
	
	' Get the previous month and year
	intPrevMonth = intThisMonth - 1
	If intPrevMonth = 0 Then
		intPrevMonth = 12
		intPrevYear = intThisYear - 1
	Else
		intPrevYear = intThisYear	
	End If
	
	' Get the next month and year
	intNextMonth = intThisMonth + 1
	If intNextMonth > 12 Then
		intNextMonth = 1
		intNextYear = intThisYear + 1
	Else
		intNextYear = intThisYear
	End If

	' Get the last day of previous month. Using this, find the sunday of
	' last week of last month
	LastMonthDate = GetLastDay(intLastMonth, intPrevYear) - intFirstWeekDay + 2
	NextMonthDate = 1

	' Initialize the print day to 1  
	intPrintDay = 1

	' Open a record set of schedules
	
%>
