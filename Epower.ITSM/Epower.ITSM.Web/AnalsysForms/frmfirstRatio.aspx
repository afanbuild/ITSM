<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmfirstRatio.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmfirstRatio"
    Title="首次解决率分析" %>

<%@ Register Src="../Controls/ctrdateandtime.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript" src="../Controls/Calendar/Popup.js"></script>

    <script language="javascript" type="text/javascript" src="../Js/App_Common.js"> </script>
    
    <script language="javascript" type="text/javascript">
        function btnClick()
        {
            document.getElementById("<%=btnSelect.ClientID %>").click();
        }    
    </script>
    
    <div style="display:none;">
        <asp:Button ID="btnSelect" Width="0px" runat="server" OnClick="btnSelect_Click" />
    </div>
    <table id="tblTitle" width="98%" class="listContent" cellpadding="2" runat="server">
        <tr>
            <td class="listTitleRight" width="5%">
                时间范围:
            </td>
            <td class="list" align="left" width="35%" colspan="3">
                <uc1:ctrDateSelectTime ID="ctrDateTime" runat="server" OnChangeScript="btnClick()" />
            </td>
            <td class="listTitleRight" width="12%" style="display:none;">
                <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list" style="display:none;">
                <asp:DropDownList ID="dpdMastShortName" runat="server" AutoPostBack="True" Width="112px"
                    OnSelectedIndexChanged="dpdMastShortName_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="98%" class="listContent" border="0" cellpadding="0" cellspacing="0">
        <tr id="tr2" runat="server">
            <td id="tdResultChart" runat="server" align="left" class="list">
                <div id="ReportDiv" runat="server">
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
