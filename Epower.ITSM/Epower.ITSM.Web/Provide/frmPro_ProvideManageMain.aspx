<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmPro_ProvideManageMain.aspx.cs" Inherits="Epower.ITSM.Web.Provide.frmPro_ProvideManageMain"
    Title="供应商管理" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
        //全选复选框
        function checkAll(checkAll) {
            var len = document.forms[0].elements.length;
            var cbCount = 0;
            for (i = 0; i < len; i++) {
                if (document.forms[0].elements[i].type == "checkbox") {
                    if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgPro_ProvideManage") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                        document.forms[0].elements[i].checked = checkAll.checked;

                        cbCount += 1;
                    }
                }
            }
        }

        function ServerOndblclick(value1, value2) {
            var arr = new Array();
            // ID
            arr[0] = value1;
            // 名称
            arr[1] = value2;
            window.parent.returnValue = arr;
            // 关闭窗口
            top.close();
        }
                    
    </script>

    <table width='98%' cellpadding="2" cellspacing="0" class="listContent">
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                供应商名称
            </td>
            <td class='list' style="width: 35%">
                <asp:TextBox ID='txtName' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight' style='width: 12%;'>
                供应商代码
            </td>
            <td class='list'>
                <asp:TextBox ID='txtCode' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                联系人
            </td>
            <td class='list'>
                <asp:TextBox ID='txtContract' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight'>
                联系人电话
            </td>
            <td class='list'>
                <asp:TextBox ID='txtContractTel' runat='server'></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" width="98%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgPro_ProvideManage" runat="server" Width="100%" CellPadding="1"
                    CellSpacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgPro_ProvideManage_ItemCommand"
                    OnItemDataBound="dgPro_ProvideManage_ItemDataBound" OnItemCreated="dgPro_ProvideManage_ItemCreated">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitleRight"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="False"></asp:BoundColumn>
                        <asp:HyperLinkColumn DataNavigateUrlField="id" DataTextField="name" HeaderText="供应商名称"
                            Target="_self" DataNavigateUrlFormatString="frmPro_ProvideManageEdit.aspx?ReadyOnly=1&amp;id={0}">
                        </asp:HyperLinkColumn>
                        <asp:BoundColumn DataField="Code" HeaderText="供应商代码"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Contract" HeaderText="联系人"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ContractTel" HeaderText="联系人电话"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="修改">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="修改" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="44" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='Name' HeaderText='Name' Visible="False"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="right">
                <uc2:ControlPageFoot ID="cpServiceStaff" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
