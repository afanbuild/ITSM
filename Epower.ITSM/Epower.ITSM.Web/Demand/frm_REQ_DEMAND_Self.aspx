<%@ Page Language="C#" MasterPageFile="~/FlowForms.Master" AutoEventWireup="true"
    CodeBehind="frm_REQ_DEMAND_Self.aspx.cs" Inherits="Epower.ITSM.Web.Demand.frm_REQ_DEMAND_Self"
    Title="需求自助登单页面" %>

<%@ Register Src="../Controls/CtrFlowRemark.ascx" TagName="CtrFlowRemark" TagPrefix="uc12" %>
<%@ Register Src="~/Controls/Catalog_DymSchameCtrList.ascx" TagName="Catalog_DymSchameCtrList"
    TagPrefix="uc14" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
    //拼凑待办事项标题
    function TransferValue()
    {
      //拼凑待办事项标题
       if (typeof(document.all.<%=hidDemandSubject.ClientID%>)!="undefined" )
       {
          if(document.all.<%=hidDemandSubject.ClientID%>.value.trim()!="")
          {
	            parent.header.flowInfo.Subject.value = document.all.<%=hidDemandSubject.ClientID%>.value.trim();
	      }
       }
    }
     //脚本检查是否输入
	function DoUserValidate(lngActionID,strActionName)
    {
        TransferValue();
	    return CheckCustAndType();
    }
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
    <input id="hidDemandSubject" type="hidden" runat="server" />
    <input id="hidDemandTypeID" runat="server" type="hidden" value="0" />
    <input id="hidDemandType" runat="server" type="hidden" />
    <input id="hidDemandNo" runat="server" type="hidden" />
    <input id="hidDemandStatusID" runat="server" type="hidden" value="0" />
    <input id="hidDemandStatus" runat="server" type="hidden" />
    <input id="hidReqTempID" runat="server" type="hidden" value="0" />
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
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
            <td id="AddCustTD" valign="middle" align="right" class="listTitleNew" style="width: 120"
                runat="server">
                <asp:Button ID="bthCreateCus" CssClass="btnClass3" runat="server" Text="添加客户" OnClientClick="getCustomId(this);"
                    UseSubmitBehabior="False" UseSubmitBehavior="False" />
                <asp:Button ID="bthAddEqu" runat="server" Text="添加资产" OnClientClick="AddEqu(this);"
                    UseSubmitBehavior="False" />
            </td>
        </tr>
    </table>
    <table class="listContent" width="100%" align="center" id="Table2" border="0" cellspacing="1"
        cellpadding="1">
        <tr>
            <td style="width: 12%" class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="LitkhCustName" runat="server" Text="客户名称"></asp:Literal>
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
                <asp:Literal ID="LitkhContact" runat="server" Text="客户联系人"></asp:Literal>
            </td>
            <td class="list">
                <asp:Label ID="labContact" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtContact" runat="server" MaxLength="20"></asp:TextBox>&nbsp;
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitkhCTel" runat="server" Text="联系人电话"></asp:Literal>
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
                <asp:Literal ID="LitEquipmentName" runat="server" Text="资产名称"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:Label ID="lblEqu" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtEqu" runat="server" MaxLength="80" onblur="GetEquID(this);"></asp:TextBox>
                <input id="cmdEqu" onclick="SelectEqu(this)" type="button" value="..." runat="server"
                    name="cmdEqu" class="btnClass2" />
                &nbsp;
            </td>
        </tr>
    </table>
    <br />
    <table id="Tables0" width="100%" align="center" class="listContent" border="0">
        <tr>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitReqDemandContent" runat="server" Text="详细描述"></asp:Literal>
            </td>
            <td colspan="3" class="list">
                <uc12:CtrFlowRemark ID="txtContent" runat="server" MustInput="true" MaxLength="500"
                    OnBlurScript="txtKeyNameBack();" OnFocusScript="txtKeyNameClear();" TextToolTip="详细描述" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitProgressBar" runat="server" Text="当前进度"></asp:Literal>
            </td>
            <td colspan="3" class="list">
                <img id="ShowImg" runat="server" />
            </td>
        </tr>
        <!--Begin: 常用类别配置项-->
        <tr id="trshema" runat="server">
            <td class="list" colspan="4">
                <uc14:catalog_dymschamectrlist id="Catalog_DymSchameCtrList1" runat="server" />
            </td>
        </tr>
        <!--End: 常用类别配置项-->
    </table>
</asp:Content>
