<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmEngineer.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmEngineer" %>

<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc5" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc6" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>����ʦ�����������</title>

    <script language="javascript" type="text/javascript" src="../Controls/Calendar/Popup.js"></script>

    <script language="javascript" type="text/javascript" src="../Js/App_Common.js"> </script>
        
</head>
<body>
    <form id="form1" runat="server">
    
    <script language="javascript" type="text/javascript">
        function btnClick()
        {
            document.getElementById("<%=btnSelect.ClientID %>").click();
        }    
    </script>
        <div style="display:none;">
        <asp:Button ID="btnSelect" Width="0px" runat="server" OnClick="btnSelect_Click" />
    </div>
    
    <table id="tblTitle" width="100%" class="listContent" runat="server">
        <tr>
            <td class="listTitle">
                ʱ�䷶Χ:
            </td>
            <td class="list">
                <uc5:ctrDateSelectTime ID="ctrDateTime" runat="server" OnChangeScript="btnClick()"  />
            </td>
              <td class="listTitleRight" style="width: 12%">
                ����״̬:
            </td>
            <td class="list">
                <uc6:ctrFlowCataDropListNew ID="CtrFCDDealStatus" runat="server" RootID="1017" ShowType="2" />
            </td>
            <td class="listTitle">
                <asp:Literal ID="LitMastShortName" runat="server" Text="����λ"></asp:Literal>
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
                        <asp:HyperLinkColumn DataTextField="Name" HeaderText="����ʦ����" Target="_blank" DataNavigateUrlField="ID"
                            DataNavigateUrlFormatString="../CustManager/frmCst_ServiceStaffShow.aspx?ID={0}">
                        </asp:HyperLinkColumn>
                        <asp:BoundColumn DataField="BlongDeptName" HeaderText="����λ"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Num" HeaderText="�¼�����"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TotalHours" HeaderText="��ʱ��"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TotalAmount" HeaderText="�ܷ���"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
