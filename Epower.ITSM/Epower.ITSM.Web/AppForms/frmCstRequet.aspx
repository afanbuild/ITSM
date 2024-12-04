<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCstRequet.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmCstRequet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>请求提交</title>
    <link href="../App_Themes/NewOldMainPage/css.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0">
        <tr runat="server" id="trRequest">
            <td align="center">
                <table width="960" class='listContent'>
                    <tr>
                        <td align="center" style="height: 40px" valign="middle" colspan="2" class="list">
                            <asp:Label ID="Label1" runat="server" Text="欢迎提交服务请求，我们将尽快为您处理"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class='listTitle' align='right' style='width: 30%;'>
                            公司名/姓名<span style="color: #ff0000">*</span>
                        </td>
                        <td class='list' align='left'>
                            <asp:TextBox ID='txtContract' runat='server' Width="240px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtContract"
                                Display="Dynamic" ErrorMessage="联系人不能为空！" SetFocusOnError="True">联系人不能为空！</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="listTitle" style="width: 30%">
                            联系电话<span style="color: #ff0000">*</span>
                        </td>
                        <td align="left" class="list">
                            <asp:TextBox ID="txtCTel" runat="server" Width="240px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCTel"
                                ErrorMessage="联系电话不能为空！方便进一步联系您" SetFocusOnError="True" Display="Dynamic">联系电话不能为空！方便进一步联系您</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class='listTitle' align='right' style="width: 30%; height: 119px;">
                            请求内容<span style="color: #ff0000">*</span>
                        </td>
                        <td class='list' align='left' style="height: 119px">
                            <asp:TextBox ID='txtContent' runat='server' Width="574px" Height="123px" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtContent"
                                ErrorMessage="请求内容不能为空！" SetFocusOnError="True" Display="Dynamic">请求内容不能为空！</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2" class="listTitle">
                            <asp:Button ID="cmdSubmit" runat="server" CssClass="btnClass" Text="提交请求" Width="114px"
                                OnClick="cmdSubmit_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trFinished" runat="server" visible="false">
            <td align="center">
                <table width="960" class='listContent'>
                    <tr>
                        <td align="center" style="height: 80px" valign="middle" colspan="2" class="list">
                            <asp:Label ID="Label2" runat="server" Text="您提交的请求已经成功，我们将尽快为您处理，谢谢！"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
