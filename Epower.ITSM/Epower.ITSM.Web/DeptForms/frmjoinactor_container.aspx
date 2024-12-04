<%@ Page language="c#" Inherits="Epower.ITSM.Web.Forms.frmJoinActor_Container" Codebehind="frmJoinActor_Container.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head >
    <title>加入用户组</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </head>
  <body>
	
    <form id="Form1" method="post" runat="server">
		<iframe src="frmJoinActor2.aspx?ActorType=<%=Request["ActorType"].ToString()%>&ObjectID=<%=Request["ObjectID"].ToString()%>" width=100%  height=100% scrolling=no ></iframe>
     </form>
	
  </body>
</html>
