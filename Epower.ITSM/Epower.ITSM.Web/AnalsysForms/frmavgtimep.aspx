<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True"
    CodeBehind="frmAvgTimeP.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmAvgTimeP"
    Title="frmAvgTimeP" %>

<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc5" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        function btnClick()
        {
            document.getElementById("<%=btnSelect.ClientID %>").click();
        }    
    </script>
    
    <div style="display:none;">
        <asp:Button ID="btnSelect" Width="0px" runat="server" OnClick="btnSelect_Click" />
    </div>

    <table id="Table1" width="98%" cellpadding="2" cellspacing="0" class="listContent">
        <tr>
            <td class="listTitleRight" style="width:12%">
                流程名称
            </td>
            <td class="list">
                <asp:DropDownList ID="dpoFlows" runat="server" Width="152px" AutoPostBack="True"
                    OnSelectedIndexChanged="dpoFlows_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="listTitleRight" style="width:12%">
                时间范围
            </td>
            <td class="list">
                <uc5:ctrDateSelectTime ID="ctrDateTime" runat="server" OnChangeScript="btnClick()"  />
            </td>
        </tr>
    </table>
    <br />
    <table width="98%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgResult" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemDataBound="dgResult_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="flowmodelid" HeaderText="流程版本">
                            <HeaderStyle Width="20%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="nodename" HeaderText="环节名称">
                            <HeaderStyle Width="30%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="PersonName" HeaderText="姓名">
                            <HeaderStyle Width="25%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="avgTime" HeaderText="平均办结周期">
                            <HeaderStyle Width="25%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TimeUnit" Visible="False"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
