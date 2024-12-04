<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmNormalQuery.aspx.cs" Inherits="Epower.ITSM.Web.NormalQuery.frmNormalQuery" Title="无标题页" %>

<%@ Register Src="../Controls/DeptPicker.ascx" TagName="DeptPicker" TagPrefix="uc2" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register src="../Controls/ctrDateSelectTime.ascx" tagname="ctrDateSelectTime" tagprefix="uc5" %>

<%@ Register Src="~/Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTimeV2" TagPrefix="uc5" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">
    function OpenDeleteFlow(obj)  //删除流程
    {
        var FlowID = document.getElementById(obj.id.replace("btnDelete","hidDelete")).value;
         var	value=window.showModalDialog("../Common/frmFlowDelete.aspx?FlowID=" + FlowID,window,"dialogHeight:230px;dialogWidth:320px");
        if(value!=null)
        {
            if(value[0]=="0") //成功
                event.returnValue = true;
            else
                event.returnValue = false;
        }
        else
        {
            event.returnValue = false;
        }
    }
</script>
<table id="tabMain" cellpadding='1' cellspacing='2' width='98%' border='0' class="listContent" runat=server>
<TR>
	<TD noWrap align="left" class="listTitleRight" style="height: 25px">状态</TD>
	<TD noWrap align="left" class="list" style="height: 25px">
		<asp:DropDownList id="cboStatus" runat="server" Width="120px"></asp:DropDownList>
	</TD>
	<TD noWrap class="listTitleRight" align="left" style="height: 25px">
        标 题</TD>
	<TD noWrap align="left" class="list" style="height: 25px">
		<asp:TextBox id="txtTitle" runat="server" Width="152px"></asp:TextBox>
	</TD>
</TR>
<TR>
	<TD noWrap align="left" class="listTitleRight">
        登记单位</TD>
	<TD noWrap align="left" class="list">
        <uc2:DeptPicker ID="DeptPicker1" runat="server" DeptID="0" />
    </TD>
	<TD noWrap align="left" class="listTitleRight">
        登 记 人</TD>
	<TD noWrap align="left" class="list">
        <uc3:UserPicker ID="UserPicker1" runat="server" UserID="0" />
    </TD>
</TR>
    <tr>
        <td align="left" class="listTitleRight" nowrap="nowrap">
            开始时间</td>
        <td align="left" class="list" colspan="3" nowrap="nowrap">
            <uc5:ctrDateSelectTimeV2 ID="ctrDateSelectTimeV21" runat="server" />
        </td>
    </tr>
</TABLE>
<br />
<table cellpadding="0" cellspacing="0" width="98%" border="0" >
<TR>
	<TD class="listContent">
		<asp:datagrid id="dgDispatch" runat="server" Width="100%" CellSpacing="2" CellPadding="1" BorderWidth="0px"
			BorderColor="White" AutoGenerateColumns="False"  CssClass="Gridtable"  AllowPaging="True" OnItemCreated="dgDispatch_ItemCreated" OnDeleteCommand="dgDispatch_DeleteCommand">
			<AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
            <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
		    <HeaderStyle CssClass="listTitle"></HeaderStyle>
			<Columns>
				<asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="FlowID"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="DeptID"></asp:BoundColumn>
				<asp:BoundColumn DataField="flowname" ReadOnly="True" HeaderText="标题">
                    <HeaderStyle Width="250px" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="startdate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="开始日期">
                </asp:BoundColumn>
				<asp:TemplateColumn HeaderText="流程状态">
                    <HeaderStyle Width="8%"></HeaderStyle>
                    <ItemTemplate>
                        <%#((EpowerGlobal.e_FlowStatus)Convert.ToInt32(DataBinder.Eval(Container.DataItem, "status"))) == EpowerGlobal.e_FlowStatus.efsHandle ? "<font color='blue'>正在处理</font>" : (((EpowerGlobal.e_FlowStatus)Convert.ToInt32(DataBinder.Eval(Container.DataItem, "status"))) == EpowerGlobal.e_FlowStatus.efsStop ? "<font color='red'>流程暂停</font>" 
                             : ((EpowerGlobal.e_FlowStatus)Convert.ToInt32(DataBinder.Eval(Container.DataItem, "status"))) == EpowerGlobal.e_FlowStatus.efsAbort ? "<font color='red'>流程终止</font>" : "<font color='green'>正常结束</font>")%>
                    </ItemTemplate>
                </asp:TemplateColumn>                     
                <asp:TemplateColumn HeaderText="处理">
					<HeaderStyle Width="5%"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" />
					<ItemTemplate>
						<INPUT id="CmdDeal" class="btnClass1" onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>' type="button" value='查看' runat="server" >
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" HeaderText="删除">
		            <HeaderStyle Width="5%"></HeaderStyle>
		            <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" Text="删除" CommandName="Delete" SkinID="btnClass1" OnClientClick="OpenDeleteFlow(this);" />
                        <input id="hidDelete" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "flowid")%>' />
		            </ItemTemplate>
	            </asp:TemplateColumn>
			</Columns>
			<PagerStyle Visible="False" HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF"
				Mode="NumericPages"></PagerStyle>
		</asp:datagrid></TD>
</TR>
<TR>
	<TD align="right">
		<uc1:controlpage id="ControlPageIssues" runat="server"></uc1:controlpage>
    </TD>
</TR>
</TABLE>
</asp:Content>
