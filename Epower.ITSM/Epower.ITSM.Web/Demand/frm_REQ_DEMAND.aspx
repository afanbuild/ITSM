<%@ Page Language="C#" MasterPageFile="~/FlowForms.Master" AutoEventWireup="true"
    CodeBehind="frm_REQ_DEMAND.aspx.cs" Inherits="Epower.ITSM.Web.Demand.frm_REQ_DEMAND"
    Title="需求登单" %>

<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc8" %>
<%@ Register Src="../Controls/ServiceStaff.ascx" TagName="ServiceStaff" TagPrefix="uc7" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/CtrMonitor.ascx" TagName="CtrMonitor" TagPrefix="uc5" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc4" %>
<%@ Register Src="../Controls/common/DictionaryPicker.ascx" TagName="DictionaryPicker"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc1" %>
<%@ Register Src="../Controls/CtrFeedBack.ascx" TagName="CtrFeedBack" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ServiceStaffMastCust.ascx" TagName="ServiceStaffMastCust"
    TagPrefix="uc9" %>
<%@ Register Src="../Controls/ctrbuttons.ascx" TagName="ctrbuttons" TagPrefix="uc10" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc11" %>
<%@ Register Src="../Controls/CtrFlowRemark.ascx" TagName="CtrFlowRemark" TagPrefix="uc12" %>
<%@ Register Src="~/Controls/Extension_DaySchemeCtrList.ascx" TagName="Extension_DayCtrList"
    TagPrefix="uc13" %>
<%@ Register Src="~/Controls/Catalog_DymSchameCtrList.ascx" TagName="Catalog_DymSchameCtrList"
    TagPrefix="uc14" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../Js/Plugin/jquery.autocomplete.js"></script>

    <link rel="stylesheet" type="text/css" href="../Js/themes/jquery.autocomplete.css" />
    <style type="text/css">
        body:nth-of-type(1) .other_fix_width_for_chrome
        {
            width: 100px !important;
        }
        .fixed-grid-border2
        {
            border-right: 1px #A3C9E1 solid;
        }
        .fixed-grid-border2 td
        {
            border-left: solid 1px #CEE3F2;
            border-right: 0px;
        }
        .fixed-grid-border2 tr
        {
            border-bottom: solid 1px #CEE3F2;
            border-top: solid 1px #CEE3F2;
        }
    </style>

  <script language="javascript" type="text/javascript">
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

     //脚本检查是否输入
	function DoUserValidate(lngActionID,strActionName)
    {
        TransferValue();
	    return CheckCustAndType();
    }
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
                var txtAddr = document.all.<%=txtAddr.ClientID%>.value;
                var txtContact = document.all.<%=txtContact.ClientID%>.value;
                var txtTell = document.all.<%=txtCTel.ClientID%>.value;
                var lblCustDeptName = document.all.<%=lblCustDeptName.ClientID%>.outerText;
                var lblEmail = document.all.<%=lblEmail.ClientID%>.outerText;
                var lblMastCust = document.all.<%=lblMastCust.ClientID%>.outerText;
                var lbljob = document.all.<%=lbljob.ClientID%>.outerText;
                
                var url="../AppForms/frmBr_ECustomerEdit.aspx?Page=IssueBase&randomid="+GetRandom()+"&CustName=" + escape(txtCustName) + "&Addr=" + escape(txtAddr) + "&Contact=" + escape(txtContact)
                    + "&Tell="+ escape(txtTell) + "&CustDeptName=" + escape(lblCustDeptName) + "&Email=" + escape(lblEmail) + "&MastCust=" 
                    + escape(lblMastCust) +"&job=" + escape(lbljob)+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
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
                var txtEquPos = "";
                
                var url="../EquipmentManager/frmEqu_DeskEdit.aspx?subjectid=1&randomid="+GetRandom()+"&Page=IssueBase&EquName=" + escape(txtEquName) + "&EquPos=" + escape(txtEquPos)+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=DemandBase";
                 window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=100");           
            }
		
		//客户选择
			function CustomSelect(obj) 
			{
			    var CustName = document.all.<%=txtCustAddr.ClientID%>.value;
			    var CustAddress = document.all.<%=txtAddr.ClientID%>.value;
			    var CustLinkMan = document.all.<%=txtContact.ClientID%>.value;
			    var CustTel = document.all.<%=txtCTel.ClientID%>.value;
		
				var url="../Common/frmDRMUserSelectajax.aspx?IsSelect=true&randomid="+GetRandom()+"&CustID=" + document.getElementById(obj.id.replace('cmdCust','hidCustID')).value 
				            + "&FlowID="+ '<%=FlowID%>' + "&CustName=" + escape(CustName)  + "&CustAddress=" + escape(CustAddress)
				            + "&CustLinkMan=" + escape(CustLinkMan) + "&CustTel=" + escape(CustTel)
				            + "&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrom=DemandBase&PageType=4" ;
				open(url, 'E8OpenWin', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=50');				
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
	            var url="../mydestop/frmQuickLocateCustAjax.aspx?IsSelect=true&randomid="+GetRandom()+"&Name=" + escape(name)+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&PageType=3";
  
	            window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=600");	            
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
		            
		            document.getElementById(obj.id.replace("txtCustAddr","txtEqu")).value="";        //资产名称
	                document.getElementById(obj.id.replace("txtCustAddr","hidEquName")).value="";   //资产名称
	                document.getElementById(obj.id.replace("txtCustAddr","hidEqu")).value="0";   //资产id
	                document.getElementById(obj.id.replace("txtCustAddr","hidListName")).value="";   //资产目录
	                document.getElementById(obj.id.replace("txtCustAddr","hidListID")).value="0";   //资产目录ID
	                return;
	            }
	            if(obj.value.trim()==document.getElementById(obj.id.replace("txtCustAddr","hidCust")).value.trim() && (document.getElementById(obj.id.replace("txtCustAddr","hidCustID") ).value.trim()!="" || document.getElementById(obj.id.replace("txtCustAddr","hidCustID") ).value.trim()!="0"))
	            {
	                return;
	            }	            
	             $.ajax({
	                url:"../MyDestop/frmXmlHttpAjax.aspx?randomid="+GetRandom()+"&Cust=" + escape(obj.value),
	                datatype:"json",
	                type:'GET',
	                success:function(data)
	                {
	                    if(data =="-1")
	                    {	                  	                        
						        alert("此客户不存在！");	
                                document.getElementById(obj.id.replace("txtCustAddr","hidCustID") ).value="";
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
            		            
		                        document.getElementById(obj.id.replace("txtCustAddr","txtEqu")).value="";        //资产名称
	                            document.getElementById(obj.id.replace("txtCustAddr","hidEquName")).value="";   //资产名称
	                            document.getElementById(obj.id.replace("txtCustAddr","hidEqu")).value="0";   //资产id
	                            document.getElementById(obj.id.replace("txtCustAddr","hidListName")).value="";   //资产目录
	                            document.getElementById(obj.id.replace("txtCustAddr","hidListID")).value="0";   //资产目录ID                                                                          	                                            
	                    }
                        else if(data=="0") //找到多个
			            {	
			                if($.browser.safari)
			                {
			                    alert("多个客户名称相同，请到旁边选项中选择！");
			                    return;
			                }
			                SelectSomeCust(obj);
			            }
			            else
			            {
			         
			                var json= eval("("+data+")");
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
        			            
                                document.getElementById(obj.id.replace("txtCustAddr","txtEqu")).value = record[i].equname;   //资产名称
                                document.getElementById(obj.id.replace("txtCustAddr","hidEquName")).value = record[i].equname;   //资产名称
                                document.getElementById(obj.id.replace("txtCustAddr","hidEqu") ).value = record[i].equid;  //资产IDontact")).id);                                                                        
                                document.getElementById(obj.id.replace("txtCustAddr","hidListName")).value = record[i].listname;   //资产目录
                                document.getElementById(obj.id.replace("txtCustAddr","hidListID") ).value = record[i].listid;  //资产目录ID                                                                                     
                            }
			            
			         }
	                    
	                }
	             
	             });	            	           
        }
       
        function SelectSomeEqu(obj)   //选择多个设备
			{
		
		        var newDateObj = new Date()
	            var sparamvalue =  newDateObj.getYear().toString() + newDateObj.getMonth().toString();
	            var name = obj.value.trim();
	            
	            if(name.trim()=="")
	            {
	                return;
	            }
	             var EquipmentCatalogID = document.all.<%=hidListID.ClientID%>.value.trim();
	            var CustName ="";
	            var url="../mydestop/frmQuickLocateEqu.aspx?IsSelect=true&randomid="+GetRandom()+"&Name=" + escape(name)+"&EquCust="+escape(CustName)+"&EquipmentCatalogID="+EquipmentCatalogID+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrom=DemandBase";
  
	            window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=600");	            
			}
			
        function GetEquID(obj)
        {
            var EquipmentCatalogID = document.all.<%=hidListID.ClientID%>.value.trim();
            if(obj.value.trim()=="")
	            {
	                document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value = "0"; 
	                return;
	            }
	        if(obj.value.trim()==document.getElementById(obj.id.replace("txtEqu","hidEquName")).value.trim() && (document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value.trim()!="" || document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value.trim()!="0"))
	            {
	                return;
	            }
	        var CustName = "";
	            $.ajax({         
	                    url: "../MyDestop/frmXmlHttpAjax.aspx?Equ=" + escape(obj.value) +  "&randomid="+GetRandom()+"&EquCust=" + escape(CustName)+"&EquipmentCatalogID="+EquipmentCatalogID,         
	                    datatype: "json",         
	                    type: 'GET',         
	                    success: function (data) {   //成功后回调  
	                                  if(data =="-1")  //没有
										{
										    alert("此资产不存在，请重新查找！"); 
										    document.getElementById(obj.id.replace("txtEqu","txtEqu")).value="";
										    document.getElementById(obj.id.replace("txtEqu","hidEquName")).value="";
										    document.getElementById(obj.id.replace("txtEqu","hidEqu")).value=0;
										    
										    document.getElementById(obj.id.replace("txtEqu","hidListName")).value="";
										    document.getElementById(obj.id.replace("txtEqu","hidListID")).value=0;
									    }
									    else if(xmlhttp.responseText=="0") //找到多个
									    {
									        SelectSomeEqu(obj);
									    }
									    else  //找到唯一
									    {
	      	                                var json= eval("("+data+")");
                                            var record=json.record;
                                            
                                            for(var i=0; i < record.length; i++)
	                                        {
									        
									            document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = record[i].name;   //设备名称
                                                document.getElementById(obj.id.replace("txtEqu","hidEquName")).value = record[i].name;   //设备名称
                                                document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value = record[i].id;  //设备IDontact")).id);			                                                                                                    
                                                document.getElementById(obj.id.replace("txtEqu","hidListName")).value = record[i].listname;   //资产目录名称
                                                document.getElementById(obj.id.replace("txtEqu","hidListID") ).value = record[i].listid;  //资产目录ID
                                            }
									    }  
	                             },         
	                    error: function(e){    //失败后回调                 
	                           },         
	                    beforeSend: function(){  //发送请求前调用，可以放一些"正在加载"之类额话                    
	                            } 
	                   });	                  
        }
			
			//设备
			function SelectEqu(obj) 
			{
			
			    var EquipmentCatalogID = document.all.<%=hidListID.ClientID%>.value.trim();
                    			        
			    var EquName = document.all.<%=txtEqu.ClientID%>.value.trim();
			    var CustName = document.all.<%=txtCustAddr.ClientID%>.value.trim();
			    var MastCust = document.all.<%=hidMastCust.ClientID%>.value.trim();
			    var url="../EquipmentManager/frmEqu_DeskMain.aspx?IsSelect='1'&randomid="+GetRandom()+"&FlowID="+ '<%=FlowID%>' + "&EquName=" + escape(EquName) + "&Cust=" + escape(CustName) + "&MastCust=" + escape(MastCust)+"&EquipmentCatalogID="+EquipmentCatalogID
			            + "&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=DemandBase" ;

			    open(url, 'E8OpenWin', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=50');               
			}
			
				
    //显示隐藏表格
    function ShowTable(imgCtrl)
        {

              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","Table");              
              var tableCtrl;              
              tableCtrl = document.getElementById(TableID);
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
        
        // 常用类别扩展项
        function GetShema()
        {
            document.getElementById("<%=btnShema.ClientID %>").click();
        }
            
    
    </script>


    
    <input id="hidAppID" type="hidden" runat="server" value="0" />
    <input id="hidFlowID" type="hidden" runat="server" value="0" />
    <input id="hidEqu" type="hidden" runat="server" value="-1" />
    <input id="hidEquName" type="hidden" runat="server" />
    <input id="hidCustID" runat="server" type="hidden" value="" />
    <input id="hidCust" runat="server" type="hidden" />
    <input id="hidAddr" runat="server" type="hidden" />
    <input id="hidContact" runat="server" type="hidden" />
    <input id="hidTel" runat="server" type="hidden" />
    <input id="hidCustDeptName" type="hidden" runat="server" />
    <input id="hidCustEmail" type="hidden" runat="server" />
    <input id="hidMastCust" type="hidden" runat="server" />
    <input id="hidjob" type="hidden" runat="server" />
    <input id="hidListName" value="" type="hidden" runat="server" />
    <input id="hidListID" value="0" type="hidden" runat="server" />
    <input id="hidReqTempID" runat="server" type="hidden" value="0" />    
    
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />

                                    
    <asp:Button ID="btnShema" runat="server" onclick="btnShema_Click" Text="获取扩展信息" style="display:none;" />
    
    <!--Begin: 客户/资产信息的表头-->
    
    <table id="Table12" width="100%" runat="server" class="listNewContent" border="0"
        cellspacing="2" cellpadding="0">
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
            <td id="AddCustTD" valign="middle" align="right" class="listTitleNew" style="width: 120">
                <asp:Button ID="bthCreateCus" CssClass="btnClass3" runat="server" Text="添加客户" OnClientClick="getCustomId(this);"
                    UseSubmitBehabior="False" UseSubmitBehavior="False" />
                <asp:Button ID="bthAddEqu" runat="server" Text="添加资产" OnClientClick="AddEqu(this);"
                    UseSubmitBehavior="False" />
            </td>
        </tr>
    </table>
    
    <!--End: 客户/资产信息的表头-->
    
<!--Begin: 客户信息-->    
    <table class="listContent" width="100%" align="center" id="Table2"
        border="0" cellspacing="1" cellpadding="1">
        <tr>
            <td style="width: 12%" class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="LitCustName" runat="server" Text="客户名称"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:Label ID="labCustAddr" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtCustAddr" runat="server" MaxLength="200" onblur="GetCustID(this)"></asp:TextBox>
                <input id="cmdCust" onclick="CustomSelect(this)" type="button" value="..." runat="server"
                    class="btnClass2" />
                <asp:Label ID="rWarning" Style="margin-left: 7px;" runat="server" Font-Bold="False"
                    Font-Size="Small" ForeColor="Red">*</asp:Label>
                &nbsp;
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitCustAddress" runat="server" Text="客户地址"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblAddr" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtAddr" runat="server" MaxLength="200"></asp:TextBox>&nbsp;
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitContact" runat="server" Text="客户联系人"></asp:Literal>
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
                <asp:TextBox ID="txtCTel" runat="server" AutoPostBack="false" MaxLength="20"></asp:TextBox>&nbsp;
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="LitCustDeptName" runat="server" Text="客户所属部门"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblCustDeptName" runat="server"></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="litCustEmail" runat="server" Text="客户邮箱地址"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblEmail" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitMastShortName" runat="server" Text="客户所属单位"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lblMastCust" runat="server"></asp:Label>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitJob" runat="server" Text="客户职位"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="lbljob" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="litEquDeskName" runat="server" Text="资产名称"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:Label ID="lblEqu" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtEqu" runat="server" MaxLength="80" onblur="javascript:clearEquEngineer(this);"></asp:TextBox>
                <input id="cmdEqu" onclick="SelectEqu(this)" type="button" value="..." runat="server"
                    name="cmdEqu" class="btnClass2" />
                &nbsp;
            </td>
        </tr>
    </table>
<!--End: 客户信息-->


<!--Begin: 资产信息-->
    <table id="Table15" width="100%" align="center" runat="server" class="listNewContent"
        style="display: none;">
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
<!--End: 资产信息-->

    <br />
  
  <!--Begin: 基础资料表头-->
    <table height="29" border="0" cellpadding="0" cellspacing="0">
        <tr style="cursor: hand">
            <td width="7" valign="top">
                <img src="../Images/lm-left.gif" width="7" height="29" />
            </td>
            <td width="95" height="29" align="center" valign="middle" id="name0" class="td_3"
                onclick="" background="../Images/lm-b.gif" onmouseover="chang(this,'lm-c.gif')"
                onmouseout="chang(this,'lm-a.gif')">
                <span id="a0" class="td_3">基本信息</span>
            </td>
            <td width="7" valign="top">
                <img src="../Images/lm-right.gif" width="7" height="29" />
            </td>
        </tr>
    </table>
  <!--End: 基础资料表头-->
    
    <!--Begin: 基础资料-->
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <table id="Tables0" width="100%" align="center" class="listContent" border="0" cellspacing="1"
                    cellpadding="1">
                    <tr id="ShowServiceNo" runat="server">
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="LitReqDemandRegUserName" runat="server" Text="登单人"></asp:Literal>
                        </td>
                        <td class="list" style="width: 35%">
                            <uc8:UserPicker ID="RegUser" runat="server" ContralState="eReadOnly" TextToolTip="登单人" />
                        </td>
                        <td class="listTitleRight" style="width: 12%">
                            <asp:Literal ID="LitReqDemandRegTime" runat="server" Text="登记时间"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc1:ctrdateandtime ID="CtrDTRegTime" runat="server" ContralState="eReadOnly" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitReqDemandNo" runat="server" Text="需求单号"></asp:Literal>
                        </td>
                        <td class="list">
                            <asp:Label ID="lblBuildCode" runat="server"></asp:Label><asp:Label ID="labServiceNo"
                                runat="server"></asp:Label>
                        </td>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitReqDemandType" runat="server" Text="需求类别"></asp:Literal>
                        </td>
                        <td class="list">
                            <uc11:ctrFlowCataDropListNew ID="ctrFCDServiceType" runat="server" RootID="1003"
                                MustInput="true" TextToolTip="需求类别" OnChangeScript="GetShema();"/>
                            <input id="hidServiceTypeID" runat="server" type="hidden" />
                        </td>
                    </tr>                                                   
                    
                    <tr>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitReqDemandSubject" runat="server" Text="需求主题"></asp:Literal>
                        </td>
                        <td class="list" colspan="3">
                            <uc4:CtrFlowFormText ID="CtrFlowFTSubject" Width="95%" runat="server" TextToolTip="需求主题"
                                MaxLength="100" MustInput="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitReqDemandContent" runat="server" Text="详细描述"></asp:Literal>
                        </td>
                        <td colspan="3" class="list">
                            <uc12:CtrFlowRemark ID="txtContent" runat="server" MustInput="true" MaxLength="500"
                                TextToolTip="详细描述" />
                        </td>
                    </tr>

                    <tr>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitReqDemandStatus" runat="server" Text="需求状态"></asp:Literal>
                        </td>
                        <td class="list" colspan="3">
                            <uc6:ctrFlowCataDropListNew ID="CtrDealState" runat="server" RootID="1008" TextToolTip="需求状态"
                                MustInput="true" />
                            <asp:Label ID="lblDealStatus" Text="" runat="server"></asp:Label>
                            <input id="hidDealStatusID" type="hidden" runat="server" value="0" />
                            <input id="hidDealStatus" type="hidden" runat="server" value="" />
                        </td>
                    </tr>
                    
                    
                    <tr>
                        <td class="listTitleRight">
                            <asp:Literal ID="LitProgressBar" runat="server" Text="当前进度"></asp:Literal>
                        </td>
                        <td class="list" colspan="3">
                            <img id="ShowImg" runat="server" />
                        </td>
                    </tr>
                           <!--Begin: 常用类别配置项-->
                    <tr id="trshema" runat="server">
                        <td class="list" colspan="4">
                            <uc14:Catalog_DymSchameCtrList ID="Catalog_DymSchameCtrList1" runat="server" />
                        </td>
                    </tr>
                    <!--End: 常用类别配置项-->
                </table>
                
            </td>
        </tr>
    </table>
    <!--End: 基础资料-->

    <script type="text/javascript" language="javascript">        
    function chang(obj,img){
    
//            if(obj!=selectobj)	    
//		        obj.background="../Images/"+img;
    }
    </script>
</asp:Content>
