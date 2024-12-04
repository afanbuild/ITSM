<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="CST_Issue_Base_Self_List.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.CST_Issue_Base_Self_List"
    Title="事件单查询" %>

<%@ Register Src="../Controls/ServiceStaffMastCust.ascx" TagName="ServiceStaffMastCust"
    TagPrefix="uc4" %>
<%@ Register Src="../Controls/ServiceStaff.ascx" TagName="ServiceStaff" TagPrefix="uc3" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>

    <script language="javascript" src="../Js/Common.js"></script>

    <script language="javascript" type="text/javascript">
        function checkAll(objectCheck) {
            var demo = document.getElementById('<%=gridUndoMsg.ClientID%>');
            var gg = demo.getElementsByTagName('INPUT');
            for (i = 0; i < gg.length; i++) {
                if (gg[i].type == "checkbox") {
                    gg[i].checked = objectCheck.checked;
                }
            }
        }

        function ShowDetailsInfo(obj, id) {
            $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "frmBr_CustomXmlHttp.aspx?ZHServiceDP=" + id }).responseText; } });
        }
    

    </script>

    <style>
        #tooltip
        {
            position: absolute;
            z-index: 3000;
            border: 1px solid #111;
            background-color: #eee;
            padding: 5px;
            opacity: 0.85;
        }
        #tooltip h3, #tooltip div
        {
            margin: 0;
        }
    </style>
    <input type="hidden" runat="server" id="hidIsGaoji" value="0" />
    <table class="listTitleNew" width="98%" align="center" id="tbTest" runat="server">
        <tr id="trShowCondi" runat="server">
            <td class="list" align="left" valign="top">
                <table width="100%" id="Table1">
                    <tr valign="top">
                        <td align="left" style="border-right-style: none;">
                            <asp:LinkButton ID="lkbMy" runat="server" OnClick="lkbMy_Click">由我登记</asp:LinkButton>
                            <asp:LinkButton ID="lkbHandle" runat="server" OnClick="lkbHandle_Click">正在处理</asp:LinkButton>
                            <asp:LinkButton ID="lkbEnd" runat="server" OnClick="lkbEnd_Click">正常结束</asp:LinkButton>
                            <asp:LinkButton ID="lkbOverTimeF" runat="server" OnClick="lkbOverTimeF_Click">超时完成</asp:LinkButton>
                            <asp:LinkButton ID="lkbOverTimeU" runat="server" OnClick="lkbOverTimeU_Click">超时未完成</asp:LinkButton>
                            <asp:LinkButton ID="lkbUnFeedBack" runat="server" OnClick="lkbUnFeedBack_Click">未回访事件</asp:LinkButton>
                            <asp:DropDownList ID="ddlPeriod" runat="server" Width="84px">
                                <asp:ListItem Value="0">一个月内</asp:ListItem>
                                <asp:ListItem Value="1">三个月内</asp:ListItem>
                                <asp:ListItem Value="2">六个月内</asp:ListItem>
                                <asp:ListItem Value="3">一年内</asp:ListItem>
                                <asp:ListItem Value="4">全部</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="98%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td colspan="2">
                <asp:DataGrid ID="gridUndoMsg" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnDeleteCommand="gridUndoMsg_DeleteCommand" OnItemCreated="gridUndoMsg_ItemCreated"
                    OnItemDataBound="gridUndoMsg_ItemDataBound">
                    <Columns>
                        <asp:TemplateColumn Visible="false">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="事件单号">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="Lb_ServiceNo" Text='<%#DataBinder.Eval(Container, "DataItem.ServiceNo")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="CustName" HeaderText="客户名称">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ctel" HeaderText="联系电话">
                            <HeaderStyle Width="8%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ServiceLevel" HeaderText="服务级别">
                            <HeaderStyle Width="15%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustTime" HeaderText="发生时间" DataFormatString="{0:g}">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                            
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EquipmentName" HeaderText="资产名称">
                            <HeaderStyle Width="8%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="dealstatus" HeaderText="处理状态">
                            <HeaderStyle Width="5%" Wrap="False" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Content" HeaderText="详细描述">
                            <HeaderStyle Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="处理">
                            <HeaderStyle Width="5%" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>  
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl(Convert.ToString(DataBinder.Eval(Container.DataItem, "FlowID")))%>'
                                    type="button" value='<%#GetButtonValue(Convert.ToString(DataBinder.Eval(Container.DataItem, "status")))%>'
                                    name="CmdDeal" runat="server" /> <%--(int)--%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="dealstatus"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="endtime"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="status"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr id="trShowControlPage" runat="server">
            <td width="500" height="30">
            </td>
            <td align="right">
                <uc5:ControlPageFoot ID="cpCST_Issue" runat="server" />
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
        ShowMessageBox="true" ShowSummary="False" />
</asp:Content>
