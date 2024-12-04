<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false"
    AutoEventWireup="true" CodeBehind="frmSubjectedit.aspx.cs" Inherits="Epower.ITSM.Web.CustManager.frmSubjectedit"
    Title="配置项模板" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
        //全选复选框
        function checkAll(checkAll) {
            var len = document.forms[0].elements.length;
            var cbCount = 0;
            for (i = 0; i < len; i++) {
                if (document.forms[0].elements[i].type == "checkbox") {
                    if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgBr_SchemaItems") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                        document.forms[0].elements[i].checked = checkAll.checked;

                        cbCount += 1;
                    }
                }
            }
        }

        function doAddNewItem() {
            var newDateObj = new Date();
            var sparamvalue = newDateObj.getHours().toString() + newDateObj.getSeconds().toString() + newDateObj.getSeconds().toString() + newDateObj.getMilliseconds().toString();
            //====zxl==
            var url="../CustManager/frmBr_SchemaItemsMain.aspx?IsChecked=True&IsSelect='1'&sparamvalue=" + sparamvalue+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
             window.open(url,"",'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=600,height=480,left=150,top=50');
        }

        function AddNewItem() {
            var newDateObj = new Date();
            var sparamvalue = newDateObj.getHours().toString() + newDateObj.getSeconds().toString() + newDateObj.getSeconds().toString() + newDateObj.getMilliseconds().toString();
            //=====zxl==
            var url="../CustManager/frmBr_SchemaItemsEdit.aspx?IsNew=true&sparamvalue=" + sparamvalue+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
            window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=600,height=480");
        }

        String.prototype.trim = function()  //去空格
        {
            return this.replace(/(^\s*)|(\s*$)/g, "");
        }
    </script>
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
    <table cellpadding="0" cellspacing="0" width="98%" align="center" border="0">
        <tr>
            <td align="right">
                <input id="cbtnAdd" class="btnClass" onclick="doAddNewItem();" runat="server" type="button"
                    value="批量添加" causesvalidation="false" />
                <input id="cbtnNew" class="btnClass" onclick="AddNewItem();" runat="server" type="button"
                    value="新属性项" causesvalidation="false" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:DataGrid ID="dgSchema" runat="server" Width="100%" CellPadding="1" CellSpacing="2"
                    BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgPro_ProvideManage_ItemCommand"
                    OnItemDataBound="dgPro_ProvideManage_ItemDataBound" OnItemCreated="dgPro_ProvideManage_ItemCreated">
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtID" Text='<%# DataBinder.Eval(Container, "DataItem.ID")%>' onblur="CheckDoubleID(this,'txtID');"
                                    Width="85%" runat="server" Enabled="False"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="类别">
                            <ItemTemplate>
                                &nbsp;<asp:DropDownList ID="ddlTypeName" runat="server" onchange="CheckDefaultControlStatus(this);"
                                    Width="95%" SelectedValue='<%# DataBinder.Eval(Container, "DataItem.TypeName") %>'
                                    Enabled="False">
                                    <asp:ListItem>基础信息</asp:ListItem>
                                    <asp:ListItem>关联配置</asp:ListItem>
                                    <asp:ListItem>备注信息</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="属性名称">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCHName" Text='<%# DataBinder.Eval(Container, "DataItem.CHName")%>'
                                    Width="90%" runat="server" Enabled="False"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Width="25%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="初值">
                            <ItemTemplate>
                                <asp:Panel ID="PanDefault" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"0")%>'
                                    runat="server" Height="16px" Width="125px">
                                    <asp:CheckBox ID="chkDefault" Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.Default"))%>'
                                        runat="server" /></asp:Panel>
                                <asp:Panel ID="PantxtDefault" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"1")%>'
                                    runat="server" Height="24px" Width="95%">
                                    <asp:TextBox ID="txtDefault" Text='<%# DataBinder.Eval(Container, "DataItem.Default")%>'
                                        Width="100%" runat="server"></asp:TextBox></asp:Panel>
                                <asp:Panel ID="PantxtMDefault" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"2")%>'
                                    runat="server" Height="24px" Width="95%">
                                    <asp:TextBox ID="txtMDefault" Text='<%# DataBinder.Eval(Container, "DataItem.Default")%>'
                                        Width="100%" runat="server" TextMode="MultiLine"></asp:TextBox></asp:Panel>
                            </ItemTemplate>
                            <HeaderStyle Width="22%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="组">
                            <ItemTemplate>
                                <asp:TextBox ID="txtGroup" Text='<%# DataBinder.Eval(Container, "DataItem.Group")%>'
                                    Width="95%" runat="server"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Width="30%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="删除">
                            <ItemTemplate>
                                <asp:Button ID="lnkdelete" SkinID="btnClass1" runat="server" Text="删除" CommandName="delete"
                                    CausesValidation="false" />
                            </ItemTemplate>
                            <HeaderStyle Width="60" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="60" />
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <input id="hidSchemaXml" type="hidden" runat="server" name="hidSchemaXml" />
    <input id="hidCatalogID" type="hidden" runat="server" name="hidCatalogID">
    <input id="hidTempID" runat="server" type="hidden" />
    <input id="Hidden1" runat="server" name="hidClientId_ForOpenerPage"  type="hidden" />
    <asp:Button ID="btnAddNewItem" runat="server" Text="填加配置项"  style="display:none;" CssClass="btnClass" OnClick="btnAddNewItem_Click"
         CausesValidation="false" />  <%--Width="0px"--%>
</asp:Content>
