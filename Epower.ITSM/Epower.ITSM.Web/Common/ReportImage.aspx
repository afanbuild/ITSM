<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportImage.aspx.cs" Inherits="Epower.ITSM.Web.Common.ReportImage" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrtitle.ascx" TagName="ctrtitle" TagPrefix="uc1" %>
<%@ Register Src="../Controls/ctrEquCataDropList.ascx" TagName="ctrEquCataDropList"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
   <form id="form1" runat="server"  >
    <table  width="100%" border="0" cellpadding="0" cellspacing="0"  id="Table1" runat="server">
            <tr id="Tr1"  runat="server" >  
            <td align="right">
                            <div id="ReportDiv" runat="server" >
                            </div>
            </td>
            
        </tr>
            <tr id="Tr2" runat="server">
            <td>
                            <div id="_YearDiv" runat="server" >
                            </div>
            </td>
        </tr>
        </table>
    </form>
</body>
</html>
