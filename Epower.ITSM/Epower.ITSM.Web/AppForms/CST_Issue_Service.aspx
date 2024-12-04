﻿<%@ Page Language="C#" MasterPageFile="~/FlowForms.Master" AutoEventWireup="true" ValidateRequest="false"
CodeBehind="CST_Issue_Service.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.CST_Issue_Service" 
Title="故障单登记" %>


<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc8" %>
<%@ Register Src="../Controls/ServiceStaff.ascx" TagName="ServiceStaff" TagPrefix="uc7" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/CtrMonitor.ascx" TagName="CtrMonitor" TagPrefix="uc5" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc4" %>
<%@ Register Src="../Controls/common/DictionaryPicker.ascx" TagName="DictionaryPicker"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/ctrdateandtimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc1" %>
<%@ Register Src="../Controls/CtrFeedBack.ascx" TagName="CtrFeedBack" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ServiceStaffMastCust.ascx" TagName="ServiceStaffMastCust"
    TagPrefix="uc9" %>
<%@ Register Src="../Controls/ctrbuttons.ascx" TagName="ctrbuttons" TagPrefix="uc10" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc11" %>
<%@ Register Src="../Controls/CtrFlowRemark.ascx" TagName="CtrFlowRemark" TagPrefix="uc12" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <script language="javascript" src="../Js/jShowDiv.js"></script>

    <script language="javascript" src="../Js/jUtility.js"></script>
    <link  rel="stylesheet" type="text/css" href="../Js/themes/jquery.autocomplete.css" />
   <script type="text/javascript" src="../Js/Plugin/jquery.autocomplete.js"></script>
    <script language="javascript">

function TransferValue()
{
//拼凑待办事项标题
   if (typeof(document.all.<%=CtrFlowFTSubject.ClientID%>)!="undefined" )
   {
      if(document.all.<%=CtrFlowFTSubject.ClientID%>.value.trim()!="")
      {
	        parent.header.flowInfo.Subject.value = document.all.<%=CtrFlowFTSubject.ClientID%>.value.trim();
	  }
   }
}

//打印
function printdiv()
{
    var flowid="<%=FlowID%>";
    var AppID="<%=AppID%>";
    var FlowMoldelID="<%=FlowModelID%>";
    window.open("../Print/printRule.aspx?FlowId="+flowid+"&AppID="+AppID+"&FlowMoldelID="+FlowMoldelID,'','toolbar=no,menubar=no,status=yes,resizable=yes,tilebar=yes,scrollbars=yes');
    return false;
}

function next() { if (event.keyCode==13) event.keyCode=9;} 

function stringtrim(str){
		while (str.charAt(0)==' ')
			str=str.substr(1);
		while (str.charAt(str.length-1)==' ')
			str=str.substr(0,str.length-1);
		return str;
		
	}

function enterkey(obj,name)
            {
                var className;
	            var objectFullName;
	            var tmepName;
	            var obj;
	            objectFullName = obj.id;
                className = objectFullName.substring(0,objectFullName.indexOf(name)-1);
                tmepName = className.substr(className.indexOf("gvBillItem"),className.length-className.indexOf("gvBillItem"));
                i = tmepName.substr(tmepName.indexOf("ctl")+3,tmepName.length-tmepName.indexOf("ctl"));
                j = eval(i);
                
                if(event.keyCode==40)
                {
                    j = j+1;
                    
                     if(j<10)
                   {   
					    obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_" +name);
					}
					else
					{
					    obj = document.all.item(className.substr(0,className.length-i.length)+j+"_" +name);
					}
                    
                    if (obj !=null)
                    {  
                        obj.focus(); 
                        obj.select();
                    }
                }
                //向上键




                if(event.keyCode==38)
                {
                    j = j-1;
                    if(j<10)
                   {  
					    obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_" +name);
					}
					else
					{
					    obj = document.all.item(className.substr(0,className.length-i.length)+j+"_" +name);
					}
                    
                    if (obj !=null)
                    {  
                        obj.focus(); 
                        obj.select();
                    }
                }
                
                //回车
                if(event.keyCode == 13)
                {
                  
                    if(name=="txtDFareName")
                    {
                        if(j<10)
                       {  
					        obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtDModelName");
					    }
					    else
					    {
					        obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtDModelName");
					    }
                    }
                    if(name=="txtDModelName")
                    {
                        if(j<10)
                       {  
					        obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtDQuantity");
					    }
					    else
					    {
					        obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtDQuantity");
					    }
                    }
                    
                    if(name=="txtDQuantity")
                    {
                        if(j<10)
                       {  
					        obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtDPrice");
					    }
					    else
					    {
					        obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtDPrice");
					    }
                    }
                    
                    if(name=="txtDPrice")
                    {
                        if(j<10)
                       {  
					        obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtDHumanAmount");
					    }
					    else
					    {
					        obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtDHumanAmount");
					    }
                    }
                    
                    if(name=="txtDFareAmount")
                    {
                        if(j<10)
                       {  
					        obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtDHumanAmount");
					    }
					    else
					    {
					        obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtDHumanAmount");
					    }
                    }
                    
                    if(name=="txtDHumanAmount")
                    {
                        if(j<10)
                       {  
					        obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtDRemark");
					    }
					    else
					    {
					        obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtDRemark");
					    }
                    }
                    
                    if(name=="txtDRemark")
                    {
                        if(j<10)
                       {  
					        obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+1+"_txtDFareName");
					    }
					    else
					    {
					        obj = document.all.item(className.substr(0,className.length-i.length)+j+1+"_txtDFareName");
					    }
                    }
                    
                    //新增
                    if(name=="txtAddDFareName")
                    {
                        if(j<10)
                       {  
					        obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtAddDModelName");
					    }
					    else
					    {
					        obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtAddDModelName");
					    }
                    }
                    if(name=="txtAddDModelName")
                    {
                        if(j<10)
                       {  
					        obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtAddDQuantity");
					    }
					    else
					    {
					        obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtAddDQuantity");
					    }
                    }
                    
                    if(name=="txtAddDQuantity")
                    {
                        if(j<10)
                       {  
					        obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtAddDHumanAmount");
					    }
					    else
					    {
					        obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtAddDHumanAmount");
					    }
                    }
                    
                    if(name=="txtAddDFareAmount")
                    {
                        if(j<10)
                       {  
					        obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtAddDHumanAmount");
					    }
					    else
					    {
					        obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtAddDHumanAmount");
					    }
                    }
                    
                    if(name=="txtAddDHumanAmount")
                    {
                        if(j<10)
                       {  
					        obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtAddDRemark");
					    }
					    else
					    {
					        obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtAddDRemark");
					    }
                    }
                    
                    if (obj !=null)
                    {  
                        
                        obj.focus(); 
                        obj.select();
                    }
                    event.returnValue=false; 
                }
                
                return false;
            }
            
            //检查是否是正确的数值，并计算结果




		    function CheckIsnum(obj,name)
			{
			    
			    var svalue = obj.value;
			    if (isNaN(svalue))
			    {
			        alert("输入必须为数值类型！");
			        obj.focus(); 
                    obj.select(obj.value.length);
                    
			   }
			   
			   if(name != "txtDQuantity")
			   {
			        if(svalue!="")
			        {
					    obj.value = eval(svalue).toFixed(2);
					}
			   }
			    var objOld;
			    var className;
	            var objectFullName;
	            var tmepName;
	            var obj;
	            var tempQuantity;
	            var tempPrice;
	            var tempFareAmount;
	            var tempHumanAmount;
	            var tempTotalAmount;
	            objOld = obj;
	            objectFullName = obj.id;
                className = objectFullName.substring(0,objectFullName.indexOf(name)-1);
                tmepName = className.substr(className.indexOf("gvBillItem"),className.length-className.indexOf("gvBillItem"));
                i = tmepName.substr(tmepName.indexOf("ctl")+3,tmepName.length-tmepName.indexOf("ctl"));
                j = eval(i);
                 if(j<10)
                   {  
					    obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtDQuantity");
					}
					else
					{
					    obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtDQuantity");
					}
                                
                 if (obj !=null)
                 {
                    if(obj.value != "")
                    {
						tempQuantity = eval(obj.value);
					}
					else
					{
					   tempQuantity = 0;
					}
                 }
                 else
                 {
                    tempQuantity = 0;
                 }
                 if(j<10)
                   {  
					    obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtDPrice");
					}
					else
					{
					    obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtDPrice");
					}
                if (obj !=null)
                 {
					if(obj.value != "")
                    {
						tempPrice = eval(obj.value);
					 }
					else
					{
						tempPrice = 0;
					}
                 }
                 else
                 {
                    tempPrice = 0;
                 }
                 
                 tempFareAmount = tempQuantity * tempPrice;
                 
                  if(j<10)
                   {  
					    obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtDHumanAmount");
					}
					else
					{
					    obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtDHumanAmount");
					}
                if (obj !=null)
                 {
                    if(obj.value != "")
                    {
						tempHumanAmount = eval(obj.value);
					 }
					else
					{
						tempHumanAmount = 0;
					}
                 }
                 else
                 {
                    tempHumanAmount = 0;
                 }
                 
                 tempTotalAmount = tempHumanAmount + tempFareAmount;
                 
                 
                  if(j<10)
                   {  
					    obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtDFareAmount");
					}
					else
					{
					    obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtDFareAmount");
					}
                 
			    if (obj !=null)
                 {
                    obj.value = (tempFareAmount).toFixed(2);
                 }
                 
                 
                  if(j<10)
                   {  
					    obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtDTotalAmount");
					}
					else
					{
					    obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtDTotalAmount");
					}
                 
			    if (obj !=null)
                 {
                    obj.value = (tempTotalAmount).toFixed(2);
                 }
                 
                 //计算合计金额
                 
                 CalcAmount(objOld,name);
			   
			}
			
			  function CheckIsnumAdd(obj,name)
			{
			    
			    var svalue = obj.value;
			    if (isNaN(svalue))
			    {
			        alert("输入必须为数值类型！");
			        obj.focus(); 
                    obj.select(obj.value.length);
                    
			   }
			   if(name != "txtAddDQuantity")
			   {
					if(svalue!="")
			        {
					    obj.value = eval(svalue).toFixed(2);
					}
			   }
			    var objOld;
			    var className;
	            var objectFullName;
	            var tmepName;
	            var obj;
	            var tempQuantity;
	            var tempPrice;
	            var tempFareAmount;
	            var tempHumanAmount;
	            var tempTotalAmount;
	            objOld = obj;
	            objectFullName = obj.id;
                className = objectFullName.substring(0,objectFullName.indexOf(name)-1);
                tmepName = className.substr(className.indexOf("gvBillItem"),className.length-className.indexOf("gvBillItem"));
                i = tmepName.substr(tmepName.indexOf("ctl")+3,tmepName.length-tmepName.indexOf("ctl"));
                j = eval(i);
                
                 if(j<10)
                   {  
					    obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtAddDQuantity");
					}
					else
					{
					    obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtAddDQuantity");
					}
                                
                 if (obj !=null)
                 {
                    if(obj.value != "")
                    {
						tempQuantity = eval(obj.value);
					}
					else
					{
					   tempQuantity = 0;
					}
                 }
                 else
                 {
                    tempQuantity = 0;
                 }
                 if(j<10)
                   {  
					    obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtAddDPrice");
					}
					else
					{
					    obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtAddDPrice");
					}
                if (obj !=null)
                 {
					if(obj.value != "")
                    {
						tempPrice = eval(obj.value);
					 }
					else
					{
						tempPrice = 0;
					}
                 }
                 else
                 {
                    tempPrice = 0;
                 }
                 
                 tempFareAmount = tempQuantity * tempPrice;
                 
                  if(j<10)
                   {  
					    obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtAddDHumanAmount");
					}
					else
					{
					    obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtAddDHumanAmount");
					}
                if (obj !=null)
                 {
                    if(obj.value != "")
                    {
						tempHumanAmount = eval(obj.value);
					 }
					else
					{
						tempHumanAmount = 0;
					}
                 }
                 else
                 {
                    tempHumanAmount = 0;
                 }
                 
                 tempTotalAmount = tempHumanAmount + tempFareAmount;
                 
                 
                  if(j<10)
                   {  
					    obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtAddDFareAmount");
					}
					else
					{
					    obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtAddDFareAmount");
					}
                 
			    if (obj !=null)
                 {
                    obj.value = (tempFareAmount).toFixed(2);
                 }
                 
                 
                  if(j<10)
                   {  
					    obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtAddDTotalAmount");
					}
					else
					{
					    obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtAddDTotalAmount");
					}
                 
			    if (obj !=null)
                 {
                    obj.value = (tempTotalAmount).toFixed(2);
                 }
                 
                 //计算合计金额
                 
                 CalcAmount(objOld,name);
			   
			}
			
			function CalcAmount(obj,name)
			{
				var className;
	            var objectFullName;
	            var tmepName;
	            var obj;
	            var tempFareName;
	            var tempAmount;
	            var tempTotalAmount;
	         
	            
	            
	            tempTotalAmount = 0;
	           
	            objectFullName = obj.id;
                className = objectFullName.substring(0,objectFullName.indexOf(name)-1);
                tmepName = className.substr(className.indexOf("gvBillItem"),className.length-className.indexOf("gvBillItem"));
                i = tmepName.substr(tmepName.indexOf("ctl")+3,tmepName.length-tmepName.indexOf("ctl"));
                j = 2;
   
                while(true)
                {
                   if(j<10)
                   {  
				        obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtDFareName");
				    }
				    else
				    {
				        obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtDFareName");
				    }
					if (obj !=null)
					{
						tempFareName = obj.value;
						
					}
					else
					{
						tempFareName = "";
						break;   //找不到控件时退出去
					}

					if(tempFareName != "" )
					{
					   //日期为空不处理




					  if(j<10)
                       {  
					        obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtDTotalAmount");
					    }
					    else
					    {
					        obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtDTotalAmount");
					    }
					  
						if (obj !=null)
						{
							if(obj.value != "")
							{
								tempAmount = eval(obj.value);
							}
							else
							{
								tempAmount = 0;
							}
						}
						else
						{
							tempAmount = 0;
							break;   //找不到控件时退出去
						}
               
						tempTotalAmount += tempAmount;

                    }
                    j++;
                    
                 }
                 
                 if(name.substr(0,6)=="txtAdd")
                 {
                        if(j<10)
                       {  
					        obj = document.all.item(className.substr(0,className.length-i.length)+"0"+j+"_txtAddDTotalAmount");
					    }
					    else
					    {
					        obj = document.all.item(className.substr(0,className.length-i.length)+j+"_txtAddDTotalAmount");
					    }
					    
					    if (obj !=null)
						{
							if(obj.value != "")
							{
								tempTotalAmount += eval(obj.value);
							}
						}
                 }
                
                 document.all.<%=labTotalAmount.ClientID%>.innerText = tempTotalAmount.toFixed(2);
			}

            
            //控制非数字输入

		    function NumberInputlocal()
            {
                if(((event.keyCode<48)||(event.keyCode>57))) 
                if ((event.keyCode !=37) & (event.keyCode !=39) & (event.keyCode !=8) 
                    & (event.keyCode !=190) & (event.keyCode !=9) & (event.keyCode !=189) & (event.keyCode !=109) & ((event.keyCode<96) || (event.keyCode >110)))
                    event.returnValue=false;    
            } 
            
            
            function CalcuteTotalHours()
            {
              try{
                    var sFinished,sStart;
                    var sFinishH,sFinishM,sStartH,sStartM;
                    if(typeof(document.all.ctl00_ContentPlaceHolder1_CtrDTFinishedTime_txtDate) != "undefined")
                    {
                      
                        sFinished = document.all.ctl00_ContentPlaceHolder1_CtrDTFinishedTime_txtDate.value;
                        sFinishH = document.all.ctl00_ContentPlaceHolder1_CtrDTFinishedTime_ddlHours.value;
                        sFinishM = document.all.ctl00_ContentPlaceHolder1_CtrDTFinishedTime_ddlMinutes.value;
                        
                    }
                    else if(typeof(document.all.ctl00_ContentPlaceHolder1_CtrDTFinishedTime_labDate) !="undefined")
                    {
                        if(document.all.ctl00_ContentPlaceHolder1_CtrDTFinishedTime_labDate.innerText != "--")
                        {
                            sFinished = document.all.ctl00_ContentPlaceHolder1_CtrDTFinishedTime_labDate.innerText.substring(0,10);
                            sFinishH = document.all.ctl00_ContentPlaceHolder1_CtrDTFinishedTime_labDate.innerText.substring(11,13);
                            sFinishM = document.all.ctl00_ContentPlaceHolder1_CtrDTFinishedTime_labDate.innerText.substring(14,16);
                        }
                        else
                        {
                           
                           sFinished = "";
                        }
                    }
                    
                    
                    
                     if(typeof(document.all.ctl00_ContentPlaceHolder1_CtrDTOutTime_txtDate) != "undefined")
                    {                       
                         sStart = document.all.ctl00_ContentPlaceHolder1_CtrDTOutTime_txtDate.value;
                        sStartH = document.all.ctl00_ContentPlaceHolder1_CtrDTOutTime_ddlHours.value;
                        sStartM = document.all.ctl00_ContentPlaceHolder1_CtrDTOutTime_ddlMinutes.value;
                        
                    }
                    else if(typeof(document.all.ctl00_ContentPlaceHolder1_CtrDTOutTime_labDate) !="undefined")
                    {
                       if(document.all.ctl00_ContentPlaceHolder1_CtrDTOutTime_labDate.innerText != "--")
                       {
                            sStart = document.all.ctl00_ContentPlaceHolder1_CtrDTOutTime_labDate.innerText.substring(0,10);
                             sStartH = document.all.ctl00_ContentPlaceHolder1_CtrDTOutTime_labDate.innerText.substring(11,13);
                            sStartM = document.all.ctl00_ContentPlaceHolder1_CtrDTOutTime_labDate.innerText.substring(14,16);
                        }
                        else
                        {
                             sStart = "";
                        }
                    }
                    if( sStart !="" && sFinished != "")
                    {
                        var now=new Date(sFinished.substring(0,4),parseInt(sFinished.substring(5,7))-1,sFinished.substring(8,10),sFinishH,sFinishM);
                        var old=new Date(sStart.substring(0,4),parseInt(sStart.substring(5,7))-1,sStart.substring(8,10),sStartH,sStartM);
                       
                       if(now.getTime()-old.getTime() < 0)
                       {
                            alert("处理完成时间不能小于派出时间!");
                           
                       }
                       
                       document.all.ctl00_ContentPlaceHolder1_labTotalHours.innerText = Math.abs(old.getTime()-now.getTime())/(60*60*1000).toFixed(2);                                              
                   }
               }catch(e)
               {
                 return false;
               }
            }
			
			//脚本检查是否输入




			 function DoUserValidate(lngActionID,strActionName)
	        {
	            TransferValue();
			    return CheckCustAndType();
		    }
		    
		
            //检查是否选择了事件来源和事件类型
			function CheckCustAndType()
			{
			    return true;
			}
			
			String.prototype.trim = function()  //去空格

			{
				// 用正则表达式将前后空格

				// 用空字符串替代。

				return this.replace(/(^\s*)|(\s*$)/g, "").replace("&#160;","");
			}
		    //新增客户信息
		    function getCustomId(obj)
            {
                var txtCustName = document.all.<%=txtCustAddr.ClientID%>.value;
                var txtTell = document.all.<%=txtCTel.ClientID%>.value;
                var lblCustDeptName = document.all.<%=lblCustDeptName.ClientID%>.outerText;
                var lblEmail = document.all.<%=lblEmail.ClientID%>.outerText;
                var lblMastCust = document.all.<%=lblMastCust.ClientID%>.outerText;
                var txtAddr = document.all.<%=txtAddr.ClientID%>.value;
                var txtContact ="";
                var lbljob ="";                
                var url="frmBr_ECustomerEdit.aspx?Page=IssueBase&randomid="+GetRandom()+"&CustName=" + escape(txtCustName) + "&Addr=" + escape(txtAddr) + "&Contact=" + escape(txtContact)
                    + "&Tell="+ escape(txtTell) + "&CustDeptName=" + escape(lblCustDeptName) + "&Email=" + escape(lblEmail) + "&MastCust=" 
                    + escape(lblMastCust) +"&job=" + escape(lbljob)+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=CTS_Issue_Service";
                window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=600");                        
            }            
         
            //新增资产信息
            function AddEqu(obj)
            {
                var dCustID =document.all.<%=hidCustID.ClientID %>.value;
                var txtCustName = "";
                if(dCustID != "")
                {
                    txtCustName = document.all.<%=txtCustAddr.ClientID%>.value;
                }
                var txtEquName = document.all.<%=txtEqu.ClientID %>.value;
                var txtEquPos = document.all.<%=txtListName.ClientID %>.value;
                var url="../EquipmentManager/frmEqu_DeskEdit.aspx?subjectid=1&randomid="+GetRandom()+"&Page=IssueBase&EquName=" + escape(txtEquName) + "&EquPos=" 
                + escape(txtEquPos)+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=CST_Issue_Service";
                window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=600");
                
            }
		
		//客户选择
			function CustomSelect(obj) 
			{
			    var CustName = document.all.<%=txtCustAddr.ClientID%>.value;
			    var CustAddress = document.all.<%=txtAddr.ClientID%>.value;
			    var CustLinkMan = "";
			    var CustTel = document.all.<%=txtCTel.ClientID%>.value;
			    
			    var url="../Common/frmDRMUserSelectajax.aspx?IsSelect=true&randomid="+GetRandom()+"&CustID=" + document.getElementById(obj.id.replace('cmdCust','hidCustID')).value 
				    + "&FlowID="+ '<%=FlowID%>' + "&CustName=" + escape(CustName)  + "&CustAddress=" + escape(CustAddress)
				    + "&CustLinkMan=" + escape(CustLinkMan) + "&CustTel=" + escape(CustTel)+" &Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrom=CST_Issue_Service";
				    window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=600");				 
			}
			
			function SelectSomeCust(obj)   //选择多个客户
			{
			    var newDateObj = new Date()
	            var sparamvalue =  newDateObj.getYear().toString() + newDateObj.getMonth().toString();
	            var name = obj.value.trim();
	            if(name.trim()=="")
	            {
	                return;
	            }
				var	value=window.showModalDialog("../mydestop/frmQuickLocateCustAjax.aspx?IsSelect=true&randomid="+GetRandom()+"&Name=" + escape(name),"","dialogHeight:500px;dialogWidth:900px");
				if(value != null)
				{
				    var json = value;
				    var record=json.record;
				    
					for(var i=0; i < record.length; i++)
					{
			            document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr")).value = record[i].shortname;   //客户
			            document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = record[i].shortname;
			            document.getElementById(obj.id.replace("txtCustAddr","txtAddr")).value = record[i].address;   //地址
			            document.getElementById(obj.id.replace("txtCustAddr","hidAddr") ).value = record[i].address;   //地址
 
			            document.getElementById(obj.id.replace("txtCustAddr","txtCTel")).value = record[i].tel1;   //联系人电话



			            document.getElementById(obj.id.replace("txtCustAddr","hidTel")).value = record[i].tel1;
			            document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = record[i].id;    //客户ID号


			           
			            document.getElementById(obj.id.replace("txtCustAddr","lblCustDeptName")).innerHTML = record[i].custdeptname;   //所属部门

		                document.getElementById(obj.id.replace("txtCustAddr","lblEmail")).innerHTML = record[i].email;  //电子邮件
		                document.getElementById(obj.id.replace("txtCustAddr","lblMastCust")).innerHTML = record[i].mastcustname;   //服务单位
		                document.getElementById(obj.id.replace("txtCustAddr","hidCustDeptName")).value = record[i].custdeptname;   //所属部门

		                document.getElementById(obj.id.replace("txtCustAddr","hidCustEmail")).value = record[i].email;  //电子邮件
		                document.getElementById(obj.id.replace("txtCustAddr","hidMastCust")).value = record[i].mastcustname;   //服务单位

			            document.getElementById(obj.id.replace("txtCustAddr","txtCustMobile")).value = record[i].mobile;   //手机号码
			            document.getElementById(obj.id.replace("txtCustAddr","hidCustMobile") ).value = record[i].mobile;   //手机号码
			            document.getElementById(obj.id.replace("txtCustAddr","hidCustAreaID") ).value = record[i].custareaid;   //客户片区
			            document.getElementById(obj.id.replace("txtCustAddr","hidCustArea") ).value = record[i].custarea;   //客户片区

			            if (typeof(document.all.<%=Table3.ClientID%>)!="undefined" )
			            {
			                document.getElementById(obj.id.replace("txtCustAddr","txtEqu")).value = record[i].equname;   //资产名称
                            document.getElementById(obj.id.replace("txtCustAddr","hidEquName")).value = record[i].equname;   //资产名称
                            document.getElementById(obj.id.replace("txtCustAddr","hidEqu") ).value = record[i].equid;  //资产IDontact")).id);                            
                            
                            document.getElementById(obj.id.replace("txtCustAddr","txtListName")).value = record[i].listname;   //资产目录
                            document.getElementById(obj.id.replace("txtCustAddr","hidListName")).value = record[i].listname;   //资产目录
                            document.getElementById(obj.id.replace("txtCustAddr","hidListID") ).value = record[i].listid;  //资产目录ID
                            
                        }               
					}
				}
				else
				{
				     document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = "";    //客户ID号	
			         document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr") ).value="";//客户名称	
			                                                
			         document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = "";
			         document.getElementById(obj.id.replace("txtCustAddr","txtAddr")).value = "";   //地址
			         document.getElementById(obj.id.replace("txtCustAddr","hidAddr") ).value = "";   //地址 
			         document.getElementById(obj.id.replace("txtCustAddr","txtCTel")).value = "";   //联系人电话

			         document.getElementById(obj.id.replace("txtCustAddr","hidTel")).value = "";			                                                            
			         document.getElementById(obj.id.replace("txtCustAddr","lblCustDeptName")).innerHTML = "";   //所属部门

		             document.getElementById(obj.id.replace("txtCustAddr","lblEmail")).innerHTML = "";  //电子邮件
		             document.getElementById(obj.id.replace("txtCustAddr","lblMastCust")).innerHTML = "";   //服务单位
		             document.getElementById(obj.id.replace("txtCustAddr","hidCustDeptName")).value = "";   //所属部门

		             document.getElementById(obj.id.replace("txtCustAddr","hidCustEmail")).value = "";  //电子邮件
		             document.getElementById(obj.id.replace("txtCustAddr","hidMastCust")).value = "";   //服务单位

			         document.getElementById(obj.id.replace("txtCustAddr","txtCustMobile")).value = "";   //手机号码
			         document.getElementById(obj.id.replace("txtCustAddr","hidCustMobile") ).value = "";   //手机号码
			         document.getElementById(obj.id.replace("txtCustAddr","hidCustAreaID") ).value = "0";   //客户片区
			         document.getElementById(obj.id.replace("txtCustAddr","hidCustArea") ).value = "";   //客户片区
				}
			}
		
		var xmlhttp = null;
        function CreateXmlHttpObject()
        {
			try  
			{  
				xmlhttp = new ActiveXObject("MSXML2.XMLHTTP");  
			}  
			catch(e)  
			{  
				try  
				{  
					xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");  
				}  
				catch(e2){}  
			}
			return xmlhttp;
        }
        
        function GetCustID(obj)
        {
            if(obj.value.trim()=="")
	            {
	                document.getElementById(obj.id.replace("txtCustAddr","hidCustID") ).value="";
	                document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr") ).value="";//客户名称	
			        document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = "";
			        document.getElementById(obj.id.replace("txtCustAddr","txtAddr")).value = "";   //地址
			        document.getElementById(obj.id.replace("txtCustAddr","hidAddr") ).value = "";   //地址 
			        document.getElementById(obj.id.replace("txtCustAddr","txtCTel")).value = "";   //联系人电话

			        document.getElementById(obj.id.replace("txtCustAddr","hidTel")).value = "";			                                                            
			        document.getElementById(obj.id.replace("txtCustAddr","lblCustDeptName")).innerHTML = "";   //所属部门

		            document.getElementById(obj.id.replace("txtCustAddr","lblEmail")).innerHTML = "";  //电子邮件
		            document.getElementById(obj.id.replace("txtCustAddr","lblMastCust")).innerHTML = "";   //服务单位
		            document.getElementById(obj.id.replace("txtCustAddr","hidCustDeptName")).value = "";   //所属部门

		            document.getElementById(obj.id.replace("txtCustAddr","hidCustEmail")).value = "";  //电子邮件
		            document.getElementById(obj.id.replace("txtCustAddr","hidMastCust")).value = "";   //服务单位
			        document.getElementById(obj.id.replace("txtCustAddr","txtCustMobile")).value = "";   //手机号码
			        document.getElementById(obj.id.replace("txtCustAddr","hidCustMobile") ).value = "";   //手机号码
			        document.getElementById(obj.id.replace("txtCustAddr","hidCustAreaID") ).value = "0";   //客户片区
			        document.getElementById(obj.id.replace("txtCustAddr","hidCustArea") ).value = "";   //客户片区			        
	                return;
	            }
	        if(obj.value.trim()==document.getElementById(obj.id.replace("txtCustAddr","hidCust")).value.trim() && (document.getElementById(obj.id.replace("txtCustAddr","hidCustID") ).value.trim()!="" || document.getElementById(obj.id.replace("txtCustAddr","hidCustID") ).value.trim()!="0"))
	            {
	                return;
	            }
            if(xmlhttp == null)
                xmlhttp = CreateXmlHttpObject();       
            if(xmlhttp != null)
            {
                try
                {	
					xmlhttp.open("GET", "../MyDestop/frmXmlHttpAjax.aspx?randomid="+GetRandom()+"&Cust=" + escape(obj.value), true); 
                    xmlhttp.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
					xmlhttp.onreadystatechange = function() 
												{ 
													if ( xmlhttp.readyState==4 ) 
													{ 
														if(xmlhttp.responseText=="-1")  //没有
														{														   
														   // document.getElementById(obj.id.replace("txtCustAddr","hidCustID") ).value.trim()!="0"
														    alert("此客户不存在！");	
			                                                document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = "";    //客户ID号	
			                                                //document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr") ).value="";//客户名称	
			                                                
			                                                document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = "";
			                                                document.getElementById(obj.id.replace("txtCustAddr","txtAddr")).value = "";   //地址
			                                                document.getElementById(obj.id.replace("txtCustAddr","hidAddr") ).value = "";   //地址
//			                                                document.getElementById(obj.id.replace("txtCustAddr","txtContact")).value = "";    //联系人

//			                                                document.getElementById(obj.id.replace("txtCustAddr","hidContact")).value = "";   
			                                                document.getElementById(obj.id.replace("txtCustAddr","txtCTel")).value = "";   //联系人电话

			                                                document.getElementById(obj.id.replace("txtCustAddr","hidTel")).value = "";			                                                            
			                                                document.getElementById(obj.id.replace("txtCustAddr","lblCustDeptName")).innerHTML = "";   //所属部门

		                                                    document.getElementById(obj.id.replace("txtCustAddr","lblEmail")).innerHTML = "";  //电子邮件
		                                                    document.getElementById(obj.id.replace("txtCustAddr","lblMastCust")).innerHTML = "";   //服务单位
		                                                    document.getElementById(obj.id.replace("txtCustAddr","hidCustDeptName")).value = "";   //所属部门

		                                                    document.getElementById(obj.id.replace("txtCustAddr","hidCustEmail")).value = "";  //电子邮件
		                                                    document.getElementById(obj.id.replace("txtCustAddr","hidMastCust")).value = "";   //服务单位
//		                                                    document.getElementById(obj.id.replace("txtCustAddr","lbljob")).innerHTML = "";   //职位
//		                                                    document.getElementById(obj.id.replace("txtCustAddr","hidjob")).value = "";   //职位

			                                                document.getElementById(obj.id.replace("txtCustAddr","txtCustMobile")).value = "";   //手机号码
			                                                document.getElementById(obj.id.replace("txtCustAddr","hidCustMobile") ).value = "";   //手机号码
			                                                document.getElementById(obj.id.replace("txtCustAddr","hidCustAreaID") ).value = "0";   //客户片区
			                                                document.getElementById(obj.id.replace("txtCustAddr","hidCustArea") ).value = "";   //客户片区
			                                                //document.getElementById(obj.id.replace("txtCustAddr","lblCustArea") ).innerHTML = "";   //客户片区
			                                                //obj.focus();
														    //obj.select();                                                
			                                               	                                         
													    }
													    else if(xmlhttp.responseText=="0") //找到多个
													    {
													        SelectSomeCust(obj);
													    }
													    else  //找到唯一
													    {
													        
													            
													      	    var json= eval("("+xmlhttp.responseText+")");
				                                                var record=json.record;
                                            				    
					                                            for(var i=0; i < record.length; i++)
					                                            {   
					                                                    document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr")).value = record[i].shortname;   //客户
			                                                            document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = record[i].shortname;
			                                                            document.getElementById(obj.id.replace("txtCustAddr","txtAddr")).value = record[i].address;   //地址
			                                                            document.getElementById(obj.id.replace("txtCustAddr","hidAddr") ).value = record[i].address;   //地址
//			                                                            document.getElementById(obj.id.replace("txtCustAddr","txtContact")).value = record[i].linkman1;    //联系人



//			                                                            document.getElementById(obj.id.replace("txtCustAddr","hidContact")).value = record[i].linkman1;   
			                                                            document.getElementById(obj.id.replace("txtCustAddr","txtCTel")).value = record[i].tel1;   //联系人电话



			                                                            document.getElementById(obj.id.replace("txtCustAddr","hidTel")).value = record[i].tel1;
			                                                            document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = record[i].id;    //客户ID号


                                                			            
			                                                            document.getElementById(obj.id.replace("txtCustAddr","lblCustDeptName")).innerHTML = record[i].custdeptname;   //所属部门

		                                                                document.getElementById(obj.id.replace("txtCustAddr","lblEmail")).innerHTML = record[i].email;  //电子邮件
		                                                                document.getElementById(obj.id.replace("txtCustAddr","lblMastCust")).innerHTML = record[i].mastcustname;   //服务单位
		                                                                document.getElementById(obj.id.replace("txtCustAddr","hidCustDeptName")).value = record[i].custdeptname;   //所属部门

		                                                                document.getElementById(obj.id.replace("txtCustAddr","hidCustEmail")).value = record[i].email;  //电子邮件
		                                                                document.getElementById(obj.id.replace("txtCustAddr","hidMastCust")).value = record[i].mastcustname;   //服务单位
//		                                                                document.getElementById(obj.id.replace("txtCustAddr","lbljob")).innerHTML = record[i].job;   //职位
//		                                                                document.getElementById(obj.id.replace("txtCustAddr","hidjob")).value = record[i].job;   //职位

			                                                            document.getElementById(obj.id.replace("txtCustAddr","txtCustMobile")).value = record[i].mobile;   //手机号码
			                                                            document.getElementById(obj.id.replace("txtCustAddr","hidCustMobile") ).value = record[i].mobile;   //手机号码
			                                                            document.getElementById(obj.id.replace("txtCustAddr","hidCustAreaID") ).value = record[i].custareaid;   //客户片区
			                                                            document.getElementById(obj.id.replace("txtCustAddr","hidCustArea") ).value = record[i].custarea;   //客户片区
			                                                            //document.getElementById(obj.id.replace("txtCustAddr","lblCustArea") ).innerHTML = record[i].custarea;   //客户片区
                                                			            
			                                                            if (typeof(document.all.<%=Table3.ClientID%>)!="undefined" )
			                                                            {
			                                                                document.getElementById(obj.id.replace("txtCustAddr","txtEqu")).value = record[i].equname;   //资产名称
                                                                            document.getElementById(obj.id.replace("txtCustAddr","hidEquName")).value = record[i].equname;   //资产名称
                                                                            document.getElementById(obj.id.replace("txtCustAddr","hidEqu") ).value = record[i].equid;  //资产IDontact")).id);
                                                                            
			                                                                document.getElementById(obj.id.replace("txtCustAddr","txtListName")).value = record[i].listname;   //资产目录
                                                                            document.getElementById(obj.id.replace("txtCustAddr","hidListName")).value = record[i].listname;   //资产目录
                                                                            document.getElementById(obj.id.replace("txtCustAddr","hidListID") ).value = record[i].listid;  //资产目录ID
                                                                            
                                                                        }                      
					                                            }

													} 
												    } 
												}
					xmlhttp.send(null); 
				}catch(e3){}
            }
        }
        
        function SelectSomeEqu(obj)   //选择多个设备
			{
			    var newDateObj = new Date();
	            var sparamvalue =  newDateObj.getYear().toString() + newDateObj.getMonth().toString();
	            var name = obj.value.trim();
	            if(name.trim()=="")
	            {
	                return;
	            }
	            var EquipmentCatalogID = document.all.<%=hidListID.ClientID%>.value.trim();
	            var CustName ="";
				var	value=window.showModalDialog("../mydestop/frmQuickLocateEqu.aspx?IsSelect=true&randomid="+GetRandom()+"&Name=" + escape(name) + "&EquCust=" + escape(CustName)+"&EquipmentCatalogID="+EquipmentCatalogID,"","dialogHeight:500px;dialogWidth:600px");
				if(value != null)
				{
					if(value.length>1)
					{
				        document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = value[2];   //设备名称
                        document.getElementById(obj.id.replace("txtEqu","hidEquName")).value = value[2];   //设备名称
                        document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value = value[1];  //设备IDontact")).id);                                                
                        document.getElementById(obj.id.replace("txtEqu","txtListName")).value = value[3];   //资产目录名称
			            document.getElementById(obj.id.replace("txtEqu","hidListName")).value = value[3];   //资产目录名称
			            document.getElementById(obj.id.replace("txtEqu","hidListID") ).value = value[4];  //资产目录ID    
					}
					else
					{
					    document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = "";   //设备名称
                        document.getElementById(obj.id.replace("txtEqu","hidEquName")).value = "";   //设备名称
                        document.getElementById(obj.id.replace("txtEqu","hidEqu")).value = "0";  //设备IDontact")).id);                                                                   
					}
				}
				else
				{
				    document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = document.getElementById(obj.id.replace("txtEqu","hidEquName")).value;   //设备名称
				}
			}
        function GetEquID(obj)
        {
            var EquipmentCatalogID = document.all.<%=hidListID.ClientID%>.value.trim();
            if(obj.value.trim()=="")
	            {
	                document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value = "0";  //设备IDontact")).id);
	                return;
	            }
	        if(obj.value.trim()==document.getElementById(obj.id.replace("txtEqu","hidEquName")).value.trim() && (document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value.trim()!="" || document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value.trim()!="0"))
	            {
	                return;
	            }
	        var CustName = "";
            if(xmlhttp == null)
                xmlhttp = CreateXmlHttpObject();       
            if(xmlhttp != null)
            {
                try
                {	
					xmlhttp.open("GET", "../MyDestop/frmXmlHttpAjax.aspx?Equ=" + escape(obj.value) +  "&randomid="+GetRandom()+"&EquCust=" + escape(CustName)+"&EquipmentCatalogID="+EquipmentCatalogID, true); 
                    xmlhttp.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
					xmlhttp.onreadystatechange = function() 
												{ 
													if ( xmlhttp.readyState==4 ) 
													{ 
														if(xmlhttp.responseText=="-1")  //没有
														{
														    alert("此资产不存在，请重新查找！"); 
														    document.getElementById(obj.id.replace("txtEqu","txtEqu")).value="";
														    document.getElementById(obj.id.replace("txtEqu","hidEqu")).value=0;
														    obj.focus();
														    obj.select();
													    }
													    else if(xmlhttp.responseText=="0") //找到多个
													    {
													        SelectSomeEqu(obj);
													    }
													    else  //找到唯一
													    {
													        
													        var json= eval("("+xmlhttp.responseText+")");
				                                            var record=json.record;
				                                            
				                                            for(var i=0; i < record.length; i++)
					                                        {
													        
													            document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = record[i].name;   //设备名称
			                                                    document.getElementById(obj.id.replace("txtEqu","hidEquName")).value = record[i].name;   //设备名称
			                                                    document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value = record[i].id;  //设备IDontact")).id);			                                                    
			                                                    document.getElementById(obj.id.replace("txtEqu","txtListName")).value = record[i].listname;   //资产目录名称
			                                                    document.getElementById(obj.id.replace("txtEqu","hidListName")).value = record[i].listname;   //资产目录名称
			                                                    document.getElementById(obj.id.replace("txtEqu","hidListID") ).value = record[i].listid;  //资产目录ID
			                                                }
													    }
													} 
												} 
					xmlhttp.send(null); 
				}catch(e3){}
            }
        }
			
			//设备
			function SelectEqu(obj) 
			{
			    var EquipmentCatalogID = document.all.<%=hidListID.ClientID%>.value.trim();
                    			        
			    var EquName = document.all.<%=txtEqu.ClientID%>.value.trim();
			    var CustName = document.all.<%=txtCustAddr.ClientID%>.value.trim();
			    var MastCust = document.all.<%=hidMastCust.ClientID%>.value.trim();
			    
			    var url="../EquipmentManager/frmEqu_DeskMain.aspx?IsSelect='1'&randomid="+GetRandom()+"&FlowID="+ '<%=FlowID%>' + "&EquName=" + escape(EquName) + "&Cust=" + escape(CustName) + "&MastCust=" + escape(MastCust)+"&EquipmentCatalogID="+EquipmentCatalogID+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrom=CST_Issue_Service"; 
			    window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=600");			   
			}
			
			//选择资产目录
			function SelectEquCatalog(obj)
			{
			        var	value=window.showModalDialog("../EquipmentManager/frmEqu_DeskCateListSel.aspx?random=" + GetRandom(),window,"dialogWidth=800px; dialogHeight=600px;") ;

	                if(value != null)
	                {
		                if(value.length>1)
		                {			
		                    if(document.getElementById(obj.id.replace("cmdListName","hidListID") ).value == value[0])
		                    {}
		                    else
		                    {
		                        document.getElementById(obj.id.replace("cmdListName","txtListName")).value = value[1];   //资产目录名称
			                    document.getElementById(obj.id.replace("cmdListName","hidListName")).value = value[1];   //资产目录名称
			                    document.getElementById(obj.id.replace("cmdListName","hidListID") ).value = value[0];  //资产目录ID
		                    }
		                }
		                else
		                {			                
			                document.getElementById(obj.id.replace("cmdListName","txtListName")).value = "";         //资产目录名称
			                document.getElementById(obj.id.replace("cmdListName","hidListName")).value = "";         //资产目录名称
			                document.getElementById(obj.id.replace("cmdListName","hidListID") ).value = "0";         //资产目录ID
		                }
	                }
	                else
	                {			                
			                document.getElementById(obj.id.replace("cmdListName","txtListName")).value = "";    //资产目录名称
			                document.getElementById(obj.id.replace("cmdListName","hidListName")).value = "";    //资产目录名称
			                document.getElementById(obj.id.replace("cmdListName","hidListID") ).value = "0";    //资产目录ID
	                }
			}
			
	        //打开客户对应的事件记录



	        function OpenServiceHistory(type)
	        {
	            
	            if(type=="0")
	            {
	                var CataID;
	                if (typeof(document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_hidCataID"))!="undefined" )
	                    CataID = document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_hidCataID").value
	                else
	                    CataID =<%=CatelogID%>;
	                window.open("frmIssueList.aspx?NewWin=true&ID=0&randomid="+GetRandom()+"&ServiceType=" + CataID + "&FlowID="+ '<%=FlowID%>','',"scrollbars=no,status=yes ,resizable=yes,width=800,height=600");
	            }
	            else if(type=="1")
	            {
	                var custid;
	                custid = document.all.<%=hidCustID.ClientID%>.value;
	                if(custid=="0")
	                {
	                    custid = "-1";
	                }
	                window.open("frmIssueList.aspx?NewWin=true&randomid="+GetRandom()+"&ID=" + custid + "&FlowID="+ '<%=FlowID%>','',"scrollbars=no,status=yes ,resizable=yes,width=800,height=600");
	            }
	            else if(type=="2")
	            {
	                window.open("frmIssueList.aspx?NewWin=true&IsHistory=true&ID=0&randomid="+GetRandom()+"&EquID=" + document.all.<%=hidEqu.ClientID%>.value + "&FlowID="+ '<%=FlowID%>','',"scrollbars=no,status=yes ,resizable=yes,width=800,height=600");
	            }
	            event.returnValue = false;
	        }

	        function ShowTable(imgCtrl)
            {
                  var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
                  var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
                  var TableID = imgCtrl.id.replace("Img","Table");
                  var className;
                  var objectFullName;
                  var tableCtrl;
                  objectFullName = <%=ValidationSummary1.ClientID%>.id;
                  className = objectFullName.substring(0,objectFullName.indexOf("ValidationSummary1")-1);
                  tableCtrl = document.getElementById(className.substr(0,className.length)+"_"+TableID);
                  if(imgCtrl.src.indexOf("icon_expandall") != -1)
                  {
                    tableCtrl.style.display ="";
                    imgCtrl.src = ImgMinusScr ;
                  }
                  else
                  {
                    tableCtrl.style.display ="none";
                    imgCtrl.src = ImgPlusScr ;		 
                  }
            } 
            
            //摘要自动生成
            function CreateTitle()
            {
                var subject=document.all.<%=CtrFlowFTSubject.ClientID%>.value.trim();//获取摘要
                if (typeof(document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_hidCatalogName"))!="undefined"&&subject=="")
                {                
	                document.all.<%=CtrFlowFTSubject.ClientID%>.value = document.all.<%=txtCustAddr.ClientID%>.value.trim()+document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_hidCatalogName").value;
	            }
            }
            
            
            //查看服务级别明细
            function OpenServiceLevelDetail(obj)
            {
                var sID = document.getElementById(obj.id.replace("lnkServiceLevel","hidServiceLevelID")).value;
                if(sID != "0" && sID != "")
                {
                    window.open("frmCst_ServiceLevelEdit.aspx?IsSelect=1&randomid="+GetRandom()+"&id=" + sID,"_blank","scrollbars=yes,status=yes ,resizable=yes,width=600,height=480");
                }
                
                event.returnValue = false;
            }
            
            function InitText()
            {
                var ServiceTypeID = "0";
                var CustID = document.all.<%=hidCustID.ClientID%>.value;
			    var EquID = ""
			    if(typeof(document.all.<%=txtEqu.ClientID%>) != "undefined")
			    {
			         EquID = document.all.<%=txtEqu.ClientID%>.value;
			    }
			    else
			    {
			        EquID = document.all.<%=lblEqu.ClientID%>.innerText;
			    }
			    var ServiceLevelID = document.all.<%=hidServiceLevelID.ClientID%>.value;
                if (typeof(document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_hidCataID"))!="undefined" )
			    {
			        ServiceTypeID = document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_hidCataID").value;
			    }
			    else
			    {			    
			        ServiceTypeID = document.all.<%=hidServiceTypeID.ClientID%>.value;
			    }
			    if(typeof(document.all.<%=Sjwxr.ClientID%>_hidCustID)!="undefined" )
			    {
			        document.all.<%=Sjwxr.ClientID%>_hidEquID.value = EquID;
			        document.all.<%=Sjwxr.ClientID%>_hidCustID.value = CustID;
			        document.all.<%=Sjwxr.ClientID%>_hidServiceTypeID.value = ServiceTypeID;
			        document.all.<%=Sjwxr.ClientID%>_hidServiceLevelID.value = ServiceLevelID;
			        
			        document.all.<%=Sjwxr.ClientID%>_hidMastCustName.value = document.all.<%=lblMastCust.ClientID%>.innerHTML.trim();
			        document.all.<%=Sjwxr.ClientID%>_hidEquName.value = document.all.<%=txtEqu.ClientID%>.value.trim();
			        document.all.<%=Sjwxr.ClientID%>_hidCustName.value = document.all.<%=txtCustAddr.ClientID%>.value.trim();
			        
			        if (typeof(document.all.<%=txtServiceLevel.ClientID%>)!="undefined" )
	                {     
                        document.all.<%=Sjwxr.ClientID%>_hidLevelName.value = document.all.<%=txtServiceLevel.ClientID%>.value.trim();
                    }
                    else if (typeof(document.all.<%=labServiceLevel.ClientID%>)!="undefined" )
                    {
                        document.all.<%=Sjwxr.ClientID%>_hidLevelName.value = document.all.<%=labServiceLevel.ClientID%>.outerText;
                    }
			        if (typeof(document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_hidCatalogName"))!="undefined" )
	                {
                        document.all.<%=Sjwxr.ClientID%>_hidTypeName.value = document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_hidCatalogName").value;
                    }
                    else
                    {
                        document.all.<%=Sjwxr.ClientID%>_hidTypeName.value = document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_labCate1").innerHTML;
                    }
			    }
            }
            
            //自动获取服务级别
            function GetServiceLevel()
            {
                var  obj = document.all.<%=cmdPopServiceLevel.ClientID%>;
                
                var ServiceTypeID = "0";
			    var ServiceKindID = "0";
			    var ServiceEffID = "0";
			    var ServiceInsID = "0";
			    
			    var CustID = document.all.<%=hidCustID.ClientID%>.value;
			    var EquID = document.all.<%=hidEqu.ClientID%>.value;
			    
			    if (typeof(document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_hidCataID"))!="undefined" )
			    {
			        ServiceTypeID = document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_hidCataID").value;
			    }
			    
			    if (typeof(document.all.<%=CtrFCDEffect.ClientID%>)!="undefined" )
			    {
			        var ideff= document.all.<%=CtrFCDEffect.ClientID%>.id;
                    var ddleff=document.getElementById(ideff);      
                    
			        if(ddleff.selectedIndex!=null&&ddleff.selectedIndex!="undefined")
                    {
			            ServiceEffID= ddleff.options[ddleff.selectedIndex].value.trim();
			        }
			        else
			        {
			            var oRadioButtonLists=ddleff.getElementsByTagName('input');
                        for(var i=0;i<oRadioButtonLists.length;i++)
                        {
                             if(oRadioButtonLists[i].checked) 
                             {
                                  ServiceEffID = oRadioButtonLists[i].value;
                             }
                        }
			        }
			    }
			    
			    			    
			    if (typeof(document.all.<%=CtrFCDInstancy.ClientID%>)!="undefined" )
			    {
			        var idins= document.all.<%=CtrFCDInstancy.ClientID%>.id;
                    var ddlins=document.getElementById(idins);      
			        
			        if(ddlins.selectedIndex!=null&&ddlins.selectedIndex!="undefined")
                    {
			            ServiceInsID= ddlins.options[ddlins.selectedIndex].value.trim();
			        }
			        else
			        {
			            var oRadioButtonLists=ddlins.getElementsByTagName('input');
                        for(var i=0;i<oRadioButtonLists.length;i++)
                        {
                             if(oRadioButtonLists[i].checked) 
                             {
                                  ServiceInsID = oRadioButtonLists[i].value;
                             }
                        }
			        }
			    }
			   
			    
			    if(ServiceEffID=="0" || ServiceInsID=="0")
			        return;
			    
			    var pars = "act=ServiceLevel&CustID=" + CustID + "&EquID="+ EquID + "&TypeID=" + ServiceTypeID + "&KindID=" + ServiceKindID + "&EffID=" + ServiceEffID + "&InsID=" + ServiceInsID;
			     $.ajax({
                    type: "post",
                    data:pars,
                    async:false,
                    url: "../Forms/Handler.ashx",
                    success: function(data, textStatus){
                        //alert(data);
                        if(data=="0") //找到多个
                        {
                            ServiceLevelSelect(obj);
                        }
                        else if(data=="")
                        {
                                document.getElementById(obj.id.replace("cmdPopServiceLevel","trServiceLevelDetail")).style.display = "none";   //整体显示        
			                    document.getElementById(obj.id.replace("cmdPopServiceLevel","txtServiceLevel")).value = "";   //级别名称        
                                document.getElementById(obj.id.replace("cmdPopServiceLevel","hidServiceLevel")).value = "";   //级别名称
			                    document.getElementById(obj.id.replace("cmdPopServiceLevel","hidServiceLevelID")).value = "0";  //级别ID
			                    document.getElementById(obj.id.replace("cmdPopServiceLevel","divSLDefinition")).innerHTML = "";   //级别定义
        			            
			                    document.getElementById(obj.id.replace("cmdPopServiceLevel","divSLTimeLimt")).innerHTML = "";  //级别时限
        			            
			                    document.getElementById(obj.id.replace("cmdPopServiceLevel","lnkServiceLevel")).innerText = "";
        			            
			                    document.getElementById(obj.id.replace("cmdPopServiceLevel","hidServiceLevelChange")).value = "true";  //更改过服务级别

                        }
                        else 
                        {
                            var json = eval("(" + data + ")");
                                                    
                            if (json.record != null && json.record.length > 0)
                            {
                                document.getElementById(obj.id.replace("cmdPopServiceLevel","trServiceLevelDetail")).style.display = "";   //整体显示        
			                    document.getElementById(obj.id.replace("cmdPopServiceLevel","txtServiceLevel")).value = json.record[0].levelname;   //级别名称        
                                document.getElementById(obj.id.replace("cmdPopServiceLevel","hidServiceLevel")).value = json.record[0].levelname;   //级别名称
			                    document.getElementById(obj.id.replace("cmdPopServiceLevel","hidServiceLevelID")).value = json.record[0].id;  //级别ID
			                    document.getElementById(obj.id.replace("cmdPopServiceLevel","divSLDefinition")).innerHTML = "<table class='listContent' width='100%'> <tr><td class='listTitle' width='15%'>级别定义</td><td class='list'>" + json.record[0].definition.replace(/@/g,";") + "</td></tr></table>";   //级别定义
        			            
			                    document.getElementById(obj.id.replace("cmdPopServiceLevel","divSLTimeLimt")).innerHTML = "<table class='listContent' width='100%'> <tr><td class='listTitle'  width='15%'>时限要求</td><td class='list'>" + json.record[0].limitstr + "</td></tr></table>";  //级别时限
        			            
			                    document.getElementById(obj.id.replace("cmdPopServiceLevel","lnkServiceLevel")).innerText = "详情";
        			            
			                    document.getElementById(obj.id.replace("cmdPopServiceLevel","hidServiceLevelChange")).value = "true";  //更改过服务级别

                            }
                        }
		            }
                 });
            
            }
            
            //服务级别选择
			function ServiceLevelSelect(obj) 
			{
			    var ServiceTypeID = "0";
			    var ServiceKindID = "0";
			    var ServiceEffID = "0";
			    var ServiceInsID = "0";
			    
			    var CustID = document.all.<%=hidCustID.ClientID%>.value;
			    var EquID = document.all.<%=hidEqu.ClientID%>.value;
			    if (typeof(document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_hidCataID"))!="undefined" )
			    {
			        ServiceTypeID = document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_hidCataID").value;
			    }
			    
			    if (typeof(document.all.<%=CtrFCDEffect.ClientID%>)!="undefined" )
			    {
			        var ideff= document.all.<%=CtrFCDEffect.ClientID%>.id;
                    var ddleff=document.getElementById(ideff);      
                    
			        if(ddleff.selectedIndex!=null&&ddleff.selectedIndex!="undefined")
                    {
			            ServiceEffID= ddleff.options[ddleff.selectedIndex].value.trim();
			        }
			        else
			        {
			            var oRadioButtonLists=ddleff.getElementsByTagName('input');
                        for(var i=0;i<oRadioButtonLists.length;i++)
                        {
                             if(oRadioButtonLists[i].checked) 
                             {
                                  ServiceEffID = oRadioButtonLists[i].value;
                             }
                        }
			        }
			    }
			    
			    			    
			    if (typeof(document.all.<%=CtrFCDInstancy.ClientID%>)!="undefined" )
			    {
			        var idins= document.all.<%=CtrFCDInstancy.ClientID%>.id;
                    var ddlins=document.getElementById(idins);      
			        
			        if(ddlins.selectedIndex!=null&&ddlins.selectedIndex!="undefined")
                    {
			            ServiceInsID= ddlins.options[ddlins.selectedIndex].value.trim();
			        }
			        else
			        {
			            var oRadioButtonLists=ddlins.getElementsByTagName('input');
                        for(var i=0;i<oRadioButtonLists.length;i++)
                        {
                             if(oRadioButtonLists[i].checked) 
                             {
                                  ServiceInsID = oRadioButtonLists[i].value;
                             }
                        }
			        }
			    }			    
			        var url="frmCst_ServicLevelSelect.aspx?IsSelect=true&randomid="+GetRandom()+"&CustID=" + CustID + "&EquID="+ EquID + "&TypeID=" + ServiceTypeID + "&KindID=" + ServiceKindID + "&EffID=" + ServiceEffID + "&InsID=" +
			         ServiceInsID+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=CST_Issue_Service";
			        
			        window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=600");			
			}
			//案例分析
			function DoItemQuestionAnalysis(lngAppID,lngFlowID)
	        {
		        window.open("../ProbleForms/frmPro_ProblemAnalyseMain.aspx?AppID=" + lngAppID + "&randomid="+GetRandom()+"&FlowID=" + lngFlowID,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=500");
	        }
            //知识归档
            function DoKmAdd(lngMessageID,lngAppID,lngFlowID)
	        {
	             window.open("../Forms/form_all_flowmodel.aspx?NewWin=true&appid=400&randomid="+GetRandom()+"&ep=" + lngMessageID,"","scrollbars=no,status=yes ,resizable=yes,width=680,height=500");
	        }
	         //邮箱验证
        function EmailValidate() {
            var search_str = /^[\w\-\.]+@[\w\-\.]+(\.\w+)+$/;
            var email_val = document.getElementById("<%=lblEmail.ClientID%>").outerText;
            if (email_val != "") {
                if (!search_str.test(email_val)) {
                    alert("请输入正确的电子邮件！");
                    document.getElementById("<%=lblEmail.ClientID%>").focus();
                }
            }
        }
        //判断时间期望时间大约申请时间
        function CalcuteTotalHours1()
        {
       
        try{
            var sCreateTime,sDTQWTime;
            var sCreateTimeH,sCreateTimeM,sDTQWTimeH,sDTQWTimeM;
            //期望时间
            if(typeof(document.all.ctl00_ContentPlaceHolder1_CtrDTQWTime_txtDate) != "undefined")
            {                      
                sDTQWTime = document.all.ctl00_ContentPlaceHolder1_CtrDTQWTime_txtDate.value;
                sDTQWTimeH = document.all.ctl00_ContentPlaceHolder1_CtrDTQWTime_ddlHours.value;
                sDTQWTimeM = document.all.ctl00_ContentPlaceHolder1_CtrDTQWTime_ddlMinutes.value;                        
            }
            else if(typeof(document.all.ctl00_ContentPlaceHolder1_CtrDTQWTime_labDate) !="undefined")
            {
                if(document.all.ctl00_ContentPlaceHolder1_CtrDTFinishedTime_labDate.innerText != "--")
                 {
                      sDTQWTime = document.all.ctl00_ContentPlaceHolder1_CtrDTQWTime_labDate.innerText.substring(0,10);
                      sDTQWTimeH = document.all.ctl00_ContentPlaceHolder1_CtrDTQWTime_labDate.innerText.substring(11,13);
                      sDTQWTimeM = document.all.ctl00_ContentPlaceHolder1_CtrDTQWTime_labDate.innerText.substring(14,16);
                 }
                 else
                 {                           
                      sDTQWTime = "";
                 }
            }
            //申请时间
            if(typeof(document.all.ctl00_ContentPlaceHolder1_CtrDTCreateTime_txtDate) != "undefined")
            {
                  sCreateTime = document.all.ctl00_ContentPlaceHolder1_CtrDTCreateTime_txtDate.value;
                  sCreateTimeH = document.all.ctl00_ContentPlaceHolder1_CtrDTCreateTime_ddlHours.value;
                  sCreateTimeM = document.all.ctl00_ContentPlaceHolder1_CtrDTCreateTime_ddlMinutes.value;                        
            }
            else if(typeof(document.all.ctl00_ContentPlaceHolder1_CtrDTCreateTime_labDate) !="undefined")
            {
                  if(document.all.ctl00_ContentPlaceHolder1_CtrDTCreateTime_labDate.innerText != "--")
                  {
                    sCreateTime = document.all.ctl00_ContentPlaceHolder1_CtrDTCreateTime_labDate.innerText.substring(0,10);
                    sCreateTimeH = document.all.ctl00_ContentPlaceHolder1_CtrDTCreateTime_labDate.innerText.substring(11,13);
                    sCreateTimeM = document.all.ctl00_ContentPlaceHolder1_CtrDTCreateTime_labDate.innerText.substring(14,16);
                  }
                  else
                 {
                     sCreateTime = "";
                 }
            } 
           if( sDTQWTime != "" && sCreateTime !="")
           {                        
                var sDTQWTime=new Date(sDTQWTime.replace("-",   "/") +" "+sDTQWTimeH+":"+sDTQWTimeM);
                var sCreateTime=new Date(sCreateTime.replace("-",   "/") +" "+sCreateTimeH+":"+sCreateTimeM);
                      
              if(sDTQWTime.getTime()-sCreateTime.getTime() < 0)
              {
                    alert("期望时间不能小于申请时间!");         
                   document.all.ctl00_ContentPlaceHolder1_CtrDTQWTime_txtDate.value="";          
                  
              }                                                             
           }
            
        }catch(e)
        {
            return false;
        }
            
        }
        //判断时间
        function CalcuteTotalHours2()
            {
              try{
                    var sReportingTime,sDTCustTime;
                    var sReportingTimeH,sReportingTimeM,sDTCustTimeH,sDTCustTimeM;
                    if(typeof(document.all.ctl00_ContentPlaceHolder1_CtrReportingTime_txtDate) != "undefined")
                    {                      
                        sReportingTime = document.all.ctl00_ContentPlaceHolder1_CtrReportingTime_txtDate.value;
                        sReportingTimeH = document.all.ctl00_ContentPlaceHolder1_CtrReportingTime_ddlHours.value;
                        sReportingTimeM = document.all.ctl00_ContentPlaceHolder1_CtrReportingTime_ddlMinutes.value;                        
                    }
                    else if(typeof(document.all.ctl00_ContentPlaceHolder1_CtrReportingTime_labDate) !="undefined")
                    {
                        if(document.all.ctl00_ContentPlaceHolder1_CtrDTFinishedTime_labDate.innerText != "--")
                        {
                            sReportingTime = document.all.ctl00_ContentPlaceHolder1_CtrReportingTime_labDate.innerText.substring(0,10);
                            sReportingTimeH = document.all.ctl00_ContentPlaceHolder1_CtrReportingTime_labDate.innerText.substring(11,13);
                            sReportingTimeM = document.all.ctl00_ContentPlaceHolder1_CtrReportingTime_labDate.innerText.substring(14,16);
                        }
                        else
                        {                           
                           sReportingTime = "";
                        }
                    }
                    if(typeof(document.all.ctl00_ContentPlaceHolder1_CtrDTCustTime_txtDate) != "undefined")
                    {
                        sDTCustTime = document.all.ctl00_ContentPlaceHolder1_CtrDTCustTime_txtDate.value;
                        sDTCustTimeH = document.all.ctl00_ContentPlaceHolder1_CtrDTCustTime_ddlHours.value;
                        sDTCustTimeM = document.all.ctl00_ContentPlaceHolder1_CtrDTCustTime_ddlMinutes.value;                        
                    }
                    else if(typeof(document.all.ctl00_ContentPlaceHolder1_CtrDTCustTime_labDate) !="undefined")
                    {
                       if(document.all.ctl00_ContentPlaceHolder1_CtrDTCustTime_labDate.innerText != "--")
                       {
                            sDTCustTime = document.all.ctl00_ContentPlaceHolder1_CtrDTCustTime_labDate.innerText.substring(0,10);
                            sDTCustTimeH = document.all.ctl00_ContentPlaceHolder1_CtrDTCustTime_labDate.innerText.substring(11,13);
                            sDTCustTimeM = document.all.ctl00_ContentPlaceHolder1_CtrDTCustTime_labDate.innerText.substring(14,16);
                        }
                        else
                        {
                             sDTCustTime = "";
                        }
                    }          
                    
                    if( sReportingTime != "" && sDTCustTime !="")
                    {                        
                        var ReportingTime=new Date(sReportingTime.replace("-",   "/") +" "+sReportingTimeH+":"+sReportingTimeM);
                        var DTCustTime=new Date(sDTCustTime.replace("-",   "/") +" "+sDTCustTimeH+":"+sDTCustTimeM);
                       
                       if(ReportingTime.getTime()-DTCustTime.getTime() < 0)
                       {
                            alert("报告时间不能小于发生时间!");                           
                       }                 
                       if(ReportingTime.getTime()-(new Date()).getTime() > 0)
                       {
                            alert("报告时间不能大于当前时间!");                           
                       }                                              
                   }
               }catch(e)
               {
                 return false;
               }
            }			
    </script>
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
     <input id="hidIssTempID" type="hidden" runat="server" value="0" />
    <input id="hidServiceID" type="hidden" value="0" runat="server" />
    <input id="hidEqu" type="hidden" runat="server" value="-1" />
    <input id="hidCustID" runat="server" type="hidden" value="" />
    <input id="hidCust" runat="server" type="hidden" />
    <input id="hidAddr" runat="server" type="hidden" />
    <input id="hidContact" runat="server" type="hidden" />
    <input id="hidTel" runat="server" type="hidden" />
    <input id="hidCustDeptName" type="hidden" runat="server" />
    <input id="hidCustEmail" type="hidden" runat="server" />
    <input id="hidMastCust" type="hidden" runat="server" />
    <input id="hidjob" type="hidden" runat="server" />
    <input id="hidEquName" type="hidden" runat="server" />
    <input id="hidEquDeskPositions" type="hidden" runat="server" />
    <input id="hidEquDeskCode" type="hidden" runat="server" />
    <input id="hidEquDeskSerialNumber" type="hidden" runat="server" />
    <input id="hidEquDeskModel" type="hidden" runat="server" />
    <input id="hidEquDeskBreed" type="hidden" runat="server" />
    <input id="hidFeedBackCount" type="hidden" runat="server" />
    <input id="hidFeedBack" type="hidden" runat="server" value="0" />
    <input id="hidFlowID" type="hidden" runat="server" value="0" />
    <input id="hidActorClass" type="hidden" runat="server" value="-1" />
    <input id="hidCustAreaID" runat="server" type="hidden" value="0" />
    <input id="hidCustArea" runat="server" type="hidden" />
    <table id="Table12" width="100%" runat="server" class="listNewContent" border="0"
        cellspacing="2" cellpadding="0">
        <tr>
            <td align="left" valign="bottom" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                align="absbottom" />
                            申请人信息

                        </td>
                    </tr>
                </table>
            </td>
            <td id="AddCustTD" valign="middle" align="right" class="listTitleNew" style="width: 120">
                <asp:Button ID="bthCreateCus" CssClass="btnClass3" runat="server" Text="添加客户" OnClientClick="getCustomId(this);" />
                <asp:Button ID="bthAddEqu" runat="server" Text="添加资产" OnClientClick="AddEqu(this);" />
            </td>
        </tr>
    </table>
    <table class="listContent" width="100%" align="center" runat="server" id="Table2"
        border="0" cellspacing="1" cellpadding="1">
        <tr>
            <td style="width: 12%" class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="LitApplication" runat="server" Text="客户名称"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:Label ID="labCustAddr" runat="server" Visible="False"></asp:Label>
               
                <asp:TextBox ID="txtCustAddr" runat="server" onblur="GetCustID(this)" MaxLength="200"
                    onkeydown="if(event.keyCode==13){event.keyCode=9;};"></asp:TextBox>
                <asp:Label ID="rWarning" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
                <input id="cmdCust" onclick="CustomSelect(this)" type="button" value="..." runat="server"
                    class="btnClass2" />
                <asp:LinkButton ID="lnkCustHistory" runat="server" ForeColor="#0000C0" OnClientClick="OpenServiceHistory('1');">(历史参照)</asp:LinkButton>
                &nbsp;
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitAppAddress" runat="server" Text="申请人地址"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblAddr" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtAddr" runat="server" MaxLength="200"></asp:TextBox>&nbsp;
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitCTel" runat="server" Text="联系人电话"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="labCTel" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtCTel" runat="server" MaxLength="20"></asp:TextBox>&nbsp;
            </td>
            <td class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="LitCustDeptName" runat="server" Text="客户部门"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCustDeptName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitCustMobile" runat="server" Text="手机号码"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCustMobile" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtCustMobile" runat="server" MaxLength="50"></asp:TextBox>
                <input id="hidCustMobile" runat="server" type="hidden" />
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="litCustEmail" runat="server" Text="电子邮件"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblEmail" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:Label ID="lblMastCust" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
   
    <table id="Table15" class="listNewContent" runat="server" idth="100%" align="center"
        style="display:none;">
        <tr>
                <td valign="top" align="left" class="listTitleNew">
                    <table width="150" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td height="24" class="bt_di">
                                <img class="icon" id="Img3" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                    width="16" align="absbottom" />
                                资产信息
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
    </table>
    
    <table class="listContent" width="100%" align="center" runat="server" id="Table3"
        cellspacing="0" cellpadding="2" rules="cols" border="0">
       
        <tr runat="server" id="trEqu">
            <td style="width: 12%; display: none;" class="listTitleRight">
                <asp:Literal ID="LitListName" runat="server" Text="资产目录"></asp:Literal>
            </td>
            <td class="list" style="width: 35%; display: none;">
                <asp:Label ID="lblListName" runat="server" Visible="false"></asp:Label>
                <asp:TextBox ID="txtListName" runat="server" MaxLength="20" ReadOnly="true"></asp:TextBox>
                <input id="cmdListName" onclick="SelectEquCatalog(this)" type="button" value="..."
                    runat="server" name="cmdListName" class="btnClass2" />
                <input id="hidListName" value="" type="hidden" runat="server" />
                <input id="hidListID" value="0" type="hidden" runat="server" />
            </td>
        </tr>
    </table>
    <br />
    <table height="29" border="0" cellpadding="0" cellspacing="0">
        <tr style="cursor: hand">
            <td width="7" valign="top">
                <img src="../Images/lm-left.gif" width="7" height="29" />
            </td>
            <td width="95" height="29" align="center" valign="middle" id="name0" class="td_3"
                onclick="chang_Class('name',6,0)" background="../Images/lm-b.gif" onmouseover="chang(this,'lm-c.gif')"
                onmouseout="chang(this,'lm-a.gif')">
                <span id="a0" class="td_3">基本信息</span>
            </td>
            <td width="95" height="29" align="center" valign="middle" id="name1" class="STYLE4"
                onclick="chang_Class('name',6,1)" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-c.gif')"
                onmouseout="chang(this,'lm-a.gif')" style="display: none">
                <span id="a1" class="STYLE4">处理信息</span>
            </td>
            <td width="95" height="29" align="center" valign="middle" id="name2" class="STYLE4"
                onclick="chang_Class('name',6,2)" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-c.gif')"
                onmouseout="chang(this,'lm-a.gif')" style="display: none">
                <span id="a2" class="STYLE4">督办回访</span>
            </td>
            <td width="95" height="29" align="center" valign="middle" id="name5" class="STYLE4"
                onclick="chang_Class('name',6,5)" background="../Images/lm-a.gif" onmouseover="chang(this,'lm-c.gif')"
                onmouseout="chang(this,'lm-a.gif')" style="display: none;">
                <span id="a5" class="STYLE4">知识参考</span>
            </td>
            <td width="95" height="29" align="center" valign="middle" id="name3" class="STYLE4"
                style="display: none" onclick="chang_Class('name',6,3)" background="../Images/lm-a.gif"
                onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')">
                <span id="a3" class="STYLE4">相关变更</span>
            </td>
            <td width="95" height="29" align="center" valign="middle" id="name4" class="STYLE4"
                style="display: none" onclick="chang_Class('name',6,4)" background="../Images/lm-a.gif"
                onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')">
                <span id="a4" class="STYLE4">相关问题</span>
            </td>
            <td width="7" valign="top">
                <img src="../Images/lm-right.gif" width="7" height="29" />
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <table id="Tables0" width="100%" align="center" class="listContent" border="0" cellspacing="1"
                    cellpadding="1">
                    <tr id="ShowServiceNo" runat="server" style="display: none;">
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="LitRegUserName" runat="server" Text="登单人"></asp:Literal>
                        </td>
                        <td class="list" style="width: 35%">
                            <uc8:UserPicker ID="RegUser" runat="server" ContralState="eReadOnly" TextToolTip="登单人" />
                        </td>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="LitRegSysDate" runat="server" Text="登记时间"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc1:ctrdateandtime ID="CtrDTRegTime" runat="server" ContralState="eReadOnly" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitEquipmentName1" runat="server" Text="服务目录名称"></asp:Literal>
                        </td>
                        <td class="list">
                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                                <Services>
                                    <asp:ServiceReference Path="~/AppForms/AutoWebService.asmx" />
                                </Services>
                            </asp:ScriptManager>
                            <asp:Label ID="lblEqu" runat="server" Visible="False"></asp:Label>
                            <asp:TextBox ID="txtEqu" runat="server" MaxLength="80" onblur="GetEquID(this)"></asp:TextBox>
                            <input id="cmdEqu" onclick="SelectEqu(this)" type="button" value="..." runat="server"
                                name="cmdEqu" class="btnClass2" />
                            <asp:LinkButton ID="lnkEquHistory" runat="server" ForeColor="#0000C0" OnClientClick="OpenServiceHistory('2');">(历史参照)</asp:LinkButton>
                            &nbsp;
                        </td>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitServiceNo" runat="server" Text="事件单号"></asp:Literal>
                        </td>
                        <td class="list">
                            <asp:Label ID="lblBuildCode" runat="server"></asp:Label><asp:Label ID="labServiceNo"
                                runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="Litapptime" runat="server" Text="申请时间"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc1:ctrdateandtime ID="CtrDTCreateTime" runat="server" TextToolTip="申请时间" MustInput="true" />
                        </td>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="LitExpectedTime1" runat="server" Text="期望时间"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc1:ctrdateandtime ID="CtrDTQWTime" runat="server" TextToolTip="期望时间" MustInput="true"
                                OnChangeScript="CalcuteTotalHours1();" />
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="listTitleRight">
                            <asp:Literal ID="LitServiceType" runat="server" Text="事件类别"></asp:Literal>
                        </td>
                        <td class="list" colspan="3">
                        
                            <uc11:ctrFlowCataDropListNew ID="ctrFCDServiceType" runat="server" RootID="1001"
                                TextToolTip="事件类别" />
                            <input id="hidServiceTypeID" runat="server" type="hidden" />
                        </td>
                    </tr>
                    <tr runat="server" id="trCustTime" visible="false">
                        <td class="listTitleRight">
                            <asp:Literal ID="LitCustTime" runat="server" Text="发生时间"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc1:ctrdateandtime ID="CtrDTCustTime" runat="server" MustInput="false" TextToolTip="发生时间" />
                        </td>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitReportingTime" runat="server" Text="报告时间"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc1:ctrdateandtime ID="CtrReportingTime" runat="server" MustInput="false" TextToolTip="报告时间"
                                OnChangeScript="CalcuteTotalHours2();" />
                        </td>
                    </tr>
                    <tr runat="server" id="trServiceKind" visible="false">
                        <td class="listTitleRight">
                            <asp:Literal ID="LitInstancyName" runat="server" Text="紧急度"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc6:ctrFlowCataDropListNew ID="CtrFCDInstancy" runat="server" RootID="1024" OnChangeScript="GetServiceLevel();"
                                TextToolTip="紧急度" />
                        </td>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitEffectName" runat="server" Text="影响度"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc6:ctrFlowCataDropListNew ID="CtrFCDEffect" Width="300px" runat="server" RootID="1023"
                                OnChangeScript="GetServiceLevel();" TextToolTip="影响度" />
                        </td>
                    </tr>
                    <tr runat="server" id="trInstancyName" visible="false">
                        <td class="listTitleRight">
                            <asp:Literal ID="LitServiceKind" runat="server" Text="事件性质"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc6:ctrFlowCataDropListNew ID="ctrFCDWTType" runat="server" RootID="1002" />
                        </td>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitSubject" runat="server" Text="摘要"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc4:CtrFlowFormText ID="CtrFlowFTSubject" Width="95%" runat="server" TextToolTip="摘要"
                                MaxLength="100" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitReason" runat="server" Text="申请原因"></asp:Literal>
                        </td>
                        <td class="list" colspan="3">
                                <uc12:CtrFlowRemark ID="CtrFlowFormText1" runat="server" MustInput="true" MaxLength="300" TextToolTip="申请原因" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitContent" runat="server" Text="详细描述"></asp:Literal>
                        </td>
                        <td colspan="3" class="list">
                        
                            <uc12:CtrFlowRemark ID="txtContent" runat="server" MustInput="true" MaxLength="500"
                                TextToolTip="详细描述" />
                        </td>
                    </tr>
                    <tr id="trShowServiceLevel" runat="server">
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="LitServiceLevel" runat="server" Text="服务级别"></asp:Literal>
                        </td>
                        <td class="list" colspan="3">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 100%">
                                        <asp:TextBox ID="txtServiceLevel" runat="server" WWidth="59%" ReadOnly="True"></asp:TextBox>
                                        <input id="cmdPopServiceLevel" onclick="ServiceLevelSelect(this);" runat="server"
                                            class="btnClass2" type="button" value="..." />
                                        <input id="hidServiceLevelID" runat="server" type="hidden" />
                                        <input id="hidServiceLevel" runat="server" type="hidden" />
                                        <input id="hidServiceLevelChange" runat="server" value="false" type="hidden" />
                                        <asp:Label ID="labServiceLevel" runat="server" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trServiceLevelDetail" runat="server">
                                    <td style="width: 100%;">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 45%">
                                                    <div id="divSLDefinition" runat="server" style="width: 100%">
                                                    </div>
                                                </td>
                                                <td style="width: 45%">
                                                    <div id="divSLTimeLimt" runat="server" style="width: 100%">
                                                    </div>
                                                </td>
                                                <td style="width: 5%">
                                                    <asp:LinkButton ID="lnkServiceLevel" runat="server" ForeColor="#0000C0" OnClientClick="OpenServiceLevelDetail(this);"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="listTitleRight">
                            <asp:Literal ID="LitCloseReason" runat="server" Text="关闭理由"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc6:ctrFlowCataDropListNew ID="CtrCloseReason" runat="server" RootID="1040" MustInput="false"
                                TextToolTip="关闭理由" />
                        </td>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitReSouse" runat="server" Text="事件来源"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc6:ctrFlowCataDropListNew ID="CtrReSouse" runat="server" RootID="1041" MustInput="false"
                                TextToolTip="事件来源" />
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="listTitleRight">
                            <asp:Literal ID="LitDealStatus" runat="server" Text="事件状态"></asp:Literal>
                        </td>
                        <td class="list">
                            <asp:Label ID="lblDealStatus" Text="" runat="server"></asp:Label>
                            <input id="hidDealStatusID" type="hidden" runat="server" value="0" />
                            <input id="hidDealStatus" type="hidden" runat="server" value="" />
                        </td>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitIssueRoot" runat="server" Text="事件根源"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc6:ctrFlowCataDropListNew ID="CtrFlowIssueRoot" runat="server" RootID="1054" />
                        </td>
                    </tr>
                    <tr id="ShowHFList" runat="server" style="display: none;">
                        <td colspan="4" class="list">
                            <asp:Literal ID="ltlHFList" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr id="ShowFeedBack1" runat="server">
                        <td colspan="4" class="list">
                            <div>
                                <uc3:CtrFeedBack ID="CtrFeedBack2" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>
                <table id="Tables1" width="100%" align="center" class="listContent" style="display: none;"
                    border="0" cellspacing="1" cellpadding="1">
                    <tr>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="LitOuttime" runat="server" Text="派出时间"></asp:Literal>
                        </td>
                        <td class="list" style="width: 35%">
                            <uc1:ctrdateandtime ID="CtrDTOutTime" runat="server" />
                        </td>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="LitServiceTime" runat="server" Text="上门时间"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc1:ctrdateandtime ID="CtrDTServiceTime" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="LitSjwxr" runat="server" Text="执行人"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc9:ServiceStaffMastCust ID="Sjwxr" runat="server" OnKeyDownScript="InitText();" />
                        </td>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitFinishedTime" runat="server" Text="处理完成时间"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc1:ctrdateandtime ID="CtrDTFinishedTime" runat="server" AllowNull="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="LitDealContent" runat="server" Text="措施及结果"></asp:Literal>
                        </td>
                        <td class="list" colspan="3">
                            <ftb:FreeTextBox ID="freeTextBox1" runat="server" Width="100%" ButtonPath="../Forms/images/epower/officexp/"
                             StartMode="HtmlMode"
                                ImageGalleryPath="Attfiles\\Photos" Height="100px">
                            </ftb:FreeTextBox>
                            <asp:Label ID="labDealContent" runat="server" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap align="left" bgcolor="#ccffff" colspan="4" class="listTitleNew">
                            费用明细
                        </td>
                    </tr>
                    <tr>
                        <td nowrap align="center" colspan="4" class="list">
                            <asp:DataGrid ID="gvBillItem" runat="server" Width="100%" DataKeyField="CostID" OnItemCommand="gvBillItem_ItemCommand"
                                ShowFooter="True" border="0" CellSpacing="1" CellPadding="1">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="费用项">
                                        <HeaderStyle Width="30%" Wrap="true"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDFareName" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.FareName")%>'></asp:Label>
                                            <asp:TextBox ID="txtDFareName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FareName")%>'
                                                onkeydown="enterkey(this,'txtDFareName');" Width="90%" MaxLength="50"></asp:TextBox>
                                            <input id="cmdDFare" onclick="FareSelect(this,'1')" type="button" value="..." name="cmdSubject"
                                                runat="server" class="btnClass2" visible="false">
                                            <input id="hidDFareCode" style="width: 56px;" type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.FareCode")%>'>
                                            <input id="hidDFareName" style="width: 56px;" type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.FareName")%>'>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtAddDFareName" runat="server" Text='' onkeydown="enterkey(this,'txtAddDFareName');"
                                                Width="90%" MaxLength="50"></asp:TextBox>
                                            <input id="cmdAddDFare" onclick="FareSelect(this,'2')" type="button" value="..."
                                                runat="server" class="btnClass2" visible="false">
                                            <input id="hidAddDFareCode" style="width: 36px;" type="hidden" runat="server" value=''>
                                            <input id="hidAddDFareName" style="width: 56px;" type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.FareName")%>'>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="型号" Visible="False">
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDModelName" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.ModelName")%>'></asp:Label>
                                            <asp:TextBox ID="txtDModelName" Text='<%# DataBinder.Eval(Container, "DataItem.ModelName")%>'
                                                onkeydown="enterkey(this,'txtDModelName');" runat="server" Width="90%" MaxLength="50"
                                                ReadOnly="true"></asp:TextBox>
                                            <input id="hidDModelName" style="width: 56px;" type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.ModelName")%>'>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtAddDModelName" runat="server" Text='' onkeydown="enterkey(this,'txtAddDModelName');"
                                                Width="90%" MaxLength="50" ReadOnly="true"></asp:TextBox>
                                            <input id="hidAddDModelName" style="width: 56px;" type="hidden" runat="server" value=''>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="单价">
                                        <HeaderStyle Width="8%"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDPrice" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Price","{0:F2}")%>'></asp:Label>
                                            <asp:TextBox ID="txtDPrice" Style="ime-mode: Disabled" Text='<%# DataBinder.Eval(Container, "DataItem.Price","{0:F2}")%>'
                                                onkeydown="NumberInputlocal();enterkey(this,'txtDPrice');" onblur="CheckIsnum(this,'txtDPrice');"
                                                runat="server" Width="90%" MaxLength="10">
                                            </asp:TextBox>
                                            <input id="hidDPrice" style="width: 56px;" type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Price","{0:F2}")%>'>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtAddDPrice" Style="ime-mode: Disabled" runat="server" Text=''
                                                onkeydown="NumberInputlocal();enterkey(this,'txtAddDPrice');" onblur="CheckIsnumAdd(this,'txtAddDPrice');"
                                                Width="90%" MaxLength="10"></asp:TextBox>
                                            <input id="hidAddDPrice" style="width: 56px;" type="hidden" runat="server" value=''>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="数量">
                                        <HeaderStyle Width="8%"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDQuantity" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Quantity","{0:F0}")%>'></asp:Label>
                                            <asp:TextBox ID="txtDQuantity" Style="ime-mode: Disabled" Text='<%# DataBinder.Eval(Container, "DataItem.Quantity","{0:F0}")%>'
                                                onkeydown="NumberInput('');enterkey(this,'txtDQuantity');" onblur="CheckIsnum(this,'txtDQuantity');"
                                                runat="server" Width="90%" MaxLength="10"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtAddDQuantity" runat="server" Style="ime-mode: Disabled" Text=''
                                                onkeydown="NumberInput('');enterkey(this,'txtAddDQuantity');" onblur="CheckIsnumAdd(this,'txtAddDQuantity');"
                                                Width="90%" MaxLength="10"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="小计">
                                        <HeaderStyle Width="8%"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDFareAmount" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.FareAmount","{0:F2}")%>'></asp:Label>
                                            <asp:TextBox ID="txtDFareAmount" Style="ime-mode: Disabled" Text='<%# DataBinder.Eval(Container, "DataItem.FareAmount","{0:F2}")%>'
                                                onkeydown="NumberInputlocal();enterkey(this,'txtDFareAmount');" onblur="CheckIsnum(this,'txtDFareAmount');"
                                                runat="server" Width="90%" ReadOnly="True"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtAddDFareAmount" Style="ime-mode: Disabled" runat="server" Text=''
                                                onkeydown="NumberInputlocal();enterkey(this,'txtAddDFareAmount');" onblur="CheckIsnumAdd(this,'txtAddDFareAmount');"
                                                Width="90%" MaxLength="10" ReadOnly="True"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="其它">
                                        <HeaderStyle Width="8%"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDHumanAmount" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.HumanAmount","{0:F2}")%>'></asp:Label>
                                            <asp:TextBox ID="txtDHumanAmount" Style="ime-mode: Disabled" Text='<%# DataBinder.Eval(Container, "DataItem.HumanAmount","{0:F2}")%>'
                                                onkeydown="NumberInputlocal();enterkey(this,'txtDHumanAmount');" onblur="CheckIsnum(this,'txtDHumanAmount');"
                                                runat="server" Width="90%" MaxLength="10"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtAddDHumanAmount" Style="ime-mode: Disabled" runat="server" Text=''
                                                onkeydown="NumberInputlocal();enterkey(this,'txtAddDHumanAmount');" onblur="CheckIsnumAdd(this,'txtAddDHumanAmount');"
                                                Width="90%" MaxLength="10"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="合计">
                                        <HeaderStyle Width="8%"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDTotalAmount" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.TotalAmount","{0:F2}")%>'></asp:Label>
                                            <asp:TextBox ID="txtDTotalAmount" runat="server" Style="ime-mode: Disabled" Text='<%# DataBinder.Eval(Container, "DataItem.TotalAmount","{0:F2}")%>'
                                                Width="90%" ReadOnly="True">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtAddDTotalAmount" Style="ime-mode: Disabled" runat="server" Text=''
                                                Width="90%" MaxLength="10" ReadOnly="True"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="备注">
                                        <HeaderStyle Width="25%"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDRemark" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Remark")%>'></asp:Label>
                                            <asp:TextBox ID="txtDRemark" Text='<%# DataBinder.Eval(Container, "DataItem.Remark")%>'
                                                onkeydown="enterkey(this,'txtDRemark');" runat="server" Width="90%"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtAddDRemark" runat="server" Text='' Width="90%" onkeydown="enterkey(this,'txtAddDRemark');"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle Width="5%" VerticalAlign="Top"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Button ID="btndelete" CommandName="Delete" runat="server" Text="删除" CausesValidation="False"
                                                SkinID="btnClass1"></asp:Button>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button ID="btnadd" CommandName="Add" runat="server" Text="新增" CausesValidation="False"
                                                SkinID="btnClass1"></asp:Button>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                                </PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitTotalAmount" runat="server" Text="总计金额"></asp:Literal>
                        </td>
                        <td class="list">
                            <asp:Label ID="labTotalAmount" runat="server"></asp:Label>
                        </td>
                        <td nowrap class="listTitleRight">
                            <asp:Literal ID="LitTotalHours" runat="server" Text="合计小时"></asp:Literal>
                        </td>
                        <td class="list">
                            <asp:Label ID="labTotalHours" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr id="ShowFlowLimit" runat="server" align="left">
                        <td nowrap class="listTitleRight">
                            <asp:Literal ID="LitExpectedTime" runat="server" Text="处理时限"></asp:Literal>
                        </td>
                        <td colspan="3" class="list">
                            <asp:Label ID="labExpectedTime" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table id="Tables2" width="100%" align="center" class="listContent" style="display: none;"
                    border="0" cellspacing="1" cellpadding="1">
                    <tr id="trShowMonitor" width="100%" runat="server">
                        <td width="12%" class="listTitleRight">
                            <asp:Literal ID="LitMonitor" runat="server" Text="督办内容"></asp:Literal>
                        </td>
                        <td colspan="3" width="90%" class="list" style="word-break: break-all">
                            <uc5:CtrMonitor ID="CtrMonitor1" runat="server" />
                        </td>
                    </tr>
                    <tr id="ShowFeedBack" runat="server">
                        <td colspan="4" class="list">
                            <div>
                                <uc3:CtrFeedBack ID="CtrFeedBack1" runat="server" />
                            </div>
                            <div style="text-align: center;">
                                <asp:Button ID="btnEmailBack" runat="server" Text="邮件回访" Visible="false" OnClientClick="SendMail(1)" />
                                <asp:Label ID="lblEmailBack" runat="server" Text="未邮件回访" Font-Bold="True" Font-Size="Small"
                                    ForeColor="Red"></asp:Label>
                            </div>
                        </td>
                    </tr>
                </table>
                <table id="Tables5" width="100%" align="center" class="listContent" style="display: none;"
                    border="0" cellspacing="1" cellpadding="1">
                    <tr>
                        <td align="right" class="listTitleRight">
                            <iframe id='Iframe5' name="Iframe5" src="" width='100%' height='300' scrolling='auto'
                                frameborder='no'></iframe>
                        </td>
                    </tr>
                </table>
                <table id="Tables6" width="100%" align="center" class="listContent" style="display: none;"
                    border="0" cellspacing="1" cellpadding="1">
                    <tr>
                        <td align="right" class="listTitleRight">
                            <iframe id='Iframe6' name="Iframe6" src="" width='100%' height='300' scrolling='auto'
                                frameborder='no'></iframe>
                        </td>
                    </tr>
                </table>
                <table id="Tables3" width="100%" align="center" class="listContent" style="display: none;"
                    border="0" cellspacing="1" cellpadding="1">
                    <tr>
                        <td align="right" class="listTitleRight">
                            <iframe id='Iframe3' name="Iframe3" src="" width='100%' height='100' scrolling='auto'
                                frameborder='no'></iframe>
                        </td>
                    </tr>
                </table>
                <table id="Tables4" width="100%" align="center" class="listContent" style="display: none;"
                    border="0" cellspacing="1" cellpadding="1">
                    <tr>
                        <td align="right" class="listTitleRight">
                            <iframe id='Iframe4' name="Iframe4" src="" width='100%' height='100' scrolling='auto'
                                frameborder='no'></iframe>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        function SendMail(sType) {
            window.open("../Common/frmSendMailFeedBack.aspx?Type=" + sType + "&randomid=" + GetRandom() + "&FlowID=" + '<%=FlowID%>', '', 'scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600');
            event.returnValue = false;
        }
    </script>

    <script type="text/javascript" language="javascript">
    var selectobj = document.getElementById("name0");
    
    function chang(obj,img){
            if(obj!=selectobj)	    
		        obj.background="../Images/"+img;
    }
    
function chang_Class(name,num,my)
{
	for(i=0;i<num;i++){
		if(i!=my){
			document.getElementById(name+i).className="STYLE4";			
			$("#" + name+i ).css("background-image","url(../Images/lm-a.gif)");
			document.getElementById("a"+i).className="STYLE4";
		}
	}
	selectobj = document.getElementById(name+my);
	document.getElementById(name+my).className="td_3";	
		
	$("#" + name+my ).css("background-image","url(../Images/lm-b.gif)");
	document.getElementById("a"+my).className="td_3";

	switch(my)
	{
        case 0:     //基本信息
            document.getElementById("Tables0").style.display ="";
	        document.getElementById("Tables1").style.display ="none";
	        document.getElementById("Tables2").style.display ="none";
	        document.getElementById("Tables3").style.display ="none";
	        document.getElementById("Tables4").style.display ="none";
	        document.getElementById("Tables5").style.display ="none";
	        document.getElementById("Tables6").style.display ="none";
	        break;
	    case 1:    //处理信息
            document.getElementById("Tables0").style.display ="none";
	        document.getElementById("Tables1").style.display ="";
	        document.getElementById("Tables2").style.display ="none";
	        document.getElementById("Tables3").style.display ="none";
	        document.getElementById("Tables4").style.display ="none";
	        document.getElementById("Tables5").style.display ="none";
	        document.getElementById("Tables6").style.display ="none";
	        
	        if (window.FTB_API && window.FTB_API['ctl00_ContentPlaceHolder1_freeTextBox1']){
                window.FTB_API['ctl00_ContentPlaceHolder1_freeTextBox1'].GoToDesignMode();
            }
	    break;
	    case 2:  //督办/回访
            document.getElementById("Tables0").style.display ="none";
	        document.getElementById("Tables1").style.display ="none";
	        document.getElementById("Tables2").style.display ="";
	        document.getElementById("Tables3").style.display ="none";
	        document.getElementById("Tables4").style.display ="none";
	        document.getElementById("Tables5").style.display ="none";
	        document.getElementById("Tables6").style.display ="none";
	    break;
	    case 3:   //相关变更单

            document.getElementById("Tables0").style.display ="none";
	        document.getElementById("Tables1").style.display ="none";
	        document.getElementById("Tables2").style.display ="none";
	        document.getElementById("Tables3").style.display ="";
	        document.getElementById("Tables4").style.display ="none";
	        document.getElementById("Tables5").style.display ="none";
	        document.getElementById("Tables6").style.display ="none";
	        if(Iframe3.location =="about:blank")
	        {
                Iframe3.location="../EquipmentManager/frmShowEquRel.aspx?EquID=" + '<%=ChangeFlowID%>' + "&randomid="+GetRandom()+"&Type=7";   
            }
	    break;
	    case 4:   //相关问题单

            document.getElementById("Tables0").style.display ="none";
	        document.getElementById("Tables1").style.display ="none";
	        document.getElementById("Tables2").style.display ="none";
	        document.getElementById("Tables3").style.display ="none";
	        document.getElementById("Tables4").style.display ="";
	        document.getElementById("Tables5").style.display ="none";
	        document.getElementById("Tables6").style.display ="none";
	        if(Iframe4.location =="about:blank")
	        {
                Iframe4.location="../EquipmentManager/frmShowEquRel.aspx?EquID=" + '<%=ProblemFlowID%>' + "&randomid="+GetRandom()+"&Type=6";
            }
	    break;
	     case 5:  //知识参考

            document.getElementById("Tables0").style.display ="none";
	        document.getElementById("Tables1").style.display ="none";
	        document.getElementById("Tables2").style.display ="none";
	        document.getElementById("Tables3").style.display ="none";
	        document.getElementById("Tables4").style.display ="none";
	        document.getElementById("Tables5").style.display ="";
	        document.getElementById("Tables6").style.display ="none";
	        
	        var sKey = "";
            sKey=document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_hidCatalogName").value;
            Iframe5.location="../InformationManager/frmInf_InformationMain.aspx?IsSelect=1&randomid="+GetRandom()+"&PKey=" + encodeURIComponent(sKey);
	    break;
	    case 6:  //配置信息
            document.getElementById("Tables0").style.display ="none";
	        document.getElementById("Tables1").style.display ="none";
	        document.getElementById("Tables2").style.display ="none";
	        document.getElementById("Tables3").style.display ="none";
	        document.getElementById("Tables4").style.display ="none";
	        document.getElementById("Tables5").style.display ="none";
	        document.getElementById("Tables6").style.display ="";
            var CustID = document.all.<%=hidCustID.ClientID%>.value;
            if(CustID=="")
            {
                CustID = 0;
            }
            var EquID = document.all.<%=hidEqu.ClientID%>.value;
            if(EquID=="")
            {
                EquID = 0;
            }
            Iframe6.location="frmServiceProtocolShow.aspx?ID=" + CustID + "&randomid="+GetRandom()+"&EquID=" + EquID;
	    break;
	 }
}
    </script>

    <script type="Text/javascript" language="javascript">
        var temp = 0;
        for (i = 0; i < 6; i++) {
            if (i != temp) {
                document.getElementById("name" + i).className = "STYLE4";
                document.getElementById("name" + i).background = "../Images/lm-a.gif"; ;
                document.getElementById("a" + i).className = "STYLE4";
                document.getElementById("name" + i).style.display = "";
            }
        }
        if ('<%=MessageID%>' == "0") {
            document.getElementById("name2").style.display = "none";
        }
        if ('<%=ChangeFlowID%>' == "") {
            document.getElementById("name3").style.display = "none";
        }
        if ('<%=ProblemFlowID%>' == "") {
            document.getElementById("name4").style.display = "none";
        }
        var CustomVisble = '<%=StrCustomVisble()%>';
        if (CustomVisble == "bool") {
            document.getElementById("name6").style.display = "none";
        }

        document.getElementById("name" + temp).className = "";
        document.getElementById("name" + temp).background = "../Images/lm-b.gif"; 
    </script>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="744px" ShowMessageBox="True"
        ShowSummary="False" HeaderText="对不起,您输入的数据不完整,请正确输入以下数据:"></asp:ValidationSummary>
</asp:Content>
