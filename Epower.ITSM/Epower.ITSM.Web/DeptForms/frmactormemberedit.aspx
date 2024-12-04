<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmActorMemberEdit" Codebehind="frmActorMemberEdit.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title>用户组成员维护</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript" src="../Js/Common.js"></script>
		
	</HEAD>
	<body>
	
	<script language="javascript">
	    function ClearValues() {
	        document.all.txtObjectName.value = "";
	        document.all.hidObjectID.value = 0;
	    }
	    function SelectObject() {
	        switch (document.all.dpdActorType.value) {
	            case "10": //部门
	                var url = "frmpopdept.aspx?randomid=" + GetRandom()+"&TypeFrm=frmactormemberedit"
				            + "&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
	                open(url, 'E8OpenWin', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=500,height=400px,left=250,top=100');

	                break;


	            case "20": //人员
	            	var url = "frmselectstaffright.aspx?condid=" + document.all.hidObjectID.value+"&TypeFrm=frmactormemberedit"
				            + "&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
	                open(url, 'E8OpenWin', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=450,height=500px,left=150,top=100');

	                break;

	            case "30": //人员条件
	            //========zxl==
	            var url="frmActorCondSelect.aspx?condid=" + document.all.hidObjectID.value+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
	            window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=500px,left=150,top=50");
	            

	        }

	    }

	    function GetRandom() {
	        return Math.floor(Math.random() * 1000 + 1);
	    }
		</script>
		<form id="Form1" method="post" runat="server">
		    <asp:HiddenField ID="hidClientId_ForOpenerPage" runat="server" />
		    <table id="tbmain" width="100%" class="listContent">
				<tr height="30" class="list">
					<td colspan=2>
						<uc1:CtrTitle id="CtrTitle" runat="server"></uc1:CtrTitle>
					</td>
				</tr>
				<tr height="30">
					<td class="listTitle">成员类型：
					</td>
					<td class="list">
						<asp:DropDownList id="dpdActorType" Runat="server" Width="100"></asp:DropDownList>
					</td>
				</tr>
				<tr height="30">
					<td class="listTitle">成员标识：
					</td>
					<td class="list">
						<asp:TextBox id="txtObjectName" runat="server" ReadOnly="True"></asp:TextBox><INPUT id="hidObjectID" type="hidden" runat="server">
						<INPUT id="cmdPop" type="button" value="..." onclick="SelectObject()" class="btnClass2">
					</td>
				</tr>
			</table>
			<br />
			<table width="100%" class="listContent">
				<tr>
					<td class="listTitle" align="center"><asp:Button id="cmdSave" runat="server" Text="保存" onclick="cmdSave_Click" CssClass="btnClass"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;
						<INPUT id="cmdExit" type="button" value="取消" onclick="javascript:window.close()" class="btnClass">
					</td>
				</tr>
			</table>
			<INPUT id="hidID" type="hidden" runat="server"><INPUT id="hidActorID" type="hidden" runat="server">
		</form>
	</body>
</HTML>
