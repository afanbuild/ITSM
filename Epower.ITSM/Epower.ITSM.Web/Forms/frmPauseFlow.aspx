<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPauseFlow.aspx.cs" Inherits="Epower.ITSM.Web.Forms.frmPauseFlow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>流程暂停事由</title>
</head>
<script language="javascript" type="text/javascript">
			function Click_OK()
			{
			    if(document.all.<%=txtOpinion.ClientID%>.value!="")
			    {
			    //==========zxl==
				  //  window.returnValue=document.all.<%=txtOpinion.ClientID%>.value;
				  var value=document.all.<%=txtOpinion.ClientID%>.value;
				   if(value != null && value !="")
			    {
			        window.opener.document.getElementById("<%=Opener_ClientId %>hidPauseFlow").value=value;
                    window.opener.document.getElementById("<%=Opener_ClientId %>cmdPauseFlow").click();
				
				  
			    }
			    //====zxl===
				  
				    window.close();
				}
				else
				{
				    alert("流程暂停事由不能为空！");
				}
			}
			function Click_Cancel()
			{
				window.returnValue="";
				window.close();
			}
			
			
			
		</script>
<body>
    <form id="form1" runat="server">
        <table class="listContent" width="100%">
        <tr height="200">
            <td class="list"><asp:TextBox ID="txtOpinion" runat="server" Height="200px" TextMode="MultiLine" Width="90%"></asp:TextBox>
            <asp:Label ID="rWarning" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" class="list">
                <input id="Button1" type="button" value="确定" onclick="Click_OK();" class="btnClass" /> 
                <INPUT id="cmdCancel"  class="btnClass" type="button" value="取消" onclick="Click_Cancel()">
            </td>
        </tr>
        </table>
        
        <br />
    </form>
</body>
</html>

