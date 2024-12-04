<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frm_EmailTimeSet.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frm_EmailTimeSet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table  class="listNewContent" >
    <tr>
    <td class="listTitleNew">间隔时间(分钟):</td>
    <td class="listTitleNew"><asp:TextBox ID="txt_Time" runat="server" MaxLength="200" ></asp:TextBox></td>
    <td class="listTitleNew"><asp:Button ID="btn_SaveTime" runat="server" Text="保存" 
            onclick="btn_SaveTime_Click" /></td>
    </tr>
    </table>
    </div>
    </form>
</body>
</html>
