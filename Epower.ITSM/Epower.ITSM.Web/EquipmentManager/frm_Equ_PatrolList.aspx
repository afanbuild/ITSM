<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frm_Equ_PatrolList.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frm_Equ_PatrolList" Title="设备/产品巡检记录" %>
<%@ Register Src="../Controls/controlpage.ascx" TagName="controlpage" TagPrefix="uc1" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register src="../Controls/ControlPageFoot.ascx" tagname="ControlPageFoot" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <table cellpadding="0" cellspacing="0" width="100%" border="0" >
            <tr>
                <td class="listContent">
                    <asp:DataGrid ID="grd" runat="server" Width="100%" cellpadding="1" cellspacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCreated="grd_ItemCreated">
                        <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                        <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			            <HeaderStyle CssClass="listTitle"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField='Title' HeaderText='标题'></asp:BoundColumn>
					        <asp:BoundColumn DataField='RegUserName' HeaderText='登记人'></asp:BoundColumn>
					        <asp:BoundColumn DataField='RegDeptName' HeaderText='登记人部门'></asp:BoundColumn>  
					        <asp:TemplateColumn HeaderText="登记日期">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "RegTime", "{0:d}")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="流程状态">
		                        <HeaderStyle Width="8%"></HeaderStyle>
		                        <ItemTemplate>
			                        <%#((EpowerGlobal.e_FlowStatus)DataBinder.Eval(Container.DataItem, "status")) == EpowerGlobal.e_FlowStatus.efsHandle ? "<font color='blue'>正在处理</font>" : (((EpowerGlobal.e_FlowStatus)DataBinder.Eval(Container.DataItem, "status")) == EpowerGlobal.e_FlowStatus.efsStop ? "<font color='red'>暂停</font>" : "<font color='green'>正常结束</font>")%>
		                        </ItemTemplate>
	                        </asp:TemplateColumn>                     
                            <asp:TemplateColumn HeaderText="处理">
								<HeaderStyle Width="5%" HorizontalAlign="center"></HeaderStyle>
								<ItemStyle HorizontalAlign="Center" />
								<ItemTemplate>
									<INPUT id="CmdDeal" class="btnClass" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>' type="button" value='查看' runat="server">
								</ItemTemplate>
							</asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td align="right" class="listTitle">
                    <uc2:ControlPageFoot ID="cpPatrol" runat="server" />
                </td>
            </tr>
        </table>
</asp:Content>
