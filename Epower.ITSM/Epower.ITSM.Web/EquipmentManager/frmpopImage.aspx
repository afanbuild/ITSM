<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmpopImage.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmpopImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>图片选择</title>
	<script language="javascript">
	    function GetReturnImageUrl(obj)
	    {
	        var arr = new Array();
	        var id = obj.id.replace("Image1","hidImage");
	        arr[0] = document.getElementById(id).value;     
	        window.opener.document.getElementById("hidImage").value=arr[0];	     
	        window.opener.document.getElementById("DispImage").click();
	        
	        top.close();  	       
	    }
	</script>
</head>
<body>
<form id="form1" runat="server">
    <table style="width:100%" class="listContent" align="center" runat="server" id="Table1">   
    <tr>
        <td class="list" width="100%">
            <asp:DataList ID="dtlstImage" runat="server" RepeatColumns="4" Width="100%" RepeatLayout="Table" RepeatDirection="Horizontal" OnItemCommand="dtlstImage_ItemCommand">
            <ItemTemplate>
		            <td style="cursor:hand;">
		                <input id="hidImage" runat="server" type="hidden" value='<%#((string)Container.DataItem)%>' />
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%#((string)Container.DataItem)%>' onclick="GetReturnImageUrl(this);"/>
		            </td>
			</ItemTemplate>
                
            </asp:DataList>
          </td>
    </tr>
    </table>
</form>
</body>
</html>
