<%@ Page Language="c#" Inherits="Epower.ITSM.Web.Forms.ShowNews" CodeBehind="ShowNews.aspx.cs"
    AutoEventWireup="True" %>

<%@ Register Src="../Controls/ctrattachment.ascx" TagName="ctrattachment" TagPrefix="uc1" %>
<html>
<head id="HEAD1" runat="server">
    <title></title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table id="Table2" width="90%" align="center" cellpadding="2" class="listContent"
        style="z-index: 101; left: 48px; position: absolute; top: 32px">
        <tr>
            <td align="center" class="list" rowspan="1" height="50" colspan="2">
                <asp:Label ID="LblTitle" runat="server" Font-Size="12" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" rowspan="" class="listTitle" colspan="2">
                提供者:
                <asp:Label ID="LblWriter" runat="server" Font-Size="9"></asp:Label>（发布时间:
                <asp:Label ID="LblPubDate" runat="server" Font-Size="9"></asp:Label>）
            </td>
        </tr>
        <tr>
            <td class="listTitle">
                <asp:Label ID="LblIsFile" runat="server" Text="附件"></asp:Label>
            </td>
            <td align="left" class="list">
                <div align="center">
                    <uc1:ctrattachment ID="Ctrattachment1" runat="server" />
                </div>
        </tr>
        <tr>
            <td class="listTitle" style="width: 12%">
                内容
            </td>
            <td class="list ueditor_tbl">
                <div align="center">
                    <asp:Image ID="Image1" runat="server"></asp:Image></div>
                <br>
                <asp:Label ID="LblContent" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
