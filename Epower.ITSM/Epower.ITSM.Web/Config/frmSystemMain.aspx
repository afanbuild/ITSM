<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmSystemMain.aspx.cs" Inherits="Epower.ITSM.Web.Config.frmSystemMain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
	<head runat="server">
		<TITLE></TITLE>
	</head>
	 

   <frameset rows="*" cols="210,*" frameborder="NO" border="0" framespacing="0">
		<frame src="frmLeft.aspx" name="frmLeft" scrolling="no" noresize>
		<frame src="frmSetSystemParams.aspx?NodesName=SystemName" id="frmSystemMain" name="frameNav" scrolling="auto">
	</frameset>

<noframes>
	</noframes>
		<body>
		   
		</body>
</html>
