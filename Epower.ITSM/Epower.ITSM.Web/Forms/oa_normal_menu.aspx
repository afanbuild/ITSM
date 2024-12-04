<%@ Register TagPrefix="uc1" TagName="FormSendMain" Src="../Controls/FormSendMain.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FormSend" Src="../Controls/FormSend.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.OA_Normal_Menu" Codebehind="OA_Normal_Menu.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>OA_Normal_Menu</title>
		<script language="javascript" type="text/javascript" src="../Js/jscommon.js"> </script>
		<script type="text/javascript" src="../Js/jquery-1.7.2.min.js"></script>
		<script  type="text/javascript"  language="javascript">
<!--
	var winOpin;
	var winAttach;
	var winflowSender;
	var strSpecRight = "10";    //无特殊处理的值


	var isFisrtLoad = true;
	 function SendFlowPub()
	 {
	    	     
	     var sJumpToNM = "0";	
	    
	     
	     if(window.flowInfo.SpecRightType.value == "40")
	     {
	         //跳转的时候选择要跳转到的环节模型ＩＤ
	         if ($.browser.msie ) {
	             var tempsJumpToNM = GetJumpToNodeModel(0);
	             sJumpToNM = tempsJumpToNM;
	         }
	         else {	             
	             GetJumpToNodeModel(0);
	             if (!isFisrtLoad) {
	                 sJumpToNM = $("#DialogBehavior_Value").val();
	             }
	         }
	         if (!$.browser.msie && $("#DialogBehavior_Value").val() == "-1") {	           
	             isFisrtLoad = false;
	             return false ;
	         }      
	         if (!sJumpToNM )
			{
				alert("没有可供跳转的环节或者您没有选择跳转的环节！");
				return false;
			}
			else
			{
				window.flowInfo.JumpToNodeModel.value = sJumpToNM;
				window.flowSubmit.JumpToNodeModel.value = sJumpToNM;
            }
            isFisrtLoad = false;
			
	     }

	    
	      if(window.flowInfo.SpecRightType.value == "90") {

	          if ($.browser.msie) {
	              //跳转的时候选择要跳转到的环节模型ＩＤ
	              sJumpToNM = GetJumpToNodeModel(1);
	          }
	          else {
	              GetJumpToNodeModel(1);
	              if (!isFisrtLoad) {
	                  sJumpToNM = $("#DialogBehavior_Value").val();
	              }
	          }
	          if (!$.browser.msie && $("#DialogBehavior_Value").val() == "-1") {
	              isFisrtLoad = false;
	              return false;
	          }
	     
			
			if(!sJumpToNM)
			{
				alert("没有可供驳回的环节或者您没有选择驳回的环节！");
				return false;
			}
			else
			{
			    //驳回返回的值为  1,0  [环节编号],[方式]
			    var arr=sJumpToNM.split(",");
			    if(arr[1] == "1")
			    {
			        //仅重审
			        window.flowInfo.SpecRightType.value = "89"
			    }
			    
			    if(arr[1] == "2")
			    {
			        ////仅按流程继续处理
			        window.flowInfo.SpecRightType.value = "91"
			    }
			    
				window.flowInfo.JumpToNodeModel.value = arr[0];
				window.flowSubmit.JumpToNodeModel.value = arr[0];
			}
			
	     }
	     
	    window.flowInfo.action = "flow_sender.aspx";
		//window.open("","flow_sender1" ,"scrollbars=no,resizable=no,top="+(screen.availheight-410)/2 + ",left=" +(screen.availwidth-537)/2 + ",width=537,height=410");
		flowInfo.target="fraprocess";
		flowInfo.submit();
		 
	 }
	 


	//提交流程退回
	function SendBack()
	 {
	     window.flowSubmit.SubmitType.value = "SendBack";
	     var sobj =window.parent.main.document.all.hidFormid.value;
         var obj = window.parent.main.document.getElementById(sobj.replace("hidActionID","txtOpinion"));
	     if (typeof(obj) != "undefined")
		 {
		     window.flowInfo.OpinionValue.value = obj.value;
		    if(window.flowInfo.OpinionValue.value == "")
		    {
		        obj.focus();
				alert("请输入退回的处理意见!");
				return;
		    }
		 }	
		 else
		 {
			window.flowInfo.OpinionValue.value="";
		 }
		 FlowSubmitPub();
	 }

	
	//提交流程暂存
     function TempSave()
	 {
		window.parent.main.GetFieldValuesForSave(0);
	 }
	 
	 //提交流程暂存
	 function TempSavePub()
	 {
		window.flowSubmit.SubmitType.value= "Save";
		FlowSubmitPub();
	 }
	 
	 // 提交流程发送
	 function FlowSubmit()
	 {
		window.flowSubmit.SubmitType.value= "Send";
		FlowSubmitPub();
	 }
	  
	 // 提交流程发送
	 function FlowSubmitPub()
	 {
		/// 取相关参数的值 主题、模型编号 、信息体、意见、接收人员、（附件）
		window.flowSubmit.Subject.value= window.flowInfo.Subject.value.trim();
		window.flowSubmit.FlowModelID.value= window.flowInfo.FlowModelID.value;
		window.flowSubmit.MessageID.value= window.flowInfo.MessageID.value;
		window.flowSubmit.FlowID.value= window.flowInfo.FlowID.value;
		window.flowSubmit.ActionID.value= window.flowInfo.ActionID.value;
		
		window.flowSubmit.FormXMLValue.value= window.flowInfo.FormXMLValue.value;
		
		window.flowSubmit.OpinionValue.value= window.flowInfo.OpinionValue.value;
		
		window.flowSubmit.SpecRightType.value= window.flowInfo.SpecRightType.value;
		window.flowSubmit.Importance.value= window.flowInfo.Importance.value;
		
		//注意 ： 附件处理程序没做，这里的值为空串。
		window.flowSubmit.Attachment.value= window.flowInfo.Attachment.value;
		
		window.flowSubmit.target='_parent';
		window.flowSubmit.submit();
	 }
	 
	 
	  function ReceiveMessage()
	  { 
		
		window.flowInfo.action ="flow_Receive_submit.aspx";
		
		window.flowInfo.target = '_parent';
		window.flowInfo.submit(); 
	  } 
	 
	 function XMLQuote(input)
	{
		return input.replace(/&/g,"&amp;").replace(/</g,"&lt;").replace(/>/g,"&gt;");		
	}



	function ViewFlowChart() {
	   
	     if ($.browser.safari) {
	         window.open('<%=sApplicationUrl %>' + "/Forms/flow_View_Chart_SVG.aspx?MessageID=" + window.flowInfo.MessageID.value, "flowChart", "scrollbars=yes,status=yes ,resizable=yes,top=100,left=100,width=650,height=470");
	         return;
	     }
	     else {
	         window.open('<%=sApplicationUrl %>' + "/Forms/flow_View_Chart.aspx?MessageID=" + window.flowInfo.MessageID.value, "flowChart", "scrollbars=yes,status=yes ,resizable=yes,top=100,left=100,width=650,height=470");
	         return;
	     }	
		
	 }
	 
	 function GetJumpToNodeModel(iType)
	 {
	    
	    // alert(iType);
	     if ($.browser.msie) {
	         var retNodeModelID = window.showModalDialog("../Forms/flow_SelectNode.aspx?MessageID=" + window.flowInfo.MessageID.value + "&itype=" + iType, "flowChart11", "scrollbars=yes;status=yes;resizable=yes;center=no;dialogTop=20;dialogLeft=20;dialogWidth=45;dialogHeight=40");
	         if (retNodeModelID == "undefined") {
	           //  alert("");
	        
	             return "";
	         }
	         else {
	             //alert(retNodeModelID);
	             return retNodeModelID;
	         }
	     }
	     else {
	         window.open("../Forms/flow_SelectNode_svg.aspx?MessageID=" + window.flowInfo.MessageID.value + "&itype=" + iType, "flowChart11", "scrollbars=yes;status=yes;resizable=yes;center=no;dialogTop=20;dialogLeft=20;dialogWidth=45;dialogHeight=40");
	     }		
	 }
	 
	 
	  function SendFlowForAttemper(lngAttermperID,lngFlowModelID)
     {
            document.all.AttemperID.value = lngAttermperID;
            window.parent.header.flowInfo.action = "flow_sender.aspx?SelType=1&AttemperID=" + lngAttermperID + "&FlowModelID=" + lngFlowModelID;
	        window.parent.header.flowInfo.target="fraprocess";
	         
	        window.parent.header.flowInfo.submit();
     }

     function DialogBehaviorButton_JSClick() {
         
         SendFlowPub();
     }
//-->
		</script>
	</HEAD>
	<body>
		<form id="flowSubmit" onsubmit="return false;" action="flow_Normal_submit.aspx" method="post">
			<uc1:formsendmain id="FormSendMain1" runat="server"></uc1:formsendmain>
			<INPUT id='ExpectedLimit' type='hidden'	name='ExpectedLimit' value='0'>
			<INPUT id='WarningLimit' type='hidden'	name='WarningLimit' value='0'>
		</form>
		<form id="flowInfo" onsubmit="return false;" action="flow_sender.aspx" method="post">
			<uc1:formsend id="Formsend2" runat="server" SendType="Normal"></uc1:formsend>
			<INPUT id='DialogBehaviorButton' type='submit' style=" display:none;"	  onclick="javascript:DialogBehaviorButton_JSClick();">
			<input id ="DialogBehavior_Value" type='hidden' value="-1"	/>
		</form>
		
		<form id="Form1" method="post" runat="server">
			
		</form>
	</body>
</HTML>
