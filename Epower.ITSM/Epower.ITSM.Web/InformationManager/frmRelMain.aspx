<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmRelMain.aspx.cs" Inherits="Epower.ITSM.Web.InformationManager.frmRelMain"
    Title="知识关联" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">	
    //选择关联知识
    function SelectProblems()
	{
	    var newDateObj = new Date()
	    var sparamvalue =  newDateObj.getMinutes() + newDateObj.getMilliseconds();
	    //========zxl===========
	    var url='frmInf_RelSelect.aspx?pDate='+sparamvalue+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
	    
	    window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=800,height=500,left=150,top=50");
    }
    </script>
    <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"  type="hidden" />
    <input id="hidCustArrID" runat="server" type="hidden" />
    <input id="hidCustArrIDold" runat="server" type="hidden" />
    <input id="hidFlag" runat="server" type="hidden" />
    <table cellpadding='1' cellspacing='2' width='98%' border='0' class='listContent'>
        <tr>
            <td class='listTitle' align='right' style='width: 12%;'>
                主知识主题
            </td>
            <td class='list' colspan="3">
                <asp:Label ID='lblEventTitle' runat='server'></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding='1' cellspacing='2' width='98%' border='0' class="listContent">
        <tr>
            <td align="center" class="listTitle">
                <asp:Button ID="btnAdd" runat="server" Text="新  增" OnClientClick="SelectProblems();"
                     CssClass="btnClass" />                
                <asp:Button ID="btnAddHid" runat="server" Text="新  增"
                    OnClick="btnAdd_Click" CssClass="btnClass" style="display:none"/>
                &nbsp;
                <asp:Button ID="btnSave" runat="server" Text="保  存" OnClick="btnSave_Click"
                    OnClientClick="return CheckValue();" CssClass="btnClass" Visible="false"  />  <%--Width="0px"--%>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td colspan="6">
                <asp:DataGrid ID="dgPro_ProblemAnalyse" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    CellPadding="1" CellSpacing="2" BorderWidth="0px" OnItemCommand="dgPro_ProblemAnalyse_ItemCommand"
                    OnItemCreated="dgPro_ProblemAnalyse_ItemCreated" OnItemDataBound="dgPro_ProblemAnalyse_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" />
                    <FooterStyle CssClass="listTitle" />
                    <Columns>
                        <asp:BoundColumn DataField='KBID' HeaderText='KBID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='RelID' HeaderText='RelID' Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Title' HeaderText='Title' Visible="false"></asp:BoundColumn>
                        <asp:HyperLinkColumn DataTextField="Title" HeaderText="关联知识主题" Target="_blank" DataNavigateUrlField="RelID"
                            DataNavigateUrlFormatString="frmKBShow.aspx?KBID={0}"></asp:HyperLinkColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center"
                            HeaderText="删除">
                            <HeaderStyle Width="8%" VerticalAlign='Top'></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btndelete" CommandName="Delete" runat="server" Text="删除" CausesValidation="False"
                                    SkinID="btnClass1"></asp:Button>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
