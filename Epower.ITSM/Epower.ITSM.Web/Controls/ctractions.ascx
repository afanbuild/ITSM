<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.CtrActions" Codebehind="CtrActions.ascx.cs" %>
<script language="javascript">
<!--
	function DoActionsCtr(lngActionID,strActionName)
	{
		
		var blnContinue;
		//alert("dd");
		
		if(typeof(document.all.ctl00_hidActionName) != "undefined")
		{
		    document.all.ctl00_hidActionName.value = strActionName;
		}
		if(typeof(document.all.ctl00_hidActionID) != "undefined")
		{
		      document.all.ctl00_hidActionID.value = lngActionID;
		}
		
		//���ж�ĸ��ҳ�ϵĹ�����֤
		if(typeof(DoMasterPublicValidate) != "undefined")
		{
			blnContinue = DoMasterPublicValidate(lngActionID,strActionName);
			//alert(blnContinue);
			//DoUserValidate(lngActionID);
		}
		
		//�ж�����ҳ���Զ�����֤
		if(typeof(DoUserValidate) != "undefined" && blnContinue != false)
		{
			blnContinue = DoUserValidate(lngActionID,strActionName);
			//alert(blnContinue);
			//DoUserValidate(lngActionID);
		}
		
		if(blnContinue != false)
		{
		    
		    if (typeof(window.parent.header.flowInfo.SpecRightType)!="undefined")
		         window.parent.header.flowInfo.SpecRightType.value = '10';
		   
			GetFieldValues(lngActionID);
		}
	}
//-->
</script>

<asp:Literal id="ltlActions" runat="server"></asp:Literal>
