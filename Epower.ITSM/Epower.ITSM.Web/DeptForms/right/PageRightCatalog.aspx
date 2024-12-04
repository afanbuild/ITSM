<%--<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

--%><%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageRightCatalog.aspx.cs" Inherits="Epower.ITSM.Web.DeptForms.right.PageRightCatalog" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1"  runat="server">
    <title>扫描文件配置</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
	<meta name="CODE_LANGUAGE" Content="C#">
	<meta name="vs_defaultClientScript" content="JavaScript">
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">

</head>
<body>
    
    <form id="form1" method="post" runat="server">  
		     <%--<iewc:TreeView id="tvSubject" runat="server" ></iewc:TreeView>--%>
		     
		     <asp:TreeView ID="tvSubject" runat="server" ForeColor="Red" >
            </asp:TreeView>

	  </form>  
</body>
</html>
