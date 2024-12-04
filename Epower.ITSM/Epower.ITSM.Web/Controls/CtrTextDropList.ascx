<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrTextDropList.ascx.cs" Inherits="Epower.ITSM.Web.Controls.CtrTextDropList" %>
<script language="javascript" type="text/javascript" src="../Js/App_Droplst.js"> </script>
<asp:Label ID="labCaption" runat="server"   Visible="False"></asp:Label><asp:TextBox ID="txtText" runat="server"></asp:TextBox><asp:Label ID="rWarning" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
<div runat=server id='fieldsForDropdownLayer' style='display: none; position:absolute; width:120px; left: 120; top: 90; z-index:2'> 
   <select runat=server id='fieldsForDropdown'  style="width: 100%; background-color: #ccffcc;" size='16'> <option value=''></option> </select> </div>
