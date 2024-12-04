<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_OA_ReleaseManagementList.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frm_OA_ReleaseManagementList"
    Title="无标题页" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register Src="~/Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc3" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc3" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register Src="~/Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc5" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
        function OpenDeleteFlow(obj)  //删除流程
        {
            var FlowID = document.getElementById(obj.id.replace("btnDelete", "hidDelete")).value;
            var value = window.showModalDialog("../Common/frmFlowDelete.aspx?FlowID=" + FlowID, window, "dialogHeight:230px;dialogWidth:320px");
            if (value != null) {
                if (value[0] == "0") //成功
                    event.returnValue = true;
                else
                    event.returnValue = false;
            }
            else {
                event.returnValue = false;
            }
        }  
        
         function ChangeAssociate(obj)  //关联变更单

    {
        var lngFlowID = document.getElementById(obj.id.replace("btnAssociate","hidAssociate")).value;
        window.open("../Forms/form_all_flowmodel.aspx?NewWin=true&appid=420&ep=" + lngFlowID,"","scrollbars=no,status=yes ,resizable=yes,width=680,height=500");
        event.returnValue = false;
    }
    </script>

    <input id="hidTable" value="" runat="server" type="hidden" />
    <table width="100%" align="center" runat="server" id="tbltitle" style="display:none ">
        <tr>
            <td class="list" align="center" colspan="2">
                <uc1:CtrTitle ID="CtrTitle1" runat="server" Title="发布管理"></uc1:CtrTitle>
            </td>
        </tr>
    </table>
    <table class="listContent" width="100%" id="tblContent" runat="server">
        <tr>
            <td align="right" class="listTitle" style="width: 15%">
                版本名称
            </td>
            <td align="left" class="list" style="width: 35%">
                <asp:TextBox ID="txtVersionName" runat="server" Width="216px" meta:resourcekey="txtVersionNameResource1"></asp:TextBox>
            </td>
            <td align="right" class="listTitle" style="width: 15%">
                版&nbsp;&nbsp;本&nbsp;&nbsp;号
            </td>
            <td align="left" class="list" style="width: 35%">
                <asp:TextBox ID="txtVersionCode" runat="server" Width="216px" meta:resourcekey="txtVersionCodeResource1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td nowrap align="right" class="listTitle" style="width: 15%">
                发布范围
            </td>
            <td align="left" class="list" style="width: 35%">
                <uc6:ctrFlowCataDropList ID="ctrReleaseScope" runat="server" ContralState="eNormal"
                    RootID="1037" TextToolTip="发布范围" Visible="true" />
            </td>
            <td nowrap align="right" class="listTitle" style="width: 15%">
                流程状态
            </td>
            <td align="left" class="list" style="width: 35%">
                <asp:DropDownList ID="cboStatus" runat="server" meta:resourcekey="cboStatusResource1">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td nowrap align="right" class="listTitle" style="width: 15%">
                版本性质
            </td>
            <td align="left" class="list" style="width: 35px;">
                <uc6:ctrFlowCataDropList ID="ctrVersionKind" runat="server" ContralState="eNormal"
                    RootID="1038" TextToolTip="版本性质" Visible="true" />
            </td>
            <td nowrap align="right" class="listTitle" style="width: 15%">
                版本类型
            </td>
            <td align="left" class="list" style="width: 35%">
                <uc6:ctrFlowCataDropList ID="ctrVersionType" runat="server" ContralState="eNormal"
                    RootID="1039" TextToolTip="版本类型" Visible="true" />
            </td>
        </tr>
        <tr>
            <td nowrap align="right" class="listTitle" style="width: 15%">
                联&nbsp;&nbsp;系&nbsp;&nbsp;人
            </td>
            <td align="left" class="list" style="width: 35%">
                <uc3:UserPicker ID="UserPicker1" runat="server" TextToolTip="联系人" />
            </td>
            <td nowrap align="right" class="listTitle" style="width: 15%">
                发布时间
            </td>
            <td nowrap align="left" class="list" style="width: 35%">
                <uc2:ctrdateandtime ID="CtrDCreateRegTime" runat="server" ShowTime="false" />
                ~<uc2:ctrdateandtime ID="CtrDEndRegTime" runat="server" ShowTime="false" />
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" width="100%" align="center" class="listContent">
        <tr>
            <td colspan="4" class="listContent">
                <asp:DataGrid ID="gridUndoMsg" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    CssClass="listContent_1" OnItemDataBound="gridUndoMsg_ItemDataBound" OnItemCreated="gridUndoMsg_ItemCreated"
                    OnDeleteCommand="gridUndoMsg_DeleteCommand" meta:resourcekey="gridUndoMsgResource1">
                    <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" />
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="flowid"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="flowdiffminute"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="status"></asp:BoundColumn>
                        <asp:BoundColumn DataField="versionname" HeaderText="版本名称">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="versioncode" HeaderText="版本号">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="releasedate" HeaderText="发布时间" DataFormatString="{0:yyyy-MM-dd}">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="releasescopename" HeaderText="发布范围">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="releasepersonname" HeaderText="联系人">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="releasephone" HeaderText="联系电话">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="versionkindname" HeaderText="版本性质">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="versiontypename" HeaderText="版本类型">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="flowStatus" HeaderText="流程状态">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="处理" ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <input id="CmdDeal" class="btnClass" type="button" value='详情'  onclick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "FlowID"))%>' runat="server">
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="center" Width="5%" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btnDelete" runat="server" Text="删除" CommandName="Delete" CssClass="btnClass"
                                    OnClientClick="OpenDeleteFlow(this);" meta:resourcekey="btnDeleteResource1" />
                                <input id="hidDelete" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "flowid") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btnAssociate" runat="server" Text="变更" CommandName="Associate" CssClass="btnClass"
                                    OnClientClick="ChangeAssociate(this);" meta:resourcekey="btnAssociateResource1" />
                                <asp:Label ID="lblAssociate" runat="server" Text="已变更" ForeColor="Red" meta:resourcekey="lblAssociateResource1"></asp:Label>
                                <input id="hidAssociate" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "flowid") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr id="trShowControlPage" runat="server">
            <td class="listTitle" align="right">
                <uc5:ControlPageFoot ID="cpCST_Issue" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
