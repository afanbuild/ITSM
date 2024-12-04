
SEP_PADDING = 5
HANDLE_PADDING = 7

var yToolbars =	new Array();

var YInitialized = false;

function document.onreadystatechange()
{
  if (YInitialized) return;
  YInitialized = true;

  var i, s, curr;

  for (i=0; i<document.body.all.length;	i++)
  {
    curr=document.body.all[i];
    if (curr.className == "yToolbar")
    {
      InitTB(curr);
      yToolbars[yToolbars.length] = curr;
    }
  }

  DoLayout();
  window.onresize = DoLayout;

  Composition.document.open()
  Composition.document.write("<head><style type=\"text/css\">body {font-size: 10.8pt}</style><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><BODY bgcolor=\"#FFFFFF\" MONOSPACE></body>");
  Composition.document.close()
  Composition.document.designMode="On"
}

function InitBtn(btn)
{
  btn.onmouseover = BtnMouseOver;
  btn.onmouseout = BtnMouseOut;
  btn.onmousedown = BtnMouseDown;
  btn.onmouseup	= BtnMouseUp;
  btn.ondragstart = YCancelEvent;
  btn.onselectstart = YCancelEvent;
  btn.onselect = YCancelEvent;
  btn.YUSERONCLICK = btn.onclick;
  btn.onclick =	YCancelEvent;
  btn.YINITIALIZED = true;
  return true;
}

function InitTB(y)
{
  y.TBWidth = 0;

  if (!	PopulateTB(y)) return false;

  y.style.posWidth = y.TBWidth;

  return true;
}


function YCancelEvent()
{
  event.returnValue=false;
  event.cancelBubble=true;
  return false;
}

function BtnMouseOver()
{
  if (event.srcElement.tagName != "IMG") return	false;
  var image = event.srcElement;
  var element =	image.parentElement;

  if (image.className == "Ico")	element.className = "BtnMouseOverUp";
  else if (image.className == "IcoDown") element.className = "BtnMouseOverDown";

  event.cancelBubble = true;
}

function BtnMouseOut()
{
  if (event.srcElement.tagName != "IMG") {
    event.cancelBubble = true;
    return false;
  }

  var image = event.srcElement;
  var element =	image.parentElement;
  yRaisedElement = null;

  element.className = "Btn";
  image.className = "Ico";

  event.cancelBubble = true;
}

function BtnMouseDown()
{
  if (event.srcElement.tagName != "IMG") {
    event.cancelBubble = true;
    event.returnValue=false;
    return false;
  }

  var image = event.srcElement;
  var element =	image.parentElement;

  element.className = "BtnMouseOverDown";
  image.className = "IcoDown";

  event.cancelBubble = true;
  event.returnValue=false;
  return false;
}

function BtnMouseUp()
{
  if (event.srcElement.tagName != "IMG") {
    event.cancelBubble = true;
    return false;
  }

  var image = event.srcElement;
  var element =	image.parentElement;

  if (element.YUSERONCLICK) eval(element.YUSERONCLICK +	"anonymous()");

  element.className = "BtnMouseOverUp";
  image.className = "Ico";

  event.cancelBubble = true;
  return false;
}

function PopulateTB(y)
{
  var i, elements, element;

  elements = y.children;
  for (i=0; i<elements.length; i++) {
    element = elements[i];
    if (element.tagName	== "SCRIPT" || element.tagName == "!") continue;

    switch (element.className) {
    case "Btn":
      if (element.YINITIALIZED == null)	{
		if (! InitBtn(element))
			return false;
      }

      element.style.posLeft = y.TBWidth;
      y.TBWidth	+= element.offsetWidth + 1;
      break;

    case "TBGen":
      element.style.posLeft = y.TBWidth;
      y.TBWidth	+= element.offsetWidth + 1;
      break;

    case "TBSep":
      element.style.posLeft = y.TBWidth	+ 2;
      y.TBWidth	+= SEP_PADDING;
      break;

    case "TBHandle":
      element.style.posLeft = 2;
      y.TBWidth	+= element.offsetWidth + HANDLE_PADDING;
      break;

    default:
      return false;
    }
  }

  y.TBWidth += 1;
  return true;
}

function DebugObject(obj)
{
  var msg = "";
  for (var i in	TB) {
    ans=prompt(i+"="+TB[i]+"\n");
    if (! ans) break;
  }
}

function LayoutTBs()
{
  NumTBs = yToolbars.length;

  if (NumTBs ==	0) return;

  var i;
  var ScrWid = (document.body.offsetWidth) - 6;
  var TotalLen = ScrWid;
  for (i = 0 ; i < NumTBs ; i++) {
    TB = yToolbars[i];
    if (TB.TBWidth > TotalLen) TotalLen	= TB.TBWidth;
  }

  var PrevTB;
  var LastStart	= 0;
  var RelTop = 0;
  var LastWid, CurrWid;

  var TB = yToolbars[0];
  TB.style.posTop = 0;
  TB.style.posLeft = 0;

  var Start = TB.TBWidth;
  for (i = 1 ; i < yToolbars.length ; i++) {
    PrevTB = TB;
    TB = yToolbars[i];
    CurrWid = TB.TBWidth;

    if ((Start + CurrWid) > ScrWid) {
      Start = 0;
      LastWid =	TotalLen - LastStart;
    }
    else {
      LastWid =	PrevTB.TBWidth;
      RelTop -=	TB.offsetHeight;
    }

    TB.style.posTop = RelTop;
    TB.style.posLeft = Start;
    PrevTB.style.width = LastWid;

    LastStart =	Start;
    Start += CurrWid;
  }

  TB.style.width = TotalLen - LastStart;

  i--;
  TB = yToolbars[i];
  var TBInd = TB.sourceIndex;
  var A	= TB.document.all;
  var item;
  for (i in A) {
    item = A.item(i);
    if (! item)	continue;
    if (! item.style) continue;
    if (item.sourceIndex <= TBInd) continue;
    if (item.style.position == "absolute") continue;
    item.style.posTop =	RelTop;
  }
}

function DoLayout()
{
  LayoutTBs();
}

function validateMode()
{
  if (!	bTextMode) return true;
  alert("请取消“使用 HTML 语法书写”选项再使用系统编辑功能!");
  Composition.focus();
  return false;
}

function format1(what,opt)
{
  if (opt=="removeFormat")
  {
    what=opt;
    opt=null;
  }

  if (opt==null) Composition.document.execCommand(what);
  else Composition.document.execCommand(what,"",opt);

  pureText = false;
  Composition.focus();
}

function format(what,opt)
{
  if (!validateMode()) return;

  format1(what,opt);
}

function setMode(newMode)
{
  bTextMode = newMode;
  var cont;
  if (bTextMode) {
    cleanHtml();
    cleanHtml();

    cont=Composition.document.body.innerHTML;
    Composition.document.body.innerText=cont;
  } else {
    cont=Composition.document.body.innerText;
    Composition.document.body.innerHTML=cont;
  }

  Composition.focus();
}

function getEl(sTag,start)
{
  while	((start!=null) && (start.tagName!=sTag)) start = start.parentElement;
  return start;
}

function UserDialog(what)
{
  if (!validateMode()) return;

  Composition.document.execCommand(what, true);

  pureText = false;
  Composition.focus();
}

function foreColor()
{
  if (!	validateMode())	return;
  var arr = showModalDialog("selcolor.htm", "", "dialogWidth:18.5em; dialogHeight:17.5em; status:0");
  if (arr != null) format('forecolor', arr);
  else Composition.focus();
}

function fortable()
{
  if (!	validateMode())	return;
  var arr = showModalDialog("table.htm", "", "dialogWidth:18.5em; dialogHeight:11.5em; status:0");

  if (arr != null){
  var ss;
  ss=arr.split("*")
  row=ss[0];
  col=ss[1];
  var string;
  string="<table border=1>";
  for(i=1;i<=row;i++){
  string=string+"<tr>";
  for(j=1;j<=col;j++){
  string=string+"<td></td>";
  }
  string=string+"</tr>";
  }
  string=string+"</table>";
  content=Composition.document.body.innerHTML;
  content=content+string;
   Composition.document.body.innerHTML=content;
  }
  else Composition.focus();
}
function cleanHtml()
{
  var fonts = Composition.document.body.all.tags("FONT");
  var curr;
  for (var i = fonts.length - 1; i >= 0; i--) {
    curr = fonts[i];
    if (curr.style.backgroundColor == "#ffffff") curr.outerHTML	= curr.innerHTML;
  }
}

function getPureHtml()
{
  var str = "";
  var paras = Composition.document.body.all.tags("P");
  if (paras.length > 0)	{
    for	(var i=paras.length-1; i >= 0; i--) str	= paras[i].innerHTML + "\n" + str;
  } else {
    str	= Composition.document.body.innerHTML;
  }
  return str;
}

var bLoad=false
var pureText=true
var bodyTag="<head><style type=\"text/css\">body {font-size: 9pt;line-height: 20px;}</style><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><BODY bgcolor=\"#FFFFFF\" MONOSPACE>"
var bTextMode=false

public_description=new Editor

function Editor()
{
  this.put_HtmlMode=setMode;
  this.put_value=putText;
  this.get_value=getText;
}

function getText()
{
	if (bTextMode)
		return Composition.document.body.innerText;
	else
	{
		cleanHtml();
		cleanHtml();
		return Composition.document.body.innerHTML;
	}
}

function putText(v)
{
	if (bTextMode)
		Composition.document.body.innerText = v;
	else
		Composition.document.body.innerHTML = v;
}

function InitDocument()
{
	Composition.document.open();
	Composition.document.write(bodyTag);
	Composition.document.close();
	bLoad=true;
}

function doSelectClick(str, el) {
	var Index = el.selectedIndex;
	if (Index != 0){
		el.selectedIndex = 0;
		if (el.id == "specialtype")
			specialtype(el.options[Index].value);
		else
			format(str,el.options[Index].value);
	}
}
var bIsIE5 = navigator.userAgent.indexOf("IE 5")  > -1;
var edit;
var RangeType;

function specialtype(Mark){
	var strHTML;
	if (bIsIE5) selectRange();
	if (RangeType == "Text"){
		strHTML = "<" + Mark + ">" + edit.text + "</" + Mark + ">";
		edit.pasteHTML(strHTML);
		Composition.focus();
		edit.select();
	}
}

function selectRange(){
	edit = Composition.document.selection.createRange();
	RangeType =  Composition.document.selection.type;
}

function help()
{
    var helpmess;
    helpmess="---------------帮助系统---------------\r\n\r\n"+
    	 "1.发布资料前请先查找有无雷同的资料，以免重复发布!\r\n\r\n"+
         "2.同一段换行时请用Shit+Enter。\r\n\r\n"+
		 "3.若非特殊需要，请别随意改变字体、大小及颜色。\r\n\r\n"+
         "4.出处和标题不能超过50个字符,若超过则自动截断.";
    alert(helpmess);

}