<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frm_Equ_HistoryChartView_SVG.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frm_Equ_HistoryChartView_SVG" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>资产配置基线图</title>
</head>
<body>

    <script language="javascript" type="text/javascript" src="../Controls/Calendar/Popup.js"></script>

    <script language="javascript" type="text/javascript" src="../Js/App_Common.js"> </script>

    <script language="javascript" type="text/javascript" src="../Js/App_Base.js"> </script>

    <script language="javascript" type="text/javascript" src="../Js/jquery-1.3.2.js"> </script>

    <script language="javascript" type="text/javascript" src="../Js/jquery-ui-1.7.2.custom.min.js"> </script>

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>
    
    <script type="text/javascript" language="javascript" >
        $(document).ready(function() {
                 
            $("#IMG_HIDDEN_10094sdf").attr("visibility", "visible");

        });

    
    </script>

    <script language="javascript" type="text/javascript">
        var preDispDivId = "";
    $.ajaxSetup({ cache: false });
 var xmlhttpGetShot;  //客户端XML对象
 var blnHasShow = false;
 function CreateDroplstXmlHttpObject()
    {
		try  
		{  
			xmlhttpDroplst = new ActiveXObject("MSXML2.XMLHTTP");  
		}  
		catch(e)  
		{  
			try  
			{  
				xmlhttpDroplst = new ActiveXObject("Microsoft.XMLHTTP");  
			}  
			catch(e2){}  
		}
		return xmlhttpDroplst;
}


function ShowEquShot(obj, nid, version) {
    alert(nid);
}

 function absoluteLocation(element, offset) 
    { var c = 0; while (element) {  c += element[offset];  element = element.offsetParent; } return c; 
    } 
    
function hideMe(id,status) {

    var object = document.getElementById(id);

    if (object != null) {
        //$("#" + id).fadeOut(1000);
        object.style.display = status;

        if (status == "none")
            blnHasShow = false;
    }
}





function LookChangeDetail(flowid)
{
     if(flowid != 0)
     {
        window.open("../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + flowid, '_blank','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');
     }
}


function LookEquDetail(obj,nid,version)
{
   
     var idFields = nid;
     window.open("frmEqu_DeskEdit.aspx?newWin=true&ShowDetail=true&id=" + idFields + "&FlowID=-1&Version=" + version,"_blank","scrollbars=yes,status=yes ,resizable=yes,width=680,height=480");
 }

 function GetEquShot(obj, nid, version) {
     var idFields = nid;
     if (blnHasShow == true) {
         return;
     }

     blnHasShow = true;

     $.ajax({
         url: "frmEqu_DeskShot.aspx?Compare=1&id=" + idFields + "&Version=" + version,
         datatype: "json",
         type: 'GET',
         success: function(data) {   //成功后回调
             sXml = data;
             var object = document.getElementById("divShowEquShot");
                      
             if (object != null) {
                 if (status != "" && status != "none")
                     status = "";
                 //$("#divShowEquShot").fadeIn(2000);
                 object.style.display = status;
             }
             object.innerHTML = sXml;


             if (absoluteLocation(obj, 'offsetLeft') >= 300 && absoluteLocation(obj, 'offsetLeft') <= 700) {
                 object.style.left = absoluteLocation(obj, 'offsetLeft') - object.offsetWidth / 2 + "px";
             }
             if (absoluteLocation(obj, 'offsetLeft') < 300) {
                 object.style.left = absoluteLocation(obj, 'offsetLeft') + 2 + "px";
             }
             if (absoluteLocation(obj, 'offsetLeft') > 700) {
                 object.style.left = absoluteLocation(obj, 'offsetLeft') - object.offsetWidth - 2 + "px";
             }

             if (absoluteLocation(obj, 'offsetTop') > 350) {
                 object.style.top = object.style.top = absoluteLocation(obj, 'offsetTop') - object.offsetHeight + "px";
             }
             else {
                 object.style.top = absoluteLocation(obj, 'offsetTop') + obj.offsetHeight + 2 + "px";
             }
         },
         error: function(e) {    //失败后回调             
             //alert(e);    
             //alert("");     
         },
         beforeSend: function() {  //发送请求前调用，可以放一些"正在加载"之类额话            
             //alert("正在加载");         
         }
     });  
  
 }

String.prototype.trim = function()  //去空格

			{
				// 用正则表达式将前后空格

				// 用空字符串替代。

				return this.replace(/(^\s*)|(\s*$)/g, "");
			}
    </script>

    <style>
        #tooltip
        {
            position: absolute;
            z-index: 3000;
            border: 1px solid #111;
            background-color: #eee;
            padding: 5px;
            opacity: 0.85;
        }
        #tooltip h3, #tooltip div
        {
            margin: 0;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function GetEquShot123(obj, nid, version) {
     
            var idFields = nid;
            $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "frmEqu_DeskShot.aspx?Compare=1&id=" + idFields + "&Version=" + version }).responseText; } });
        }
    
    </script>

    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
