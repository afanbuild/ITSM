<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmFlowNodeShot.aspx.cs" Inherits="Epower.ITSM.Web.Forms.frmFlowNodeShot" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <table style="width:100%" class="listContent" runat="server" id="tabRender"  >
        <tr>
            <td align="right" class="listTitle" style="width: 10%; height: 21px">
                业务类别</td>
            <td class="list" style="width: 40%; height: 21px">
                <asp:Label ID="lblType" runat="server"></asp:Label></td>
            <td align="right" class="listTitle" style="width: 10%; height: 21px">
        环节名称</td>
            <td class="list" style="width: 40%; height: 21px">
                <asp:Label ID="lblName" runat="server"></asp:Label></td>
        </tr>
<tr>
	<td class='listTitle'  align='right' style="width:10%; height: 21px;">
        时限信息</td>		
<td class='list'  style="width:40%; height: 21px;">
    <asp:Label ID="lblTimes" runat="server"></asp:Label></td>		
        <td  class='listTitle'  align='right' style="width:10%; height: 21px;">
            环节权限</td>
        <td class="list"  style="width:40%; height: 21px;">
            <asp:Label ID="lblRights" runat="server"></asp:Label></td>
</tr>
        <tr id="trRemark" runat=server>
            <td align="right" class="listTitle" style="width: 10%">
                环节描述</td>
            <td class="list" colspan="5">
                <asp:Label ID="lblRemark" runat="server"></asp:Label></td>
        </tr>
   
    <tr id="trDealInfo" runat="server">
        <td align="right" class="listTitle" style="width: 10%">
            处理信息</td>
        <td class="list" colspan="5">
            <asp:Literal ID="litMessage" runat="server"></asp:Literal></td>
    </tr>
        <tr id="trActors" runat="server">
            <td align="right" class="listTitle" style="width: 10%">
                环节人员</td>
            <td class="list" colspan="5">
                <asp:Literal ID="litProcess" runat="server"></asp:Literal></td>
        </tr>
</table>
    </form>
</body>
</html>
