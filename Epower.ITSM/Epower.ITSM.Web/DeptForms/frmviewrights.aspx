<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmViewRights" Codebehind="frmViewRights.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>用户权限清单</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" width="95%" align="center" class="listContent">
					<TR>
						<TD align="center" class="listTitle">
							<asp:Label id="lblName" runat="server" Font-Size="Larger" Font-Bold="True"></asp:Label></TD>
					</TR>
					<TR>
						<TD class="list" align="center"><INPUT onclick="window.close();" type="button" value="返 回" class="btnClass">
						</TD>
					</TR>
		    </table>
		    <TABLE id="Table2" width="95%" align="center" class="listContent">
					<TR>
						<TD class="list">
							<asp:Repeater id="rptRights" runat="server">
								<ItemTemplate>
									<tr class="listContent">
										<td width="40%" class="list"><%# DataBinder.Eval(Container.DataItem,"Value.OperateName")%>(<%# DataBinder.Eval(Container.DataItem,"key")%>)</td>
										<td width="9%" align="center" class="list"><font color="#ff3300"><%# GetRightName(DataBinder.Eval(Container.DataItem,"Value.CanRead").ToString())%></font></td>
										<td width="9%" align="center" class="list"><font color="#00cc00"><%# GetRightName(DataBinder.Eval(Container.DataItem,"Value.CanAdd").ToString())%></font></td>
										<td width="9%" align="center" class="list"><font color="#00cc00"><%# GetRightName(DataBinder.Eval(Container.DataItem,"Value.CanModify").ToString())%></font></td>
										<td width="9%" align="center" class="list"><font color="#00cc00"><%# GetRightName(DataBinder.Eval(Container.DataItem,"Value.CanDelete").ToString())%></font></td>
										<td width="24%" align="center" class="list"><%# GetRangeName(DataBinder.Eval(Container.DataItem,"Value.RightRange").ToString(),DataBinder.Eval(Container.DataItem,"Value.ExtDeptList").ToString())%></td>
									</tr>
								</ItemTemplate>
								<HeaderTemplate>
									<tr style="height:25px;">
										<td width="40%" class="list">操作项</td>
										<td width="9%" align="center" class="list">可见</td>
										<td width="9%" align="center" class="list">新增权限</td>
										<td width="9%" align="center" class="list">修改权限</td>
										<td width="9%" align="center" class="list">删除权限</td>
										<td width="24%" align="center" class="list">权限范围</td>
									</tr>
								</HeaderTemplate>
						</asp:Repeater>
			        </TD>
			    </TR>
		</TABLE>
		</form>
	</body>
</HTML>
