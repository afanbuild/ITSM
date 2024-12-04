<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CST_IssueListLog.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.CST_IssueListLog" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList" TagPrefix="uc6" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc5" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>
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
        $.ajaxSetup({ cache: false });
        function ShowDetailsInfo(obj, id) {
            $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "frmBr_CustomXmlHttp.aspx?ZHServiceDP=" + id }).responseText; } });
        }
        
 
    function checkAll(objectCheck) 
    {

        var demo = document.getElementById('<%=gridUndoMsg.ClientID%>');
        var gg = demo.getElementsByTagName('INPUT');
        for (i = 0; i < gg.length; i++) 
        {
            if (gg[i].type == "checkbox") {
                gg[i].checked = objectCheck.checked;
            }
        }
    }
    
    </script>
<table class="listContent" width="100%" id="Table1">
                    <tr>
                        <td nowrap align="left" class="listTitle">
                            用户信息
                        </td>
                        <td align="left" class="list">
                            <asp:TextBox ID="txtCustInfo" runat="server"></asp:TextBox>
                        </td>
                        <td nowrap align="left" class="listTitle">
                            事件状态
                        </td>
                        <td align="left" class="list">
                            <uc6:ctrFlowCataDropList ID="CtrFCDDealStatus" runat="server" RootID="1017" />
                        </td>
                        <td nowrap align="left" class="listTitle">
                            流程状态
                        </td>
                        <td align="left" class="list">
                            <asp:DropDownList ID="cboStatus" runat="server" Width="120px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap align="left" class="listTitle">
                            发生时间
                        </td>
                        <td colspan="5" nowrap align="left" class="list">
                            <asp:TextBox ID="txtMsgDateBegin" runat="server" Width="108px"></asp:TextBox>
                            <img id="imgSBegin" runat="server" src="../Controls/Calendar/calendar.gif" style="cursor: hand" />
                            ~<asp:TextBox ID="txtMsgDateEnd" runat="server" Width="108px"></asp:TextBox>
                            <img id="imgEEnd" runat="server" src="../Controls/Calendar/calendar.gif" style="cursor: hand" />
                            <asp:RegularExpressionValidator ID="Regularexpressionvalidator2" runat="server" ErrorMessage="格式错误"
                                Display="Static" SetFocusOnError="true" ControlToValidate="txtMsgDateBegin" ValidationExpression="^\s*((\d{4})|(\d{2}))([-/]|\. ?)(\d{1,2})\4(\d{1,2})\s*$">*</asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator Display="None" ID="Regularexpressionvalidator3" runat="server"
                                ControlToValidate="txtMsgDateEnd" ErrorMessage="格式错误" ValidationExpression="^\s*((\d{4})|(\d{2}))([-/]|\. ?)(\d{1,2})\4(\d{1,2})\s*$">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
 
 <table width="100%" align="center" id="tblEmail" runat="server" class="listContent"
        visible="false">
        <tr>
            <td class="listTitleNoAlign" align="center">
                <asp:Button ID="btnSendMail" runat="server" Text="批量邮件回访" CssClass="btnClass" OnClick="btnSendMail_Click1" />
            </td>
        </tr>
    </table>
 <table width="100%" align="center" class="listContent" cellpadding="0">
        <tr>
            <td class="listContent">
                <asp:DataGrid ID="gridUndoMsg" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"    OnItemCreated="gridUndoMsg_ItemCreated"
                    OnItemDataBound="gridUndoMsg_ItemDataBound">
                    <Columns>  
                        <asp:TemplateColumn Visible="false">                        
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                        </HeaderTemplate>
						<ItemTemplate>
							<asp:CheckBox id="chkDel" runat="server"></asp:CheckBox>
						</ItemTemplate>
						<HeaderStyle Width="5%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
				        </asp:TemplateColumn>                   
                        <asp:TemplateColumn HeaderText="事件单号">                        
                        <ItemTemplate>
                            <asp:Label runat="server" id="Lb_ServiceNo" Text='<%#DataBinder.Eval(Container, "DataItem.ServiceNo")%>'></asp:Label>              
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:TemplateColumn>                        
                        <asp:BoundColumn DataField="CustName" HeaderText="客户名称">
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustAddress" HeaderText="客户地址">
                            <HeaderStyle Width="15%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ctel" HeaderText="联系电话">
                            <HeaderStyle Width="8%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustTime" HeaderText="发生时间" DataFormatString="{0:g}">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EquipmentSN" HeaderText="PN/SN">
                            <HeaderStyle Width="8%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="dealstatus" HeaderText="处理状态">
                            <HeaderStyle Width="5%" Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Content" HeaderText="详细描述">
                            <HeaderStyle Width="20%"></HeaderStyle>
                        </asp:BoundColumn>   
                         <asp:TemplateColumn HeaderText="处理">
                            <HeaderStyle  HorizontalAlign="Center"></HeaderStyle>                       
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                    type="button" value='<%#GetButtonValue((int)DataBinder.Eval(Container.DataItem, "status"))%>'
                                    name="CmdDeal" runat="server" />
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
        <tr>
            <td class="listTitle" align="right">
                <uc5:ControlPageFoot ID="cpCST_Issue" runat="server" />
            </td>
        </tr>  
    </table>
</asp:Content>
