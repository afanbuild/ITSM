<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrCataSelectShowInpText.ascx.cs" Inherits="Epower.ITSM.Web.Controls.CtrCataSelectShowInpText" %>

<script type="text/javascript">
    function <%=hidControlId.ClientID %>_Change(cont, cotName) {
        var hid = document.getElementById("<%=hidCatalogId.ClientID %>");
        var hidSel = document.getElementById("<%=hidSelCatalogId.ClientID %>");
        hidSel.value = cont.value;
        if (hid.value.indexOf(cont.value)!=-1&&cont.value!="") {
            document.getElementById(cotName).style.display = "block";
        }
        else {
            document.getElementById(cotName).style.display = "none";  
        }
    }
    
    //文本框获取光标
    function <%=hidControlId.ClientID %>_Focus(cont){
        var defaultStr = document.getElementById('<%=hidText.ClientID %>').value;
        if(cont.value == defaultStr)
        {
            cont.value="";
        }
    }
    
    //文本框失去光标
    function <%=hidControlId.ClientID %>_Blur(cont){
        var defaultStr = document.getElementById('<%=hidText.ClientID %>').value;
        if(cont.value == "")
        {
            cont.value=defaultStr;
        }
    }
    
    $(document).ready(function() {
        if(document.getElementById('<%=selCatalog.ClientID %>')!=null)
        {
            document.getElementById('<%=selCatalog.ClientID %>').onchange = function(){
                <%=hidControlId.ClientID %>_Change(this,'<%=trContent.ClientID %>');        
            };  
        }  
        
        if(document.getElementById('<%=txtContent.ClientID %>')!=null)   
        {
            document.getElementById('<%=txtContent.ClientID %>').onfocus = function(){
                <%=hidControlId.ClientID %>_Focus(this);        
            }; 
            document.getElementById('<%=txtContent.ClientID %>').onblur = function(){
                <%=hidControlId.ClientID %>_Blur(this);
            };      
        }  
    });
</script>

<asp:HiddenField ID="hidText" runat="server" />
<asp:HiddenField ID="hidControlId" runat="server" />
<asp:HiddenField ID="hidCatalogId" runat="server" />
<asp:HiddenField ID="hidSelCatalogId" Value="0" runat="server" />
<table style="width:100%">  
    <tr>
        <td>
            <asp:Label ID="lblCatalog" runat="server" Text="" Visible="false"></asp:Label>
            <select runat="server" id="selCatalog"  style="width:150px"  >                          
            </select>
            <span style="color:Red;margin-left: 7px; font-size: small; font-weight: normal;" runat="server" visible="false" id= "sp">*</span>
        </td>
    </tr>
    <tr id="trContent" runat="server" style="display:none;">
        <td style="width:100%">
            <asp:Label ID="lblContent" runat="server" Text="" Visible="false"></asp:Label>
            <asp:TextBox ID="txtContent" TextMode="MultiLine" runat="server"  Width='90%' />
            <asp:Label ID="lblStar" Visible="false" Style="margin-left:7px;" runat="server" Font-Bold="False" Font-Size="Small"  ForeColor="Red">*</asp:Label>
        </td>
    </tr>
</table>
