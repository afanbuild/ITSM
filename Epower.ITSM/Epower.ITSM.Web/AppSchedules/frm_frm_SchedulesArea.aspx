<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_frm_SchedulesArea.aspx.cs" Inherits="Epower.ITSM.Web.AppSchedules.frm_frm_SchedulesArea" %>

<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc5" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <table id="Table1" align="center" style="width: 98%;" cellpadding="0" class="listContent">
        <tr>
            <td align="right" class="listTitle" width="12%">
                ��ʼ����:
            </td>
            <td class="list" align="Left">
                <uc:ctrdateandtime ID="txtStartDate" runat="server" MustInput="true" ShowTime="false"  >
                </uc:ctrdateandtime>
            </td>
            <td align="right" height="22" class="listTitle" width="100px">
                ��������:
            </td>
            <td class="list" align="Left">
                <uc:ctrdateandtime ID="txtEndDate" runat="server" MustInput="true" ShowTime="false">
                </uc:ctrdateandtime>
            </td>
            <td nowrap align="Center" class="listTitle">
                <asp:Button ID="Button1" runat="server" Text="��ʼ�Ű�" CssClass="btnClass" OnClick="btnStart_Click"  OnClientClick="javascript:return confirmStart();" /> 
            </td>
        </tr>
        <tr>
            <td valign="top" align="center" colspan="5" class="listContent">
                <asp:DataGrid ID="gridScheduleLog" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="Gridtable" OnItemCommand="gridScheduleLog_ItemCommand" 
                    onitemcreated="gridScheduleLog_ItemCreated" 
                    onitemdatabound="gridScheduleLog_ItemDataBound">
                    <Columns>
                        <asp:TemplateColumn HeaderText="���">
                            <ItemTemplate>
                                <%# Container.ItemIndex + 1%>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="StartDate" HeaderText="��ʼ����" DataFormatString="{0:yyyy-MM-dd}">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EndDate" HeaderText="��������" DataFormatString="{0:yyyy-MM-dd}">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="��ע">
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
                                <asp:LinkButton ID="lnkView" runat="server" Text="�鿴" CommandArgument='<%#Container.ItemIndex %>'
                                    CommandName="View"></asp:LinkButton>
                                <asp:LinkButton ID="lnkSupplement" runat="server" Text="����" CommandArgument='<%#Container.ItemIndex %>'
                                    CommandName="Supplement"></asp:LinkButton>
                                <asp:LinkButton ID="lnkSchedules" runat="server" Text="�Ű�" 
                                    OnClientClick='javascript:return confirmStartRow(<%#Eval("STATUS") %>);'
                                    CommandArgument='<%#Container.ItemIndex %>'
                                    CommandName="Do"></asp:LinkButton>
                                <asp:LinkButton ID="lnkDelete" runat="server" Text="ɾ��"  OnClientClick="javascript:return confirm('��ȷ��Ҫɾ�������Ű���Ϣ��');"
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
                alert("�����뿪ʼ���ڡ�");
                return false;
            }
            if ($("#<%=txtEndDate.ClientID %>_txtDate").val() == "") {
                alert("������������ڡ�");
                return false;
            }
            var msg = "��ȷ����ʼ"
                    + $("#<%=txtStartDate.ClientID %>_txtDate").val()
                    + "��"
                    + $("#<%=txtEndDate.ClientID %>_txtDate").val() + "���Ű���";

            return confirm(msg);
        }


        function confirmStartRow(status) {
            if (status == "0") {
                return confirm("��ȷ����ʼ���ڵ��Ű���Ϣ��");
            }
            else {
                return true;
            }
        }
    </script>
</asp:Content>
