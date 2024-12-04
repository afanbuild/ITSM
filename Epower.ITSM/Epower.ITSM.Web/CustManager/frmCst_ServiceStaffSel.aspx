<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmCst_ServiceStaffSel.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmCst_ServiceStaffSel"
    Title="����ʦѡ��" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
        function CancelconfirmMast()     //ȡ��
        {
            if (confirm("ȷ��Ҫȡ����?")) {
                top.close();
            }
            event.returnValue = false;
        }
        function doubleSelect(jsonstr)
        {
            window.parent.returnValue = jsonstr;
            top.close();
        }        
    </script>
   
    <table runat="server" width='98%' cellpadding="2" cellspacing="0" class='listContent'>
        <tr>
            <td class='listTitleRight' style='width: 12%;' colspan="3">
                ����λ
            </td>
            <td class='list' colspan="3">
                <asp:DropDownList ID="ddltMastCustID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddltMastCustID_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgCst_ServiceStaff" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    CellPadding="1" CellSpacing="2" BorderWidth="0px" OnItemCommand="dgCst_ServiceStaff_ItemCommand"
                    OnItemCreated="dgCst_ServiceStaff_ItemCreated" OnItemDataBound="dgCst_ServiceStaff_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>                       
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="false"></asp:BoundColumn>                        
                        <asp:BoundColumn DataField='Name' HeaderText='����ʦ����'></asp:BoundColumn>
                        <asp:BoundColumn DataField='BlongDeptName' HeaderText='����λ'></asp:BoundColumn>
                        <asp:BoundColumn DataField='UserName' HeaderText=' ��Ӧ�û�'></asp:BoundColumn>
                        <asp:BoundColumn DataField="JoinDate" HeaderText=" ��ְʱ��" DataFormatString="{0:d}">
                        </asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="ѡ��" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="44"></HeaderStyle>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc2:ControlPageFoot ID="cpServiceStaff" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
