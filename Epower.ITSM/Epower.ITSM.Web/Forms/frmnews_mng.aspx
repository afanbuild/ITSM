<%@ Page Title="������Ϣά��" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="FrmNews_Mng.aspx.cs" Inherits="Epower.ITSM.Web.Forms.FrmNews_Mng" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--#include file="~/Js/tableSort.js" -->

    <script language="javascript" type="text/javascript">
            function delete_confirm()     //ɾ��ǰִ�нű�
	        {
		       event.returnValue =confirm("ȷ��Ҫɾ����?");
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
                        <asp:BoundColumn DataField="TypeName" HeaderText="��Ϣ���">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="NewsId" DataNavigateUrlFormatString="ShowNews.aspx?NewsID={0}"
                            DataTextField="Title" HeaderText="��Ϣ��Ŀ">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:HyperLinkColumn>
                        <asp:BoundColumn DataField="Writer" HeaderText="������">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="DispFlag" HeaderText="�Ƿ���ʾ">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="PubDate" HeaderText="����ʱ��" DataFormatString="{0:yyyy-MM-dd}">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="OutDate" HeaderText="��ֹʱ��" DataFormatString="{0:yyyy-MM-dd}">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="name" HeaderText="������">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="�޸�">
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" Text="�޸�" SkinID="btnClass1" CommandName="update"
                                    Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="ɾ��">
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btnDelete" runat="server" Text="ɾ��" SkinID="btnClass1" CommandName="Delete"
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
