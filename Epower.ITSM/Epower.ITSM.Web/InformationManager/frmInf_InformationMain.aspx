<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmInf_InformationMain.aspx.cs" Inherits="Epower.ITSM.Web.InformationManager.frmInf_InformationMain"
    Title="知识库" EnableEventValidation="false"  %>

<%@ Register Src="../Controls/ctrKBCataDropList.ascx" TagName="ctrKBCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
        //全选复选框
        function checkAll(checkAll) {
            var len = document.forms[0].elements.length;
            var cbCount = 0;
            for (i = 0; i < len; i++) {
                if (document.forms[0].elements[i].type == "checkbox") {
                    if (document.forms[0].elements[i].name.indexOf("chkDel") != -1 &&
				document.forms[0].elements[i].name.indexOf("dgInf_Information") != -1 &&
				document.forms[0].elements[i].disabled == false) {
                        document.forms[0].elements[i].checked = checkAll.checked;

                        cbCount += 1;
                    }
                }
            }
        }
    </script>

    <table width='98%' border='0' class="listContent Gridtable" cellpadding="2" cellspacing="0">
        <tr id="visbletr0" runat="server">
            <td class='listTitleRight' style='width: 12%;'>
              <asp:Literal ID="info_PKey" runat="server" Text="关键字"></asp:Literal>  

            </td>
            <td class='list'>
                <asp:TextBox ID='txtPKey' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight' style='width: 12%;'>
              <asp:Literal ID="info_Title" runat="server" Text="主题"></asp:Literal>    
            </td>
            <td class='list'>
                <asp:TextBox ID='txtTitle' runat="server" MaxLength="25"></asp:TextBox>
            </td>
        </tr>
        <tr id="visbletr1" runat="server">
            <td class='listTitleRight'>
             <asp:Literal ID="info_Tags" runat="server" Text="摘要"></asp:Literal>     
            </td>
            <td class='list'>
                <asp:TextBox ID='txtTags' runat='server' MaxLength="25"></asp:TextBox>
            </td>
            <td class='listTitleRight'>
              <asp:Literal ID="info_Content" runat="server" Text="内容"></asp:Literal>      
            </td>
            <td class='list'>
                <asp:TextBox ID='txtContent' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr id="visbletr2" runat="server">
            <td class='listTitleRight' style='width: 12%;'>
             <asp:Literal ID="info_TypeName" runat="server" Text="类型"></asp:Literal>         
            </td>
            <td class='list' colspan="3">
                <uc2:ctrKBCataDropList ID="CtrKBCataDropList1" runat="server" RootID="1" />
                <asp:CheckBox ID="chkIncludeSub" Text="包含子类" runat="server" Checked="true" />
            </td>
        </tr>
        <tr id="InfSearch" runat="server" visible="false">
            <td class='listTitleRight' style='width: 12%;'>
                知识搜索
            </td>
            <td colspan="3" class="list">
            <asp:TextBox ID='txtKeyName' runat='server'  Text="" ></asp:TextBox>
             <asp:Button ID="btnSearch" runat="server" Text="搜索" onclick="btnSearch_Click" />
            </td>
        </tr>
    </table>
    <br />
    <table width="98%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgInf_Information" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    OnItemCommand="dgInf_Information_ItemCommand" OnItemCreated="dgInf_Information_ItemCreated"
                    OnItemDataBound="dgInf_Information_ItemDataBound">
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
                        <asp:BoundColumn DataField='ID' HeaderText='ID' Visible="False"></asp:BoundColumn>
                        <asp:HyperLinkColumn DataTextField="Title" HeaderText="主题" Target="_blank" DataNavigateUrlField="ID"
                            DataNavigateUrlFormatString="frmKBShow.aspx?KBID={0}" ItemStyle-HorizontalAlign="Left">
                        </asp:HyperLinkColumn>
                        <asp:BoundColumn DataField='PKey' HeaderText='关键字' ItemStyle-HorizontalAlign="Left">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='TypeName' HeaderText='类型' ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='Tags' HeaderText='摘要' ItemStyle-HorizontalAlign="Left">
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="修改">
                            <ItemTemplate>
                                <asp:Button ID="lnkedit" SkinID="btnClass1" runat="server" Text="修改" CommandName="edit" />
                            </ItemTemplate>
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="关联知识">
                            <ItemTemplate>
                                <asp:Button ID="lnklink" Width="60" SkinID="btnClass1" runat="server" Text="关联知识"
                                    CommandName="link" />
                            </ItemTemplate>
                            <HeaderStyle Width="64"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc3:ControlPageFoot ID="cpInformation" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
