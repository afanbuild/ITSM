<%@ Page validateRequest="false" language="c#" Inherits="Epower.ITSM.Web.Forms.frmActorCondEdit" Codebehind="frmActorCondEdit.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD id="HEAD1" runat="server">
		<title>������Ա����</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script type="text/javascript" src="../Js/jquery-1.7.2.min.js" ></script>
		<style type="text/css">
		.TextBoxReadOnly
        {
            border:1px solid #C0C0C0; 
            text-align:left; 
            background-color:#D3D3D3;
            width:100px;
            readonly:expression(this.readOnly=true);
        }
        .TextBoxReadWrite
        {
            border:1px solid #C0C0C0; 
            text-align:left; 
            background-color:#FFFFFF;
            width:100px;
            readonly:expression(this.readOnly=false);
        }
		</style>
		
	</HEAD>
	<script language="javascript">
		function SetReadOnly(ctr,isReadOnly)
        {
            var octr=document.getElementById(ctr);
            if(octr!=null)
            {
                if(isReadOnly)
                    octr.className="TextBoxReadOnly";
                else
                    octr.className="TextBoxReadWrite";
                    
            }
        }

			function PopCondValue(obj)
			{
				reg=/cmdPop/g;
				var idtag=obj.id.replace(reg,"");
				
				var v=document.all(idtag+"cboItems").value;
				var v=v.split(',')[0];
				switch(v)
				{
					case '10' ://��Ա.����
					       //=====zxl====
					       var url="frmpopdept.aspx?objID="+obj.id+"&TypeFrm=frmactorcondedit";
					       window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=520,height=300,left=150,top=50");
						
						break;
					case '30' ://��Ա.ְλ
						break;
					case '40' ://��Ա.�Ա�
						break;
					case '50' ://��Ա.ѧ��
						break;
					case '60' ://��Ա.��ɫ
						break;/**/
					
				}
				
			}
			
			function CboCondType_Click(obj)
			{	
				reg=/cboItems/g;
				var idtag=obj.id.replace(reg,"");
				var v=obj.value;
				var v=v.split(',')[0];
				if(v!=10)//��Ա.����
				{
					document.all(idtag+"cmdPop").style.visibility="Hidden";
					//document.all(idtag+"txtValue").readOnly=true;
				}else
				{
					document.all(idtag+"cmdPop").style.visibility="Visible";
					//document.all(idtag+"txtValue").readOnly=false;
				}
			}
			
			
					//�����ֶεĸı���ıȽϷ�
		function ChangeFieldName(obj)
		{
			var ddlOperator = document.getElementById(obj.id.replace("cboItems","cboOperate"));		
			var txtValue = document.getElementById(obj.id.replace("cboItems","txtValue"));	
			SetReadOnly(txtValue.id,false);
			while(ddlOperator.options.length>0) {ddlOperator.remove(0);}//��ձȽϷ�
			
			var SelectField = obj.options[obj.selectedIndex].value;
			var FieldType=SelectField.split(',')[1];
			var newOption;
			//alert(FieldType);
			switch(FieldType)
			{
				case "CHAR":
					newOption = document.createElement("OPTION");					
					newOption.text  = "����";
					newOption.value = "0";
					newOption.selected=true;
					ddlOperator.options.add(newOption);
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "������";
					newOption.value = "1";
					ddlOperator.options.add(newOption);	
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "��..��ͷ";   //���ڵ���
					newOption.value = "3";
					ddlOperator.options.add(newOption);	

					newOption = document.createElement("OPTION");					
					newOption.text  = "����";
					newOption.value = "6";
					ddlOperator.options.add(newOption);	
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "������";
					newOption.value = "7";
					ddlOperator.options.add(newOption);	
					
					
					break;
				case "DEPT":
					newOption = document.createElement("OPTION");					
					newOption.text  = "����";
					newOption.value = "0";
					newOption.selected=true;
					ddlOperator.options.add(newOption);
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "������";
					newOption.value = "1";
					ddlOperator.options.add(newOption);	
					
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "����";
					newOption.value = "6";
					ddlOperator.options.add(newOption);	
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "������";
					newOption.value = "7";
					ddlOperator.options.add(newOption);	
					
					SetReadOnly(txtValue.id,true);
					break;
				case "SEX":
					newOption = document.createElement("OPTION");					
					newOption.text  = "����";
					newOption.value = "0";
					newOption.selected=true;
					ddlOperator.options.add(newOption);
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "������";
					newOption.value = "1";
					ddlOperator.options.add(newOption);	
					
					
					
					
					break;
				//���������Ժ�����	
				case "INT": 
				case "FLOAT":
				case "BOOL":
					break;
				default:
					newOption = document.createElement("OPTION");					
					newOption.text  = "����";
					newOption.value = "0";
					newOption.selected=true;
					ddlOperator.options.add(newOption);
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "������";
					newOption.value = "1";
					ddlOperator.options.add(newOption);	
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "����";
					newOption.value = "2";
					ddlOperator.options.add(newOption);	
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "���ڵ���";
					newOption.value = "3";
					ddlOperator.options.add(newOption);	
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "С��";
					newOption.value = "4";
					ddlOperator.options.add(newOption);	
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "С�ڵ���";
					newOption.value = "5";
					ddlOperator.options.add(newOption);	
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "����";
					newOption.value = "6";
					ddlOperator.options.add(newOption);	
					
					newOption = document.createElement("OPTION");					
					newOption.text  = "������";
					newOption.value = "7";
					ddlOperator.options.add(newOption);	
					break;
			}			
			ddlOperator.selectedIndex=0;
		}
			
			
		</script>
	<body>
		<form id="Form1" method="post" runat="server">
			<table id="tabBoder" width="100%" class="listContent">
			    <tr height="10" class="list">
					<td><uc1:ctrtitle id="CtrTitle" runat="server"></uc1:ctrtitle></td>	
				</tr>
				<tr height="40">
					<td class="listTitle">����:<asp:textbox id="txtCondName" runat="server" Width="216px" MaxLength="25"></asp:textbox></td>
				</tr>
			</table>
		    <table cellPadding="0" width="100%" class="listContent">		
				<tr>
					<td noWrap class="listContent" width="100%">
					    <asp:datagrid id="dgCondition" runat="server" AutoGenerateColumns="False"  CssClass="Gridtable"  Width="100%" OnItemDataBound="dgCondition_ItemDataBound">
							<Columns>
								<asp:BoundColumn Visible="False" DataField="id"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="�߼���ϵ">
									<ItemTemplate>
										<asp:DropDownList id=cboRelation runat="server" Width="60" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Relation") %>'>
											<asp:ListItem Value="0" Selected="True">����</asp:ListItem>
											<asp:ListItem Value="1">����</asp:ListItem>
										</asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="������">
									<ItemTemplate>
										<asp:DropDownList id=cboItems onclick=CboCondType_Click(this) onchange="ChangeFieldName(this);" runat="server" Width="100" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "CondType") %>'>
										    <asp:ListItem Value="0,NULL" Selected="True">--ѡ��������--</asp:ListItem>
											<asp:ListItem Value="10,DEPT">[��Ա.����]</asp:ListItem>
											<asp:ListItem Value="30,CHAR">[��Ա.ְλ]</asp:ListItem>
											<asp:ListItem Value="40,SEX">[��Ա.�Ա�]</asp:ListItem>
											<asp:ListItem Value="50,CHAR">[��Ա.ѧ��]</asp:ListItem>
											<asp:ListItem Value="60,CHAR">[��Ա.��ɫ]</asp:ListItem>
										</asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="�ȽϹ�ϵ">
									<ItemTemplate>
										<asp:DropDownList id=cboOperate runat="server" Width="80" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Operate") %>'>
											<asp:ListItem Value="0" Selected="True">����</asp:ListItem>
											<asp:ListItem Value="1">������</asp:ListItem>
											<asp:ListItem Value="2">����</asp:ListItem>
											<asp:ListItem Value="3">���ڵ���</asp:ListItem>
											<asp:ListItem Value="4">С��</asp:ListItem>
											<asp:ListItem Value="5">С�ڵ���</asp:ListItem>
											<asp:ListItem Value="6">����</asp:ListItem>
											<asp:ListItem Value="7">������</asp:ListItem>
										</asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="�Ƚ�ֵ">
									<ItemStyle Wrap="true" Width="140px"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id=txtValue runat="server" Width="100" Text='<%# DataBinder.Eval(Container.DataItem, "Expression") %>'>
										</asp:TextBox>
										<input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
										<%--<asp:Button ID="cmdPop" CommandName="cmdPopCommand" runat="server" Text="..." style="width:19px; height:22px;" CssClass="btnClass2" />--%>
										<INPUT id="cmdPop" onclick="PopCondValue(this)" type="button" value="..." runat="server" class="btnClass2">
										<INPUT id=hidValue type=hidden value='<%# DataBinder.Eval(Container.DataItem, "Tag") %>' runat="server">
										
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:ButtonColumn Visible="False" Text="����" CommandName="Update">
									<ItemStyle Wrap="False"></ItemStyle>
								</asp:ButtonColumn>
								<asp:ButtonColumn Text="ɾ��" CommandName="Delete">
									<ItemStyle Wrap="False"></ItemStyle>
								</asp:ButtonColumn>
							</Columns>
						</asp:datagrid>
					</td>
					<td class="listTitle" valign="middle"><asp:button id="cmdAdd" runat="server" Text="���" onclick="cmdAdd_Click" CssClass="btnClass"></asp:button></td>
				</tr>
			</table>
			<table id="Table1" width="100%" class="listContent">
				<tr>
					<td class="listTitle" align="center"><asp:button id="cmdSave" runat="server" Text="����" onclick="cmdSave_Click" CssClass="btnClass"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;
						<INPUT id="cmdExit" onclick="javascript:window.close()" type="button" value="ȡ��" class="btnClass"><INPUT id="hidCondID" type="hidden" runat="server">
					</td>
				</tr>
			</table>
			<asp:label id="labMsg" runat="server" Font-Bold="True" ForeColor="#ff3333" ></asp:label>
		</form>
	</body>
</HTML>
<script type="text/javascript">
    $(document).ready(function(){
        $('.Gridtable tr').each(function(){
            var val = $(this).find('td:eq(1)').find('select:first').val();
            var txtval = $(this).find('td:eq(3)').find('input:eq(0)').val();
            if (val != '10,DEPT' && txtval != '') {                
                $(this).find('td:eq(3)').find('input:eq(2)').hide();
            }
        });
    });
</script>