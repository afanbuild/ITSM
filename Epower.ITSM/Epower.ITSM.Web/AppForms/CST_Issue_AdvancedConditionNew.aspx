<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CST_Issue_AdvancedConditionNew.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.CST_Issue_AdvancedConditionNew" %>
<%@ Register Src="../Controls/ServiceStaffMastCust.ascx" TagName="ServiceStaffMastCust"
    TagPrefix="uc4" %>
<%@ Register Src="../Controls/ServiceStaff.ascx" TagName="ServiceStaff" TagPrefix="uc3" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc2" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc5" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc11" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<link  rel="stylesheet" type="text/css" href="../Js/themes/jquery.autocomplete.css" />
   <script type="text/javascript" src="../Js/Plugin/jquery.autocomplete.js"></script>
   
 <input type="hidden" id="HidIS" runat="server" />
    <input type="hidden" id="hidDeptID" runat="server" />
    <input type="hidden" id="hidDeptName" runat="server" />
    <input type="hidden" id="hidValue" runat="server" />
    
    <input id="hidCustID" runat="server" type="hidden" value="" />
    <asp:HiddenField ID="hidClientId_ForOpenerPage" runat="server" />
    <center>
        <table style="height: 300px; background-color:White; ">
            <tr>
                <td style="height: 300px">
                    <table class="listContent" width="600px" id="Table2" style="top: 0; height: 300px">
                        <tr>
                            <td colspan="6" align="center" class="list">
                                <strong style="font-weight: bold; font-size: 20px; color: #08699E;">事件单高级查询</strong>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap align="right" class="listTitle">
                                选择收藏查询条件
                            </td>
                            <td align="left" class="list" colspan="1">
                                <asp:DropDownList ID="DropSQLwSave" runat="server" Width="152px" AutoPostBack="True"
                                    OnSelectedIndexChanged="DropSQLwSave_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td nowrap align="right" class="listTitle" >
                                显示类别                                                                    

                            </td>
                            <td align="left" class="list">
                                <asp:DropDownList ID="DropDownList1" runat="server" Width="90px" AutoPostBack="True"
                                    OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                </asp:DropDownList>                                                                                                                        
                                <input type="button" value="显示字段" id="btn_columns" class="btnClass"  />
                            </td>
                        </tr>
                        <tr>
                            <td nowrap align="right" class="listTitle" style="width: 12%">
                                <asp:Literal ID="LitCustName" runat="server" Text="客户名称"></asp:Literal>
                            </td>
                            <td nowrap align="left" class="list" style="width: 35%">
                                <asp:TextBox ID="txtCustName" runat="server" onblur="GetCustID(this)"></asp:TextBox>
                                <input id="cmdCust" onclick="CustomSelect(this)" type="button" value="..." runat="server"
                                    class="btnClass2" />
                            </td>
                            <td nowrap align="right" class="listTitle" style="width: 12%">
                                <asp:Literal ID="LitServiceNo" runat="server" Text="事件单号"></asp:Literal>
                            </td>
                            <td align="left" class="list">
                                <asp:TextBox ID="txtIssueNo" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr><td align="right" class="listTitle" nowrap="nowrap">
                                <asp:Literal ID="LitSubject" runat="server" Text="摘要"></asp:Literal>
                            </td>
                            <td class="list">
                                <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
                            </td>
                            
                            <td nowrap align="right" class="listTitle">
                                流程状态
                            </td>
                            <td align="left" class="list">
                                <asp:DropDownList ID="cboStatus" runat="server" Width="152px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap align="right" class="listTitle">
                                <asp:Literal ID="LitEffectName" runat="server" Text="影响度"></asp:Literal>
                            </td>
                            <td align="left" class="list">
                                <uc6:ctrFlowCataDropList ID="CtrFCDEffect" runat="server" RootID="1023" />
                            </td>
                            <td nowrap align="right" class="listTitle">
                                <asp:Literal ID="LitDealStatus" runat="server" Text="事件状态"></asp:Literal>
                            </td>
                            <td align="left" class="list" style="width: 3000px;">
                                <uc6:ctrFlowCataDropList ID="ctrDealStatus" runat="server" RootID="1017" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="listTitle" nowrap="nowrap">
                                <asp:Literal ID="LitInstancyName" runat="server" Text="紧急度"></asp:Literal>
                            </td>
                            <td class="list">
                                <uc6:ctrFlowCataDropList ID="CtrFCDInstancy" runat="server" RootID="1024" Width="152px" />
                            </td>
                            <td align="right" class="listTitle" nowrap="nowrap">
                                <asp:Literal ID="LitServiceType" runat="server" Text="事件类别"></asp:Literal>
                            </td>
                            <td class="list">
                                <uc11:ctrFlowCataDropListNew ID="ctrServiceType" runat="server" ShowType="2" RootID="1001" />
                                <input id="hidServiceTypeID" runat="server" type="hidden" />
                            </td>
                        </tr>
                        <tr>
                            <td nowrap align="right" class="listTitle">
                                <asp:Literal ID="LitCustTime" runat="server" Text="发生时间"></asp:Literal>
                            </td>
                            <td nowrap align="left" class="list">
                                <uc5:ctrDateSelectTime ID="ctrDateSelectTime1" runat="server" />
                            </td>
                            <td align="right" class="listTitle">
                                <asp:Literal ID="LitregUserName" runat="server" Text="登单人"></asp:Literal>
                            </td>
                            <td align="left" class="list">
                                <asp:TextBox ID="txtperson" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="listTitle" nowrap="nowrap">
                                <asp:Literal ID="LitServiceLevel" runat="server" Text="服务级别"></asp:Literal>
                            </td>
                            <td class="list">
                                <asp:DropDownList ID="ddltServiceLevel" runat="server" Width="152px">
                                </asp:DropDownList>
                                 <input id="hidServiceLevelID" runat="server" type="hidden" />
                                 <input id="hidServiceLevel" runat="server" type="hidden" />
                                 <asp:Label ID="labServiceLevel" runat="server" Visible="False"></asp:Label>
                            </td>
                            <td align="right" class="listTitle">
                                <asp:Literal ID="LitEquipmentName" runat="server" Text="资产名称"></asp:Literal>
                            </td>
                            <td align="left" class="list">
                                <asp:Label ID="lblEqu" runat="server" Visible="False"></asp:Label>
                                <asp:TextBox ID="txtEqu" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="listTitle" nowrap="nowrap" style="display: none">
                                <asp:Literal ID="LitServiceKind" runat="server" Text="事件性质"></asp:Literal>
                            </td>
                            <td class="list" style="display: none">
                                <uc6:ctrFlowCataDropList ID="ctrFCDWTType" runat="server" RootID="1002" Width="152px" />
                            </td>
                            <td align="right" class="listTitle" nowrap="nowrap">
                                <asp:Literal ID="LitSjwxr" runat="server" Text="工程师"></asp:Literal>
                            </td>
                            <td class="list" colspan="3">
                                <uc4:ServiceStaffMastCust ID="UserPSjzxr" runat="server" ></uc4:ServiceStaffMastCust>
                            </td>
                            <td align="right" class="listTitle" nowrap="nowrap" style="display:none;" >
                                <asp:Literal ID="Literal1" runat="server" Visible="false"  Text="邮件回访状态"></asp:Literal>
                            </td>
                            <td class="list" style="display:none;">
                                <asp:DropDownList ID="ddltEmailState" runat="server" Width="152px"  Visible="false" >
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="0">未邮件回访</asp:ListItem>
                                            <asp:ListItem Value="1">已邮件回访通知</asp:ListItem>
                                            <asp:ListItem Value="2">已邮件回访</asp:ListItem>
                                 </asp:DropDownList>
                              
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="listTitle">
                                <asp:Literal ID="Literal3" runat="server" Text="查询条件名称"></asp:Literal>
                            </td>
                            <td align="left" class="list" colspan="3">
                                <asp:TextBox ID="txtSQLName" runat="server"></asp:TextBox>
                                <asp:Label ID="rWarning" runat="server" Style="margin-left:7px;" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
                                <asp:Button ID="chkSave" runat="server" Text="保存" CssClass="btnClass" OnClick="chkSave_Click" />
                                <asp:Button ID="btn_delete" runat="server" Text="删除" CssClass="btnClass" OnClick="btn_delete_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center" class="list">
                                <asp:Button ID="btnOK" runat="server" Text="确定" CssClass="btnClass" OnClick="btnOK_Click" />
                                <asp:Button ID="btnClose" runat="server" Text="关闭" CssClass="btnClass" OnClick="btnClose_Click" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td>
                                <asp:Button ID="Button1" runat="server" CssClass="btnClass" OnClick="Button1_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="height: 300px">
                    <div id="_DisplayColumnDiv" style="display: none; top: 0; left: 601px; width: 375px;
                        height: 300px">
                        <table>
                            <tr style="height: 300px">
                                <td valign="top" style="height: 300px">
                                    <asp:ListBox ID="_TableColumnCheckBoxList" runat="server" Height="300px" Width="150px"
                                        SelectionMode="Multiple"></asp:ListBox>
                                </td>
                                <td valign="middle" align="center" class="listTitle">
                                    <input class="FLOWBUTTON" id="btnAdd" title="->" style="width: 44px; height: 24px"
                                        type="button" size="20" value="选择" name="btnAdd" runat="server"><br />
                                    <br />
                                    <input class="FLOWBUTTON" id="btnRemove" title="<-" style="width: 44px" type="button"
                                        size="20" value="移除" name="btnRemove" runat="server"><br />
                                    <br />
                                    <input class="FLOWBUTTON" id="btnClear" title="<<" style="width: 44px" type="button"
                                        size="30" value="清除" name="cmdSelect" runat="server">
                                </td>
                                <td style="width: 270px" valign="top">
                                <asp:ListBox ID="lsbDeptTo" Width="150px" runat="server" Height="300px" >
                                    </asp:ListBox>
                                </td>
                            </tr>
                </td>
            </tr>
        </table>
      
    </center>
    
    <script type="text/javascript">
        function GetRandom() {
            return Math.floor(Math.random() * 1000 + 1);
        }
        		//客户选择
			function CustomSelect(obj) 
			{
			    var CustName = document.all.<%=txtCustName.ClientID%>.value;
			    var CustAddress = "";
			    var CustLinkMan = "";
			    var CustTel = "";
			    var url="../Common/frmDRMUserSelectajax.aspx?IsSelect=true&randomid="+GetRandom()+"&CustID=0" 
				        + "&CustName=" + escape(CustName)  + "&CustAddress=" + escape(CustAddress)
				        + "&CustLinkMan=" + escape(CustLinkMan) + "&CustTel=" + escape(CustTel)
				        + "&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>"
				        + "&PageType=2" ;
				open(url, 'E8OpenWin_Sub', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800px,height=600px,left=150,top=50');
			}
			
			function SelectSomeCust(obj)   //选择多个客户
			{
			    var newDateObj = new Date()
	            var sparamvalue =  newDateObj.getYear().toString() + newDateObj.getMonth().toString();
	            var name = obj.value;
	            if(name=="")
	            {
	                return;
	            }
	            
	            var url = "../mydestop/frmQuickLocateCustAjax.aspx?IsSelect=true&randomid="+GetRandom()+"&Name=" + escape(name)+"&PageType=2&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>"
	            open(url, 'E8OpenWin_Sub', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800px,height=600px,left=150,top=50');
	            return;
	            
				var	value=window.showModalDialog("../mydestop/frmQuickLocateCustAjax.aspx?IsSelect=true&randomid="+GetRandom()+"&Name=" + escape(name),"","dialogHeight:500px;dialogWidth:900px");
				if(value != null)
				{				    
				    var json = value;
				    var record=json.record;
				    
					for(var i=0; i < record.length; i++)
					{
			            document.getElementById(obj.id.replace("txtCustName","txtCustName")).value = record[i].shortname;   //客户
					}
				}
				else
				{
				     document.getElementById(obj.id.replace("txtCustName","txtCustName")).value = "";    //客户ID号	
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
            if(obj.value=="")
	            {
	                document.getElementById(obj.id.replace("txtCustName","txtCustName") ).value="";
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
														    alert("此客户不存在！");	
			                                                document.getElementById(obj.id.replace("txtCustName","txtCustName") ).value = "";                                               
			                                               	                                         
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
					                                                    document.getElementById(obj.id.replace("txtCustName","txtCustName")).value = record[i].shortname;   //客户                    
					                                            }

													} 
												    } 
												}
					xmlhttp.send(null); 
				}catch(e3){}
            }
        }
          //显示字段
        
        function show(va)
        {
           var isClick =document.getElementById('<%=HidIS.ClientID %>').value;
         
             if(va==1&&isClick!="true")
             {
                 if( document.getElementById("_DisplayColumnDiv").style.display=="none")
                 {
                    window.dialogWidth="1500px";
                 }
             }             
           else
             {
                 if( document.getElementById("_DisplayColumnDiv").style.display=="none")
                 {
                    document.getElementById("_DisplayColumnDiv").style.display="block";
                    window.dialogWidth="1500px";
                 }else
                 {
                   document.getElementById("_DisplayColumnDiv").style.display="none";
                   window.dialogWidth="1500px";
                 }
                 if(isClick!="true")
                 document.getElementById("<%=Button1.ClientID %>").click();
             }
             
        } 
        function Choice_Dept()
		    {	
		  	  var isclicked = false;
		       //在JS端调用CheckBoxList    
              var chkObject = document.getElementById('<%=_TableColumnCheckBoxList.ClientID%>');    
            
              for(var i=0;i<chkObject.options.length;i++)    
               {    
                  if(chkObject.options[i].selected)   
                  {    
                        isclicked =true;
                        break;
                        
                   }    
                }  
		        
		        if (isclicked ==false)
				{
				    alert("请选择需要的字段名！");
				    return;
				}
				var ishave = false;
            for(var i=0;i<chkObject.options.length;i++)     
               {    
                  if(chkObject.options[i].selected)    
                  {    
                        var oOption = window.document.createElement("OPTION");
		                oOption.text= chkObject.options[i].text;
		                oOption.value= chkObject.options[i].value;
			            var j;
			            var iID = "<%=lsbDeptTo.ClientID %>";	
			            var objDeptTo = document.getElementById(iID);
			            if(objDeptTo.options.length>6)
			            {
			              alert("最多能选择显示7个字段,你选择超过7个字段");
			               var j;
		                    var oOption;
		                    var iID = "<%=lsbDeptTo.ClientID %>";	
				            var objDeptTo = document.getElementById(iID);
				            while(objDeptTo.options.length>0)
			                {
				                oOption=objDeptTo.options(0);
				                objDeptTo.remove(0);	        
			                }
			                Get_DeptIDs(objDeptTo);
			              break;
			            }
		                for (j=0; j<objDeptTo.options.length; j++) 
		                {
			                if (objDeptTo.options(j).value==chkObject.options[i].value)
			                {
				                ishave =true;
				                break;
			                }			        
		                }
		                if(!ishave)
		                {
		                    objDeptTo.add(oOption);
		                    Get_DeptIDs(objDeptTo);		
		                }else
		                {
		                    alert("警告，选择字段重复！");
		                    break;
		                }                        
                   }    
                } 
		    }
		    //删除已经选择
		    function Removeone()
		    {
		        var i;
		        var j;
		        var k = 0;
		        var iID = "<%=lsbDeptTo.ClientID %>";	
				var objDeptTo = document.getElementById(iID);
				i=objDeptTo.selectedIndex;
				if(i==-1)
				{
				    alert("请选择需要删除的字段！");
				}
				else
				{
				    oOption=objDeptTo.options(i);
				    objDeptTo.remove(i);
				    Get_DeptIDs(objDeptTo);
				}
		    }
		     //删除全部选择
		    function Removeall()
		    {
		        if(confirm("是否要清除字段吗？"))
		        {
		            var j;
		            var oOption;
		            var iID = "<%=lsbDeptTo.ClientID %>";	
				    var objDeptTo = document.getElementById(iID);
				    while(objDeptTo.options.length>0)
			        {
				        oOption=objDeptTo.options(0);
				        objDeptTo.remove(0);	        
			        }
			        Get_DeptIDs(objDeptTo);
				}
		    }
		    //获取相关字段ID串
		    function Get_DeptIDs(obj)
		    {
			    var i;
			    var DeptIDs="";
			    var DeptNames="";
			    for (i=0; i<obj.options.length; i++) 
			    {
				    DeptIDs=DeptIDs + obj.options(i).value + ",";
				    DeptNames=DeptNames +obj.options(i).text + "," ;    				
			    }    			
			    if(DeptIDs.length==1)	DeptIDs="";
    						
			    document.all.<%=hidDeptID.ClientID %>.value=DeptIDs;
			    document.all.<%=hidDeptName.ClientID %>.value=DeptNames;
			    
			    document.getElementById('<%=hidValue.ClientID %>').value = DeptIDs+"@"+DeptNames;
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
			    if(typeof(document.all.<%=UserPSjzxr.ClientID%>_hidCustID)!="undefined" )
			    {
			        document.all.<%=UserPSjzxr.ClientID%>_hidEquID.value = EquID;
			        document.all.<%=UserPSjzxr.ClientID%>_hidCustID.value = CustID;
			        document.all.<%=UserPSjzxr.ClientID%>_hidServiceTypeID.value = ServiceTypeID;
			        document.all.<%=UserPSjzxr.ClientID%>_hidServiceLevelID.value = ServiceLevelID;
			        
			        
			        document.all.<%=UserPSjzxr.ClientID%>_hidEquName.value = document.all.<%=txtEqu.ClientID%>.value.trim();
			        document.all.<%=UserPSjzxr.ClientID%>_hidCustName.value = document.all.<%=txtCustName.ClientID%>.value.trim();
			        
			        if (typeof(document.all.<%=ddltServiceLevel.ClientID%>)!="undefined" )
	                {     
                        document.all.<%=UserPSjzxr.ClientID%>_hidLevelName.value = document.all.<%=ddltServiceLevel.ClientID%>.value.trim();
                    }
                    else if (typeof(document.all.<%=labServiceLevel.ClientID%>)!="undefined" )
                    {
                        document.all.<%=UserPSjzxr.ClientID%>_hidLevelName.value = document.all.<%=labServiceLevel.ClientID%>.outerText;
                    }
			        if (typeof(document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_hidCatalogName"))!="undefined" )
	                {
                        document.all.<%=UserPSjzxr.ClientID%>_hidTypeName.value = document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_hidCatalogName").value;
                    }
                    else
                    {
                        document.all.<%=UserPSjzxr.ClientID%>_hidTypeName.value = document.getElementById("ctl00_ContentPlaceHolder1_ctrFCDServiceType_labCate1").innerHTML;
                    }
			    }
           
                
            }	
</script>

<!--Begin: 初始化基础脚本-->
<script src="../js/epower.base.js" type="text/javascript"></script>
<script type="text/javascript">    
    epower.advance_search = {};
    epower.advance_search.deptIdCtrlID = '<%=hidDeptID.ClientID %>';
    epower.advance_search.deptNameCtrlID = '<%=hidDeptName.ClientID %>';    
    epower.advance_search.hidValueCtrlID = '<%=hidValue.ClientID %>';    
</script>
<!--Begin: 初始化基础脚本-->

<!--Begin: 事件高级搜索脚本-->
<script src="../js/epower.appforms.advance_search.js" type="text/javascript"></script>
<!--End: 事件高级搜索脚本-->
</asp:Content>
