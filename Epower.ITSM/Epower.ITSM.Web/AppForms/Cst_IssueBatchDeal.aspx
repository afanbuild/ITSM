<%@ Page Title="批量指派" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Cst_IssueBatchDeal.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.Cst_IssueBatchDeal" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        function hideMe(id, status) {

            var object = document.getElementById(id);

            if (object != null) {
                object.style.display = status;

                if (status == "none")
                    blnHasShow = false;
            }
            //alert(object.style.display);
        }
        function checkAll(objectCheck) {
            var demo = document.getElementById('<%=gridReceiveMsg.ClientID%>');
            var gg = demo.getElementsByTagName('input');
            for (i = 0; i < gg.length; i++) {
                if (gg[i].type == "checkbox") {
                    gg[i].checked = objectCheck.checked;
                }
            }
        }
        function confirmreceiver() {
            event.returnValue = confirm("确认批量指派吗?");
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

    <script type="text/javascript" language="javascript">
        function ShowDetailsInfo(obj, id) {
            $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "frmBr_CustomXmlHttp.aspx?ZHServiceDP=" + id }).responseText; } });
        }
    
    </script>

    <table cellpadding="0" width="98%" class="listContent">
        <tr>
            <td class="listTitleNoAlign" align="right">
                <asp:Button ID="btnReceiver" runat="server" Text="批量指派" OnClientClick="confirmreceiver();"
                    OnClick="btnReceiver_Click" />
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DataGrid ID="gridReceiveMsg" runat="server" Width="100%" OnItemCreated="gridUndoMsg_ItemCreated"
                    OnItemDataBound="gridUndoMsg_ItemDataBound" AutoGenerateColumns="False"  CssClass="Gridtable" >
                    <Columns>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="事件单号">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="Lb_ServiceNo" Text='<%#DataBinder.Eval(Container, "DataItem.ServiceNo")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="ServiceType" HeaderText="事件类别">
                            <HeaderStyle Width="7%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustName" HeaderText="报告人">
                            <HeaderStyle Width="7%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="MastCust" HeaderText="报告单位">
                            <HeaderStyle Width="7%" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ctel" HeaderText="联系电话">
                            <HeaderStyle Width="7%" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="dealstatus" HeaderText="事件状态">
                            <HeaderStyle Width="7%" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Content" HeaderText="详细描述">
                            <HeaderStyle Width="27%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="sendernodename" HeaderText="发送环节">
                            <HeaderStyle Width="7%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="处理" >
                            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "FlowID")))%>'
                                    type="button" value='详情' name="CmdDeal" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="dealstatus"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="endtime"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="FlowID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="messageid"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="nodemodelidnew"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="FlowModelID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Appid"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="status"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
