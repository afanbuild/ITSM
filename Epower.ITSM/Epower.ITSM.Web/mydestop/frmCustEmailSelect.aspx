<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmCustEmailSelect.aspx.cs" Inherits="Epower.ITSM.Web.mydestop.frmCustEmailSelect" Title="无标题页" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<SCRIPT language="javascript" type="text/javascript">
		<!--
			function addtomailto(strname,stremail)
			{
				var j;
				var el1 = window.parent.mail.document.fmSubmit.List1;
				var el2 = window.parent.mail.document.fmSubmit.List2;
				var el3 = window.parent.mail.document.fmSubmit.List3;
				var oOption = window.parent.mail.document.createElement("OPTION");
				oOption.text=strname;
				oOption.value=stremail;
				for (j=0; j<el1.options.length; j++) 
					{
					if (el1.options(j).value==stremail)
						{
						alert("该邮件地址选择重复！");
						return;
						}
					}
				for (j=0; j<el2.options.length; j++) 
					{
					if (el2.options(j).value==stremail)
						{
						alert("该邮件地址选择重复！");
						return;
						}
					}
				for (j=0; j<el3.options.length; j++) 
					{
					if (el3.options(j).value==stremail)
						{
						alert("该邮件地址选择重复！");
						return;
						}
					}
					el1.add(oOption);
					Show_EmailPanel();	
			}
			
			
			function addtocc(strname,stremail)
			{
				var j
				var el1 = window.parent.mail.document.fmSubmit.List1;
				var el2 = window.parent.mail.document.fmSubmit.List2;
				var el3 = window.parent.mail.document.fmSubmit.List3;
				var oOption = window.parent.mail.document.createElement("OPTION");
				oOption.text=strname;
				oOption.value=stremail;
				for (j=0; j<el1.options.length; j++) 
					{
					if (el1.options(j).value==stremail)
						{
						alert("该邮件地址选择重复！");
						return;
						}
					}
				for (j=0; j<el2.options.length; j++) 
					{
					if (el2.options(j).value==stremail)
						{
						alert("该邮件地址选择重复！");
						return;
						}
					}
				for (j=0; j<el3.options.length; j++) 
					{
					if (el3.options(j).value==stremail)
						{
						alert("该邮件地址选择重复！");
						return;
						}
					}   
				el2.add(oOption);
				Show_EmailPanel();	
			}

			function addtoBcc(strname,stremail)
			{
				var j
				var el1 = window.parent.mail.document.fmSubmit.List1;
				var el2 = window.parent.mail.document.fmSubmit.List2;
				var el3 = window.parent.mail.document.fmSubmit.List3;
				var oOption = top.frames(1).document.createElement("OPTION");
				oOption.text=strname;
				oOption.value=stremail;
				for (j=0; j<el1.options.length; j++) 
					{
					if (el1.options(j).value==stremail)
						{
						alert("该邮件地址选择重复！");
						return;
						}
					}
				for (j=0; j<el2.options.length; j++) 
					{
					if (el2.options(j).value==stremail)
						{
						alert("该邮件地址选择重复！");
						return;
						}
					}
				for (j=0; j<el3.options.length; j++) 
					{
					if (el3.options(j).value==stremail)
						{
						alert("该邮件地址选择重复！");
						return;
						}
					}   
				el3.add(oOption);
				Show_EmailPanel();	
			}
			
			//显示邮件面板
			function Show_EmailPanel()
			{
			   // alert(parent.frameUserInfo.rows);
				parent.frameCustInfo.rows="148,*";
			}
		-->
</SCRIPT>
<table width='100%' class="listContent">
    <tr>
		<td class="listTitle" align="right" style="width:15%; height: 24px;">
            <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal></td>
		<td class="list" align="left" style="height: 24px">
            <asp:DropDownList ID="ddltMastCustID" runat="server">
            </asp:DropDownList></td>
		<td noWrap class="listTitle" align="right"  style="width:15%; height: 24px;">
            <asp:Literal ID="LitCustomerType" runat="server" Text="客户类型"></asp:Literal></td>
		<td class="list" align="left" style="height: 24px"><uc2:ctrFlowCataDropList ID="ctrFCDServiceType" runat="server" ContralState="eNormal"
                RootID="1019" Visible="true" /></td>
	</tr>
	<tr>
		<td noWrap class="listTitle" align="right"  style="width:15%; height: 26px;">
            <asp:Literal ID="LitCustName" runat="server" Text="客户名称"></asp:Literal></td>
		<td class="list" align="left" style="height: 26px"><asp:TextBox id="txtShortName" runat="server"></asp:TextBox></td>
		<td class="listTitle" align="right" style="width:15%; height: 26px;">
		<asp:Literal ID="LitFullName" runat="server" Text="英文名称"></asp:Literal></td>
		<td class="list" align="left" style="height: 26px"><asp:textbox id="txtFullName" runat="server"></asp:textbox></td>
	</tr>
	<tr>
		<td noWrap class="listTitle" align="right"  style="width:15%">
            <asp:Literal ID="LitContact" runat="server" Text="联系人"></asp:Literal></td>
		<td class="list" align="left"><asp:TextBox id="txtLinkMan1" runat="server"></asp:TextBox></td>
		<td class="listTitle" align="right" style="width:15%">
		<asp:Literal ID="LitCTel" runat="server" Text="联系电话"></asp:Literal></td>
		<td class="list" align="left"><asp:textbox id="txtTel1" runat="server"></asp:textbox></td>
	</tr>
    <tr>
        <td align="right" class="listTitle" nowrap="nowrap" style="width: 15%">
            <asp:Literal ID="LitCustomCode" runat="server" Text="客户代码"></asp:Literal></td>
        <td align="left" class="list">
            <asp:TextBox ID="txtCustomCode" runat="server" Width="128px"></asp:TextBox></td>
        <td align="right" class="listTitle" style="width: 15%">
        </td>
        <td align="left" class="list">
        </td>
    </tr>
</table>
<br />
<table cellpadding="0" width="100%" class="listContent">
	<tr>
		<td class="listContent">
		    <asp:datagrid id="dgECustomer" runat="server" Width="100%" cellpadding="1" cellspacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCreated="dgECustomer_ItemCreated">
				<AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			    <HeaderStyle CssClass="listTitle"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="center" Visible="false">
						<ItemTemplate>
							<asp:CheckBox id="chkDel" runat="server"></asp:CheckBox>
						</ItemTemplate>
						<HeaderStyle Width="5%" HorizontalAlign="center" Wrap="false"></HeaderStyle>
					</asp:TemplateColumn>
					<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
					<asp:BoundColumn DataField="MName" HeaderText="服单单位"></asp:BoundColumn>
					<asp:BoundColumn DataField="CustomerTypeName" HeaderText="类型"></asp:BoundColumn>
					<asp:BoundColumn DataField="ShortName" HeaderText="客户名称"></asp:BoundColumn>
					<asp:BoundColumn DataField="FullName" HeaderText="英文名称"></asp:BoundColumn>					
					<asp:BoundColumn DataField="linkman1" HeaderText="联系人">
						<HeaderStyle Wrap="False"></HeaderStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="tel1" HeaderText="联系电话">
						<HeaderStyle Wrap="False"></HeaderStyle>
					</asp:BoundColumn>
					<asp:TemplateColumn HeaderText="电子邮件">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<table border="0" cellpadding="0" cellspacing="0">
									<tr>
									    <td nowrap style="<%= strZHShow %>"><%#DataBinder.Eval(Container.DataItem, "Email")%></td>
										<td nowrap style="<%= strZHHiden %>"><A href='mailto:<%#DataBinder.Eval(Container.DataItem, "email")%>' ><%#DataBinder.Eval(Container.DataItem, "email")%></A></td>
										<td nowrap style="<%= strZHHiden %>" width="38"><%#GetEmailAction("发送","addtomailto", DataBinder.Eval(Container.DataItem, "ShortName").ToString(),DataBinder.Eval(Container.DataItem, "email").ToString())%></td>
										<td nowrap style="<%= strZHHiden %>" width="38"><%#GetEmailAction("抄送","addtocc", DataBinder.Eval(Container.DataItem, "ShortName").ToString(),DataBinder.Eval(Container.DataItem, "email").ToString())%></td>
										<td nowrap style="<%= strZHHiden %>" width="38"><%#GetEmailAction("密送","addtoBcc", DataBinder.Eval(Container.DataItem, "ShortName").ToString(),DataBinder.Eval(Container.DataItem, "email").ToString())%></td>
									</tr>
								</table>
							</ItemTemplate>
						</asp:TemplateColumn>
				</Columns>
			</asp:datagrid>
		</td>
	</tr>
	<tr><td class="listTitle" align="right"><uc1:controlpage id="ControlPage1" runat="server"></uc1:controlpage></td></tr>
</table>
<asp:label id="labMsg" runat="server" Visible="False" ForeColor="Red"></asp:label>
</asp:Content>