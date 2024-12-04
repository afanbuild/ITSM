<%          
			' Initialize the end of rows flag to false
			EndRows = False
			Response.Write vbCrLf
			' Loop until all the rows are exhausted
		 	Do While EndRows = False
				' Start a table row
				Response.Write "	<TR>" & vbCrLf
				' This is the loop for the days in the week
				For intLoopDay = cSUN To cSAT
					' If the first day is not sunday then print the last days of previous month in grayed font
					If intFirstWeekDay > cSUN Then
						Write_TD LastMonthDate, "NON"
						LastMonthDate = LastMonthDate + 1
						intFirstWeekDay = intFirstWeekDay - 1
					' The month starts on a sunday	
					Else
						' If the dates for the month are exhausted, start printing next month's dates
						' in grayed font
						If intPrintDay > intLastDay Then
							Write_TD NextMonthDate, "NON"
							NextMonthDate = NextMonthDate + 1
							EndRows = True 
						Else
							' If last day of the month, flag the end of the row
							If intPrintDay = intLastDay Then
								EndRows = True
							End If
							
							dToday = CDate(intThisMonth & "/" & intPrintDay & "/" & intThisYear) 
							'Response.Write date()
							
							
							End If
							
							' If the event flag is not raise for that day, print it in a plain font
							
						End If 
						
						' Increment the date. Done once in the loop.
						intPrintDay = intPrintDay + 1
				
				' Move to the next day in the week
				Next
				Response.Write "	</TR>" & vbCrLf
				
			Loop 
		%>
