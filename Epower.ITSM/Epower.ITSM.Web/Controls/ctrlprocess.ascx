<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.CtrlProcess" Codebehind="CtrlProcess.ascx.cs" %>
<asp:literal id="litProcess" runat="server"></asp:literal>
<div  id='divShowMessageDetail' style='position:absolute;display: none;width:0; height:0;'></div>

<script type="text/javascript">
var blnHasShow;
var xmlhttp = null;
function CreateXmlHttpObject()
{
	try  
	{  
		xmlhttp = new ActiveXObject("MSXML2.XMLHTTP");  
	}  
	catch(e)  
	{  
		try  
		{  
			xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");  
		}  
		catch(e2){}  
	}
	return xmlhttp;
}
function absoluteLocation(element, offset) 
{ 
    var c = 0;
    while (element) {  c += element[offset];  element = element.offsetParent; } 
    return c; 
} 
function GetFlowShotInfo(obj,userid)
{
     if(blnHasShow == true)
      {
          return;
      }

   blnHasShow = true;
 //“Ï≤ΩªÒ»°
    if(xmlhttp == null)
         xmlhttp = CreateXmlHttpObject();   
        
    if(xmlhttp != null)
    {
        try
        {	
	        xmlhttp.open("GET", "../Common/frmGetFlowShot.aspx?userid=" + userid , true); 
            xmlhttp.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
	        xmlhttp.onreadystatechange = function() 
			        { 
				        if ( xmlhttp.readyState==4 ) 
				        {
                             var sXml = xmlhttp.responseText;
                             var object = document.getElementById("divShowMessageDetail");
                             if(object != null)
                             {
                                  object.style.display = "";
                             }
                             object.innerHTML = sXml;
                								       
                			//alert(object.offsetHeight);				       
                            object.style.left = absoluteLocation(obj, 'offsetLeft') +  obj.offsetWidth / 2 + "px";  
                            if(absoluteLocation(obj, 'offsetTop') - object.offsetHeight < 20)
                            {
                                object.style.top = absoluteLocation(obj, 'offsetTop') + obj.offsetHeight / 2 + "px"; 
                            }
                            else
                            {
                                object.style.top = absoluteLocation(obj, 'offsetTop') + obj.offsetHeight / 2 - object.offsetHeight + "px";
                            }
                         }
                         
                    }   
              xmlhttp.send(null);   
         }
         catch(e3)
         {
         }
     }    
      
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
function showuser(userid)
{
    var values = window.showModalDialog("../AppForms/frmUserShow.aspx?userid=" + escape(userid),window,"dialogHeight:300px;dialogWidth:400px");
}
</script>