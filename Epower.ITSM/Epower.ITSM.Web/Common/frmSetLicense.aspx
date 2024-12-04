<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmSetLicense.aspx.cs"
    Inherits="Epower.ITSM.Web.Common.frmSetLicense" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>E8.ITSM 许可注册页面</title>
    <link href="../App_Themes/StandardThemes/css.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 100%; height: 100%; text-align: center; vertical-align: text-bottom;
        font-size: x-large; font-weight: bold; color: Red;">
        软件许可已受限，请联系产品供应商！
    </div>
    <table id="Table2" width="500" class="listContent" align="center" style="display: none;">
        <tr>
            <td style="width: 209px" class="listTitle">
                机器码：
            </td>
            <td class="list" align="left">
                <asp:Label ID="lblServerCode" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 209px" class="listTitle">
                客户编号：
            </td>
            <td class="list" align="left">
                <asp:TextBox ID="txtCustCode" runat="server" Width="80%" CssClass="bian"></asp:TextBox><asp:Label
                    ID="rWarning" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Red">*</asp:Label>
                <asp:RequiredFieldValidator ID="rfq1" runat="server" ControlToValidate="txtCustCode"
                    ErrorMessage="客户编号不能为空！" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                <asp:Label ID="labCustCode" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 209px; height: 23px" class="listTitle">
                产品编号：
            </td>
            <td style="height: 23px" class="list" align="left">
                <asp:TextBox ID="txtProdCode" runat="server" Width="80%" CssClass="bian"></asp:TextBox><asp:Label
                    ID="rWarning1" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Red">*</asp:Label>
                <asp:RequiredFieldValidator ID="rfq2" runat="server" ControlToValidate="txtProdCode"
                    ErrorMessage="产品编号不能为空！" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                <asp:Label ID="labProdCode" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 209px; height: 28px" class="listTitle">
                许可码：
            </td>
            <td style="height: 28px" valign="middle" class="list" align="left">
                <asp:TextBox ID="txtLicense" runat="server" Width="80%" CssClass="bian"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Red">*</asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLicense"
                    ErrorMessage="许可码不能为空！" SetFocusOnError="True">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr class="StatusBarForPop">
            <td align="center" colspan="2" class="list">
                <asp:Button ID="cmdSave" runat="server" Text="确认" OnClick="cmdSave_Click" CssClass="btnClass">
                </asp:Button>&nbsp;&nbsp;
                <input onclick="javascript:window.close()" type="button" value="取消" class="btnClass">
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
    </form>
</body>
</html>
