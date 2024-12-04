<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEqu_Monitoring_Rule_Resource_Details.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_Monitoring_Rule_Resource_Details"
    Title="资产监控规则配置 - 设置规则" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link type="text/css" href="../css/epower.equ.tabs.css" rel="stylesheet" />
    <link type="text/css" href="../App_Themes/NewOldMainPage/css.css" rel="stylesheet" />
    <link type="text/css" href="../css/epower.equ.resource.mointoring.css" rel="stylesheet" />
    <asp:Literal ID="literal_alert_messages" runat="server" Visible="false">
        <div class="alert" style="margin-top:10px; margin-bottom:10px;">
        抱歉, 没有找到该资源可监控的项目。
        </div>
    </asp:Literal>
    <asp:DataGrid ID="dgRuleList" AutoGenerateColumns="False" runat="server" CssClass="table-layout rule-list"
        CellPadding="4" ForeColor="#333333" GridLines="Both" BorderColor="#CEE3F2" BorderStyle="Solid"
        OnItemDataBound="dgRuleList_ItemDataBound">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditItemStyle BackColor="#999999" />
        <SelectedItemStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <AlternatingItemStyle BackColor="White" ForeColor="#284775" />
        <ItemStyle CssClass="hand" BackColor="White" ForeColor="#333333" BorderStyle="Solid"
            BorderColor="#CEE3F2" />
        <Columns>
            <asp:BoundColumn DataField="Description" HeaderText="描述"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="显示图标">
                <ItemTemplate>
                    <img src="../images/resource_state_{0}.gif" width="25" height="25" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="Priority" HeaderText="优先级"></asp:BoundColumn>
            <asp:BoundColumn DataField="RuleId" HeaderText="编号"></asp:BoundColumn>
            <asp:BoundColumn DataField="Way" HeaderText="逻辑关系"></asp:BoundColumn>
            <asp:BoundColumn DataField="CTIME" ItemStyle-BorderStyle="Solid" ItemStyle-BorderColor="#CEE3F2"
                ItemStyle-Font-Bold="false" HeaderText="创建时间"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="选择">
                <ItemTemplate>
                    <strong class="remove-rule" style="cursor: pointer;" ruleid="{0}">移除</strong>
                    <div style="display: none;">
                        {1}</div>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
        <HeaderStyle BackColor="#EBF5FF" BorderColor="#CEE3F2" BorderStyle="Solid" ForeColor="#08699E"
            Font-Bold="True" />
    </asp:DataGrid>
    <div id="panel_toolbar">
        <div>
            <input id="btn_open_add_rule_panel" type="button" value="添加新规则" />
        </div>
        <div>
            <input id="btn_close_rule_item_view_panel" type="button" value="关闭规则项列表" class="hide" />
        </div>
    </div>
    <!-- 添加规则 -->
    <div id="panel_add_new_rule" style="display: none;">
        <div class="add_rule_head clear">
            <h3>
                为该资源添加新规则</h3>
            <div class="line clear">
            </div>
            <div class="add_rule_head_item" style="margin-top: 10px; margin-bottom: 10px;">
                <div class="add_rule_head_item_left">
                    <label>
                        简单描述这个规则</label>
                </div>
                <div class="add_rule_head_item_right">
                    <textarea id="txt_description" rows="2"></textarea>
                </div>
            </div>
            <div class="add_rule_head_item" style="margin-bottom: 10px;">
                <div class="add_rule_head_item_left">
                    <label>
                        设定它的判断方式</label>
                </div>
                <div class="add_rule_head_item_right">
                    <select id="logic_way">
                        <option value="and">并且</option>
                        <option value="or">或者</option>
                    </select>
                </div>
            </div>
            <div class="add_rule_head_item" style="margin-bottom: 10px;">
                <div class="add_rule_head_item_left">
                    <label>
                        提示图片</label>
                </div>
                <div class="add_rule_head_item_right">
                    <input type="text" id="txt_alert_image" />
                </div>
            </div>
            <div class="add_rule_head_item" style="margin-bottom: 10px;">
                <div class="add_rule_head_item_left">
                    <label>
                        优先级</label>
                </div>
                <div class="add_rule_head_item_right">
                    <input type="text" id="txt_priority" />
                </div>
            </div>
        </div>
        <div class="line clear">
        </div>
        <div class="add_rule_item_list clear">
            <div class="add_rule_item">
                <label>
                    监控项</label>
                <asp:DropDownList ID="ddl_support_key_list" runat="server">
                </asp:DropDownList>
                <label>
                    操作符</label>
                <asp:DropDownList ID="ddl_operator_list" runat="server">
                </asp:DropDownList>
                <label>
                    预设值</label>
                <input type="text" id="txtWarningValue" />
                <input id="btn_add_rule_item" type="button" value="添加" />
            </div>
            <div class="add_rule_list_item border" key="cpu" style="display: none;">
                <label>
                    监控项</label>
                <label>
                    CPU
                </label>
                <label>
                    操作符</label>
                <label>
                    >
                </label>
                <label>
                    预设值</label>
                <label>
                    15
                </label>
                <input id="btn_remove" type="button" value="移除" />
            </div>
        </div>
        <div class="add_rule_foot clear">
            <label class="alert_red">
                注意: 只有点击 "保存规则设置" 按钮，才永久保存该规则。
            </label>
            <input id="btn_save_changes" type="button" value="保存规则设置" />
            <%=DateTime.Now.Ticks.ToString() %>
            <input id="btn_close_add_rule_panel" type="button" value="关掉这个面板" />
        </div>
    </div>
    <!-- 编辑/修改 -->
    <div id="panel_rule_list" class="hide">
        <div class="add_rule_head clear">
            <h3>
                修改规则设置</h3>
            <div class="line clear">
            </div>
            <div class="add_rule_head_item" style="margin-top: 10px; margin-bottom: 10px;">
                <div class="add_rule_head_item_left">
                    <label>
                        简单描述这个规则</label>
                </div>
                <div class="add_rule_head_item_right">
                    <textarea id="txt_description_update" rows="2"></textarea>
                </div>
            </div>
            <div class="add_rule_head_item" style="margin-bottom: 10px;">
                <div class="add_rule_head_item_left">
                    <label>
                        设定它的判断方式</label>
                </div>
                <div class="add_rule_head_item_right">
                    <select id="select_logicway_update">
                        <option value="and">并且</option>
                        <option value="or">或者</option>
                    </select>
                </div>
            </div>
            <div class="add_rule_head_item" style="margin-bottom: 10px;">
                <div class="add_rule_head_item_left">
                    <label>
                        提示图片</label>
                </div>
                <div class="add_rule_head_item_right">
                    <input type="text" id="txt_alert_image_update" />
                </div>
            </div>
            <div class="add_rule_head_item" style="margin-bottom: 10px;">
                <div class="add_rule_head_item_left">
                    <label>
                        优先级</label>
                </div>
                <div class="add_rule_head_item_right">
                    <input type="text" id="txt_priority_update" />
                </div>
            </div>
        </div>
        <div class="line clear">
        </div>
        <div class="panel_rule_list_item">
            <div class="rule_item_template hide border" key="cpu">
                <label>
                    监控项</label>
                <label>
                    CPU
                </label>
                <label>
                    操作符</label>
                <label>
                    >a
                </label>
                <label>
                    预设值</label>
                <label>
                    15
                </label>
                <input class="panel_rule_list_item_btn_edit" type="button" value="编辑" />
                <input class="panel_rule_list_item_btn_remove" type="button" value="移除" />
            </div>
        </div>
        <div class="panel_rule_list_foot">
            <label class="alert_red">
                注意: 只有点击 "保存规则设置" 按钮，才永久保存该规则。
            </label>
            <input id="panel_rule_list_btn_savechanges" type="button" value="保存规则设置" />
        </div>
    </div>

    <script type="text/javascript" language="javascript" src="../js/epower.equ.resource.mointoring.js"></script>

</asp:Content>