

// var month=1;
//$(document).ready(function ()
//{
//    //获得默认时间
//    var uom1 = new Date();
//   $("#textyear").val(uom1.getFullYear());   
//   $("#DateYear").text(uom1.getFullYear());   
//   $("#DateMonth").text(uom1.getMonth()+1);
//   
//   //  ShowGrid();
//   //收入支出图
//    month=(uom1.getMonth()+1);
//   // AjaxSRZCTTalbe(month,"SRZCTJT"); 
//    
//    //加载时间控件
//    infoDateTime(uom1.getFullYear(),month);
//    
// 
//  //选中当天日期
//  XZdayStley();
//   
//});

//选中当天的日期
//function XZdayStley()
//{
//    var uom1 = new Date();
//   var dtYear=uom1.getFullYear()
//   var dtMonth=uom1.getMonth()+1;
//   var dtDay=uom1.getDate();
//    var tdDay=document.getElementById(dtYear+"-"+dtMonth+"-"+dtDay);
//    OnClickNodeTopDay(tdDay);
//}


////时间年的变更
//function upYear(pay)
//{
//   
//   var year=$("#textyear").val();
//   
//    if(pay=="1")
//    {    
//        $("#textyear").val(year-0-1);
//    }else
//    {
//        $("#textyear").val(year-0+1); 
//    }
//}

////时间年的变更年
//function upYear2(pay)
//{
//   
//   var year=$("#DateYear").text();
//   
//    if(pay=="1")
//    {    
//        $("#DateYear").text(year-0-1);
//    }else
//    {
//        $("#DateYear").text(year-0+1); 
//    }
//    infoDateTime($("#DateYear").text(),$("#DateMonth").text());
//}
////获取月
//function upMonth(pay)
//{
//    var strmonth=$("#DateMonth").text();
//        
//    if(pay=="1")
//    {    
//        if(strmonth=="1")
//        {
//            var year=$("#DateYear").text();
//            $("#DateYear").text(year-0-1);
//            $("#DateMonth").text(12);
//        }
//        else
//        {
//            $("#DateMonth").text(strmonth-0-1);
//        }
//    }else
//    {
//        if(strmonth=="12")
//        {
//            var year=$("#DateYear").text();
//            $("#DateYear").text(year-0+1);
//            $("#DateMonth").text(1);
//        }
//        else
//        {
//            $("#DateMonth").text(strmonth-0+1); 
//        }
//        
//    }
//    infoDateTime($("#DateYear").text(),$("#DateMonth").text());
//}


//---日期获得鼠标样式方法开始-----------------------------------------



var lastTdObjBegin = null;
function OnClickNodeTopDayBegin(tdObj){
	if(lastTdObjBegin != null)
	{	
		lastTdObjBegin.className = 'DayStyle1';
    }
        tdObj.className = 'DayStyle01';
        //暂时去掉链接   
        lastTdObjBegin = tdObj;
}
//鼠标进入导航树节点
function OnOverNodeTopDayBegin(tdObj)
{
	if(tdObj != lastTdObjBegin)
	{
		tdObj.className = 'DayStyle01';	
	}
}
//鼠标移出导航树节点
function OnOutNodeTopDayBegin(tdObj)
{
	if(tdObj != lastTdObjBegin)
	{
		tdObj.className = 'DayStyle1';
	}
}




var lastTdObjEnd=null;
function OnClickNodeTopDayEnd(tdObj)
{
	if(lastTdObjEnd != null)
	{	
		lastTdObjEnd.className = 'DayStyle1';
    }
    
        tdObj.className = 'DayStyle01';
        //暂时去掉链接   
        lastTdObjEnd = tdObj;
 
}

//鼠标进入导航树节点
function OnOverNodeTopDayEnd(tdObj)
{
	if(tdObj != lastTdObjEnd)
	{
		tdObj.className = 'DayStyle01';	
	}
}
//鼠标移出导航树节点
function OnOutNodeTopDayEnd(tdObj)
{
	if(tdObj != lastTdObjEnd)
	{
		tdObj.className = 'DayStyle1';
	}
}

//---------------结束-----------------------------------------------------------------


//鼠标进入导航树节点
function OnOverNodeTime(tdObj)
{
	
	tdObj.className = 'DayStyle01';	
	
}
//鼠标移出导航树节点
function OnOutNodeTime(tdObj)
{
	
	tdObj.className = '';	
}




function ShowDivHoursEnd()
{   
   //选择月时，过滤掉年的方法
   if(document.getElementById('ShowDivYears')!=null&&document.getElementById('ShowDivYears')!="undefined")
    {
        document.getElementById('YesarOrMonth').removeChild(document.getElementById('ShowDivYears'));
         document.getElementById("SpanYears").style.background=""; 
    }
    
    if(document.getElementById('ShowDivYearsEnd')!=null&&document.getElementById('ShowDivYearsEnd')!="undefined")
    {
            document.getElementById('YesarOrMonthEnd').removeChild(document.getElementById('ShowDivYearsEnd'));     
            document.getElementById("SpanYearsEnd").style.background="";              
    }
    
    if(document.getElementById('showHoursDiv')!=null&&document.getElementById('showHoursDiv')!="undefined")
    {
        document.getElementById('YesarOrMonth').removeChild(document.getElementById('showHoursDiv'));
         document.getElementById("SpanMonth").style.background=""; 
    } 
    
   var bgObj=document.createElement("div"); 
   bgObj.setAttribute('id','showHoursDivEnd'); 
   bgObj.style.position="absolute"; 
   bgObj.style.top="17px"; 
   bgObj.style.background="#ffffff"; 
   //bgObj.style.filter="progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=75,finishOpacity=25"; 
   bgObj.style.zIndex="10002";
   bgObj.style.left="98px"; 
   bgObj.className="TanChuDiv";
   bgObj.onblur="showTimeDivClose()";
    bgObj.style.width="5px"; 
   bgObj.style.height= "5px";
   var htmlValue="<table style=\"background-color:#ffffff;border:#bbb 1px solid;\"><tr><td  style=\"cursor:pointer\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" onclick=\"onclickMouthEnd('1')\">一月</td><td  onclick=\"onclickMouthEnd('7')\" style=\"cursor:pointer\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >七月</td></tr>";
   htmlValue+="<tr><td style=\"cursor:pointer\"  onclick=\"onclickMouthEnd('2')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >二月</td><td  onclick=\"onclickMouthEnd('8')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >八月</td></tr>";
   htmlValue+="<tr><td style=\"cursor:pointer\" onclick=\"onclickMouthEnd('3')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >三月</td><td  onclick=\"onclickMouthEnd('9')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >九月</td></tr>";
   htmlValue+="<tr><td  style=\"cursor:pointer\" onclick=\"onclickMouthEnd('4')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >四月</td><td  onclick=\"onclickMouthEnd('10')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >十月</td></tr>";
   htmlValue+="<tr><td  style=\"cursor:pointer\" onclick=\"onclickMouthEnd('5')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >五月</td><td  onclick=\"onclickMouthEnd('11')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >十一</td></tr>";
   htmlValue+="<tr><td style=\"cursor:pointer\" onclick=\"onclickMouthEnd('6')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >六月</td><td  onclick=\"onclickMouthEnd('12')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >十二</td></tr>";   
   htmlValue+="</table>";
   document.getElementById('YesarOrMonthEnd').appendChild(bgObj);
   
   document.getElementById('showHoursDivEnd').innerHTML=htmlValue;
   
    document.getElementById("SpanMonthEnd").style.background="#ffffff";
}



function ShowDivHours()
{   
   //选择月时，过滤掉年的方法
   if(document.getElementById('ShowDivYears')!=null&&document.getElementById('ShowDivYears')!="undefined")
    {
        document.getElementById('YesarOrMonth').removeChild(document.getElementById('ShowDivYears'));
         document.getElementById("SpanYears").style.background=""; 
    }
    
    if(document.getElementById('ShowDivYearsEnd')!=null&&document.getElementById('ShowDivYearsEnd')!="undefined")
    {
            document.getElementById('YesarOrMonthEnd').removeChild(document.getElementById('ShowDivYearsEnd'));     
            document.getElementById("SpanYearsEnd").style.background="";              
    }
    
    if(document.getElementById('showHoursDivEnd')!=null&&document.getElementById('showHoursDivEnd')!="undefined")
    {
        document.getElementById('YesarOrMonthEnd').removeChild(document.getElementById('showHoursDivEnd'));
         document.getElementById("SpanMonthEnd").style.background=""; 
    } 
    
    
   var bgObj=document.createElement("div"); 
   bgObj.setAttribute('id','showHoursDiv'); 
   bgObj.style.position="absolute"; 
   bgObj.style.top="17px"; 
   bgObj.style.background="#ffffff"; 
   //bgObj.style.filter="progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=75,finishOpacity=25"; 
   bgObj.style.zIndex="10002";
   bgObj.style.left="98px"; 
   bgObj.className="TanChuDiv";
   bgObj.onblur="showTimeDivClose()";
    bgObj.style.width="5px"; 
   bgObj.style.height= "5px";
   var htmlValue="<table style=\"background-color:#ffffff;border:#bbb 1px solid;\"><tr><td  style=\"cursor:pointer\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" onclick=\"onclickMouth('1')\">一月</td><td  onclick=\"onclickMouth('7')\" style=\"cursor:pointer\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >七月</td></tr>";
   htmlValue+="<tr><td style=\"cursor:pointer\"  onclick=\"onclickMouth('2')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >二月</td><td  onclick=\"onclickMouth('8')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >八月</td></tr>";
   htmlValue+="<tr><td style=\"cursor:pointer\" onclick=\"onclickMouth('3')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >三月</td><td  onclick=\"onclickMouth('9')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >九月</td></tr>";
   htmlValue+="<tr><td  style=\"cursor:pointer\" onclick=\"onclickMouth('4')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >四月</td><td  onclick=\"onclickMouth('10')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >十月</td></tr>";
   htmlValue+="<tr><td  style=\"cursor:pointer\" onclick=\"onclickMouth('5')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >五月</td><td  onclick=\"onclickMouth('11')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >十一</td></tr>";
   htmlValue+="<tr><td style=\"cursor:pointer\" onclick=\"onclickMouth('6')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >六月</td><td  onclick=\"onclickMouth('12')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >十二</td></tr>";   
   htmlValue+="</table>";
   document.getElementById('YesarOrMonth').appendChild(bgObj);
   
   document.getElementById('showHoursDiv').innerHTML=htmlValue;
   
   document.getElementById("SpanMonth").style.background="#ffffff";
}


//年的选择 结束时间的年选择
function ShowDivYearsEnd(objYears)
{
    
    objYears = objYears.replace('&nbsp;','').replace('&nbsp;','').replace('&nbsp;','').replace('&nbsp;','').replace('&nbsp;','').replace('&nbsp;','').replace('&nbsp;','').replace('&nbsp;','')
    //选择年 判断 月的选层是否显示，显示则隐藏
    if(document.getElementById('showHoursDiv')!=null&&document.getElementById('showHoursDiv')!="undefined")
    {
        document.getElementById('YesarOrMonth').removeChild(document.getElementById('showHoursDiv'));
         document.getElementById("SpanMonth").style.background=""; 
    }    
    if(document.getElementById('ShowDivYears')!=null&&document.getElementById('ShowDivYears')!="undefined")
    {
            document.getElementById('YesarOrMonth').removeChild(document.getElementById('ShowDivYears'));     
            document.getElementById("SpanYears").style.background="";              
    }
    
    if(document.getElementById('showHoursDivEnd')!=null&&document.getElementById('showHoursDivEnd')!="undefined")
    {
        document.getElementById('YesarOrMonthEnd').removeChild(document.getElementById('showHoursDivEnd'));
         document.getElementById("SpanMonthEnd").style.background=""; 
    } 
    
        
   var bgObj=document.createElement("div"); 
   bgObj.setAttribute('id','ShowDivYearsEnd'); 
   bgObj.style.position="absolute"; 
   bgObj.style.top="17px"; 
   bgObj.style.background="#ffffff"; 
   //bgObj.style.filter="progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=75,finishOpacity=25"; 
   bgObj.style.zIndex="10002";
   bgObj.style.left="22px"; 
   bgObj.className="TanChuDiv";
   bgObj.onblur="showTimeDivClose()";
    bgObj.style.width="5px"; 
   bgObj.style.height= "5px";
   
   var yeses6=objYears-6;
   var yeses5=objYears-5;
   var yeses4=objYears-4;
   var yeses3=objYears-3;
   var yeses2=objYears-2;
   var yeses1=objYears-1;
   var yeses0=objYears;
   var yesesYI=objYears-0+1;
   var yesesER=objYears-0+2;
   var yesesSan=objYears-0+3;
   var yesesSi=objYears-0+4;
   var yesesWu=objYears-0+5;
   
   var yesesliu=objYears-0+6;
   var yeses7=objYears-6;
   
   
   var htmlValue="<table style=\"background-color:#ffffff;border:#bbb 1px solid;\"><tr><td  style=\"cursor:pointer\"  onmouseover=\"OnOverNodeTime(this)\"  onmouseout=\"OnOutNodeTime(this)\" onclick=\"onclickYearEnd('"+yeses6+"')\">"+yeses6+"</td><td style=\"cursor:pointer\" onclick=\"onclickYearEnd('"+yeses0+"')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >"+yeses0+"</td></tr>";
   htmlValue+="<tr><td style=\"cursor:pointer\" onclick=\"onclickYearEnd('"+yeses5+"')\" onmouseover=\"OnOverNodeTime(this)\"  onmouseout=\"OnOutNodeTime(this)\" >"+yeses5+"</td><td  onclick=\"onclickYearEnd('"+yesesYI+"')\"  style=\"cursor:pointer\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >"+yesesYI+"</td></tr>";
   htmlValue+="<tr><td style=\"cursor:pointer\"  onclick=\"onclickYearEnd('"+yeses4+"')\" onmouseover=\"OnOverNodeTime(this)\"  onmouseout=\"OnOutNodeTime(this)\" >"+yeses4+"</td><td  onclick=\"onclickYearEnd('"+yesesER+"')\"  style=\"cursor:pointer\"  onmouseover=\"OnOverNodeTime(this)\"  onmouseout=\"OnOutNodeTime(this)\" >"+yesesER+"</td></tr>";
   htmlValue+="<tr><td  style=\"cursor:pointer\"  onclick=\"onclickYearEnd('"+yeses3+"')\" onmouseover=\"OnOverNodeTime(this)\"  onmouseout=\"OnOutNodeTime(this)\" >"+yeses3+"</td><td  onclick=\"onclickYearEnd('"+yesesSan+"')\"  style=\"cursor:pointer\" onmouseover=\"OnOverNodeTime(this)\"  onmouseout=\"OnOutNodeTime(this)\" >"+yesesSan+"</td></tr>";
   htmlValue+="<tr><td  style=\"cursor:pointer\" onclick=\"onclickYearEnd('"+yeses2+"')\"  onmouseover=\"OnOverNodeTime(this)\"  onmouseout=\"OnOutNodeTime(this)\" >"+yeses2+"</td><td  onclick=\"onclickYearEnd('"+yesesSi+"')\"  style=\"cursor:pointer\" onmouseover=\"OnOverNodeTime(this)\"  onmouseout=\"OnOutNodeTime(this)\" >"+yesesSi+"</td></tr>";
   htmlValue+="<tr><td style=\"cursor:pointer\"  onclick=\"onclickYearEnd('"+yeses1+"')\"  onmouseover=\"OnOverNodeTime(this)\"  onmouseout=\"OnOutNodeTime(this)\" >"+yeses1+"</td><td  onclick=\"onclickYearEnd('"+yesesWu+"')\"  style=\"cursor:pointer\" onmouseover=\"OnOverNodeTime(this)\"  onmouseout=\"OnOutNodeTime(this)\" >"+yesesWu+"</td></tr>";   
   htmlValue+="<tr><td style=\"cursor:pointer\"  onclick=\"ShowDivYearsEnd('"+yeses7+"')\"  onmouseover=\"OnOverNodeTime(this)\"  onmouseout=\"OnOutNodeTime(this)\" >←—</td><td  onclick=\"ShowDivYearsEnd('"+yesesliu+"')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >—→</td></tr>";   
   htmlValue+="</table>";
   document.getElementById('YesarOrMonthEnd').appendChild(bgObj);
   
   document.getElementById('ShowDivYearsEnd').innerHTML=htmlValue;
   
   document.getElementById("SpanYearsEnd").style.background="#ffffff";
}


function ShowDivYears(objYears)
{   
  
    objYears = objYears.replace('&nbsp;','').replace('&nbsp;','').replace('&nbsp;','').replace('&nbsp;','').replace('&nbsp;','').replace('&nbsp;','').replace('&nbsp;','').replace('&nbsp;','')
    //选择年 判断 月的选层是否显示，显示则隐藏
    if(document.getElementById('showHoursDiv')!=null&&document.getElementById('showHoursDiv')!="undefined")
    {
        document.getElementById('YesarOrMonth').removeChild(document.getElementById('showHoursDiv'));
         document.getElementById("SpanMonth").style.background=""; 
    }
    
     if(document.getElementById('ShowDivYearsEnd')!=null&&document.getElementById('ShowDivYearsEnd')!="undefined")
    {
            document.getElementById('YesarOrMonthEnd').removeChild(document.getElementById('ShowDivYearsEnd'));     
            document.getElementById("SpanYearsEnd").style.background="";              
    }
    
    if(document.getElementById('showHoursDivEnd')!=null&&document.getElementById('showHoursDivEnd')!="undefined")
    {
        document.getElementById('YesarOrMonthEnd').removeChild(document.getElementById('showHoursDivEnd'));
         document.getElementById("SpanMonthEnd").style.background=""; 
    } 
    
    
   var bgObj=document.createElement("div"); 
   bgObj.setAttribute('id','ShowDivYears'); 
   bgObj.style.position="absolute"; 
   bgObj.style.top="17px"; 
   bgObj.style.background="#ffffff"; 
   //bgObj.style.filter="progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=75,finishOpacity=25"; 
   bgObj.style.zIndex="10002";
   bgObj.style.left="22px"; 
   bgObj.className="TanChuDiv";
   bgObj.onblur="showTimeDivClose()";
    bgObj.style.width="5px"; 
   bgObj.style.height= "5px";
   
   var yeses6=objYears-6;
   var yeses5=objYears-5;
   var yeses4=objYears-4;
   var yeses3=objYears-3;
   var yeses2=objYears-2;
   var yeses1=objYears-1;
   var yeses0=objYears;
   var yesesYI=objYears-0+1;
   var yesesER=objYears-0+2;
   var yesesSan=objYears-0+3;
   var yesesSi=objYears-0+4;
   var yesesWu=objYears-0+5;
   
   var yesesliu=objYears-0+6;
   var yeses7=objYears-6;
   
   
   var htmlValue="<table style=\"background-color:#ffffff;border:#bbb 1px solid;\"><tr><td  style=\"cursor:pointer\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" onclick=\"onclickYear('"+yeses6+"')\">"+yeses6+"</td><td  onclick=\"onclickYear('"+yeses0+"')\" style=\"cursor:pointer\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >"+yeses0+"</td></tr>";
   htmlValue+="<tr><td style=\"cursor:pointer\"  onclick=\"onclickYear('"+yeses5+"')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >"+yeses5+"</td><td  onclick=\"onclickYear('"+yesesYI+"')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >"+yesesYI+"</td></tr>";
   htmlValue+="<tr><td style=\"cursor:pointer\" onclick=\"onclickYear('"+yeses4+"')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >"+yeses4+"</td><td  onclick=\"onclickYear('"+yesesER+"')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >"+yesesER+"</td></tr>";
   htmlValue+="<tr><td  style=\"cursor:pointer\" onclick=\"onclickYear('"+yeses3+"')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >"+yeses3+"</td><td  onclick=\"onclickYear('"+yesesSan+"')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >"+yesesSan+"</td></tr>";
   htmlValue+="<tr><td  style=\"cursor:pointer\" onclick=\"onclickYear('"+yeses2+"')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >"+yeses2+"</td><td  onclick=\"onclickYear('"+yesesSi+"')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >"+yesesSi+"</td></tr>";
   htmlValue+="<tr><td style=\"cursor:pointer\" onclick=\"onclickYear('"+yeses1+"')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >"+yeses1+"</td><td  onclick=\"onclickYear('"+yesesWu+"')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >"+yesesWu+"</td></tr>";   
   htmlValue+="<tr><td style=\"cursor:pointer\" onclick=\"ShowDivYears('"+yeses7+"')\" onmouseover=\"OnOverNodeTime(this)\" onmouseout=\"OnOutNodeTime(this)\" >←—</td><td  onclick=\"ShowDivYears('"+yesesliu+"')\" onmouseover=\"OnOverNodeTime(this)\" style=\"cursor:pointer\" onmouseout=\"OnOutNodeTime(this)\" >—→</td></tr>";   
   htmlValue+="</table>";
   document.getElementById('YesarOrMonth').appendChild(bgObj);
   
   document.getElementById('ShowDivYears').innerHTML=htmlValue;
   
   document.getElementById("SpanYears").style.background="#ffffff";
}

function onclickYearEnd(obj)
{
    document.getElementById("SpanYearsEnd").innerHTML="&nbsp;&nbsp;&nbsp;&nbsp;"+obj+"&nbsp;&nbsp;&nbsp;&nbsp;";   
    var Year=obj;
    var month=document.getElementById("SpanMonthEnd").innerHTML;
  
    var ObjDiv=document.getElementById("TimeDay2");
    changeMonthEnd(Year,month,ObjDiv);//更新日期
    
    document.getElementById('YesarOrMonthEnd').removeChild(document.getElementById('ShowDivYearsEnd'));
    document.getElementById("SpanYearsEnd").style.background=""; 
}


function onclickYear(obj)
{
    document.getElementById("SpanYears").innerHTML="&nbsp;&nbsp;&nbsp;&nbsp;"+obj+"&nbsp;&nbsp;&nbsp;&nbsp;";   
    var Year=obj;
    var month=document.getElementById("SpanMonth").innerHTML;
  
    var ObjDiv=document.getElementById("TimeDay");
    changeMonthBegin(Year,month,ObjDiv);//更新日期
    
    document.getElementById('YesarOrMonth').removeChild(document.getElementById('ShowDivYears'));
    document.getElementById("SpanYears").style.background=""; 
}

function onclickMouthEnd(obj)
{
      document.getElementById("SpanMonthEnd").innerHTML="&nbsp;&nbsp;&nbsp;&nbsp;"+obj+"&nbsp;&nbsp;&nbsp;&nbsp;";   
  var month=obj;
  var Year=document.getElementById("SpanYearsEnd").innerHTML;
  
   var ObjDiv=document.getElementById("TimeDay2");
    changeMonthEnd(Year,month,ObjDiv);//更新时间
    
    document.getElementById('YesarOrMonthEnd').removeChild(document.getElementById('showHoursDivEnd'));
    document.getElementById("SpanMonthEnd").style.background=""; 
}

function onclickMouth(obj)
{
   document.getElementById("SpanMonth").innerHTML="&nbsp;&nbsp;&nbsp;&nbsp;"+obj+"&nbsp;&nbsp;&nbsp;&nbsp;";   
  var month=obj;
  var Year=document.getElementById("SpanYears").innerHTML;
  
   var ObjDiv=document.getElementById("TimeDay");
    changeMonthBegin(Year,month,ObjDiv);//更新时间
    
    document.getElementById('YesarOrMonth').removeChild(document.getElementById('showHoursDiv'));
    document.getElementById("SpanMonth").style.background=""; 
  
}


function OnClickTimeDayBegin(objYear,objMonth,objDay)
{

      var tdDay=document.getElementById("TD_DivBegin-"+objYear+"-"+objMonth+"-"+objDay);
      OnClickNodeTopDayBegin(tdDay);
      
      var BeginTime=objYear+"-"+objMonth+"-"+objDay;
      document.getElementById("BeginTime").value=BeginTime;
}

function OnClickTimeDayEnd(objYear,objMonth,objDay)
{

      var tdDay=document.getElementById("TD_DivEnd-"+objYear+"-"+objMonth+"-"+objDay);
      OnClickNodeTopDayEnd(tdDay);
            
      var EndTime=objYear+"-"+objMonth+"-"+objDay;
      document.getElementById("EndTime").value=EndTime;
      
}

 //确定按钮点击
function onConfirm()
{
    var objbeginTime=document.getElementById("BeginTime").value;
    var objEndTime=document.getElementById("EndTime").value;
    window.parent.parentonConfirm(objbeginTime,objEndTime,"")
   
}

//清空
function onClear()
{
    var objbeginTime="";
    var objEndTime="";
    window.parent.parentonConfirm(objbeginTime,objEndTime,"")
}
//今天
function onDateToday()
{
    var now = new Date();
    var dateTodate = now.getTime();    
    var objBeginTime=changeTime(dateTodate);
    var objEndTime=changeTime(dateTodate);    
    window.parent.parentonConfirm(objBeginTime,objEndTime,"今天");
}
//昨天
function onYesterday()
{
    var now = new Date();
    var dateTodate = now.getTime();
    var todaty  =changeTime(dateTodate);     
    
    var Yesterday=todaty.split("-");
    var day=Yesterday[2]-1;
    var objBeginTime=Yesterday[0]+"-"+Yesterday[1]+"-"+day;
    var objEndTime=Yesterday[0]+"-"+Yesterday[1]+"-"+day;        
    window.parent.parentonConfirm(objBeginTime,objEndTime,"昨天");
}
//本周 
function onThisWeekDay()
{
    var now = new Date();   
    var currentWeek = now.getDay();
    if ( currentWeek == 0 )
    {
        currentWeek = 7;
    }    
    var weekday = now.getTime() - (currentWeek)*24*60*60*1000;   //星期日
    var dateTodate = now.getTime();// - (currentWeek-1)*24*60*60*1000;   //星期一 
    var objBeginTime=changeTime(weekday);
    var objEndTime=changeTime(dateTodate);   
    window.parent.parentonConfirm(objBeginTime,objEndTime,"本周");
    
}

//本月
function onThisMonth()
{
    var now = new Date();
    var dateTodate = now.getTime();
    var todaty  =changeTime(dateTodate);         
    var Yesterday=todaty.split("-");    
    var objBeginTime=Yesterday[0]+"-"+Yesterday[1]+"-1";
    var objEndTime=todaty;    
    window.parent.parentonConfirm(objBeginTime,objEndTime,"本月");
}


//获得时间函数
function changeTime(str){
   str= new Date(str).toLocaleDateString();
  var curYear = str.substring(0,str.indexOf('年'));
  var curMonth =str.substring(str.indexOf('年')+1,str.indexOf('月'));
  var curDay =str.substring(str.indexOf('月')+1,str.indexOf('日'));
   
  if (curMonth<10){
    //curMonth="0"+curMonth;
    curMonth=curMonth;
  }
  if(curDay<10){
    //curDay="0"+curDay;
    curDay=curDay;
  }
  var returnDate = curYear+"-"+curMonth+"-"+curDay; 
 return returnDate;
} 


// //获得本周的周一和周末
// function getThisWeekDate(){
// var now = new Date();
// var week=new Array();
// var currentWeek = now.getDay();
// if ( currentWeek == 0 )
//    {
//   currentWeek = 7;
//    }

// var monday = now.getTime() - (currentWeek-1)*24*60*60*1000;   //星期一
// var tuesday  = now.getTime() - (currentWeek-2)*24*60*60*1000; //星期二
// var wednesday = now.getTime() - (currentWeek-3)*24*60*60*1000; //星期三
// var thursday = now.getTime() - (currentWeek-4)*24*60*60*1000; //星期四
// var friday  = now.getTime() - (currentWeek-5)*24*60*60*1000; //星期五
// var saturday = now.getTime() - (currentWeek-6)*24*60*60*1000; //星期六
// var sunday = now.getTime() + (7-currentWeek)*24*60*60*1000;      //星期日
// 
// week=weektoday(monday,tuesday,wednesday,thursday,friday,saturday,sunday);
//  
// return week; 
// 
// }


//function weektoday(monday,tuesday,wednesday,thursday,friday,saturday,sunday){

// var days=new Array();

// var monday1 = new Date(monday).toLocaleDateString();//星期一
// var tuesday1= new Date(tuesday).toLocaleDateString(); //星期二
// var wednesday1 = new Date(wednesday).toLocaleDateString(); //星期三
// var thursday1 = new Date(thursday).toLocaleDateString(); //星期四
// var friday1= new Date(friday).toLocaleDateString(); //星期五
// var saturday1 =new Date(saturday).toLocaleDateString();//星期六
// var sunday1 = new Date(sunday).toLocaleDateString();//星期日
// 

// 
// var new_monday=changeTime(monday1);
// var new_tuesday=changeTime(tuesday1);
// var new_wednesday=changeTime(wednesday1);
// var new_thursday=changeTime(thursday1);
// var new_friday=changeTime(friday1);
// var new_saturday=changeTime(saturday1);
// var new_sunday=changeTime(sunday1);

// days[0]=new_monday;
// days[1]=new_tuesday;
// days[2]=new_wednesday;
// days[3]=new_thursday;
// days[4]=new_friday;
// days[5]=new_saturday;
// days[6]=new_sunday;
// 
// return days;

//}


 var xmlhttp = null;
        function CreateXmlHttpObject()
        {   try  
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
			return xmlhttp;
        }        
        function changeMonthBegin(hidYear,HidMonth,objDiv)
        {       
        
            if(xmlhttp == null)
                xmlhttp = CreateXmlHttpObject();       
            if(xmlhttp != null)
            {
                try
                {	
					xmlhttp.open("GET", "DateTimeDay.ashx?years="+escape(hidYear)+"&month=" + escape(HidMonth)+"&type=Begin", true); 
                    xmlhttp.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
					xmlhttp.onreadystatechange = function() 
												{ 
													if ( xmlhttp.readyState==4 ) 
													{ 													
													    var msg=xmlhttp.responseText													 
													    objDiv.innerHTML=msg;  
													    
													    //重置层的高度
													    parent.jsHeight();
												    } 
												}
					xmlhttp.send(null); 
				}catch(e3){}
            }
        }
        function changeMonthEnd(hidYear,HidMonth,objDivEnd)
        {             
            if(xmlhttp == null)
                xmlhttp = CreateXmlHttpObject();       
            if(xmlhttp != null)
            {
                try
                {	
					xmlhttp.open("GET", "DateTimeDay.ashx?years="+escape(hidYear)+"&month=" + escape(HidMonth)+"&type=End", true); 
                    xmlhttp.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
					xmlhttp.onreadystatechange = function() 
												{ 
													if ( xmlhttp.readyState==4 ) 
													{ 													
													    var msg=xmlhttp.responseText													 
													    objDivEnd.innerHTML=msg;  		
													    //重置层的高度
													    parent.jsHeight();											
												    } 
												}
					xmlhttp.send(null); 
				}catch(e3){}
            }
        }