<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmServiceMonitor_Child.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmServiceMonitor_Child" Title="服务监控详情" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input id="hidStrArr" type="hidden" runat="server" />
    <table  id="TabIssue" width="98%" border="0" cellpadding="0" cellspacing="0" runat="server">
        <tr id="trIssue" runat="server" align="left">
            <td id="tdResultChart" runat="server" align="left" style="width:50%">
                <asp:DataGrid ID="gridIssues" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemDataBound="dg_ItemDataBound" OnItemCreated="dg_ItemCreated">
                    <Columns>
                        <asp:TemplateColumn HeaderText="事件单号" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="Lb_ServiceNo" Text='<%#DataBinder.Eval(Container, "DataItem.ServiceNo")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="CustName" HeaderText="客户名称" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustAddress" HeaderText="客户地址" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="15%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ctel" HeaderText="联系电话" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustTime" HeaderText="发生时间" DataFormatString="{0:g}" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Content" HeaderText="详细描述" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="15%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="详情">                      
                            <HeaderStyle  HorizontalAlign="Center" Width="5%"></HeaderStyle>     
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                    type="button" value="详情" name="CmdDeal" runat="server"/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
         <tr id="trShowControlPage" runat="server">
            <td align="right">
                <uc5:ControlPageFoot ID="cpCST_Issue" runat="server" />
            </td>
        </tr>
    </table>
    <table id="TabChange" width="98%" border="0" cellpadding="0" cellspacing="0" runat="server">
        <tr id="trChange" runat="server" align="left">
            <td id="td1" runat="server" align="left" style="width:50%">
                <asp:DataGrid ID="dgChange" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemDataBound="dg_ItemDataBound" OnItemCreated="dg_ItemCreated">
                    <Columns>
                        <asp:BoundColumn DataField="ChangeNo" HeaderText="变更单号"  ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Custname" HeaderText="客户名称"  ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
					    <asp:BoundColumn DataField="Contact" HeaderText="联系人" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
					    <asp:BoundColumn DataField="EquipmentName" HeaderText="资产名称" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
					    <asp:BoundColumn DataField="ChangeTime" HeaderText="变更日期" DataFormatString="{0:g}" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
					    <asp:BoundColumn DataField="DealStatus" HeaderText="变更状态" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="详情">                      
                            <HeaderStyle  HorizontalAlign="Center" Width="5%"></HeaderStyle>     
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                    type="button" value="详情" name="CmdDeal" runat="server"/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
       <tr id="tr1" runat="server">
            <td align="right">
                <uc5:ControlPageFoot ID="cpCST_Change" runat="server" />
            </td>
       </tr>
      </table>
    <table id="tabProblem" width="98%" border="0" cellpadding="0" cellspacing="0" runat="server">
        <tr id="tr2" runat="server" align="left">
            <td id="td2" runat="server" align="left" style="width:50%">
                <asp:DataGrid ID="dgProblem" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"   ItemStyle-HorizontalAlign="Left" OnItemDataBound="dg_ItemDataBound" OnItemCreated="dg_ItemCreated">
                    <Columns>
                        <asp:TemplateColumn HeaderText="问题单号" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="Lb_ProblemNo" Text='<%#DataBinder.Eval(Container, "DataItem.ProblemNo")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Problem_TypeName" HeaderText="问题类别" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Problem_LevelName" HeaderText="问题级别" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="StateName" HeaderText="状态" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="详情">
                            <HeaderStyle  HorizontalAlign="Center" Width="5%"></HeaderStyle>     
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                    type="button" value="详情" name="CmdDeal" runat="server"/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
       <tr id="tr3" runat="server">
            <td align="right">
                <uc5:ControlPageFoot ID="cpCST_Problem" runat="server" />
            </td>
       </tr>
      </table>
     <table id="tabEqu" width="98%" border="0" cellpadding="0" cellspacing="0" runat="server">
        <tr id="tr4" runat="server" align="left">
            <td id="td3" runat="server" align="left" style="width:50%">
                <asp:DataGrid ID="dgEqu" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemDataBound="dg_ItemDataBound" OnItemCreated="dg_ItemCreated">
                    <Columns>
                        <asp:BoundColumn DataField='Code' HeaderText='资产编号'  ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField='costomname' HeaderText='所属客户' ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="详情">
                            <ItemTemplate>
                                <input id="CmdDealEqu" class="btnClass1" onclick='<%#GetEqu((decimal)DataBinder.Eval(Container.DataItem, "ID"))%>'
                                    type="button" value="详情" runat="server"/>
                                <%--<asp:Button ID="lnkLook" CssClass="btnClass" runat="server" Text="详情" CommandName="look" />--%>
                            </ItemTemplate>
                            <HeaderStyle  HorizontalAlign="Center" Width="5%"></HeaderStyle>     
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
       <tr id="tr5" runat="server">
            <td align="right">
                <uc5:ControlPageFoot ID="cpCST_Equ" runat="server" />
            </td>
       </tr>
      </table>
     <table id="tabInf" width="98%" border="0" cellpadding="0" cellspacing="0" runat="server">
        <tr id="tr6" runat="server" align="left">
            <td id="td4" runat="server" align="left" style="width:50%">
                <asp:DataGrid ID="dgInf" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemDataBound="dg_ItemDataBound" OnItemCreated="dg_ItemCreated">
                    <Columns>
                        <asp:BoundColumn DataField='PKey' HeaderText='关键字' ItemStyle-HorizontalAlign="Left">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='TypeName' HeaderText='类型' ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='Tags' HeaderText='摘要' ItemStyle-HorizontalAlign="Left">
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="详情">
                        <HeaderStyle  HorizontalAlign="Center" Width="5%"></HeaderStyle>     
                            <ItemTemplate>
                                <input id="CmdDealInf" class="btnClass1" onclick='<%#GetInf((decimal)DataBinder.Eval(Container.DataItem, "ID"))%>'
                                    type="button" value="详情" runat="server"/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
       <tr id="tr7" runat="server">
            <td align="right">
                <uc5:ControlPageFoot ID="cpCST_Inf" runat="server" />
            </td>
       </tr>
      </table>
</asp:Content>
