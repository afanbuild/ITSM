<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IfromOftenPage.aspx.cs"
    Inherits="Epower.ITSM.Web.NewMainPage.IfromOftenPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript" language="javascript">
        function E8MenuConfigGoToUrl(url) {
            //E8MenuConfighideAllWindows(0);
            if (url != "") {
                window.top.MainFrame.location = url;
            }
        }
    </script>

    <style>
        li
        {
            list-style-type: none;
        }
    </style>
</head>
<body style="height: 250; margin: 0px;">
    <form id="from1" runat="server">
    <table style="vertical-align: top" id="tdItem" border="0">
        <tr>
            <td align="left" valign="top">
                <div id="DeskPageId" runat="server">                    
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
