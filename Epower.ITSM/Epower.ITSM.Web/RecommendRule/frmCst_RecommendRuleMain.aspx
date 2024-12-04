<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmCst_RecommendRuleMain.aspx.cs" Inherits="Epower.ITSM.Web.RecommendRule.frmCst_RecommendRuleMain" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register src="../Controls/ControlPageFoot.ascx" tagname="ControlPageFoot" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
 
    function checkAll(objectCheck) 
    {

        var demo = document.getElementById('<%=dgCst_ServiceLevel.ClientID%>');
        var gg = demo.getElementsByTagName('INPUT');
        for (i = 0; i < gg.length; i++) 
        {
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
   
    

    <table cellpadding='2' cellspacing='0' width='98%' border='0' class='listContent'>
        <tr>
            <td class='listTitle' align='right' style='width: 12%;'>
                规则名称
            </td>
            <td class='list' style='width: 35%;'>
                <asp:TextBox ID='txtLevelName' runat='server'></asp:TextBox>
            </td>
             <td class='listTitle' style='width: 12%;''>
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
    <table cellpadding="0" width="98%" align="center" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td align="center">
                <asp:DataGrid ID="dgCst_ServiceLevel" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgCst_ServiceLevel_ItemCommand"
                    OnItemCreated="dgCst_ServiceLevel_ItemCreated" OnItemDataBound="dgCst_ServiceLevel_ItemDataBound">
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
                          <asp:TemplateColumn HeaderText="规则名称">                        
                            <ItemTemplate>
                                <asp:Label runat="server" id="Lb_ServiceNo" Text='<%#DataBinder.Eval(Container, "DataItem.RuleName")%>'></asp:Label>              
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>                                                  
                        <asp:BoundColumn DataField='DESCRIPT' HeaderText='规则说明'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='IsAvail' HeaderText='是否有效'>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Left"  HeaderText="修改">
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
