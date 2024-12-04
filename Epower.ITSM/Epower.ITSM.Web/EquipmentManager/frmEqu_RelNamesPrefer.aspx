<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEqu_RelNamesPrefer.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_RelNamesPrefer"
    Title="无标题页" %>
<%@ Register TagPrefix="uc1" TagName="CtrDeptTree" Src="~/Controls/UserPicker.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<link type="text/css" href="../css/epower.equ.tabs.css" rel="stylesheet" />
<link type="text/css" href="../App_Themes/NewOldMainPage/css.css" rel="stylesheet" />    
    <div class="div-tr">
    <label style="font-weight:bold;">选择人员:</label>
    <uc1:CtrDeptTree ID="deptTree" runat="server" />
    <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="btnClass" onclick="btnSearch_Click" />
    </div>
    <hr />
    <asp:DataGrid ID="dgPreferList" runat="server" AutoGenerateColumns="False"  CssClass="table-layout"
        CellPadding="4" ForeColor="#333333" GridLines="Both"  BorderColor="#CEE3F2" BorderStyle="Solid">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditItemStyle BackColor="#999999" />
                    <SelectedItemStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <AlternatingItemStyle BackColor="White" ForeColor="#284775" />
                    <ItemStyle BackColor="White" ForeColor="#333333" BorderStyle="Solid" BorderColor="#CEE3F2" />
        <Columns>
            <asp:BoundColumn DataField="ID" HeaderText="视角编号"></asp:BoundColumn>
            <asp:BoundColumn DataField="RelKey" HeaderText="视角名称"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="选择">
                <ItemTemplate>
                    <asp:CheckBox ID="chkprefer" runat="server" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
        <HeaderStyle BackColor="#EBF5FF" BorderColor="#CEE3F2" BorderStyle="Solid" ForeColor="#08699E" Font-Bold="True"  />
    </asp:DataGrid>        
    
    <asp:Label ID="literalAlert" runat="server" Font-Bold="true" ForeColor="Red" Font-Size="14px"></asp:Label>
    
    <asp:HiddenField ID="hfUserId" runat="server" />
    <asp:Button ID="btnSaveChanges" runat="server" Text="保存更改"  CssClass="btnClass"
        onclick="btnSaveChanges_Click" />       
        &nbsp; &nbsp;
    <asp:Button ID="btnReturn" runat="server" Text="返回" CssClass="btnclass" 
        onclick="btnReturn_Click" />
        <hr />
</asp:Content>
