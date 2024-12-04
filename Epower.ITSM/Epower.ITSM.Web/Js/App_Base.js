//<script language=javascript>
//应用开发的基础脚本函数：苏康胜 

//
function NumberInput(sTag)
{
 if(sTag=="") 
   {
//       if((event.keyCode<48)||(event.keyCode>57)) 
//          event.returnValue=false;
     if((event.keyCode<48)||(event.keyCode>57)) 
        if(event.keyCode!=46 && event.keyCode !=8) 
            if((event.keyCode>95 && event.keyCode<106) || event.keyCode==9 || (event.keyCode>32 && event.keyCode<47))
            {event.returnValue=true;}
            else {event.returnValue=false;}
   }
 else
   {
     if((event.keyCode<48)||(event.keyCode>57 )) 
       if(event.keyCode!=46 || event.keyCode<0) 
         if((event.keyCode>95 && event.keyCode<106) || event.keyCode==13 || event.keyCode==9 || event.keyCode==110 || event.keyCode==190 || event.keyCode==8 || (event.keyCode>32 && event.keyCode<47))
         {
            event.returnValue=true;
         }
         else
            event.returnValue=false;
   }      
}

function CheckIsnum(obj,strDisplay)
{
    var svalue = obj.value;
    if (isNaN(svalue))
    {
        alert(strDisplay);
        obj.focus(); 
        obj.select(obj.value.length);
    }
}


function BASEtrim(str){
	  str=str.toString()
	  lIdx=0;rIdx=str.length;
	  if (BASEtrim.arguments.length==2)
	    act=BASEtrim.arguments[1].toLowerCase()
	  else
	    act="all"
      for(var i=0;i<str.length;i++){
	  	thelStr=str.substring(lIdx,lIdx+1)
		therStr=str.substring(rIdx,rIdx-1)
        if ((act=="all" || act=="left") && thelStr==" "){
			lIdx++
        }
        if ((act=="all" || act=="right") && therStr==" "){
			rIdx--
        }
      }
	  str=str.slice(lIdx,rIdx)
      return str
}
function BASEisAllEnter(str){
	temp_str=BASEtrim(str)
	if (temp_str=="")
		return false
	for (i=0;i<temp_str.length;i=i+2){
		if(temp_str.charCodeAt(i)!="13" && temp_str.charCodeAt(i+1)!="10"){
			return false
		}
	}
	return true
}
function BASEdelSpace(str){  
      len=str.length
      for(var i=0;i<len;i++){
	    sep_num=0
        if (str.substring(i,i+1)==" ")
			sep_num=1
		if (str.substring(i,i+6)=="&nbsp;")
			sep_num=6
		if (sep_num>0){
	      str=str.substring(0,i)+str.substring(i+sep_num)
		  i-=sep_num
	      len-=sep_num
        }
      }
      return str
}
function BASEisNotNum(theNum){
	if (BASEtrim(theNum)=="")
		return true
	for(var i=0;i<theNum.length;i++){
	    oneNum=theNum.substring(i,i+1)

        if (oneNum<"0" || oneNum>"9")
          return true
    }
	return false
}

function BASEisNotInt(theInt){

	theInt=BASEtrim(theInt)
	if ((theInt.length>1 && theInt.substring(0,1)=="0") || BASEisNotNum(theInt)){
		return true
	}
	return false
}

function BASEisNotFloat(theFloat){
	len=theFloat.length
	dotNum=0
	if (len==0)
		return true
	for(var i=0;i<len;i++){
	    oneNum=theFloat.substring(i,i+1)
		if (oneNum==".")
			dotNum++
        if ( ((oneNum<"0" || oneNum>"9") && oneNum!=".") || dotNum>1)
          return true
    }
	if (len>1 && theFloat.substring(0,1)=="0"){
		if (theFloat.substring(1,2)!=".")
			return true
	}
	return false
}
function BASEisNotDate(theDate,divi){
	var theYear;
	var theMonth;
	var theDay;
	theDate=theDate.split(divi);	
	if(theDate.length<3){
		return true;
	}
	theYear=parseInt(theDate[0]);
	theMonth=parseInt(theDate[1]);
	theDay=parseInt(theDate[2]);
	if(theYear<1900||theYear>9999){
		return  true;
	}
	if(theMonth<1||theMonth>12){
		return  true;
	}
	if(theDay<1||theDay>31){
		return  true;
	}			
	return false;
}
function BASEisNotDateTime(theDateTime,divi){
	var theHour;
	var theMinute;
	var theSecond;
	var theDate;
	theDateTime=BASEtrim(theDateTime);
	theDate=theDateTime.split(' ')[0];
	if(BASEisNotDate(theDate,divi)){
		return true;			
	}
	if(theDate.length==theDateTime.length){
		return false;
	}
	theDate=theDateTime.substring(theDate.length+1,theDateTime.length);
	if(theDate==''){
		return false;
	}
	theDate=theDate.split(':');	
	theHour=parseInt(theDate[0]);
	theMinute=parseInt(theDate[1]);
	theSecond=parseInt(theDate[2]);
	if(theHour<0||theHour>23){
		return  true;
	}
	if(theMinute<0||theMinute>59){
		return  true;
	}
	if(theSecond<0||theSecond>59){
		return  true;
	}			
	return false;
}
function BASEalert(theText,notice){  
	  alert(notice)
	  theText.focus()
	  if (theText.type.indexOf("select")==-1)
		  theText.select()
	  return false
}

function BASEreal_len(theText){ 
	  var real_len=0;
	  text_val=theText.value;
	  text_len=theText.value.length;
	  for(i=0;i<text_len;i++){
	    if (text_val.charCodeAt(i)>127){
		  real_len=real_len+2;
		}
	    else{
		  real_len++;
		}
  	  }
	  return(real_len);
}

function BASEset_selected_item(theSelect,idx){ 
	  theSelect.selectedIndex=idx;
	  theSelect.options[idx].defaultSelected=true
	  return true
}

function BASEset_option(theSelect,opt_str){ 
      with(theSelect)
        for(var k=0;k<length;k++)
          if (options[k].value==opt_str){
	        selectedIndex=k
	        break
	      }
      return true
}

function BASEinit_serial_num(theSelect,max_num){ 
	  for(var i=0;i<max_num;i++)
	    theSelect[i]=new Option(i+1,i+1)
	  BASEset_selected_item(theSelect,max_num-1)
	  return true
}

function BASEasign_select_to_text(theSelect,destText,theNone,default_val){
	  with (theSelect){
	    the_val=options[selectedIndex].value
	    the_text=options[selectedIndex].text
	  }
	  if (the_val!=theNone)
	    destText.value=the_text
	  else
	    destText.value=default_val
      return true
}

function BASEreset_rec(theForm){  
      for (var i=1;i<(e=theForm.elements).length;i++){
	    e=e[i]
        if (e.type=="text")
	      e.value=""
		if (e.type=="checkbox")
		  e.checked=false
	    if (e.type=="select-one" || e.type=="select-multiple")
	      e.selectedIndex=0
      }
	  return true
}

function BASEcheck_select(theSelect) {
	var cc=0
	with(theSelect){
		if(options[0].selected){
			for(i=1;i<length;i++){
				if(options[i].selected) cc++
			}
			if(cc>0) options[0].selected=false
		}
		else{
			if (selectedIndex==-1)
				options[0].selected=true
		}
	}
	return true
}
	
function BASEcreateSelect(theSelect,select_str){
	select_str+=","
	theSelect.length=0
	fromIndex=0;
	nextIndex=0;
	count=0;
	i=0
	while ((nextIndex=select_str.indexOf(",",fromIndex))!=-1){
		result_str=select_str.substring(fromIndex,nextIndex)
		count++;
		if (count%2==1){
		  text_str=result_str
		}
		if (count%2==0){
		  theSelect.length=theSelect.length+1
		  theSelect.options[i].text=text_str
		  theSelect.options[i].value=result_str
		  i++
		}
	    fromIndex=nextIndex+1
	  }
	  //theSelect.selectedIndex=0;
	  //theSelect.options[0].defaultSelected=true
  return true
}

function GetRandom() {
    return Math.floor(Math.random() * 1000 + 1);
}

