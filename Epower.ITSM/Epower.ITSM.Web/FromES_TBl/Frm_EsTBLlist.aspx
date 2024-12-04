<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Frm_EsTBLlist.aspx.cs" Inherits="Epower.ITSM.Web.FromES_TBl.Frm_EsTBLlist" Title="配置查询" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register src="../Controls/CtrFlowFormText.ascx" tagname="CtrFlowFormText" tagprefix="uc1" %>
<%@ Register src="../Controls/UserPicker.ascx" tagname="UserPicker" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table class="listContent" width="100%" align="center" runat="server" id="Table2">
        <tr>
            <td>
            <asp:datagrid id="dgEs_TBL" runat="server" Width="100%" cellpadding="1" 
                    cellspacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  
                    onitemcommand="dgEs_TBL_ItemCommand">
				<AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			    <HeaderStyle CssClass="listTitle"></HeaderStyle>
				<Columns>					
					<asp:BoundColumn DataField='ID' HeaderText='系统编号'></asp:BoundColumn>
					<asp:BoundColumn DataField='tbl_Name' HeaderText='配置名称'></asp:BoundColumn>
					<asp:TemplateColumn>
						<ItemTemplate>
						    <asp:Button ID="lnkLook" CssClass="btnClass" runat="server" Text="详情" CommandName="look" />
						</ItemTemplate>
						<HeaderStyle Width="44"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
					</asp:TemplateColumn>
				</Columns>
				</asp:datagrid>  
           
            </td>           
        </tr>        
        <tr>
            <td>
            
                <uc2:UserPicker ID="UserPicker1" runat="server" DeptId="10013" />
            
            </td>
        </tr>
        
 </table>
</asp:Content>
