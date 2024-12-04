<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True"
    CodeBehind="RepEquList.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.RepEquList"
    Title="资产明细表" %>

<%@ Register Src="../Controls/ctrdateandtime.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc5" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
    TagPrefix="uc6" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
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
        
        function OpenDetail(obj)
        {
            var sValue = document.getElementById(obj.id.replace("lnkLook","hidID")).value;
            window.open("../EquipmentManager/frmEqu_DeskEdit.aspx?IsSelect=1&id=" + sValue + "&FlowID=0&Soure=0&newWin=0","","scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
            event.returnValue = false;
        }
    </script>

    <table id="Table1" width="98%" class="listContent" cellpadding="2">
        <tr>
            <td class="listTitleRight" style="width: 12%;">
                <asp:Literal ID="LitEquSta" runat="server" Text="资产状态"></asp:Literal>
            </td>
            <td class="list">
                <uc6:ctrFlowCataDropListNew ID="CtrFCDDealStatus" runat="server" RootID="1018" ShowType="2" />
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="dpdMastShortName" runat="server" AutoPostBack="True" Width="152px"
                    OnSelectedIndexChanged="dpdMastShortName_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="98%">
        <tr>
            <td valign="top" align="left" width="100%" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            展示数据
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="98%" cellpadding="0" border="0" id="tblResult" runat="server">
        <tr id="tr1" runat="server">
            <td>
                <asp:DataGrid ID="dgMaterialStat" runat="server" Width="100%" AllowCustomPaging="True"
                    PageSize="100" OnItemCreated="dgMaterialStat_ItemCreated" OnItemDataBound="dgMaterialStat_ItemDataBound">
                    <Columns>
                        <asp:BoundColumn DataField="ID" HeaderText="ID" Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CatalogName" HeaderText="资产类别">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Name" HeaderText="资产名称">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="code" HeaderText="资产编号">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustMastName" HeaderText="服务单位">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CostomName" HeaderText="所属客户">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="详情">
                            <ItemTemplate>
                                <asp:Button ID="lnkLook" SkinID="btnClass1" runat="server" Text="详情" OnClientClick="OpenDetail(this)" />
                                <input type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.ID")%>'
                                    id="hidID" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc5:ControlPageFoot ID="cpCST_Issue" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
