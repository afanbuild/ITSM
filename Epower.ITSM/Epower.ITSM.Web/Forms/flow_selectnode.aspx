<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.flow_SelectNode" Codebehind="flow_SelectNode.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat=server>
		<title>选择跳转环节</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
       
		<script language="javascript" type="text/javascript"  src="../Js/jquery-1.7.2.min.js"> </script>
		<script type="text/javascript" src="../Js/Jquery.Query.js"></script>
	<script type="text/javascript" >
	</script>
	</HEAD>
	<body>
	<script language="javascript" type="text/javascript">


 var xmlhttpGetShot;  //客户端XML对象
 var blnHasShow = false;
 var strHasSelected = "false";
 var strOldColor = "";

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
    
    
function onClickNew()
{
	var e = event.srcElement;
	var strNodeList = document.all.CanJumpNodeList.value;
	var strID = e.id.replace(/\L/,"");
	
	var strType = document.all.hidJumpType.value;
	var rbltable = document.getElementById("rblHowDo");
    
    var value = 0;
    if(strType == "1")
    {
        var rbl= rbltable.getElementsByTagName("INPUT");
        for(var i = 0;i<rbl.length;i++)
        { 
            if(rbl[i].checked)
            { 
               value=rbl[i].value;
            }
         }
    }
	
	//if(strNodeList.indexOf(strID + ",") != -1 && strHasSelected == "false" )
	if(CheckNodeValidate(strNodeList,strID) == true && strHasSelected == "false" )
	{
		strNodeName = eval("parent.flow_Chart.document.all." + e.id).innerText;
		str1 = document.all.MsgTitle1.value;
		str2 = document.all.MsgTitle2.value;
		strMsg = str1 + strNodeName + str2;
		//alert(window.confirm(strMsg));
		//alert(window.confirm("确定跳转到　" + strNodeName + "　环节吗？"));
	    blnConfirm = window.confirm(strMsg);
	    if(blnConfirm == true)
	    {

			DoSelectNode(e);
			strHasSelected = "true";
			if(strType == "0")
			{
			    window.returnValue = strID;
			}
			else
			{
			    //驳回时返回参数多一个
			     window.returnValue = strID + "," + value;
			}
			window.close();
		}
		else
		{
		    window.close();
		}
	}
	
	
	//alert(document.all.CanJumpNodeList.value);
	
}

function CheckNodeValidate(strNodeList,strID)
{
    //根据当前环境决定如何判断是否可以选择
    var ret = false;
    var strType = document.all.hidJumpType.value;
    
    


    
    if(strType == "0")
    {
        //跳转
        if(strNodeList.indexOf(strID + ",") != -1 )
         {
            ret = true;
         }
    }
    else
    {
        //驳回
        var rbltable = document.getElementById("rblHowDo");
          var rbl= rbltable.getElementsByTagName("INPUT");
         var value = 0;
          for(var i = 0;i<rbl.length;i++)
          { 
            if(rbl[i].checked)
            { 
               value=rbl[i].value;
            }
          }
         if(value == 1)
         {
            //仅为重审时可以全部选则
            if(strNodeList.indexOf(strID + "|") != -1 )
             {
                ret = true;
             }
         }
         else
         {
             //路径不一样，还不能选择。。。 未完成
             var cPath =  document.all.hidPahtID.value;
             
             if(strNodeList.indexOf(strID + "|" + cPath + ","  ) != -1 )
             {
                ret = true;
             }
         }
    }
    
    return ret;
}

function onMouseOverSelectNew(hasDo)
{
	var e = event.srcElement;
	
	//alert(eval("parent.flow_Chart.document.all." + e.id).innerText);
	//alert(document.all.txt1.innerHTML);
	var strNodeList = document.all.CanJumpNodeList.value;
	//alert(strNodeList);
	var strID = e.id.replace(/\L/,"");
	//if(strNodeList.indexOf(strID + ",") != -1 && strHasSelected == "false" )
	if(CheckNodeValidate(strNodeList,strID) == true && strHasSelected == "false" )
	{
		DoSelectNode(e);
		strNodeName = eval("parent.flow_Chart.document.all." + e.id).innerText;
		GetFlowNodeShot(e,hasDo)
		return true;
	}
	
	
	
}

function onMouseOutSelectNew()
{
	var e = event.srcElement;
	var strNodeList = document.all.CanJumpNodeList.value;

	var strID = e.id.replace(/\L/,"");
	//if(strNodeList.indexOf(strID + ",") != -1 && strHasSelected == "false" )
	if(CheckNodeValidate(strNodeList,strID) == true && strHasSelected == "false" )
	{
		UnDoSelectNode(e);
		hideMe("divShowFlowShot","none");
		window.status = "";
	}
	else
	{
	    if(eval("parent.flow_Chart.document.all." + e.id) !=null)
	    {
		    strOldColor = eval("parent.flow_Chart.document.all." + e.id).style.color;
		 }
	}
	
}

function DoSelectNode(e)
{
	if(eval("parent.flow_Chart.document.all." + e.id) !=null)
	{
		strOldColor = eval("parent.flow_Chart.document.all." + e.id).style.color;
		eval("parent.flow_Chart.document.all." + e.id).style.color="hotpink";
		
		eval("parent.flow_Chart.document.all." + e.id).style.borderStyle="groove";
		eval("parent.flow_Chart.document.all." + e.id).style.borderWidth="0.5mm";
		eval("parent.flow_Chart.document.all." + e.id).style.borderColor="red";
	}
}

function UnDoSelectNode(e)
{
	if(eval("parent.flow_Chart.document.all." + e.id) !=null)
	{
		eval("parent.flow_Chart.document.all." + e.id).style.color = strOldColor;
		eval("parent.flow_Chart.document.all." + e.id).style.borderStyle="";
		eval("parent.flow_Chart.document.all." + e.id).style.borderWidth="0mm";
		eval("parent.flow_Chart.document.all." + e.id).style.borderColor="";
		strHasSelected = "false";
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




function GetFlowNodeShot(obj,hasDo)
{	
      
      var nmid = obj.id.replace(/\L/,"");
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
		<form id="flow_View_Chart" method="post" runat="server">
		    <font face="宋体">
			 <table id="tb001" runat=server class="listContent" width="100%" cellpadding=0>
			    <tr><td class ="listTitle" width="30%" align=center>
                驳回后处理方法
                </td>
                <td class ="list" align=center>
                <asp:RadioButtonList ID="rblHowDo" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="0">重审或按流程</asp:ListItem>
                    <asp:ListItem Value="1">仅重审</asp:ListItem>
                    <asp:ListItem Value="2">仅按流程</asp:ListItem>
                </asp:RadioButtonList>
                </td></tr>
               </table>
               
               </font>
		</form>
	</body>
</HTML>
