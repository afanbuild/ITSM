<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmServiceLevelCata.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmServiceLevelCata" %>

<%@ Register Src="../Controls/ctrEquCataDropList.ascx" TagName="ctrEquCataDropList"
    TagPrefix="uc1" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    
    <title>类别值选择</title>
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 100%; height: 100%">
            <tr>
                <td align="center" style="width: 100px" valign="middle">
                
        <uc1:ctrEquCataDropList ID="EquCata" runat="server" Visible="false" />
        <uc2:ctrFlowCataDropList ID="NormalCata" runat="server" Visible="false" />
        <asp:Button ID="cmdOK" runat="server" CssClass="btnClass" OnClick="cmdOK_Click" Text="确定" />
        
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
