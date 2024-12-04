<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmPro_ProblemSelect.aspx.cs" Inherits="Epower.ITSM.Web.ProbleForms.frmPro_ProblemSelect" Title="问题选择" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register src="../Controls/ctrDateSelectTimeV2.ascx" tagname="ctrDateSelectTime" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" language="javascript">
    //全选复选框
    function checkAll(checkAll) {
        var len = document.forms[0].elements.length;
        var cbCount = 0;
        for (i = 0; i < len; i++) {
            if (document.forms[0].elements[i].type == "checkbox") {
                if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgProblem") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                    document.forms[0].elements[i].checked = checkAll.checked;

                    cbCount += 1;
                }
            }
        }
    }
    function SelectOndbClick(sID)
    {
        var arr = new Array();
        arr[0] = sID;
        
        //window.parent.returnValue = arr;
        if(typeof(arr) !="undefined" && arr[0] !="")
        {
            window.opener.document.getElementById("<%=Opener_ClientId %>hidCustArrID").value=arr[0];
            window.opener.document.getElementById("<%=Opener_ClientId %>hidFlag").value="OK";
        }
        else
        {
            window.opener.document.getElementById("<%=Opener_ClientId %>hidFlag").value="NO";
        }
        
        top.close();
    }
</script>
<table cellpadding='1' cellspacing='2' width='100%' border='0' class="listContent">
	<tr>
		<td noWrap class="listTitle" width="15%" align="right">
            <asp:Literal ID="litProbleState" runat="server" Text="问题状态"></asp:Literal></td>
		<td class="list" align="left" width="35%">
            <uc2:ctrFlowCataDropList ID="CtrFlowProblemState" runat="server" RootID="1021" />
        </td>
		<td class="listTitle"  width="15%" align="right">
            <asp:Literal ID="litProbleType" runat="server" Text="问题类别"></asp:Literal></td>
		<td class="list" align="left" width="35%">
            <uc2:ctrFlowCataDropList ID="CataProblemType" runat="server" RootID="1006" />
        </td>
	</tr>
	<tr>
		<td noWrap class="listTitle" align="right">
            <asp:Literal ID="litProbleLevel" runat="server" Text="问题级别"></asp:Literal></td>
		<td class="list" align="left">
            <uc2:ctrFlowCataDropList ID="CataProblemLevel" runat="server" RootID="1007" />
        </td>
		<td class="listTitle" align="right">
            <asp:Literal ID="litProbleRegUserName" runat="server" Text="登记人"></asp:Literal></td>
		<td class="list" align="left">
		<asp:textbox id="txtRegUser" runat="server"></asp:textbox></td>
	</tr>
	<tr>
		<td class="listTitle" align="right">
            <asp:Literal ID="litProbleRegTime" runat="server" Text="登记时间"></asp:Literal></td>
		<td class="list" align="left">
	        <uc3:ctrDateSelectTime ID="ctrDateSelectTime1" runat="server" />
	</td>
	<td noWrap class="listTitle" align="right">
            <asp:Literal ID="litProbleSubject" runat="server" Text="标题"></asp:Literal></td>
		<td class="list" align="left" ><asp:TextBox id="txtTitle" runat="server"></asp:TextBox>
            </td>
	</tr>
</table>
<br />
<table cellpadding='1' cellspacing='2' width='100%' border='0' class="listContent">
<tr>
    <td align="right" class="listTitle">
        <asp:Button ID="btnConfirm" runat="server" Text="确  定"  OnClick="btnConfirm_Click" CssClass="btnClass" />
        <asp:Button ID="btnClose" runat="server" Text="取  消"  OnClick="btnClose_Click" CssClass="btnClass" />
    </td>
</tr>
</table>
<br />
<table cellpadding="0" cellspacing="0" width="100%" border="0" >
	<tr>
		<td class="listContent"><asp:datagrid id="dgProblem" runat="server" Width="100%" cellpadding="1" cellspacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemDataBound="dgProblem_ItemDataBound">
				<AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
			    <HeaderStyle CssClass="listTitle"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn>
					    <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
						<ItemTemplate>
							<asp:CheckBox id="chkDel" runat="server"></asp:CheckBox>
						</ItemTemplate>
						<HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
					</asp:TemplateColumn>
					<asp:BoundColumn Visible="False" DataField="Problem_ID" HeaderText="ID"></asp:BoundColumn>
					<asp:BoundColumn DataField="Problem_Title" HeaderText="标题">
						<HeaderStyle Wrap="False"></HeaderStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Problem_TypeName" HeaderText="问题类别">
						<HeaderStyle Wrap="False"></HeaderStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Problem_LevelName" HeaderText="问题级别">
						<HeaderStyle Wrap="False"></HeaderStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="RegUserName" HeaderText="登记人">
						<HeaderStyle Wrap="False"></HeaderStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="StateName" HeaderText="状态">
						<HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="True"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
					</asp:BoundColumn>
					<asp:TemplateColumn HeaderText="处理" Visible="False">
							<HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
							<ItemTemplate >
								<INPUT id="CmdDeal" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>' type="button" value='详情' runat="server" class="btnClass" >
							</ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateColumn>
					<asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="status"></asp:BoundColumn>
				</Columns>
				</asp:datagrid>
			</td>
	</tr>
	<tr>
	    <td align="right" class="listTitle"><uc1:controlpage id="ControlPage1" runat="server"></uc1:controlpage></td>
	</tr>
</table>

</asp:Content>
