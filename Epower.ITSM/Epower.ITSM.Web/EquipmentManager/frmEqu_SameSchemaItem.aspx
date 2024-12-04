<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEqu_SameSchemaItem.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_SameSchemaItem"
    Title="资产列表" %>

<%@ Register Src="../Controls/ctrEquCataDropList.ascx" TagName="ctrEquCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/BussinessControls/CustomCtr.ascx" TagName="CustomCtr"
    TagPrefix="uc5" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register src="../Controls/DeptPicker.ascx" tagname="DeptPicker" tagprefix="uc3" %>
<%@ Register src="../Controls/UserPicker.ascx" tagname="UserPicker" tagprefix="uc4" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .high 
        {
        	BORDER-RIGHT: #228DC7 0px solid; 
            BORDER-TOP: #228DC7 0px solid; 
            BORDER-LEFT: #228DC7 0px solid; 
            BORDER-BOTTOM: #228DC7 0px solid;
            PADDING-RIGHT: 5px; 
            PADDING-LEFT: 5px; 
            background-image:url(../Images/an1.gif);
           width: 68px; 
           height:23px;
           CURSOR: hand;
        }
    </style>
    <script language="javascript" type="text/javascript">
var openobj = window;
if(typeof(window.dialogArguments) == "object")
{
    openobj =  window.dialogArguments;
}
//设备服务记录
function SelectService(obj) 
{
    var lngID = document.getElementById(obj.id.replace("CmdService","hidID")).value;
	openobj.open("../AppForms/frmIssueList.aspx?NewWin=true&ID=0&EquID=" + lngID ,'','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');
	event.returnValue = false;
}
//设备巡检
function SelectByts(obj) 
{
    var lngID = document.getElementById(obj.id.replace("CmdByts","hidID")).value;
	openobj.open("frm_Equ_PatrolList.aspx?EquID=" + lngID ,'','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');
	event.returnValue = false;
}

function BatchUpdateCanDo()
{
     var obj = document.getElementById('<%=btnBatchUpdate.ClientID %>');
     if(obj != null)
        obj.disabled = "";
}

	//全选复选框
		function checkAll(checkAll)
		{			  
			var len = document.forms[0].elements.length;
			var cbCount = 0;
			for (i=0;i < len;i++)
			{
				if (document.forms[0].elements[i].type == "checkbox")
				{
					if (document.forms[0].elements[i].name.indexOf("chkSelect") != -1 && 
						document.forms[0].elements[i].name.indexOf("dgEqu_Desk") != -1 &&
						document.forms[0].elements[i].disabled == false)
					{
						document.forms[0].elements[i].checked = checkAll.checked;

						cbCount += 1;
					}
				}
			}		
		} 
        
        
        function BatchImplyEqu(obj)
        {
            var value = window.showModalDialog('frmEquBatchRelMain.aspx?newWin=true&ItemFieldID=' + document.all.<%=HidItemFieldID.ClientID %>.value 
                             + '&ItemFieldValue=' + document.all.<%=HidItemFieldValue.ClientID %>.value + '&ID=' + document.all.<%=HidEqusID.ClientID %>.value,window,"dialogHeight:600px;dialogWidth:800px");
            
            if(value != null)
            {
                var json = value;
                var record = json.record;
                
                document.all.<%=hidImpEquID.ClientID%>.value= record[0].id;   //资产ID
                document.getElementById('<%=lblImplyEqu.ClientID%>').innerText= record[0].name;   //资产名称
            }
            else
            {
                document.all.<%=hidImpEquID.ClientID%>.value= "0";   //资产ID
                document.getElementById('<%=lblImplyEqu.ClientID%>').innerText= '';   //资产名称
            }        
            
            event.returnValue = false; 
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
        
        function GetEquID(obj)
        {            
            if(obj.value=="")
	            {
	                document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value = "0";  
	                return;
	            }
	        if(obj.value==document.getElementById(obj.id.replace("txtEqu","hidEquName")).value && (document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value!="" || document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value!="0"))
	            {
	                return;
	            }
	            
	        var varEquIDs = document.all.<%=HidEqusID.ClientID%>.value;
	        var varEquName = document.all.<%=txtEqu.ClientID%>.value;
            if(xmlhttp == null)
                xmlhttp = CreateXmlHttpObject();       
            if(xmlhttp != null)
            {
                try
                {	
					xmlhttp.open("GET", "../MyDestop/frmXmlHttpAjax.aspx?EquIDs=" + escape(varEquIDs) + "&EquName=" + escape(varEquName) + "&randomid="+GetRandom(), true); 
                    xmlhttp.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
					xmlhttp.onreadystatechange = function() 
												{ 
													if ( xmlhttp.readyState==4 ) 
													{ 
														if(xmlhttp.responseText=="-1")  //没有
														{
														    alert("此资产不存在，请重新查找！"); 
														 
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
													        var json= eval("(" +xmlhttp.responseText + ")");
				                                            var record=json.record;
				                                            
				                                            for(var i=0; i < record.length; i++)
					                                        {
													            document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = record[i].name;   //设备名称
			                                                    document.getElementById(obj.id.replace("txtEqu","hidEquName")).value = record[i].name;   //设备名称
			                                                    document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value = record[i].id;  //设备IDontact")).id);
			                                                    
			                                                    document.all.<%=deskPropCh.ClientID %>.click();		                                                    			                                                    
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
			    var varEquIDs = document.all.<%=HidEqusID.ClientID%>.value;
	            var name = obj.value;
	            if(name == "")
	            {
	                return;
	            }
				var	value=window.showModalDialog("../mydestop/frmQuickLocateEqu.aspx?IsChange=true&IsSelect=true&randomid="+GetRandom() + "&Name=" + name + "&EquIDs=" + escape(varEquIDs),"","dialogHeight:500px;dialogWidth:600px");
				if(value != null)
				{
					if(value.length>1)
					{
				        document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = value[2];   //设备名称
                        document.getElementById(obj.id.replace("txtEqu","hidEquName")).value = value[2];   //设备名称
                        document.getElementById(obj.id.replace("txtEqu","hidEqu") ).value = value[1];  //设备IDontact")).id);      
                        
                        document.all.<%=deskPropCh.ClientID %>.click();		                                          
					}
					else
					{
					    document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = "";   //设备名称
                        document.getElementById(obj.id.replace("txtEqu","hidEquName")).value = "";   //设备名称
                        document.getElementById(obj.id.replace("txtEqu","hidEqu")).value = "0";  //设备IDontact")).id);     
                        
                        document.all.<%=deskPropCh.ClientID %>.click();		                                                                
					}
				}
				else
				{
				    document.getElementById(obj.id.replace("txtEqu","txtEqu")).value = document.getElementById(obj.id.replace("txtEqu","hidEquName")).value;   //设备名称
				}
			}
        
        function GetRandom() {
                return Math.floor(Math.random() * 1000 + 1);
            }
            
        //选择资产
        function SelectEqu(obj) 
			{          
			    var varEquIDs = document.all.<%=HidEqusID.ClientID%>.value;
			    //===zxl
			    var url="../EquipmentManager/frmEqu_DeskMainImply.aspx?IsSelect=1&randomid="+GetRandom()+ "&EquIDs="+ escape(varEquIDs)+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
			    window.open(url,"",'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=50');
			}
			
		function SellChanceFirm()
        {
            var len = document.forms[0].elements.length;
            var cbCount = 0;
                for (i = 0; i < len; i++) {
                    if (document.forms[0].elements[i].type == "checkbox") {
                        if (document.forms[0].elements[i].name.indexOf("chkSelect") != -1 &&
						    document.forms[0].elements[i].name.indexOf("dgEqu_Desk") != -1 &&
						    document.forms[0].elements[i].disabled == false) {
						    if(document.forms[0].elements[i].checked==true)
                                cbCount += 1;
                        }
                    }
                }
                
                if(cbCount<=0)
                {
                    alert("请确认选择的资产列表不为空！");
                    return false;
                }
               
                event.returnValue = true;
            

        }
    </script>
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
    <input id="hidEqu" type="hidden" runat="server" value="-1" />
    <input id="hidEquName" type="hidden" runat="server" />
    <input type="hidden" runat="server" id="HidEqusID" />
    <input type="hidden" runat="server" id="HidItemFieldID" />
    <input type="hidden" runat="server" id="HidItemFieldValue" />
    <div id="divClick" runat="server" style="display: none">
        <asp:Button ID="deskPropCh" runat="server" OnClick="btnDeskPropChange_Click" />
    </div>
    <table cellpadding='1' cellspacing='2' width='98%' border='0' class="listContent">
        <tr>
            <td align="right" class="listTitle" style="width: 12%">
                配置项名称

            </td>
            <td class="list" style="width: 35%">
                <asp:TextBox ID='txtFieldID' runat='server' Width="208px" Enabled="False"></asp:TextBox>
            </td>
            <td align="right" class="listTitle" style="width: 12%">
                匹配值

            </td>
            <td class="list">
                <asp:TextBox ID='txtItemValue' runat='server' Width="144px"></asp:TextBox>
                <asp:CheckBox ID="chkItemValue" runat="server" />
                <uc7:ctrFlowCataDropList ID="ctrFlowCataDropDefault" runat="server" RootID="1" OnChangeScript="BatchUpdateCanDo();" />
                <uc3:DeptPicker ID="DeptPicker1" runat="server"  OnChangeScript="BatchUpdateCanDo();"  />
                <uc4:UserPicker ID="UserPicker1" runat="server" OnChangeScript="BatchUpdateCanDo();" />
                <asp:Button ID="btnBatchUpdate" runat="server" Text="批量更新" OnClick="btnBatchUpdate_Click" />
                <%--<asp:Button ID="btnImplyEqu" runat="server" Text="批量关联资产" OnClientClick="BatchImplyEqu(this);" />--%>
                <input type="hidden" runat="server" id="hidImpEquID" />
                <asp:Label ID="lblImplyEqu" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitle">
                <asp:Label Text="关联资产" runat="server"></asp:Label>
            </td>
            <td align="left" class="list">
                <asp:TextBox ID="txtEqu" runat="server" Width="120px" MaxLength="80" onblur="GetEquID(this)"></asp:TextBox>
                <input id="cmdEqu" onclick="SelectEqu(this)" type="button" value="..." runat="server"
                    name="cmdEqu" class="btnClass2" />
                <asp:DropDownList ID="ddlDeskProp" runat="server">
                </asp:DropDownList>
            </td>
            <td class="list" align="right">
                <uc6:ctrFlowCataDropList ID="txt_RelDescription" runat="server" RootID="1052" />
            </td>
            <td class="list">
                <asp:Button ID="btnImplyEquProp" runat="server" SkinID="btnClass3" Text="批量关联资产" OnClick="SaveRel_Click" />
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgEqu_Desk" runat="server" Width="100%" CellPadding="1" CellSpacing="2"
                    BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgEqu_Desk_ItemCommand"
                    OnItemDataBound="dgEqu_Desk_ItemDataBound" OnItemCreated="dgEqu_Desk_ItemCreated">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);" />
                            </HeaderTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Name' HeaderText='资产名称'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='Code' HeaderText='资产编号'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='costomname' HeaderText='所属客户'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='MastCustName' HeaderText='服务单位'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='CatalogName' HeaderText='资产类别'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='Default' Visible="false"></asp:BoundColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:TextBox ID="txtCurrValue" runat="server" Style='<%# GetControlDisplayStatus("0") %>'
                                    Text='<%# DataBinder.Eval(Container, "DataItem.Default")%>' Width="95%" Enabled="False"></asp:TextBox><asp:CheckBox
                                        ID="chkCurrValue" runat="server" Style='<%# GetControlDisplayStatus("1") %>'
                                        Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.Default"))%>'
                                        Enabled="False" />
                                        <asp:Label ID="Label1" runat="server" Style='<%# GetControlDisplayStatus("2") %>' Enabled="false"><%# DataBinder.Eval(Container, "DataItem.Default")%></asp:Label>
                                        <asp:Label ID="lblDefalutValue3" runat="server" Style='<%# GetControlDisplayStatus("3") %>' Enabled="false"></asp:Label>
                                        <asp:Label ID="lblDefalutValue4" runat="server" Style='<%# GetControlDisplayStatus("4") %>' Enabled="false"></asp:Label>
                                        <asp:Label ID="lblDefalutValue5" runat="server" Style='<%# GetControlDisplayStatus("5") %>' Enabled="false"></asp:Label>
                                        <asp:Label ID="Label2" runat="server" Style='<%# GetControlDisplayStatus("6") %>' Enabled="false"><%# DataBinder.Eval(Container, "DataItem.Default")%></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Style='<%# GetControlDisplayStatus("7") %>' Enabled="false"><%# DataBinder.Eval(Container, "DataItem.Default")%></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="False" HeaderText="修改">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="修改" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="60"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="详情">
                            <ItemTemplate>
                                <asp:Button ID="lnkLook" SkinID="btnClass1" runat="server" Text="详情" CommandName="look" />
                            </ItemTemplate>
                            <HeaderStyle Width="60"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="事件记录" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <input type="hidden" runat="server" id="hidID" value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                                <asp:Button ID="CmdService" runat="server" Text="事件记录" OnClientClick="SelectService(this);"
                                    SkinID="btnClass1" CausesValidation="false" />
                            </ItemTemplate>
                            <HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc1:ControlPage ID="ControlPage1" runat="server"></uc1:ControlPage>
            </td>
        </tr>
    </table>
    <script type="text/jscript">
        $(document).ready(function() 
        {
               $("#<%=btnBatchUpdate.ClientID %>").attr("class", "high"); 
               $("#<%=btnImplyEquProp.ClientID%>").attr("class", "high");           
        });
    </script>
</asp:Content>
