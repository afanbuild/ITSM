<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Epower.ITSM.Web.NewMainPage.Index" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
	<head id="Head1" runat="server">
		<TITLE></TITLE>
		<base onmouseover="window.status='Epower ITSM';return true">
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
</head>
		<script language="JavaScript" type="text/javascript">
<!--
publicPath='./'; 
//-->
		</script>
	</head> 	
   <frameset id="topFrame" rows="50,*" cols="*" frameborder="NO" border="0" framespacing="0">        
		<frame src="Header.aspx" name="frameHeader" id="frameHeader"  scrolling="no" noresize>
		<frameset rows="*" cols="187,*" id="frameMain" name="frameMain" framespacing="0" frameborder="no" border="0"	noresize>
			<frame src="Menu.aspx" id="frameNav" name="frameNav" scrolling="no" noresize>
			<frame src='../AppForms/frm_Services_List.aspx' id="MainFrame"  name="MainFrame" scrolling="auto" noresize>
		</frameset>				
	</frameset>		
</html>
