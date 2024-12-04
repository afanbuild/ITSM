<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmJoinActor2" Codebehind="frmJoinActor2.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="CtrActorTree" Src="../Controls/ctractortreeV52.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD runat="server">
		<title>加入用户组</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script>
		<!--

		function Tree_Click()
		{
			var lngPreDeptID = 0;
			     var selectedNodeID = Form1.elements["CtrActorTree1_TreeView1_SelectedNode"].value;
			   //zxl====
			     var id;
                if (selectedNodeID != "") {
                    var selectedNode = document.getElementById(selectedNodeID);
                    var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
                     id=value.substring(value.lastIndexOf('\\') + 1);
                   // return value.substring(value.lastIndexOf('\\') + 1);
                }
                 document.all.hidActorID.value=id;
         }
		
		//-->
		</script>
</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体">
				<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 0px"  height="50%" width="100%" class="listContent"> <%----%>
					<TR>
						<TD vAlign="top" align="left" width="100%" colSpan="3" class="list"><uc1:ctractortree id="CtrActorTree1" runat="server" Height="460" Width="100%"></uc1:ctractortree><FONT face="宋体"></FONT></TD>
					</TR>
					<TR>
						<TD align="center" width="100%" colSpan="3" height="20" class="list"><INPUT id="hidActorID" style="WIDTH: 49px; HEIGHT: 22px" type="hidden" size="2" name="hidActorID"
								runat="server">
							<asp:button id="cmdJoin" runat="server"  Text="加 入" onclick="cmdJoin_Click"></asp:button>&nbsp;<INPUT class="btnClass" id="cmdCancel" onclick="top.close();"
								type="button" value="返 回" name="cmdCancel"></TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
