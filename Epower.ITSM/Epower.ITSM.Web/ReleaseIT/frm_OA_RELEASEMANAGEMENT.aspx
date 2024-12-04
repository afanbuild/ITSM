<%@ Page Language="C#" MasterPageFile="~/FlowForms.Master" AutoEventWireup="true"
    CodeBehind="frm_OA_RELEASEMANAGEMENT.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frm_OA_ReleaseManagement"
    Title="发布管理" %>

<%@ Register TagPrefix="uc3" TagName="CtrFlowFormText" Src="../Controls/CtrFlowFormText.ascx" %>
<%@ Register Src="~/Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc2" TagName="ctrdateandtime" Src="../Controls/CtrDateAndTimeV2.ascx" %>
<%@ Register Src="~/Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc6" %>
<%@ Register Src="~/Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/ctrattachment.ascx" TagName="ctrattachment" TagPrefix="uc1" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" src="../Js/App_Common.js"> </script>

    <script language="javascript" src="../Controls/Calendar/Popup.js"></script>

    <script language="javascript" type="text/javascript">
    <!--
            function TransferValue()
            {
                 if (typeof(parent.header.flowInfo.Subject)!="undefined" && typeof(document.all.<%=CtrVersionName.ClientID%>)!="undefined" )
                {
				    parent.header.flowInfo.Subject.value = "[" + document.all.<%=CtrVersionName.ClientID%>.value + "]发布管理";
				}
            }

            function DoUserValidate(lngActionID,strActionName)
	        {
				TransferValue();
			    return CheckValue();
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
        
            //
			function CheckValue()
			{
			    return true;
			}
			//
			String.prototype.trim = function()  
			{
				return this.replace(/(^\s*)|(\s*$)/g, "");
			}
			
			function CustomSelect(obj) 
			{
			    var CustName = document.all.<%=txtCustAddr.ClientID%>.value; 
				var	value=window.showModalDialog("../Common/frmDRMUserSelect.aspx?IsSelect=true&randomid="+GetRandom()+"&CustID=" + document.getElementById(obj.id.replace('cmdCust','hidCustID')).value 
				    + "&FlowID="+ '<%=FlowID%>' + "&CustName=" + escape(CustName)   ,window,"dialogHeight:600px;dialogWidth:800px");
				if(value != null)
				{
					if(value.length>1)
					{
			            document.getElementById(obj.id.replace("cmdCust","txtCustAddr")).value = value[1];   //客户
			            document.getElementById(obj.id.replace("cmdCust","hidCust") ).value = value[1]; 
			            document.getElementById(obj.id.replace("cmdCust","txtCTel")).value = value[3];   //联系人电话 
			            document.getElementById(obj.id.replace("cmdCust","hidCustID")).value = value[4];    //客户ID号 
					}
				}
				else
				{
				    document.getElementById(obj.id.replace("cmdCust","txtCustAddr")).value = "";   //客户
		            document.getElementById(obj.id.replace("cmdCust","hidCust") ).value = ""; 
		            document.getElementById(obj.id.replace("cmdCust","txtCTel")).value = "";   //联系人电话
 
		            document.getElementById(obj.id.replace("cmdCust","hidCustID")).value = 0;    //客户ID号 
				}
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
				var	value=window.showModalDialog("../mydestop/frmQuickLocateCust.aspx?IsSelect=true&randomid="+GetRandom()+"&Name=" + escape(name),"","dialogHeight:500px;dialogWidth:560px");
				if(value != null)
				{
					if(value.length>1)
					{
				        document.getElementById(obj.id.replace("txtCustAddr","txtCustAddr")).value = value[1];   //客户
			            document.getElementById(obj.id.replace("txtCustAddr","hidCust") ).value = value[1]; 
 
			            document.getElementById(obj.id.replace("txtCustAddr","txtCTel")).value = value[3];   //联系人电话
  
			            document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = value[4];    //客户ID号 
					}
				}
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
					xmlhttp.open("GET", "../MyDestop/frmXmlHttp.aspx?randomid="+GetRandom()+"&Cust=" + escape(obj.value), true); 
                    xmlhttp.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
					xmlhttp.onreadystatechange = function() 
												{ 
													if ( xmlhttp.readyState==4 ) 
													{ 
														if(xmlhttp.responseText=="-1")  //没有
														{
														   // document.getElementById(obj.id.replace("txtCustAddr","hidCustID") ).value.trim()!="0"
														    alert("此用户不存在，请重新查找！");	
			                                                document.getElementById(obj.id.replace("txtCustAddr","hidCustID")).value = "0";    //客户ID号		                                                
			                                               	                                         
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
//			                                              
			                                                document.getElementById(obj.id.replace("txtCustAddr","txtCTel")).value = arr[4];   //联系人电话
 
													    }
													} 
												} 
					xmlhttp.send(null); 
				}catch(e3){}
            }
        }	 
  //-->
    </script>

    <input id="hidCustID" runat="server" type="hidden" value="-1" />
    <input id="hidCust" runat="server" type="hidden" />
    <table id="Table1" width="100%" align="center" class="listContent">
        <tr>
            <td nowrap class="listTitle" style="width: 110px">
                版本名称
            </td>
            <td class="list" style="width: *">
                <uc3:CtrFlowFormText ID='CtrVersionName' runat='server' MaxLength="200" TextToolTip='版本名称'
                    MustInput="true" Width="216px" />
            </td>
            <td style="height: 28px; width: 110px" nowrap class="listTitle">
                版&nbsp;&nbsp;本&nbsp;&nbsp;号
            </td>
            <td style="height: 28px; width: *" class="list">
                <uc3:CtrFlowFormText ID='CtrVersionCode' runat='server' MaxLength="200" TextToolTip='版本号'
                    MustInput="true" Width="216px" />
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitle" style="width: 110px">
                发布时间
            </td>
            <td class="list">
                <uc2:ctrdateandtime ID="CtrDCreateRegTime" runat="server" ShowTime="false" />
            </td>
            <td nowrap class="listTitle" style="width: 110px">
                发布范围
            </td>
            <td class="list">
                <uc6:ctrFlowCataDropList ID="ctrReleaseScope" runat="server" RootID="1037" MustInput="true"
                    TextToolTip="发布范围" />
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitle" style="width: 110px">
                联&nbsp;&nbsp;系&nbsp;&nbsp;人
            </td>
            <td class="list">
                <%--<uc3:UserPicker ID="UserPicker1" runat="server" MustInput="true" TextToolTip="联系人"/>--%>
                <asp:Label ID="labCustAddr" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtCustAddr" runat="server" Width="120px" onblur="GetCustID(this)"
                    MaxLength="200"></asp:TextBox>
                <input id="cmdCust" onclick="CustomSelect(this)" type="button" value="..." runat="server"
                    class="btnClass" title="可在文本框中输入电话、姓名、工号、邮件地址实现查询" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtCustAddr"
                    runat="server" ErrorMessage="必须选择联系人"></asp:RequiredFieldValidator>
            </td>
            <td nowrap class="listTitle" style="width: 110px">
                联系电话
            </td>
            <td class="list">
                <asp:Label ID="labCTel" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtCTel" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                <%-- <uc3:CtrFlowFormText ID='CtrPhone' runat='server' MaxLength="200" TextToolTip='联系电话'
                    MustInput="true" Width="216px" />--%>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtCTel"
                    runat="server" ErrorMessage="必须选择联系人电话"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitle" style="width: 110px">
                版本性质
            </td>
            <td class="list">
                <uc6:ctrFlowCataDropList ID="ctrVersionKind" runat="server" RootID="1038" MustInput="true"
                    TextToolTip="版本性质" />
            </td>
            <td nowrap class="listTitle" style="width: 110px">
                版本类型
            </td>
            <td nowrap class="list">
                <uc6:ctrFlowCataDropList ID="ctrVersionType" runat="server" RootID="1039" MustInput="true"
                    TextToolTip="版本类型" />
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitle" style="width: 110px">
                版本投产要求
            </td>
            <td class="list" colspan="3">
                <asp:DataGrid ID="gvBillItem" runat="server" Width="100%" OnItemCommand="gvBillItem_ItemCommand"
                    ShowFooter="True" AutoGenerateColumns="false" CssClass="listContent_1">
                    <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="建议时间" ItemStyle-Width="12%" HeaderStyle-Width="12%"
                            FooterStyle-Width="13%">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblSUGGESTDATE" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.SUGGESTDATE")%>'></asp:Label>
                                <uc2:ctrdateandtime ID="CtrSUGGESTDATE" runat="server" dateTime='<%# DataBinder.Eval(Container, "DataItem.SUGGESTDATE")%>'
                                    ShowTime="false" Disparity="true" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <uc2:ctrdateandtime ID="CtrSUGGESTDATEAdd" runat="server" ShowTime="false" Disparity="true" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="版  本  号" ItemStyle-Width="10%" HeaderStyle-Width="10%"
                            FooterStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblVERSIONCODE" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.VERSIONCODE")%>'></asp:Label>
                                <asp:TextBox ID="txtVERSIONCODE" Text='<%# DataBinder.Eval(Container, "DataItem.VERSIONCODE")%>'
                                    runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <span style="float: left">
                                    <asp:TextBox ID="txtVERSIONCODEAdd" runat="server" Text='' Width="85%"></asp:TextBox><font
                                        color="red">*</font></span>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="实施负责人" ItemStyle-Width="10%" HeaderStyle-Width="10%"
                            FooterStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblRESPONSIBLEPERSON" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.RESPONSIBLEPERSON")%>'></asp:Label>
                                <asp:TextBox ID="txtRESPONSIBLEPERSON" Text='<%# DataBinder.Eval(Container, "DataItem.RESPONSIBLEPERSON")%>'
                                    runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <span style="float: left">
                                    <asp:TextBox ID="txtRESPONSIBLEPERSONAdd" runat="server" Text='' Width="85%"></asp:TextBox><font
                                        color="red">*</font></span>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="应用室实施人" ItemStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblAPPIMPLE" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.APPIMPLE")%>'></asp:Label>
                                <asp:TextBox ID="txtAPPIMPLE" Text='<%# DataBinder.Eval(Container, "DataItem.APPIMPLE")%>'
                                    runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:TextBox ID="txtAPPIMPLEAdd" runat="server" Text='' Width="90%"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="系统室实施人" ItemStyle-Width="10%" HeaderStyle-Width="10%"
                            FooterStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblSYSIMPLE" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.SYSIMPLE")%>'></asp:Label>
                                <asp:TextBox ID="txtSYSIMPLE" Text='<%# DataBinder.Eval(Container, "DataItem.SYSIMPLE")%>'
                                    runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:TextBox ID="txtSYSIMPLEAdd" runat="server" Text='' Width="90%"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="设备室实施人" ItemStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblEQUIPEIMPLE" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.EQUIPEIMPLE")%>'></asp:Label>
                                <asp:TextBox ID="txtEQUIPEIMPLE" Text='<%# DataBinder.Eval(Container, "DataItem.EQUIPEIMPLE")%>'
                                    runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:TextBox ID="txtEQUIPEIMPLEAdd" runat="server" Text='' Width="90%"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="网络室实施人" ItemStyle-Width="10%" HeaderStyle-Width="10%"
                            FooterStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblNETIMPLE" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.NETIMPLE")%>'></asp:Label>
                                <asp:TextBox ID="txtNETIMPLE" Text='<%# DataBinder.Eval(Container, "DataItem.NETIMPLE")%>'
                                    runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:TextBox ID="txtNETIMPLEAdd" runat="server" Text='' Width="90%"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="其它室实施人" ItemStyle-Width="10%" HeaderStyle-Width="10%"
                            FooterStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblOTHERIMPLE" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.OTHERIMPLE")%>'></asp:Label>
                                <asp:TextBox ID="txtOTHERIMPLE" Text='<%# DataBinder.Eval(Container, "DataItem.OTHERIMPLE")%>'
                                    runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:TextBox ID="txtOTHERIMPLEAdd" runat="server" Text='' Width="90%"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="计划发布时间" ItemStyle-Width="12%" HeaderStyle-Width="12%"
                            FooterStyle-Width="13%">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblPLANRELEASEDATE" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.PLANRELEASEDATE")%>'></asp:Label>
                                <asp:TextBox ID="txtPLANRELEASEDATE" Text='<%# DataBinder.Eval(Container, "DataItem.PLANRELEASEDATE")%>'
                                    runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:TextBox ID="txtPLANRELEASEDATEAdd" runat="server" Text='' Width="90%"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn ItemStyle-Width="6%" HeaderStyle-Width="6%" FooterStyle-Width="4%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btndelete" CommandName="Delete" runat="server" Text="删除" CausesValidation="False"
                                    CssClass="btnClass" UseSubmitBehavior="false"></asp:Button>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Button ID="btnadd" CommandName="Add" runat="server" Text="新增" CausesValidation="False"
                                    CssClass="btnClass"></asp:Button>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitle" style="width: 110px">
                版本发布内容简介
            </td>
            <td class="list" colspan="3">
                <asp:Label ID="lblContent" runat="server" Visible="False"></asp:Label>
                <ftb:FreeTextBox ID="txtContent" runat="server" Width="100%" ButtonPath="../Forms/images/epower/officexp/"
                    ImageGalleryPath="Attfiles\\Photos" Height="200px">
                </ftb:FreeTextBox>
            </td>
        </tr>
    </table>

</asp:Content>
