<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmuserqueryMult.aspx.cs" Inherits="Epower.ITSM.Web.MyDestop.frmuserqueryMult" Title="��ַ��" %>

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
			
				window.open('frmpopdept.aspx?CurrDeptID='+ document.all.<%=hidDeptID.ClientID%>.value + "&LimitCurr=<%=IsLimit %>&TypeFrm=frmuserqueryMult",'','resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=150,top=50');
				

			}
			
			
			function ClearQueryDept()
			{
				document.all.<%=txtDeptName.ClientID%>.value="";
				document.all.<%=hidQueryDeptID.ClientID%>.value="";
			}
			
			//ȫѡ��ѡ��
		    function checkAll(checkAll)
		    {			  
			    var len = document.forms[0].elements.length;
			    var cbCount = 0;
			    for (i=0;i < len;i++)
			    {
				    if (document.forms[0].elements[i].type == "checkbox")
				    {
					    if (document.forms[0].elements[i].name.indexOf("chkSelect") != -1 && 
						    document.forms[0].elements[i].name.indexOf("dgUserInfo") != -1 &&
						    document.forms[0].elements[i].disabled == false)
					    {
						    document.forms[0].elements[i].checked = checkAll.checked;

						    cbCount += 1;
					    }
				    }
			    }		
		    } 
		    
		    function CancelSelect()     //ȡ��ѡ��
	        {
	            if(confirm("ȷ��ȡ��ѡ����?"))
	            {
	                window.close();
	            }
	            event.returnValue = false;
	        }
		-->
</SCRIPT>
<TABLE id="Table5" width="98%"  class="listContent">
		<TR>
			<TD style="WIDTH: 15%;" noWrap align="right" class="listTitle">����</TD>
			<TD style="WIDTH: 35%;;" noWrap class="list"><asp:textbox id="txtDeptName" runat="server" ReadOnly="True" Width="120px"></asp:textbox>
			<INPUT id="cmdPopDept" title="Ϊ��ѯ����ѡ����" onclick="SelectDept();" type="button" value="..."
						name="cmdPopParentDept" class="btnClass2"><!--<INPUT id="btnClearDept" onclick="ClearQueryDept();" type="button" size="18" value="��" name="cmdPopParentDept" title="�Ƴ����Ų�ѯ����"></TD>-->
			<TD style="WIDTH: 15%;;" noWrap align="right" class="listTitle">�ʺ�</TD>
			<TD style="WIDTH: 35%;" noWrap class="list"><FONT face="����" ><asp:textbox id="txtLoginName" runat="server" Width="101"></asp:textbox></FONT></TD>
		</TR>
		<tr>
		    <TD noWrap align="right" class="listTitle">ѧ��</TD>
			<TD noWrap class="list"><asp:dropdownlist id="ddlEdu" runat="server" Width="85px">
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
			<TD noWrap class="list"><asp:textbox id="txtEmail" runat="server" Width="122px"></asp:textbox></TD>
		</tr>
		<TR>
			<TD  noWrap align="right" class="listTitle">ְλ</TD>
			<TD  noWrap class="list"><uc2:ctrFlowCataDropList ID="ctrFCDServiceType" runat="server"  RootID="1014" /></TD>
			<TD  noWrap align="right" class="listTitle">����</TD>
			<TD  noWrap class="list"><asp:textbox id="txtName" runat="server" Width="101px"></asp:textbox></TD>
		</TR>
		<tr>
		    <TD align="right" class="listTitle">�绰</TD>
			<TD class="list"><asp:textbox id="txtTEL" runat="server" Width="85"></asp:textbox></TD>
			<TD noWrap align="right" class="listTitle">����</TD>
			<TD noWrap align="left" class="list"><asp:dropdownlist id="ddlSort" runat="server" Width="68px">
				<asp:ListItem Value="Dept" Selected="True">����</asp:ListItem>
				<asp:ListItem Value="Name">����</asp:ListItem>
			</asp:dropdownlist>
		    </TD>
		</tr>
	</TABLE>
	<TABLE id="Table2" width="98%"  class="listContent">
		<TR>
			<TD noWrap align="center" class="list">
			    <asp:Button ID="btnSelect" runat="server" Text="����ѡ��" 
                    onclick="btnSelect_Click" />
                <asp:Button ID="btnAdd" runat="server" Text="���ѡ��" 
                    onclick="btnAdd_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="ȡ��ѡ��" 
                    OnClientClick="CancelSelect();" />
			</TD>
		</TR>
	</TABLE>
	<br />
<TABLE id="Table1" width="98%" class="listContent" cellpadding=0>
    <TR>
			<TD vAlign="top" align="center" colSpan="2" class="listContent">
			<asp:datagrid id="dgUserInfo" runat="server" Width="100%" PageSize="50" AutoGenerateColumns="False"  CssClass="Gridtable" >
					<Columns>
					    <asp:TemplateColumn>
						    <HeaderTemplate>
				                <asp:CheckBox id="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
				            </HeaderTemplate>
							<ItemTemplate>
								<asp:CheckBox id="chkSelect" runat="server"></asp:CheckBox>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:BoundColumn Visible="False" DataField="UserId"></asp:BoundColumn>
						<asp:BoundColumn Visible="False" DataField="name"></asp:BoundColumn>
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
						<asp:TemplateColumn HeaderText="�����ʼ�" Visible="false">
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
