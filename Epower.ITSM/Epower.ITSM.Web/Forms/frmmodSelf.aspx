<%@ Page Language="C#" MasterPageFile="~/MasterPageSingle.master" AutoEventWireup="true"
    Inherits="Epower.ITSM.Web.Forms.frmmodSelf" Title="������Ϣ����" CodeBehind="frmmodSelf.aspx.cs" %>

<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPageSingle.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table id="Table1"  height="400" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="middle" align="center">
                <table id="Table2" width="500" class="listContent Gridtable" cellpadding="2" cellspacing="0">
                    <tr>
                        <td colspan="2" class="listTitleNew" style="text-align:center">
                            <uc1:CtrTitle ID="CtrTitle1" Title="������Ϣ����" runat="server"></uc1:CtrTitle>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight" style="width: 40%">
                            ��¼�û����ƣ�
                        </td>
                        <td class="list" align="left">
                            <asp:Label ID="txtLoginName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                                �û����ƣ�
                        </td>
                        <td class="list" align="left">
                            <asp:TextBox ID="txtName" runat="server" MaxLength="10"></asp:TextBox><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator3" runat="server" ErrorMessage="����Ϊ��" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                                �Ա�
                        </td>
                        <td class="list" align="left">
                            <asp:DropDownList ID="dropSex" runat="server" Width="152px">
                                <asp:ListItem Value="1">��</asp:ListItem>
                                <asp:ListItem Value="2">Ů</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td class="listTitleRight">
                                ְλ��
                        </td>
                        <td class="list" align="left">
                            <uc2:ctrFlowCataDropList ID="CtrFlowCataDropListjob" runat="server" RootID="1014" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                                �绰��
                        </td>
                        <td class="list" align="left">
                            <asp:TextBox ID="txtTelNo" runat="server" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                                �ֻ����룺
                        </td>
                        <td class="list" align="left">
                            <asp:TextBox ID="txtMobile" runat="server" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                                �����ʼ���
                        </td>
                        <td class="list" align="left">
                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="50"></asp:TextBox><asp:RegularExpressionValidator
                                ID="RegularExpressionValidator2" runat="server" ErrorMessage="�����ʼ�����" ControlToValidate="txtEmail"
                                ValidationExpression="^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td class="listTitleRight">
                                    QQ���룺
                        </td>
                        <td class="list" align="left">
                            <asp:TextBox ID="txtQQ" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                                ѧ����
                        </td>
                        <td class="list" align="left">
                            <asp:DropDownList ID="dropEdu" runat="server" Width="152px">
                                <asp:ListItem Value="" Selected="True">��ѡ��...</asp:ListItem>
                                <asp:ListItem Value="��ʿ">��ʿ</asp:ListItem>
                                <asp:ListItem Value="˶ʿ">˶ʿ</asp:ListItem>
                                <asp:ListItem Value="��ѧ">��ѧ</asp:ListItem>
                                <asp:ListItem Value="��ר">��ר</asp:ListItem>
                                <asp:ListItem Value="��ר">��ר</asp:ListItem>
                                <asp:ListItem Value="����">����</asp:ListItem>
                                <asp:ListItem Value="����">����</asp:ListItem>
                                <asp:ListItem Value="Сѧ">Сѧ</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td class="listTitleRight">
                                ��ɫ��
                        </td>
                        <td class="list" align="left">
                            <asp:TextBox ID="txtRole" runat="server" MaxLength="20"></asp:TextBox>
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="152px" Visible="False">
                                <asp:ListItem Value="0" Selected="True">����</asp:ListItem>
                                <asp:ListItem Value="2">����</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 209px; height: 21px" colspan="2" class="listTitle">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2" class="listTitle">
                            <asp:Button ID="cmdSave" runat="server" Text="ȷ��" CssClass="btnClass" OnClick="cmdSave_Click">
                            </asp:Button>&nbsp;&nbsp;
                            <input type="reset" class="btnClass" value="ȡ��">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
