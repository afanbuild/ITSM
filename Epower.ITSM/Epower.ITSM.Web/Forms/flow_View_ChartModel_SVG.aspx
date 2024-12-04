<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="flow_View_ChartModel_SVG.aspx.cs" Inherits="Epower.ITSM.Web.Forms.flow_View_ChartModel_SVG" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    	<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		
	<script type="text/javascript" src="../Js/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../Js/Jquery.Query.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <script language="javascript" type="text/javascript">


 var xmlhttpGetShot;  //客户端XML对象
 var blnHasShow = false;


 function absoluteLocation(element, offset) 
    { var c = 0; while (element) {  c += element[offset];  element = element.offsetParent; } return c; 
    } 
    
    

    

    
function hideMe(id,status)
    {
        
        var object = document.getElementById(id);
        
        if(object != null)
        {
            object.style.display = status;
            
            if(status == "none")
              blnHasShow = false;
        }
        //alert(object.style.display);
    }




function GetFlowNodeShot(obj,hasDo)
{    
    var nmid = obj.id.replace(/\T/, "");
    nmid = obj.id.replace("NNodeLabel_", "");
    
      var fmid = document.getElementById("hidFlowModelID").value;
      var fid = "0";
      
      if(blnHasShow == true  || parseInt(nmid) <= 0)
      {
          return;
      }

      blnHasShow = true;


      $.ajax({
          type: "get",
          url: "frmFlowNodeShot.aspx?FlowModelid=" + fmid + "&NodeModelID=" + nmid + "&FlowID=" + fid + "&HasDo=" + hasDo,
          beforeSend: function(XMLHttpRequest) {
              //ShowLoading();
          },
          success: function(data, textStatus) {
             
              var object = document.getElementById("divShowFlowShot");

              if (object != null) {
                  object.style.display = status;
              }
              object.innerHTML = data;


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
                  //object.style.top= absoluteLocation(obj, 'offsetTop') + obj.offsetHeight + 2 + "px"; 
              }
              else {
                  object.style.top = absoluteLocation(obj, 'offsetTop') + obj.offsetHeight + 2 + "px";
              }
              // alert( " DIV HE :" + obje

              },
          complete: function(XMLHttpRequest, textStatus) {
              //HideLoading();
          },
          error: function() {
              //请求出错处理
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


    </div>
    </form>
</body>
</html>
