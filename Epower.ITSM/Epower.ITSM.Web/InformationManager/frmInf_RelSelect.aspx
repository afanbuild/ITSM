<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmInf_RelSelect.aspx.cs" Inherits="Epower.ITSM.Web.InformationManager.frmInf_RelSelect"
    Title="知识库" %>

<%@ Register Src="../Controls/ctrKBCataDropList.ascx" TagName="ctrKBCataDropList"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
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
    
    
    <style type="text/css">
   
        
        #table1 td
        {
        	border: 1px solid #CEE3F2;
        }

        #<%=dgInf_Information.ClientID%> td
        {
        	border: 1px solid #CEE3F2;
        }        
        
    </style>    

    <table id="table1" cellpadding='2' cellspacing='0' width='98%' border='0' class="listContent">
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                关键字
            </td>
            <td class='list' style='width: 35%;'>
                <asp:TextBox ID='txtPKey' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight' style='width: 12%;'>
                类型
            </td>
            <td class='list'>
                &nbsp;<uc2:ctrKBCataDropList ID="CtrKBCataDropList1" runat="server" RootID="1" />
                <asp:CheckBox ID="chkIncludeSub" Text="包含子类" runat="server" />
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                主题
            </td>
            <td class='list' colspan="3">
                <asp:TextBox ID='txtTitle' runat='server'></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class='listTitleRight'>
                内容
            </td>
            <td class='list' colspan="3">
                <asp:TextBox ID='txtContent' runat='server'></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding='1' cellspacing='2' width='98%' border='0' class="listContent">
        <tr>
            <td align="center" class="listTitle">
                <asp:Button ID="btnConfirm" runat="server" Text="确定" OnClick="btnConfirm_Click" />
                &nbsp;
                <asp:Button ID="btnClose" runat="server" Text="取消" OnClick="btnClose_Click" />
                
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgInf_Information" runat="server" Width="100%" CellPadding="1"
                    CellSpacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgInf_Information_ItemCommand"
                    OnItemCreated="dgInf_Information_ItemCreated" OnItemDataBound="dgInf_Information_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" />
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
                            DataNavigateUrlFormatString="frmKBShow.aspx?KBID={0}"></asp:HyperLinkColumn>
                        <asp:BoundColumn DataField='PKey' HeaderText='关键字'></asp:BoundColumn>
                        <asp:BoundColumn DataField='TypeName' HeaderText='类型'></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc1:ControlPage ID="ControlPage1" runat="server"></uc1:ControlPage>
            </td>
        </tr>
    </table>
</asp:Content>
