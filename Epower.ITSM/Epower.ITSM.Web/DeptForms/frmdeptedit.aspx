<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmDeptEdit" Codebehind="frmDeptEdit.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>���ű༭</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="../Controls/Calendar/Popup.js"></script>
		<script language="javascript" src="../Js/App_Common.js"></script>
		
	</HEAD>
	<script language="javascript">		
			function SelectPDept()
			{
				if(document.all.hidDeptID.value == 1)
				{
					alert("�Ѿ���������");
					return;
				}
				
			    var url="frmpopdept.aspx?CurrDeptID=" + document.all.hidDeptID.value+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmdeptedit";
			    
				window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=500,height=400,left=250,top=50");

			}
			function SelectLeader()
			{
			    var url="frmselectstaffright.aspx?Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmdeptedit";
			    window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=499,height=523,left=250,top=50");
			}
			
			function SelectManager()
			{
			    var url="frmselectstaffright.aspx?Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>&TypeFrm=frmdeptedit_Manager";
			    window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=499,height=523,left=250,top=50");
			    return;
				var value=window.showModalDialog("frmpopstaff.aspx");
				if(value != null){
					if(value.length>1)
					{
						arr=value.split("@");
						document.all.txtManagerName.value=arr[1];
						document.all.hidMangerName.value = arr[1];
						document.all.hidManagerID.value=arr[0];
					}
					else
					{
					    document.all.txtManagerName.value="��������Ա";
					    document.all.hidMangerName.value = "��������Ա";
						document.all.hidManagerID.value="1";
					}
				}
			}
			
			function delete_confirm()
			{
				if (event.srcElement.value =="ɾ��" )
					event.returnValue =confirm("ȷ��Ҫɾ�����������?");
			}
			document.onclick=delete_confirm;
			
			
			function JoinActor()
			{
				var features =
				'dialogWidth:380px;' +
				'dialogHeight:500px;' +
				'directories:no; localtion:no; menubar:no; status=no; toolbar=no;scroll:no;resizable=yes';
				//window.showModalDialog('frmJoinActor2.aspx?ActorType=20&ObjectID='+document.all.hidUserID.value,'',features);
				window.open('frmJoinActor_Container.aspx?ActorType=10&ObjectID='+document.all.hidDeptID.value,"","scrollbars=no,status=yes ,resizable=yes,width=470,height=530,left=250,top=50");
				//window.showModalDialog('frmJoinActor_Container.aspx?ActorType=10&ObjectID='+document.all.hidDeptID.value,'',features);
			}
			
			
			//ֻ������������
			function NumberKey()
			{
				if(!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode==8 || event.keyCode==46))
				{
					//alert(event.keyCode);
					event.returnValue = false;
				}
			}
			
			function ChangeDeptType(DeptKind)
			{
				var newOption;
				var dpdDeptType=document.all.dpdDeptType;
				while(dpdDeptType.options.length>0) {dpdDeptType.remove(0);}
				
				switch(DeptKind)
				{
					case "0":
						newOption = document.createElement("OPTION");
						newOption.value ="0";
						newOption.text = "���ز���";
						dpdDeptType.options.add(newOption);
						
						newOption = document.createElement("OPTION");
						newOption.value ="1";
						newOption.text = "�ۺϰ칫��";
						dpdDeptType.options.add(newOption);

						newOption = document.createElement("OPTION");
						newOption.value ="5";
						newOption.text = "���쵼";
						dpdDeptType.options.add(newOption);
						
						break;
						
					case "1":
						newOption = document.createElement("OPTION");
						newOption.value ="3";
						newOption.text = "������λ";
						dpdDeptType.options.add(newOption);
						
						newOption = document.createElement("OPTION");
						newOption.value ="4";
						newOption.text = "ֱ����λ";
						dpdDeptType.options.add(newOption);
					
						break;
				}
			}

			
		</script>
	<body>
		<form id="Form1" method="post" runat="server">
		<input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
			<table style="width:100%" class="listContent" cellpadding="2">
				<tr>
					<td colSpan="2" class="list"><uc1:ctrtitle id="CtrTitle" runat="server"></uc1:ctrtitle></td>
				</tr>
				<tr height="40">
					<td colSpan="2" class="listTitle"><asp:button id="cmdAdd" runat="server" Text="����" onclick="cmdAdd_Click" CssClass="btnClass"></asp:button>&nbsp;&nbsp;
						<asp:button id="cmdSave" runat="server" Text="����" onclick="cmdSave_Click" CssClass="btnClass"></asp:button>&nbsp;&nbsp;
						<asp:button id="cmdDelete" runat="server" Text="ɾ��" onclick="cmdDelete_Click" CssClass="btnClass"></asp:button>&nbsp;&nbsp; 
						<INPUT id="cmdJoinActor" onclick="JoinActor();" type="button" value="�����û���" class="btnClass">&nbsp;
					</td>
				</tr>
				<tr>
					<td noWrap class="listTitle">��������:
					</td>
					<td class="list"><asp:textbox id="txtDeptName" runat="server" Width="273px" MaxLength="25"></asp:textbox>
					<label style="color:Red;">*</label> 
					</td>
				</tr>
				<TR>
					<TD noWrap class="listTitle"><FONT face="����">���ű���:</FONT></TD>
					<TD noWrap class="list"><FONT face="����"><asp:textbox id="txtDeptCode" runat="server" Width="273px" MaxLength="25"></asp:textbox></FONT></TD>
				</TR>
				<tr>
					<td noWrap class="listTitle">��������:
					</td>
					<td class="list"><asp:textbox id="txtDesc" runat="server" Width="273" Height="56px" TextMode="MultiLine" onblur="MaxLength(this,100,'�����������ȳ����޶�����:');"></asp:textbox></td>
				</tr>
				<TR id="trDeptKind" runat=server>
					<TD style="HEIGHT: 28px" noWrap class="listTitle"><FONT face="����">��������:</FONT></TD>
					<TD style="HEIGHT: 28px" class="list"><FONT face="����"><asp:dropdownlist id="dpdDeptKind" runat="server" Width="273px" ></asp:dropdownlist></FONT></TD>
				</TR>
				<TR>
					<TD noWrap class="listTitle">�������:</TD>
					<TD class="list"><asp:dropdownlist id="dpdDeptProfessional" runat="server" Width="273px"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD noWrap class="listTitle">�ϼ�����:</TD>
					<TD class="list"><asp:textbox id="txtPDeptName" runat="server" Width="258" ReadOnly="True"></asp:textbox>
					<INPUT id="hidPDeptID" style="WIDTH: 56px" type="hidden" runat="server"><INPUT id="cmdPopParentDept" onclick="SelectPDept()" type="button" value="..." class="btnClass2"></TD>
				</TR>
				<tr>
					<td noWrap class="listTitle">���Ź���Ա:
					</td>
					<td class="list"><asp:textbox id="txtManagerName" runat="server" Width="258" ReadOnly="True"></asp:textbox>
					<INPUT id="hidManagerID" style="WIDTH: 56px; HEIGHT: 19px" type="hidden" size="4" runat="server"><INPUT id="cmdPopManager" onclick="SelectManager()" type="button" value="..." class="btnClass2">
					</td>
				</tr>
				<tr>
					<td noWrap class="listTitle">�����쵼:
					</td>
					<td class="list"><asp:textbox id="txtLeaderName" runat="server" Width="259" ReadOnly="True"></asp:textbox>
					<INPUT id="hidLeaderID" style="WIDTH: 56px; HEIGHT: 22px" type="hidden" size="4" runat="server"><INPUT id="cmdPopLeader" onclick="SelectLeader()" type="button" value="..." class="btnClass2">
					</td>
				</tr>
				<TR>
					<TD noWrap class="listTitle"><FONT face="����">��������ID:</FONT></TD>
					<TD noWrap class="list"><FONT face="����"><asp:textbox id="txtSortID" runat="server" Width="258px" MaxLength="9">-1</asp:textbox><asp:rangevalidator id="RangeValidator1" runat="server" ControlToValidate="txtSortID" ErrorMessage="RangeValidator"
								Type="Integer" MinimumValue="-1" MaximumValue="999999999"> ��������Чֵ</asp:rangevalidator></FONT></TD>
				</TR>
                <TR>
					<TD noWrap class="listTitle"><FONT face="����">����ID:</FONT></TD>
					<TD noWrap class="list"><FONT face="����">
					&nbsp;&nbsp;
					<asp:Label ID="lblDeptID" runat="server"></asp:Label>
										
					</FONT></TD>
				</TR>
				<TR>
					<TD class="listTitle"></TD>
					<TD noWrap class="listTitle"><FONT face="����">*ע:����IDΪ-1ʱ,��ϵͳָ��������ID,��Чֵ��Χ(-1 ~ 99999)</FONT></TD>
				</TR>
			</table>
			<asp:label id="labMsg" runat="server" Font-Bold="True" ForeColor="Red"></asp:label>
			<asp:checkbox id="chkbIsTempDept" runat="server" Text=" ��" Visible="False"></asp:checkbox><asp:textbox id="txtStartDate" runat="server" Width="112px" Visible="False" ReadOnly="True"></asp:textbox><asp:textbox id="txtEndDate" runat="server" Width="112px" Visible="False" ReadOnly="True"></asp:textbox>
			<input type="hidden" id="hidMangerName" runat="server" />
			<INPUT id="hidOrgID" type="hidden" runat="server"> <INPUT id="hidDeptID" type="hidden" runat="server">
			
			<asp:Literal ID="literalJavaScript" Visible="false" runat="server">
			    
			</asp:Literal>
		</form>
		<script>
		<!--
			//���ű������ܼ����û���	
			if(document.all.hidDeptID.value.length>0)
			{
				document.all.cmdJoinActor.style.visibility="visible";
			}
			else
			{
				document.all.cmdJoinActor.style.visibility="hidden";
			}

		-->
		</script>
	</body>
</HTML>
