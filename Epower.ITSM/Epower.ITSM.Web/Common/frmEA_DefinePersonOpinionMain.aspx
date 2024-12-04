<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEA_DefinePersonOpinionMain.aspx.cs" Inherits="Epower.ITSM.Web.Common.frmEA_DefinePersonOpinionMain"
    Title="快速意见管理" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="uc3" TagName="CtrFlowFormText" Src="../Controls/CtrFlowFormText.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style='width: 98%' class='listContent'>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                快速意见：
            </td>
            <td class='list'>
                <uc3:CtrFlowFormText ID='CtrFlowName' runat='server' TextToolTip='快速意见' MustInput='true'
                    MaxLength="200" />
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0">
        <tr>
            <td align="center" class="listContent">
                <asp:DataGrid ID="dgEA_DefinePersonOpinion" runat="server" Width="100%" CellPadding="1"
                    CellSpacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCreated="dgEA_DefinePersonOpinion_ItemCreated"
                    OnItemDataBound="dgEA_DefinePersonOpinion_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
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
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Name' HeaderText='快速意见'></asp:BoundColumn>
                        <asp:BoundColumn DataField='UserName' HeaderText='创建人'></asp:BoundColumn>
                        <asp:BoundColumn DataField='CreateTime' HeaderText='创建时间'></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
    //全选复选框
    function checkAll(checkAll) {
        var len = document.forms[0].elements.length;
        var cbCount = 0;
        for (i = 0; i < len; i++) {
            if (document.forms[0].elements[i].type == "checkbox") {
                if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgEA_DefinePersonOpinion") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                    document.forms[0].elements[i].checked = checkAll.checked;

                    cbCount += 1;
                }
            }
        }
    } 
    </script>

</asp:Content>
