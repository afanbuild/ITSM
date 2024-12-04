<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.CtrOperates" Codebehind="CtrOperates.ascx.cs" %>
<script language="javascript">
	function chkSelectAllLeader_Click(obj)
	{
		reg=/chkSelectAllLeader/g;
		var idhead=obj.id.replace(reg,"");
		var idtag=idhead.split('__')[0];
		//alert(idtag);
		// vs2005 机制不同
		idtag = idtag.substr(0,idtag.length -4);
		for(i=0;i<document.all.length;i++)
		{
			var sid=document.all(i).id;
			if(document.all(i).type=='checkbox')
			{
			   // alert(sid + " ---- " + idtag + "  ----  " + idtag.length);
				if(sid!="")
				{
					if(sid.substr(0,idtag.length)==idtag)
					{
						document.all(i).checked=obj.checked;
					}
				}
			}
		}
	}
</script>
<asp:datalist id="dlLeaders" RepeatColumns="4" BorderStyle="None" BorderWidth="1px" RepeatDirection="Horizontal"
	runat="server">
	<HeaderTemplate>
		操作项<INPUT id="chkSelectAllLeader" onclick="chkSelectAllLeader_Click(this)" type="checkbox"
			name="chkSelectAllLeader" runat="server">全选
	</HeaderTemplate>
	<ItemStyle Wrap="False"></ItemStyle>
	<ItemTemplate>
		<asp:CheckBox id=chkSelect runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OpName") %>'>
		</asp:CheckBox><INPUT id=hidLeaderID type=hidden value='<%# DataBinder.Eval(Container.DataItem, "OperateID")%>' runat="server">
		<INPUT id=hidLeaderName type=hidden value='<%# DataBinder.Eval(Container.DataItem, "OpName")%>' runat="server">
	</ItemTemplate>
	<HeaderStyle ForeColor="Green"></HeaderStyle>
</asp:datalist>
