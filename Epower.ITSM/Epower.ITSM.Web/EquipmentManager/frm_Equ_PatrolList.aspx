<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frm_Equ_PatrolList.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frm_Equ_PatrolList" Title="�豸/��ƷѲ���¼" %>
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
                            <asp:BoundColumn DataField='Title' HeaderText='����'></asp:BoundColumn>
					        <asp:BoundColumn DataField='RegUserName' HeaderText='�Ǽ���'></asp:BoundColumn>
					        <asp:BoundColumn DataField='RegDeptName' HeaderText='�Ǽ��˲���'></asp:BoundColumn>  
					        <asp:TemplateColumn HeaderText="�Ǽ�����">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "RegTime", "{0:d}")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="����״̬">
		                        <HeaderStyle Width="8%"></HeaderStyle>
		                        <ItemTemplate>
			                        <%#((EpowerGlobal.e_FlowStatus)DataBinder.Eval(Container.DataItem, "status")) == EpowerGlobal.e_FlowStatus.efsHandle ? "<font color='blue'>���ڴ���</font>" : (((EpowerGlobal.e_FlowStatus)DataBinder.Eval(Container.DataItem, "status")) == EpowerGlobal.e_FlowStatus.efsStop ? "<font color='red'>��ͣ</font>" : "<font color='green'>��������</font>")%>
		                        </ItemTemplate>
	                        </asp:TemplateColumn>                     
                            <asp:TemplateColumn HeaderText="����">
								<HeaderStyle Width="5%" HorizontalAlign="center"></HeaderStyle>
								<ItemStyle HorizontalAlign="Center" />
								<ItemTemplate>
									<INPUT id="CmdDeal" class="btnClass" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>' type="button" value='�鿴' runat="server">
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
