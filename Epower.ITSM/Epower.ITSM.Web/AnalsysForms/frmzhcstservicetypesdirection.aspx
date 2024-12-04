<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True"
    CodeBehind="frmzhcstservicetypesdirection.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmzhcstservicetypesdirection"
    Title="全年服务量趋势分析" %>

<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register Src="../Controls/common/DictionaryPicker.ascx" TagName="DictionaryPicker"
    TagPrefix="uc2" %>
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
                &nbsp;&nbsp;<asp:Label ID="labManageOffice" runat="server" Text="责任部门:" Visible="false"></asp:Label>&nbsp;
                <asp:DropDownList ID="dpdManageOffice" runat="server" AutoPostBack="True" Width="80px"
                    OnSelectedIndexChanged="dpdManageOffice_SelectedIndexChanged" Visible="false">
                </asp:DropDownList>
            </td>
            <td class="listTitleRight" width="12%">
                事件类别:
            </td>
            <td class="list" colspan="3">
                <uc6:ctrFlowCataDropList ID="ctrFCDServiceType" runat="server" RootID="1001" />
            </td>
            <td class="listTitleRight" width="12%" style="display:none;">
                <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list" style="display:none;">
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
    <table width="98%" cellpadding="0" border="0" id="tblResult" runat="server">
        <tr id="tr1" runat="server" style="display: none;">
            <td width="60%" align="left" valign="top" class="list">
                <asp:DataGrid ID="dgTypesCount" cssclass="GridTable" runat="server" Width="100%">
                    <Columns>
                        <asp:BoundColumn DataField="事件类别" HeaderText="事件类别"></asp:BoundColumn>
                        <asp:BoundColumn DataField="一月" HeaderText="一月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="二月" HeaderText="二月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="三月" HeaderText="三月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="四月" HeaderText="四月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="五月" HeaderText="五月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="六月" HeaderText="六月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="七月" HeaderText="七月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="八月" HeaderText="八月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="九月" HeaderText="九月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="十月" HeaderText="十月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="十一月" HeaderText="十一月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="十二月" HeaderText="十二月"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
            <td width="1%" class="list">
            </td>
            <td width="39%" align="left" valign="top" class="list">
                <asp:DataGrid ID="grdTypeDirection" runat="server" Width="100%" AutoGenerateColumns="False"  CssClass="Gridtable" >
                    <Columns>
                        <asp:BoundColumn DataField="months" ReadOnly="True" HeaderText="月份">
                            <HeaderStyle Width="40%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="qty" ReadOnly="True" HeaderText="事件数量">
                            <HeaderStyle Width="60%"></HeaderStyle>
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Left" Mode="NumericPages" VerticalAlign="top"></PagerStyle>
                    
                </asp:DataGrid>
                <div runat="server" id="divGrid">
                
                </div>
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
                            展示图表
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="98%" class="listContent" cellpadding="0" cellspacing="0" border="0">
        <tr id="tr2" runat="server">
            <td id="tdResultChart" runat="server" align="left" class="list">
                <div id="ReportDiv" runat="server">
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
