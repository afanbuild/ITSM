<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmFlow_QuestHouseQuery.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmFlow_QuestHouseQuery"
    Title="进入操作间查询" %>

<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc5" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../Controls/Calendar/Popup.js"></script>

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>
    <!--Begin: 引入基础脚本库-->
    <script type="text/javascript" language="javascript" src="../js/epower.base.js"></script>
    <!--End: 引入基础脚本库-->
    
    <script language="javascript" type="text/javascript">
        $.ajaxSetup({ cache: false });
        function OpenDeleteFlow(obj)  //删除流程
        {
            var FlowID = document.getElementById(obj.id.replace("btnDelete", "hidDelete")).value;
            var url="../Common/frmFlowDelete.aspx?FlowID=" + FlowID+"&Opener_hiddenDelete=<%=hidd_btnDelete.ClientID %>&TypeFrm=frmFlow_QuestHouseQuery";

            var _xy = epower.tools.computeXY('c', window, 320, 230);                
            
            window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=320px,height=230px,left=" + _xy.x + ",top=" + _xy.y);
          
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

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        $.ajaxSetup({ cache: false });
        function ShowDetailsInfo(obj, id) {
            $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "../AppForms/frmBr_CustomXmlHttp.aspx?V_EquChange=" + id }).responseText; } });
        }
    
    </script>

    <table width='98%' class="listContent">
        <tr>
            <td class="listTitleRight" width="12%">
                <asp:Literal ID="Lititilno" runat="server" Text="ITIL号"></asp:Literal>
            </td>
            <td class="list" width="35%">
                <asp:TextBox ID="txtCustInfo" runat="server"></asp:TextBox>
            </td>
            <td class="listTitleRight" width="12%">
                流程状态
            </td>
            <td class="list">
                <asp:DropDownList ID="cboStatus" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left" class="listTitleRight">
                处理状态
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlStatus" runat="server" Width="152px">
                    <asp:ListItem Text="" Value="0"></asp:ListItem>
                    <asp:ListItem Text="新建" Value="10001"></asp:ListItem>
                    <asp:ListItem Text="申请" Value="10002"></asp:ListItem>
                    <asp:ListItem Text="室主管审批" Value="10006"></asp:ListItem>
                    <asp:ListItem Text="操作室审批" Value="10003"></asp:ListItem>
                    <asp:ListItem Text="网络室审批" Value="10004"></asp:ListItem>
                    <asp:ListItem Text="关闭" Value="10005"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="litChangeCustTime1" runat="server" Text="申请时间"></asp:Literal>
            </td>
            <td class="list">
                <uc5:ctrDateSelectTime ID="ctrDateTime" runat="server" />
            </td>
        </tr>
    </table>
    <input id="hidTable" runat="server" type="hidden" /><br />
    <table cellpadding="0" width="98%" class="listContent">
        <tr>
            <td class="listContent">
                <asp:DataGrid ID="dgProblem" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCreated="dgProblem_ItemCreated" OnDeleteCommand="dgProblem_DeleteCommand"
                    OnItemDataBound="dgProblem_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitleRight"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn Visible="False">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%" HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="houseId" HeaderText="houseId"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="ITIL号">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="Lb_Subject" Text='<%#DataBinder.Eval(Container, "DataItem.ITILNO")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="execByNo" HeaderText="申请人工号">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="execByName" HeaderText="申请人姓名">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="execByDeptName" HeaderText="申请人部门（室）">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ComeInDate" HeaderText="进入时间" DataFormatString="{0:g}">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="outDate" HeaderText="离开时间" DataFormatString="{0:g}">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="StatusName" HeaderText="处理状态">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="流程状态">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemTemplate>
                                <%#Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 20 ? "<font color='blue'>正在处理</font>" : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 40 ? "<font color='red'>流程暂停</font>" 
                             : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 50 ? "<font color='red'>流程终止</font>" : "<font color='green'>正常结束</font>"))%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="处理">
                            <HeaderStyle Width="44" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "FlowID")))%>'
                                    type="button" value='详情' runat="server">
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="status"></asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="删除" HeaderStyle-HorizontalAlign="center">
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btnDelete" runat="server" Text="删除" CommandName="Delete" SkinID="btnClass1"
                                    OnClientClick="OpenDeleteFlow(this);" />
                                <input id="hidDelete" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "flowid")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <asp:Button ID="hidd_btnDelete" runat="server" style="display:none;"   Text="删除时重新调用数据" onclick="hidd_btnDelete_Click"  />
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitleRight">
                <uc3:ControlPageFoot ID="cpChange" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
