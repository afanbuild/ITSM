<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.CtrFlowSpecRight" Codebehind="CtrFlowSpecRight.ascx.cs" %>
<asp:Literal id="ltlFlowSpecRight" runat="server"></asp:Literal>
<script type="text/javascript">
<!--
	
	function DoFreeTakeOverCtr()  //交接
	{
	     var blnContinue;
	    //先判断母版页上的公共验证
		if(typeof(DoMasterPublicValidate) != "undefined")
		{
			blnContinue = DoMasterPublicValidate(0,"");
			//alert(blnContinue);
		}
		
		//判断内容页上自定义验证
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
	
	function DoFreeTransmitCtr()   //传阅
	{
		window.parent.header.flowInfo.SpecRightType.value = '60';
		GetFieldValues(0);
	}
	
	function DoFreeAssistCtr()   //协作
	{
		window.parent.header.flowInfo.SpecRightType.value = '70';
		GetFieldValues(0);
	}
	
	function DoFreeCommunicCtr()   //沟通
	{
		window.parent.header.flowInfo.SpecRightType.value = '80';
		GetFieldValues(0);
	}
	
	function DoJumpCtr()    //跳转
	{
	
	    var blnContinue;
	    //先判断母版页上的公共验证
		if(typeof(DoMasterPublicValidate) != "undefined")
		{
			blnContinue = DoMasterPublicValidate(0,"");
			//alert(blnContinue);
		}
		//判断内容页上自定义验证
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
	
	function DoTakeBackHasDoneCtr()  //驳回
	{
	    var blnContinue;
	    //先判断母版页上的公共验证
		if(typeof(DoMasterPublicValidate) != "undefined")
		{
			blnContinue = DoMasterPublicValidate(0,"");
			//alert(blnContinue);
		}
		
		//判断内容页上自定义验证
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

