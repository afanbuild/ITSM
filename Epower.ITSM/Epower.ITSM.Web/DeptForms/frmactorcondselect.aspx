<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmActorCondSelect" Codebehind="frmActorCondSelect.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>frmActorCondSelect</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript" src="../Js/Common.js"></script>
		
	</HEAD>
	<script language="javascript">
			function EditCond(CondID)
			{
				//OpenNoBarWindow("frmActorCondEdit.aspx?CondID="+CondID,600,400);
				window.open("frmActorCondEdit.aspx?CondID="+CondID,500,300);
			}
			
			function OnlySelectOne(obj)
			{
				for(i=0;i<document.all.length;i++)
				{
					var sid=document.all(i).id;
					if(sid!="")
					{
						if(sid.substr(sid.length-10,10)=="rbSelected")
						{
							document.all(i).checked=false;
						}
					}
				}
				obj.checked=true;
				
				reg=/rbSelected/g;
				idtag=obj.id.replace(reg,"");
				
				document.all.hidCondId_Name.value=document.all(idtag+"labCondID").innerText+"@"+document.all(idtag+"labCondName").innerText;
				
			}
			
			function cmdOk_Click()
			{
			//	window.returnValue=document.all.hidCondId_Name.value;
			var value=document.all.hidCondId_Name.value;
			
			if (value != null) {
	                    if (value.length > 1) {
	                      var  arr = value.split("@");
	                        
	                         window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value=arr[1];
	                        window.opener.document.getElementById("<%=Opener_ClientId %>hidObjectID").value=arr[0];
	                        
	                    }
	                }
			    
				//alert(document.all.hidCondId_Name.value);
				window.close();
			}
			
			function cmdExit_Click()
			{
				window.returnValue="@";
				window.close();
			}
			
			function DbSelectclick(ID,Name)
			{
			  //  window.returnValue=ID + "@" + Name;
			   var value=ID+"@"+name;
			   if(value !=null)
			   {
			     if(value.length>1)
			     {
			        var arr=value.split("@");
			        window.opener.document.getElementById("<%=Opener_ClientId %>txtObjectName").value =arr[1];
			        window.opener.document.getElementById("<%=Opener_ClientId %>hidObjectID").value=arr[0];
			        
			     }
			   }
				//alert(document.all.hidCondId_Name.value);
				window.close();
			}
		</script>
	<body>
		<form id="Form1" method="post" runat="server">
			<table id="tbmain" cellpadding="0" cellspacing="0" border="0" Width="100%">
				<tr>
					<td width="30">
					</td>
					<td>
						<uc1:CtrTitle id="CtrTitle1" runat="server"></uc1:CtrTitle>
					</td>
				</tr>
				<tr>
					<td>
					</td>
					<td>&nbsp;
					</td>
				</tr>
				<tr>
					<td>
					</td>
					<td>
						<asp:DataGrid id="dgActorConds" runat="server" AutoGenerateColumns="False"  CssClass="Gridtable"  Width="100%" OnItemCommand="dgActorConds_ItemCommand" OnItemDataBound="dgActorConds_ItemDataBound">
							<Columns>
								<asp:TemplateColumn HeaderText="选择">
									<ItemTemplate>
										<asp:RadioButton id="rbSelected" runat="server" onclick="OnlySelectOne(this)"></asp:RadioButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false">
									<ItemTemplate>
										<asp:HyperLink id=hl1 runat="server" NavigateUrl="#" onclick='<%#"javascript:EditCond("+DataBinder.Eval(Container.DataItem, "CondID")+")"%>'>查看</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="ID">
									<ItemTemplate>
										<asp:Label id="labCondID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CondID") %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="名称">
									<ItemTemplate>
										<asp:Label id="labCondName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CondName") %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:DataGrid>
					</td>
				</tr>
				<tr height="40">
					<td align="center" colspan="2" class="list"><INPUT id="cmdOK" type="button" value="确定" onclick="cmdOk_Click()"  class="btnClass"><INPUT id="cmdExit" type="button" value="取消" onclick="cmdExit_Click()" class="btnClass">
					</td>
				</tr>
			</table>
			<input type="hidden" id="hidCondId_Name">
		</form>
	</body>
</HTML>
