// 公共方法库

var $o = function(o) { return document.getElementById(o); };

var $F = function(o) { return $o(o).value; };

String.prototype.replaceAll = function(oldChar, newChar) { return this.replace(new RegExp(oldChar, "gm"), newChar); }

function selectValue(s, value)
{
    for (var i = 0; i < s.options.length; i++)
        if (s.options[i].value == value)
        { s.options[i].selected = true; break; }
}

  /*
  * 功能：UBB编辑控制函数
  * 参数: tag 为标签名称 val 为标签参数
  * 返回：[标签名称=参数]修饰文字[/标签名称]
  * [标签名称]修饰文字[/标签名称]
  * [标签名称=参数][/标签名称]
  */
function ubbaction(tag, val){
  //判断是否是火狐浏览器
  if(navigator.userAgent.indexOf('Firefox') >= 0)
  {
      ubbaction2(tag,val);
      return;
  }

  var tag = tag.toLowerCase();
  if(typeof(val) == "undefined"){ val = "";}
  if(!val){ val = "/";}
  var r = document.selection.createRange().text;
  if(tag == "url" && val == "1"){ val = prompt("请输入连接地址:(留空为选定地址)", "http://");
  if(val != "http://" && val != ""){val = "=" + val;
  }else{ val = "/"; }}
  if(tag == "img" && val == "1"){ val = prompt("请输入图片地址:(留空为选定地址)", "");
  if(val != ""){ val = "=" + val;
  }else{ val = "/"; } }
  rr = "(" + tag + val + ")" + r + "(/" + tag +")";
  if(r){ document.selection.createRange().text = rr; }else{
    document.all.TxtContent.value += rr;}
}

function ubbaction2(tag, val){
  var tag = tag.toLowerCase();
  if(typeof(val) == "undefined"){ val = "";}
  if(!val){ val = "/";}
  var r = window.getSelection().text;
  if(tag == "url" && val == "1"){ val = prompt("请输入连接地址:(留空为选定地址)", "http://");
  if(val != "http://" && val != ""){val = "=" + val;
  }else{ val = "/"; }}
  if(tag == "img" && val == "1"){ val = prompt("请输入图片地址:(留空为选定地址)", "");
  if(val != ""){ val = "=" + val;
  }else{ val = "/"; } }
  rr = "(" + tag + val + ")" + r + "(/" + tag +")";
  if(r){ window.getSelection().text = rr; }else{
    window.getSelection().text += rr;}
}

/*
  用JavaScript实现urldecode函数
*/
function urldecode(encodedString)
{
	var output = encodedString;
	var binVal, thisString;
	var myregexp = /(%[^%]{2})/;
	function utf8to16(str)
	{
		var out, i, len, c;
		var char2, char3;

		out = "";
		len = str.length;
		i = 0;
		while(i < len) 
		{
			c = str.charCodeAt(i++);
			switch(c >> 4)
			{ 
				case 0: case 1: case 2: case 3: case 4: case 5: case 6: case 7:
				out += str.charAt(i-1);
				break;
				case 12: case 13:
				char2 = str.charCodeAt(i++);
				out += String.fromCharCode(((c & 0x1F) << 6) | (char2 & 0x3F));
				break;
				case 14:
				char2 = str.charCodeAt(i++);
				char3 = str.charCodeAt(i++);
				out += String.fromCharCode(((c & 0x0F) << 12) |
						((char2 & 0x3F) << 6) |
						((char3 & 0x3F) << 0));
				break;
			}
		}
		return out;
	}
	while((match = myregexp.exec(output)) != null
				&& match.length > 1
				&& match[1] != '')
	{
		binVal = parseInt(match[1].substr(1),16);
		thisString = String.fromCharCode(binVal);
		output = output.replace(match[1], thisString);
	}
	
	return utf8to16(output);
}

/*
 功能: 识别浏览器版本
 返回: 1 IE   2 Firefox  3 Opera
*/

function browserVersion() {
    //var browserName = navigator.appName;          // 如：Microsoft Internet Explorer
    //var browserVersion = navigator.appVersion;     // 如：4
    var browserAgent = navigator.userAgent;       // 如：Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)
    
    if( (browserAgent.indexOf('MSIE') >= 0) && (browserAgent.indexOf('Opera') < 0) ){
        return 1;
    } else if(browserAgent.indexOf('Firefox') >=0 ){
        return 2;
    } else if(browserAgent.indexOf('Opera') >= 0){
        return 3;
    } 
}

/*
 功能: 操作控件中的内容
 type: 0 减数据 1 加数据
 controlName: 控件名称
 objValue: 要操作的值
*/

function addOrCutData(type, controlName, objValue) {
    if (type == 0) {
        var str = document.getElementById(controlName).value;
        str = str.replace(objValue + "|", "")
        document.getElementById(controlName).value = str;
    }
    else if (type == 1) {
        document.getElementById(controlName).value = document.getElementById(controlName).value + objValue + "|";
    }
}

/*
 功能: 操作控件中的内容
 type: 0 减数据 1 加数据
 controlName: 控件名称
 objValue: 要操作的值
 schar: 用什么符号分隔 (如 , | $ ...)
*/

function addOrCutDataByChar(type, controlName, objValue, schar) {
    if (type == 0) {
        var str = document.getElementById(controlName).value;
        str = str.replace(objValue + schar, "")
        document.getElementById(controlName).value = str;
    }
    else if (type == 1) {
        document.getElementById(controlName).value = document.getElementById(controlName).value + objValue + schar;
    }
}

/*
 功能: 数据列表进行分页
 currentpage: 当前页
 pageall: 总页数
 fclick: 查询数据的方法名
*/

function showPage(currentpage, pageall, fclick) {
    currentpage = parseInt(currentpage);
    pageall = parseInt(pageall);
    var begin, end;
    if (currentpage - 3 < 1) {
        begin = 1;
        end = (7 > pageall ? pageall : 7);
    }
    else {
        if (currentpage + 3 < pageall) {
            begin = currentpage - 3;
            end = currentpage + 3;
        }
        else {
            begin = (pageall - 7 > 1 ? pageall - 7 : 1);
            end = pageall;
        }
    }

    if (fclick == null) fclick = "showList";

    var pagehtml = '';
    if (currentpage > 1) pagehtml = '<span style="cursor:pointer;" onclick="' + fclick + '(' + (currentpage - 1) + ');"><a href="javascript:void(0);">上一页</a></span>';
    if (begin > 1) pagehtml += ' <a href="javascript:void(0);" onclick="' + fclick + '(1);">' + 1 + '</a> ';
    if (begin > 2) pagehtml += '...';
    for (var i = begin; i <= end; i++) {
        if (i == currentpage) pagehtml += ' <b>' + i + '</b> ';
        else pagehtml += ' <a href="javascript:void(0);" onclick="' + fclick + '(' + i + ');">' + i + '</a> ';
    }
    if (pageall - end > 2) pagehtml += '...';
    if (pageall - end > 0) pagehtml += ' <a href="javascript:void(0);" onclick="' + fclick + '(' + pageall + ');">' + pageall + '</a> ';
    if (currentpage < pageall) pagehtml += '<span style="cursor:pointer;" onclick="' + fclick + '(' + (currentpage + 1) + ');"><a href="javascript:void(0);">下一页</a></span>';

    return pagehtml;
}

/*
 功能: 获取地址栏参数
 name: 参数名
 return: 返回参数值
*/

function GetQueryString(name) {
//    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
//    var r = window.location.search.substr(1).match(reg);
//    if (r != null) return unescape(r[2]);
    
    var url = window.location.search.substr(1)
    var pattern = /(\w+)=(\w+)/ig;
    var parames = {};
    url.replace(pattern, function(a, b, c){
      parames[b] = c;
     });
    return parames[name];
}

/*
 功能: 添加一条数据到listbox中
 control: 控件对象( var control = document.getElementById('name'); )
 objValue: 要添加的值
*/

function ListBoxAdd(control, objValue) {
    var node = document.createElement("OPTION");
    control.options.add(node);
    node.text = objValue;
    node.value = objValue;
    
    control.appendChild(node);
}

/*
 功能: 检索listbox中是否有该数据
 controlName: 控件名称
 objValue: 要检索的值
 返回 true 为存在，false 为不存在
*/

function ListBoxSearch(controlName, objValue) {
    var obj = document.getElementById(controlName);
    var length = obj.length;

    for (var i = 0; i < length; i++) {
        var value = obj.options[i].value;
        var text = obj.options[i].text;

        if (text == objValue) return true;
    }

    return false;
}

/*
 功能: 获取文件名带有扩展名 
 obj: file元素对象
 返回文件名如(01.jpg)
*/

function getFileName(obj) {
    if (obj.value == "") return "";

    obj.value.replace("\\", "\\\\");
    var pos = obj.value.lastIndexOf("\\") * 1;
    return obj.value.substring(pos + 1);
}

/*
 功能: 获取文件扩展名 
 obj: file元素对象
 返回文件扩展名如(jpg)
*/

function getFileExt(obj) {
    var index = obj.value.lastIndexOf(".");
    var end = obj.value.length;

    return obj.value.substring(index, end);
}

/*
 功能: 匹配文件扩展名 
 strRegex: 用于验证图片扩展名的正则表达式(如: "(.jpg|.JPG|.gif|.GIF)$")
 fileName: 要匹配的文件名
 返回 true 为格式匹配，false 为格式不匹配
*/

function checkFileType(strRegex, fileName) {
    var reg = new RegExp(strRegex);

    if (reg.test(fileName)) {
        return true;
    } else {
        return false;
    }
}

/*
 功能: 向Select里添加Option
 obj: select对象
 sName: option的text值
 sValue: option的value值
*/

function selectAddOption(obj, sName, sValue) {
    var oOption = document.createElement("option");
    oOption.appendChild(document.createTextNode(sName));

    if (arguments.length == 3) {
        oOption.setAttribute("value", sValue);
    }

    obj.appendChild(oOption);
}

/*
 功能: 将特殊字符转换为 字符串
 str: 需要转换的字符
 return: 返回转换后的字符串
*/

function codeToStr(str) {
    var s = "";
    if (str.length == 0) return "";
    for (var i = 0; i < str.length; i++) {
        switch (str.substr(i, 1)) {
        case "<":
            s += "&lt;";
            break;
        case ">":
            s += "&gt;";
            break;
        case "&":
            s += "&amp;";
            break;
        case "   ":
            s += "&nbsp;";
            break;
        case "\"":
            s += "&quot;";
            break;
        case "\n":
            s += "<br>";
            break;
        default:
            s += str.substr(i, 1);
            break;
        }
    }
    return s;
}

/*
 功能: 将字符串转换为 特殊字符 
 str: 需要转换的字符
 return: 返回转换后的字符串
*/

function strToCode(str) {
    var s = "";
    if (str.length == 0) return "";

    s = str.replace(/&lt;/g, "<");
    s = s.replace(/&gt;/g, ">");
    s = s.replace(/&amp;/g, "&");
    s = s.replace(/&nbsp;/g, " ");
    s = s.replace(/&quot;/g, "\"");
    s = s.replace(/&<br>;/g, "\n");
    s = s.replace(/\/\/\//g, "\r\n");  
    
    return s;
}


/*
 功能: 获取本地文件大小(适用于IE6)
 fileName: 本地文件路径,即file控件中的value值
 return: 返回文件大小
 */

function getFileSizeByIE6() {
    var file = new Image(); //把附件当做图片处理放在缓冲区预加载   
    file.dynsrc = document.getElementById("fudPolicy").value; //设置附件的url    
    var filesize = file.fileSize; //获取上传的文件的大小

    return filesize;
}

/*
 功能: 获取本地文件大小(适用于IE7)
 fileName: 本地文件路径,即file控件中的value值
 return: 返回文件大小
 */

function getFileSizeByIE7(fileName) {
    if (document.layers) {
        if (navigator.javaEnabled()) {
            var file = new java.io.File(fileName);
            if (location.protocol.toLowerCase() != 'file:') netscape.security.PrivilegeManager.enablePrivilege('UniversalFileRead');
            return file.length();
        }
        else return -1;
    }
    else if (document.all) {
        window.oldOnError = window.onerror;
        window.onerror = function (err) {
            if (err.indexOf('utomation') != -1) {
                alert('file access not possible'); // 文件无法访问
                return true;
            }
            else return false;
        };
        var fso = new ActiveXObject('Scripting.FileSystemObject');
        var file = fso.GetFile(fileName);
        window.onerror = window.oldOnError;
        return file.Size;
    }
}

/*
 功能: 获取本地图片分辩率(只适用于IE7)
 fileName: 本地文件路径,即file控件中的value值
 return: 返回文件大小
 */

function checkImageDimensions(fileName) {
    var imgURL = 'file:///' + fileName;

    var img = new Image();
    img.OnInit = loadHandler;
    if (document.layers && location.protocol.toLowerCase() != 'file:' && navigator.javaEnabled()) netscape.security.PrivilegeManager.enablePrivilege('UniversalFileRead');
    img.src = imgURL;

    alert(this.width + 'x' + this.height);
}


/*
 功能：等分一个字符串
 str：要拆分的字符串
 num：等分为几段
*/

function splitAverage(str, num) {
    var length = str.length;   // 总长度
    var unit = length/num;     // 单位长度
   
    var arrayStr = new Array(num);
    
    for(var i = 0; i< num; i++) { 
       var text = str.substring(i*unit, (i+1)*unit); 
  
       arrayStr[i] = text;
    }
   
    return arrayStr;
}