<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.CtrFlowSpecRight" Codebehind="CtrFlowSpecRight.ascx.cs" %>
<asp:Literal id="ltlFlowSpecRight" runat="server"></asp:Literal>
<script type="text/javascript">
<!--
	
	function DoFreeTakeOverCtr()  //����
	{
	     var blnContinue;
	    //���ж�ĸ��ҳ�ϵĹ�����֤
		if(typeof(DoMasterPublicValidate) != "undefined")
		{
			blnContinue = DoMasterPublicValidate(0,"");
			//alert(blnContinue);
		}
		
		//�ж�����ҳ���Զ�����֤
		if(typeof(DoUserValidate) != "undefined" && blnContinue != false)
		{
			blnContinue = DoUserValidate(0,"");
			//alert(blnContinue);
		}
		if(blnContinue != false)
		{
		    window.parent.header.flowInfo.SpecRightType.value = '25';
		    GetFieldValues(0);
		}
	}
	
	function DoFreeTransmitCtr()   //����
	{
		window.parent.header.flowInfo.SpecRightType.value = '60';
		GetFieldValues(0);
	}
	
	function DoFreeAssistCtr()   //Э��
	{
		window.parent.header.flowInfo.SpecRightType.value = '70';
		GetFieldValues(0);
	}
	
	function DoFreeCommunicCtr()   //��ͨ
	{
		window.parent.header.flowInfo.SpecRightType.value = '80';
		GetFieldValues(0);
	}
	
	function DoJumpCtr()    //��ת
	{
	
	    var blnContinue;
	    //���ж�ĸ��ҳ�ϵĹ�����֤
		if(typeof(DoMasterPublicValidate) != "undefined")
		{
			blnContinue = DoMasterPublicValidate(0,"");
			//alert(blnContinue);
		}
		//�ж�����ҳ���Զ�����֤
		if(typeof(DoUserValidate) != "undefined" && blnContinue != false)
		{
			blnContinue = DoUserValidate(0,"");
			//alert(blnContinue);
		}
		if(blnContinue != false)
		{
		    window.parent.header.flowInfo.SpecRightType.value = '40';
		    GetFieldValues(0);
		}
	}
	
	function DoTakeBackHasDoneCtr()  //����
	{
	    var blnContinue;
	    //���ж�ĸ��ҳ�ϵĹ�����֤
		if(typeof(DoMasterPublicValidate) != "undefined")
		{
			blnContinue = DoMasterPublicValidate(0,"");
			//alert(blnContinue);
		}
		
		//�ж�����ҳ���Զ�����֤
		if(typeof(DoUserValidate) != "undefined" && blnContinue != false)
		{
			blnContinue = DoUserValidate(0,"");
			//alert(blnContinue);
		}
		if(blnContinue != false)
		{
		    window.parent.header.flowInfo.SpecRightType.value = '90';
		    GetFieldValues(0);
		}
	}
	
	function DoTransfLimitValue(obj,type)
	{
	   
	   if(type==1)
	   {
			if(typeof(parent.header.flowSubmit.ExpectedLimit)!="undefined")
				parent.header.flowSubmit.ExpectedLimit.value = obj.value;
	   }     
	    if(type==2)
	   {
			if(typeof(parent.header.flowSubmit.WarningLimit)!="undefined")
				parent.header.flowSubmit.WarningLimit.value = obj.value;
	   }     
	}

	
//-->
</script>

