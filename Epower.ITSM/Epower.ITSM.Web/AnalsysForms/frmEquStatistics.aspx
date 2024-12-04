<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEquStatistics.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmEquStatistics"
    Title="资产统计表" %>

<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrtitle.ascx" TagName="ctrtitle" TagPrefix="uc1" %>
<%@ Register Src="../Controls/ctrEquCataDropList.ascx" TagName="ctrEquCataDropList"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
function ShowTable(imgCtrl)
        {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","tr");
              
              var className;
              var objectFullName;
              var tableCtrl;
              objectFullName = <%=tr1.ClientID%>.id;
              className = objectFullName.substring(0,objectFullName.indexOf("tr1")-1);
              tableCtrl = document.all.item(className.substr(0,className.length)+"_"+TableID);
              
              if(imgCtrl.src.indexOf("icon_expandall") != -1)
              {
                tableCtrl.style.display ="";
                imgCtrl.src = ImgMinusScr ;
              }
              else
              {
                tableCtrl.style.display ="none";
                imgCtrl.src = ImgPlusScr ;		 
              }
        }
    </script>

    <table width="98%" class="listContent" cellpadding="2" cellspacing="0">
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                <asp:Literal ID="LitMastCust" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class='list'>
                <asp:DropDownList ID="ddlMastCust" Width="152px" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlMastCust_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class='listTitleRight' style='width: 12%;'>
                <asp:Literal ID="LitEquDeskType" runat="server" Text="资产类别"></asp:Literal>&nbsp;
            </td>
            <td class='list'>
                <uc2:ctrEquCataDropList ID="ctrEquCataDropList1" runat="server" RootID="1" />
                <asp:CheckBox ID="chkIncludeSub" Text="包含子类" Checked="true" runat="server" />
            </td>
        </tr>
    </table>
    <table width="98%">
        <tr>
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            展示数据
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="98%" border="0" cellpadding="0" cellspacing="0" id="tblResult" runat="server">
        <tr id="tr1" runat="server">
            <td>
                <asp:DataGrid ID="dgEquStatistics" runat="server" Width="100%" AllowCustomPaging="True"
                    PageSize="100" OnItemCreated="dgEquStatistics_ItemCreated" OnItemDataBound="dgEquStatistics_ItemDataBound">
                    <Columns>
                        <asp:TemplateColumn HeaderText="序号">
                            <ItemTemplate>
                                <%# Container.ItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="CatalogName" HeaderText="资产类别">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="MastCust" HeaderText="服务单位">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Equstts" HeaderText="正在维修总数" ItemStyle-Width="500px">
                            <ItemStyle Width="500px"></ItemStyle>
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
