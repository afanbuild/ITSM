<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_Services_TemplateMain.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frm_Services_TemplateMain"
    Title=" 服务项定义" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc1" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">

        function checkAll(objectCheck) {

            var demo = document.getElementById('<%=dgECustomer.ClientID%>');
            var gg = demo.getElementsByTagName('INPUT');
            for (i = 0; i < gg.length; i++) {
                if (gg[i].type == "checkbox") {
                    gg[i].checked = objectCheck.checked;
                }
            }
        }

        function ServiceLevelSelect(obj) {
            var ServiceTypeID = "0";
            var ServiceKindID = "0";
            var ServiceEffID = "0";
            var ServiceInsID = "0";
            var CustID = "";
            var EquID = "";

            var value = window.showModalDialog("frmCst_ServicLevelSelect.aspx?IsSelect=true&randomid=" + GetRandom() + "&CustID=" + CustID + "&EquID=" + EquID + "&TypeID=" + ServiceTypeID + "&KindID=" + ServiceKindID + "&EffID=" + ServiceEffID + "&InsID=" + ServiceInsID, window, "dialogHeight:600px;dialogWidth:800px");
            if (value != null) {
                if (value.length > 1) {
                    document.getElementById(obj.id.replace("cmdPopServiceLevel", "txtServiceLevel")).value = value[2];   //级别名称        
                    document.getElementById(obj.id.replace("cmdPopServiceLevel", "hidServiceLevel")).value = value[2];   //级别名称
                    document.getElementById(obj.id.replace("cmdPopServiceLevel", "hidServiceLevelID")).value = value[1];  //级别ID                            
                }
            }
        } 
    </script>

    <table cellpadding="2" width="98%" class="listContent">
        <tr>
            <td nowrap class="listTitleRight" style="width: 12%">
                一级服务目录
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="drParentList" runat="server" Width="152px">
                    
                </asp:DropDownList>  
            </td>
            <td nowrap class="listTitleRight" style="width: 12%">
                服务名称
            </td>
            <td class="list" style="width: 35%">
                <uc1:CtrFlowFormText ID="CtrFTTemplateName" runat="server" />
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgECustomer" runat="server" AutoGenerateColumns="False"  CssClass="Gridtable"  BorderWidth="0px"
                    CellPadding="1" CellSpacing="2" OnItemCommand="dgECustomer_ItemCommand" OnItemCreated="dgECustomer_ItemCreated"
                    Width="100%" OnItemDataBound="dgECustomer_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitleRight"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="5%" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="TemplateID" HeaderText="ID" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ServiceLevel" HeaderText="服务目录">
                            <HeaderStyle Width="25%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TemplateName" HeaderText="服务名称">
                            <HeaderStyle Wrap="False" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="IssTempName" HeaderText="事件模板">
                            <HeaderStyle Width="25%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="修改" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" runat="server" CommandName="edit" SkinID="btnClass1" Text="修改" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                             <HeaderStyle Width="44" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <uc2:ControlPageFoot ID="cpTemplate" runat="server" />
            </td>
        </tr>
    </table>
    <asp:Label ID="labMsg" runat="server" Visible="False" ForeColor="Red"></asp:Label>
</asp:Content>
