<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMulAddUser.aspx.cs"
    Inherits="Epower.ITSM.Web.DeptForms.frmMulAddUser" %>

<%@ Register Src="../Controls/DeptPickerMult.ascx" TagName="DeptPickerMult" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ctrtitle.ascx" TagName="ctrtitle" TagPrefix="uc1" %>
<%@ Register Src="../Controls/UserPickerMult.ascx" TagName="UserPickerMult" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>批量添加成员</title>
</head>
<body>
    <form id="form1" runat="server">
    <table class="listContent" width="100%">
        <tr>
            <td class="list" align="center">
                <uc1:ctrtitle ID="Ctrtitle1" runat="server" Title="批量添加成员" />
            </td>
        </tr>
    </table>
    <table class="listContent" width="100%">
        <tr runat="server" id="truser">
            <td class="listTitle" width="20%">
                用户选择：
            </td>
            <td class="list" width="75%">
                <uc2:UserPickerMult ID="UserPickerMult1" runat="server" IsLimit="true" />
            </td>
        </tr>
        <tr runat="server" id="trdept">
            <td class="listTitle" width="20%">
                部门选择：
            </td>
            <td class="list" width="75%">
                <uc3:DeptPickerMult ID="DeptPickerMult1" runat="server" IsLimit="true" />
            </td>
        </tr>
    </table>
    <table class="listContent" width="100%">
        <tr>
            <td class="list" align="center">
                <asp:Button ID="btnConfirm" runat="server" Text="保 存" OnClick="btnConfirm_Click" />
                &nbsp;&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="取 消" OnClientClick="window.close();event.returnValue = false;" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
