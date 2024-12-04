var strOldColor = "";
var strHasSelected = "false";
var lngSelectID = 0;

function onMouseOver()
{
	var e = event.srcElement;
	//alert(document.all.txt1.innerHTML);
	var str = new String();
	str = e.innerHTML;
	
	
	
	if(eval("document.all.Msg" + e.id.replace(/\T/,"")) !=null)
		str = str + " " + eval("document.all.Msg" + e.id.replace(/\T/,"")).value;
	
	window.status = str;
	//if(eval("l"e.name.replace(/\T/,"")
}

function onMouseOverNew()
{
     
	var e = event.srcElement;
	//alert(document.all.txt1.innerHTML);
	var str = new String();
	str = e.innerHTML;
	
	if(eval("document.all.Msg" + e.id.replace(/\L/,"")) !=null)
		str = str + " " + eval("document.all.Msg" + e.id.replace(/\L/,"")).value;
	
	window.status = str;
	//if(eval("l"e.name.replace(/\T/,"")
}

function onMouseOut()
{
	//alert(document.all.txt1.innerHTML);
	window.status = "";
}

function onMouseOverSelect()
{
	var e = event.srcElement;
	//alert(eval("parent.flow_Chart.document.all." + e.id).innerText);
	//alert(document.all.txt1.innerHTML);
	var strNodeList = document.all.CanJumpNodeList.value;
	var strID = e.id.replace(/\T/,"");
	if(strNodeList.indexOf(strID + ",") != -1 && strHasSelected == "false" )
	{
		DoSelectNode(e);
		strNodeName = eval("parent.flow_Chart.document.all." + e.id).innerText;
		str1 = document.all.MsgTitle1.value;
		str2 = document.all.MsgTitle2.value;
		strMsg = str1 + strNodeName + str2;
		
		window.status = strMsg;
		return true;
	}
	
	
	
}

function onMouseOutSelect()
{
	var e = event.srcElement;
	
	if(strHasSelected == "false")
	{
		UnDoSelectNode(e);
		window.status = "";
	}
	
}


function onClick()
{
	var e = event.srcElement;
	var strNodeList = document.all.CanJumpNodeList.value;
	var strID = e.id.replace(/\T/,"");
	if(strNodeList.indexOf(strID + ",") != -1  && strHasSelected == "false" )
	{
		//strNodeName = eval("parent.flow_Chart.document.all." + e.id).innerText;
		strNodeName = e.innerText;
		str1 = document.all.MsgTitle1.value;
		str2 = document.all.MsgTitle2.value;
		strMsg = str1 + strNodeName + str2;
		//alert(window.confirm(strMsg));
		//alert(window.confirm("确定跳转到　" + strNodeName + "　环节吗？"));
	    blnConfirm = window.confirm(strMsg);
	    if(blnConfirm == true)
	    {

			DoSelectNode(e);
			strHasSelected = "true";
			window.returnValue = strID;
			window.close();
		}
		else
		{
		    window.close();
		}
	}
	
	
	//alert(document.all.CanJumpNodeList.value);
	
}








