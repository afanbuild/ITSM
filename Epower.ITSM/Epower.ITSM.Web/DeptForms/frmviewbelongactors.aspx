<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmViewBelongActors" Codebehind="frmViewBelongActors.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>所属用户组</title>
		<meta name="vs_snapToGrid" content="True">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="../Controls/Calendar/Popup.js"></script>
		
	</HEAD>
	<script language="javascript">
		<!--
			function JoinActor()
			{
				var features =
				'dialogWidth:380px;' +
				'dialogHeight:500px;' +
				'directories:no; localtion:no; menubar:no; status=no; toolbar=no;scroll:no;resizable=yes';
				//window.showModalDialog('frmJoinActor2.aspx?ActorType=20&ObjectID='+document.all.hidUserID.value,'',features);
				//===zxl==
				//debugger;
				
				var url='frmJoinActor2.aspx?ActorType=20&ObjectID='+document.all.hidUserID.value+"&TypeFrm=frmJoinActor_Container";
				
				
				window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=480,height=530,left=250,top=100");
				//window.location=window.location.href;
			}

		-->
		</script>
	<body>
		<form id="Form1" method="post" runat="server">
				<asp:listbox id="lsbActors" style="Z-INDEX: 101; LEFT: 3px; POSITION: absolute; TOP: 3px" runat="server"
					Width="264px" Height="296px"></asp:listbox><INPUT id="btnDelete" style="Z-INDEX: 104; LEFT: 104px; POSITION: absolute; TOP: 304px;"
					type="button" value="删 除" name="btnDelete" runat="server" onserverclick="btnDelete_ServerClick" class="btnClass"><INPUT id="hidUserID" style="Z-INDEX: 103; LEFT: 24px; WIDTH: 64px; POSITION: absolute; TOP: 328px; HEIGHT: 19px"
					type="hidden" size="5" name="hidUserID" runat="server"><INPUT style="Z-INDEX: 102; LEFT: 24px; POSITION: absolute; TOP: 304px;"
					onclick="JoinActor();" type="button" value="添 加" class="btnClass"><INPUT style="Z-INDEX: 101; LEFT: 184px; POSITION: absolute; TOP: 304px; "
					onclick="window.close();" type="button" value="返 回" class="btnClass"></form>
	</body>
</HTML>
