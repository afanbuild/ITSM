<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCstRequetFinished.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmCstRequetFinished" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
        <link href="~/skins/2004/style.css" type="text/css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <table width="100%" border="0">
     
    <tr id="trFinished" runat=server >
    <td  align="center">
        <table width="960" class='listContent'>
            <tr>
                
                <td align="center" style="height:80px" valign ="middle"  colspan="2" class="list">
                    <asp:Label ID="Label2" runat="server" Text="您提交的请求已经成功，我们将尽快为您处理，谢谢！"></asp:Label></td>
            </tr></table></td></tr>

</table>
    </div>
    </form>
</body>
</html>
