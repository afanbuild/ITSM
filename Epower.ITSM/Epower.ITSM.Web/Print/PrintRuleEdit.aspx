<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="PrintRuleEdit.aspx.cs" Inherits="Epower.ITSM.Web.Print.PrintRuleEdit"
    Title="ģ����Ϣ�༭" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/CustSchemeCtr.ascx" TagName="CustSchemeCtr" TagPrefix="uc3" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc4" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc5" %>
<%@ Register Src="../Controls/CtrFlowNumeric.ascx" TagName="CtrFlowNumeric" TagPrefix="uc7" %>
<%@ Register Src="../Controls/UserPickerMult.ascx" TagName="UserPickerMult" TagPrefix="uc9" %>

<%@ Register src="../Controls/UEditor.ascx" tagname="UEditor" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 98%" class="listContent" align="center" runat="server" id="Table2">
        <tr id="ShowRefUser" runat="server">
            <td class="listTitleRight " style="width: 12%">
                <asp:Literal ID="LitRuleName" runat="server" Text="��������"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <uc4:CtrFlowFormText ID="CtrRuleName" runat="server" MustInput="true" TextToolTip="��������"
                    MaxLength="50" />
            </td>
            <td class="listTitleRight " style="width: 12%">
                <asp:Literal ID="litStatus" runat="server" Text="�Ƿ���Ч"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="ddlStatus" runat="server" Width="152px">
                    <asp:ListItem Text="����" Value="0"></asp:ListItem>
                    <asp:ListItem Text="����" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight " style="width: 12%">
                Ӧ������
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="cboApp" Width="152px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboApp_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="listTitleRight " style="width: 12%">
                ����ģ������
            </td>
            <td class="list" style="width: 12%">
                <asp:DropDownList ID="cboFlowModel" Width="152px" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight " style="width: 12%">
                <asp:Literal ID="LitRemark1" runat="server" Text="��ע"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:TextBox ID="txtRemark" runat="server" Width="90%" Rows="3" TextMode="MultiLine"
                    onblur="MaxLength(this,200,'��ע���ȳ����޶�����:');"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight " style="width: 12%">
                <asp:Literal ID="Literal2" runat="server" Text="����˵��"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <div id="divDesc" runat="server">
                </div>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight " style="width: 12%">
                <asp:Literal ID="Literal1" runat="server" Text="��ӡģ������"></asp:Literal>
            </td>
            <td class="list" colspan="3">                
                <uc1:UEditor ID="UEMailContent" runat="server" UEditorFrameWidth="900" />                
            </td>
        </tr>
        <tr>
            <td class="listTitleRight " style="width: 12%">
                <asp:Literal ID="Literal3" runat="server" Text="�������"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:DropDownList ID="drIsProcess" runat="server" Width="152px">
                    <asp:ListItem Value="0">��</asp:ListItem>
                    <asp:ListItem Value="1">��</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</asp:Content>
