<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Frm_EsTBLFileslist.aspx.cs" Inherits="Epower.ITSM.Web.FromES_TBl.Frm_EsTBLFileslist" Title="配置项查询" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register src="../Controls/CtrFlowFormText.ascx" tagname="CtrFlowFormText" tagprefix="uc1" %>
<%@ Register src="../Controls/UserPicker.ascx" tagname="UserPicker" tagprefix="uc2" %>
<%@ Register src="../Controls/ctrDateSelectTimeV2.ascx" tagname="ctrDateSelectTime" tagprefix="uc3" %>
<%@ Register src="../Controls/CtrDateAndTimeV2.ascx" tagname="ctrdateandtime" tagprefix="uc4" %>
<%@ Register src="../Controls/CtrEquNature.ascx" tagname="CtrEquNature" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table class="listContent" width="100%" align="center" runat="server" id="Table2">
 <tr>
 <td>
      
 </td>
 </tr>
        <tr>
            <td colspan="2">
            <asp:datagrid id="dgEs_TBL" runat="server" Width="100%" cellpadding="1" 
                    cellspacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  
                    onitemcommand="dgEs_TBL_ItemCommand">
				<AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			    <HeaderStyle CssClass="listTitle"></HeaderStyle>
				<Columns>					
					<asp:BoundColumn DataField='ID' HeaderText='系统编号'></asp:BoundColumn>
					<asp:BoundColumn DataField='tbl_Name' HeaderText='配置名称'></asp:BoundColumn>
					<asp:BoundColumn DataField='FilesName' HeaderText='配置项名称'></asp:BoundColumn>
					<asp:TemplateColumn>
						<ItemTemplate>
						    <asp:Button ID="lnkLook" CssClass="btnClass" runat="server" Text="修改" CommandName="look" />
						</ItemTemplate>
						<HeaderStyle Width="44"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
					</asp:TemplateColumn>
				</Columns>
				</asp:datagrid>  
           
            </td>           
        </tr>   
        <tr>
        <td><uc3:ctrDateSelectTime ID="ctrDateSelectTime2" runat="server" /></td>
        <td>
            <uc4:ctrdateandtime ID="ctrdateandtime2" runat="server" />
            </td>
        </tr>     
           <tr style="display:none">
            <td> 
                <uc4:ctrdateandtime ID="ctrdateandtime1" runat="server" />
            </td>
            <td>
            </td>
            </tr>
        <tr>
        <td>资产属性选择控件<uc5:CtrEquNature ID="CtrEquNature1" runat="server" EquId="10184"/></td><td><uc5:CtrEquNature ID="CtrEquNature2" runat="server" EquId="10184"/></td>
        </tr>
          <tr>
        <td>&nbsp;</td><td></td>
        
        </tr>
          <tr>
        <td>
            </td><td></td>
        
        </tr>
          <tr>
        <td>&nbsp;</td><td></td>
        
        </tr>
          <tr>
        <td>&nbsp;</td><td></td>
        
        </tr>
        
 </table>
</asp:Content>
