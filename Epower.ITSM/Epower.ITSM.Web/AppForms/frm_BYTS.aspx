<%@ Page Language="C#" MasterPageFile="~/FlowForms.Master" ValidateRequest="false" AutoEventWireup="True" CodeBehind="frm_BYTS.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frm_BYTS" Title="无标题页" %>

<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc6" %>

<%@ Register Src="../Controls/DeptPicker.ascx" TagName="DeptPicker" TagPrefix="uc4" %>

<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc3" %>
<%@ Register Src="../Controls/CtrFeedBack.ascx" TagName="CtrFeedBack" TagPrefix="uc5" %>
<%@ Register Src="../Controls/CtrMonitor.ascx" TagName="CtrMonitor" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="CtrDateAndTime" Src="../Controls/CtrDateAndTimeV2.ascx" %>
<%@ MasterType VirtualPath="~/FlowForms.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<link href="../WindowOpenDeam/showdiv.css" rel="stylesheet" type="text/css"  />
<script language="javascript" src="../WindowOpenDeam/showdiv.js" type="text/javascript"></script>

<script type="text/javascript" language="javascript">
<!--
            function TransferValue()
            {
                if (typeof(document.all.<%=txtBY_PersonName.ClientID%>)!="undefined" )
                { 
	                parent.header.flowInfo.Subject.value = document.all.<%= txtBY_PersonName.ClientID%>.value + "[投诉单]";
	            }
            }

             function DoUserValidate(lngActionID,strActionName)
	        {
	            TransferValue();
			    return CheckCustAndType();
		    }
		    
		
            //检查是否选择了事件来源和事件类型
			function CheckCustAndType()
			{
			    if (typeof(document.all.<%=txtBY_PersonName.ClientID%>)!="undefined" )
			    {
		            if (document.all.<%=txtBY_PersonName.ClientID%>.value.trim()=="")         //投诉人
			        {
			            document.all.<%=txtBY_PersonName.ClientID%>.focus();
				        alert("投诉人不能为空！");
				        return false;
			        }
			    }
			    return true;
			}
			
			String.prototype.trim = function()  //去空格
			{
				// 用正则表达式将前后空格
				// 用空字符串替代。
				return this.replace(/(^\s*)|(\s*$)/g, "");
			}
			//检查是否为数值
			function CheckIsnum(obj,strDisplay)
            {
                var svalue = obj.value;
                if (isNaN(svalue))
                {
                    alert(strDisplay);
                    obj.focus(); 
                    obj.select(obj.value.length);
                }
            }
            
            //客户选择
			function CustomSelect(obj) 
			{
			   var CustName = document.all.<%=txtCustAddr.ClientID%>.value;
				var	value=window.showModalDialog("../Common/frmDRMUserSelect.aspx?IsSelect=true&CustID" + document.getElementById(obj.id.replace('cmdCust','hidCustID')).value + "&FlowID="+ '<%=FlowID%>' + "&CustName=" + escape(CustName),"","dialogHeight:600px;dialogWidth:800px");
				if(value != null)
				{
					if(value.length>1)
					{
			            document.getElementById(obj.id.replace("cmdCust","txtCustAddr")).value = value[1];   //瀹㈡埛
			            document.getElementById(obj.id.replace("cmdCust","hidCust") ).value = value[1];
			            document.getElementById(obj.id.replace("cmdCust","txtBY_ContactAddress")).value = value[7];   //鍦板潃
			            document.getElementById(obj.id.replace("cmdCust","hidaddress")).value = value[7];   //鍦板潃
			            document.getElementById(obj.id.replace("cmdCust","txtBY_PersonName")).value = value[2];    //鑱旂郴浜?
			            document.getElementById(obj.id.replace("cmdCust","hidContact")).value = value[2];           
			            document.getElementById(obj.id.replace("cmdCust","txtBY_ContactPhone")).value = value[3];   //鑱旂郴浜虹數璇?
			            document.getElementById(obj.id.replace("cmdCust","hidTel")).value = value[3];
			            document.getElementById(obj.id.replace("cmdCust","hidCustID")).value = value[4];    //瀹㈡埛ID鍙?			            
			            document.getElementById(obj.id.replace("cmdCust","txtBY_Email")).value = value[13];  //鐢靛瓙閭欢
		                document.getElementById(obj.id.replace("cmdCust","hidBY_Email")).value = value[13];  //鐢靛瓙閭欢

					}
				}
				else
				{
				    document.getElementById(obj.id.replace("cmdCust","txtCustAddr")).value = "";   //瀹㈡埛
		            document.getElementById(obj.id.replace("cmdCust","hidCust") ).value = "";
		            document.getElementById(obj.id.replace("cmdCust","txtBY_ContactAddress")).value = "";   //鍦板潃
		            document.getElementById(obj.id.replace("cmdCust","hidaddress")).value = "";   //鍦板潃
		            document.getElementById(obj.id.replace("cmdCust","txtBY_PersonName")).value = "";    //鑱旂郴浜?
		            document.getElementById(obj.id.replace("cmdCust","hidContact")).value = "";           
		            document.getElementById(obj.id.replace("cmdCust","txtBY_ContactPhone")).value = "";   //鑱旂郴浜虹數璇?
		            document.getElementById(obj.id.replace("cmdCust","hidTel")).value = "";
		            document.getElementById(obj.id.replace("cmdCust","hidCustID")).value = 0;    //瀹㈡埛ID鍙?		            
		            document.getElementById(obj.id.replace("cmdCust","txtBY_Email")).value = "";  //鐢靛瓙閭欢
		            document.getElementById(obj.id.replace("cmdCust","hidBY_Email")).value = "";  //鐢靛瓙閭欢


				}
			}
			
			function getvalue(obj)
			{
			    var	value=obj;
				if(value != null)
				{
					if(value.length>1)
					{
					    
			           document.all.<%=txtCustAddr.ClientID%>.value = value[1];   //客户
			            document.all.<%=hidCust.ClientID%>.value = value[1];
			            document.all.<%=txtBY_ContactAddress.ClientID%>.value=value[7];//地址			            
			            document.all.<%=hidaddress.ClientID%>.value=value[7];   //地址			            
			            document.all.<%=txtBY_PersonName.ClientID%>.value=value[2];        //联系人			            document.getElementById(obj.id.replace("cmdCust","hidContact")).value = value[2];           
			            document.all.<%=txtBY_ContactPhone.ClientID%>.value = value[3];   //联系人电话			            document.getElementById(obj.id.replace("cmdCust","hidTel")).value = value[3];
			            document.all.<%=hidCustID.ClientID%>.value = value[4];    //客户ID号			            
			            document.all.<%=txtBY_Email.ClientID%>.value = value[13];  //电子邮件
		                document.all.<%=hidBY_Email.ClientID%>.value = value[13];  //电子邮件
					}
				}
				else
				{
				    
			           document.all.<%=txtCustAddr.ClientID%>.value = "";   //客户
			            document.all.<%=hidCust.ClientID%>.value = "";
			            document.all.<%=txtBY_ContactAddress.ClientID%>.value="";//地址			            
			            document.all.<%=hidaddress.ClientID%>.value="";   //地址			            
			            document.all.<%=txtBY_PersonName.ClientID%>.value="";        //联系人			            document.getElementById(obj.id.replace("cmdCust","hidContact")).value = value[2];           
			            document.all.<%=txtBY_ContactPhone.ClientID%>.value = "";   //联系人电话			            document.getElementById(obj.id.replace("cmdCust","hidTel")).value = value[3];
			            document.all.<%=hidCustID.ClientID%>.value = "";    //客户ID号			            
			            document.all.<%=txtBY_Email.ClientID%>.value = "";  //电子邮件
		                document.all.<%=hidBY_Email.ClientID%>.value = "";  //电子邮件
				}
				return "";
			}
			
			 //打开客户对应的事件记录
	        function OpenServiceHistory(type)
	        {
	            var newDateObj = new Date();
	            var sparamvalue =  newDateObj.getYear().toString() + newDateObj.getMonth().toString();
	            window.open("frmBytsList.aspx?NewWin=true&ID=" + document.all.<%=hidCustID.ClientID%>.value + "&FlowID="+ '<%=FlowID%>' + "&sparamvalue=" + sparamvalue,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=500");
	            event.returnValue = false;
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
				var	value=window.showModalDialog("../mydestop/frmQuickLocateCust.aspx?Name=" + escape(name),"","dialogHeight:500px;dialogWidth:500px");
				if(value != null)
				{
					if(value.length>1)
					{
				        document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr")).value = value[1];   //客户
			            document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = value[1];
			            document.getElementById(obj.id.replace("txtCustAddr","txtBY_ContactAddress")).value = value[5];   //地址
			            document.getElementById(obj.id.replace("txtCustAddr","hidaddress") ).value = value[5];   //地址
			            document.getElementById(obj.id.replace("txtCustAddr","txtBY_PersonName")).value = value[2];    //联系人
			            document.getElementById(obj.id.replace("txtCustAddr","hidContact")).value = value[2];   
			            document.getElementById(obj.id.replace("txtCustAddr","txtBY_ContactPhone")).value = value[3];   //联系人电话
			            document.getElementById(obj.id.replace("txtCustAddr","hidTel")).value = value[3];
			            document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = value[4];    //客户ID号			            
			            document.getElementById(obj.id.replace("txtCustAddr","txtBY_Email")).value = value[12];  //电子邮件
		                document.getElementById(obj.id.replace("txtCustAddr","hidBY_Email")).value = value[12];  //电子邮件

					}
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
					xmlhttp.open("GET", "../MyDestop/frmXmlHttp.aspx?Cust=" + escape(obj.value), true); 
                    xmlhttp.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
					xmlhttp.onreadystatechange = function() 
												{ 
													if ( xmlhttp.readyState==4 ) 
													{ 
														if(xmlhttp.responseText=="-1")  //没有
														{
														    document.getElementById(obj.id.replace("txtCustAddr","hidCustID") ).value.trim()!="0";
														    alert("此客户不存在！"); 
													    }
													    else if(xmlhttp.responseText=="0") //找到多个
													    {
													        SelectSomeCust(obj);
													    }
													    else  //找到唯一
													    {
													        
													        var sreturn=xmlhttp.responseText;
													        arr=sreturn.split(",");	
													        document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr")).value = arr[1];   //名称	 
													        document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = arr[1];   //名称                                               
			                                                document.getElementById(obj.id.replace("txtCustAddr","txtBY_ContactAddress")).value = arr[2];   //地址
			                                                document.getElementById(obj.id.replace("txtCustAddr","hidaddress")).value = arr[2];   //地址
			                                                document.getElementById(obj.id.replace("txtCustAddr","txtBY_PersonName")).value = arr[3];    //联系人
			                                                document.getElementById(obj.id.replace("txtCustAddr","hidContact")).value = arr[3];   
			                                                document.getElementById(obj.id.replace("txtCustAddr","txtBY_ContactPhone")).value = arr[4];   //联系人电话
			                                                document.getElementById(obj.id.replace("txtCustAddr","hidTel")).value = arr[4];
			                                                document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = arr[0];    //客户ID
			                                                
			                                                document.getElementById(obj.id.replace("txtCustAddr","txtBY_Email")).value = arr[6];  //电子邮件
		                                                    document.getElementById(obj.id.replace("txtCustAddr","hidBY_Email")).value = arr[6];  //电子邮件
													    }
													} 
												} 
					xmlhttp.send(null); 
				}catch(e3){}
            }
        }
        
        function ShowTable(imgCtrl)
        {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","Table");
              var className;
              var objectFullName;
              var tableCtrl;
              objectFullName = <%=Table11.ClientID%>.id;
              className = objectFullName.substring(0,objectFullName.indexOf("Table11")-1);
              tableCtrl =document.getElementById(className.substr(0,className.length)+"_"+TableID);
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
        
        function SelectService()
        {
            var	value=window.showModalDialog('CST_Issue_List.aspx?IsSelect=true',"","dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no") ;
		    if(typeof(value) != "undefined" && value[0]!="")
		    {
		        document.all.<%=hidCustArrID.ClientID%>.value= value[0];
		        document.all.<%=hidRefEvent.ClientID%>.value = "true";
		    }
        }        
        
        //打印
        function printdiv()
        {
            var flowid="<%=FlowID%>";
            var AppID="<%=AppID%>";
            var FlowMoldelID="<%=FlowModelID%>";
            window.open("../Print/printRule.aspx?FlowId="+flowid+"&AppID="+AppID+"&FlowMoldelID="+FlowMoldelID);
            return false;
        }
        
        
        //案例分析
		function DoItemQuestionAnalysis(lngAppID,lngFlowID)
        {
	        window.open("../ProbleForms/frmPro_ProblemAnalyseMain.aspx?AppID=" + lngAppID + "&FlowID=" + lngFlowID,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=500");
        }
        //知识归档
        function DoKmAdd(lngMessageID,lngAppID,lngFlowID)
        {
             window.open("../Forms/form_all_flowmodel.aspx?NewWin=true&appid=400&ep=" + lngMessageID,"","scrollbars=no,status=yes ,resizable=yes,width=680,height=500");
        }
        //知识参考
        function FormDoKmRef()
        {
           window.open("../InformationManager/frmInf_InformationMain.aspx?IsSelect=1","","scrollbars=no,status=yes ,resizable=yes,width=800,height=600");
        }
  //-->
</script>
<input id="OpenReturnCustname" type="hidden" value="" />
<table id="Table11"  width="100%" align="center" runat="server"  class="listNewContent">
<tr>
    <td vAlign="top" align="left" class="listTitleNew">
          <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif" width="16"/>客户信息
    </td>
</tr>
</table>
<table class="listContent" width="100%" align="center" runat="server" id="Table1">
<TR>
	    <td class="listTitle" nowrap="nowrap"  runat="server" id="tdcustTitle" style="width:99px;">
            <asp:Literal ID="LitCustName" runat="server" Text="客户名称"></asp:Literal></td>
        <td class="list" nowrap="nowrap" runat="server" id="tdcust" style="width: *">
           <asp:label id="labCustAddr" runat="server" Visible="False"></asp:label>
           <asp:textbox id="txtCustAddr" runat="server" Width="180px" MaxLength="50" onblur="GetCustID(this)" CssClass="WPAspFieldTxt"></asp:textbox>
           <input id="cmdCust" onclick="CustomSelect(this)" type="button" value="..." runat="server" class="btnClass" />
           <asp:linkbutton id="lnkServiceHistory" runat="server" ForeColor="#0000C0" OnClientClick="OpenServiceHistory();">(历史参照)</asp:linkbutton>
           <input id="hidCust" runat="server" type="hidden" />
           <input id="hidCustID" runat="server" type="hidden" value="-1" />
        </td>
		<TD style="width: 99px;" noWrap class="listTitle">投诉人</TD>
		<TD class="list" style="width:*"><asp:TextBox ID="txtBY_PersonName" runat="server" MaxLength="50"></asp:TextBox><asp:label id="labBY_PersonName" runat="server" Visible="false"></asp:label>
		<input id="hidContact" runat="server" type="hidden" />
		<asp:Label ID="rBy_PersonName" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Red">*</asp:Label>
		</TD>
	</TR>
	<TR>
		<TD class="listTitle">联系手机</TD>
		<TD class="list"><asp:TextBox ID="txtBY_Mobile" runat="server" MaxLength="20"></asp:TextBox><asp:label id="labBY_Mobile" runat="server" Visible="False"></asp:label></TD>
		<TD class="listTitle">
            <asp:Literal ID="LitCTel" runat="server" Text="联系人电话"></asp:Literal></TD>
		<TD class="list"><asp:TextBox ID="txtBY_ContactPhone" runat="server" MaxLength="20"></asp:TextBox><asp:label id="labBY_ContactPhone" runat="server" Visible="False"></asp:label>
		<input id="hidTel" runat="server" type="hidden" />
		</TD>
	</TR>
	<tr>
	    <TD class="listTitle">
            <asp:Literal ID="litCustEmail" runat="server" Text="电子邮件"></asp:Literal></TD>
		<TD class="list"><asp:TextBox ID="txtBY_Email" runat="server" MaxLength="50"></asp:TextBox><asp:label id="labBY_Email" runat="server" Visible="False"></asp:label>
            <input id="hidBY_Email" runat="server" type="hidden" /></TD>
		<TD class="listTitle">
            <asp:Literal ID="LitCustAddress" runat="server" Text="地址"></asp:Literal></TD>
		<TD class="list" ><asp:TextBox ID="txtBY_ContactAddress" runat="server" MaxLength="100"></asp:TextBox>
		<asp:label id="labBY_ContactAddress" runat="server" Visible="False"></asp:label>
		<input id="hidaddress" runat="server" type="hidden" />
		</TD>
	</tr>
</table>
<table id="Table12"  width="100%" align="center" runat="server" class="listNewContent" >
<tr>
    <td vAlign="top" align="left" class="listTitleNew">
          <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif" width="16"/>基本信息
    </td>
</tr>
</table>
<table class="listContent" width="100%" align="center" runat="server" id="Table2">
<TR>
		<TD class="listTitle" style="width: 99px">投诉来源</TD>
		<TD class="list" style="width:*">
            <uc3:ctrFlowCataDropList ID="CataSource" runat="server"  RootID="1009"/>
            </TD>
		<TD class="listTitle" style="width: 99px">投诉类型</TD>
		<TD class="list" style="width:*">
            <uc3:ctrFlowCataDropList ID="CataType" runat="server" RootID="1010" />
        </TD>
	</TR>
	<TR>
        <TD noWrap width="99" class="listTitle">被投诉人</TD>
		<TD class="list" colspan="3">
            <uc6:UserPicker ID="UserPicker1" runat="server" />
		</TD>
	</TR>
	<tr>
	    <TD class="listTitle" >接收时间</TD>
		<TD class="list"><uc1:CtrDateAndTime id="Ctr_ReceiveTime" runat="server"></uc1:CtrDateAndTime></TD>
		 <TD class="listTitle">投诉次数</TD>
		<TD class="list"><asp:TextBox ID="txtBY_InformNum" runat="server" MaxLength="9" style="ime-mode:Disabled" onblur="CheckIsnum(this,'投诉次数必须为数值！');" onkeydown="NumberInput('');" Text="1"></asp:TextBox><asp:label id="labBY_InformNum" runat="server" Visible="False"></asp:label></TD>
	</tr>
	<TR>
		<TD class="listTitle" >投诉内容</TD>
		<TD class="list" colspan="3" style="word-break:break-all">
		<asp:TextBox ID="txtBY_Content" runat="server" Rows="3" Width="95%" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
		<asp:label id="labBY_Content" runat="server" Visible="False"></asp:label>
		</TD>
	</TR>
</table>
<input id="hidCustArrID" runat="server" type="hidden"/>
<input id="hidCustArrIDold" runat="server" type="hidden"/>
<table id="Table13"  width="100%" align="center" runat="server" class="listNewContent">
<tr>
    <td vAlign="top" align="left"  class="listTitleNew">
          <img class="icon" id="Img3" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif" width="16"/>处理信息
    </td>
</tr>
</table>
<table class="listContent" width="100%" align="center" runat="server" id="Table3">
<tr>
    <TD class="listTitle" style="width:99px">投诉性质</TD>
		<TD class="list" >
            <uc3:ctrFlowCataDropList ID="CataKind" runat="server" RootID="1011" />
        </TD>
</tr>
<TR>
		<TD class="listTitle">措施及结果</TD>
		<TD class="list">
		<ftb:FreeTextBox ID="freeTextBox1" runat="server" ButtonPath="../Forms/images/epower/officexp/" ImageGalleryPath="Attfiles\\Photos" Width="100%" Height="200px"></ftb:FreeTextBox>
		<asp:label id="lblDealContent" runat="server" Visible="False"></asp:label>
		</TD>
	</TR>
    <tr id="trRefEvent" runat="server">
	    <TD class="listTitle">相关事件</TD>
	    <td  class="list">
			        <asp:datagrid id="gridUndoMsg" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="gridUndoMsg_ItemCommand" OnItemDataBound="gridUndoMsg_ItemDataBound">
			            <FooterStyle CssClass="listTitle" />
				        <Columns>
					        <asp:BoundColumn DataField="subject" HeaderText="标题">
		                        <HeaderStyle Width="20%"></HeaderStyle>
	                        </asp:BoundColumn>
	                        <asp:BoundColumn DataField="RegUserName" HeaderText="受理人">
		                        <HeaderStyle Width="5%"></HeaderStyle>
	                        </asp:BoundColumn>
	                        <asp:BoundColumn DataField="CustTime" HeaderText="发生时间" DataFormatString="{0:g}">
		                        <HeaderStyle Width="7%"></HeaderStyle>
                                <ItemStyle Wrap="False" />
	                        </asp:BoundColumn>
	                        <asp:BoundColumn DataField="custName" HeaderText="客户" Visible="false">
		                        <HeaderStyle Width="10%"></HeaderStyle>
	                        </asp:BoundColumn>
	                        <asp:BoundColumn DataField="contact" HeaderText="联系人">
		                        <HeaderStyle Width="5%"></HeaderStyle>
	                        </asp:BoundColumn>
                            <asp:BoundColumn DataField="dealstatus" HeaderText="事件状态" HeaderStyle-Wrap="false">
                                <HeaderStyle Width="5%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="status" HeaderText="流程状态" HeaderStyle-Wrap="false">
					            <HeaderStyle Width="5%"></HeaderStyle>
				            </asp:BoundColumn>
	                        <asp:TemplateColumn HeaderText="">
		                        <HeaderStyle Width="5%"></HeaderStyle>
		                        <ItemTemplate>
			                        <input id="CmdDeal"  onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>' type="button" value='详  情' runat="server" causesvalidation="false" class="btnClass">
		                        </ItemTemplate>
	                        </asp:TemplateColumn>
	                        <asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
	                        <asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
	                        <asp:BoundColumn Visible="False" DataField="dealstatus"></asp:BoundColumn>
	                        <asp:BoundColumn Visible="False" DataField="status"></asp:BoundColumn>
					        <asp:TemplateColumn>
						        <HeaderStyle Width="5%" VerticalAlign='Top'></HeaderStyle>
						        <HeaderTemplate>
						            <asp:Button ID="btnAdd" runat="server" Text="新增" OnClientClick="SelectService();" OnClick="btnAdd_Click"  CssClass="btnClass" />
						        </HeaderTemplate>
						        <ItemTemplate>
							        <asp:button id="btndelete" CommandName="Delete" runat="server" Text="删  除" CausesValidation="False" CssClass="btnClass"></asp:button>
						        </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
					        </asp:TemplateColumn>
				        </Columns>
				        </asp:datagrid>
	    </td>
	</tr>
</table>
<table class="listContent"  width="100%" align="center" >
	<TR id="trShowMonitor" width="100%" runat="server">
	  <td  noWrap align="left" class="listTitle" style="width:99px">督办内容</td>
      <td  width="100%" class="list" style="word-break:break-all">
          <uc2:CtrMonitor ID="CtrMonitor1" runat="server" />
       </td>
    </TR>
    <TR id="ShowFeedBack" width="100%" runat="server">
      <td  width="100%" class="list">
        <uc5:CtrFeedBack ID="CtrFeedBack1" runat="server" />
       </td>
		
    </TR>
</table>
<input id="hidRefEvent" runat="server"  value="false" type="hidden"/>
<input id="hidFormFrom" runat="server" type="hidden"/>
</asp:Content>
