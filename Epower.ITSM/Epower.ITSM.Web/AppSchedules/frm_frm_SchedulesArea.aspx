<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_frm_SchedulesArea.aspx.cs" Inherits="Epower.ITSM.Web.AppSchedules.frm_frm_SchedulesArea" %>

<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc5" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <table id="Table1" align="center" style="width: 98%;" cellpadding="0" class="listContent">
        <tr>
            <td align="right" class="listTitle" width="12%">
                开始日期:
            </td>
            <td class="list" align="Left">
                <uc:ctrdateandtime ID="txtStartDate" runat="server" MustInput="true" ShowTime="false"  >
                </uc:ctrdateandtime>
            </td>
            <td align="right" height="22" class="listTitle" width="100px">
                结束日期:
            </td>
            <td class="list" align="Left">
                <uc:ctrdateandtime ID="txtEndDate" runat="server" MustInput="true" ShowTime="false">
                </uc:ctrdateandtime>
            </td>
            <td nowrap align="Center" class="listTitle">
                <asp:Button ID="Button1" runat="server" Text="开始排班" CssClass="btnClass" OnClick="btnStart_Click"  OnClientClick="javascript:return confirmStart();" /> 
            </td>
        </tr>
        <tr>
            <td valign="top" align="center" colspan="5" class="listContent">
                <asp:DataGrid ID="gridScheduleLog" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="Gridtable" OnItemCommand="gridScheduleLog_ItemCommand" 
                    onitemcreated="gridScheduleLog_ItemCreated" 
                    onitemdatabound="gridScheduleLog_ItemDataBound">
                    <Columns>
                        <asp:TemplateColumn HeaderText="序号">
                            <ItemTemplate>
                                <%# Container.ItemIndex + 1%>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="StartDate" HeaderText="开始日期" DataFormatString="{0:yyyy-MM-dd}">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EndDate" HeaderText="结束日期" DataFormatString="{0:yyyy-MM-dd}">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="备注">
                            <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("remarks") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="">
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                     
                            <ItemTemplate>
                                <span style=" margin-left:10px;"></span>
                                <asp:LinkButton ID="lnkView" runat="server" Text="查看" CommandArgument='<%#Container.ItemIndex %>'
                                    CommandName="View"></asp:LinkButton>
                                <asp:LinkButton ID="lnkSupplement" runat="server" Text="补班" CommandArgument='<%#Container.ItemIndex %>'
                                    CommandName="Supplement"></asp:LinkButton>
                                <asp:LinkButton ID="lnkSchedules" runat="server" Text="排班" 
                                    OnClientClick='javascript:return confirmStartRow(<%#Eval("STATUS") %>);'
                                    CommandArgument='<%#Container.ItemIndex %>'
                                    CommandName="Do"></asp:LinkButton>
                                <asp:LinkButton ID="lnkDelete" runat="server" Text="删除"  OnClientClick="javascript:return confirm('您确定要删除该期排班信息吗？');"
                                    CommandArgument='<%#Container.ItemIndex %>'
                                    CommandName="Delete"></asp:LinkButton>
                                <asp:HiddenField ID="hidAreaId" runat="server"  Value ='<%#Eval("AreaId") %>' />
                                <asp:HiddenField ID="hidStartDate" runat="server" Value='<%#Eval("StartDate") %>' />
                                <asp:HiddenField ID="hidEndDate" runat="server" Value='<%#Eval("EndDate") %>' />
                                <asp:HiddenField ID="hidAreaStatus" runat="server" Value='<%#Eval("STATUS") %>'  />
                                <asp:HiddenField ID="hidNOISSUESCOUNT" runat="server" Value='<%#Eval("NOISSUESCOUNT") %>'  />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr id="trShowControlPage" runat="server">
            <td width="500" height="30">
            </td>
            <td align="right">
                <%--<uc5:controlpagefoot id="ControlPageScheduleInfo" runat="server" />--%>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidPreAreaRemark" runat="server" />
    <script type="text/javascript">
        function confirmStart() {

            if ($("#<%=txtStartDate.ClientID %>_txtDate").val() == "") {
                alert("请输入开始日期。");
                return false;
            }
            if ($("#<%=txtEndDate.ClientID %>_txtDate").val() == "") {
                alert("请输入结束日期。");
                return false;
            }
            var msg = "您确定开始"
                    + $("#<%=txtStartDate.ClientID %>_txtDate").val()
                    + "到"
                    + $("#<%=txtEndDate.ClientID %>_txtDate").val() + "的排班吗？";

            return confirm(msg);
        }


        function confirmStartRow(status) {
            if (status == "0") {
                return confirm("您确定开始该期的排班信息吗");
            }
            else {
                return true;
            }
        }
    </script>
</asp:Content>
