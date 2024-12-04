<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Test2.aspx.cs" Inherits="Epower.ITSM.Web.Common.Test2" Title="无标题页" %>

<%@ Register Src="../Controls/CtrTextDropList.ascx" TagName="CtrTextDropList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:CtrTextDropList ID="CtrTextDropList1" runat="server" />
</asp:Content>
