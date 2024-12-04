<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True"
    CodeBehind="frmAvgTime.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmAvgTime"
    Title="平均办理周期分析" %>
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
            <td class="listTitleRight">
                流程名称
            </td>
            <td nowrap class="list">
                <asp:DropDownList ID="dpoFlows" runat="server" Width="152px" AutoPostBack="True"
                    OnSelectedIndexChanged="dpoFlows_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="listTitleRight">
                时间范围
            </td>
            <td nowrap class="list">
               <uc5:ctrDateSelectTime ID="ctrDateTime" runat="server" OnChangeScript="btnClick()"  />
            </td>
        </tr>
    </table>
    <table id="tblResult" runat="server" cellspacing="0" cellpadding="0" width="98%" border="0">
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
                        <asp:BoundColumn DataField="TotalHours" HeaderText="预计工时">
                            <HeaderStyle Width="25%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="avgTime" HeaderText="平均办结周期">
                            <HeaderStyle Width="25%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TimeUnit" Visible="False"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <table id="Table2" width="98%" class="listContent">
        <tr>
            <td width="50%" class="list">
                <div id="ReportDiv" runat="server">
                </div>
            </td>
            <td width="50%" class="list">
                <div id="ReportDiv2" runat="server">
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
