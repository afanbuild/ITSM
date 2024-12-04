<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="PersonIssues.aspx.cs" Inherits="Epower.ITSM.Web.AppSchedules.PersonIssues" %>
<%@ Register src="ShowPersonIssues.ascx" tagname="ShowPersonIssues" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ShowPersonIssues ID="PersonIssues1" runat="server" />
</asp:Content>
