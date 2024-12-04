<%@ Page Title="��������" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Cst_IssueBatchReceiver.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.Cst_IssueBatchReceiver" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>

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
        
            var len = document.forms[0].elements.length;
            var cbCount = 0;
                for (i = 0; i < len; i++) {
                    if (document.forms[0].elements[i].type == "checkbox") {
                        if (document.forms[0].elements[i].name.indexOf("chkSelect") != -1 &&
						    document.forms[0].elements[i].name.indexOf("gridReceiveMsg") != -1 &&
						    document.forms[0].elements[i].disabled == false) {
						    if(document.forms[0].elements[i].checked==true)
                                cbCount += 1;
                        }
                    }
                }
            if(cbCount<=0)
            {
                alert("��ѡ��Ҫ���յ��¼�����");
                event.returnValue= false;
                return;
            }
        
            event.returnValue = confirm("ȷ������������?");
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
                <asp:Button ID="btnReceiver" runat="server" Text="��������" OnClientClick="confirmreceiver();"
                    OnClick="btnReceiver_Click" />
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DataGrid ID="gridReceiveMsg" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCreated="gridUndoMsg_ItemCreated" OnItemDataBound="gridUndoMsg_ItemDataBound">
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
                        <asp:TemplateColumn HeaderText="�¼�����">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="Lb_ServiceNo" Text='<%#DataBinder.Eval(Container, "DataItem.ServiceNo")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="ServiceType" HeaderText="�¼����">
                            <HeaderStyle Width="7%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustName" HeaderText="������">
                            <HeaderStyle Width="7%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="MastCust" HeaderText="���浥λ">
                            <HeaderStyle Width="7%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ctel" HeaderText="��ϵ�绰">
                            <HeaderStyle Width="7%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="dealstatus" HeaderText="�¼�״̬">
                            <HeaderStyle Width="7%" Wrap="False" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Content" HeaderText="��������">
                            <HeaderStyle Width="17%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="sendernodename" HeaderText="���ͻ���">
                            <HeaderStyle Width="7%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="����">
                            <HeaderStyle Width="5%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "MessageID"))%>'
                                    type="button" value="����" runat="server" name="CmdDeal">
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="dealstatus"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="endtime"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="FlowID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="messageid"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="nodemodelid"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="FlowModelID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Appid"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
