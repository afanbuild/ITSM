<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.OA_AddNew_Menu" Codebehind="OA_AddNew_Menu.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="FormSend" Src="../Controls/FormSend.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FormSendMain" Src="../Controls/FormSendMain.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Rise_AddNew_Menu</title>
		<script language="javascript" type="text/javascript" src="../Js/jscommon.js"> </script>
		<script language="javascript">
		
<!--
	
	var winOpin;
	var winAttach;

	 var winflowSender;
	 
	 
	 function SendFlowPub()
	 {
	     window.flowInfo.action = "flow_sender.aspx";
		flowInfo.target="fraprocess";
		flowInfo.submit();
		 
	 }
	 
	 
	 
	 function TempSave()
	 {
	    
		window.parent.main.GetFieldValuesForSave(0);
	 }
	 
	 //�ύ�����ݴ�
	 function TempSavePub()
	 {
		window.flowSubmit.SubmitType.value= "Save";
		FlowSubmitPub();
		
	 }
	 
	 // �ύ���̷���
	 function FlowSubmit()
	 {
		
		window.flowSubmit.SubmitType.value= "Send";
		FlowSubmitPub();
	 }
	 
	 // �ύ���̷���
	 function FlowSubmitPub()
	 {
		/// ȡ��ز�����ֵ ���⡢ģ�ͱ�� ����Ϣ�塢�����������Ա����������
		
		window.flowSubmit.Subject.value= window.flowInfo.Subject.value.trim();
		window.flowSubmit.FlowModelID.value= window.flowInfo.FlowModelID.value;
		window.flowSubmit.MessageID.value= window.flowInfo.MessageID.value;
		window.flowSubmit.ActionID.value= window.flowInfo.ActionID.value;
		
		//ע�� �� �ύǰ ���ڱ����ֶ�ֵ��XML����ֵ��ȡ�ĳ���  δ��  *****  û����Send ��click�¼��б�д ****
		window.flowSubmit.FormXMLValue.value= window.flowInfo.FormXMLValue.value;
		
		window.flowSubmit.OpinionValue.value= window.flowInfo.OpinionValue.value;
		
	
		window.flowSubmit.Importance.value= window.flowInfo.Importance.value;
		
		window.flowSubmit.Attachment.value= window.flowInfo.Attachment.value;

		window.flowSubmit.target='_parent';
		window.flowSubmit.submit();
	 }
	 
	 function XMLQuote(input)
	{
		return input.replace(/&/g,"&amp;").replace(/</g,"&lt;").replace(/>/g,"&gt;");		
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
		<form id="flowSubmit" onsubmit="return false;" action="flow_AddNew_submit.aspx" method="post">
			<uc1:formsendmain id="FormSendMain1" runat="server"></uc1:formsendmain>
			<INPUT id='PreMessageID'  type='hidden' name='PreMessageID' value='<%=Session["PreMessageID"]%>'>
			<INPUT id='FlowJoinType'  type='hidden'	name='FlowJoinType' value='<%=Session["FlowJoinType"]%>'>
			<INPUT id='ExpectedLimit' type='hidden' name='ExpectedLimit' value='0'>
			<INPUT id='WarningLimit' type='hidden' name='WarningLimit' value='0'>
		</form>
		<form id="flowInfo" onsubmit="return false;" action="flow_sender.aspx" method="post">
			<uc1:formsend id="Formsend2" runat="server"></uc1:formsend>
		    
		</form>
		<form id="Form1" method="post" runat="server">
		</form>
	</body>
</HTML>
