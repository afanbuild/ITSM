<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.flow_View_Chart" Codebehind="flow_View_Chart.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head runat=server>
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
         //异步获取
            if(xmlhttpGetShot == null)
                 xmlhttpGetShot = CreateDroplstXmlHttpObject();       
            if(xmlhttpGetShot != null)
            {
                try
                {	
                
			        xmlhttpDroplst.open("GET", "frmFlowNodeShot.aspx?FlowModelid=" + fmid + "&NodeModelID=" + nmid + "&FlowID= " + fid + "&HasDo=" + hasDo, true); 
			        //window.open("../Common/frmXmlHttpDroplst.aspx?id=" + idFields);
                    xmlhttpDroplst.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
			        xmlhttpDroplst.onreadystatechange = function() 
										        { 
    										        
											        if ( xmlhttpGetShot.readyState==4 ) 
											        {   
											            sXml = xmlhttpGetShot.responseText;
    											        var object = document.getElementById("divShowFlowShot");
        
                                                        if(object != null)
                                                        {
                                                            object.style.display = status;
                                                        }
                                                        object.innerHTML = sXml;
												       
												       
												         if(absoluteLocation(obj, 'offsetLeft') >=300 && absoluteLocation(obj, 'offsetLeft') <=700 )
												         {
												            object.style.left = absoluteLocation(obj, 'offsetLeft') - object.offsetWidth / 2 + "px";       
												         }
												         if(absoluteLocation(obj, 'offsetLeft') <300 )
												         {
												            object.style.left = absoluteLocation(obj, 'offsetLeft') + 2 + "px";       
												         }
												         if(absoluteLocation(obj, 'offsetLeft') >700 )
												         {
												            object.style.left = absoluteLocation(obj, 'offsetLeft') - object.offsetWidth - 2 + "px";       
												         }
												         
												         if(absoluteLocation(obj, 'offsetTop') >350)
												         {
												           object.style.top = object.style.top = absoluteLocation(obj, 'offsetTop') - object.offsetHeight + "px"; 
												           //object.style.top= absoluteLocation(obj, 'offsetTop') + obj.offsetHeight + 2 + "px"; 
												         }
												         else
												         {
												            object.style.top = absoluteLocation(obj, 'offsetTop') + obj.offsetHeight + 2 + "px"; 
												         }
												         // alert( " DIV HE :" + object.offsetHeight  + " top :" + absoluteLocation(obj, 'offsetTop'));
												        
                                                         
    											        
                                                     }
                                                     
                                                }   
                      xmlhttpDroplst.send(null);   
                 }
                 catch(e3)
                 {
                 }
                 
           }
}


String.prototype.trim = function()  //去空格

			{
				// 用正则表达式将前后空格

				// 用空字符串替代。

				return this.replace(/(^\s*)|(\s*$)/g, "");
			}
</script>
		<form method="post" runat="server">
			<FONT face="宋体"></FONT>
		</form>
	</body>
</HTML>
