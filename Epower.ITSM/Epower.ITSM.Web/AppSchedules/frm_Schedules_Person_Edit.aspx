<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frm_Schedules_Person_Edit.aspx.cs" Inherits="Epower.ITSM.Web.AppSchedules.frm_Schedules_Person_Edit" %>
<%@ Register src="ShowWorkIssuesOfDay.ascx" tagname="ShowWorkIssuesOfDay" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div>
    <div style="float:left; width:60%;text-align:left; margin-left:30px;">
     <span>时间：</span><asp:Label ID="lblTimeArea" runat="server" ></asp:Label>
    </div>
    <div style="float:left; text-align:right;width:35%;">
        <asp:Button ID="btnBack" runat="server" Text="返回" CssClass="btnClass" 
            onclick="btnBack_Click" />
    </div>
    <uc1:ShowWorkIssuesOfDay ID="ShowWorkIssuesOfDay1" runat="server" />
</div>

</asp:Content>
