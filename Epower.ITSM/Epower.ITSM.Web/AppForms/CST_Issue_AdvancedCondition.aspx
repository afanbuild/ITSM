<%@ Page Language="c#" 
    Inherits="Epower.ITSM.Web.Forms.CST_Issue_AdvancedCondition"
    CodeBehind="CST_Issue_AdvancedCondition.aspx.cs" %>
<%@ Register Src="../Controls/ServiceStaffMastCust.ascx" TagName="ServiceStaffMastCust"
    TagPrefix="uc4" %>
<%@ Register Src="../Controls/ServiceStaff.ascx" TagName="ServiceStaff" TagPrefix="uc3" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc2" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/ctrDateSelectTime.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc5" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc11" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <base target="_self" />
    <title>�¼����߼���ѯ�Ի���</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    
    
    <script type="text/javascript" src="../Js/jquery-1.7.2.min.js"></script>  
    <script type="text/javascript" src="../Js/Plugin/jquery.autocomplete.js"></script>
  
    <script type="text/javascript" src="../Js/App_Common.js"></script>
    
    <link  rel="stylesheet" type="text/css" href="../Js/themes/jquery.autocomplete.css" />
<style type="text/css" >
.hiddenControl;
{
visibility:hidden;
}
</style>
</head>

<script type="text/javascript">
        function GetRandom() {
            return Math.floor(Math.random() * 1000 + 1);
        }
        		//�ͻ�ѡ��
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
			
			function SelectSomeCust(obj)   //ѡ�����ͻ�
			{
			    var newDateObj = new Date()
	            var sparamvalue =  newDateObj.getYear().toString() + newDateObj.getMonth().toString();
	            var name = obj.value;
	            if(name=="")
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
			            document.getElementById(obj.id.replace("txtCustName","txtCustName")).value = record[i].shortname;   //�ͻ�
					}
				}
				else
				{
				     document.getElementById(obj.id.replace("txtCustName","txtCustName")).value = "";    //�ͻ�ID��	
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
														if(xmlhttp.responseText=="-1")  //û��
														{														   
														    alert("�˿ͻ������ڣ�");	
			                                                document.getElementById(obj.id.replace("txtCustName","txtCustName") ).value = "";                                               
			                                               	                                         
													    }
													    else if(xmlhttp.responseText=="0") //�ҵ����
													    {
													        SelectSomeCust(obj);
													    }
													    else  //�ҵ�Ψһ
													    {
													        
													            
													      	    var json= eval("("+xmlhttp.responseText+")");
				                                                var record=json.record;
                                            				    
					                                            for(var i=0; i < record.length; i++)
					                                            {   
					                                                    document.getElementById(obj.id.replace("txtCustName","txtCustName")).value = record[i].shortname;   //�ͻ�                    
					                                            }

													} 
												    } 
												}
					xmlhttp.send(null); 
				}catch(e3){}
            }
        }
          //��ʾ�ֶ�
        
        function show(va)
        {
        
           var isClick =document.getElementById('<%=HidIS.ClientID %>').value;
         
             if(va==1&&isClick!="true")
             {
                 if( document.getElementById("_DisplayColumnDiv").style.display=="none")
                 {
                    window.dialogWidth="1400px";
                 }
             }             
           else
             {
                 if( document.getElementById("_DisplayColumnDiv").style.display=="none")
                 {
                    document.getElementById("_DisplayColumnDiv").style.display="block";
                    window.dialogWidth="1400px";
                 }else
                 {
                   document.getElementById("_DisplayColumnDiv").style.display="none";
                   window.dialogWidth="1400px";
                 }
                 if(isClick!="true")
                 document.getElementById("<%=Button1.ClientID %>").click();
             }
             
        } 
        function Choice_Dept()
		    {	
		  	  var isclicked = false;
		       //��JS�˵���CheckBoxList    
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
				    alert("��ѡ����Ҫ���ֶ�����");
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
			              alert("�����ѡ����ʾ7���ֶ�,��ѡ�񳬹�7���ֶ�");
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
		                    alert("���棬ѡ���ֶ��ظ���");
		                    break;
		                }                        
                   }    
                } 
		    }
		    //ɾ���Ѿ�ѡ��
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
				    alert("��ѡ����Ҫɾ�����ֶΣ�");
				}
				else
				{
				    oOption=objDeptTo.options(i);
				    objDeptTo.remove(i);
				    Get_DeptIDs(objDeptTo);
				}
		    }
		     //ɾ��ȫ��ѡ��
		    function Removeall()
		    {
		        if(confirm("�Ƿ�Ҫ����ֶ���"))
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
		    //��ȡ����ֶ�ID��
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

<body onload="show(1)">
    <form id="form1" runat="server">
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
                                <strong style="font-weight: bold; font-size: 20px; color: #08699E;">�¼����߼���ѯ</strong>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap align="right" class="listTitle">
                                ѡ���ղز�ѯ����
                            </td>
                            <td align="left" class="list">
                                <asp:DropDownList ID="DropSQLwSave" runat="server" Width="152px" AutoPostBack="True"
                                    OnSelectedIndexChanged="DropSQLwSave_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td nowrap align="right" class="listTitle">
                                ��ʾ���
                            </td>
                            <td align="left" class="list">
                                <asp:DropDownList ID="DropDownList1" runat="server" Width="152px" AutoPostBack="True"
                                    OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                </asp:DropDownList>
                                <input type="button" runat="server" id="show" value="��ʾ�ֶ�" onclick="show(2)" class="btnClass" />
                            </td>
                        </tr>
                        <tr>
                            <td nowrap align="right" class="listTitle" style="width: 12%">
                                <asp:Literal ID="LitCustName" runat="server" Text="�ͻ�����"></asp:Literal>
                            </td>
                            <td nowrap align="left" class="list" style="width: 35%">
                                <asp:TextBox ID="txtCustName" runat="server" onblur="GetCustID(this)"></asp:TextBox>
                                <input id="cmdCust" onclick="CustomSelect(this)" type="button" value="..." runat="server"
                                    class="btnClass2" />
                            </td>
                            <td nowrap align="right" class="listTitle" style="width: 12%">
                                <asp:Literal ID="LitServiceNo" runat="server" Text="�¼�����"></asp:Literal>
                            </td>
                            <td align="left" class="list">
                                <asp:TextBox ID="txtIssueNo" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr><td align="right" class="listTitle" nowrap="nowrap">
                                <asp:Literal ID="LitSubject" runat="server" Text="ժҪ"></asp:Literal>
                            </td>
                            <td class="list">
                                <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
                            </td>
                            
                            <td nowrap align="right" class="listTitle">
                                ����״̬
                            </td>
                            <td align="left" class="list">
                                <asp:DropDownList ID="cboStatus" runat="server" Width="152px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap align="right" class="listTitle">
                                <asp:Literal ID="LitEffectName" runat="server" Text="Ӱ���"></asp:Literal>
                            </td>
                            <td align="left" class="list">
                                <uc6:ctrFlowCataDropList ID="CtrFCDEffect" runat="server" RootID="1023" />
                            </td>
                            <td nowrap align="right" class="listTitle">
                                <asp:Literal ID="LitDealStatus" runat="server" Text="�¼�״̬"></asp:Literal>
                            </td>
                            <td align="left" class="list" style="width: 3000px;">
                                <uc6:ctrFlowCataDropList ID="ctrDealStatus" runat="server" RootID="1017" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="listTitle" nowrap="nowrap">
                                <asp:Literal ID="LitInstancyName" runat="server" Text="������"></asp:Literal>
                            </td>
                            <td class="list">
                                <uc6:ctrFlowCataDropList ID="CtrFCDInstancy" runat="server" RootID="1024" Width="152px" />
                            </td>
                            <td align="right" class="listTitle" nowrap="nowrap">
                                <asp:Literal ID="LitServiceType" runat="server" Text="�¼����"></asp:Literal>
                            </td>
                            <td class="list">
                                <uc11:ctrFlowCataDropListNew ID="ctrServiceType" runat="server" ShowType="2" RootID="1001" />
                                <input id="hidServiceTypeID" runat="server" type="hidden" />
                            </td>
                        </tr>
                        <tr>
                            <td nowrap align="right" class="listTitle">
                                <asp:Literal ID="LitCustTime" runat="server" Text="����ʱ��"></asp:Literal>
                            </td>
                            <td nowrap align="left" class="list">
                                <uc5:ctrDateSelectTime ID="ctrDateSelectTime1" runat="server" />
                            </td>
                            <td align="right" class="listTitle">
                                <asp:Literal ID="LitregUserName" runat="server" Text="�ǵ���"></asp:Literal>
                            </td>
                            <td align="left" class="list">
                                <asp:TextBox ID="txtperson" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="listTitle" nowrap="nowrap">
                                <asp:Literal ID="LitServiceLevel" runat="server" Text="���񼶱�"></asp:Literal>
                            </td>
                            <td class="list">
                                <asp:DropDownList ID="ddltServiceLevel" runat="server" Width="152px">
                                </asp:DropDownList>
                                 <input id="hidServiceLevelID" runat="server" type="hidden" />
                                 <input id="hidServiceLevel" runat="server" type="hidden" />
                                 <asp:Label ID="labServiceLevel" runat="server" Visible="False"></asp:Label>
                            </td>
                            <td align="right" class="listTitle">
                                <asp:Literal ID="LitEquipmentName" runat="server" Text="�ʲ�����"></asp:Literal>
                            </td>
                            <td align="left" class="list">
                                <asp:Label ID="lblEqu" runat="server" Visible="False"></asp:Label>
                                <asp:TextBox ID="txtEqu" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="listTitle" nowrap="nowrap" style="display: none">
                                <asp:Literal ID="LitServiceKind" runat="server" Text="�¼�����"></asp:Literal>
                            </td>
                            <td class="list" style="display: none">
                                <uc6:ctrFlowCataDropList ID="ctrFCDWTType" runat="server" RootID="1002" Width="152px" />
                            </td>
                            <td align="right" class="listTitle" nowrap="nowrap">
                                <asp:Literal ID="LitSjwxr" runat="server" Text="����ʦ"></asp:Literal>
                            </td>
                            <td class="list" colspan="3">
                                <uc4:ServiceStaffMastCust ID="UserPSjzxr" runat="server" ></uc4:ServiceStaffMastCust>
                            </td>
                            <td align="right" class="listTitle" nowrap="nowrap" style="display:none;" >
                                <asp:Literal ID="Literal1" runat="server" Visible="false"  Text="�ʼ��ط�״̬"></asp:Literal>
                            </td>
                            <td class="list" style="display:none;">
                                <asp:DropDownList ID="ddltEmailState" runat="server" Width="152px"  Visible="false" >
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="0">δ�ʼ��ط�</asp:ListItem>
                                            <asp:ListItem Value="1">���ʼ��ط�֪ͨ</asp:ListItem>
                                            <asp:ListItem Value="2">���ʼ��ط�</asp:ListItem>
                                 </asp:DropDownList>
                              
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="listTitle">
                                <asp:Literal ID="Literal3" runat="server" Text="��ѯ��������"></asp:Literal>
                            </td>
                            <td align="left" class="list" colspan="3">
                                <asp:TextBox ID="txtSQLName" runat="server"></asp:TextBox>
                                <asp:Button ID="chkSave" runat="server" Text="����" CssClass="btnClass" OnClick="chkSave_Click" />
                                <asp:Button ID="btn_delete" runat="server" Text="ɾ��" CssClass="btnClass" OnClick="btn_delete_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center" class="list">
                                <asp:Button ID="btnOK" runat="server" Text="ȷ��" CssClass="btnClass" OnClick="btnOK_Click" />
                                <asp:Button ID="btnClose" runat="server" Text="�ر�" CssClass="btnClass" OnClick="btnClose_Click" />
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
                                        type="button" size="20" value="ѡ��" name="btnAdd" runat="server" onclick="Choice_Dept();"><br />
                                    <br />
                                    <input class="FLOWBUTTON" id="btnRemove" title="<-" style="width: 44px" type="button"
                                        size="20" value="�Ƴ�" name="btnRemove" runat="server" onclick="Removeone();"><br />
                                    <br />
                                    <input class="FLOWBUTTON" id="btnClear" title="<<" style="width: 44px" type="button"
                                        size="30" value="���" name="cmdSelect" runat="server" onclick="Removeall();">
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
    </form>
</body>
</html>
