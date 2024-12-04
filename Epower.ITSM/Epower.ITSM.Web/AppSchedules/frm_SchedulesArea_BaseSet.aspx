<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_SchedulesArea_BaseSet.aspx.cs" Inherits="Epower.ITSM.Web.AppSchedules.frm_SchedulesArea_BaseSet" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc5" %>
<%@ Register Src="CrtTime.ascx" TagName="ctrdateandtime" TagPrefix="uc1" %>
<%@ Register Src="../Controls/CtrFlowRemark.ascx" TagName="CtrFlowRemark" TagPrefix="uc12" %>
<%@ Register Src="~/Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc12" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <script type="text/javascript" language="javascript">
         function CheckDropDownList() {
             var dropDownList1 = document.getElementById("<%=ddEndsegmenttime.ClientID %>"); //获取DropDownList控件
             var dropDownList2 = document.getElementById("<%=ddStartsegmenttime.ClientID %>"); //获取DropDownList控件
             var dropDownList3 = document.getElementById("<%=ddEnddaytime.ClientID %>"); //获取DropDownList控件
             
             var dropDownListValue1 = dropDownList1.options[dropDownList1.selectedIndex].value; //获取选择项的值
             var dropDownListValue2 = dropDownList1.options[dropDownList2.selectedIndex].value; //获取选择项的值
             var dropDownListValue3 = dropDownList1.options[dropDownList3.selectedIndex].value; //获取选择项的值
           
             if (dropDownListValue1 == 1) {
             
                 dropDownList2.selectedIndex=1;
                 dropDownList3.selectedIndex=1;
                 return false;
             }
             else if(dropDownListValue2==1)
             {
                 dropDownList3.selectedIndex=1;
                 return false;
             }
             else {
                 return true;
             }
         }
     </script>
    <table class="listContent" width="98%" align="center" runat="server" id="Table1"
        border="0" cellspacing="1" cellpadding="1">
        <tr>
            <td align="right" colspan="4" class="listTitle">
                <asp:Button ID="Button1" runat="server" Text="确  定" OnClick="BtnSub_ok_Click" CssClass="btnClass" />
                <asp:Button ID="btnCancel2" Text="取  消" runat="server" CssClass="btnClass" OnClick="btnCancel_Click" />
            </td>
        </tr>
        <tr>
            <td style="width: 12%" class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="fullName" runat="server" Text="全称"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:TextBox ID="txtFullName" MaxLength="50" runat="server"></asp:TextBox>
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="simpleName" runat="server" Text="简称"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtSimpleName" MaxLength="10" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr runat="server" id="trCustTime">
            <td class="listTitleRight">
                <asp:Literal ID="LitCustTime" runat="server" Text="上午上班时间"></asp:Literal>
            </td>
            <td class="list">
                <uc1:ctrdateandtime ID="CtrStartdaytime" runat="server" />
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="LitReportingTime" runat="server" Text="中午下班时间"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="ddEndsegmenttime" runat="server" Width="48px" onchange="CheckDropDownList()">
                    <asp:ListItem Text="当天" Value="0" Selected="True" />
                    <asp:ListItem Text="隔天" Value="1" />
                </asp:DropDownList>
                <uc1:ctrdateandtime ID="CtrEndsegmenttime" runat="server" />
            </td>
        </tr>
        <tr runat="server" id="tr1">
            <td class="listTitleRight">
                <asp:Literal ID="Literal1" runat="server" Text="下午上班时间"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="ddStartsegmenttime" runat="server" Width="48px" onchange="CheckDropDownList()">
                    <asp:ListItem Text="当天" Value="0" Selected="True" />
                    <asp:ListItem Text="隔天" Value="1" />
                </asp:DropDownList>
                <uc1:ctrdateandtime ID="CtrStartsegmenttime" runat="server" />
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="Literal2" runat="server" Text="下午下班时间"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="ddEnddaytime" runat="server" Width="48px" onchange="CheckDropDownList()">
                    <asp:ListItem Text="当天" Value="0" Selected="True" />
                    <asp:ListItem Text="隔天" Value="1" />
                </asp:DropDownList>
                <uc1:ctrdateandtime ID="CtrEnddaytime" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="LitContent" runat="server" Text="备注"></asp:Literal>
            </td>
            <td colspan="3" class="list">
                <uc12:CtrFlowRemark ID="txtContent" runat="server" MaxLength="20" TextToolTip="备注" />
            </td>
        </tr>
    </table>
</asp:Content>
