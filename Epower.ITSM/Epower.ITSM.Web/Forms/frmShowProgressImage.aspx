<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmShowProgressImage.aspx.cs" Inherits="Epower.ITSM.Web.Forms.frmShowProgressImage" Title="查看进度条图片" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <img id="showImg" runat="server" />
    </div>
</asp:Content>
