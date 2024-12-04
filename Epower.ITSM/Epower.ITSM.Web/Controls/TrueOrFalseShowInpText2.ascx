<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrueOrFalseShowInpText2.ascx.cs" Inherits="Epower.ITSM.Web.Controls.TrueOrFalseShowInpText2" %>
<script type="text/javascript">
    //选择改变
    function <%=hidControlId.ClientID %>_Change(cont) {
        
        var isShowText="<%=IsShowText%>";
        var selId = '<%=hidControlId.ClientID %>';
        var trId = selId.replace("hidControlId", "trContent");
        if (cont.value == document.getElementById('<%=hidSelValue.ClientID %>').value&&isShowText=="true") {

            document.getElementById(trId).style.display = "block";
        }
        else {
            document.getElementById(trId).style.display = "none";  
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
        if(document.getElementById('<%=selTrueOrFalse.ClientID %>')!=null)
        {
            document.getElementById('<%=selTrueOrFalse.ClientID %>').onchange = function(){
                <%=hidControlId.ClientID %>_Change(this);        
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
        var isShowText="<%=IsShowText%>";
        if(document.getElementById('<%=hidSelValue.ClientID %>').value != "1")
        {
            $("#<%=selTrueOrFalse.ClientID %>").val('1');//
            document.getElementById('<%=trContent.ClientID %>').style.display = "none";  
        }
        if(isShowText=="false")
        {
        document.getElementById('<%=trContent.ClientID %>').style.display = "none";
        }
    });
    
    
</script>
<asp:HiddenField ID="hidText" runat="server" />
<asp:HiddenField ID="hidControlId" runat="server" />
<asp:HiddenField ID="hidSelValue" Value="1" runat="server" />
<table style="width:100%">
    <tr>
        <td>
            <asp:Label ID="lblTrueOrFalse" runat="server" Text="" Visible="false"></asp:Label>
            <select runat="server" id="selTrueOrFalse" style="width:150px" >
                <option value=""></option>
                <option value="0">否</option>
                <option value="1">是</option>                           
            </select>
            <span style="color:Red;margin-left: 7px; font-size: small; font-weight: normal;" runat="server" visible="false" id= "sp">*</span>
        </td>
    </tr>
    <tr id="trContent" runat="server" style="display:none;">
        <td style="width:100%">
            <asp:Label ID="lblContent" runat="server" Text="" Visible="false"></asp:Label>
            <asp:TextBox ID="txtContent" TextMode="MultiLine" runat="server"   Width='90%' />
            <asp:Label ID="lblStar" Visible="false" Style="margin-left:7px;" runat="server" Font-Bold="False" Font-Size="Small"  ForeColor="Red">*</asp:Label>
        </td>
    </tr>
</table>

