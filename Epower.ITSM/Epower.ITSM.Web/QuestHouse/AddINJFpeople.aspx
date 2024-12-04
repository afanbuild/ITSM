<%@ Page Title="其他申请人进入人" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="AddINJFpeople.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.AddINJFpeople" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
        function SelectStaff() {
            var url="Allfrmpopstaff.aspx?TypeFrm=AddINJFpeople&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
            window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=501,height=516,left=150,top=50");
        }
        //检查是否选择数据
        function CheckSelected() { 
             var listchk=$("#<%=dgProblem.ClientID %> input:checked");
            if(listchk.size() ==0)
            {
                 alert("请选择删除数据！");
                 return false;
            }
        }
        function btnColse()
        {
            window.opener.document.getElementById("<%=Opener_ClientId %>btnHidd").click();
            window.close();
        }
    </script>

    <table class="listContent" width="100%" align="center">
        <tr>
            <td class="listTitleRight" style="width: 12%;">
                选择人员
            </td>
            <td class="list" style="width: *">
                <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
                <input type="hidden" id="HidPeople" runat="server"><asp:Label ID="Lb_people" runat="server"
                    Text=""></asp:Label><asp:Button ID="Button1" runat="server" Text="选择" OnClientClick="SelectStaff()" />
            </td>
        </tr>
        <tr>
            <td class="list" style="width: *; text-align: center" colspan="2">
                <input type="hidden" id="UserValues" runat="server" />
                <asp:Button ID="btn_server" runat="server" Text="确认保存" OnClick="btn_server_Click" /><input
                    type="button" value="退出" onclick="btnColse();" class="btnClass" />
            </td>
        </tr>
    </table>
    <table class="listContent" width="100%" align="center">
        <tr>
            <td class="list" style="width: *" colspan="4">
                <asp:DataGrid ID="dgProblem" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
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
                        <asp:BoundColumn Visible="true" DataField="username" HeaderText="申请人姓名"></asp:BoundColumn>
                        <asp:BoundColumn DataField="logName" HeaderText="申请人工号"></asp:BoundColumn>
                        <asp:BoundColumn DataField="deptName" HeaderText="申请人部门（室）"></asp:BoundColumn>
                        <asp:BoundColumn DataField="INJFpeopleID" Visible="false"></asp:BoundColumn>
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
