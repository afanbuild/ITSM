<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectTime.aspx.cs" Inherits="Epower.ITSM.Web.Controls.TimeSelect.SelectTime" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title> 
    <script language="javascript" src="ForToday.js" type="text/javascript" charset="gb2312"></script> 
    
    <style type="text/css"> 

        .DayStyle01
        {
	        /*font-size:10px;*/
	      /*  font-weight:bold;	*/
	        /*background-color:#fcffd0; color:#0b333c;*/
	        background-color:#A3C9E1; color:#0b333c;
	        cursor:pointer;
        }
        .DayStyle1
        {
        	background-color:#FFF;
	        /*font-size:10px;	*:
	       /*  font-weight:bold;		*/
        }
        
        .WdateDiv *{font-size:9pt;white-space:nowrap;}
        .dpButton{ 
	        height:18px;
	        width:45px;
	        border:0px;
	        padding-top:2px;
	        background:url(../TimeSelect/images/btnbg.jpg);
	        color:#FFF;
	        cursor:pointer;
        }
          <!--
    BODY {
	    margin: 0;

    }
    -->
</style>
  
</head>
<body style="width:220">
    <form id="form1" runat="server">
    <!-------时间开始--------->
    <input id="hidYear" runat="server" type="hidden" />
    <input id="HidMonth" runat="server"  type="hidden" /> 
    
    <input id="hidYearEnd" runat="server" type="hidden" />
    <input id="HidMonthEnd" runat="server"  type="hidden" />
        
    <table border="0" align="center" cellpadding="0" cellspacing="0" style="padding-left:0px;padding-top:0px;" class="WdateDiv">     
    <tr>
    <td id="calenda" style="padding-left:0px;padding-top:0px;" align="center" valign="top" background="../images/calendar-bg_r3_c1.jpg">
    <div>
        <table width="180px">
            <tr>
                <td align="center" style="position:relative; background-image:url(../TimeSelect/images/bg.jpg);height:20px;" id="YesarOrMonth"><span id="SpanYears" onclick="ShowDivYears(document.getElementById('SpanYears').innerHTML)" style="cursor:pointer;"></span>&nbsp;&nbsp;&nbsp;年&nbsp;&nbsp;&nbsp;<span id="SpanMonth" onclick="ShowDivHours()" style="cursor:pointer;"></span>&nbsp;&nbsp;&nbsp;月</td>
            </tr>
        </table> 
    </div>
    <div id="TimeDay" runat="server">
    </div></td>
    <td>&nbsp;&nbsp;</td>
    <td id="calenda2" style="padding-left:0px;padding-top:0px;" align="center" valign="top" background="../images/calendar-bg_r3_c1.jpg">
     <div>
        <table width="180px">
            <tr>
                <td align="center" style="position:relative; background-image:url(../TimeSelect/images/bg.jpg);height:20px;" id="YesarOrMonthEnd"><span id="SpanYearsEnd" onclick="ShowDivYearsEnd(document.getElementById('SpanYearsEnd').innerHTML)" style="cursor:pointer;"></span>&nbsp;&nbsp;&nbsp;年&nbsp;&nbsp;&nbsp;<span id="SpanMonthEnd" onclick="ShowDivHoursEnd()" style="cursor:pointer;"></span>&nbsp;&nbsp;&nbsp;月</td>
            </tr>
        </table> 
    </div>
    <div id="TimeDay2" runat="server"></div>
    </td>
    </tr>  
    <tr>
    <td colspan="3">
        <div>
            <table width="100%">
            <tr>
            <td ><img id="quick" src="../TimeSelect/images/qs.jpg" style="cursor:pointer;" title="快速选择" /></td>
            <td align="right" ><input type="button" id="Button5" class="dpButton" class="dpButton"  value="本月" onclick="onThisMonth()" />&nbsp;<input type="button" id="Button4" class="dpButton" class="dpButton"  value="本周" onclick="onThisWeekDay()" />&nbsp;<input type="button" id="Button3" class="dpButton" class="dpButton"  value="昨天" onclick="onYesterday()" />&nbsp;<input type="button" id="Button1" class="dpButton" class="dpButton"  value="今天" onclick="onDateToday()" />&nbsp;<input type="button" id="inputClear" class="dpButton"  value="清空" onclick="onClear()" />&nbsp;<input type="button" id="Button2" class="dpButton"  value="确定" onclick="onConfirm();" />&nbsp;</td>
            </tr>
            </table>
        </div>
    </td>
    </tr>       
    </table>   
    <!-------时间结束--------->  
     <input type="hidden" id="BeginTime" value="" runat="server" />
     <input type="hidden" id="EndTime" value="" runat="server" />
   
      <script type="text/javascript" language="javascript" defer="defer">
      
       //获取已经选中的值 给控件赋初值
       var bTimeAndeTime = window.parent.parenReturnConfirm();
       if(bTimeAndeTime!="")
       {
           var arr = bTimeAndeTime.split("|");
           var bTime = arr[0];
           var eTime = arr[1];
           document.getElementById("<%=BeginTime.ClientID%>").value=bTime;
           document.getElementById("<%=EndTime.ClientID%>").value=eTime;
           var barr = bTime.split("-");
           var earr = eTime.split("-");
           
           document.getElementById("<%=hidYear.ClientID%>").value = barr[0];
           document.getElementById("<%=HidMonth.ClientID%>").value = barr[1];
           document.getElementById("<%=hidYearEnd.ClientID%>").value = earr[0];
           document.getElementById("<%=HidMonthEnd.ClientID%>").value = earr[1];
       }
      
       
       document.getElementById("SpanYears").innerHTML="&nbsp;&nbsp;&nbsp;&nbsp;"+document.getElementById("<%=hidYear.ClientID%>").value+"&nbsp;&nbsp;&nbsp;&nbsp;"; 
       document.getElementById("SpanMonth").innerHTML="&nbsp;&nbsp;&nbsp;&nbsp;"+document.getElementById("<%=HidMonth.ClientID%>").value+"&nbsp;&nbsp;&nbsp;&nbsp;"; 
       
       
       
       document.getElementById("SpanYearsEnd").innerHTML="&nbsp;&nbsp;&nbsp;&nbsp;"+document.getElementById("<%=hidYearEnd.ClientID%>").value+"&nbsp;&nbsp;&nbsp;&nbsp;"; 
       document.getElementById("SpanMonthEnd").innerHTML="&nbsp;&nbsp;&nbsp;&nbsp;"+document.getElementById("<%=HidMonthEnd.ClientID%>").value+"&nbsp;&nbsp;&nbsp;&nbsp;"; 
         
        var  hidYear=document.getElementById("<%=hidYear.ClientID%>").value;
        var  HidMonth=document.getElementById("<%=HidMonth.ClientID%>").value;         
        changeMonthBegin2(hidYear,HidMonth);//开始时间        
        var  hidYearEnd=document.getElementById("<%=hidYearEnd.ClientID%>").value;
        var  HidMonthEnd=document.getElementById("<%=HidMonthEnd.ClientID%>").value;                 
        changeMonthEnd2(hidYearEnd,HidMonthEnd);//结束时间
        
        
            
        function changeMonthBegin2(hidYear,HidMonth)
        {       
        
              
          var xmlhttp = null;
          try  
			{  
				xmlhttp = new ActiveXObject("MSXML2.XMLHTTP");  
			}  
			catch(e)  
			{  
				try  
				{  
					xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");  
				}  
				catch(e2){}  
			}
            if(xmlhttp != null)
            {
                try
                {	
					xmlhttp.open("GET", "DateTimeDay.ashx?years="+escape(hidYear)+"&month=" + escape(HidMonth)+"&type=Begin", false); 
                    xmlhttp.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
					xmlhttp.onreadystatechange = function() 
												{ 
												   
													if ( xmlhttp.readyState==4 ) 
													{ 													
													    var msg=xmlhttp.responseText													 
													    document.getElementById("<%=TimeDay.ClientID%>").innerHTML=msg;  
												    } 
												}
					xmlhttp.send(null); 
				}catch(e3){}
            }
        }
        
        function changeMonthEnd2(hidYear,HidMonth,objDivEnd)
        {             
            var xmlhttp = null;
            try  
			{  
				xmlhttp = new ActiveXObject("MSXML2.XMLHTTP");  
			}  
			catch(e)  
			{  
				try  
				{  
					xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");  
				}  
				catch(e2){}  
			}   
            if(xmlhttp != null)
            {
                try
                {
                    xmlhttp.open("GET", "DateTimeDay.ashx?years=" + escape(hidYear) + "&month=" + escape(HidMonth) + "&type=End", false); 
                    xmlhttp.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
					xmlhttp.onreadystatechange = function() 
												{ 
													if ( xmlhttp.readyState==4 ) 
													{ 													
													    var msg=xmlhttp.responseText													 
													    document.getElementById("<%=TimeDay2.ClientID%>").innerHTML=msg;  													    
												    } 
												}
					xmlhttp.send(null); 
				}catch(e3){}
            }
        }
        

       if(bTimeAndeTime!="")
       {
           var arr = bTimeAndeTime.split("|");
           var bTime = arr[0];
           var eTime = arr[1];
           
           var tdDay=document.getElementById("TD_DivBegin-"+bTime);
           OnClickNodeTopDayBegin(tdDay);
           var tdDay2=document.getElementById("TD_DivEnd-"+eTime);
           OnClickNodeTopDayEnd(tdDay2);          
       }

      //  XZdayStley();
    </script>   
             </form>    
</body>
</html>
