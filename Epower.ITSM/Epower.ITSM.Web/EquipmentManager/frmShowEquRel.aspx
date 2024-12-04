<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmShowEquRel.aspx.cs"
    Inherits="Epower.ITSM.Web.EquipmentManager.frmShowEquRel" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>资产关联信息</title>

    <style type="text/css">
            body:nth-of-type(1) .other_fix_width_for_chrome
            {
            	width:100px!important;
            }                        
            
.fixed-grid-border2 
{
	border-right: 1px #A3C9E1 solid;
}

.fixed-grid-border2 td {
	border-left: solid 1px #CEE3F2;
	border-right: 0px;
}

.fixed-grid-border2 tr {
	border-bottom: solid 1px #CEE3F2;
	border-top: solid 1px #CEE3F2;
}            
    </style>
        
    
    <style type="text/css">
        .jqmWindow {
    display: none;
    
    position: fixed;
    top: 17%;
    left: 50%;
    
    margin-left: -300px;
    width: 600px;
    
    background-color: #EEE;
    color: #333;
    border: 1px solid black;
    padding: 12px;
}


    </style>
      
</head>
 <style type="text/css">
        .gridTable
        {
            border: solid 1px #EEEEE0;
        }
        .gridTable th
        {
            border-bottom: solid 1px #EEEEE0;
        }
        .gridTable td
        {
            border-bottom: solid 1px #EEEEE0;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript" language="javascript">
       // 获取资产编号, 以在客户端时使用.
       if (!window.epower) {
           window.epower = {};
       }
       epower.equid = '<%=EquID%>';
   </script>
    <table id="Table1" style="width: 100%;" visible="false" runat="server" class="listContent"
        cellpadding="0">
        <tr>
            <td class="listContent">
                <asp:DataGrid ID="gridUndoMsg" runat="server" Width="100%" CellSpacing="2" CellPadding="1"
                    BorderWidth="0px" BorderColor="White" AutoGenerateColumns="False" CssClass="Gridtable"
                    OnItemCreated="gridUndoMsg_ItemCreated" OnItemDataBound="gridUndoMsg_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Status" HeaderText="Status" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ServiceNo" HeaderText="事件单号">
                            <HeaderStyle Width="150px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ServiceType" HeaderText="事件类别">
                            <HeaderStyle Width="150px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Subject" HeaderText="摘要"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DealStatus" HeaderText="事件状态">
                            <HeaderStyle Width="70px" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="流程状态">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemTemplate>
                                <%#Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 20 ? "<font color='blue'>正在处理</font>" : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 40 ? "<font color='red'>流程暂停</font>" 
                             : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 50 ? "<font color='red'>流程终止</font>" : "<font color='green'>正常结束</font>"))%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="详情">
                            <HeaderStyle Width="5%" />
                            <ItemTemplate>
                                <input id="Button4" runat="server" name="CmdDeal" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                    type="button" value="详情" class="btnClass1">
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
    </table>
    <table id="Table2" style="width: 100%;" visible="false" runat="server" 
        cellpadding="0">
        <tr>
            <td >
                <asp:DataGrid ID="grd" runat="server" Width="100%" CellPadding="1" CellSpacing="2" 
                    BorderWidth="0px" AutoGenerateColumns="False" CssClass="GridTable fixed-grid-border2" OnItemCreated="grd_ItemCreated" 
                    OnItemDataBound="grd_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField='Title' HeaderText='标题'></asp:BoundColumn>
                        <asp:BoundColumn DataField='RegUserName' HeaderText='登记人'></asp:BoundColumn>
                        <asp:BoundColumn DataField='RegDeptName' HeaderText='登记人部门'></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="登记日期">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "RegTime", "{0:d}")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="流程状态">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemTemplate>
                                <%#Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 20 ? "<font color='blue'>正在处理</font>" : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 40 ? "<font color='red'>流程暂停</font>" 
                             : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 50 ? "<font color='red'>流程终止</font>" : "<font color='green'>正常结束</font>"))%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="处理">
                            <HeaderStyle Width="5%" HorizontalAlign="center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                    type="button" value='查看' runat="server">
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <table id="Table3" style="width: 100%;" visible="false" runat="server" class="listContent"
        cellpadding="0">
        <tr>
            <td class="listContent">
                <asp:DataGrid ID="dgChange" runat="server" Width="100%" CellPadding="1" CellSpacing="2"
                    BorderWidth="0px" AutoGenerateColumns="False" CssClass="GridTable fixed-grid-border2" OnItemCreated="dgChange_ItemCreated"
                    OnItemDataBound="dgChange_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn Visible="False">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="ChangeNo" HeaderText="变更单号">
                            <HeaderStyle Width="150px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ChangeTypeName" HeaderText="变更类别">
                            <HeaderStyle Width="150px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Subject" HeaderText="摘要"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DealStatus" HeaderText="变更状态">
                            <HeaderStyle Width="70px" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="流程状态">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemTemplate>
                                <%#Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 20 ? "<font color='blue'>正在处理</font>" : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 40 ? "<font color='red'>流程暂停</font>" 
                             : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 50 ? "<font color='red'>流程终止</font>" : "<font color='green'>正常结束</font>"))%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="详情">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                    type="button" value='详情' runat="server">
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <table id="Table4" style="width: 100%;" visible="false" runat="server" class="listContent"
        cellpadding="0">
        <tr>
            <td align="right" class="list">
                <input id="cmdOpenRelChart" class="btnClass" type="button"
                    value="资产关联图" />
            </td>
        </tr>
        <tr>
            <td align="center" class="listContent" colspan="6">
                <asp:DataGrid ID="dgPro_ProblemAnalyse" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="Gridtable fixed-grid-border2" CellPadding="1" CellSpacing="2" BorderWidth="0px" OnItemCreated="dgPro_ProblemAnalyse_ItemCreated"
                    OnItemDataBound="dgPro_ProblemAnalyse_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <FooterStyle CssClass="listTitle" />
                    <Columns>
                        <asp:BoundColumn DataField='Equ_ID' HeaderText='EquID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='RelID' HeaderText='RelID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Name' HeaderText='Name' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Code' HeaderText='资产编号'></asp:BoundColumn>
                        <asp:HyperLinkColumn DataTextField="Name" HeaderText="关联资产" Target="_blank" DataNavigateUrlField="RelID"
                            DataNavigateUrlFormatString="frmEqu_DeskEdit.aspx?IsSelect=1&FlowID=0&IsRel=0&ID={0}">
                        </asp:HyperLinkColumn>
                        <asp:BoundColumn DataField='RelDescription' HeaderText='关系描述'></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <table id="Table5" style="width: 100%;" visible="false" runat="server" class="listContent"
        cellpadding="0">
        <tr>
            <td align="center" class="listContent" colspan="6">
                <asp:DataGrid ID="dg_EquRelIn" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="Gridtable fixed-grid-border2" CellPadding="1" CellSpacing="2" BorderWidth="0px" OnItemCreated="dg_EquRelIn_ItemCreated"
                    OnItemDataBound="dg_EquRelIn_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <FooterStyle CssClass="listTitle" />
                    <Columns>
                        <asp:BoundColumn DataField='Equ_ID' HeaderText='EquID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='RelID' HeaderText='RelID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Name' HeaderText='Name' Visible="false"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="资产编号">
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="关联资产">
                            <ItemTemplate>
                                <asp:HyperLink ID="hypBtnRelEqu" runat="server"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <%--<asp:HyperLinkColumn  HeaderText="关联资产" Target="_blank" DataNavigateUrlField="Equ_ID" DataNavigateUrlFormatString="frmEqu_DeskEdit.aspx?FlowID=0&IsRel=0&ID={0}"></asp:HyperLinkColumn>--%>
                        <asp:BoundColumn DataField='RelDescription' HeaderText='关系描述'></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <table id="Table6" style="width: 100%;" visible="false" runat="server" 
        cellpadding="0">
        <tr>
            <td align="center" colspan="6">
                <asp:DataGrid ID="dgProblem" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="Gridtable fixed-grid-border2" OnItemDataBound="dgProblem_ItemDataBound">
                    <Columns>
                        <asp:TemplateColumn Visible="False">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="Problem_ID" HeaderText="ID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="State" HeaderText="状态"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Problem_Type" HeaderText="问题类别"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Problem_Level" HeaderText="问题级别"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="RegUserID" HeaderText="登记人"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ServiceNo" HeaderText="问题单号">
                            <HeaderStyle Width="150px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Problem_TypeName" HeaderText="问题类别">
                            <HeaderStyle Width="150px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Problem_Title" HeaderText="摘要"></asp:BoundColumn>
                        <asp:BoundColumn DataField="StateName" HeaderText="问题状态">
                            <HeaderStyle Width="70px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="scale" HeaderText="关联权重" DataFormatString="{0:0.00}"
                            Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField="effect" HeaderText="关联影响度" DataFormatString="{0:0.00}"
                            Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField="stress" HeaderText="关联紧迫性" DataFormatString="{0:0.00}"
                            Visible="false"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="流程状态">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemTemplate>
                                <%#Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 20 ? "<font color='blue'>正在处理</font>" : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 40 ? "<font color='red'>流程暂停</font>" 
                             : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 50 ? "<font color='red'>流程终止</font>" : "<font color='green'>正常结束</font>"))%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="详情">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                    type="button" value='详情' runat="server">
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="status"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <table id="Table7" style="width: 100%;" visible="false" runat="server" class="listContent"
        cellpadding="0">
        <tr>
            <td align="center" class="listContent" colspan="6">
                <asp:DataGrid ID="dgChangeEvent" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="Gridtable fixed-grid-border2" OnItemDataBound="dgChangeEvent_ItemDataBound">
                    <Columns>
                        <asp:TemplateColumn Visible="False">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="ChangeNo" HeaderText="变更单号">
                            <HeaderStyle Width="150px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ChangeTypeName" HeaderText="变更类别">
                            <HeaderStyle Width="150px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Subject" HeaderText="摘要"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DealStatus" HeaderText="变更状态">
                            <HeaderStyle Width="70px" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="流程状态">
                            <HeaderStyle Width="8%"></HeaderStyle>
                            <ItemTemplate>
                                <%#Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 20 ? "<font color='blue'>正在处理</font>" : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 40 ? "<font color='red'>流程暂停</font>" 
                             : (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "status")) == 50 ? "<font color='red'>流程终止</font>" : "<font color='green'>正常结束</font>"))%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="详情">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>'
                                    type="button" value='详情' runat="server">
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <!--html结构: 弹出对话框-->    
<div id="ex3a" class="jqmWindow">
<div class="jqmdTL"><div class="jqmdTR"><div class="jqmdTC jqDrag">
<h3>选择资产关联图视角</h3>
</div></div></div>
<div class="jqmdBL"><div class="jqmdBR"><div class="jqmdBC">

<div class="jqmdMSG">
    <div style="font-size:14px;">        
        <asp:DropDownList Style="padding:5px; height:34px;" ID="ddlYourRelName" runat="server"></asp:DropDownList>                                
    </div>

<input type="button" id="btnConfirm" value="确认" class="btnClass" />
<input type="button" id="btnCancel" value="取消" class="btnClass" />
</div>

</div></div></div>
</div>
    </form>    

</body>
</html>

<script type="text/javascript" src="../Js/jquery-1.7.2.min.js"></script>
<script language="javascript" type="text/javascript" src="../Js/jqModal.js"> </script>
<script type="text/javascript" language="javascript" src="../js/epower.equ.frmshowequrel.js"></script>