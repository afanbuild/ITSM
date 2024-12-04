<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmPro_ProblemAnalyseMain.aspx.cs" Inherits="Epower.ITSM.Web.ProbleForms.frmPro_ProblemAnalyseMain" Title="案例分析" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">	
    //选择问题
    function SelectProblems()
	{
	    var newDateObj = new Date()
	    var sparamvalue =  newDateObj.getMinutes() + newDateObj.getMilliseconds();
		var features =
		'dialogWidth:800px;' +
		'dialogHeight:500px;' +
		'directories:no; localtion:no; menubar:no; status=no; toolbar=no;scroll:no;resizable=yes';

		var url='frmPro_ProblemSelect.aspx?pDate='+sparamvalue+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
		window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=600");
	}
	function CheckIsnum(obj,strDisplay,stype)
    {
        var svalue = eval(obj.value);
        var soldvalue = 0;
        soldvalue = eval(document.getElementById(obj.id.replace("txt","hid")).value); 
        if (isNaN(svalue))
        {
            alert(strDisplay);
            obj.focus(); 
            obj.select(obj.value.length);
        }
        else
        {
            var oldvalue; 
            if(stype=="1")
            {
                oldvalue= eval(document.all.<%=lblScale.ClientID%>.innerText);
                document.all.<%=lblScale.ClientID%>.innerText = (oldvalue - soldvalue + svalue).toFixed(2);
            }
            else if(stype=="2")
            {
                oldvalue= eval(document.all.<%=lblEffect.ClientID%>.innerText);
                document.all.<%=lblEffect.ClientID%>.innerText = (oldvalue - soldvalue + svalue).toFixed(2);
            }
            else
            {
                oldvalue= eval(document.all.<%=lblStress.ClientID%>.innerText);
                document.all.<%=lblStress.ClientID%>.innerText = (oldvalue - soldvalue + svalue).toFixed(2);
            }
            document.getElementById(obj.id.replace("txt","hid")).value = svalue;
        }
    }
    
    function CheckValue()
    {
        if(eval(document.all.<%=lblScale.ClientID%>.innerText)!='100')
        {
            alert("权重必需为100%！");
            return false;
        }
        if(eval(document.all.<%=lblEffect.ClientID%>.innerText)!='100')
        {
            alert("影响度必需为100%！");
            return false;
        }
        if(eval(document.all.<%=lblStress.ClientID%>.innerText)!='100')
        {
            alert("紧迫性必需为100%！");
            return false;
        }
    }
</script>

<input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
<input id="hidCustArrID" runat="server" type="hidden"/>
<input id="hidCustArrIDold" runat="server" type="hidden"/>
<input id="hidFlag" runat="server" type="hidden" />
<table cellpadding='1' cellspacing='2' width='100%' border='0' class='listContent'>
<tr>
    <td align="right" class="listTitle">
        <asp:Button ID="btnAdd" runat="server" Text="新  增"  OnClientClick="SelectProblems();"  CssClass="btnClass"/>
       <%-- zxl 增加了一个隐藏按钮--%>
        <asp:Button ID="hidBtnAdd" runat="server" Text="aaaa" style="display:none;" OnClick="btnAdd_Click" /> <%-- OnClick="btnAdd_Click"--%>
        <asp:Button ID="btnSave" runat="server" Text="保  存"  OnClick="btnSave_Click" OnClientClick="return CheckValue();" CssClass="btnClass" />
        
    </td>
</tr>
</table>
<table cellpadding='1' cellspacing='2' width='100%' border='0' class='listContent'>
<tr>
<td class='listTitle'  align='right' style='width:15%;'>案例标题				
</td>		
<td class='list' colspan="3">
	<asp:Label ID='lblEventTitle' runat='server'></asp:Label>			
</td>	
</tr>
</table>
<br />
<table cellpadding="0"  cellspacing="0" width="100%" align="center" border="0">
	<tr>
		<td align="center"  class="listContent" colspan="6">
			<asp:datagrid id="dgPro_ProblemAnalyse" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"  cellpadding="1" cellspacing="2" BorderWidth="0px" OnItemCommand="dgPro_ProblemAnalyse_ItemCommand">
				<AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			    <HeaderStyle CssClass="listTitle"></HeaderStyle>
			    <FooterStyle CssClass="listTitle" />
				<Columns>
					<asp:BoundColumn DataField='Problem_FlowID' HeaderText='ID' Visible="false"></asp:BoundColumn>
					<asp:BoundColumn DataField='Problem_FlowID' HeaderText='Problem_FlowID' Visible="false"></asp:BoundColumn>
					<asp:BoundColumn DataField='Event_FlowID' HeaderText='Event_FlowID' Visible="false" ></asp:BoundColumn>
					<asp:BoundColumn DataField='Event_Title' HeaderText='事件标题' Visible="false"></asp:BoundColumn>
					<asp:BoundColumn DataField='Problem_Title' HeaderText='问题标题' Visible="false"></asp:BoundColumn>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="right" HeaderText="问题标题">
						<ItemTemplate>
                            <asp:Label ID="lblProblem_Title" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Problem_Title")%>'></asp:Label>
						</ItemTemplate>
						<HeaderStyle HorizontalAlign="center" Width="30%"></HeaderStyle>
					</asp:TemplateColumn>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" HeaderText="关联权重（%）">
						<ItemTemplate>
                            <asp:TextBox ID="txtScale" Width="60" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Scale")%>' onblur="CheckIsnum(this,'权重必须为数值！','1');" style="ime-mode:Disabled" onkeydown="NumberInput('1');"></asp:TextBox>
                            <input id="hidScale"  type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Scale")%>'/>
						</ItemTemplate>
						<HeaderStyle HorizontalAlign="center" Width="80"></HeaderStyle>
					</asp:TemplateColumn>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" HeaderText="关联影响度（%）">
						<ItemTemplate>
                            <asp:TextBox ID="txtEffect" Width="60" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Effect")%>' onblur="CheckIsnum(this,'影响度必须为数值！','2');" style="ime-mode:Disabled" onkeydown="NumberInput('1');"></asp:TextBox>
                            <input id="hidEffect"  type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Effect")%>'/>
						</ItemTemplate>
						<HeaderStyle HorizontalAlign="center"  Width="80"></HeaderStyle>
					</asp:TemplateColumn>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" HeaderText="关联紧迫性（%）">
						<ItemTemplate>
                            <asp:TextBox ID="txtStress" Width="60" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Stress")%>' onblur="CheckIsnum(this,'紧迫性必须为数值！','3');" style="ime-mode:Disabled" onkeydown="NumberInput('1');"></asp:TextBox>
                            <input id="hidStress"  type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Stress")%>'/>
						</ItemTemplate>
						<HeaderStyle HorizontalAlign="center"  Width="80"></HeaderStyle>
					</asp:TemplateColumn>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="备注">
						<ItemTemplate>
                            <asp:TextBox ID="txtRemark" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Remark")%>' Width="90%"></asp:TextBox>
						</ItemTemplate>
						<HeaderStyle HorizontalAlign="center"></HeaderStyle>
					</asp:TemplateColumn>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center">
						<HeaderStyle Width="8%" VerticalAlign='Top'></HeaderStyle>
						<ItemTemplate>
							<asp:button id="btndelete" CommandName="Delete" runat="server" Text="删除" CausesValidation="False" SkinID="btnClass1"></asp:button>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				</asp:datagrid>
			</td>
	</tr>
	<tr>
	    <td class="listTitle" Width="30%" align="right"><asp:Label ID="lblAll" runat="server" Text='合计:'></asp:Label></td>
	    <td class="listTitle" align="left" colspan="3">&nbsp;关联权重<asp:Label ID="lblScale" runat="server" Text="0"></asp:Label>%&nbsp;&nbsp;
	    关联影响度<asp:Label ID="lblEffect" runat="server" Text="0"></asp:Label>%&nbsp;&nbsp;
	    关联紧迫性<asp:Label ID="lblStress" runat="server" Text="0"></asp:Label>%</td>
	    <td class="listTitle" >&nbsp; 
        </td>
	    <td class="listTitle" Width="8%"> &nbsp;</td>
	</tr>
</table>
</asp:Content>
