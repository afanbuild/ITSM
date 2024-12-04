<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frm_frm_SchedulesArea_View.aspx.cs" Inherits="Epower.ITSM.Web.AppSchedules.frm_frm_SchedulesArea_View" %>
<%@ Register assembly="MyCalendar.UI" namespace="MyCalendar.UI" tagprefix="uap" %>
<%@ Register src="ShowWorkIssues.ascx" tagname="ShowWorkIssues" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div>
    <div style="float:left; width:60%;text-align:left; margin-left:30px;">
     <span>时间：</span><asp:Label ID="lblTimeArea" runat="server" ></asp:Label>
    </div>
    <div style="float:left; text-align:right;width:35%;">
        <asp:Button ID="btnDelete" runat="server" Text="重新排班" CssClass="btnClass" 
             OnClientClick="javascript: return confirm('您确定重新排班吗?');"
            onclick="btnDelete_Click" />
            
        <asp:Button ID="btnGoToDetail" runat="server" Text="返回" CssClass="btnClass" 
            onclick="btnGoToDetail_Click" />
    </div>
</div>

<br />
<div style=" clear;">
    <uc1:ShowWorkIssues ID="ShowWorkIssues1" runat="server" />
    </div>
<div>
   
    
</div>
</asp:Content>
