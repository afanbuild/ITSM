// Input 0
/**
 * get element
 * document.getElementById的封装
 * 如果浏览器不支持getElementById方法则返回null
 */
function _getElementById(a)
{
	return document.getElementById?document.getElementById(a):null
}
/**
 * get elements tag name
 * document.getElementsByTagName的封装
 * 根据tagName返回数组，*返回所有tag。
 * 如果浏览器不支持getElementsByTagName方法，则返回空数组
 */
function _getElementsByTagName(a)
{
	return document.getElementsByTagName?document.getElementsByTagName(a):new Array()
}

//标志浏览器是否为Safari
var isSafari=navigator.userAgent.indexOf("Safari")>=0;

/**
 * 一个标准的colArrayAX替换页面内容典范
 * 变量a为地址，aa为回掉处理函数
 */
function _sendXMLRequest(theURL,aa)
{
	var xmlHttpObj=getXMLHttpObj();
	if(!xmlHttpObj||isSafari&&!aa)
	{//特殊浏览器特殊照顾
		(new Image()).src=theURL;
	}
	else
	{//正常的浏览器，用XMLHTTP显示内容
		xmlHttpObj.open("GET",theURL,true);
		xmlHttpObj.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
		xmlHttpObj.send(null);
	}
}
/**
 * 拿到一个可用的XMLHttpRequest对象
 */
function getXMLHttpObj()
{
	var a=null;
	if(window.ActiveXObject)
	{
		a=new ActiveXObject("Msxml2.XMLHTTP");
		if(!a)
		{
			a=new ActiveXObject("Microsoft.XMLHTTP");
		}
	}
	else if(window.XMLHttpRequest)
	{
		a=new XMLHttpRequest();
	}
	return a;
}

function _del(a)
{
	msg="确认不在桌面上显示该模块?\n\n您可以通过自定义桌面恢复显示！";
  if(window.confirm(msg))
  {
	  var module=_getElementById("module_"+a);
	  if(module)
	    module.style.display="none";
	  aI();
	}
	return false
}

/**
 * 查找CSS，将class名字为a的aa自段定义值为ab
 * 例如setCSSAttrib("medit","display", "none");
 * 则代表将.media的display定义为none
 *
 */
function setCSSAttrib(clasName,attrName,attrValue)
{
	if(document.styleSheets)
	{//浏览器有styleSheets，查找CSS列表并修改
		clasName="."+clasName;
		for(var i=0;i<document.styleSheets.length;i++)
		{
			var classI=document.styleSheets[i];
			var rulesI=classI.rules;
			if(!rulesI)
			{
				rulesI=classI.cssRules;
				if(!rulesI){return}
			}
			for(var j=0;j<rulesI.length;j++)
			{
				if(rulesI[j].selectorText.toLowerCase()==clasName.toLowerCase())
				{
					rulesI[j].style[attrName]=attrValue
				}
			}
		}
	}
	else
	{//浏览器不支持styleSheets，一个元素一个元素地找并修改-_-b
		var elementI=_getElementsByTagName("*");
		for(var i=0;i<elementI.length;i++)
		{
			if(elementI[i].className==clasName)
			{
				elementI[i].style[attrName]=attrValue
			}
		}
	}
}


var aC="";

var _pnlo;
var _mod;
var ay=false;

function _upc()
{
//	setCSSAttrib("medit","display",_pnlo||_uli?"":"none");
//	setCSSAttrib("panelo","display",_pnlo?"":"none");
//	setCSSAttrib("panelc","display",_pnlo?"none":"");
//	setCSSAttrib("mod","display",_mod?"":"none");
//	setCSSAttrib("unmod","display",_mod?"none":"");
	//如果_pl为true，并且_uli和_pnlo有一个为true，则设置mttl CSS的鼠标形状为移动
	//如果ay又为false，则把id为c_1、c_2和c_3的这三个元素构成一个数组，传给initHead()函数
	//initHead()函数负责对c_1、c_2、c_3这三个元素下的所有id以_h结尾的子元素设置拖拽事件代码
	//然后把ay设置为true确保initHead()函数只调用一次。之后对mttl CSS设置鼠标形状为move
//	if(_pl&&(_uli||_pnlo))
	{
		if(!ay)
		{
			initHead([_getElementById("col_l"),_getElementById("col_r")]);
			ay=true
		}
		setCSSAttrib("TableHeader","cursor","move")
	}
}

var aq=0;

var colArray=[];
var ap=0;
var am=null;

/**
 * 如果am为null，将am创建为<div>标签，暂时先不显示，鼠标形状为move，
 * 背景为白色，底部padding为0px，直接创建在<body>下。最后返回am
 */
function createDiv()
{
	if(!am)
	{
		am=document.createElement("DIV");
		am.style.display="none";
		am.style.position="absolute";
		am.style.cursor="move";
		am.style.backgroundColor="#ffffff";
		am.style.paddingBottom="0px";
		document.body.appendChild(am)
	}
	return am
}

/**
 * 核心代码
 * al是这样一个对象，它有一个属性obj，默认为null，和init/start/drag/end/fixE五个方法
 * init() ---- 设置初始方法
 *	设置元素a的onmousedown事件响应为al.start方法，并设置am（那个直接隶属于
 * 	<body>的隐藏<div>）的左边在页面左上角（如果没有设置过的话），设置上a的空拖拽
 * 	事件
 * start() --- 拖拽开始事件
 * 	设置全局变量aa和al.obj为事件源（同一时刻只能有一个box在drag状态）。得到隐藏div
 * 	元素的坐标和当前事件的鼠标坐标，回掉aa变量在initHead()函数中注册的onDragStart()函数（将
 *	隐藏div内容填好，移动到鼠标位置）。将当前鼠标坐标记录在aa变量中。设置鼠标移动
 * 	事件响应和鼠标抬起事件响应。
 * drag() ---- 拖拽中事件
 * 	设置全局变量aa为事件源。得到当前鼠标坐标和移动中的div的位置，与上次鼠标坐标相比
 * 	计算出偏移量，修改移动中的div的坐标。记录鼠标当前位置，回掉aa的onDrag()函数。设置
 * 	al.obj为null，等待下个box的移动。
 * end() ----- 拖拽结束事件
 * 	设置onmousemove和onmouseup不响应事件，回掉aa的onDragEnd()函数。
 * fixE()
 * 	确保浏览器兼容性，保证变量a为event事件，并修正事件的layerX/Y（似乎没有用处）
 */
var al = {"obj":null,
	"init":function(a){
		a.onmousedown=al.start;
		if ( isNaN(parseInt(createDiv().style.left)) ) {
			createDiv().style.left="0px";
		}
		if ( isNaN(parseInt(createDiv().style.top)) ) {
			createDiv().style.top="0px";
		}
		a.onDragStart=new Function();
		a.onDragEnd=new Function();
		a.onDrag=new Function()
		},
	"start":function(a){
		var aa=al.obj=this;
		a=al.fixE(a);
		var ab=parseInt(createDiv().style.top);
		var ac=parseInt(createDiv().style.left);
		aa.onDragStart(ac,ab,a.clientX,a.clientY);
		aa.lastMouseX=a.clientX;
		aa.lastMouseY=a.clientY;
		document.onmousemove=al.drag;
		document.onmouseup=al.end;
		return false
		},
	"drag":function(a){
		a=al.fixE(a);
		var aa=al.obj;
		var ab=a.clientY;
		var ac=a.clientX;
		var ad=parseInt(createDiv().style.top);
		var ae=parseInt(createDiv().style.left);
		var af,ag;
		af=ae+ac-aa.lastMouseX;
		ag=ad+ab-aa.lastMouseY;
		createDiv().style.left=af+"px";
		createDiv().style.top=ag+"px";
		aa.lastMouseX=ac;
		aa.lastMouseY=ab;
		aa.onDrag(af,ag,a.clientX,a.clientY);
		return false
		},
	"end":function(){
		document.onmousemove=null;
		document.onmouseup=null;
		al.obj.onDragEnd(parseInt(createDiv().style.left),parseInt(createDiv().style.top));
		al.obj=null
		},
	"fixE":function(a){
		if (typeof a=="undefined") {
			a=window.event;
		}
		if (typeof a.layerX=="undefined") {
			a.layerX=a.offsetX;
		}
		if (typeof a.layerY=="undefined") {
			a.layerY=a.offsetY;
		}
		return a
		}
};

var aw=false;
/**
 * 本函数作用是设置所有标题可拖动，给元素加入拖拽事件响应代码。
 * 本函数只执行一次，aw为true时函数直接返回。
 * 对第一列c_1、第二列c_2、第三列c_3做初始设置。每一列下均有若干id为m_x的<div>标签
 * 每一个<div>标签内容均为一个<table>，该table中有一个<td>命名为m_x_h，这就是可可拖拽的
 * 标题。得到这个<td>元素，加入拖拽事件代码，就是本函数的作用。
 */
function initHead(a)
{
	if(aw)return;
	aw=true;
	//设置全局变量colArray为当前要处理的列数组，也即三个id为c_1、c_2和c_3的<td>元素
	colArray=a;
	//数组colArray中的每个元素都要执行。其实数组colArray只有三个元素，c_1、c_2和c_3，也即第一/二/三列
	for(var i=0;i<colArray.length;i++)
	{//对所有c_x的子结点遍历，其实也就是命名为m_x的div标签。最后一个div标签有其它用处，
		//故此处length-1
		for(var j=0;j<colArray[i].childNodes.length-1;j++)
		{
			var module_i=colArray[i].childNodes[j];
			var head_i=_getElementById(module_i.id+"_head");
			if(!head_i)
				continue;

			//此刻，已经得到了id为m_x_h的<td>元素，即box的标题td
			//将整个大<div>记录在ad对象的module属性中，这个module属性是？？？？
			head_i.module=module_i;
			//用al对象的init方法初始化可拖拽标题td。
			al.init(head_i);

			//得到m_x_h的<a>元素，即id为m_x_url的<a>
			var url_i=_getElementById(module_i.id+"_url");
			if(url_i)
			{//设置<a>的h属性为ad（即上层标题的<td>元素）,这个h属性是？？？？
				url_i.h=head_i;
				//当超级链接被点中，设置上层标题<td>的href和target属性
				//为当前超级链接的href和target属性。这样用户也可以拖超级链接
				url_i.onmousedown=function() {
					this.h.href=this.href;
					this.h.target=this.target;
				}
			}

			var more_i=_getElementById(module_i.id+"_more");
			if(more_i)
			{
			   more_i.module=module_i;
			   more_i.onmouseover=function() {var op_i=_getElementById(this.module.id+"_op");if(op_i) op_i.style.display="";}
			   more_i.onmouseout =function() {var op_i=_getElementById(this.module.id+"_op");if(op_i) op_i.style.display="none";}
			}

			//核心代码：拖拽开始回掉函数
			//关闭定时器，通过之前记录的module属性得到整个大<div>元素，调用aA()
			//函数计算所有box的偏移值。记录
			head_i.onDragStart=function(af,ag) {
					//关闭定时器
					clearInterval(ap);
					//通过之前记录的module属性得到整个大box的<div>元素
					var module_i=this.module;
					//计算页面上所有其它box的偏移值
					aA(module_i);
					//将下一个box的<div>元素记录下来
					module_i.origNextSibling=module_i.nextSibling;
					//得到移动的<div>，指定到鼠标位置，从隐藏状态显示出来
					//使用alpha filter将透明度设置为80,填充好内容和CSS

					//createDiv()生成一个DIV
					var module_i_copy=createDiv();
					module_i_copy.style.left=getOffset(module_i,true);
					module_i_copy.style.top=getOffset(module_i,false);
					module_i_copy.style.height=module_i.offsetHeight;
					module_i_copy.style.width=module_i.offsetWidth;
					module_i_copy.style.display="block";
					module_i_copy.style.opacity=0.8;
					module_i_copy.style.filter="alpha(opacity=80)";
					module_i_copy.innerHTML=module_i.innerHTML;
					module_i_copy.className=module_i.className;
					//设置dragged为false
					this.dragged=false
				};

			//核心代码：拖拽中回掉函数
			//全部由aG函数实现拖拽过程中的移动和“补位”
			head_i.onDrag=function(af,ag) {
				setModulePos(this.module,af,ag);
				//设置dragged为true
				this.dragged=true
				};

			//核心代码：拖拽结束函数
			head_i.onDragEnd=function(af,ag) {
					if (this.dragged) {
						//被拖拽叻，设置动态回位效果，把box安定下来
						ap=aD(this.module,150,15)
					} else {
						//box仅仅被鼠标点叻一下超级链接，需要提供正常
						//的超级链接被点击效果
						ax();
						if (this.href) {
							if (this.target){
								window.open(this.href,this.target)
							} else {
								document.location=this.href
							}
						}
					}
					this.target=null;
					this.href=null;
					//拖拽工作最后一步，取回box内的内容
					if (this.module.nextSibling!=this.module.origNextSibling) {
						aI()
					}
				}
		}//second for end
	}//first for end
}
/**
 * 隐藏浮动的拖拽移动中<div>元素
 */
function ax()
{
	createDiv().style.display="none"
}
/** 设置拖拽结束后的box动态回位效果
 */
function aD(a,aa,ab)
{
	var ac=parseInt(createDiv().style.left);
	var ad=parseInt(createDiv().style.top);
	var ae=(ac-getOffset(a,true))/ab;
	var af=(ad-getOffset(a,false))/ab;
	return setInterval(function(){if(ab<1){clearInterval(ap);ax();return}ab--;ac-=ae;ad-=af;createDiv().style.left=parseInt(ac)+"px";createDiv().style.top=parseInt(ad)+"px"},aa/ab)
}
/**
 * 全局变量colArray为列c_1、c_2和c_3数组（也即三个<td>元素）
 * 计算所有可移动的大box（即<div>）距离页面左边的偏移量和
 * 距离页面上方的偏移量。对于当前拖拽的box，在其所在列中，
 * 所有在它下面的<div>的pagePosTop值需要减去当前拖拽box的高度
 */
function aA(a)
{
	for(var aa=0;aa<colArray.length;aa++)
	{
		var ab=0;
		for(var ac=0;ac<colArray[aa].childNodes.length;ac++)
		{
			var ad=colArray[aa].childNodes[ac];
			if(ad==a)
				ab=ad.offsetHeight;
			ad.pagePosLeft=getOffset(ad,true);
			ad.pagePosTop=getOffset(ad,false)-ab
		}
	}
}
/**
 * 得到某一元素距离页面左边或上边的偏移量
 */
function getOffset(obj,isLeftOffset)
{
	var offsetValue=0;
	while(obj!=null)
	{
		offsetValue+=obj["offset"+(isLeftOffset?"Left":"Top")];
		obj=obj.offsetParent
	}
	return offsetValue
}
/**
 * 核心代码，拖拽中处理函数。变量a为box（即<div>元素），aa和ab为偏移量
 */
function setModulePos(obj,posLeft,posTop)
{
	var module=null;
	var ad=100000000;

	//对每一列遍历
	for(var i=0;i<colArray.length;i++)
	{//对每一个<div>box遍历
		for(var j=0;j<colArray[i].childNodes.length;j++)
		{
			var module_i=colArray[i].childNodes[j];
			//对于正在移动中的box自身，不作处理
			if(module_i==obj)
				continue;
			//计算某些偏移量
			var ai=Math.sqrt(Math.pow(posLeft-module_i.pagePosLeft,2)+Math.pow(posTop-module_i.pagePosTop,2));
			if(isNaN(ai))
				continue;
			if(ai<ad)
			{
				ad=ai;module=module_i
			}
		}
	}

	//再适当的位置上添加当前移动中的box
	if(module!=null&&obj.nextSibling!=module)
	{
		module.parentNode.insertBefore(obj,module);
		//TODO: 这行代码好像不起什么作用
		DisplayModule(obj)
	}
}
/**
 * 在页面上显示变量a的父节点
 * TODO: 第一行代码有什么用？
 */
function DisplayModule(obj)
{
	obj.parentNode.style.display="none";
	obj.parentNode.style.display=""
}
/**
 * 构造要取回内容的URL
 */
function aI()
{
	var a="";
	for(var i=0;i<colArray.length;i++)
	{
		a+=a!=""?":":"";
		for(var j=0;j<colArray[i].childNodes.length-1;j++)
		{
			var module=colArray[i].childNodes[j];
			if(module.id=="" || module.style.display=="none")
			   continue;
			a+=module.id.substring(7)+",";
		}
	}
	_sendXMLRequest("../mydestop/frmXmlHttp.aspx?MYTABLE="+escape(a),null)
}


/**
* 展开内容，收拢内容 
*/
function ShowTable(imgCtrl) {
    var ImgPlusScr = "../Images/icon_expandall.gif";      	// pic Plus  +
    var ImgMinusScr = "../Images/icon_collapseall.gif";     // pic Minus -
    var TableID = imgCtrl.id.replace("ImgShowControl_", "divShowControl_");
    
    var tableCtrl;
    tableCtrl = document.getElementById(TableID);
    if (tableCtrl != null) {
        if (imgCtrl.src.indexOf("icon_expandall") != -1) {
            tableCtrl.style.display = "block";
            //tableCtrl.style.visibility = "visible"
            imgCtrl.src = ImgMinusScr;
            imgCtrl.title = "隐藏";

        }
        else {
            tableCtrl.style.display = "none";
            //tableCtrl.style.visibility = "hidden"
            imgCtrl.src = ImgPlusScr;
            imgCtrl.title = "展开";
        }
        aExpandLog();
        
    }
}


/**
* 回写要保存的展开状态
*/
function aExpandLog() {
    var a = "";

    for (var i = 0; i < colArray.length; i++) {
        for (var j = 0; j < colArray[i].childNodes.length - 1; j++) {
            var module = colArray[i].childNodes[j];
            if (module.id == "" || module.style.display == "none")
                continue;
           
            var TableID = module.id.replace("module_", "divShowControl_");

            var tableCtrl;
            tableCtrl = document.getElementById(TableID);
            if (tableCtrl.style.display == "none")
                a += module.id.substring(7) + ",";
        }
    }
    //alert(a);
   _sendXMLRequest("../mydestop/frmXmlHttp.aspx?MYTABLEEXPAND=" + escape(a), null)
}
