//弹出框框
//防止操作未完成
var titleRealName="查看信息";
function fz(obj,BoolIsParentIs){ 
 if(obj==1){
   var sWidth,sHeight; 
   
   var dde = document.documentElement;
   	
    if (window.innerWidth)
    {        
        sHeight= window.innerHeight;        
    }
    else
    {        
        sHeight= dde.offsetHeight;        
    }
    
   sWidth=document.body.offsetWidth-2; 
   sHeight=sHeight-2;
   var bgObj=document.createElement("div"); 
   bgObj.setAttribute('id','fzShowMsgDiv'); 
   bgObj.style.position="absolute"; 
   bgObj.style.top="0"; 
   bgObj.style.background="#777"; 
   bgObj.style.filter="progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=75,finishOpacity=25"; 
   bgObj.style.opacity="0.6"; 
   bgObj.style.left="0"; 
   bgObj.style.width=sWidth + "px"; 
   bgObj.style.height=sHeight + "px"; 
   bgObj.style.zIndex = "10000"; 
   document.body.appendChild(bgObj);
 }else{
	  if(BoolIsParentIs==false){
		 var bgDiv=document.getElementById("fzShowMsgDiv");	 
		 document.body.removeChild(bgDiv);
	  }else{
		 for(var i=0;i<document.frames.length;i++){
		     var TempDocument=document.frames[i].document.getElementById("fzShowMsgDiv");
			 if(TempDocument!=null){
				    document.frames[i].document.body.removeChild(document.frames[i].document.getElementById("fzShowMsgDiv"));
					break;
				 }
		  }		 
	  }
 }
}
function setTitleRealName(titleName)
{
    titleRealName=titleName;
    //alert("titleRealName:"+parent.document.getElementById("divtitle").innerHTML);
    parent.document.getElementById("divtitle").innerHTML=titleRealName;
}
function ShowShangJiChoice(title,pathhtml,TopValue,BoolIsParentIs){


}
function FishChoice(BoolIsParentIs){	
    
    if(BoolIsParentIs==false){
		 var ShowChoiceDiva=document.getElementById("ShowDilagModeDiv");;
		 if(ShowChoiceDiva!=null&&ShowChoiceDiva!="undefined"){
		  document.body.removeChild(ShowChoiceDiva);
		  fz(2,BoolIsParentIs);
		 }
	}else{		 
		 var ShowChoiceDiva=parent.document.getElementById("ShowDilagModeDiv");;
		 if(ShowChoiceDiva!=null&&ShowChoiceDiva!="undefined"){
		 parent.document.body.removeChild(ShowChoiceDiva);
		 fz(2,BoolIsParentIs);
		 }
         
		}
}
function SetShowDivHeight(cwin,BoolIsParentIs)
{
	//改变图片样式
	if(BoolIsParentIs==false){
	//document.getElementById('ShowDivTitleImg').src="../images/07.gif";
	var heightdiv=0,widthdiv=0;
	var IE=getOs();
	if(IE=="Firefox"){
	 	heightdiv=cwin.contentDocument.body.scrollHeight+29;
	    widthdiv=cwin.contentDocument.body.scrollWidth+2;	
	}else{
	   	heightdiv=cwin.Document.body.scrollHeight+29;
	    widthdiv=cwin.Document.body.scrollWidth+2;
	}	
	cwin.style.height=(heightdiv-27)+"px";	
	
	document.getElementById('ShowDilagModeDiv').style.height=heightdiv+"px";
    document.getElementById('ShowDilagModeDiv').style.width=widthdiv+"px";
	 
	var v_left=(document.body.clientWidth-widthdiv)/2+document.body.scrollLeft;
	document.getElementById('ShowDilagModeDiv').style.left=v_left+ "px";
	}else{
	    //parent.document.getElementById('ShowDivTitleImg').src="../images/07.gif";
	    var heightdiv=0,widthdiv=0;
		var IE=getOs();
		if(IE=="Firefox"){
			heightdiv=cwin.contentDocument.body.scrollHeight+29;
			widthdiv=cwin.contentDocument.body.scrollWidth+2;	
		}else{
			heightdiv=cwin.Document.body.scrollHeight+29;
			widthdiv=cwin.Document.body.scrollWidth+2;
		}	
		cwin.style.height=(heightdiv-27)+"px";	
		
		parent.document.getElementById('ShowDilagModeDiv').style.height=heightdiv+"px";
		parent.document.getElementById('ShowDilagModeDiv').style.width=widthdiv+"px";
		 
		var v_left=(parent.document.body.clientWidth-widthdiv)/2+parent.document.body.scrollLeft;
		parent.document.getElementById('ShowDilagModeDiv').style.left=v_left+ "px";
	}
	 
	
}
function showdivclose(obj,objthis){
  if(obj==1){
	  objthis.src="../Images/24-A.jpg";
  }else{
	  objthis.src="../Images/24.jpg";
  }
}



function ShowDivTime(TitleObj,SrcPath,objDiv) {
    
   ShowDivTimeChoice(TitleObj,SrcPath,objDiv)
}
var showTimeDivChoice=null;
var HidBeginTime=null;
var HidBeginEnd=null;
var TxtTime=null;
function ShowDivTimeChoice(pathhtml,ShowChoiceDiv,BeginTime,EndTime,TimeValue)
{
    

    showTimeDivChoice=ShowChoiceDiv;
    HidBeginTime=BeginTime;
    HidBeginEnd=EndTime;
    TxtTime=TimeValue;
    if(document.getElementById('showTimeDiv')!=null&&document.getElementById('showTimeDiv')!="undefined")
    {
        return ;
    }
   // var  ShowChoiceDiv=objDiv;  
    var bgObj=document.createElement("div");    
    bgObj.setAttribute('id','showTimeDiv'); 
    bgObj.style.position="absolute"; 
    bgObj.style.top="22px"; 
    bgObj.style.background="#ffffff"; 
   // bgObj.style.filter="progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=75,finishOpacity=25"; 
    bgObj.style.zIndex="10001";
    bgObj.style.left="0px"; 
    bgObj.className="TanChuDiv";
    bgObj.onblur="showTimeDivClose()";
    bgObj.style.width="200px"; 
    bgObj.style.height= "100px";
   
   ShowChoiceDiv.appendChild(bgObj);   
   var html="<iframe id=\"ShowDivIframe\" src=\"../Controls/TimeSelect/SelectTime.aspx\" frameborder=\"0\" width=\"100%\" height=\"100%\" style=\"padding-top:0px;padding-left:0px;z-index:10;\" onload=\"SetShowTimeDivHeight(this)\" onblur=\"showTimeDivClose()\"  scrolling=\"no\"></iframe>";
   
   document.getElementById('showTimeDiv').innerHTML=html;
   
   showTimeDivChoice.style.zIndex="999";

}

    //获取已经选中的值 给控件赋初值
    function parenReturnConfirm()
    { 
        if(HidBeginTime.value!=""&&HidBeginEnd.value!="")   
            return HidBeginTime.value+"|"+HidBeginEnd.value;
        else
            return "";
    }

    function parentonConfirm(objbeginTime,objEndTime,showtime)
    {   
        if(objbeginTime==""&&objEndTime=="")
        {
            TxtTime.value="";     
            HidBeginTime.value=objbeginTime;
            HidBeginEnd.value=objEndTime;       
        }
        else if(objbeginTime==""&&objEndTime=="")
        {
            HidBeginTime.value=objbeginTime;
            HidBeginEnd.value=objEndTime;       
            TxtTime.value=objbeginTime+"~"+objEndTime;    
        }
        else
        {  
            var dtArrBegin = objbeginTime.split("-");
            var dtBegin = new Date(dtArrBegin[0], dtArrBegin[1], dtArrBegin[2]);
            
            var dtArrEnd = objEndTime.split("-");
            var dtEnd = new Date(dtArrEnd[0], dtArrEnd[1], dtArrEnd[2]);
            if(dtBegin>dtEnd)
            {
                HidBeginTime.value=objEndTime;
                HidBeginEnd.value=objbeginTime;                   
                TxtTime.value=objEndTime+"~"+objbeginTime;    
            }else
            {
                HidBeginTime.value=objbeginTime;
                HidBeginEnd.value=objEndTime;   
                TxtTime.value=objbeginTime+"~"+objEndTime;    
            }
            if(showtime!="")
            {
                TxtTime.value=showtime;
            }
        }
        TxtTime.onchange();
        showTimeDivClose();
    }
    
    //重新获得层的高度
    function jsHeight()
    {
        var obj =document.getElementById("ShowDivIframe");
        SetDivHeight(obj);
    }


function SetShowTimeDivHeight(cwin)
{
	//改变图片样式
//onload=\"Javascript:SetShowTimeDivHeight(this)\" 


    cwin.focus();
	var heightdiv=0,widthdiv=0;
	var IE=getOs();
	if(IE=="Firefox"){
	 	heightdiv=cwin.contentDocument.body.scrollHeight+29;
	    widthdiv=cwin.contentDocument.body.scrollWidth+2;	
	}else{
	    
	   	heightdiv=cwin.Document.body.scrollHeight+2;
	   	if(heightdiv<150)
	   	{
	   	    heightdiv=226;
	   	}
	    widthdiv=cwin.Document.body.scrollWidth+2;	   
	 }	
	//cwin.style.height=(heightdiv-27)+"px";	  
	document.getElementById('showTimeDiv').style.height=heightdiv+"px";
    document.getElementById('showTimeDiv').style.width=widthdiv+"px";	
}


function SetDivHeight(cwin)
{
	//改变图片样式
//onload=\"Javascript:SetShowTimeDivHeight(this)\" 

	var heightdiv=0,widthdiv=0;
	var IE=getOs();
	if(IE=="Firefox"){
	 	heightdiv=cwin.contentDocument.body.scrollHeight+2;
	  
	}else{
	   	heightdiv=cwin.Document.body.scrollHeight+2;
	   	if(heightdiv<150)
	   	{
	   	    heightdiv=226;
	   	}	
	 }
	//cwin.style.height=(heightdiv-27)+"px";	
	document.getElementById('showTimeDiv').style.height=heightdiv+"px";
}


function showTimeDivClose()
{
     var ShowChoiceDiva=document.getElementById("showTimeDiv");     
	 if(ShowChoiceDiva!=null&&ShowChoiceDiva!="undefined"){		  
		  showTimeDivChoice.removeChild(ShowChoiceDiva);		  
	 }
	 showTimeDivChoice.style.zIndex="0";
}


function ShowReturnValue(obj,txtID,valueID)
{   
    try{  
    if(document.getElementById(txtID)!=null&&document.getElementById(valueID)!=null)
    {
    document.getElementById(txtID).value= obj.split("-")[0].Trim();
    document.getElementById(valueID).value= obj.split("-")[1].Trim(); 
    }   
      FishChoice(false);   
    addProductInfo(); 
    }catch(e)
    { 
    }    
}
String.prototype.Trim = function()
{
    return this.replace(/(^\\s*)|(\\s*$)/g, "");
}
String.prototype.LTrim = function()
{
    return this.replace(/(^\\s*)/g, "");
}
String.prototype.RTrim = function()
{
    return this.replace(/(\\s*$)/g, "");
}

function moveStart (event, _sId){ 
var IE=getOs();
	if(IE!="Firefox"){
  var oObj = document.getElementById(_sId);
  oObj.style.filter="progid:DXImageTransform.Microsoft.Alpha(opacity=65)"; 
  document.getElementById("ShowDilagModeDivChild").style.display="none";
  oObj.onmousemove = mousemove;
  oObj.onmouseup = mouseup;
  oObj.setCapture ? oObj.setCapture() : function(){};
  oEvent = window.event ? window.event : event;
  var dragData = {x : oEvent.clientX, y : oEvent.clientY};
  var backData = {x : parseInt(oObj.style.top), y : parseInt(oObj.style.left)};
  function mousemove(){
   var oEvent = window.event ? window.event : event;
   var iLeft = oEvent.clientX - dragData["x"] + parseInt(oObj.style.left);
   var iTop = oEvent.clientY - dragData["y"] + parseInt(oObj.style.top);
   oObj.style.left = iLeft;
   oObj.style.top = iTop;
   dragData = {x: oEvent.clientX, y: oEvent.clientY};   
  }
  function mouseup(){
   oObj.style.filter=""; 
    document.getElementById("ShowDilagModeDivChild").style.display="";
   var oEvent = window.event ? window.event : event;
   oObj.onmousemove = null;
   oObj.onmouseup = null;
   if(oEvent.clientX < 1 || oEvent.clientY < 1 || oEvent.clientX > document.body.clientWidth || oEvent.clientY > document.body.clientHeight){
    oObj.style.left = backData.y;
    oObj.style.top = backData.x;
   }
   oObj.releaseCapture ? oObj.releaseCapture() : function(){};
  }
 } 
 }

function getOs() 
{ 
   var OsObject = ""; 
   if(navigator.userAgent.indexOf("MSIE")>0) { 
        return "MSIE"; 
   } 
   if(isFirefox=navigator.userAgent.indexOf("Firefox")>0){ 
        return "Firefox"; 
   } 
   if(isSafari=navigator.userAgent.indexOf("Safari")>0) { 
        return "Safari"; 
   }  
   if(isCamino=navigator.userAgent.indexOf("Camino")>0){ 
        return "Camino"; 
   } 
   if(isMozilla=navigator.userAgent.indexOf("Gecko/")>0){ 
        return "Gecko"; 
   } 
   
} 


function GetFunctionFor(functionName){
        var documentOrd=document;       
        for(var i=0;i<parent.document.frames.length;i++){
		     var TempObj=parent.document.frames[i].document.getElementById("fzShowMsgDiv");
			 if(TempObj!=null){
				    parent.document.frames[i].window.eval(functionName);
					break;
				 }
		  }
		  		
}

function ShowDialgDiv2(TitleObj,SrcPath,TopValue,BoolIsParent){
  var BoolIsParentIs=false;
  if(BoolIsParent!=null){
  BoolIsParentIs=BoolIsParent;
  }
  //先弹出一个层让屏幕变黑
  fz(1,BoolIsParentIs);   
  //再弹出一个层 让用户可以选择商机  
  ShowShangJiChoice2(TitleObj,SrcPath,TopValue,BoolIsParentIs);
}
function ShowShangJiChoice2(title,pathhtml,TopValue,BoolIsParentIs){
    titleRealName=title;
   var sWidth,sHeight; 
   sWidth=400; 
   sHeight=100;     
   var v_left=0;
   if(BoolIsParentIs==false){
      v_left=(document.body.clientWidth-sWidth)/2+document.body.scrollLeft;
   }else{
	  v_left=(parent.document.body.clientWidth-sWidth)/2+parent.document.body.scrollLeft;     
	}
   var v_top=TopValue+"px";  
   var ShowChoiceDiv=null;
   if(BoolIsParentIs==false){
    ShowChoiceDiv=document.createElement("div"); 
   }else{
	    ShowChoiceDiv=parent.document.createElement("div"); 
	   }
   ShowChoiceDiv.setAttribute('id','ShowDilagModeDiv'); 
   ShowChoiceDiv.style.position="absolute"; 
   ShowChoiceDiv.style.top=v_top;
   ShowChoiceDiv.style.background="#ffffff";    
   //ShowChoiceDiv.style.opacity="0.6"; 
   ShowChoiceDiv.style.left=v_left+ "px"; 
   ShowChoiceDiv.style.width=sWidth + "px"; 
   ShowChoiceDiv.style.height=sHeight + "px"; 
   ShowChoiceDiv.style.zIndex = "10001";
   ShowChoiceDiv.className="TanChuDiv_agn";
  
   if(BoolIsParentIs==false){
        document.body.appendChild(ShowChoiceDiv); 
   }else{
	   parent.document.body.appendChild(ShowChoiceDiv); 
	   }   	  
	   
       var html="<div class=\"TanChuDiv3_agn\"><div class=\"TanChuDivTitle_agn\" onmousedown=\"moveStart(event,'ShowDilagModeDiv');\" style=\"cursor:move;\"><div class=\"TanChuDivTitleLeft_agn\"  titile=\"移动窗口\" ><div class=\"TanChuImgClass\" title=\""+titleRealName+"\"></div><div class=\"TanChuImgTxt_agn\"><span id=\"divtitle\"><b>"+titleRealName+"</b></span></div></div><div class=\"TanChuDivTitleRight_agn\"><img src=\"../NowShMoney/images/close-01.gif\" alt=\"关闭\" onmouseout=\"showdiv2close(2,this)\" onmouseover=\"showdiv2close(1,this)\" onclick=\"FishChoice("+BoolIsParentIs+")\"/></div></div>";
      html+="<div id=\"ShowDilagModeDivChild\" class=\"TanChuDivIfrmae_agn\"><iframe id=\"showdiv2Iframe\" src=\""+pathhtml+"\" frameborder=\"0\" width=\"100%\" height=\"100%\" onload=\"Javascript:Setshowdiv2Height(this,"+BoolIsParentIs+")\" scrolling=\"no\"></iframe></div></div>";
	if(BoolIsParentIs==false){
      document.getElementById('ShowDilagModeDiv').innerHTML=html;
	}else{
		parent.document.getElementById('ShowDilagModeDiv').innerHTML=html;
		}
}

function showdiv2close(obj,objthis){
  if(obj==1){
	  objthis.src="../NowShMoney/images/close-02.gif";
  }else{
	  objthis.src="../NowShMoney/images/close-01.gif";
  }
}
function Setshowdiv2Height(cwin,BoolIsParentIs)
{
	//改变图片样式
	if(BoolIsParentIs==false){
	//document.getElementById('showdiv2TitleImg').src="../images/07.gif";
	var heightdiv=0,widthdiv=0;
	var IE=getOs();
	if(IE=="Firefox"){
	 	heightdiv=cwin.contentDocument.body.scrollHeight+40;
	    widthdiv=cwin.contentDocument.body.scrollWidth+2;	
	}else{
	   	heightdiv=cwin.Document.body.scrollHeight+40;
	    widthdiv=cwin.Document.body.scrollWidth+2;
	}	
	
	cwin.style.height=(heightdiv-40)+"px";	
	
	document.getElementById('ShowDilagModeDiv').style.height=heightdiv+"px";
    document.getElementById('ShowDilagModeDiv').style.width=widthdiv+"px";
	 
	var v_left=(document.body.clientWidth-widthdiv)/2+document.body.scrollLeft;
	document.getElementById('ShowDilagModeDiv').style.left=v_left+ "px";
	}else{
	    //parent.document.getElementById('showdiv2TitleImg').src="../images/07.gif";
	    var heightdiv=0,widthdiv=0;
		var IE=getOs();
		if(IE=="Firefox"){
			heightdiv=cwin.contentDocument.body.scrollHeight+40;
			widthdiv=cwin.contentDocument.body.scrollWidth+2;	
		}else{
			heightdiv=cwin.Document.body.scrollHeight+40;
			widthdiv=cwin.Document.body.scrollWidth+2;
		}	
		
		cwin.style.height=(heightdiv-40)+"px";	
		
		parent.document.getElementById('ShowDilagModeDiv').style.height=heightdiv+"px";
		parent.document.getElementById('ShowDilagModeDiv').style.width=widthdiv+"px";
		 
		var v_left=(parent.document.body.clientWidth-widthdiv)/2+parent.document.body.scrollLeft;
		parent.document.getElementById('ShowDilagModeDiv').style.left=v_left+ "px";
	}
	
}