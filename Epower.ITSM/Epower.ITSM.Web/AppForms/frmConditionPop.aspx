<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmConditionPop.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmConditionPop" %>

<%@ Register src="../Controls/CtrDateAndTimeV2.ascx" tagname="CtrDateAndTimeV2" tagprefix="uc3" %>
<%@ Register src="../Controls/DeptPicker.ascx" tagname="DeptPicker" tagprefix="uc4" %>

<%@ Register src="../Controls/ctrFlowCataDropListNew.ascx" tagname="ctrFlowCataDropListNew" tagprefix="uc1" %>

<%@ Register src="../Controls/UserPicker.ascx" tagname="UserPicker" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">    
    <title></title>
    <base target="_self" />
</head>
<body>
    <script language="javascript" type="text/javascript" src="<%=sApplicationUrl %>/JS/jquery-1.7.2.min.js"></script>
    <script language="javascript" type="text/javascript" src="<%=sApplicationUrl %>/Controls/My97DatePicker/WdatePicker.js"></script>
    <script language="javascript" type="text/javascript" src="<%=sApplicationUrl %>/Js/App_Base.js"></script>
    <form id="form1" runat="server">
    <div>
        <table style="width: 100%; height: 100%">
            <tr>
                <td align="center" style="width: 140px" valign="middle">                
                <uc1:ctrFlowCataDropListNew ID="ctrFlowCataDropListNew1" ShowType="2" runat="server" Visible="false" />
                <uc2:UserPicker ID="UserPicker1" runat="server" Visible="false" />
                <uc4:DeptPicker ID="DeptPicker1" runat="server" Visible="false" />                    
                <uc3:CtrDateAndTimeV2 ID="ctrDateTime" runat="server" ShowTime="false" Visible="false" />
                <asp:Button ID="cmdOK" runat="server" CssClass="btnClass" OnClick="cmdOK_Click" Text="确定" />        
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
