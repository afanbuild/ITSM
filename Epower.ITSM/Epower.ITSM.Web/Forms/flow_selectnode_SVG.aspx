<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="flow_selectnode_SVG.aspx.cs" Inherits="Epower.ITSM.Web.Forms.flow_selectnode_SVG" %>


<html xmlns:ms="urn:schemas-microsoft-com:xslt" 
        xmlns:v="urn:schemas-microsoft-com:vml" 
        xmlns:xlink="http://www.w3.org/1999/xlink" 
        xmlns:svg="http://www.w3.org/2000/svg">
<HEAD id="HEAD1" runat=server>
		<title>[选择跳转环节]</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">

<script type="text/javascript" src="../Js/jquery-1.7.2.min.js"></script>

<style type="text/css" >
ul,li,text{padding:0;margin:0;list-style:none}
li{float:left;width:80px;height:30px;line-height:30px}
a{display:block;background-color:red;color:#fff;font-size:12px;width:100%;height:100%;text-align:center;text-decoration	:none}
a:hover{background-color:green}
</style>

	</HEAD>
	<body>
	<script language="javascript" type="text/javascript">


 var xmlhttpGetShot;  //客户端XML对象
 var blnHasShow = false;
 var strHasSelected = "false";
 var strOldColor = "";



 function absoluteLocation(element, offset) 
    { var c = 0; while (element) {  c += element[offset];  element = element.offsetParent; } return c; 
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

function onMouseOverSelectNew(hasDo) {

	var e = event.srcElement;
	var strNodeList = document.all.CanJumpNodeList.value;
	
	var strID = e.id.replace(/\L/, "");
	strID = strID.toString().replace("NNodeLabel_", "");
	strID = strID.toString().replace("TNodeLabel_", "");

	//if(strNodeList.indexOf(strID + ",") != -1 && strHasSelected == "false" )
	if(CheckNodeValidate(strNodeList,strID) == true && strHasSelected == "false" ) {
	    //alert(strID);
	    //DoSelectNode(e);
	   
		//strNodeName = eval("parent.flow_Chart.document.all." + e.id).innerText;
	    GetFlowNodeShot(e, hasDo)
	   
		return true;
	}
	
	
	
}

function onMouseOutSelectNew()
{
	var e = event.srcElement;
	var strNodeList = document.all.CanJumpNodeList.value;

	var strID = e.id.replace(/\L/, "");

	strID = strID.toString().replace("NNodeLabel_", "");
	strID = strID.toString().replace("TNodeLabel_", "");
	if (String(strID.toString()).indexOf("_") > 0) {
	    strID = strID.substr(11);
	}
	
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


    

    
function hideMe(id,status) {
        
        var object = document.getElementById(id);
        
        if(object != null) {
            //var rcntId = obj.id.toString().replace("NNodeLabel_", "NNodeLabel_")
            object.style.display = status;
            
            if(status == "none")
                blnHasShow = false;

  
        }
        //alert(object.style.display);
    }



    function GetFlowNodeShot(obj, hasDo) {

        var nmid = obj.id.replace(/\T/, "");
        var fmid = document.getElementById("hidFlowModelID").value;
        var fid = document.getElementById("hidFlowID").value;

        nmid = obj.id.toString().replace("NNodeLabel_", "");
        nmid = obj.id.toString().replace("TNodeLabel_", "");
        if (String(nmid.toString()).indexOf("_") > 0) {
            nmid = nmid.substr(11);
        }

        var x = $(obj).offset().left;
        var y = $(obj).offset().top + 30;


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
                object.style.left = x;
                object.style.top = y;
                return;

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

        return;      
     
    }
</script>
		<form id="flow_View_Chart" method="post" runat="server">
		    <font face="宋体">
			 <table id="tb001" runat=server class="listContent" width="100%" cellpadding="0">
			    <tr>
			    <td class ="listTitle" width="30%" align=center>驳回后处理方法</td>
                <td class ="list" align=center>
                <asp:RadioButtonList ID="rblHowDo" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="0">重审或按流程</asp:ListItem>
                    <asp:ListItem Value="1">仅重审</asp:ListItem>
                    <asp:ListItem Value="2">仅按流程</asp:ListItem>
                </asp:RadioButtonList>
                </td></tr>
               </table>

               
               <div >
                    <asp:Label ID="lblXml" runat="server" ></asp:Label>
               </div>          
               </font>
               
   <script type="text/javascript">
       function SelectFlowNode(nodeName) {
           var e = event.srcElement;
           var nodeId = e.id.replace("NNodeLabel_", "");
           nodeId = nodeId.replace("TNodeLabel_", "");
           if (String(nodeId.toString()).indexOf("_") > 0) {
               nodeId = nodeId.substr(11);
           }
           var strType = document.all.hidJumpType.value;
           var actionName = "跳转";
           if (strType == "1") {
               actionName = "驳回";
           }
           blnConfirm = window.confirm("您确定" + actionName + "到 [ " + nodeName + " ] 吗？");
           if (blnConfirm == true) {
               setSelectNode(e, nodeId);
               if (strType == "0") {                  
                   window.opener.document.getElementById('DialogBehavior_Value').value = nodeId;
                   window.opener.document.getElementById('DialogBehaviorButton').click();
               }
               else {  
                   window.opener.document.getElementById('DialogBehavior_Value').value = nodeId + "," + $("input[name='rblHowDo']:checked").val();                   
                   window.opener.document.getElementById('DialogBehaviorButton').click();
               }
               window.close();
              
           }
           else {
               window.close();
           }
       }
       
       
       function setSelectNode(e, nodeId) {
           
       }
   </script>
		</form>
	</body>
</HTML>
