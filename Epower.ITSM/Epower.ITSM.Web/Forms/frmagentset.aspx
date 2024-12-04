<%@ Page Language="C#" MasterPageFile="~/MasterPageSingle.master" AutoEventWireup="true" Inherits="Epower.ITSM.Web.Forms.FrmAgentSet" Title="Untitled Page" Codebehind="FrmAgentSet.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPageSingle.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<br />
<br />
<TABLE id="Table1" width="700" align="center" class="listContent">
	<TR>
		<TD class="listTitleNew" style="text-align:center"><uc1:ctrtitle id="CtrTitle1" runat="server"></uc1:ctrtitle></TD>
	</TR>
	<TR>
		<TD align="center" class="list" height="20"><asp:label id="lblMsg" runat="server">出差授权已经生效</asp:label></TD>
	</TR>
	<TR height="36">
		<TD class="listTitle"></TD>
	</TR>
	<TR>
		<TD class="listTitle">第一步：设置代理 <INPUT onclick="window.open('form_Agent_Change.aspx','selectPerson','scrollbars=no,resizable=no,top=170,left=170,width=480,height=330')"
				type="button" value="全部设置为" class="btnClass"></TD>
	</TR>
	<TR>
		<TD class="list" height=20>或者点击下面按钮单独设置代理人</TD>
	</TR>
</Table>
<TABLE id="Table2" width="700" align="center" cellpadding=0 cellspacing=0>
	<TR>
		<TD class="listContent"><asp:datagrid id="grdAgent" runat="server" Width="700" AutoGenerateColumns="False"  CssClass="Gridtable" >
				<Columns>
					<asp:BoundColumn DataField="AppName" ReadOnly="True" HeaderText="应用名称">
						<HeaderStyle Width="150px"></HeaderStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="AgentName" ReadOnly="True" HeaderText="代理人">
						<HeaderStyle Width="100px"></HeaderStyle>
					</asp:BoundColumn>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="center">
						<HeaderStyle Width="80px"></HeaderStyle>
						<ItemTemplate>
							<INPUT id=cmdAgent class="btnClass1" onclick='window.open("form_Agent_Change.aspx?AppID=<%#DataBinder.Eval(Container.DataItem, "AppID")%>","selectPerson","scrollbars=no,resizable=no,top=170,left=170,width=480,height=330" );' type=button value=更改代理>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle NextPageText="下一页" PrevPageText="上一页" HorizontalAlign="Right" ForeColor="Black"
					BackColor="#C6C3C6"></PagerStyle>
			</asp:datagrid></TD>
	</TR>
</Table>
<TABLE id="Table3" width="700" align="center" class="listContent">
    <TR style="display:none;">
		<TD class="list">第二步：设置有效期限：
            <uc2:ctrdateandtime ID="CtrDateBegin" runat="server" ShowTime="true" />至
            <uc2:ctrdateandtime ID="CtrDateEnd" runat="server" ShowTime="true" />
        </TD>
	</TR>
	<TR>
		<TD class="listTitle">第二步：
			<asp:button id="cmdStatus" SkinID="btnClass3"  runat="server" Text="启动代理" onclick="cmdStatus_Click"></asp:button></TD>
	</TR>
	<TR>
		<TD class="list">第三步：
			<asp:button id="cmdCancel" SkinID="btnClass3"  runat="server" Text="取消全部代理" onclick="cmdCancel_Click"></asp:button></TD>
	</TR>
</TABLE>
<div  id='divShowMessageDetail' style='position:absolute;display: none;width:0; height:0;'></div>
</asp:Content>
