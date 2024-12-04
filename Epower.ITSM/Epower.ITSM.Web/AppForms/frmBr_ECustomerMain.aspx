<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="frmBr_ECustomerMain.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmBr_ECustomerMain" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>

    <style>
        #tooltip
        {
            position: absolute;
            z-index: 3000;
            border: 1px solid #111;
            background-color: #eee;
            padding: 5px;
            opacity: 0.85;
        }
        #tooltip h3, #tooltip div
        {
            margin: 0;
        }
    </style>

    <script language="javascript" type="text/javascript">
        $.ajaxSetup({ cache: false });
        function ShowDetailsInfo(obj, id) {
            $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "frmBr_CustomXmlHttp.aspx?id=" + id }).responseText; } });
        }

        var openobj = window;
        if (typeof (window.dialogArguments) == "object") {
            openobj = window.dialogArguments;
        }
        //资产服务记录
        function SelectService(obj) {
            var lngID = document.getElementById(obj.id.replace("CmdService", "hidID")).value;
            openobj.open("../AppForms/frmIssueList.aspx?NewWin=true&ID=" + lngID, '', 'scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');
            event.returnValue = false;
        }
        //全选复选框
        function checkAll(checkAll) {
            var len = document.forms[0].elements.length;
            var cbCount = 0;
            for (i = 0; i < len; i++) {
                if (document.forms[0].elements[i].type == "checkbox") {
                    if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgECustomer") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                        document.forms[0].elements[i].checked = checkAll.checked;

                        cbCount += 1;
                    }
                }
            }
        } 
    </script>

    <table width='98%' cellpadding="2" cellspacing="0" class="listContent GridTable">
        <tr>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="Custom_MastCustName" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="ddltMastCustID" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="Custom_CustomerType" runat="server" Text="客户类型"></asp:Literal>
            </td>
            <td class="list">
                <uc2:ctrFlowCataDropList ID="ctrFCDServiceType" runat="server" ContralState="eNormal"
                    RootID="1019" Visible="true" />
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitleRight">
                <asp:Literal ID="Custom_CustName" runat="server" Text="客户名称"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtShortName" runat="server"></asp:TextBox>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="Custom_FullName" runat="server" Text="英文名称"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtFullName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitleRight">
                <asp:Literal ID="Custom_Contact" runat="server" Text="联系人"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtLinkMan1" runat="server"></asp:TextBox>
            </td>
            <td class="listTitleRight">
                <asp:Literal ID="Custom_CTel" runat="server" Text="联系电话"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtTel1" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal ID="Custom_CustomCode" runat="server" Text="客户代码"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="txtCustomCode" runat="server"></asp:TextBox>
            </td>
            <td class="listTitleRight">
            </td>
            <td class="list">
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" width="98%" cellspacing="0" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgECustomer" runat="server" Width="100%" PageSize="10" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCommand="dgECustomer_ItemCommand" OnItemCreated="dgECustomer_ItemCreated"
                    OnItemDataBound="dgECustomer_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitleRight"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%" HorizontalAlign="center" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="客户名称">
                            <ItemTemplate>
                                <%--OnClientClick='<%#GetUrl((decimal)DataBinder.Eval(Container.DataItem, "ID"))%>'--%>
                                <asp:Label runat="server" ID="ShortName" Text='<%# DataBinder.Eval(Container, "DataItem.ShortName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="FullName" HeaderText="英文名称">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="MShortName" HeaderText="服单单位">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustomerTypeName" HeaderText="类型">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="linkman1" HeaderText="联系人">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="tel1" HeaderText="联系电话">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="修改">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" runat="server" CommandName="edit" SkinID="btnClass1" Text="修改" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="事件记录">
                            <ItemTemplate>
                                <input id="hidID" runat="server" type="hidden" value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                                <asp:Button ID="CmdService" runat="server" CausesValidation="false" SkinID="btnClass1"
                                    OnClientClick="SelectService(this);" Text="事件记录" Width="60" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc4:ControlPageFoot ID="cpfECustomerInfo" runat="server" />
            </td>
        </tr>
    </table>
    <asp:Label ID="labMsg" runat="server" Visible="False" ForeColor="Red"></asp:Label>
</asp:Content>
