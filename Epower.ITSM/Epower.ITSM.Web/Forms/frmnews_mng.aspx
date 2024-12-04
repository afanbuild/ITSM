<%@ Page Title="公告信息维护" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="FrmNews_Mng.aspx.cs" Inherits="Epower.ITSM.Web.Forms.FrmNews_Mng" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--#include file="~/Js/tableSort.js" -->

    <script language="javascript" type="text/javascript">
            function delete_confirm()     //删除前执行脚本
	        {
		       event.returnValue =confirm("确认要删除吗?");
	        }
    </script>

    <table cellspacing="0" cellpadding="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="DgNews" runat="server" GridLines="Horizontal" BorderStyle="None"
                    AutoGenerateColumns="False"  CssClass="Gridtable"  Width="100%" OnItemCreated="DgNews_ItemCreated">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="NewsId"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TypeName" HeaderText="信息类别">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="NewsId" DataNavigateUrlFormatString="ShowNews.aspx?NewsID={0}"
                            DataTextField="Title" HeaderText="信息题目">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:HyperLinkColumn>
                        <asp:BoundColumn DataField="Writer" HeaderText="发布人">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="DispFlag" HeaderText="是否显示">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="PubDate" HeaderText="发布时间" DataFormatString="{0:yyyy-MM-dd}">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="OutDate" HeaderText="截止时间" DataFormatString="{0:yyyy-MM-dd}">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="name" HeaderText="创建人">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="修改">
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" Text="修改" SkinID="btnClass1" CommandName="update"
                                    Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="删除">
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btnDelete" runat="server" Text="删除" SkinID="btnClass1" CommandName="Delete"
                                    OnClientClick="delete_confirm();" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="TypeID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="OutDate" Visible="false" DataFormatString="{0:yyyy-MM-dd}">
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="InputUser"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#000066" BackColor="#E7E7FF"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc1:ControlPage ID="ControlPageNewsInfo" runat="server"></uc1:ControlPage>
            </td>
        </tr>
    </table>
</asp:Content>
