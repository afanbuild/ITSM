<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmCst_ServiceLevelMain.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmCst_ServiceLevelMain" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">

        function checkAll(objectCheck) {

            var demo = document.getElementById('<%=dgCst_ServiceLevel.ClientID%>');
            var gg = demo.getElementsByTagName('INPUT');
            for (i = 0; i < gg.length; i++) {
                if (gg[i].type == "checkbox") {
                    gg[i].checked = objectCheck.checked;
                }
            }
        }
    </script>

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

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        $.ajaxSetup({ cache: false });
        function ShowDetailsInfo(obj, id) {
            $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "frmBr_CustomXmlHttp.aspx?ServiceLeve=" + id }).responseText; } });
        }
    
    </script>

    <table cellpadding='2' cellspacing='0' width='98%' border='0' class='listContent Gridtable'>
        <tr>
            <td class='listTitleRight' align='right' style='width: 12%;'>
                级别名称
            </td>
            <td class='list' style='width: 35%;'>
                <asp:TextBox ID='txtLevelName' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight' align='right' style='width: 12%;'>
                级别定义
            </td>
            <td class='list'>
                <asp:TextBox ID='txtDefinition' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                服务包括
            </td>
            <td class='list'>
                <asp:TextBox ID='txtBaseLevel' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight'>
                服务不包括
            </td>
            <td class='list'>
                <asp:TextBox ID='txtNotInclude' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                服务有效性
            </td>
            <td class='list'>
                <asp:TextBox ID='txtAvailability' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight'>
                是否有效
            </td>
            <td class='list'>
                <asp:RadioButtonList ID="rdbtIsAvail" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0">有效</asp:ListItem>
                    <asp:ListItem Value="1">无效</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
    <br />
    <table width="98%" align="center" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td align="center">
                <asp:DataGrid ID="dgCst_ServiceLevel" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCommand="dgCst_ServiceLevel_ItemCommand" OnItemCreated="dgCst_ServiceLevel_ItemCreated"
                    OnItemDataBound="dgCst_ServiceLevel_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" onclick="checkAll(this);"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="false"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="级别名称" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="Lb_ServiceNo" Text='<%#DataBinder.Eval(Container, "DataItem.LevelName")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='Definition' HeaderText='级别定义' ItemStyle-Width="20%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='BaseLevel' HeaderText='服务包括' ItemStyle-Width="20%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='NotInclude' HeaderText='服务不包括' ItemStyle-Width="15%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='Availability' HeaderText='服务有效性' ItemStyle-Width="15%">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='IsAvail' HeaderText='是否有效' ItemStyle-Width="60"></asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" HeaderText="修改">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="修改" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="44"></HeaderStyle>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc2:ControlPageFoot ID="cpServiceLevel" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
