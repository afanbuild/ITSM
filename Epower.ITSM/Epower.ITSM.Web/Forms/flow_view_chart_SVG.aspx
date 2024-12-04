<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="flow_view_chart_SVG.aspx.cs" Inherits="Epower.ITSM.Web.Forms.flow_view_chart_SVG" %>

<!DOCTYPE svg PUBLIC "-//W3C//DTD SVG 1.1//EN" "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd">
<html>
	<head id="Head1" runat=server>
		<title>流程视图</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!--<script language="javascript" src="../Js/NoRigthMouse.js"></script>  -->
	</head>
	<body>
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
      
      var nmid = obj.id.replace(/\T/,"");
      var fmid = document.getElementById("hidFlowModelID").value;
      var fid = document.getElementById("hidFlowID").value;
      
      if(blnHasShow == true  || parseInt(nmid) <= 0)
      {
          return;
      }

      blnHasShow = true;

      //=================


      $.ajax({
          type: "get",
          url: "frmFlowNodeShot.aspx?FlowModelid=" + fmid + "&NodeModelID=" + nmid + "&FlowID= " + fid + "&HasDo=" + hasDo,
          beforeSend: function(XMLHttpRequest) {
              //ShowLoading();
          },
          success: function(data, textStatus) {
              sXml = xmlhttpGetShot.responseText;
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
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体"></FONT>
		</form>
	</body>
</HTML>
