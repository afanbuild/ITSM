<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomCtr.ascx.cs" Inherits="Epower.ITSM.Web.Controls.BussinessControls.CustomCtr" %>

<script>
     String.prototype.trim = function()  //去空格
			{
				// 用正则表达式将前后空格
				// 用空字符串替代。
				return this.replace(/(^\s*)|(\s*$)/g, "");
			}
            function SelectCustomCtr(obj)
			{
			    var MastCust =0;
			    if("<%=OnChangeScript%>"!="")
			    {
			        MastCust = <%=OnChangeScript%>;
			    }
			
			    var CustName = document.all.<%=txtCustom.ClientID%>.value;
			    //===================
			    //Opener_ClientId
			     window.open("../Common/frmDRMUserSelectajax.aspx?IsSelect=true&MastCust="+ MastCust +"&CustName=" + escape(CustName),"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=600px,left=350,top=180");				
			}

</script>

<asp:Label ID="lblCustom" runat="server" Visible="False"></asp:Label><asp:TextBox
    ID="txtCustom" runat="server" MaxLength="80" ReadOnly="true"></asp:TextBox>
<input id="cmdPopCustom" runat="server" onclick="SelectCustomCtr(this)" type="button"
    value="..." class="btnClass2" /><input id="hidCustom" runat="server" size="4" style="width: 56px;
        height: 19px" type="hidden" />
<input id="hidCustomName" runat="server" size="4" style="width: 56px; height: 19px"
    type="hidden" />
<font color="#ff6666">
    <asp:Label ID="labCustom" runat="server" Text="*" Visible="false"  Font-Size="Small"></asp:Label></font>
