<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmEquBatchRelMain.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEquBatchRelMain" Title="批量关联资产" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ Register src="../Controls/CtrEquNature.ascx" tagname="CtrEquNature" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript" src="../Js/App_Droplst.js"> </script>

    <script language="javascript" type="text/javascript">	
    //选择关联资产
    function SelectProblems()
	{
	    var newDateObj = new Date()
	    var sparamvalue =  newDateObj.getMinutes() + newDateObj.getMilliseconds();
		var	value=window.showModalDialog('frmEqu_DeskMainImply.aspx?EquIDs=' + document.all.<%=HidEqusID.ClientID%>.value,"","dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no");
		if(value != null)
		{
		    var json = value;
            var record = json.record;
                
            document.all.<%=hidImpEquID.ClientID%>.value= record[0].id;   //资产ID
            document.all.<%=hidFlag.ClientID%>.value="OK";
		}
		else
		{
		    document.all.<%=hidFlag.ClientID%>.value="NO";
		    document.all.<%=hidImpEquID.ClientID%>.value= "0";   //资产ID
		    event.returnValue = false; 
		}
    }
    
    function window.onunload()      
        {
            if(window.opener != undefined && window.opener != null)        
                window.opener.location.reload();
        }
        
    </script>

    <input type="hidden" runat="server" id="hidImpEquID" />
    <input type="hidden" runat="server" id="HidEqusID" />
    <input id="hidCustArrID" runat="server" type="hidden" />
    <input id="hidCustArrIDold" runat="server" type="hidden" />
    <input id="hidFlag" runat="server" type="hidden" />
    <table cellpadding='1' cellspacing='2' width='100%' border='0' class='listContent'>
        <tr>
            <td align="center" class="listTitleNoAlign">
                <asp:Button ID="btnAdd" runat="server" Text="新  增" Width="60px" OnClientClick="SelectProblems();"
                    OnClick="btnAdd_Click" CssClass="btnClass" />
                &nbsp;
                <asp:Button ID="btnSave" runat="server" Text="保  存" Width="60px" OnClick="btnSave_Click"
                    CssClass="btnClass" />
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="100%" align="center" border="0">
        <tr>
            <td align="center" class="listContent" colspan="6">
                <asp:DataGrid ID="dgPro_ProblemAnalyse" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" 
                    CellPadding="1" CellSpacing="2" BorderWidth="0px" OnItemCommand="dgPro_ProblemAnalyse_ItemCommand"
                    OnItemCreated="dgPro_ProblemAnalyse_ItemCreated" OnItemDataBound="dgPro_ProblemAnalyse_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <FooterStyle CssClass="listTitle" />
                    <Columns>
                        <asp:BoundColumn DataField='RelID' HeaderText='RelID' Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Name' HeaderText='Name' Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField='RelPropID' HeaderText='RelPropID' Visible="False"></asp:BoundColumn>
                        <asp:HyperLinkColumn DataTextField="Name" HeaderText="关联资产" Target="_blank" DataNavigateUrlField="RelID"
                            DataNavigateUrlFormatString="frmEquRelMain.aspx?newWin=true&amp;ID={0}">
                            <HeaderStyle Width="50%" HorizontalAlign="Center" />
                        </asp:HyperLinkColumn>
                        <asp:TemplateColumn HeaderText="关联资产属性" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="20%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDeskProp" runat="server">
                                </asp:DropDownList>
                            </ItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="关系描述">
                            <ItemTemplate>
                                <uc6:ctrFlowCataDropList ID="txtRelDescription_RelDescription" runat="server" RootID="1052" />
                            </ItemTemplate>
                            <HeaderStyle Width="15%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle Width="8%" VerticalAlign='Top'></HeaderStyle>
                            <ItemTemplate>
                                <asp:Button ID="btndelete" CommandName="Delete" runat="server" Text="删  除" CausesValidation="False"
                                    CssClass="btnClass"></asp:Button>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
