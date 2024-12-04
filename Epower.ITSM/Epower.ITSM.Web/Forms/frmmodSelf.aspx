<%@ Page Language="C#" MasterPageFile="~/MasterPageSingle.master" AutoEventWireup="true"
    Inherits="Epower.ITSM.Web.Forms.frmmodSelf" Title="个人信息设置" CodeBehind="frmmodSelf.aspx.cs" %>

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
                            <uc1:CtrTitle ID="CtrTitle1" Title="个人信息设置" runat="server"></uc1:CtrTitle>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight" style="width: 40%">
                            登录用户名称：
                        </td>
                        <td class="list" align="left">
                            <asp:Label ID="txtLoginName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                                用户名称：
                        </td>
                        <td class="list" align="left">
                            <asp:TextBox ID="txtName" runat="server" MaxLength="10"></asp:TextBox><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator3" runat="server" ErrorMessage="不能为空" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                                性别：
                        </td>
                        <td class="list" align="left">
                            <asp:DropDownList ID="dropSex" runat="server" Width="152px">
                                <asp:ListItem Value="1">男</asp:ListItem>
                                <asp:ListItem Value="2">女</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td class="listTitleRight">
                                职位：
                        </td>
                        <td class="list" align="left">
                            <uc2:ctrFlowCataDropList ID="CtrFlowCataDropListjob" runat="server" RootID="1014" />
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                                电话：
                        </td>
                        <td class="list" align="left">
                            <asp:TextBox ID="txtTelNo" runat="server" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                                手机号码：
                        </td>
                        <td class="list" align="left">
                            <asp:TextBox ID="txtMobile" runat="server" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                                电子邮件：
                        </td>
                        <td class="list" align="left">
                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="50"></asp:TextBox><asp:RegularExpressionValidator
                                ID="RegularExpressionValidator2" runat="server" ErrorMessage="电子邮件有误！" ControlToValidate="txtEmail"
                                ValidationExpression="^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td class="listTitleRight">
                                    QQ号码：
                        </td>
                        <td class="list" align="left">
                            <asp:TextBox ID="txtQQ" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                                学历：
                        </td>
                        <td class="list" align="left">
                            <asp:DropDownList ID="dropEdu" runat="server" Width="152px">
                                <asp:ListItem Value="" Selected="True">请选择...</asp:ListItem>
                                <asp:ListItem Value="博士">博士</asp:ListItem>
                                <asp:ListItem Value="硕士">硕士</asp:ListItem>
                                <asp:ListItem Value="大学">大学</asp:ListItem>
                                <asp:ListItem Value="大专">大专</asp:ListItem>
                                <asp:ListItem Value="中专">中专</asp:ListItem>
                                <asp:ListItem Value="高中">高中</asp:ListItem>
                                <asp:ListItem Value="初中">初中</asp:ListItem>
                                <asp:ListItem Value="小学">小学</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td class="listTitleRight">
                                角色：
                        </td>
                        <td class="list" align="left">
                            <asp:TextBox ID="txtRole" runat="server" MaxLength="20"></asp:TextBox>
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="152px" Visible="False">
                                <asp:ListItem Value="0" Selected="True">启用</asp:ListItem>
                                <asp:ListItem Value="2">禁用</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 209px; height: 21px" colspan="2" class="listTitle">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2" class="listTitle">
                            <asp:Button ID="cmdSave" runat="server" Text="确认" CssClass="btnClass" OnClick="cmdSave_Click">
                            </asp:Button>&nbsp;&nbsp;
                            <input type="reset" class="btnClass" value="取消">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
