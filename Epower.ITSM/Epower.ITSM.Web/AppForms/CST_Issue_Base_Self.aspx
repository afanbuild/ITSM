<%@ Page Language="C#" MasterPageFile="~/FlowForms.Master" AutoEventWireup="true"
    CodeBehind="CST_Issue_Base_Self.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.CST_Issue_Base_Self"
    Title="网络管理流程" %>

<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc4" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc8" %>
<%@ Register Src="~/Controls/Extension_DaySchemeCtrList.ascx" TagName="Extension_DayCtrList" TagPrefix="uc13" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript">
    
    //拼凑待办事项标题
    function TransferValue()
    {
      //拼凑待办事项标题
       if (typeof(document.all.<%=hidSubject.ClientID%>)!="undefined" )
       {
          if(document.all.<%=hidSubject.ClientID%>.value.trim()!="")
          {
	            parent.header.flowInfo.Subject.value = document.all.<%=hidSubject.ClientID%>.value.trim();
	      }
       }
    }
    
    //脚本检查是否输入

	function DoUserValidate(lngActionID,strActionName)
    {
        TransferValue();
	    return CheckCustAndType();
    }
    
    //获取AJAX
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
    
    //检查是否选择了事件来源和事件类型
	function CheckCustAndType()
	{
	   if(document.getElementById('<%=txtContent.ClientID%>') !=null && document.getElementById('<%=txtContent.ClientID%>').value=='<%=KeyValue%>')
	   {
	        alert("详细描述不能为空！");
	        document.getElementById('<%=txtContent.ClientID%>').focus();
            return false;	        
	   }
	    return true;
	}
	//信息折叠		
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
    //获取客户
    function GetCustID(obj)
    {
        if(obj.value.trim()=="")
            {
                document.getElementById(obj.id.replace("txtCustAddr","hidCustID") ).value.trim()!="0";
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
												        alert("此客户不存在，请重新查找！");	
	                                                    document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = "0";    //客户ID号	
		                                                document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr") ).value="";//客户名称	
		                                                
		                                                document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = "";
		                                                document.getElementById(obj.id.replace("txtCustAddr","txtAddr")).value = "";   //地址
		                                                document.getElementById(obj.id.replace("txtCustAddr","hidAddr") ).value = "";   //地址
		                                                document.getElementById(obj.id.replace("txtCustAddr","txtContact")).value = "";    //联系人
		                                                document.getElementById(obj.id.replace("txtCustAddr","hidContact")).value = "";   
		                                                document.getElementById(obj.id.replace("txtCustAddr","txtCTel")).value = "";   //联系人电话
		                                                document.getElementById(obj.id.replace("txtCustAddr","hidTel")).value = "";			                                                            
		                                                document.getElementById(obj.id.replace("txtCustAddr","lblCustDeptName")).innerHTML = "";   //所属部门
	                                                    document.getElementById(obj.id.replace("txtCustAddr","lblEmail")).innerHTML = "";  //电子邮件
	                                                    document.getElementById(obj.id.replace("txtCustAddr","lblMastCust")).innerHTML = "";   //服务单位
	                                                    document.getElementById(obj.id.replace("txtCustAddr","hidCustDeptName")).value = "";   //所属部门
	                                                    document.getElementById(obj.id.replace("txtCustAddr","hidCustEmail")).value = "";  //电子邮件
	                                                    document.getElementById(obj.id.replace("txtCustAddr","hidMastCust")).value = "";   //服务单位
	                                                    document.getElementById(obj.id.replace("txtCustAddr","lbljob")).innerHTML = "";   //职位
	                                                    document.getElementById(obj.id.replace("txtCustAddr","hidjob")).value = "";   //职位	
	                                                    obj.focus();
														obj.select();                                                 
    	                                               	                                         
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
	                                                                document.getElementById(obj.id.replace("txtCustAddr","txtContact")).value = record[i].linkman1;    //联系人


	                                                                document.getElementById(obj.id.replace("txtCustAddr","hidContact")).value = record[i].linkman1;   
	                                                                document.getElementById(obj.id.replace("txtCustAddr","txtCTel")).value = record[i].tel1;   //联系人电话


	                                                                document.getElementById(obj.id.replace("txtCustAddr","hidTel")).value = record[i].tel1;
	                                                                document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = record[i].id;    //客户ID号

                                            			            
	                                                                document.getElementById(obj.id.replace("txtCustAddr","lblCustDeptName")).innerHTML = record[i].custdeptname;   //所属部门
                                                                    document.getElementById(obj.id.replace("txtCustAddr","lblEmail")).innerHTML = record[i].email;  //电子邮件
                                                                    document.getElementById(obj.id.replace("txtCustAddr","lblMastCust")).innerHTML = record[i].mastcustname;   //服务单位
                                                                    document.getElementById(obj.id.replace("txtCustAddr","hidCustDeptName")).value = record[i].custdeptname;   //所属部门
                                                                    document.getElementById(obj.id.replace("txtCustAddr","hidCustEmail")).value = record[i].email;  //电子邮件
                                                                    document.getElementById(obj.id.replace("txtCustAddr","hidMastCust")).value = record[i].mastcustname;   //服务单位
                                                                    document.getElementById(obj.id.replace("txtCustAddr","lbljob")).innerHTML = record[i].job;   //职位
                                                                    document.getElementById(obj.id.replace("txtCustAddr","hidjob")).value = record[i].job;   //职位
                                            			            
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
	//客户选择
	function CustomSelect(obj) 
	{
	    var CustName = document.all.<%=txtCustAddr.ClientID%>.value;
	    var CustAddress = document.all.<%=txtAddr.ClientID%>.value;
	    var CustLinkMan = document.all.<%=txtContact.ClientID%>.value;
	    var CustTel = document.all.<%=txtCTel.ClientID%>.value;	    
	    var url="../Common/frmDRMUserSelectajax.aspx?IsSelect=true&randomid="+GetRandom()+
	    "&CustID=" + document.getElementById(obj.id.replace('cmdCust','hidCustID')).value 
		    + "&FlowID="+ '<%=FlowID%>' + "&CustName=" + escape(CustName) 
		    + "&CustAddress=" + escape(CustAddress)
		    + "&CustLinkMan=" + escape(CustLinkMan) 
		    + "&CustTel=" + escape(CustTel)
		    +"&TypeFrom=CST_Issue_Base_self&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
	    
	    window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=600,left=150,top=50");	  
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
	            document.getElementById(obj.id.replace("txtCustAddr","txtContact")).value = record[i].linkman1;    //联系人


	            document.getElementById(obj.id.replace("txtCustAddr","hidContact")).value = record[i].linkman1;   
	            document.getElementById(obj.id.replace("txtCustAddr","txtCTel")).value = record[i].tel1;   //联系人电话


	            document.getElementById(obj.id.replace("txtCustAddr","hidTel")).value = record[i].tel1;
	            document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = record[i].id;    //客户ID号

	           
	            document.getElementById(obj.id.replace("txtCustAddr","lblCustDeptName")).innerHTML = record[i].custdeptname;   //所属部门
                document.getElementById(obj.id.replace("txtCustAddr","lblEmail")).innerHTML = record[i].email;  //电子邮件
                document.getElementById(obj.id.replace("txtCustAddr","lblMastCust")).innerHTML = record[i].mastcustname;   //服务单位
                document.getElementById(obj.id.replace("txtCustAddr","hidCustDeptName")).value = record[i].custdeptname;   //所属部门
                document.getElementById(obj.id.replace("txtCustAddr","hidCustEmail")).value = record[i].email;  //电子邮件
                document.getElementById(obj.id.replace("txtCustAddr","hidMastCust")).value = record[i].mastcustname;   //服务单位
                document.getElementById(obj.id.replace("txtCustAddr","lbljob")).innerHTML = record[i].job;   //职位
                document.getElementById(obj.id.replace("txtCustAddr","hidjob")).value = record[i].job;   //职位
	            
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
	        var CustName = document.all.<%=txtCustAddr.ClientID%>.value.trim();
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
														    obj.focus();
														    obj.select();
														    document.getElementById(obj.id.replace("txtEqu","hidEqu")).value=0;
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
        
         function SelectSomeEqu(obj)   //选择多个设备
			{
			    var newDateObj = new Date();
	            var sparamvalue =  newDateObj.getYear().toString() + newDateObj.getMonth().toString();
	            var name = obj.value.trim();
	            if(name.trim()=="")
	            {
	                return;
	            }
	            var ListID = document.all.<%=hidListID.ClientID%>.value.trim();
	            var CustName = document.all.<%=txtCustAddr.ClientID%>.value.trim();
				var	value=window.showModalDialog("../mydestop/frmQuickLocateEqu.aspx?IsSelect=true&randomid="+GetRandom()+"&Name=" + escape(name) + "&EquCust=" + escape(CustName)+"&EquipmentCatalogID="+ListID,"","dialogHeight:500px;dialogWidth:600px");
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
			
	//设备
	function SelectEqu(obj) 
	{
	    var EquipmentCatalogID = document.all.<%=hidListID.ClientID%>.value.trim();
            			        
	    var EquName = document.all.<%=txtEqu.ClientID%>.value.trim();
	    var CustName = document.all.<%=txtCustAddr.ClientID%>.value.trim();
	    var MastCust = document.all.<%=hidMastCust.ClientID%>.value.trim();	    
	    var url="../EquipmentManager/frmEqu_DeskMain.aspx?IsSelect='1'&randomid="
	            +GetRandom()+"&FlowID="+ '<%=FlowID%>' + "&EquName=" 
	            + escape(EquName) + "&Cust=" + escape(CustName) + "&MastCust=" 
	            + escape(MastCust)+"&EquipmentCatalogID="
	            +EquipmentCatalogID+
	            "&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrom=CST_Issue_Base_self";
	    window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=600,left=150,top=50");	    
	}
	
	//选择资产目录
	function SelectEquCatalog(obj)
	{
	        var	value=window.showModalDialog("../EquipmentManager/frmEqu_DeskCateListSel.aspx?random=" + GetRandom(),"","dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no") ;
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
			
    //清空
	function txtKeyNameClear()
    {
        if(document.getElementById('<%=txtContent.ClientID%>').value=='<%=KeyValue%>')
        {
            document.getElementById('<%=txtContent.ClientID%>').value="";
        }
    }
    
    //默认
    function txtKeyNameBack() 
    {
        if (document.getElementById('<%=txtContent.ClientID%>').value == '')
        {
            document.getElementById('<%=txtContent.ClientID%>').value = '<%=KeyValue%>';
        }
    }
    
    //邮件通知
   function SendMail(sType) 
   {
        window.open("../Common/frmSendMailFeedBack.aspx?Type=" + sType + "&randomid=" + GetRandom() + "&FlowID=" + '<%=FlowID%>', '', 'scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600');
        event.returnValue = false;
   }
    String.prototype.trim = function()  //去空格

    {
	    // 用正则表达式将前后空格

	    // 用空字符串替代。

	    return this.replace(/(^\s*)|(\s*$)/g, "").replace("&#160;","");
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
    
     function OpenServiceHistory(type)
	 {
	     if(type=="1")
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
    
    </script>           
    <input id="hidBuildCode" runat="server" type="hidden" />
    <input id="hidServiceNo" runat="server" type="hidden" />
    <input id="hidIssTempID" runat="server" type="hidden" value="0" />
    <input id="hidDealStatusID" runat="server" type="hidden" value="0" />
    <input id="hidDealStatus" runat="server" type="hidden" value="" />
    <input id="hidSubject" type="hidden" runat="server" value="" />
    <input id="hidServiceID" style="width: 56px" type="hidden" name="hidFareID" runat="server" />
    <input id="hidEqu" type="hidden" runat="server" value="-1" />
    <input id="hidCustID" runat="server" type="hidden" value="-1" />
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
    <input id="hidServiceLevelID" runat="server" type="hidden" />
    <input id="hidServiceLevel" runat="server" type="hidden" />
    <input id="hidInstancyID" runat="server" type="hidden" />
    <input id="hidInstancyName" runat="server" type="hidden" />
    <input id="hidEffectID" runat="server" type="hidden" />
    <input id="hidEffectName" runat="server" type="hidden" />
    <input id="hidCloseReasonID" runat="server" type="hidden" />
    <input id="hidCloseReasonName" runat="server" type="hidden" />
    <input id="hidReSouseID" runat="server" type="hidden" />
    <input id="hidReSouseName" runat="server" type="hidden" />
    <input id="hidServiceTypeID" runat="server" type="hidden" />
    <input id="hidServiceType" runat="server" type="hidden" />
    <input id="hidAppID" runat="server" type="hidden" value="0" />
    <input id="hidFlowID" runat="server" type="hidden" value="0" />
    
    <table id="Table12" width="100%" align="center" runat="server" class="listNewContent"
        border="0" cellspacing="2" cellpadding="0">
        <tr>
            <td align="left" valign="bottom" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                align="absbottom" />
                            客户/资产信息
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="listContent" width="100%" align="center" runat="server" id="Table2" border="0"
        cellpadding="1" cellspacing="1">
        <tr>
            <td style="width: 12%" class="listTitleRight">
                <asp:Literal ID="LitCustName" runat="server" Text="客户名称"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:Label ID="labCustAddr" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtCustAddr" runat="server" onblur="GetCustID(this)" MaxLength="200"></asp:TextBox>
                <input id="cmdCust" onclick="CustomSelect(this)" type="button" value="..." runat="server"
                    class="btnClass2" />
                <asp:LinkButton ID="lnkCustHistory" runat="server" ForeColor="#0000C0" OnClientClick="OpenServiceHistory('1');">(历史参照)</asp:LinkButton>
                &nbsp;
            </td>
            <td style="width: 12%" class="listTitleRight">
                <asp:Literal ID="LitCustAddress" runat="server" Text="地址"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblAddr" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtAddr" runat="server" MaxLength="200"></asp:TextBox>&nbsp;
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitContact" runat="server" Text="联系人"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="labContact" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtContact" runat="server" MaxLength="20"></asp:TextBox>&nbsp;
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitCTel" runat="server" Text="联系人电话"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="labCTel" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtCTel" runat="server" MaxLength="20"></asp:TextBox>&nbsp;
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitCustDeptName" runat="server" Text="所属部门"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCustDeptName" runat="server"></asp:Label>
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
            <td class="list">
                <asp:Label ID="lblMastCust" runat="server"></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitJob" runat="server" Text="职位"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lbljob" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitEquipmentName" runat="server" Text="资产名称"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:Label ID="lblEqu" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtEqu" runat="server" Width="120px" MaxLength="80" onblur="GetEquID(this)"></asp:TextBox>
                <input id="cmdEqu" onclick="SelectEqu(this)" type="button" value="..." runat="server"
                    name="cmdEqu" class="btnClass2" />
                <asp:LinkButton ID="lnkEquHistory" runat="server" ForeColor="#0000C0" OnClientClick="OpenServiceHistory('2');">(历史参照)</asp:LinkButton>
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="Table15" width="100%" align="center" runat="server" class="listNewContent" style="display:none;">
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
        cellpadding="2" cellspacing="0">
        <tr runat="server" id="trEqu">
            <td class="listTitleRight" style="width: 12%;display:none;">
                <asp:Literal ID="LitEquList" runat="server" Text="资产目录"></asp:Literal>
            </td>
            <td class="list" style="width: 35%;display:none;">
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
    <table id="Tables0" width="100%" align="center" class="listContent" border="0">
        <tr>
            <td nowrap align="left" class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitContent" runat="server" Text="详细描述"></asp:Literal>
            </td>
            <td class="list" style="word-break: break-all">
                <asp:TextBox ID="txtContent" runat="server" Width="95%" MaxLength="500" Rows="4"
                    TextMode="MultiLine" onblur="txtKeyNameBack();" onmouseover="this.style.backgroundColor='#FFFBE8'"
                    onmouseout="this.style.backgroundColor='#FFFFFF'" onclick="txtKeyNameClear();"></asp:TextBox><asp:Label
                        ID="labContent" runat="server" Visible="False"></asp:Label><asp:Label ID="rWarning"
                            runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="4" class="list">
               <uc13:Extension_DayCtrList ID="Extension_DayCtrList1" runat="server" />
            </td>
        </tr>
    </table>
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="744px" ShowMessageBox="True"
        ShowSummary="False" HeaderText="对不起,您输入的数据不完整,请正确输入以下数据:"></asp:ValidationSummary>
</asp:Content>
