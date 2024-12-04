<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginAuto.aspx.cs" Inherits="Epower.ITSM.Web.LoginAuto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
<script language="javascript">
    
    function OpenForm(_url)
    {
       // alert(_url);
	    window.parent.WebFormRel.location=_url;
    }
</script>
</head>
		<frameset rows="0,*" cols="*" id="frameMain" framespacing="0" frameborder="no" border="0">
		    <frame src='' id="MainFrame"  name="MainFrame" scrolling="auto" noresize>
			<frame src='' id="WebFormRel" name="WebFormRel" scrolling="no" noresize>
		</frameset>
<noframes>
		<body>
		   
		</body>
	</noframes>

</html>
