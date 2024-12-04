<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAddOpinions.aspx.cs" Inherits="Epower.ITSM.Web.Forms.frmAddOpinions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>补充意见录入</title>
</head>
<script type="text/javascript" language="javascript">
			function Click_OK()
			{
			//zxl===
			    
				//window.returnValue=document.all.<%=txtOpinion.ClientID%>.value;
				 var value=document.all.<%=txtOpinion.ClientID %>.value;
				 if(value != null && value !="")
			    {
			        window.opener.document.getElementById("<%=Opener_ClientId %>txtMsgProcess").value=value;
			        window.opener.document.getElementById("<%=Opener_ClientId %>cmdMsgProcess").click();
   
			    }
				 
				 
				//==zxl====
				window.close();
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
        <tr height="168">
            <td class="list"><asp:TextBox ID="txtOpinion" runat="server" Height="168px" TextMode="MultiLine" Width="100%"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="center" class="list">
                <input id="Button1" type="button" value="确定" onclick="Click_OK();" class="btnClass" /> 
                <input id="cmdCancel"  class="btnClass" type="button" value="取消" onclick="Click_Cancel()"/>
            </td>
        </tr>
        </table>
        
        <br />
    </form>
</body>
</html>
