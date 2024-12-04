var divEuqObj=null;
var HidEuqNature=null;
var txtEquNature=null;


function ShowDivEquChoice(objEquId,objDiv,objHidEuqNature,objtxtEquNature)
{     
    if(document.getElementById('showEquDiv')!=null&&document.getElementById('showEquDiv')!="undefined")
    {
        return ;
    }
    divEuqObj=objDiv
    HidEuqNature=objHidEuqNature;
    txtEquNature=objtxtEquNature;
   // var  ShowChoiceDiv=document.getElementById('DivEquNature')   
    var bgObj=document.createElement("div");    
    bgObj.setAttribute('id','showEquDiv'); 
    bgObj.style.position="absolute"; 
    bgObj.style.top="22px"; 
    bgObj.style.background="#ffffff"; 
    //bgObj.style.filter="progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=75,finishOpacity=25"; 
    bgObj.style.zIndex="10001";
    bgObj.style.left="0px"; 
    bgObj.className="TanChuDiv";
    bgObj.style.width="10px"; 
    bgObj.style.height= "10px";   
   divEuqObj.appendChild(bgObj);   
   var html="<iframe id=\"ShowDivIframeEqu\" src=\"../Controls/EquNature/selectEquNature.aspx?EquId="+objEquId+"\" frameborder=\"0\" width=\"100%\" height=\"100%\" style=\"padding-top:0px;padding-left:0px;\" onload=\"SetShowDivEquHeight(this)\" onblur=\"showDivEquClose()\"  scrolling=\"no\"></iframe>";
   
   document.getElementById('showEquDiv').innerHTML=html;
   divEuqObj.style.zIndex="999";

}

//获得div的高度
function SetShowDivEquHeight(cwin)
{
    cwin.focus();
	var heightdiv=0,widthdiv=0;
	var IE=getOs();
	if(IE=="Firefox"||IE=="Safari"){
	    if(cwin.contentDocument.body!=null)
	    {
	       heightdiv=cwin.contentDocument.body.scrollHeight+29;
	       widthdiv=cwin.contentDocument.body.scrollWidth+2; 
	    }
	 		
	 }else{	    	    
	    if(cwin.Document.body.all("checkEquNature")!=null)
	    {
	        heightdiv=cwin.Document.body.all("checkEquNature").offsetHeight+30;
	        widthdiv=cwin.Document.body.scrollWidth+2;
	    }
	 }

	cwin.style.height=(heightdiv)+"px";	
	document.getElementById('showEquDiv').style.height=heightdiv+"px";
    document.getElementById('showEquDiv').style.width=widthdiv+"px";	
}


//关闭div
function showDivEquClose()
{
    var ShowChoiceDiva=document.getElementById("showEquDiv");
	 if(ShowChoiceDiva!=null&&ShowChoiceDiva!="undefined"){		  
		  divEuqObj.removeChild(ShowChoiceDiva);
	 }
	 divEuqObj.style.zIndex="0";
}
//点击确认
function onClickValue(value,Text)
{
    //alert(value);
    //alert(Text);
    HidEuqNature.value=value;
    txtEquNature.value=Text;        
    showDivEquClose();
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