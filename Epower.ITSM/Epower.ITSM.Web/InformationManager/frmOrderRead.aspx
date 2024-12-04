<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmOrderRead.aspx.cs" Inherits="Epower.ITSM.Web.InformationManager.frmOrderRead" Title="无标题页" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register TagPrefix="uc2" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table  cellpadding='1' cellspacing='2' width="98%" align="center" border="0">
	<TR>
		<TD class="Title" align="center">
			<font size="5"><uc2:CtrTitle id="CtrTitle1" runat="server" Title="知识阅读排名榜" ></uc2:CtrTitle></font>
		</TD>
	</TR>
</table>
<table cellpadding="0" cellspacing="0" width="98%" border="0" >
	<tr>
		<td>
			<asp:datagrid id="dgInf_Information" runat="server" Width="100%" Style="word-break:break-all" 
                cellpadding="1" cellspacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  
                OnItemCreated="dgInf_Information_ItemCreated" 
                OnItemDataBound="dgInf_Information_ItemDataBound">
				<AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			     <HeaderStyle CssClass="listTitle" Font-Bold=true Font-Size=X-Small />
				<Columns>
				    <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="False"></asp:BoundColumn>
				    <asp:TemplateColumn HeaderText="名次" HeaderStyle-HorizontalAlign="Left">
						<ItemTemplate>
						    <asp:Label ID="lnklink" runat="server" Text='<%#Container.ItemIndex+1%>'/>
						</ItemTemplate>
						<HeaderStyle Width="5%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left" />
					</asp:TemplateColumn>
                    <asp:HyperLinkColumn  DataTextField="Title" HeaderStyle-HorizontalAlign="center" HeaderText="主题" Target="_blank" DataNavigateUrlField="ID" DataNavigateUrlFormatString="frmKBShow.aspx?KBID={0}">
                       <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkColumn>
					<asp:BoundColumn DataField='PKey' HeaderText='关键字' HeaderStyle-HorizontalAlign="center"><ItemStyle HorizontalAlign="Left" /></asp:BoundColumn>
					<asp:BoundColumn DataField='TypeName' HeaderText='类型' HeaderStyle-HorizontalAlign="center"><ItemStyle HorizontalAlign="Left" /></asp:BoundColumn>
					<asp:BoundColumn DataField='ReadCount' HeaderText='阅读次数' HeaderStyle-HorizontalAlign="center"><ItemStyle HorizontalAlign="Left" /></asp:BoundColumn>
					<asp:BoundColumn DataField='RegUserName' HeaderText='提供者' HeaderStyle-HorizontalAlign="center"><ItemStyle HorizontalAlign="Left" /></asp:BoundColumn>
					<asp:BoundColumn DataField='RegTime' HeaderText='创建时间' HeaderStyle-HorizontalAlign="center"><ItemStyle HorizontalAlign="Left" /></asp:BoundColumn>
				</Columns>
				</asp:datagrid>
			</td>
	</tr>
	<tr>
	    <td align="right"><uc1:ControlPage id="ControlPage1" runat="server" Visible="false"></uc1:ControlPage></td>
	</tr>
</table>
</asp:Content>
