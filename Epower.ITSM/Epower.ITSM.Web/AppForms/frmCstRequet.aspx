<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCstRequet.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmCstRequet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>�����ύ</title>
    <link href="../App_Themes/NewOldMainPage/css.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0">
        <tr runat="server" id="trRequest">
            <td align="center">
                <table width="960" class='listContent'>
                    <tr>
                        <td align="center" style="height: 40px" valign="middle" colspan="2" class="list">
                            <asp:Label ID="Label1" runat="server" Text="��ӭ�ύ�����������ǽ�����Ϊ������"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class='listTitle' align='right' style='width: 30%;'>
                            ��˾��/����<span style="color: #ff0000">*</span>
                        </td>
                        <td class='list' align='left'>
                            <asp:TextBox ID='txtContract' runat='server' Width="240px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtContract"
                                Display="Dynamic" ErrorMessage="��ϵ�˲���Ϊ�գ�" SetFocusOnError="True">��ϵ�˲���Ϊ�գ�</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="listTitle" style="width: 30%">
                            ��ϵ�绰<span style="color: #ff0000">*</span>
                        </td>
                        <td align="left" class="list">
                            <asp:TextBox ID="txtCTel" runat="server" Width="240px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCTel"
                                ErrorMessage="��ϵ�绰����Ϊ�գ������һ����ϵ��" SetFocusOnError="True" Display="Dynamic">��ϵ�绰����Ϊ�գ������һ����ϵ��</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class='listTitle' align='right' style="width: 30%; height: 119px;">
                            ��������<span style="color: #ff0000">*</span>
                        </td>
                        <td class='list' align='left' style="height: 119px">
                            <asp:TextBox ID='txtContent' runat='server' Width="574px" Height="123px" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtContent"
                                ErrorMessage="�������ݲ���Ϊ�գ�" SetFocusOnError="True" Display="Dynamic">�������ݲ���Ϊ�գ�</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2" class="listTitle">
                            <asp:Button ID="cmdSubmit" runat="server" CssClass="btnClass" Text="�ύ����" Width="114px"
                                OnClick="cmdSubmit_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trFinished" runat="server" visible="false">
            <td align="center">
                <table width="960" class='listContent'>
                    <tr>
                        <td align="center" style="height: 80px" valign="middle" colspan="2" class="list">
                            <asp:Label ID="Label2" runat="server" Text="���ύ�������Ѿ��ɹ������ǽ�����Ϊ������лл��"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
