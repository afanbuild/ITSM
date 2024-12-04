<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmEngineerAnalysis.aspx.cs"
    Inherits="Epower.ITSM.Web.AnalsysForms.frmEngineerAnalysis" %>

<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>工程师工作情况分析</title>

    <script language="javascript" type="text/javascript" src="../Controls/Calendar/Popup.js"></script>

    <script language="javascript" type="text/javascript" src="../Js/App_Common.js"> </script>

</head>
<body>
    <form id="form1" runat="server">

    <script language="javascript" type="text/javascript">
        function btnClick() {
            document.getElementById("<%=btnSelect.ClientID %>").click();
        }    
    </script>

    <div style="display: none;">
        <asp:Button ID="btnSelect" Width="0px" runat="server" OnClick="btnSelect_Click" />
    </div>
    <table id="tblTitle" width="100%" class="listContent" runat="server">
        <tr>
            <td class="listTitleRight" style="width: 12%">
                时间范围
            </td>
            <td class="list" style="width: 35%">
                <uc5:ctrDateSelectTime ID="ctrDateTime" runat="server" OnChangeScript="btnClick()" />
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="dpdMastShortName" runat="server" AutoPostBack="True" Width="112px"
                    OnSelectedIndexChanged="dpdMastShortName_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" class="listContent">
        <tr id="tr2" runat="server">
            <td id="tdResultChart" runat="server" align="left" class="list" style="width: 645px;
                height: 215px;" valign="top">
                <asp:DataGrid ID="dgMaterialStat" runat="server" Width="100%" AllowCustomPaging="True"
                    PageSize="100">
                    <Columns>
                        <asp:HyperLinkColumn DataTextField="Name" HeaderText="工程师名称" Target="_blank" DataNavigateUrlField="ID"
                            DataNavigateUrlFormatString="../CustManager/frmCst_ServiceStaffShow.aspx?ID={0}">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:HyperLinkColumn>
                        <asp:BoundColumn DataField="BlongDeptName" HeaderText="服务单位">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Num" HeaderText="正处理事件单数"></asp:BoundColumn>
                        <asp:BoundColumn DataField="IsnullWork" HeaderText="空闲"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
