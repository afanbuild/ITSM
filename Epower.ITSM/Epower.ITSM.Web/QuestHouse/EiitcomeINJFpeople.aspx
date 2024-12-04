<%@ Page Title="行外人员" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="EiitcomeINJFpeople.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.EiitcomeINJFpeople" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <script language="javascript" type="text/javascript">
//检查是否选择数据
function CheckSelected() { 
   // debugger;
   //zxl===
    var listchk=$("#<%=comperGrid.ClientID %> input:checked");
    if(listchk.size() ==0)
    {
         alert("请选择删除数据！");
         return false;
    }
}
function btnClose()
{
   window.opener.document.getElementById("<%=Opener_ClientId %>hiddButton").click();
    window.close();
}
</script>
    
    <table class="listContent" width="100%" align="center">
        <tr>
            <td style="width: 12%" class="listTitleRight" nowrap="nowrap">
                姓名
            </td>
            <td class="list" style="width: 35%">
                <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
            </td>
            <td style="width:12%" class="listTitleRight" nowrap="nowrap">
                证件号码
            </td>
            <td class="list">
                <asp:TextBox ID="CardNO" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" nowrap="nowrap">
                电话
            </td>
            <td class="list">
                <asp:TextBox ID="phone" runat="server"></asp:TextBox>
            </td>
            <td class="listTitleRight" nowrap="nowrap">
                公司名称
            </td>
            <td class="list">
                <asp:TextBox ID="compurName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="list" colspan="4" style="text-align: center">
                <asp:Button ID="ButSave" runat="server" Text="确认保存" OnClick="ButSave_Click" />
                <input  type="button" value="退出" onclick="btnClose();" class="btnClass" />
            </td>
        </tr>
    </table>
    <table class="listContent" width="100%" align="center">
        <tr>
            <td class="list" style="width: *" colspan="4">
                <asp:DataGrid ID="comperGrid" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCommand="comperGrid_ItemCommand">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitleRight"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn HeaderText="序号">
                            <ItemTemplate>
                                <%# Container.ItemIndex+1%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="true" DataField="peopleName" HeaderText="姓名"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CardNO" HeaderText="证件号码"></asp:BoundColumn>
                        <asp:BoundColumn DataField="PeoplePhone" HeaderText="电话"></asp:BoundColumn>
                        <asp:BoundColumn DataField="computeName" HeaderText="公司名称"></asp:BoundColumn>
                        <asp:BoundColumn DataField="COMEID" Visible="false"></asp:BoundColumn>
                        <asp:TemplateColumn Visible="true">
                            <HeaderTemplate>
                                <asp:Button ID="DeleteComp" runat="server" Text="删除" CommandName="Del" OnClientClick="return CheckSelected();" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" name="checkbox" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="8%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
