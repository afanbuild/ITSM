<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmEqu_RelNamesMain.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_RelNamesMain" 
Title="资产关联视角" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
    #dgRelKey { margin-top:15px; }
</style>
<link type="text/css" href="../css/epower.equ.tabs.css" rel="stylesheet" />
<link type="text/css" href="../App_Themes/NewOldMainPage/css.css" rel="stylesheet" />

    <asp:DataGrid ID="dgRelKey" AutoGenerateColumns="False" runat="server"  CssClass="table-layout"
        CellPadding="4" ForeColor="#333333" GridLines="Both"  BorderColor="#CEE3F2" BorderStyle="Solid"
        onitemdatabound="dgRelKey_ItemDataBound">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditItemStyle BackColor="#999999" />
                    <SelectedItemStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <AlternatingItemStyle BackColor="White" ForeColor="#284775" />
                    <ItemStyle BackColor="White" ForeColor="#333333" BorderStyle="Solid" BorderColor="#CEE3F2" />
    <Columns>
        <asp:BoundColumn DataField="ID" HeaderText="视角编号"></asp:BoundColumn>
        <asp:BoundColumn DataField="RelKey" ItemStyle-Font-Bold="false" HeaderText="视角名称"></asp:BoundColumn>                                
        <asp:TemplateColumn HeaderText="选择">
            <ItemTemplate>
                <a href="frmEqu_RelNamesMain.aspx?action=delete&relkeyid={0}">删除</a>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <a href="frmEqu_RelNamesMain.aspx?action=update&relkeyid={0}">修改</a>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>                
    <HeaderStyle BackColor="#EBF5FF" BorderColor="#CEE3F2" BorderStyle="Solid" ForeColor="#08699E" Font-Bold="True"  />
    </asp:DataGrid>
    
    <asp:Button ID="btnAddNew" runat="server" Text="新增" CssClass="btnClass" onclick="btnAddNew_Click" />
    <asp:Panel ID="panel" runat="server" Visible="false">
        <div class="div-tr">
        <label class="alert">请输入要添加或修改的视角名称！</label>
        </div>
        <div class="div-tr">
        <asp:TextBox ID="txtRelKey" runat="server" CssClass="big-input" MaxLength="25" ></asp:TextBox>
        <asp:HiddenField ID="hfRelKeyId" runat="server" />
        </div>
        <div class="div-tr">
        <asp:Button ID="btnAddNews" Text="保存" CssClass="btnClass" runat="server" 
            onclick="btnAddNews_Click" />
            &nbsp;&nbsp;
         <asp:Button ID="btnCancel" Text="取消" CssClass="btnclass" runat="server" 
            onclick="btnCancel_Click" />
        </div>
    </asp:Panel>
    
    <a href="frmEqu_RelNamesPrefer.aspx" style="border-bottom:1px solid black">设置用户使用偏好</a>
    <hr />
</asp:Content>
