<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrSetUserOtherRight.ascx.cs" Inherits="Epower.ITSM.Web.Controls.ctrSetUserOtherRight" %>
<script language="javascript">
//全选复选框
function checkAll(checkAll)
{			  
	var len = document.forms[0].elements.length;
	var cbCount = 0;
	for (i=0;i < len;i++)
	{
		if (document.forms[0].elements[i].type == "checkbox")
		{
			if (document.forms[0].elements[i].name.indexOf("chkSelect") != -1 && 
				document.forms[0].elements[i].name.indexOf("dgEA_ExtendRights") != -1 &&
				document.forms[0].elements[i].disabled == false)
			{
				document.forms[0].elements[i].checked = checkAll.checked;

				cbCount += 1;
			}
		}
	}		
} 
function Show_ModifyWindow(obj)
{
	reg=/cmdModify/g;
	var idtag=obj.id.replace(reg,"");
	var rid=document.all(idtag+"labRightID").innerText;
	var url ="../mydestop/frmeditright.aspx?rightid="+rid + "&OperateID=" + <%=OperateID%> + "&OperateType=" + <%=OperateType%>+"&Opener_ClientId=<%=btnRefresh.ClientID %>";
	window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=400,height=300px,left=150,top=100");
}

function Show_NewWindow(obj)
{
    var hidCatalogID= document.getElementById("hidCatalogID").value;
    if(hidCatalogID=="" || hidCatalogID.length==0)
    {
         alert("资产类别记录未保存,无法给他新增访问权限！");
         return false;
    }
    else
    {
        var url="../mydestop/frmeditright.aspx?rightid=0" + "&OperateID=" + <%=OperateID%> + "&OperateType=" + <%=OperateType%>+"&Opener_ClientId=<%=btnRefresh.ClientID %>";
        //(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=400,height=300px,left=150,top=100");
        window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=400,height=300px,left=150,top=100");
        return true;
    }
}

function delete_confirmRight()     //删除前执行脚本
{
      var hidCatalogID= document.getElementById("hidCatalogID").value;
    if(hidCatalogID=="" || hidCatalogID.length==0)
    {
         alert("资产类别记录未保存,无法给他删除访问权限的功能！");
         return false;
    }
    else
    {
        event.returnValue =confirm("确认要删除吗?");
        return true;
    }
}
</script>

<asp:Button ID="btnRefresh" runat="server" Text="刷新页面" 
    onclick="btnRefresh_Click"  style="display:none;"/>

<table cellpadding="0"  cellspacing="0" width="100%" align="center" border="0">
	<tr>
		<td align="center"  class="listContent">
			<asp:datagrid id="dgEA_ExtendRights" runat="server" Width="100%" cellpadding="1" cellspacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCreated="dgEA_ExtendRights_ItemCreated">
				<AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			    <HeaderStyle CssClass="listTitle"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Left">
					    <HeaderTemplate>
						    <asp:CheckBox id="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
						    <asp:Button ID="btnDelete" runat="server" SkinID="btnClass1" Text="删除" OnClick="btnDelete_Click" OnClientClick="return delete_confirmRight()" />
						</HeaderTemplate>
						<ItemTemplate>
							<asp:CheckBox id="chkSelect" runat="server"></asp:CheckBox>
						</ItemTemplate>
						<HeaderStyle Width="10%" Wrap=false></HeaderStyle>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="权限ID">
						<HeaderStyle Width="10%"></HeaderStyle>
						<ItemTemplate>
							<asp:Label ID="labRightID" Runat=server Text='<%#DataBinder.Eval(Container.DataItem, "RightID")%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField='ObjectType' HeaderText='授权对象类型'></asp:BoundColumn>
					<asp:BoundColumn DataField='ObjectName' HeaderText='授权对象'></asp:BoundColumn>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="center">
					    <HeaderTemplate>
						    <asp:Button ID="cmdAdd" runat="server" OnClientClick="return Show_NewWindow(this)" SkinID="btnClass1" Text="新增" />
						</HeaderTemplate>
						<ItemTemplate>
						    <asp:Button ID="cmdModify" runat="server" OnClientClick="Show_ModifyWindow(this)" SkinID="btnClass1" Text="修改" />
						</ItemTemplate>
						<HeaderStyle Width="44"></HeaderStyle>
					</asp:TemplateColumn>
				</Columns>
				</asp:datagrid>
			</td>
	</tr>
</table>