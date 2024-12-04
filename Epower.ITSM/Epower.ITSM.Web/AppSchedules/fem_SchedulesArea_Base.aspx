<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="fem_SchedulesArea_Base.aspx.cs" Inherits="Epower.ITSM.Web.AppSchedules.fem_SchedulesArea_Base" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc5" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="Table1" align="center" style="width: 98%;" cellpadding="0" class="listContent">
        <tr>
            <td align="right" class="listTitle" width="100%">
                <asp:Button ID="btnAdd" runat="server" Text="添加" onclick="btnAdd_Click" />
            </td>
        </tr>
        <tr>
            <td valign="top" align="center" colspan="5" class="listContent">
                <asp:DataGrid ID="dgSchedulesAreaBase" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="Gridtable" OnItemCommand="dgSchedulesAreaBase_ItemCommand">
                    <Columns>
                        <asp:TemplateColumn HeaderText="序号">
                            <ItemTemplate>
                                <%# Container.ItemIndex + 1%>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="fullName" HeaderText="全称">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="simpleName" HeaderText="简称">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="startdaytime" HeaderText="上班时间">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="endsegmenttime" HeaderText="休息时间">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="startsegmenttime" HeaderText="休息上班时间">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="enddaytime" HeaderText="下班时间">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="是否跨天">
                            <ItemTemplate>
                                <asp:Label ID="lblCurrName" runat="server" Text='<%#Eval("overdayflag").ToString()=="1"?"是":"否" %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="">
                            <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkView" runat="server" Text="详情" CommandArgument='<%#Eval("schedulesid") %>'
                                    CommandName="View"></asp:LinkButton>
                                <asp:LinkButton ID="lnkSchedules" runat="server" Text="删除" CommandArgument='<%#Eval("schedulesid") %>'  Visible="false" 
                                   OnClientClick="javascript:return confirm('您确定要删除该班次信息吗？');" CommandName="Delete"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr id="trShowControlPage" runat="server">
            <td width="500" height="30">
            </td>
            <td align="right">
                <%--<uc5:controlpagefoot id="cpSchedulesAreaBase" runat="server" />--%>
            </td>
        </tr>
    </table>
</asp:Content>
