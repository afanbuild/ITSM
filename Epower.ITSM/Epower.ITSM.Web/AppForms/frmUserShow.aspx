<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Epower.ITSM.Web.AppForms.frmUserShow" Title="�û���ϸ��Ϣ" Codebehind="frmUserShow.aspx.cs" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

			<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR >
					<TD  vAlign="middle" align="center" >
					<TABLE id="Table2" width="100%" class="listContent">
					        <tr>
					            <td colspan=2 class="listTitleNew" align="center"><uc1:ctrtitle id="CtrTitle1" Title="�û���ϸ��Ϣ" runat="server"></uc1:ctrtitle>
					            </td>
					        </tr>
							<TR>
								<TD  class="listTitle" width="99px">
									<DIV  align="right">�û����ƣ�</DIV>
								</TD>
								<TD style="HEIGHT: 16px" class="list" align="left" width="70%">
                                    <asp:Label ID="txtName" runat="server" Text=""></asp:Label></TD>
							</TR>
							<TR>
								<TD  class="listTitle">
									<DIV  align="right">�Ա�</DIV>
								</TD>
								<TD style="HEIGHT: 4px" class="list" align="left">
								 <asp:Label ID="dropSex" runat="server" Text=""></asp:Label>
								 </TD>
							</TR>
							<TR>
								<TD  class="listTitle">
									<DIV  align="right">ְλ��</DIV>
								</TD>
								<TD class="list" align="left"><uc2:ctrFlowCataDropList ID="CtrFlowCataDropListjob" runat="server"  RootID="1014" /></TD>
							</TR>
							<TR>
								<TD  class="listTitle">
									<DIV  align="right">�绰��</DIV>
								</TD>
								<TD class="list" align="left"><asp:Label ID="txtTelNo" runat="server" Text=""></asp:Label>
								</TD>
							</TR>
							<TR>
								<TD  class="listTitle">
									<DIV  align="right" >�ֻ����룺</DIV>
								</TD>
								<TD class="list" align="left"><asp:Label ID="txtMobile" runat="server" Text=""></asp:Label>
								</TD>
							</TR>
							<TR>
								<TD  class="listTitle">
									<DIV  align="right">�����ʼ���</DIV>
								</TD>
								<TD style="HEIGHT: 21px" class="list" align="left"><asp:Label ID="txtEmail" runat="server" Text=""></asp:Label>
								</TD>
							</TR>
							<TR>
								<TD  class="listTitle"><FONT face="����">
										<DIV  align="right">QQ���룺</DIV>
									</FONT>
								</TD>
								<TD  class="list" align="left">
								<asp:Label ID="txtQQ" runat="server" Text=""></asp:Label>
							    </TD>
							</TR>
							<TR>
								<TD  class="listTitle">
									<DIV  align="right">ѧ����</DIV>
								</TD>
								<TD  class="list" align="left">
								    <asp:Label ID="dropEdu" runat="server" Text=""></asp:Label>
								 </TD>
							</TR>
							<TR>
								<TD  class="listTitle">
									<DIV  align="right">
                                        ��ɫ��</DIV>
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
										<INPUT style="WIDTH: 49px; HEIGHT: 20px" type="button" class="btnClass" value="�ر�" onclick="window.close();"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
	</asp:Content>