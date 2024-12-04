<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmRegEventMain.aspx.cs" Inherits="Epower.ITSM.Web.NewMainPage.frmRegEventMain"
    Title="我登记事项" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="uc1" TagName="ControlPageFoot" Src="../Controls/ControlPageFoot.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgRegEvent" CssClass="Gridtable" runat="server" Width="100%" OnItemDataBound="dgRegEvent_ItemDataBound">                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="AppID" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FlowID" Visible="False"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="主题">
                            <ItemTemplate>
                                <asp:Label ID="lblSubject" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "subject") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="name" HeaderText="流程名称">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="starttime" DataFormatString="{0:yyyy-MM-dd H:mm}" HeaderText="登记时间">
                            <HeaderStyle Width="15%" HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="查看" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <input id="Button3" class="btnClass1" onclick='<%#GetUrl(Convert.ToInt64(DataBinder.Eval(Container.DataItem, "Flowid")),Convert.ToInt64(DataBinder.Eval(Container.DataItem, "AppID")))%>'
                                    type="button" value="详情" runat="server" name="CmdDeal">
                            </ItemTemplate>
                            <HeaderStyle Width="44" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>                        
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc1:ControlPageFoot ID="cpRegEvent" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
