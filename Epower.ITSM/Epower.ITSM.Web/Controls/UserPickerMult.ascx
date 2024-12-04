<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserPickerMult.ascx.cs"
    Inherits="Epower.ITSM.Web.Controls.UserPickerMult" %>


<div>
<input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
    type="hidden" />
<asp:Label ID="labUser" runat="server" Visible="False"></asp:Label>
<asp:TextBox ID="txtUser"
    runat="server" MaxLength="80" ReadOnly="True"></asp:TextBox><input id="cmdPopUser"
        runat="server" type="button" value="..." class="btnClass2" />
 <img src="../Images/saoba.jpg" 
 style="cursor:pointer; position:relative;top:5px;" 
 height="25"
 title="清除选中人员" id="clear" runat="server"/>
        <input
            id="hidUser" runat="server" name="hidUser" size="4" style="width: 56px; height: 19px"
            type="hidden" />
<input id="hidUserName" runat="server" name="hidUser" size="4" style="width: 56px;
    height: 19px" type="hidden" />
<asp:Label ID="rWarning" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Red">*</asp:Label>
</div>

<script>
    //维修人员选择
    function <%=hidClientId_ForOpenerPage.ClientID %>_SelectUserMult(obj) {
        var  strUrl='<%=sApplicationUrl %>'+"/mydestop/frmUsersMultSelect.aspx?Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
        if('<%=IsLimit %>'=="True")
        {
            strUrl='<%=sApplicationUrl %>' + "/mydestop/frmUsersMultSelect.aspx?Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
        }
        
        var retVal = window.open(strUrl,'',"resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=1000,height=550px,left=150,top=50");               
    }
    
    function <%=hidClientId_ForOpenerPage.ClientID %>_open_userlist() {            
        var  strUrl='<%=sApplicationUrl %>'+"/Controls/frmSelectedUserList.aspx?Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";        
        
        window.open(strUrl,'',"resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=281,height=400px,left=150,top=50");                       
    }
    
    document.getElementById('<%=cmdPopUser.ClientID %>').onclick = function(){
        <%=hidClientId_ForOpenerPage.ClientID %>_SelectUserMult(this);        
    };       
    
    document.getElementById('<%=clear.ClientID %>').onclick = function(){
        <%=hidClientId_ForOpenerPage.ClientID %>_open_userlist();
    };           
    
</script>
