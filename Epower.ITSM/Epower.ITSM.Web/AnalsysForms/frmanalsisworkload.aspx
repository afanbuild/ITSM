<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True"
    CodeBehind="frmAnalsisWorkLoad.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmAnalsisWorkLoad"
    Title="工作量及状态分析" %>
<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc5" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        function btnClick()
        {
            __doPostBack('datarange', '');
            //document.getElementById("<%=btnSelect.ClientID %>").click();
        }    
    </script>
    
    <div style="display:none;">
        <asp:Button ID="btnSelect" Width="0px" runat="server" OnClick="btnSelect_Click" />
    </div>

    <table id="Table2" width="98%" cellpadding="2" cellspacing="0" class="listContent">
        <tr>
            <td class="listTitleRight" style="width:12%">
                人员名称
            </td>
            <td class="list">
                <uc2:UserPicker ID="UserPicker1" runat="server" OnChangeScript="UserPickerChange();" />
            </td>
            <td class="listTitleRight" style="width:12%">
                接收时间
            </td>
            <td class="list">
               <uc5:ctrDateSelectTime ID="ctrDateTime" runat="server" OnChangeScript="btnClick()" />
            </td>
            
        </tr>
    </table>
    <asp:Button ID="btnConfirm" runat="server" OnClick="btnOk_Click" Width="0" Height="0" />
    <table id="tblResult" runat="server" cellspacing="0" cellpadding="0" width="98%" border="0">
        <tr>
            <td>
                <table id="TABLE4" runat="server" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <asp:DataGrid ID="dgResult" runat="server" Width="100%"
                                AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemDataBound="dgResult_ItemDataBound">
                                <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                                <HeaderStyle CssClass="listTitle"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="ReceiveTime" HeaderText="接收日期" DataFormatString="{0:d}">
                                        <HeaderStyle Width="20%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="TotalCount" HeaderText="任务数">
                                        <HeaderStyle Width="20%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="FCount" HeaderText="按时完成">
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="CCount" HeaderText="超时完成">
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="UCount" HeaderText="未完成">
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="OverTime" HeaderText="平均超时">
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:BoundColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                                </PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <div id="ReportDiv" runat="server">
                </div>
            </td>
        </tr>
    </table>

    <script language="javascript">
        function UserPickerChange() {
            __doPostBack('datarange', '');
            //document.all.<%=btnConfirm.ClientID%>.Click;
        }
    </script>

</asp:Content>
