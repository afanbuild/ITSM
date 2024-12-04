<%@ Register TagPrefix="uc2" TagName="flowReader" Src="../Controls/flowReader.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.OA_Reader_menu" Codebehind="OA_Reader_menu.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>flow_Reader_menu</title>
		<script language="javascript">
<!--

		
		
		 var winOpin;
		 var winAttach;
		 
		 function SendFlowPub()
		 {
			ReaderOverPub('no','no');
		 
		 }
		 
		 function ReaderOverPub(isSaved,isCommon)
		 { 
		    
			window.flowInfo.action ="flow_Reader_submit.aspx?IsSaved=" + isSaved;
			
			//window.open('','flowReaderOver','scrollbars=yes,resizable=no'); 
		    
			
			//window.flowInfo.target='flowReaderOver'; 
			window.flowInfo.target = '_parent';
			window.flowInfo.submit(); 
		 } 
		 
		 
		  function ReceiveMessage()
		 { 
		    
			window.flowInfo.action ="flow_Receive_submit.aspx";
			
			//window.open('','flowReaderOver','scrollbars=yes,resizable=no'); 
		    
			
			//window.flowInfo.target='flowReaderOver'; 
			window.flowInfo.target = '_parent';
			window.flowInfo.submit(); 
		 } 
		 
		 
		 
		  function SendFlowForAttemper(lngAttermperID,lngFlowModelID)
         {
                document.all.AttemperID.value = lngAttermperID;
                window.parent.header.flowInfo.action = "flow_sender.aspx?SelType=1&AttemperID=" + lngAttermperID + "&FlowModelID=" + lngFlowModelID;
	            window.parent.header.flowInfo.target="fraprocess";
    	         
	            window.parent.header.flowInfo.submit();
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
		
//-->
		</script>
	</HEAD>
	<body>
		<form id="flowInfo" onsubmit="return false;" action="flow_Reader_submit.aspx" method="post">
			<INPUT id="MessageID"  type="hidden" value="<%=lngMessageID.ToString()%>" name="MessageID">
			<INPUT id="AppID"  type="hidden" value="<%=lngAppID.ToString()%>" name="AppID">
			<INPUT id="OpinionValue" type="hidden" value="<%=strOpinion%>" name="OpinionValue">
			<INPUT id="ActionID" type="hidden" value="-1" name="ActionID"> 
			<INPUT id="FormXMLValue"  type="hidden" value="" name="FormXMLValue">
			<INPUT id="FormDefineValue"  type="hidden" value="" name="FormDefineValue">
            <INPUT id="Receivers"  type="hidden" value="" name="Receivers">
            <INPUT id="LinkNodeID"  type="hidden" value="0" name="LinkNodeID">
            <INPUT id="LinkNodeType"  type="hidden" value="0" name="LinkNodeType">
            <INPUT id="AttemperID"  type="hidden" value="0" name="AttemperID">
			<INPUT id="Importance"  type="hidden" value="1" name="Importance">
			<INPUT id="Attachment"  type="hidden" value="<%=sAttXml%>" name="Attachment">
			<INPUT id="ActorType"  type="hidden" value="<%=sActorType%>" name="ActorType">
			<INPUT id="Subject"  type="hidden" value="" name="Subject">
		</form>
		<form id="flow_Reader_menu" method="post" runat="server">
			<uc2:flowReader id="FlowReader1" runat="server"></uc2:flowReader>
		</form>
	</body>
</HTML>
