<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmBYTSList.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmBYTSList" Title="�ͻ�Ͷ����ʷ" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="Table1" style="width:100%" runat="server">
<tr>
	<td class="listContent">
        <asp:datagrid id="gridUndoMsg" runat="server" Width="100%" CellSpacing="2" 
            CellPadding="1" BorderWidth="0px"
            BorderColor="White" AutoGenerateColumns="False"  CssClass="Gridtable"  
            OnItemDataBound="gridUndoMsg_ItemDataBound" 
            onitemcreated="gridUndoMsg_ItemCreated">
            <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			    <HeaderStyle CssClass="listTitle"></HeaderStyle>
            <Columns>
	           <asp:BoundColumn DataField="BY_PersonName" HeaderText="Ͷ����">
					<HeaderStyle></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="BY_ProjectName" HeaderText="��Ͷ����">
					<HeaderStyle></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="BY_SoureName" HeaderText="Ͷ����Դ">
					<HeaderStyle></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="BY_TypeName" HeaderText="Ͷ������">
					<HeaderStyle></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="BY_KindName" HeaderText="Ͷ������">
					<HeaderStyle></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="BY_ReceiveTime" HeaderText="����ʱ��" DataFormatString="{0:d}">
					<HeaderStyle></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="RegTime" HeaderText="�Ǽ�����" DataFormatString="{0:g}">
					<HeaderStyle></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="status" HeaderText="����״̬" HeaderStyle-Wrap="false">
					<HeaderStyle></HeaderStyle>
				</asp:BoundColumn>
				<asp:TemplateColumn HeaderText="����" ItemStyle-HorizontalAlign="center">
					<ItemTemplate>
						<INPUT id="CmdDeal" class="btnClass" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>' type="button" value='����' runat="server">
					</ItemTemplate>
					<HeaderStyle HorizontalAlign="center" Width="5%" />
				</asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="status"></asp:BoundColumn>
			</Columns>
            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
        </asp:datagrid>
      </TD>
      </tr>
      <tr>
      <td class="listTitle" align="right"><uc1:ControlPage id="ControlPageIssues" runat="server"></uc1:ControlPage></td>
</tr>
</table>
</asp:Content>
