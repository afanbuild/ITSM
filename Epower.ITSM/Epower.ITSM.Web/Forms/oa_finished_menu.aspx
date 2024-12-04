<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.OA_Finished_menu" Codebehind="OA_Finished_menu.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>已办事项查看</title>
				<script type="text/javascript" src="../Js/jquery-1.7.2.min.js"></script>
		<script language="javascript">
		
		
<!--
	 var winOpin;
	var winAttach;
		
	 // 提交流程回收
	 function TakeBackFlow()
	 {
		window.flowTakeBack.target = '_parent';
		window.flowTakeBack.submit();
		
	 }
	 
	 
	 function ViewFlowChart() {
	     if ($.browser.safari) {
	         window.open('<%=sApplicationUrl %>' + "/Forms/flow_View_Chart_SVG.aspx?MessageID=" + window.flowTakeBack.MessageID.value, "flowChart", "scrollbars=yes,status=yes ,resizable=yes,top=100,left=100,width=650,height=470");
	         return;
	     }
	     else {
	         window.open('<%=sApplicationUrl %>' + "/Forms/flow_View_Chart.aspx?MessageID=" + window.flowTakeBack.MessageID.value, "flowChart", "scrollbars=yes,status=yes ,resizable=yes,top=100,left=100,width=650,height=470");
	         return;
	     }		

	 }
	 
	 function SendFlowForAttemper(lngAttermperID,lngFlowModelID)
     {
            document.all.AttemperID.value = lngAttermperID;
            window.parent.header.flowInfo.action = "flow_sender.aspx?SelType=1&AttemperID=" + lngAttermperID + "&FlowModelID=" + lngFlowModelID;
	        window.parent.header.flowInfo.target="fraprocess";
	         
	        window.parent.header.flowInfo.submit();
     } 
	 

	
//-->
		</script>
	</HEAD>
	<body>
		<form id="flowTakeBack" onsubmit="return false;" action="flow_Finished_submit.aspx" method="post">
			<input id="MessageID"  type=hidden value="<%=lngMessageID.ToString()%>" name="MessageID"/>
		</form>
		<form id="flowInfo" onsubmit="return false;" action="flow_Attachment.aspx" method="post">
			<INPUT id=Importance  type=hidden value="1" name=Importance>
            <INPUT id="FormXMLValue"  type=hidden value="" name="FormXMLValue">
            <INPUT id="FormDefineValue"  type=hidden value="" name="FormDefineValue">
            <INPUT id="Receivers"  type=hidden value="" name="Receivers">
            <INPUT id="LinkNodeID"  type=hidden value="0" name="LinkNodeID">
            <INPUT id="LinkNodeType"  type=hidden value="0" name="LinkNodeType">
            <INPUT id="AttemperID"  type=hidden value="0" name="AttemperID">
		</form>
		<form id="flow_Finished_menu" method="post" runat="server">
		</form>
	</body>
</HTML>
