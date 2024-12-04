<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Epower.ITSM.Web.AppForms.frmUserShow" Title="用户详细信息" Codebehind="frmUserShow.aspx.cs" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

			<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR >
					<TD  vAlign="middle" align="center" >
					<TABLE id="Table2" width="100%" class="listContent">
					        <tr>
					            <td colspan=2 class="listTitleNew" align="center"><uc1:ctrtitle id="CtrTitle1" Title="用户详细信息" runat="server"></uc1:ctrtitle>
					            </td>
					        </tr>
							<TR>
								<TD  class="listTitle" width="99px">
									<DIV  align="right">用户名称：</DIV>
								</TD>
								<TD style="HEIGHT: 16px" class="list" align="left" width="70%">
                                    <asp:Label ID="txtName" runat="server" Text=""></asp:Label></TD>
							</TR>
							<TR>
								<TD  class="listTitle">
									<DIV  align="right">性别：</DIV>
								</TD>
								<TD style="HEIGHT: 4px" class="list" align="left">
								 <asp:Label ID="dropSex" runat="server" Text=""></asp:Label>
								 </TD>
							</TR>
							<TR>
								<TD  class="listTitle">
									<DIV  align="right">职位：</DIV>
								</TD>
								<TD class="list" align="left"><uc2:ctrFlowCataDropList ID="CtrFlowCataDropListjob" runat="server"  RootID="1014" /></TD>
							</TR>
							<TR>
								<TD  class="listTitle">
									<DIV  align="right">电话：</DIV>
								</TD>
								<TD class="list" align="left"><asp:Label ID="txtTelNo" runat="server" Text=""></asp:Label>
								</TD>
							</TR>
							<TR>
								<TD  class="listTitle">
									<DIV  align="right" >手机号码：</DIV>
								</TD>
								<TD class="list" align="left"><asp:Label ID="txtMobile" runat="server" Text=""></asp:Label>
								</TD>
							</TR>
							<TR>
								<TD  class="listTitle">
									<DIV  align="right">电子邮件：</DIV>
								</TD>
								<TD style="HEIGHT: 21px" class="list" align="left"><asp:Label ID="txtEmail" runat="server" Text=""></asp:Label>
								</TD>
							</TR>
							<TR>
								<TD  class="listTitle"><FONT face="宋体">
										<DIV  align="right">QQ号码：</DIV>
									</FONT>
								</TD>
								<TD  class="list" align="left">
								<asp:Label ID="txtQQ" runat="server" Text=""></asp:Label>
							    </TD>
							</TR>
							<TR>
								<TD  class="listTitle">
									<DIV  align="right">学历：</DIV>
								</TD>
								<TD  class="list" align="left">
								    <asp:Label ID="dropEdu" runat="server" Text=""></asp:Label>
								 </TD>
							</TR>
							<TR>
								<TD  class="listTitle">
									<DIV  align="right">
                                        角色：</DIV>
								</TD>
								<TD class="list" align="left">
								    <asp:Label ID="txtRole" runat="server" Text=""></asp:Label>
							    </TD>
							</TR>
							<TR>
								<TD style="WIDTH: 209px; HEIGHT: 21px" colspan="2" class="listTitle"></TD>
							</TR>
							<TR >
								<TD align="center" colSpan="2" class="listTitle">
										<INPUT style="WIDTH: 49px; HEIGHT: 20px" type="button" class="btnClass" value="关闭" onclick="window.close();"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
	</asp:Content>