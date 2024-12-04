<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmzhcststatisfied.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmzhcststatisfied"
    Title="全年服务满意度分析" %>

<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
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
    </script>

    <table id="Table1" width="98%" class="listContent" cellpadding="2">
        <tr>
            <td class="listTitleRight" width="12%">
                选择年度:
            </td>
            <td class="list">
                <asp:DropDownList ID="dpdYear" runat="server" AutoPostBack="True" Width="80px" OnSelectedIndexChanged="dpdYear_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label ID="labManageOffice" runat="server" Text="责任部门:" Visible="false"></asp:Label>&nbsp;<asp:DropDownList
                    ID="dpdManageOffice" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dpdManageOffice_SelectedIndexChanged"
                    Visible="false">
                </asp:DropDownList>
            </td>
            <td class="listTitleRight" width="12%">
                事件类别:
            </td>
            <td class="list" colspan="3">
                <uc6:ctrFlowCataDropList ID="ctrFCDServiceType" runat="server" RootID="1001" />
            </td>
            <td class="listTitleRight" width="12%" style="display: none;">
                <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list" style="display: none;">
                <asp:DropDownList ID="dpdMastCustomer" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dpdMastCustomer_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="98%">
        <tr>
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_expandall.gif"
                                width="16" align="absbottom" />
                            展示数据
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="98%" cellpadding="0" border="0" id="Table2" runat="server">
        <tr style="display: none;">
            <td>
                &nbsp;&nbsp;服务级别:<uc6:ctrFlowCataDropList ID="ctrFCDWTType" runat="server" RootID="1020" />
            </td>
        </tr>
        <tr id="tr1" runat="server" style="display: none;">
            <td valign="top" class="list">
                <asp:DataGrid ID="grdTypeDirection" runat="server" CssClass="GridTable" EnableViewState="False"
                    Width="100%" OnItemDataBound="grdTypeDirection_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="月份" HeaderText="月份" ReadOnly="True">
                            <HeaderStyle Width="16%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="事件数量" HeaderText="事件数量" ReadOnly="True">
                            <HeaderStyle Width="16%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="回访次数" HeaderText="回访次数" ReadOnly="True">
                            <HeaderStyle Width="16%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="满意次数" HeaderText="满意次数">
                            <HeaderStyle Width="16%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="回访率" DataFormatString="{0:N2}%" HeaderText="回访率" ReadOnly="True">
                            <HeaderStyle Width="16%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="满意度" DataFormatString="{0:N2}%" HeaderText="满意度">
                            <HeaderStyle Width="17%" />
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle VerticalAlign="top" HorizontalAlign="Left" Mode="NumericPages" />
                </asp:DataGrid>
                <div runat="server" id="divGrid">
                </div>
            </td>
        </tr>
    </table>
    <table width="98%">
        <tr>
            <td valign="top" align="left" width="100%" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            展示图表
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="98%" cellpadding="2" cellspacing="0" border="0">
        <tr id="tr2" runat="server">
            <td id="tdResultChart" runat="server" align="left" class="list">
                <div id="ReportDiv" runat="server">
                </div>
                <br />
                &nbsp;年平均服务事件满意度:<asp:Label ID="lblOnTimeRate" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
