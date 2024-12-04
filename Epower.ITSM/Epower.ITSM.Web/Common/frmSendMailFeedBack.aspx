<%@ Page Language="C#" AutoEventWireup="true" validateRequest="false" CodeBehind="frmSendMailFeedBack.aspx.cs" Inherits="Epower.ITSM.Web.Common.frmSendMailFeedBack" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
    function SelectPerson()
    {
       window.open("../mydestop/frmUsers.htm","","scrollbars=yes,resizable=yes,top=100,left=100,width=800,height=600");
       event.returnValue = false;
    }
    function SelectCusts()
    {
       window.open("../mydestop/frmCusts.htm","","scrollbars=yes,resizable=yes,top=100,left=100,width=800,height=600");
       event.returnValue = false;
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
        <table class="listContent" width="100%">
        <tr>
            <td colspan="2" align="center" class="listTitle"><asp:Button ID="btnSend1" runat="server" Text="发  送" OnClick="btnSend1_Click" CssClass="btnClass" />
            </td>
        </tr>
        <tr>
        <td class="listTitle" style="width:10%;">发件人:</td>
        <td class="list">
            <asp:Label ID="lblSend" runat="server" Text=""></asp:Label>&nbsp;
            <asp:Button ID="btnSelectPerson" runat="server" OnClientClick="SelectPerson();" Text="选择人员" CssClass="btnClass" />
            <asp:Button ID="btnSelectCusts" runat="server" OnClientClick="SelectCusts();" Text="选择客户" CssClass="btnClass" /></td>
        </tr>
        <tr>
        <td class="listTitle" style="width:10%;">收件人:</td>
        <td class="list">
            <asp:TextBox ID="MsgTo" runat="server" Width="90%"></asp:TextBox></td>
        </tr>
        <tr>
        <td class="listTitle" style="width:10%;">抄  送:</td>
        <td class="list">
            <asp:TextBox ID="MsgCc" runat="server" Width="90%"></asp:TextBox></td>
        </tr>
        <tr>
        <td class="listTitle" style="width:10%;">密  送:</td>
        <td class="list">
            <asp:TextBox ID="MsgBcc" runat="server" Width="90%"></asp:TextBox></td>
        </tr>
        <tr>
        <td class="listTitle" style="width:10%;">主  题:</td>
        <td class="list">
            <asp:TextBox ID="txtSubject" runat="server" Width="90%"></asp:TextBox></td>
        </tr>
        <tr>
        <td class="listTitle" style="width:10%;">内  容:</td>
        <td class="list">
         <ftb:FreeTextBox ID="txtContent" runat="server" ButtonPath="../Forms/images/epower/officexp/"
                                        Height="400px" ImageGalleryPath="Attfiles\\Photos" Width="100%">
                                    </ftb:FreeTextBox>                                       
         </td>
        </tr>
        <tr>
            <td colspan="2" align="center" class="listTitle"><asp:Button ID="btnSend2" runat="server" Text="发  送" OnClick="btnSend1_Click" /></td>
        </tr>
        </table>
        <input type="hidden" runat="server" id="hidUrl" value />
    </form>
</body>
</html>
