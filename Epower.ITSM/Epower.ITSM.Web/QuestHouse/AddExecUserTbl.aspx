<%@ Page Title="申请用户名密码" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="AddExecUserTbl.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.AddExecUserTbl" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
//检查是否选择数据
function CheckSelected() { 

    var listchk=$("#<%=ExecUserGrid.ClientID %> input:checked");
    if(listchk.size() ==0)
    {
         alert("请选择删除数据！");
         return false;
    }

}
function btnClose()
{
   window.opener.document.getElementById("<%=Opener_ClientId %>BtnHidden").click();
    window.close();
}
    </script>

    <table class="listContent" width="100%" align="center">
        <tr>
            <td style="width: 12%" class="listTitleRight" nowrap="nowrap">
                ip地址
            </td>
            <td class="list" style="width: 35%">
                <asp:TextBox ID="IPurl" runat="server"></asp:TextBox>
            </td>
            <td style="width: 12%" class="listTitleRight" nowrap="nowrap">
                用户号
            </td>
            <td class="list">
                <asp:TextBox ID="userNO" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="list" colspan="4" style="text-align: center">
                <asp:Button ID="ButSave" runat="server" Text="确认保存" OnClick="ButSave_Click" />
                <input type="button" value="退出" onclick="btnClose();" class="btnClass" />
            </td>
        </tr>
    </table>
    <table class="listContent" width="100%" align="center">
        <tr>
            <td class="list">
                <asp:DataGrid ID="ExecUserGrid" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCommand="dgProblem_ItemCommand">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn HeaderText="序号">
                            <ItemTemplate>
                                <%# Container.ItemIndex+1%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="true" DataField="IP" HeaderText="IP" ItemStyle-Width="80px">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="username" HeaderText="用户名"></asp:BoundColumn>
                        <asp:BoundColumn DataField="execId" Visible="false"></asp:BoundColumn>
                        <asp:TemplateColumn Visible="true">
                            <HeaderTemplate>
                                <asp:Button ID="BtnAdd" runat="server" Text="删除" CommandName="Del" OnClientClick="return CheckSelected();" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="4%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
