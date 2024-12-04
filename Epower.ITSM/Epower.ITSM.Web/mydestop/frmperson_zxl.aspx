<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmperson_zxl.aspx.cs" Inherits="Epower.ITSM.Web.mydestop.frmperson_zxl" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml" >
	<HEAD id="HEAD1" runat="server">
		<title>人员页面</title>
		<script type="text/javascript" src="../Js/jquery-1.7.2.min.js"></script>
	</HEAD>		
	
	<style type="text/css">
	    table a 
	    {
	    	color:#004175;
	    	font-size: 12px;
	    }
	</style>	
	<body leftmargin="0" topmargin="0">
	<form id="Form1" method="post" runat="server">
	  <div style=" margin: 0px 0px 0px 0px;">
<asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
          <table  style=" margin: 0px 0px 0px 0px;" >
				<tr>
					<td align="left" valign="top" >
						
						<asp:TreeView ID="tvDept" runat="server"  
                            onselectednodechanged="tvDept_SelectedNodeChanged"  >
                            </asp:TreeView>
					</td>
					<td  style=" width:30px;"> &nbsp;</td>
					<td align="left" valign="top" >
				
					    <asp:ListBox ID="lsbStaff" runat="server" Width="152px" Height="280px"  onselectedindexchanged="lsbStaff_SelectedIndexChanged" AutoPostBack="True">
					        
					    </asp:ListBox>
					    
					</td>
				</tr>
			</table>
        </ContentTemplate>
    </asp:UpdatePanel>
      
		
    </div>
	
    </form>
</body>
</html>
