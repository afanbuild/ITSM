<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmUserQuery.aspx.cs" Inherits="Epower.ITSM.Web.MyDestop.frmUserQuery" Title="��ַ��" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<SCRIPT language="javascript">
		<!--
			
			function SelectDept()
			{
				var features =
				'dialogWidth:380px;' +
				'dialogHeight:500px;' +
				'directories:no; localtion:no; menubar:no; status=no; toolbar=no;scroll:no;resizable=yes';
				var	value=window.showModalDialog('frmpopdept.aspx?CurrDeptID='+ document.all.<%=hidDeptID.ClientID%>.value,'',features);
				if(value != null)
				{
					if(value.length>1)
					{
						arr=value.split("@");
						document.all.<%=txtDeptName.ClientID%>.value=arr[1];
						document.all.<%=hidQueryDeptID.ClientID%>.value=arr[0];
					}
				}
			}
			
			
			function ClearQueryDept()
			{
				document.all.<%=txtDeptName.ClientID%>.value="";
				document.all.<%=hidQueryDeptID.ClientID%>.value="";
			}
			
			
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
						alert("���ʼ���ַѡ���ظ���");
						return;
						}
					}
				for (j=0; j<el2.options.length; j++) 
					{
					if (el2.options(j).value==stremail)
						{
						alert("���ʼ���ַѡ���ظ���");
						return;
						}
					}
				for (j=0; j<el3.options.length; j++) 
					{
					if (el3.options(j).value==stremail)
						{
						alert("���ʼ���ַѡ���ظ���");
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
						alert("���ʼ���ַѡ���ظ���");
						return;
						}
					}
				for (j=0; j<el2.options.length; j++) 
					{
					if (el2.options(j).value==stremail)
						{
						alert("���ʼ���ַѡ���ظ���");
						return;
						}
					}
				for (j=0; j<el3.options.length; j++) 
					{
					if (el3.options(j).value==stremail)
						{
						alert("���ʼ���ַѡ���ظ���");
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
						alert("���ʼ���ַѡ���ظ���");
						return;
						}
					}
				for (j=0; j<el2.options.length; j++) 
					{
					if (el2.options(j).value==stremail)
						{
						alert("���ʼ���ַѡ���ظ���");
						return;
						}
					}
				for (j=0; j<el3.options.length; j++) 
					{
					if (el3.options(j).value==stremail)
						{
						alert("���ʼ���ַѡ���ظ���");
						return;
						}
					}   
				el3.add(oOption);
				Show_EmailPanel();	
			}
			
			//��ʾ�ʼ����
			function Show_EmailPanel()
			{
			   // alert(parent.frameUserInfo.rows);
				parent.frameUserInfo.rows="148,*";
			}
		-->
</SCRIPT>
<TABLE id="Table5" width="98%"  class="listContent">
		<TR>
			<TD style="WIDTH: 15%;" noWrap align="right" class="listTitle">����</TD>
			<TD style="WIDTH: 35%;;" noWrap class="list"><asp:textbox id="txtDeptName" runat="server" ReadOnly="True"></asp:textbox>
			<INPUT id="cmdPopDept" title="Ϊ��ѯ����ѡ����" onclick="SelectDept();" type="button" value="..." class="btnClass2"
						name="cmdPopParentDept" class="btnClass"><!--<INPUT id="btnClearDept" onclick="ClearQueryDept();" type="button" size="18" value="��" name="cmdPopParentDept" title="�Ƴ����Ų�ѯ����"></TD>-->
			<TD style="WIDTH: 15%;;" noWrap align="right" class="listTitle">�ʺ�</TD>
			<TD style="WIDTH: 35%;" noWrap class="list"><FONT face="����" ><asp:textbox id="txtLoginName" runat="server"></asp:textbox></FONT></TD>
		</TR>
		<tr>
		    <TD noWrap align="right" class="listTitle">ѧ��</TD>
			<TD noWrap class="list"><asp:dropdownlist id="ddlEdu" runat="server"  Width="53%">
						<asp:ListItem Value="%" Selected="True">����</asp:ListItem>
						<asp:ListItem Value="��ʿ">��ʿ</asp:ListItem>
						<asp:ListItem Value="˶ʿ">˶ʿ</asp:ListItem>
						<asp:ListItem Value="��ѧ">��ѧ</asp:ListItem>
						<asp:ListItem Value="��ר">��ר</asp:ListItem>
						<asp:ListItem Value="��ר">��ר</asp:ListItem>
						<asp:ListItem Value="����">����</asp:ListItem>
						<asp:ListItem Value="����">����</asp:ListItem>
						<asp:ListItem Value="Сѧ">Сѧ</asp:ListItem>
						<asp:ListItem Value="">��</asp:ListItem>
					</asp:dropdownlist></TD>
			<TD noWrap align="right" class="listTitle">EMail</TD>
			<TD noWrap class="list"><asp:textbox id="txtEmail" runat="server"></asp:textbox></TD>
		</tr>
		<TR>
			<TD  noWrap align="right" class="listTitle">ְλ</TD>
			<TD  noWrap class="list"><uc2:ctrFlowCataDropList ID="ctrFCDServiceType" runat="server"  RootID="1014"  Width="53%" /></TD>
			<TD  noWrap align="right" class="listTitle">����</TD>
			<TD  noWrap class="list"><asp:textbox id="txtName" runat="server"></asp:textbox></TD>
		</TR>
		<tr>
		    <TD align="right" class="listTitle">�绰</TD>
			<TD class="list"><asp:textbox id="txtTEL" runat="server"></asp:textbox></TD>
			<TD noWrap align="right" class="listTitle">����</TD>
			<TD noWrap align="left" class="list"><asp:dropdownlist id="ddlSort" runat="server" Width="53%">
				<asp:ListItem Value="Dept" Selected="True">����</asp:ListItem>
				<asp:ListItem Value="Name">����</asp:ListItem>
			</asp:dropdownlist>
		    </TD>
		</tr>
	</TABLE>
	<br />
<TABLE id="Table1" width="98%" class="listContent" cellpadding=0>
    <TR>
			<TD vAlign="top" align="center" colSpan="2" class="listContent">
			<asp:datagrid id="dgUserInfo" runat="server" Width="100%" PageSize="50" AutoGenerateColumns="False"  CssClass="Gridtable" >
					<Columns>
						<asp:BoundColumn Visible="False" DataField="UserId"></asp:BoundColumn>
						<asp:HyperLinkColumn  DataTextField="name" HeaderText="����" Target="_blank" DataNavigateUrlField="UserId" DataNavigateUrlFormatString="frmShowuser.aspx?userId={0}"></asp:HyperLinkColumn>
						<asp:BoundColumn DataField="FullDeptName" HeaderText="�� ��">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="job" HeaderText="ְλ">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="TelNo" HeaderText="�绰">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:TemplateColumn HeaderText="�����ʼ�">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<table border="0" cellpadding="0" cellspacing="0">
									<tr>
									    <td nowrap style="<%= strZHShow %>"><%#DataBinder.Eval(Container.DataItem, "email")%></td>
										<td nowrap style="<%= strZHHiden %>"><A href='mailto:<%#DataBinder.Eval(Container.DataItem, "email")%>' ><%#DataBinder.Eval(Container.DataItem, "email")%></A></td>
										<td nowrap style="<%= strZHHiden %>" width="38"><%#GetEmailAction("����","addtomailto", DataBinder.Eval(Container.DataItem, "name").ToString(),DataBinder.Eval(Container.DataItem, "email").ToString())%></td>
										<td nowrap style="<%= strZHHiden %>" width="38"><%#GetEmailAction("����","addtocc", DataBinder.Eval(Container.DataItem, "name").ToString(),DataBinder.Eval(Container.DataItem, "email").ToString())%></td>
										<td nowrap style="<%= strZHHiden %>" width="38"><%#GetEmailAction("����","addtoBcc", DataBinder.Eval(Container.DataItem, "name").ToString(),DataBinder.Eval(Container.DataItem, "email").ToString())%></td>
									</tr>
								</table>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:BoundColumn Visible="False" DataField="EduLevel" HeaderText="ѧ��">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn Visible="False" DataField="DeptID" HeaderText="����ID"></asp:BoundColumn>
					</Columns>
					<PagerStyle Visible="False" HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF"
						Mode="NumericPages"></PagerStyle>
				</asp:datagrid>
			</TD>
		</TR> 
        <TR>
			<TD class="listTitle" align="right"><uc1:controlpage id="ControlPageUserInfo" runat="server"></uc1:controlpage><INPUT id="hidQueryDeptID" type="hidden" size="6" runat="server">
			<INPUT id="hidDeptID" type="hidden" size="6" runat="server"></TD>
		</TR>
</TABLE>
</asp:Content>
