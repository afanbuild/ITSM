<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmShowSubProcess.aspx.cs" Inherits="Epower.ITSM.Web.Forms.frmShowSubProcess" %>
<%@ Register Src="../Controls/ctrDySubProcess.ascx" TagName="ctrDySubProcess" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:ctrDySubProcess ID="CtrDySubProcess1" runat="server" EnableViewState="false" />
    </form>
</body>
</html>
