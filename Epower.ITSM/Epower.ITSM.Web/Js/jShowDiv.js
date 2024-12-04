// 弹出静态div、弹出可拖拽div

/*
 功能: 获取滚动条的高度
 return: 返回数据格式 如( ,0)  0表示无滚动条 >0 表示滚动条高度
*/

function getPageScroll() {
    var yScroll;

    if (self.pageYOffset) {
        yScroll = self.pageYOffset;
    } else if (document.documentElement && document.documentElement.scrollTop) {
        yScroll = document.documentElement.scrollTop;
    } else if (document.body) {
        yScroll = document.body.scrollTop;
    }

    arrayPageScroll = new Array('', yScroll)
    return arrayPageScroll;
}

/*
 功能: 获取页面实际大小
 return: 返回数据格式 如(1800,600,1800,600) 页宽、页高、窗体宽、窗体高
*/

function getPageSize() {
    var xScroll, yScroll;

    if (window.innerHeight && window.scrollMaxY) {
        xScroll = document.body.scrollWidth;
        yScroll = window.innerHeight + window.scrollMaxY;
    } else if (document.body.scrollHeight > document.body.offsetHeight) {
        sScroll = document.body.scrollWidth;
        yScroll = document.body.scrollHeight;
    } else {
        xScroll = document.body.offsetWidth;
        yScroll = document.body.offsetHeight;
    }

    var windowWidth, windowHeight;

    if (self.innerHeight) {
        windowWidth = self.innerWidth;
        windowHeight = self.innerHeight;
    } else if (document.documentElement && document.documentElement.clientHeight) {
        windowWidth = document.documentElement.clientWidth;
        windowHeight = document.documentElement.clientHeight;
    } else if (document.body) {
        windowWidth = document.body.clientWidth;
        windowHeight = document.body.clientHeight;
    }

    var pageWidth, pageHeight
    if (yScroll < windowHeight) {
        pageHeight = windowHeight;
    } else {
        pageHeight = yScroll;
    }
    if (xScroll < windowWidth) {
        pageWidth = windowWidth;
    } else {
        pageWidth = xScroll;
    }
    arrayPageSize = new Array(pageWidth, pageHeight, windowWidth, windowHeight)
    return arrayPageSize;
}

/*
 功能: 获取内容层(被弹出层)原始尺寸
 return: 返回数据格式 如(600,400) 层宽、层高
*/

function getConSize(divId) {
    var conObj = document.getElementById(divId)
    conObj.style.position = "absolute";
    conObj.style.left = -1000 + "px";
    conObj.style.display = "";

    var arrayConSize = [conObj.offsetWidth, conObj.offsetHeight]
    conObj.style.display = "none";
    return arrayConSize;
}

/*
 功能: 将层插入到某元素后
*/

function insertAfter(newElement, targetElement) {
    var parent = targetElement.parentNode;
    if (parent.lastChild == targetElement) {
        parent.appendChild(newElement);
    }
    else {
        parent.insertBefore(newElement, targetElement.nextSibling);
    }
}

/*
 功能: 弹出内容层
 objId: 元素ID，插入到该元素父元素的后面
 divId: 被弹出的层ID
*/

function openLayer(objId, divId) {
    var arrayPageSize = getPageSize();
    var arrayPageScroll = getPageScroll();

    if (!document.getElementById("popupAddr")) {
        var popupDiv = document.createElement("div"); //创建弹出内容层        
        popupDiv.setAttribute("id", "popupAddr") //给这个元素设置属性与样式
        popupDiv.style.position = "absolute";
        popupDiv.style.border = "1px solid #ccc";
        popupDiv.style.background = "#fff";
        popupDiv.style.zIndex = 99;

        var bodyBack = document.createElement("div"); //创建弹出背景层(遮照层)
        bodyBack.setAttribute("id", "bodybg")
        bodyBack.style.position = "absolute";
        bodyBack.style.width = "100%";
        bodyBack.style.height = (arrayPageSize[1] + 35 + 'px');
        bodyBack.style.zIndex = 98;
        bodyBack.style.top = 0;
        bodyBack.style.left = 0;

        bodyBack.style.filter = "alpha(opacity=50)";
        bodyBack.style.opacity = 0.5;
        bodyBack.style.background = "#ddf";

        //实现弹出(插入到目标元素之后)
        var mybody = document.getElementById(objId);
        insertAfter(popupDiv, mybody); //执行函数insertAfter()
        insertAfter(bodyBack, mybody); //执行函数insertAfter()
    }

    document.getElementById("bodybg").style.display = ""; //显示背景层    
    var popObj = document.getElementById("popupAddr"); //显示内容层
    popObj.innerHTML = document.getElementById(divId).innerHTML;
    popObj.style.display = "";
//    popObj.style.width  = "600px";      //让弹出层在页面中垂直左右居中(统一)
//    popObj.style.height = "400px";
//    popObj.style.top  = arrayPageScroll[1] + (arrayPageSize[3] - 35 - 400) / 2 + 'px';
//    popObj.style.left = (arrayPageSize[0] - 20 - 600) / 2 + 'px'; 
           
    var arrayConSize = getConSize(divId); //让弹出层在页面中垂直左右居中(个性)
    popObj.style.top = arrayPageScroll[1] + (arrayPageSize[3] - arrayConSize[1]) / 2 - 80 + 'px';
    popObj.style.left = (arrayPageSize[0] - arrayConSize[0]) / 2 - 30 + 'px';
}

/*
 功能: 关闭弹出层
*/

function closeLayer() {
    document.getElementById("popupAddr").style.display = "none";
    document.getElementById("bodybg").style.display = "none";
    return false;
}


//拖拽
//对“拖动点”定义：onMousedown="StartDrag(this)" onMouseup="StopDrag(this)" onMousemove="Drag(this)"即可
var move = false,oldcolor, _X, _Y;

/*
 功能: 定义准备拖拽的函数
*/

function StartDrag(obj) {
    obj.setCapture(); //对当前对象的鼠标动作进行跟踪
    oldcolor = obj.style.backgroundColor;
    obj.style.background = "#999";
    move = true;
    var parentwin = document.getElementById("popupAddr"); //获取鼠标相对内容层坐标
    _X = parentwin.offsetLeft - event.clientX
    _Y = parentwin.offsetTop - event.clientY
}

/*
 功能: 定义停止拖拽函数
*/

function StopDrag(obj) {
    obj.style.background = oldcolor;
    obj.releaseCapture(); //停止对当前对象的鼠标跟踪
    move = false;
}

/*
 功能: 定义拖拽函数
*/

function Drag(obj) {
    if (move) {
        var parentwin = document.getElementById("popupAddr");
        parentwin.style.left = event.clientX + _X;
        parentwin.style.top = event.clientY + _Y;
    }
}

