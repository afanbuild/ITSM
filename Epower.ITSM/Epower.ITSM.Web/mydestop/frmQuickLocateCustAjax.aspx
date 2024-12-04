<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmQuickLocateCustAjax.aspx.cs" Inherits="Epower.ITSM.Web.MyDestop.frmQuickLocateCustAjax"
    Title="快速查询用户" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
        function ServerOndblclick(jsonstr) {
            //==zxl==
           // window.parent.returnValue = jsonstr;
           if(jsonstr != null)
				{
				    var json = jsonstr;
				    var record=json.record;
				    
					for(var i=0; i < record.length; i++)
					{
			            window.opener.document.getElementById("<%=Opener_ClientId%>txtCustAddr").value = record[i].shortname;   //客户
			            window.opener.document.getElementById("<%=Opener_ClientId%>hidCust").value = record[i].shortname;
			            window.opener.document.getElementById( "<%=Opener_ClientId%>txtAddr").value = record[i].address;   //地址
			            window.opener.document.getElementById("<%=Opener_ClientId%>hidAddr").value = record[i].address;   //地址
			            window.opener.document.getElementById("<%=Opener_ClientId%>txtContact").value = record[i].linkman1;    //联系人


			            window.opener.document.getElementById("<%=Opener_ClientId%>hidContact").value = record[i].linkman1;   
			            window.opener.document.getElementById("<%=Opener_ClientId%>txtCTel").value = record[i].tel1;   //联系人电话


			            window.opener.document.getElementById("<%=Opener_ClientId%>hidTel").value = record[i].tel1;
			            window.opener.document.getElementById("<%=Opener_ClientId%>hidCustID").value = record[i].id;    //客户ID号

			           
			            window.opener.document.getElementById("<%=Opener_ClientId%>lblCustDeptName").innerHTML = record[i].custdeptname;   //所属部门
		                window.opener.document.getElementById("<%=Opener_ClientId%>lblEmail").innerHTML = record[i].email;  //电子邮件
		                window.opener.document.getElementById("<%=Opener_ClientId%>lblMastCust").innerHTML = record[i].mastcustname;   //服务单位
		                window.opener.document.getElementById("<%=Opener_ClientId%>hidCustDeptName").value = record[i].custdeptname;   //所属部门
		                window.opener.document.getElementById("<%=Opener_ClientId%>hidCustEmail").value = record[i].email;  //电子邮件
		                window.opener.document.getElementById("<%=Opener_ClientId%>hidMastCust").value = record[i].mastcustname;   //服务单位
		                window.opener.document.getElementById("<%=Opener_ClientId%>lbljob").innerHTML = record[i].job;   //职位
		                window.opener.document.getElementById("<%=Opener_ClientId%>hidjob").value = record[i].job;   //职位
			            
			            if (typeof(document.getElementById("<%=Opener_ClientId%>Table3"))!="undefined")
			            {
			                window.opener.document.getElementById("<%=Opener_ClientId%>txtEqu").value = record[i].equname;   //资产名称
                            window.opener.document.getElementById("<%=Opener_ClientId%>hidEquName").value = record[i].equname;   //资产名称
                            window.opener.document.getElementById("<%=Opener_ClientId%>hidEqu").value = record[i].equid;  //资产IDontact")).id);                            
                            
                            window.opener.document.getElementById("<%=Opener_ClientId%>txtListName").value = record[i].listname;   //资产目录
                            window.opener.document.getElementById("<%=Opener_ClientId%>hidListName").value = record[i].listname;   //资产目录
                            window.opener.document.getElementById("<%=Opener_ClientId%>hidListID").value = record[i].listid;  //资产目录ID
                            
                        }               
					}
				}
				else
				{
				     window.opener.document.getElementById("<%=Opener_ClientId%>hidCustID").value = "";    //客户ID号	
			         window.opener.document.getElementById("<%=Opener_ClientId%>txtCustAddr").value="";//客户名称	
			                                                
			         window.opener.document.getElementById("<%=Opener_ClientId%>hidCust").value = "";
			         window.opener.document.getElementById("<%=Opener_ClientId%>txtAddr").value = "";   //地址
			         window.opener.document.getElementById("<%=Opener_ClientId%>hidAddr").value = "";   //地址
			         window.opener.document.getElementById("<%=Opener_ClientId%>txtContact").value = "";    //联系人
			         window.opener.document.getElementById("<%=Opener_ClientId%>hidContact").value = "";   
			         window.opener.document.getElementById("<%=Opener_ClientId%>txtCTel").value = "";   //联系人电话
			         window.opener.document.getElementById("<%=Opener_ClientId%>hidTel").value = "";			                                                            
			         window.opener.document.getElementById("<%=Opener_ClientId%>lblCustDeptName").innerHTML = "";   //所属部门
		             window.opener.document.getElementById("<%=Opener_ClientId%>lblEmail").innerHTML = "";  //电子邮件
		             window.opener.document.getElementById("<%=Opener_ClientId%>lblMastCust").innerHTML = "";   //服务单位
		             window.opener.document.getElementById("<%=Opener_ClientId%>hidCustDeptName").value = "";   //所属部门
		             window.opener.document.getElementById("<%=Opener_ClientId%>hidCustEmail").value = "";  //电子邮件
		             window.opener.document.getElementById("<%=Opener_ClientId%>hidMastCust").value = "";   //服务单位
		             window.opener.document.getElementById("<%=Opener_ClientId%>lbljob").innerHTML = "";   //职位
		             window.opener.document.getElementById("<%=Opener_ClientId%>hidjob").value = "";   //职位
				}           
            top.close();
        }
        function OnClientClick(selectcommandId)
        {            
            $("#" + selectcommandId ).click();
        }
    </script>

    <table id="Table1" bordercolor="#000000" cellspacing="1" bordercolordark="#ffffff"
        cellpadding="1" width="98%" bordercolorlight="#000000" border="0">
        <tr>
            <td valign="top" align="center" colspan="2" class="listContent">
                <asp:DataGrid ID="dgUserInfo" runat="server" Width="100%" PageSize="50" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    GridLines="Horizontal" CellPadding="1" CellSpacing="2" BorderWidth="0px" OnItemCommand="dgUserInfo_ItemCommand"
                    OnItemDataBound="dgUserInfo_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="id" HeaderText="id"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ShortName" HeaderText="客户名称">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Address" HeaderText="客户地址">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="LinkMan1" HeaderText="联系人">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Tel1" HeaderText="联系人电话">
                            <HeaderStyle />
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustomCode" HeaderText="客户代码">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Email" HeaderText="电子邮件">
                            <HeaderStyle />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="MastCust" HeaderText="服务单位">
                            <HeaderStyle />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="选择">
                            <ItemTemplate>
                                <asp:Button runat="server" ID="lnkselect" CommandName="Select" Text="选择" SkinID="btnClass1" />
                            </ItemTemplate>
                            <HeaderStyle Width="5%" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle Visible="False" HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF"
                        Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td class="listTitle" align="right">
                <input id="hidQueryDeptID" style="width: 72px; height: 19px" type="hidden" size="6"
                    runat="server">
                <input id="hidDeptID" style="width: 72px; height: 19px" type="hidden" size="6" runat="server">
            </td>
        </tr>
    </table>
</asp:Content>
