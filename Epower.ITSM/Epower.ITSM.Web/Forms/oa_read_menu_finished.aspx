<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.OA_Read_menu_Finished" Codebehind="OA_Read_menu_Finished.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="flowReader" Src="../Controls/flowReader.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>flow_Read_menu_Finished</title>
		<script type="text/javascript" src="../Js/jquery-1.7.2.min.js"></script>
		<script language="javascript">
<!--
		  // 提交流程回收  //会签回收需要
		 function TakeBackFlow()
		 {
			window.flowInfo.action ="flow_Finished_submit.aspx";
			window.flowInfo.target = '_parent';
			window.flowInfo.submit(); 
		  }
		  
		function ViewFlowChart() {
		   if ($.browser.safari) {
		        window.open('<%=sApplicationUrl %>' + "/Forms/flow_View_Chart_SVG.aspx?MessageID=" + window.flowInfo.MessageID.value, "flowChart", "scrollbars=yes,status=yes ,resizable=yes,top=100,left=100,width=650,height=470");
	            return;
	      }
	      else
	      {
	          window.open('<%=sApplicationUrl %>' + "/Forms/flow_View_Chart.aspx?MessageID=" + window.flowInfo.MessageID.value, "flowChart", "scrollbars=yes,status=yes ,resizable=yes,top=100,left=100,width=650,height=470");
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
		<form id="flowInfo" onsubmit="return false;" action="flow_Reader_submit.aspx" method="post">
			<INPUT id="MessageID"  type=hidden value="<%=lngMessageID.ToString()%>" name="MessageID">
			<INPUT id=Importance  type=hidden value="1" name=Importance>
			<INPUT id="FormXMLValue" type=hidden value="" name="FormXMLValue">
			<INPUT id="FormDefineValue"  type=hidden value="" name="FormDefineValue">
            <INPUT id="Receivers"  type=hidden value="" name="Receivers">
            <INPUT id="LinkNodeID"  type=hidden value="0" name="LinkNodeID">
            <INPUT id="LinkNodeType"  type=hidden value="0" name="LinkNodeType">
            <INPUT id="AttemperID"  type=hidden value="0" name="AttemperID">
		</form>
		<form method="post" runat="server">
			<uc1:flowReader id="FlowReader1" runat="server" SendType="Finished"></uc1:flowReader>
		</form>
	</body>
</HTML>
