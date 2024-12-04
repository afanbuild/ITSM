<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCheck.aspx.cs" Inherits="Epower.ITSM.Web.frmCheck" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<script language="javascript">
var xmlhttp = null;
var req = null;
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
        
        
        function LookMessage(_url)
        {
           // alert(_url);
			window.open(_url);
			
        }
        
        function checkmessage()
        {
          // alert(document.all.hidLogined.value);
            if(document.all.hidLogined.value == "false")
            {
                return false;
            }
            if(xmlhttp == null)
                xmlhttp = CreateXmlHttpObject();
                  
            if(xmlhttp != null)
            {
                try
                {
					//alert(document.all.hidMessageID.value);
					var newDateObj = new Date();
					var sparamvalue =  newDateObj.getMinutes() + newDateObj.getMilliseconds();

					xmlhttp.open("GET", "Forms/frmContentCheck.aspx?LastMessageID="+document.all.hidMessageID.value+"&CheckType=1&sparam="+sparamvalue , true); 
					xmlhttp.onreadystatechange = function() 
												{ 
													if ( xmlhttp.readyState==4 ) 
													{ 
														if(xmlhttp.responseText!="")
														{
														    document.all.hidMessage.value = xmlhttp.responseText;
														    document.all.cmdCheckMessage.click();
													    }
													    else
													    {
													        document.all.hidMessage.value = "";
														    document.all.cmdCheckMessage.click();
													    }													    
													} 
												} 
					xmlhttp.send(null); 
				}catch(e3){}
            }
        }
//10秒检测一次 
setInterval("checkmessage()",10000);

 
		</script>
<body>
    <form id="form1" runat="server">
    <div>
        <input id="cmdCheckMail" type="button" value="button" />
        <input id="cmdCheckMessage" type="button" value="button" />
        <input id="hidMessageID" value="0,0,0" type="hidden" runat="server" />
        <input id="hidMessage" value="" type="hidden" runat="server" />
        <input id="hidEmail" type="hidden" runat="server" />
        <input id="hidPsw" type="hidden" runat="server" />
        <input id="hidUser" type="hidden" runat="server" />
        <input id="hidLogined" type="hidden" value="false" runat="server" />
        </div>
    </form>
</body>
<script language="javascript">
        checkmessage();
</script>
</html>
