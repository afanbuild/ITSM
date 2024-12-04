<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmCalender.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmCalender"
    Title=" 节假日管理" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
function PopSelectDialog()
{
	switch(document.all.<%=dpdObjectType.ClientID %>.value)
	{
		case "2":   //部门
			SelectPDept();
			break;
			
		case "3":  //人员
			SelectStaff();
			break;
			
	}
}
function SelectPDept()
{
	var value=window.showModalDialog("../mydestop/frmpopdept.aspx");
	if(value==null)
	{
	    document.all.<%=hidObjectName.ClientID%>.value = "";
		document.all.<%=txtObjectName.ClientID%>.value = "";
		document.all.<%=hidObjectID.ClientID%>.value = "";
	    return;
	}
	if(value.length>1)
	{
		arr=value.split("@");
		document.all.<%=txtObjectName.ClientID%>.value=arr[1]+"(" +arr[0] + ")";
		document.all.<%=hidObjectID.ClientID%>.value=arr[0];
		document.all.<%=hidObjectName.ClientID%>.value = arr[1]+"(" +arr[0] + ")";
	}
	else
	{
		document.all.<%=hidObjectName.ClientID%>.value = "";
		document.all.<%=txtObjectName.ClientID%>.value = "";
		document.all.<%=hidObjectID.ClientID%>.value = "";
	}
}
function SelectStaff()
{
	var value=window.showModalDialog("../mydestop/frmSelectPerson.htm");
	if(value==null)
	{
	    document.all.<%=hidObjectName.ClientID%>.value = "";
		document.all.<%=txtObjectName.ClientID%>.value = "";
		document.all.<%=hidObjectID.ClientID%>.value = "";
	    return;	
	}
	if(value.length>1)
	{
		arr=value.split("@");
		document.all.<%=txtObjectName.ClientID%>.value=arr[1]+"(" +arr[0] + ")";
		document.all.<%=hidObjectID.ClientID%>.value=arr[0];
		document.all.<%=hidObjectName.ClientID%>.value = arr[1]+"(" +arr[0] + ")";
	}
	else
	{
		document.all.<%=hidObjectName.ClientID%>.value = "";
		document.all.<%=txtObjectName.ClientID%>.value = "";
		document.all.<%=hidObjectID.ClientID%>.value = "";
		
	}
}

function checkAll(objectCheck) {
            var demo = document.getElementById('<%=dgEa_DefineMainPage.ClientID%>');
            var gg = demo.getElementsByTagName('INPUT');
            for (i = 0; i < gg.length; i++) {
                if (gg[i].type == "checkbox" && gg[i].name.indexOf("chkDel") != -1) {
                    gg[i].checked = objectCheck.checked;
                }
            }
        }
    </script>

    <table id="tbMain" width="98%" class="listContent" cellpadding="2" cellspacing="0" align="center">
        <tr>
            <td style="width: 12%" class="listTitleRight">
                对象类型：
            </td>
            <td class="list" style="width: 82%">
                <asp:DropDownList ID="dpdObjectType" runat="server" Width="152px" AutoPostBack="True"
                    OnSelectedIndexChanged="dpdObjectType_SelectedIndexChanged">
                    <asp:ListItem Value="-1" Selected="true">--全部--</asp:ListItem>
                    <asp:ListItem Value="0">全局</asp:ListItem>
                    <asp:ListItem Value="1">机构</asp:ListItem>
                    <asp:ListItem Value="2">部门</asp:ListItem>
                    <asp:ListItem Value="3">人员</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblTitle" runat="server" Text="对象名称：" Visible="false"></asp:Label>
                <asp:TextBox ID="txtObjectName" runat="server" Width="88px" Visible="false" ReadOnly="true"></asp:TextBox>
                <input id="hidObjectName" runat="server" type="hidden" />
                <input id="hidObjectID" runat="server" type="hidden" />
                <input id="cmdPop" onclick="PopSelectDialog()" type="button" value="..." class="btnClass"
                    runat="server" visible="false">
                <asp:DropDownList ID="ddltObjectID" runat="server" Visible="false">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" width="98%" align="center" class="listContent">
        <tr>
            <td align="center" class="listContent">
                <asp:DataGrid ID="dgEa_DefineMainPage" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCommand="dgEa_DefineMainPage_ItemCommand" OnItemCreated="dgEa_DefineMainPage_ItemCreated">
                    <Columns>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField='ObjectType' HeaderText='对象类型'></asp:BoundColumn>
                        <asp:BoundColumn DataField='ObjectName' HeaderText='对象名称'></asp:BoundColumn>
                        <asp:BoundColumn DataField='caldate' HeaderText='假期时间'></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="修改">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="修改" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="44px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right" class="listTitle">
                <uc1:ControlPage ID="ControlPage1" runat="server"></uc1:ControlPage>
            </td>
        </tr>
    </table>
</asp:Content>
