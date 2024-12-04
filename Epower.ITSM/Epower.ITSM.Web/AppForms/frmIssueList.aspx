<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmIssueList.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmIssueList"
    Title="历史事件参考" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="Table1" style="width: 98%" border="0" cellpadding="0" cellspacing="0"
        runat="server">
        <tr>
            <td>
                <asp:DataGrid ID="gridUndoMsg" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCreated="gridUndoMsg_ItemCreated" OnItemDataBound="gridUndoMsg_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="subject" HeaderText="标题">
                            <HeaderStyle Width="40%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="RegUserName" HeaderText="受理人">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustTime" HeaderText="发生时间" DataFormatString="{0:g}">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="custName" HeaderText="客户">
                            <HeaderStyle Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="contact" HeaderText="联系人">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="dealstatus" HeaderText="处理状态" HeaderStyle-Wrap="false">
                            <HeaderStyle Width="5%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="status" HeaderText="流程状态" HeaderStyle-Wrap="false">
                            <HeaderStyle Width="5%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="处理">
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemTemplate>
                                <input id="CmdDeal" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                    type="button" value='详情' runat="server" causesvalidation="false" class="btnClass1">
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="dealstatus"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="endtime"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc1:ControlPage ID="ControlPageIssues" runat="server"></uc1:ControlPage>
            </td>
        </tr>
    </table>
</asp:Content>
