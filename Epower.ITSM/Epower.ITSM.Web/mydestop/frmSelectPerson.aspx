<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmSelectPerson.aspx.cs" Inherits="Epower.ITSM.Web.mydestop.frmSelectPerson" %>
<%@ Register TagPrefix="uc1" TagName="CtrDeptTree" Src="../Controls/CtrDeptTree.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="../App_Themes/NewOldMainPage/css.css" type="text/css" rel="stylesheet" /><link href="../App_Themes/NewOldMainPage/jquery.contextmenu.css" type="text/css" rel="stylesheet" /><link href="../App_Themes/NewOldMainPage/ui.all.css" type="text/css" rel="stylesheet" /></head>
    <script  type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>
		
<script language="javascript">
			function chkSelect_Click(obj)
			{
		
				reg=/chkSelect/g;
				var idtag=obj.id.replace(reg,"");
				if (obj.checked==true) {
					var userid=document.all(idtag+"hidUserID").value;
					var username=document.all(idtag+"hidUserName").value;
					window.parent.parent.document.all.hidUserID_Name.value=userid+"@"+username
				}
				
				OnlySelectOne(obj);
			}
			
			
			function OnlySelectOne(obj)
			{
				for(i=0;i<document.all.length;i++)
				{
					var sid=document.all(i).id;
					if(sid!="")
					{
						if(sid.substr(sid.length-9,9)=="chkSelect")
						{
							document.all(i).checked=false;
						}
					}
				}
				obj.checked=true;				
			}
			//==========zxl=== 这段代码还有用的
    	function Add_Staff() {			    		
    			
				
				i=document.all.lsbStaff.sele1ctedIndex;				
				if(i==-1)
				{
				    return;
				}
				var openerPageId = "<%=Opener_ClientId %>";
				var text = $('#lsbStaff').find("option:selected").text();
				var val = $('#lsbStaff').val();
				
				window.opener.document.all(openerPageId.replace("cmdPopUser", "txtUser")).value= text;
				window.opener.document.all(openerPageId.replace("cmdPopUser", "hidUser")).value = val;
				window.opener.document.all(openerPageId.replace("cmdPopUser", "hidUserName")).value = text;
				window.opener.location = "javascript:txtonchang();"; //修改用户时，激发onchang事件
				window.parent.close();				
			}  
    
   // =====
			
			

		</script>

<body>
    <form id="form1" runat="server">
    <div style=" margin: 0px 0px 0px 0px;">
<asp:ScriptManager runat="server" ></asp:ScriptManager>
    <asp:UpdatePanel runat="server" >
        <ContentTemplate>
          <table  style=" margin: 0px 0px 0px 0px;" >
				<tr>
					<td align="left" valign="top" >
						
						<asp:TreeView ID="tvDept" runat="server"   
                            onselectednodechanged="tvDept_SelectedNodeChanged" >
                            </asp:TreeView>
					</td>
					<td  style=" width:30px;"> &nbsp;</td>
					<td align="left" valign="top" >
				
					    <asp:ListBox ID="lsbStaff" runat="server" Width="152px" Height="280px" ondblclick="Add_Staff();"   >
					        
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

