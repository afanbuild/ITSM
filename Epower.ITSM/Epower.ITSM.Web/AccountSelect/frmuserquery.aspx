<%@ Page language="c#" validateRequest=false Inherits="Epower.ITSM.Web.AccountSelect.frmUserQuery" Codebehind="frmUserQuery.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>�û���ѯ</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="../Js/Common.js"></script>
		<!--#include file="../Js/tableSort.js" -->
		<SCRIPT language="javascript">
					        
        function GetDeptEmails(ccType)
        {            
            //2007-11-10 ��Ϊ�ӽ�����ѡ��
            AddBatchEmails("",false,ccType);
        }
        
        
        function AddBatchEmails(strList,isAlert,ccType)
        {

            ////2007-11-10 ��Ϊ�ӽ�����ѡ��
            var len = document.forms[0].elements.length;
			var cbCount = 0;
			var i=0
			for (i;i < len;i++)
			{
				if (document.forms[0].elements[i].type == "checkbox")
				{
				   
					if (document.forms[0].elements[i].name.indexOf("chkSelect") != -1 && 
						document.forms[0].elements[i].name.indexOf("dgUserInfo") != -1 &&
						document.forms[0].elements[i].disabled == false)
					{
					  
						if(document.forms[0].elements[i].checked == true)
						{
                            reg=/chkSelect/g;
				            var idtag=document.forms[0].elements[i].id.replace(reg,"");
				            var ename=document.all(idtag+"Name").innerText;
				           
				            var email=document.all(idtag+"hidUserID").value;
				            
				           
				            AddEmails(ename + "|" + email,isAlert,ccType);
						}
					}
				}
			}		
            
        }
        
        function AddEmails(strEmail,isAlert,ccType)
        {
           var arrEmailItem;
           if(strEmail.length>1)
			{
				arrEmailItem=strEmail.split("|");
			}
			if(arrEmailItem != null)
			{
			   for(i=0;i<arrEmailItem.length;i++)
			   {
			       if(ccType == "sc")
			       {
			           addtomailto(arrEmailItem[0],arrEmailItem[1],isAlert);
			       }
			       
			   }
			}
        }
        
        
			function SelectDept()
			{			
				var features =
				'dialogWidth:380px;' +
				'dialogHeight:500px;' +
				'directories:no; localtion:no; menubar:no; status=no; toolbar=no;scroll:no;resizable=yes';
				
				var url='../mydestop/frmpopdept.aspx?CurrDeptID='+ document.all.hidDeptID.value+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmuserquery";
				window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=380,height=500");				
			}
			
			
			function SendGroupEmail()
			{
			   	       GetDeptEmails("sc");
			 }
			
			
						
			function ClearQueryDept()
			{
				document.all.txtDeptName.value="";
				document.all.hidQueryDeptID.value="";
			}
			
			
			function addtomailto(strname,stremail,isAlert)
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
						  if(isAlert == true)
						     alert("���û�ѡ���ظ���");
						return;
						}
					}
				
					el1.add(oOption);
					Show_EmailPanel();	
			}
			
			
			
			//��ʾ�ʼ����
			function Show_EmailPanel()
			{			   
				parent.frameUserInfo.rows="148,*";
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
			
		-->
		</SCRIPT>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
<table cellpadding='1' cellspacing='2' width='100%' border='0' class="listContent">
	<TR>
		<TD style="WIDTH: 40px" noWrap align="right" class="listTitle"><FONT face="����">����:</FONT></TD>
		<TD style="WIDTH: 91px" noWrap class="list"><FONT face="����">
		
<input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
        
		<asp:textbox id="txtDeptName" runat="server" ReadOnly="True" Width="120px"></asp:textbox><INPUT id="cmdPopDept" title="Ϊ��ѯ����ѡ����" onclick="SelectDept();" type="button" value="..." class="btnClass"
					name="cmdPopParentDept">
		<TD style="WIDTH: 39px" noWrap align="right" class="listTitle"><FONT face="����">�ʺ�:</FONT></TD>
		<TD style="WIDTH: 68px" noWrap class="list"><FONT face="����" ><asp:textbox id="txtLoginName" runat="server" Width="101"></asp:textbox></FONT></TD>
		<TD style="WIDTH: 40px" noWrap align="right" class="listTitle"><FONT face="����">ѧ��:</FONT></TD>
		<TD style="WIDTH: 73px" noWrap class="list"><FONT face="����"><asp:dropdownlist id="ddlEdu" runat="server" Width="72px">
					<asp:ListItem Value="%" Selected="True">����</asp:ListItem>
					<asp:ListItem Value="��ʿ">��ʿ</asp:ListItem>
					<asp:ListItem Value="˶ʿ">˶ʿ</asp:ListItem>
					<asp:ListItem Value="��ѧ">��ѧ</asp:ListItem>
					<asp:ListItem Value="��ר">��ר</asp:ListItem>
					<asp:ListItem Value="��ר">��ר</asp:ListItem>
					<asp:ListItem Value="����">����</asp:ListItem>
					<asp:ListItem Value="����">����</asp:ListItem>
					<asp:ListItem Value="Сѧ">Сѧ</asp:ListItem>
					<asp:ListItem>��</asp:ListItem>
				</asp:dropdownlist></FONT></TD>
		<TD style="WIDTH: 38px" noWrap align="right" class="listTitle"><FONT face="����">EMail:</FONT></TD>
		<TD style="WIDTH: 128px" noWrap class="list"><FONT face="����"><asp:textbox id="txtEmail" runat="server" Width="104px"></asp:textbox></FONT></TD>
	</TR>
	<TR>
		<TD style="WIDTH: 40px" noWrap align="right" class="listTitle"><FONT face="����">ְλ:</FONT></TD>
		<TD style="WIDTH: 91px" noWrap class="list"><FONT face="����"><asp:textbox id="txtPosition" runat="server" Width="120px"></asp:textbox></FONT></TD>
		<TD style="WIDTH: 39px" noWrap align="right" class="listTitle"><FONT face="����">����:</FONT></TD>
		<TD style="WIDTH: 68px" noWrap class="list"><FONT face="����"><asp:textbox id="txtName" runat="server" Width="101px"></asp:textbox></FONT></TD>
		<TD style="WIDTH: 40px" align="right" class="listTitle"><FONT face="����">�绰:</FONT></TD>
		<TD style="WIDTH: 73px" class="list"><FONT face="����"><asp:textbox id="txtTEL" runat="server" Width="72px"></asp:textbox></FONT></TD>
		<TD style="WIDTH: 38px" noWrap align="right" class="listTitle"><FONT face="����">����:</FONT></TD>
		<TD style="WIDTH: 128px" noWrap align="left" class="list"><FONT face="����"><FONT face="����"><asp:dropdownlist id="ddlSort" runat="server" Width="48px">
						<asp:ListItem Value="Dept" Selected="True">����</asp:ListItem>
						<asp:ListItem Value="Name">����</asp:ListItem>
					</asp:dropdownlist><asp:button id="btnQuery" runat="server" Width="51px" CssClass="flowbutton" Text="�� ѯ"></asp:button><input id="Button1" class="btnclass" type="button" value="ѡ ��" onclick="SendGroupEmail();" />
            </FONT></FONT></TD>
	</TR>
</TABLE>
<br />
<table cellpadding="0" cellspacing="0" width="100%" border="0" >
<tr>
<td align="center" class="listContent">								
  <asp:datagrid id="dgUserInfo" runat="server" Width="100%" PageSize="50" AutoGenerateColumns="False" CssClass="Gridtable">
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
			<asp:TemplateColumn HeaderText="����">
				<HeaderStyle Wrap="False" Width="15%"></HeaderStyle>
				<ItemStyle Wrap="False"></ItemStyle>
				<ItemTemplate>
				   <asp:Label ID="Name" Runat=server Text='<%#DataBinder.Eval(Container.DataItem, "name")%>'>
</asp:Label>
                    <input id="hidUserID" runat=server style="width: 32px" type="hidden" value='<%#DataBinder.Eval(Container.DataItem, "userid")%>' />
				</ItemTemplate>
				<FooterStyle Wrap="False"></FooterStyle>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="�� ��">
				<HeaderStyle Wrap="False" Width="40%"></HeaderStyle>
				<ItemStyle Wrap="False"></ItemStyle>
				<ItemTemplate>
					<table width="100%" border="0" cellpadding="0" cellspacing="0">
						<tr>
						    <td  nowrap width="38"><%#GetEmailAction("ѡ��","addtomailto", DataBinder.Eval(Container.DataItem, "name").ToString(),DataBinder.Eval(Container.DataItem, "userid").ToString())%></td>
							<td nowrap><%#DataBinder.Eval(Container.DataItem, "FullDeptName")%></td>
						</tr>
					</table>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="job" HeaderText="ְ λ">
				<HeaderStyle Wrap="False" Width="15%"></HeaderStyle>
				<ItemStyle Wrap="False"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="TelNo" HeaderText="�绰����">
				<HeaderStyle Wrap="False" Width="30%"></HeaderStyle>
				<ItemStyle Wrap="False"></ItemStyle>
			</asp:BoundColumn>
			
																		
			<asp:BoundColumn Visible="False" DataField="EduLevel" HeaderText="ѧ��">
				<HeaderStyle Wrap="False"></HeaderStyle>
				<ItemStyle Wrap="False"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="DeptID" HeaderText="����ID"></asp:BoundColumn>
		</Columns>
		<PagerStyle Visible="False" HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF"
			Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
	</td>
    </tr>
    <tr>
        <td align="right" class="listTitle">
	        <uc1:controlpage id="ControlPageUserInfo" runat="server"></uc1:controlpage>
	    </TD>
	</tr>
</table>
                <INPUT id="hidQueryDeptID" style="WIDTH: 72px; HEIGHT: 19px" type="hidden" size="6" runat="server"><INPUT id="hidDeptID" style="WIDTH: 72px; HEIGHT: 19px" type="hidden" size="6" runat="server">
		</form>
	</body>
</HTML>
