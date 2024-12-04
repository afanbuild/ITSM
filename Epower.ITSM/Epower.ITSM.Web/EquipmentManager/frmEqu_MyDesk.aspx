<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmEqu_MyDesk.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_MyDesk" Title="无标题页" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">
var openobj = window;
if(typeof(window.dialogArguments) == "object")
{
    openobj =  window.dialogArguments;
}
//设备服务记录
function SelectService(obj) 
{
    var lngID = document.getElementById(obj.id.replace("CmdService","hidID")).value;
	openobj.open("../AppForms/frmIssueList.aspx?NewWin=true&ID=0&EquID=" + lngID ,'','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');
	event.returnValue = false;
}
</script>
<table id="Table1" width="100%" class="listContent"  cellpadding=0>
<tr>
    <td align="left">
        <asp:DataGrid ID="dgUserInfo" runat="server" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgUserInfo_ItemCommand"
        PageSize="50" Width="100%" OnItemCreated="dgUserInfo_ItemCreated">
        <Columns>
            <asp:BoundColumn DataField="id" HeaderText="id" Visible="False"></asp:BoundColumn>
            <asp:BoundColumn DataField="Name" HeaderText="资产名称">
                <HeaderStyle Wrap="False" />
            </asp:BoundColumn>
            <asp:BoundColumn DataField="CatalogName" HeaderText="分类">
                <HeaderStyle Wrap="False" />
            </asp:BoundColumn>
            <asp:BoundColumn DataField="SerialNumber" HeaderText="SN">
                <HeaderStyle Wrap="False" />
            </asp:BoundColumn>
            <asp:BoundColumn DataField="breed" HeaderText="品牌"></asp:BoundColumn>
            <asp:BoundColumn DataField="model" HeaderText="型号"></asp:BoundColumn>
            <asp:BoundColumn DataField="Code" HeaderText="代码" Visible="false">
                <HeaderStyle Wrap="False" />
            </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="详情">
                <ItemTemplate>
                    <asp:Button ID="lnkselect" runat="server" CommandName="look" SkinID="btnClass1" Text="详情" />
                </ItemTemplate>
                <HeaderStyle Width="5%" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateColumn>
            <asp:TemplateColumn  HeaderText="事件记录">
				<ItemTemplate >
				    <input type="hidden" runat="server" id="hidID" value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
				    <asp:Button ID="CmdService" Width=60 runat="server" Text="事件记录" OnClientClick="SelectService(this);" SkinID="btnClass1" CausesValidation="false" />						    
				</ItemTemplate>
				<HeaderStyle Width="64" HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" />
			</asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    </td>
</tr>
</TABLE>
</asp:Content>
