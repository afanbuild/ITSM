<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.ctrdepttreeBranch" Codebehind="ctrdepttreeBranch.ascx.cs" %>

<FONT face="ËÎÌו">	
	<asp:TreeView ID="tvDept" runat="server" onclick="TreeNode_Click()" 
    onselectednodechanged="tvDept_SelectedNodeChanged">	    
</asp:TreeView>
    <asp:HiddenField ID="hfSelectedId" runat="server" />
</FONT>
	

