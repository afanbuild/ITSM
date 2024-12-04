<%@ Page Language="C#" MasterPageFile="~/MasterPageSingle.master" AutoEventWireup="true" Inherits="Epower.ITSM.Web.Forms.frmModUser" Title="Untitled Page" Codebehind="frmModUser.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPageSingle.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
			<TABLE id="Table1" height="300" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR >
					<TD  vAlign="middle" align="center" >
					<TABLE id="Table2" width="500" class="listContent">
					        <tr>
					            <td colspan=2 class="listTitleNew" style="text-align:center"><uc1:ctrtitle id="CtrTitle1" Title="修改密码" runat="server"></uc1:ctrtitle>
					            </td>
					        </tr>
							<TR>
								<TD style="WIDTH: 209px" class="listTitle" align="right" style="WIDTH: 209px; HEIGHT: 23px">
									登录用户名称：
								</TD>
								<TD align=left class="list">
                                    <asp:Label ID="txtLoginName" runat="server" Text=""></asp:Label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 209px; HEIGHT: 23px" class="listTitle" align="right">
									用户名称：
								</TD>
								<TD style="HEIGHT: 23px" align=left class="list">
								<asp:Label ID="txtName" runat="server" Text=""></asp:Label>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 209px; HEIGHT: 28px" class="listTitle" align="right">
									新密码：
								</TD>
								<TD style="HEIGHT: 28px" align=left class="list">
									<asp:textbox id="txtFistPwd" runat="server" TextMode="Password" MaxLength="25"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 209px; HEIGHT: 28px" class="listTitle" align="right">
									确认新密码：
								</TD>
								<TD style="HEIGHT: 28px" align=left class="list">
                                    <asp:textbox id="txtLastPwd" runat="server"  TextMode="Password" MaxLength="25"></asp:textbox>
									<asp:CompareValidator id="CompareValidator1" runat="server" ControlToValidate="txtLastPwd" ErrorMessage="两次输入不一致" Operator="Equal"
										ControlToCompare="txtFistPwd"></asp:CompareValidator>
										</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 209px; HEIGHT: 21px" colspan="2" class="listTitle"></TD>
							</TR>
							<TR class="StatusBarForPop">
								<TD align="center" colSpan="2" class="listTitle"><asp:button id="cmdSave" runat="server" Text="确认" CssClass="btnClass" onclick="cmdSave_Click"></asp:button>&nbsp;&nbsp;
										<INPUT  type="reset" class="btnClass"
											value="取消"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
			<INPUT id="hidUserID" type="hidden" runat="server"> &nbsp; <INPUT id="hidPassWord" type="hidden" runat="server">&nbsp;
			<asp:Literal id="Literal1" runat="server"></asp:Literal>
	</asp:Content>