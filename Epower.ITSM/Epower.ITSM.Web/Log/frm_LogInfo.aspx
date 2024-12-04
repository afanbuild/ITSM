<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_LogInfo.aspx.cs" Inherits="Epower.ITSM.Web.Log.frm_LogInfo" %>

<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc6" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        function ClearConfirm()
        {
            event.returnValue =confirm("确认要清除吗，清除后是不能恢复的?");
        }
    </script>

    <table cellpadding='2' cellspacing='0' width='98%' class='listContent'>
        <tr style="display: none">
            <td class='listTitleRight' style='width: 12%;'>
                操作模块
            </td>
            <td class='list' colspan="3">
                <asp:DropDownList ID="ddlType" runat="server" Width="152px">
                    <asp:ListItem Text="请选择" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="项目基本资料" Value="Prj_BaseInfo"></asp:ListItem>
                    <asp:ListItem Text="项目类别" Value="Prj_Type"></asp:ListItem>
                    <asp:ListItem Text="项目模板环节" Value="Prj_Type_StageModel"></asp:ListItem>
                    <asp:ListItem Text="项目计划" Value="Prj_Plan_Node"></asp:ListItem>
                    <asp:ListItem Text="项目年度投资计划" Value="Prj_YearPlan"></asp:ListItem>
                    <asp:ListItem Text="项目月报" Value="Prj_MonthTalk"></asp:ListItem>
                    <asp:ListItem Text="年度计划" Value="Prj_YearPlanBase"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                操作日期
            </td>
            <td class='list' style="width:35%">
                <uc3:ctrdateandtime ID="CtrdateBeginTime" runat="server" ShowTime="false" />
                ~<uc3:ctrdateandtime ID="CtrdateEndTime" runat="server" ShowTime="false" />
            </td>
            <td class='listTitleRight' style='width: 12%;'>
                操作类别
            </td>
            <td class='list'>
                <asp:DropDownList ID="ddlStatus" runat="server" Width="152px">
                    <asp:ListItem Text="请选择" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="系统登录" Value="1"></asp:ListItem>
                    <asp:ListItem Text="新增" Value="2"></asp:ListItem>
                    <asp:ListItem Text="修改" Value="3"></asp:ListItem>
                    <asp:ListItem Text="删除" Value="4"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="listTitle" align="center" colspan="4">
                <asp:Button ID="btnBackUp" SkinID="btnClass3" runat="server" Text="备份当前日志" OnClick="btnBackUp_Click" />
                <asp:Button ID="btnClear" SkinID="btnClass3" runat="server" Text="清理当前日志" OnClick="btnClear_Click" OnClientClick="ClearConfirm();" />
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgPrj_OperLogsInfo" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCreated="dgPrj_OperLogsInfo_ItemCreated">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField='OperateID' HeaderText='OperateID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='SysName' HeaderText='系统名称' ItemStyle-Width="80"></asp:BoundColumn>
                        <asp:BoundColumn DataField='UserName' HeaderText='操作用户名称' ItemStyle-Width="80"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Dept' HeaderText='操作人所在部门' ItemStyle-Width="100"></asp:BoundColumn>
                        <asp:BoundColumn DataField='IPAddress' HeaderText='操作IP地址' ItemStyle-Width="80">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='OPEndTime2' HeaderText='操作时间' ItemStyle-Width="100" DataFormatString="{0:yyyy-MM-dd hh:mm}">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='ActionName' HeaderText='操作类别' ItemStyle-Width="60"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Memo' HeaderText='操作内容'></asp:BoundColumn>
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
