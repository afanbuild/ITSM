<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  AutoEventWireup="true" CodeBehind="frm_SchedulesArea_Summary.aspx.cs" Inherits="Epower.ITSM.Web.AppSchedules.frm_SchedulesArea_Summary" %>

<%@ Register assembly="MyCalendar.UI" namespace="MyCalendar.UI" tagprefix="uap" %>

<%@ Register src="ShowWorkIssuesSummary.ascx" tagname="ShowWorkIssuesSummary" tagprefix="uc2" %>

<%@ Register src="ShowWorkIssuesOfDay.ascx" tagname="ShowWorkIssuesOfDay" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div>
    <div style="float:left; width:60%;text-align:left; margin-left:30px;">
     <span>时间：</span><asp:Label ID="lblTimeArea" runat="server" ></asp:Label>
    </div>
    <div style="float:left; text-align:right;width:35%;">
        <asp:Button ID="btnDelete" runat="server" Text="重新排班" CssClass="btnClass" 
             OnClientClick="javascript: return confirm('您确定重新排班吗?');"
            onclick="btnDelete_Click" />
          <asp:Button ID="btnViewSchedure" runat="server" Text="排班汇总表" CssClass="btnClass"  CommandName="ViewSchedure"
            onclick="btnGoToSummary_Click" />
              
          <asp:Button ID="btnGoToSummary" runat="server" Text="排班表" CssClass="btnClass" CommandName="ViewSummary"
            onclick="btnGoToSummary_Click" />
         

    </div>
</div>
<br />
<div style="clear;"></div>

    

  
  <table width="100%" style=" margin-left:20px; margin-top :10px;" >
    <tr>
        <td width="60%" align="left" valign="top" >
            <uc2:ShowWorkIssuesSummary ID="ShowWorkIssuesSummary1" runat="server" />    
        </td>
         <td width="1%">
        
        </td>
         <td width="30%" align="left" valign="top"  class="listContent" >
            <uc1:ShowWorkIssuesOfDay ID="ShowWorkIssuesOfDay1" runat="server" />
        </td>
    </tr>
  </table>

    
</asp:Content>
