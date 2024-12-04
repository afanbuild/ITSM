//文件名:jquery.Validator.js
//功能说明:本js文件为jquery类库的一个插件,表单验证.
//作者:net
//注意:验证单/多选按钮组修改中。分组验证暂无.
//最后修改:2008-09-05
(function($) { 
    $.fn.tooltip = function(options){
        var opts = $.extend({}, $.fn.tooltip.defaults, options);
        $('body').append('<div class="tooltipshowpanel"></div>');
        $(document).mouseover(function(){
        $('.tooltipshowpanel').hide();
        
        }      
        );
        this.each(function(){
            // Tooltip
            if($(this).attr('tip') != undefined)
            {
                //by watson 2009-06-05
                    $(this).focus(function(){
                    $('.tooltipshowpanel').css({left:$.getLeft(this)+'px',top:$.getTop(this)+'px'});
                    $('.tooltipshowpanel').html($(this).attr('tip')+'<iframe style=\"position: absolute; z-index: 10; width: 100%; height: 100%; top: 0; left: 0; scrolling:no;opacity:0;filter: alpha(opacity=0);\" frameborder=\"0\" src=\"about:blank\" ></iframe>');
                    $('.tooltipshowpanel').fadeIn("fast");
                });
                //end by    
                $(this).hover(function(){
                    $('.tooltipshowpanel').css({left:$.getLeft(this)+'px',top:$.getTop(this)+'px'});
                    $('.tooltipshowpanel').html($(this).attr('tip')+'<iframe style=\"position: absolute; z-index: 10; width: 100%; height: 100%; top: 0; left: 0; scrolling:no;opacity:0;filter: alpha(opacity=0);\" frameborder=\"0\" src=\"about:blank\" ></iframe>');
                    $('.tooltipshowpanel').fadeIn("fast");
                },
                function(){
                    $('.tooltipshowpanel').hide();
                });
            } 
         
            // 验证
            if($(this).attr('dataType') != undefined)
            {
                $(this).focus(function(){
                    $(this).removeClass('tooltipinputerr');
                }).blur(function(){
                    var msg = "";
                    /**这里去验证*/
                    msg = $.isValidator(this);
                    /**根据验证结果显示信息*/
                    if(msg=='t')//if(msg)
                    {
                          $(this).attr("tip",$(this).attr("tipsucc"));
                         $(this).removeClass('tooltipinputerr').addClass('tooltipinputok');
                    }
                    else
                    {
                        $(this).attr("tip",msg.substring(1,msg.length-1));
                       // alert("错误信息是："+msg.substring(1,msg.length-1));
                        $(this).removeClass('tooltipinputok').addClass('tooltipinputerr');
                    }
                    if($(this).attr('toupper') == 'true')
                    {
                        this.value = this.value.toUpperCase();
                    }
                });
            }
        });
        // 表单验证
        if(opts.onsubmit)
        {   
            $('aspnetForm').submit( function () {
                var isSubmit = true;
                 var allinfo=""; 
                $(this).find('textarea[dataType],select[dataType],input[dataType]').each(function(){
                    var msg = "";                  
                    msg = $.isValidator(this);//string   
                    if(msg!='t')
                    {
                        $(this).attr("tip",msg.substring(1,msg.length-1));
                       allinfo+= msg;
                       // alert("错误信息是："+msg.substring(1,msg.length-1));
                        $(this).removeClass('tooltipinputok').addClass('tooltipinputerr');
                        isSubmit = false;
                    }else{
                        $(this).attr("tip",$(this).attr("tipsucc"));
                    }
                });
               return isSubmit;
            } );  
        }
    };
    
    
    
    $.extend({ 
　　            GetValidResult:function()
　　            {
　　                  var isSubmit = true;
　　                  var allinfo1="";
                $("#aspnetForm").find('textarea[dataType],select[dataType],input[dataType]').each(function(){
                    var msg = "";
                    msg = $.isValidator(this);//string
                    if(msg!='t')//if(!msg)
                    {
                         $(this).attr("tip",msg.substring(1,msg.length-1));
                         allinfo1+=msg;
                        //alert("错误信息是："+msg.substring(1,msg.length-1));
                        $(this).removeClass('tooltipinputok').addClass('tooltipinputerr');
                        
                        this.focus();
                        isSubmit = false;
                    }else{
                    $(this).attr("tip",$(this).attr("tipsucc"));
                   // isSubmit=true;
                    }
                });
                 if(isSubmit)
                {
                 return true;
                }else
                {
                //alert("error is: "+allinfo1);
                return false;
                }               
　　  } 
            }); 

/**可以验证单个条件，也可验证多个复合条件，
复合条件在页面的参数以大写F开头并从F开始以后每个验证条件用半角“|”符号隔开(如dataType="F|Require|English")，
验证有范围和长度的需要在页面上设置min="1"和max="20"两个参数,
//比较验证需有三个参数op1和op2的值，operator为比较类型
提示信息为动态信息，开始的提示信息为固定必须写在页面上。
如果需要在填写正确的情况下也要有正确的提示，需要增加一个正确提示项在页面上 tipsucc=""; tip="";
*/
    
    $.extend({
        //验证单个控件
        isValidator : function(object){        
            var dataType = $(object).attr("dataType");
            var _require = $(object).attr("require");
	    if(_require == undefined){_require = "true"}
            var msg = true;
            var info="t";
            if(dataType != undefined)
            {
                if(dataType != "Group" && _require != "false" && object.value.length <= 0)
                {
                    return info+"不能为空。|";//没有内容且不是组验证,不能为空。
                 }
               if(dataType.indexOf("F")>=0&&dataType.indexOf("|"))
               {                
                 var myArray=dataType.split("|");
                 for(var i=1;i<myArray.length;i++)
                 {
                    //alert("myArray[i]: "+myArray[i]);
                     switch(myArray[i])
                    {
                      case "Require"://必填
                        msg = object.value.length > 0;
                        if(!msg){info+="必填，";}
                        break;
                        case "Chinese"://中文
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isChinese(object);if(!msg){info+="必须为中文，";}}
                            }
                            else{msg = $.isChinese(object);}
                            break;
                        case "English"://英文
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isEnglish(object);if(!msg){info+="必须为英文，";}}
                            }
                            else{msg = $.isEnglish(object);if(!msg){info+="必须为英文，";}}
                            break;
                        case "Number"://数字（正整数包括0）
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isNumber(object);if(!msg){info+="必须为数字（正整数包括0），";}}
                            }
                            else{msg = $.isNumber(object);if(!msg){info+="必须为数字（正整数包括0），";}}
                            break;
                        case "Integer"://整数（正整数和负整数）
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isInteger(object);if(!msg){info+="必须为整数（正整数和负整数），";}}
                            }
                            else{msg = $.isInteger(object);if(!msg){info+="必须为整数（正整数和负整数），";}}    
                            break;
                        case "Double"://实数（小数）
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isDouble(object);if(!msg){info+="必须为实数（小数），";}}
                            }
                            else{msg = $.isDouble(object);if(!msg){info+="必须为实数（小数），";}}
                            break;

                        case "Currency"://货币格式
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isCurrency(object);if(!msg){info+="必须为货币格式，";}}
                            }
                            else{msg = $.isCurrency(object);if(!msg){info+="必须为货币格式，";}}
                            break;
                        case "QQ"://QQ
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isQQ(object);if(!msg){info+="必须为QQ号，";}}
                            }
                            else{msg = $.isQQ(object);if(!msg){info+="必须为QQ号，";}}
                            break;
                        case "Email"://邮箱地址
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isEmail(object);if(!msg){info+="必须为邮箱地址，";}}
                            }
                            else{msg = $.isEmail(object);if(!msg){info+="必须为邮箱地址，";}}
                            break;
                        case "Url"://Url地址
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isURL(object);if(!msg){info+="必须为Url地址，";}}
                            }
                            else{msg = $.isURL(object);if(!msg){info+="必须为Url地址，";}}
                            break;
                        case "IP"://IP地址
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isIpAddress(object);if(!msg){info+="必须为IP地址，";}}
                            }
                            else{msg = $.isIpAddress(object);if(!msg){info+="必须为IP地址，";}}
                            break;
                        case "Zip"://邮编
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isPostalCode(object);if(!msg){info+="必须为邮编，";}}
                            }
                            else{msg = $.isPostalCode(object);if(!msg){info+="必须为邮编，";}}
                            break;
                        case "IdCard"://身份证
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isIDCard(object);if(!msg){info+="必须为身份证，";}}
                            }
                            else{msg = $.isIDCard(object);if(!msg){info+="必须为身份证，";}}
                            break;
                        case "Phone"://电话号话
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isPhoneCall(object);if(!msg){info+="必须为电话号话，";}}
                            }
                            else{msg = $.isPhoneCall(object);if(!msg){info+="必须为电话号话，";}}
                            break;
                        case "Mobile"://手机号码
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isMobile(object);if(!msg){info+="必须为手机号码，";}}
                            }
                            else{msg = $.isMobile(object);if(!msg){info+="必须为手机号码，";}}
                            break;
                        case "UserName"://用户名，长度在6-50之间的，只包含字母，数字和下划线
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isValidUserName(object);if(!msg){info+="用户名，长度在6-20之间的，只包含字母，数字和下划线，";}}
                            }
                            else{msg = $.isValidUserName(object);if(!msg){info+="用户名，长度在6-20之间的，只包含字母，数字和下划线，";}}
                            break;
                        case "PassWord"://密码，长度在6-20之间
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isValidPass(object);if(!msg){info+="密码，长度在6-20之间，";}}
                            }
                            else{msg = $.isValidPass(object);if(!msg){info+="密码，长度在6-20之间，";}}
                            break;
                       case "Repeat"://重复输入
                            msg = $.isRepeat(object);
                            if(!msg){info+="两次输入不一致，";}
                            break;
                       case "Compare"://关系比较
                            msg = $.isCompare(object);
                             if(!msg){info+="关系比较失败，";}
                            break;
                       case "Range"://输入范围
                            msg = $.isRange(object);
                             if(!msg){info+="输入超出预定范围："+$(object).attr('min')+"-"+$(object).attr('max')+"，";}
                            break;
                       case "Limit"://限制输入长度
                            msg = $.isLimit(object);
                            if(!msg){info+="输入长度限制为："+$(object).attr('min')+"-"+$(object).attr('max')+"，";}
                            break;
                       case "LimitB"://限制输入的字节长度
                            msg = $.isLimitB(object);
                             if(!msg){info+="限制输入的字节长度："+$(object).attr('min')+"-"+$(object).attr('max')+"，";}
                            break;
                       case "PassWord"://密码，长度在6-20之间
                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isPassWord(object); if(!msg){info+="密码，长度在6-20之间，";}}
                            }
                            else{msg = $.isPassWord(object); if(!msg){info+="密码，长度在6-20之间，";}}
                            break;
                       case "Group"://验证单/多选按钮组
                            msg = $.isGroup(object);
                            if(!msg){info+="最少选择一个项，";}
                            break;
                       case "Length"://验证单/多选按钮组
                            msg = $.isLength(object);
                            if(!msg){info+="长度太长，";}
                            break; 
                       case "OverZero"://大于零的整数

                            if(_require == "false")
                            {
                                if(object.value.length <= 0){msg = true;}
                                else{msg = $.isOverZero(object);if(!msg){info+="必须选择一项，";}}
                            }
                            else{msg = $.isOverZero(object);if(!msg){info+="必须选择一项，";}}
                            break;
                    case "OverEqualsZero"://大于零等于的整数
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isOverEqualsZero(object);if(!msg){info+="必须选择一项，";}}
                        }
                        else{msg = $.isOverEqualsZero(object);if(!msg){info+="必须选择一项，";}}
                        break; 
                      }                    
                   }
               }else{               
                switch(dataType)
                {
                    case "Require"://必填
                        msg = object.value.length > 0;
                        if(!msg){info+="必填，";}
                        break;
                    case "Chinese"://中文
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isChinese(object);if(!msg){info+="必须为中文，";}}
                        }
                        else{msg = $.isChinese(object);}
                        break;
                    case "English"://英文
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isEnglish(object);if(!msg){info+="必须为英文，";}}
                        }
                        else{msg = $.isEnglish(object);if(!msg){info+="必须为英文，";}}
                        break;
                    case "Number"://数字（正整数包括0）
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isNumber(object);if(!msg){info+="必须为数字（正整数包括0），";}}
                        }
                        else{msg = $.isNumber(object);if(!msg){info+="必须为数字（正整数包括0），";}}
                        break;
                    case "Integer"://整数（正整数和负整数）
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isInteger(object);if(!msg){info+="必须为整数（正整数和负整数），";}}
                        }
                        else{msg = $.isInteger(object);if(!msg){info+="必须为整数（正整数和负整数），";}}    
                        break;
                    case "Double"://实数（小数）
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isDouble(object);if(!msg){info+="必须为实数（小数），";}}
                        }
                        else{msg = $.isDouble(object);if(!msg){info+="必须为实数（小数），";}}
                        break;

                    case "Currency"://货币格式
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isCurrency(object);if(!msg){info+="必须为货币格式，";}}
                        }
                        else{msg = $.isCurrency(object);if(!msg){info+="必须为货币格式，";}}
                        break;
                    case "QQ"://QQ
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isQQ(object);if(!msg){info+="必须为QQ号，";}}
                        }
                        else{msg = $.isQQ(object);if(!msg){info+="必须为QQ号，";}}
                        break;
                    case "Email"://邮箱地址
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isEmail(object);if(!msg){info+="必须为邮箱地址，";}}
                        }
                        else{msg = $.isEmail(object);if(!msg){info+="必须为邮箱地址，";}}
                        break;
                    case "Url"://Url地址
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isURL(object);if(!msg){info+="必须为Url地址，";}}
                        }
                        else{msg = $.isURL(object);if(!msg){info+="必须为Url地址，";}}
                        break;
                    case "IP"://IP地址
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isIpAddress(object);if(!msg){info+="必须为IP地址，";}}
                        }
                        else{msg = $.isIpAddress(object);if(!msg){info+="必须为IP地址，";}}
                        break;
                    case "Zip"://邮编
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isPostalCode(object);if(!msg){info+="必须为邮编，";}}
                        }
                        else{msg = $.isPostalCode(object);if(!msg){info+="必须为邮编，";}}
                        break;
                    case "IdCard"://身份证
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isIDCard(object);if(!msg){info+="必须为身份证，";}}
                        }
                        else{msg = $.isIDCard(object);if(!msg){info+="必须为身份证，";}}
                        break;
                    case "Phone"://电话号话
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isPhoneCall(object);if(!msg){info+="必须为电话号话，";}}
                        }
                        else{msg = $.isPhoneCall(object);if(!msg){info+="必须为电话号话，";}}
                        break;
                    case "Mobile"://手机号码
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isMobile(object);if(!msg){info+="必须为手机号码，";}}
                        }
                        else{msg = $.isMobile(object);if(!msg){info+="必须为手机号码，";}}
                        break;
                    case "UserName"://用户名，长度在6-50之间的，只包含字母，数字和下划线
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isValidUserName(object);if(!msg){info+="用户名，长度在6-20之间的，只包含字母，数字和下划线，";}}
                        }
                        else{msg = $.isValidUserName(object);if(!msg){info+="用户名，长度在6-20之间的，只包含字母，数字和下划线，";}}
                        break;
                    case "PassWord"://密码，长度在6-20之间
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isValidPass(object);if(!msg){info+="密码，长度在6-20之间，";}}
                        }
                        else{msg = $.isValidPass(object);if(!msg){info+="密码，长度在6-20之间，";}}
                        break;
                   case "Repeat"://重复输入
                        msg = $.isRepeat(object);
                        if(!msg){info+="两次输入不一致，";}
                        break;
                   case "Compare"://关系比较
                        msg = $.isCompare(object);
                         if(!msg){info+="关系比较失败，";}
                        break;
                   case "Range"://输入范围
                        msg = $.isRange(object);
                         if(!msg){info+="输入超出预定范围："+$(object).attr('min')+"-"+$(object).attr('max')+"，";}
                        break;
                   case "Limit"://限制输入长度
                        msg = $.isLimit(object);
                        if(!msg){info+="输入长度限制为："+$(object).attr('min')+"-"+$(object).attr('max')+"，";}
                        break;
                   case "LimitB"://限制输入的字节长度
                        msg = $.isLimitB(object);
                         if(!msg){info+="限制输入的字节长度："+$(object).attr('min')+"-"+$(object).attr('max')+"，";}
                        break;
                   case "PassWord"://密码，长度在6-20之间
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isPassWord(object); if(!msg){info+="密码，长度在6-20之间，";}}
                        }
                        else{msg = $.isPassWord(object); if(!msg){info+="密码，长度在6-20之间，";}}
                        break;
                   case "Group"://验证单/多选按钮组
                        msg = $.isGroup(object);
                        if(!msg){info+="最少选择一个项，";}
                        break;
                   case "Length"://验证单/多选按钮组
                        msg = $.isLength(object);
                        if(!msg){info+="长度太长，";}
                        break; 
                   case "OverZero"://大于零的整数
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isOverZero(object);if(!msg){info+="必须选择一项，";}}
                        }
                        else{msg = $.isOverZero(object);if(!msg){info+="必须选择一项，";}}
                        break;
                  case "OverEqualsZero"://大于零的整数
                        if(_require == "false")
                        {
                            if(object.value.length <= 0){msg = true;}
                            else{msg = $.isOverEqualsZero(object);if(!msg){info+="必须选择一项，";}}
                        }
                        else{msg = $.isOverEqualsZero(object);if(!msg){info+="必须选择一项，";}}
                        break; 
                }
                }//else end
            }            
              return info;// msg;
        },
        getWidth : function(object) {
            return object.offsetWidth;
        },
        getLeft : function(object) {
            var go = object;
            var oParent,oLeft = go.offsetLeft;
            while(go.offsetParent!=null) {
                oParent = go.offsetParent;
                oLeft += oParent.offsetLeft;
                go = oParent;
            }
            return oLeft;
        },
        getTop : function(object) {
            var go = object;
            var oParent,oTop = go.offsetTop;
            while(go.offsetParent!=null) {
                oParent = go.offsetParent;
                oTop += oParent.offsetTop;
                go = oParent;
            }
            return oTop + 22;
        },
        //去除左边的空格
        ltrim: function(_str)
        {
            return _str.replace(/(^\s*)/g, "");
        },
        //去除右边的空格
        rtrim: function(_str)
        {
            return _str.replace(/(\s*$)/g, "");
        },
        //因为jquery本身已经有了trim方法,故这里不再重新定义
        //计算字符串长度，一个双字节字符长度计2，ASCII字符计1
        lengthw: function(_str)
        {
           return  _str.replace(/[^\x00-\xff]/g,"rr").length; 
        },
        //转换为大写
        toUpper: function(_str)
        {
            return _str.toUpperCase();
        },
        //转换为小写
        toLower: function(_str)
        {
            return _str.toLowerCase();
        },
        //15位身份证转换为18位,如果参数字符串中有非数字字符,则返回"#"表示参数不正确
        idCardUpdate: function(_str)
        { 
            var idCard18;
            var regIDCard15 = /^(\d){15}$/;
            if(regIDCard15.test(_str))
            {
                var nTemp = 0;
                var ArrInt = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2);
                var ArrCh = new Array('1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2');
                _str = _str.substr(0,6) + '1' + '9' + _str.substr(6,_str.length-6);
                for(var i=0;i<_str.length;i++)
                {
                    nTemp += parseInt(_str.substr(i,1)) * ArrInt[i];
                }
                _str += ArrCh[nTemp % 11];
                idCard18 = _str;        
            }
            else
            {
                idCard18 = "#";
            }
            return idCard18;
        },
        //是否为空字符串
        isEmpty: function(_str)
        {
            var tmp_str = jQuery.trim(_str);
            return tmp_str.length == 0; 
        },
        //是否中文
        isChinese: function(object)
        {
            return /^[\u4E00-\u9FA5]{0,25}$/.test(object.value);
        },
        //是否英文
        isEnglish: function(object)
        {
            return /^[A-Za-z]+$/.test(object.value);
        },
        //数字（正整数）
        isNumber : function(object)
        {
            return /^\d+$/.test(object.value);
        },
        //整数（正整数和负整数）
        isInteger : function(object)
        {
            return /^[-\+]?\d+$/.test(object.value);
        },
        //实数（小数）
        isDouble : function(object)
        {
            return /^[-\+]?\d+(\.\d+)?$/.test(object.value);
        },
        //QQ
        isQQ : function(object)
        {
            return /^[1-9]\d{3,8}$/.test(object.value);
        },
        //是否为合法电子邮件地址
        isEmail: function(object)
        {
           return /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/.test(object.value); 
        },
        //是否合法url地址
        isURL: function(object)
        {
            var regTextUrl = /^(file|http|https|ftp|mms|telnet|news|wais|mailto):\/\/(.+)$/;
            return regTextUrl.test(object.value);
        },
        //是否为合法ip地址
        isIpAddress: function(object)
        {
            reVal = /^(\d{1}|\d{2}|[0-1]\d{2}|2[0-4]\d|25[0-5])\.(\d{1}|\d{2}|[0-1]\d{2}|2[0-4]\d|25[0-5])\.(\d{1}|\d{2}|[0-1]\d{2}|2[0-4]\d|25[0-5])\.(\d{1}|\d{2}|[0-1]\d{2}|2[0-4]\d|25[0-5])$/;
            return (reVal.test (object.value));    
        },
        //是否邮政编码(中国)
        isPostalCode: function(object)
        {
            var regTextPost = /^(\d){6}$/;
            return regTextPost.test(object.value);
        },
        //是否是有效中国身份证
        isIDCard: function(object)
        {
            var iSum=0;
            var info="";
            var sId;
            var aCity={11:"北京",12:"天津",13:"河北",14:"山西",15:"内蒙古",21:"辽宁",22:"吉林",23:"黑龙江",31:"上海",32:"江苏",33:"浙江",34:"安徽",35:"福建",36:"江西",37:"山东",41:"河南",42:"湖北",43:"湖南",44:"广东",45:"广西",46:"海南",50:"重庆",51:"四川",52:"贵州",53:"云南",54:"西藏",61:"陕西",62:"甘肃",63:"青海",64:"宁夏",65:"新疆",71:"台湾",81:"香港",82:"澳门",91:"国外"};
            //如果输入的为15位数字,则先转换为18位身份证号
            if(object.value.length == 15)    
                sId = jQuery.idCardUpdate(object.value);    
            else
                sId = object.value;
            
            if(!/^\d{17}(\d|x)$/i.test(sId))
            {
                return false;
            }
            sId=sId.replace(/x$/i,"a");
            //非法地区
            if(aCity[parseInt(sId.substr(0,2))]==null)
            {
                return false;
            }
            var sBirthday=sId.substr(6,4)+"-"+Number(sId.substr(10,2))+"-"+Number(sId.substr(12,2));
            var d=new Date(sBirthday.replace(/-/g,"/"))    
            //非法生日
            if(sBirthday!=(d.getFullYear()+"-"+ (d.getMonth()+1) + "-" + d.getDate()))
            {
                return false;
            }
            for(var i = 17;i>=0;i--) 
            {
                iSum += (Math.pow(2,i) % 11) * parseInt(sId.charAt(17 - i),11);
            }
            if(iSum%11!=1)
            {
                return false;
            }
            return true;    
        },
        //是否有效的电话号码(中国)
        isPhoneCall: function(object)
        {
            return /(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{6,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^[0-9]{3,4}[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)/.test(object.value);
        },
        //是否有效的手机号码(最新的手机号码段可以是15|18开头的
        isMobile: function(object)
        {
            return /^0{0,1}1(3|5|8)[0-9]{9}$/.test(object.value);
        },
        //是否有效用户名,长度在6-50之间的，只包含字母，数字和下划线
        isValidUserName: function(object)
        {
            return /^\w{6,20}$/.test(object.value);
        },
       //是否有输入,长度在6-20之间的，只包含字母和数字
        isValidInput: function(object)
        {
            return /^[a-z0-9A-Z]{6,20}$/.test(object.value);
        }, 
        //货币格式（小数及有正负）
        isCurrency: function(object)
        {
            return /^[-\+]?\d+(\.\d+)?$/.test(object.value);
        },
        //是否有效密码,长度在6-20之间
        isValidPass: function(object)
        {
			return /^\w{6,20}$/.test(object.value);
        },
        
       //是否有是大于零的整数 OverZero by watson 2009-03-06
        isOverZero: function(object)
        {
			return /(^0*)+([1-9]+)+(\d*$)/.test(object.value);
        },
       //是否有是大于=零的整数 OverZero by watson 2009-03-06
        isOverEqualsZero: function(object)
        {
			return /(^[0-9]+)+(\d*$)/.test(object.value);
        }, 
        
        //重复输入
        isRepeat: function(object)
        {
            var to_obj = document.getElementsByName($(object).attr('to'))[0];
            if(to_obj == undefined){to_obj = document.getElementsById($(object).attr('to'))[0];}
            if(to_obj == undefined){return true;}
            else
            {
                if(to_obj == undefined){return true;}
                else
                {
                    if(to_obj.value == object.value){return true;}
                    else{return false;}
                }  
            }
        },
        //关系比较
        isCompare : function(object)
        {
             var operator = $(object).attr('operator');
             if(operator == undefined){return true;}
             else{$.compare(object.value,operator,$(object).attr('to'));}
        },
         //输入长度
        isLength : function(object)
        {
            var len = object.value.length;            
            return $(object).attr('length')>=len;
        },
        //输入范围
        isRange : function(object)
        {
            return $(object).attr('min') <= (object.value|0) && (object.value|0) <= $(object).attr('max');
        },
        //限制输入长度
        isLimit : function(object)
        {
            var min = $(object).attr('min');
            var max = $(object).attr('max');
            var len = object.value.length;            
            min = min || 0;
            max = max || Number.MAX_VALUE;
            return min <= len && len <= max;
        },
        //限制输入的字节长度
        isLimitB : function(object)
        {
            var min = $(object).attr('min');
            var max = $(object).attr('max');
            var len = $.lengthw(object.value);
            min = min || 0;
            max = max || Number.MAX_VALUE;
            return min <= len && len <= max;
        },
        //自定义正则表达式验证
        isCustom : function(object)
        {
            return new RegExp($(object).attr('regexp'),"g").test(object.value);
        },
        //验证单/多选按钮组
        isGroup : function(object)
        {
            var min = $(object).attr('min');
            var max = $(object).attr('max');
            var hasChecked = 0;
            min = min || 1;
            max = max || object.length;
            for(var i=object.length-1;i>=0;i--)
            if(object[i].checked) hasChecked++;
                return min <= hasChecked && hasChecked <= max; 
        },
        
        //比较op1和op2的值，operator为比较类型
        compare : function(op1,operator,op2)
        {
            switch (operator)
            {
                case "NotEqual":
                    return (op1 != op2);
                case "GreaterThan":
                    return (op1 > op2);
                case "GreaterThanEqual":
                    return (op1 >= op2);
                case "LessThan":
                    return (op1 < op2);
                case "LessThanEqual":
                    return (op1 <= op2);
                default:
                    return (op1 == op2); 
           }           
        },
        		
        onsubmit : true	
    });  
    $.fn.tooltip.defaults = { onsubmit: true }; //默认表单是否验证：true/false
	$(document).ready(function(){jQuery('textarea[tip],select[tip],input[tip]').tooltip();}); 
})(jQuery)