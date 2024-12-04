<%@ Page Language="c#" Inherits="Epower.ITSM.Web.Forms.form_FlowModel_Set" CodeBehind="form_FlowModel_Set.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>常用流程设置</title>
</head>
<body>
    <form method="post" runat="server">
    <table id="Table1" width="80%" align="center" class="listContent">
        <tr>
            <td align="center" class="list">
                <br />
                <p>
                    <uc1:CtrTitle ID="CtrTitle1" runat="server"></uc1:CtrTitle>
                </p>
                <br />
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" width="80%" border="0" align="center">
        <tr>
            <td align="center" class="list">
                <asp:Button ID="btnOK" CssClass="btnClass" runat="server" Text="确定" OnClick="btnOK_Click">
                </asp:Button>
            </td>
        </tr>
        <tr>
            <td align="center" class="listContent">
                <asp:DataGrid ID="grdFlowModel" runat="server" Width="100%" PageSize="16" AutoGenerateColumns="False"  CssClass="Gridtable" >
                    <Columns>
                        <asp:TemplateColumn HeaderText="常用" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%#GetCheckedValue(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "SetStatus")))%>'>
                                </asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="FlowName" ReadOnly="True" HeaderText="流程名称">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Remark" ReadOnly="True" HeaderText="备注"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AppName" ReadOnly="True" HeaderText="应用名称" ItemStyle-HorizontalAlign="center">
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="FlowModelID"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle NextPageText="下一页" PrevPageText="上一页"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
