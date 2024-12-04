<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmUserIsDel.aspx.cs" Inherits="Epower.ITSM.Web.DeptForms.frmUserIsDel"
    Title="已删除用户查询" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="98%" class="listContent">
        <tr>
            <td class="listTitleRight" width="12%">
                <asp:Label ID="lblLoginName" runat="server" Text="登录用户名称"></asp:Label>
            </td>
            <td class="list">
                <asp:TextBox ID="txtLoginName" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgUserIsDel" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    GridLines="Horizontal" PageSize="50" OnItemCommand="dgUserIsDel_ItemCommand"
                    OnItemDataBound="dgUserIsDel_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="UserId"></asp:BoundColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle Width="12px"></HeaderStyle>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnStatus" runat="server" ImageUrl="../Images/PERSON.GIF">
                                </asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="用户名称">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle Wrap="False"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "name")%>
                            </ItemTemplate>
                            <FooterStyle Wrap="False"></FooterStyle>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="登录用户名称">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle Wrap="False"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "LoginName")%>
                            </ItemTemplate>
                            <FooterStyle Wrap="False"></FooterStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="FullDeptName" HeaderText="所属部门">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="job" HeaderText="职位">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TelNo" HeaderText="电话">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Mobile" HeaderText="手机号码">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="QQ" HeaderText="QQ号码">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EduLevel" HeaderText="学历">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="groupname" HeaderText="角色">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="email" HeaderText="电子邮件">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="还原用户" HeaderStyle-HorizontalAlign="center">
                            <HeaderStyle HorizontalAlign="Center" Width="90px" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Button ID="btnBackUser" runat="server" Text="还原用户" CommandName="Backuser"
                                    SkinID="btnClass1" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="deleted"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
