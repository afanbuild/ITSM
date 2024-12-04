﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShowWorkIssues.ascx.cs" Inherits="Epower.ITSM.Web.AppSchedules.ShowWorkIssues" %>
<%@ Register assembly="MyCalendar.UI" namespace="MyCalendar.UI" tagprefix="uap" %>


<uap:IssuesCalendar ID="DataCalendar1" runat="server" Width="800" 
            CssClass="dayStyle" onselectionchanged="DataCalendar1_SelectionChanged" 
           
             >
                <TitleStyle BackColor="#0033ff"  ForeColor="white" Font-Bold="True"/>
                <DayHeaderStyle BackColor="#6699ff"  Font-Size="12" ForeColor="white" Font-Bold="True"/>
                <NextPrevStyle ForeColor="white" Font-Size="18px" Font-Bold="True"/>                                                            
                <DayStyle Font-Size="10" />
                <DayWithEventsStyle ForeColor="#0033ff" Font-Bold="True" />                
                <SelectedDayStyle BackColor="#99ccff" />
                <WeekendDayStyle BackColor="lightYellow" />
                <OtherMonthDayStyle BackColor="LightGray" ForeColor="DarkGray"  />
                <OutOfAreaStyle BackColor="lightYellow" />
               <PersonItemTemplate>
                        <br />  <%--<%#Container.DataItem.PersonName%>(<%#Container.DataItem.WorkType%>)--%>
                        <a href="PersonIssues.aspx?StartDate=<%=StartTime.ToShortDateString() %>&EndDate=<%=EndTime.ToShortDateString() %>&AreaID=<%=AreaId %>&EngineerId=<%#Container.DataItem.PersonId %>"
                                                        ><%#Container.DataItem.PersonName%>(<%#Container.DataItem.WorkType%>)</a>
    
               </PersonItemTemplate>                
        </uap:IssuesCalendar>