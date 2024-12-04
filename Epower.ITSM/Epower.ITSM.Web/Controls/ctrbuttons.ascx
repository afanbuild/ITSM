<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.CtrButtons" Codebehind="CtrButtons.ascx.cs" %>
<script type="text/javascript" language="javascript">
<!--

    function DoMainFlowPause(itype)
    {
         DoButtosControlPause(itype);
    }

        function DoMainFlowDelete()
    {
         if(!confirm("确认要删除吗"))
         {
            return false;
         }
         else
         {
             DoButtosControlDelete();
             if('<%=FromForms%>'=="close")
             {
                top.close();
             }
          }
    }
    
    function DoLeftReDoBack(lngNMID,lngNMType,lngNUser)
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
		    window.parent.header.flowInfo.SpecRightType.value = '92';
		    window.parent.header.flowInfo.JumpToNodeModel.value = lngNMID;
		    
		    window.parent.header.flowInfo.ReDoUserID.value = lngNUser;
		    window.parent.header.flowInfo.ReDoNMType.value = lngNMType;
		    
		    window.parent.header.flowSubmit.JumpToNodeModel.value = lngNMID;
		    GetFieldValues(0);
		}
	}
    
    
    function OpenAbortFlow(id)  //终止流程
    {
        var	value=window.showModalDialog('<%=sApplicationUrl %>' +"/Common/frmFlowDelete.aspx?isAbort=true&FlowID=" + id,window,"dialogHeight:230px;dialogWidth:320px");
        if(value!=null)
        {
            if(value[0]=="0") //成功
            {
                window.location = window.location;
                //event.returnValue = true;
            }
            else
                event.returnValue = false;
        }
        else
        {
            event.returnValue = false;
        }
    }
    
	function DoLeftSave()
	{
		var blnContinue=true;
	    
	    //判断内容页上暂存时自定义验证
		if(typeof(DoUserSaveValidate) != "undefined" && blnContinue != false)
		{
			blnContinue = DoUserSaveValidate();						
		}
		
		if(blnContinue != false)
		{		
		    window.parent.header.TempSave();
		}

	}
	
	function DoLeftFlowChart()
	{
		window.parent.header.ViewFlowChart();
	}
	function DoLeftReadOver()
	{
		window.parent.header.ReaderOver();
	}
	
	function DoLeftReadOver()
	{
		window.parent.header.ReaderOver();
	}
	
	function DoLeftBack()
	{
		window.parent.header.SendBack();
	}
	
	function DoLeftTakeBack()
	{
	var   DialogBehavior_Value   =   document.getElementById("DialogBehavior_Value"); 
	if (DialogBehavior_Value != null)
	{
	    document.getElementById("DialogBehavior_Value").value="-1";
	}
		window.parent.header.TakeBackFlow();
	}
	 // 2007-05-22 将接收按钮放到表单下部分　实现　苏康胜
//	function DoLeftReceive()
//	{
//	    window.parent.header.ReceiveMessage();
//	}
	
	function DoExit()
	{
	    //alert('<%=strExitUrl%>');
	    if('<%=strExitUrl%>'=='close')
	    {
	        window.top.close();
	    }
	    else
	    {
	        window.location = '<%=strExitUrl%>';
	    }
	}
	
	function DoViewFlow()
	{   
	    if(<%=MessageID %>==0)
	    {	     
	        if($.browser.safari) {
	            window.open('<%=sApplicationUrl %>' +"/Forms/flow_View_ChartModel_SVG.aspx?flowmodelid=" + <%=FlowModelID %> ,"" ,"scrollbars=yes,resizable=yes,top=0,left=200,width=800,height=600");	
	            return;
	        }
	        window.open('<%=sApplicationUrl %>' +"/Forms/flow_View_ChartModel.aspx?flowmodelid=" + <%=FlowModelID %> ,"" ,"scrollbars=yes,resizable=yes,top=0,left=200,width=800,height=600");	        
	    }
	    else
	    {
	        if("<%=IsSelfMode %>"=="false")
	            window.parent.header.ViewFlowChart();
	        else
	        {
	            //自助查询页面进入
	             if ($.browser.safari) {
	                 window.open('<%=sApplicationUrl %>' + "/Forms/flow_View_Chart_SVG.aspx?MessageID=<%=MessageID %>", "flowChart", "scrollbars=yes,status=yes ,resizable=yes,top=100,left=100,width=650,height=470");
	                 return;
	             }
	             else {
	                 window.open('<%=sApplicationUrl %>' + "/Forms/flow_View_Chart.aspx?MessageID=<%=MessageID %>", "flowChart", "scrollbars=yes,status=yes ,resizable=yes,top=100,left=100,width=650,height=470");
	                 return;
	             }	
	        }
	    }
	}
//-->
</script>

<asp:Literal id="ltlButtons" runat="server"></asp:Literal>
