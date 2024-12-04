<%@ Page Language="C#" AutoEventWireup="true"  validateRequest="false" CodeBehind="test1.aspx.cs" Inherits="Epower.ITSM.Web.Common.test1" %>

<%@ Register Src="../Controls/CtrTextDropList.ascx" TagName="CtrTextDropList" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<script language="javascript" type="text/javascript" src="../Js/App_Droplst.js"> </script>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:ctrtextdroplist id="CtrTextDropList1" runat="server"></uc1:ctrtextdroplist>
        <input id="hidXml" runat="server" type="hidden" />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" /><br />
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <asp:Button ID="Button2" runat="server" Text="转拼音" OnClick="Button2_Click" />
        <table style="background-color:#ccffcc;">
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
