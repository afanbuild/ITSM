<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEqu_PlanDetailMain.aspx.cs" Inherits="Epower.ITSM.Web.Common.frmEqu_PlanDetailMain"
    Title="资产巡检计划" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
        function SetEffcet(obj) {
            document.getElementById(obj.id.replace("chkEffect", "lnkEffect")).click();
        }
        function checkAll(objectCheck) {
            var demo = document.getElementById('<%=dgEA_PlanDetail.ClientID%>');
            var gg = demo.getElementsByTagName('INPUT');
            for (i = 0; i < gg.length; i++) {
                if (gg[i].type == "checkbox" && gg[i].name.indexOf("chkDel") != -1) {
                    gg[i].checked = objectCheck.checked;
                }
            }
        }
    </script>

    <table width='98%' class='listContent Gridtable' cellpadding="2" cellspacing="0">
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                计划名称
            </td>
            <td class='list'>
                <asp:TextBox ID='txtPlanName' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight' style='width: 12%;'>
                运行状态
            </td>
            <td class='list'>
                <asp:RadioButtonList ID="ddltRunState" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0">成功</asp:ListItem>
                    <asp:ListItem Value="1">失败</asp:ListItem>
                    <asp:ListItem Value="2">未执行</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                计划有效性
            </td>
            <td class='list'>
                <asp:RadioButtonList ID="ddltPlanState" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0">有效</asp:ListItem>
                    <asp:ListItem Value="1">无效</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td class='listTitleRight'>
                执行人
            </td>
            <td class='list'>
                <asp:TextBox ID='txtPlanDutyUserName' runat='server'></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0" class="Gridtable">
        <tr>
            <td>
                <asp:DataGrid ID="dgEA_PlanDetail" runat="server" Width="100%" AutoGenerateColumns="False" 
                    CellPadding="1" CellSpacing="2" BorderWidth="0px" OnItemCommand="dgEA_PlanDetail_ItemCommand"
                    OnItemCreated="dgEA_PlanDetail_ItemCreated" OnItemDataBound="dgEA_PlanDetail_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='RunStatus' HeaderText='RunStatus' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='PlanState' HeaderText='PlanState' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='PlanName' HeaderText='计划名称'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='LastTime' HeaderText='最后执行时间' HeaderStyle-Wrap="false">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="下次执行时间" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblNextTime" Text='<%# DataBinder.Eval(Container, "DataItem.NextTime")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='RunStatus' HeaderText='运行状态' HeaderStyle-Wrap="false">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='PlanDutyUserName' HeaderText='执行人' HeaderStyle-Wrap="false">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="计划有效性" HeaderStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkEffect" CommandName="effect" Width="0px"></asp:LinkButton>
                                <asp:CheckBox ID="chkEffect" runat="server" onclick="SetEffcet(this)" />
                            </ItemTemplate>
                            <HeaderStyle Width="9%"></HeaderStyle> 
                        </asp:TemplateColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="修改">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="修改" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="44"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="运行记录">
                            <ItemTemplate>
                                <asp:Button ID="lnkshow" Width="84" SkinID="btnClass1" runat="server" Text="运行记录"
                                    CommandName="show" />
                            </ItemTemplate>
                            <HeaderStyle Width="88"></HeaderStyle>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc2:ControlPageFoot ID="cpfPlanDetail" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
