<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Codebehind="frm_FlowDeleteLogInfo.aspx.cs" Inherits="Epower.ITSM.Web.Log.frm_FlowDeleteLogInfo" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ Register src="../Controls/ControlPageFoot.ascx" tagname="ControlPageFoot" tagprefix="uc6" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding='2' cellspacing='0' width='98%' class='listContent'>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                流程应用
            </td>
            <td class='list' style="width:35%">
                <asp:DropDownList ID="ddltApp" runat="server" Width="152px">
                </asp:DropDownList></td>
            <td class='listTitleRight' style="width: 12%;">
                操作日期
            </td>
            <td class='list'>
                <uc3:ctrdateandtime ID="CtrdateBeginTime" runat="server" ShowTime="false"/>
                ~<uc3:ctrdateandtime ID="CtrdateEndTime" runat="server" ShowTime="false"/>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgPrj_OperLogsInfo" runat="server" Width="100%" CellPadding="1"
                    CellSpacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCreated="dgPrj_OperLogsInfo_ItemCreated">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField='AppID' HeaderText='AppID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='AppName' HeaderText='应用名称' ItemStyle-Width='80'></asp:BoundColumn>
                        <asp:BoundColumn DataField='Name' HeaderText='操作人' ItemStyle-Width="80"></asp:BoundColumn>
                        <asp:BoundColumn DataField='DeletedTime' HeaderText='删除时间' ItemStyle-Width="100" DataFormatString="{0:yyyy-MM-dd hh:mm}" ></asp:BoundColumn>
                        <asp:BoundColumn DataField='Subject' HeaderText='主题'></asp:BoundColumn>
                        <asp:BoundColumn DataField='Remark' HeaderText='删除原因'></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                 <uc6:ControlPageFoot ID="cpHol" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
