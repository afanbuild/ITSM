<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_remote_message_manager.aspx.cs" Inherits="Epower.ITSM.Web.Forms.frm_remote_message_manager"
    Title="手机远程消息服务端" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        #dgRelKey
        {
            margin-top: 15px;
        }
    </style>
    <link type="text/css" href="../css/epower.equ.tabs.css" rel="stylesheet" />
    <link type="text/css" href="../App_Themes/NewOldMainPage/css.css" rel="stylesheet" />
    <asp:DataGrid ID="dgRemoteMessageList" AutoGenerateColumns="False" runat="server" CssClass="table-layout"
        CellPadding="4" ForeColor="#333333" GridLines="Both" BorderColor="#CEE3F2" BorderStyle="Solid"
        OnItemDataBound="dgRelKey_ItemDataBound">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditItemStyle BackColor="#999999" />
        <SelectedItemStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <AlternatingItemStyle BackColor="White" ForeColor="#284775" />
        <ItemStyle BackColor="White" ForeColor="#333333" BorderStyle="Solid" BorderColor="#CEE3F2" />
        <Columns>
            <asp:BoundColumn DataField="id" HeaderText="编号"></asp:BoundColumn>
            <asp:BoundColumn DataField="type" ItemStyle-BorderStyle="Solid" ItemStyle-BorderColor="#CEE3F2"
                ItemStyle-Font-Bold="false" HeaderText="类型"></asp:BoundColumn>
            <asp:BoundColumn DataField="content" ItemStyle-BorderStyle="Solid" ItemStyle-BorderColor="#CEE3F2"
                ItemStyle-Font-Bold="false" HeaderText="内容"></asp:BoundColumn>
            <asp:BoundColumn DataField="pub_date" ItemStyle-BorderStyle="Solid" ItemStyle-BorderColor="#CEE3F2"
                ItemStyle-Font-Bold="false" HeaderText="发布日期"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="选择">
                <ItemTemplate>
                    <a href="frmEqu_RelNamesMain.aspx?action=delete&relkeyid={0}">删除</a> &nbsp;&nbsp;&nbsp;&nbsp;
                    <a href="frmEqu_RelNamesMain.aspx?action=update&relkeyid={0}">修改</a>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
        <HeaderStyle BackColor="#EBF5FF" BorderColor="#CEE3F2" BorderStyle="Solid" ForeColor="#08699E"
            Font-Bold="True" />
    </asp:DataGrid>
</asp:Content>
