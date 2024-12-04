<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRecommendRuleCata.aspx.cs" Inherits="Epower.ITSM.Web.RecommendRule.frmRecommendRuleCata" %>

<%@ Register Src="../Controls/ctrEquCataDropList.ascx" TagName="ctrEquCataDropList"
    TagPrefix="uc1" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    
    <title>类别值选择</title>
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 100%; height: 100%">
            <tr>
                <td align="center" style="width: 100px" valign="middle">
                    <asp:DropDownList ID="ddlServiceLevel" runat="server" Visible="false" Width="152px">
                    </asp:DropDownList>
        <uc2:ctrFlowCataDropList ID="NormalCata" runat="server" Visible="false" />
        
        
                </td>
            </tr>
            <tr>
                <td class="listTitle" align="center">
                    <asp:Button ID="cmdOK" runat="server" CssClass="btnClass" OnClick="cmdOK_Click" Text="确定" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

