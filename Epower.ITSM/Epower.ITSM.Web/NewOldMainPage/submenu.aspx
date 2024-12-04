<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="submenu.aspx.cs" Inherits="Epower.ITSM.Web.NewOldMainPage.submenu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>    
 

</head>
<body>
    <form id="form1" runat="server">
    
        <asp:TreeView ID="TreeView1" runat="server"  
            DataSourceID="SiteMapDataSource1" 
            Target="MainFrame" NodeIndent="10"  
            OnDataBound="TreeView1_DataBound" 
            OnTreeNodeDataBound="TreeView1_TreeNodeDataBound" 
            EnableViewState="False" ExpandDepth="1">
            <HoverNodeStyle Font-Underline="False" />
            <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" VerticalPadding="0px" ImageUrl="~/Skins/2004/images/icon/folderOpen.gif" />
            <NodeStyle Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" HorizontalPadding="5px"
                NodeSpacing="0px" VerticalPadding="0px" ImageUrl="~/Skins/2004/images/icon/folderClosed.gif" />
            <RootNodeStyle ImageUrl="~/Skins/2004/images/icon/folderOpen.gif" />
        </asp:TreeView>
        <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" ShowStartingNode="False" StartingNodeOffset="0" EnableViewState="False" SiteMapProvider="websitemap" />
    
    </form>
</body>
</html>

<!--Begin: 引入基础脚本库-->
<script  type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>
<!--End: 引入基础脚本库-->

<!--Begin: 引入处理脚本-->
<script  type="text/javascript" src="../Js/epower.sa.submenu.js"> </script>
<!--End: 引入处理脚本-->