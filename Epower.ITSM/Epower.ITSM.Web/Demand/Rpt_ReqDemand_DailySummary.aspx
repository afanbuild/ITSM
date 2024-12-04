<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Rpt_ReqDemand_DailySummary.aspx.cs" Inherits="Epower.ITSM.Web.Demand.Rpt_ReqDemand_DailySummary" Title="需求 - 服务量日报表" %>


<%@ Register Src="../Controls/common/DictionaryPicker.ascx" TagName="DictionaryPicker"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/ctrdateandtime.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/ctrDateSelectTimeV2.ascx" TagName="ctrDateSelectTime"
    TagPrefix="uc5" %>
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

    <script language="javascript" type="text/javascript">
        function btnClick()
        {
            document.getElementById("<%=btnSelect.ClientID %>").click();
        }    
    </script>
    
    <div style="display:none;">
        <asp:Button ID="btnSelect" Width="0px" runat="server" OnClick="btnSelect_Click" />
    </div>
    

    <table id="Table1" width="98%" class="listContent" cellpadding="2">
        <tr>
            <td class="listTitleRight" width="12%">
                时间范围:
            </td>
            <td class="list">
                <uc5:ctrDateSelectTime ID="ctrDateTime" runat="server" OnChangeScript="btnClick()"/>
                <asp:Literal ID="lblDept" runat="server" Text="责任部门" Visible="false"></asp:Literal>
                <asp:DropDownList ID="dpdManageOffice" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dpdManageOffice_SelectedIndexChanged"
                    Visible="false">
                </asp:DropDownList>
            </td>
            <td class="listTitleRight" align="center" width="12%">
                需求类别:
            </td>
            <td class="list" colspan="3">
                <uc6:ctrFlowCataDropList ID="ctrFCDServiceType" runat="server" RootID="1003" />
            </td>
            <td class="listTitleRight" width="12%" style="display:none;">
                <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list" style="display:none;">
                <table cellpadding="2" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <asp:DropDownList ID="dpdMastShortName" runat="server" AutoPostBack="True" Width="140px"
                                OnSelectedIndexChanged="dpdMastShortName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
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
    <table width="98%" class="listContent" cellpadding="0" border="0">
        <tr id="tr1" runat="server" style="display: none;">
            <td align="left" class="listContent">
                <asp:DataGrid ID="dgTypesCount" runat="server" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False"  CssClass="Gridtable" >
                    <Columns>
                        <asp:BoundColumn DataField="Regsysdate" HeaderText="日期">
                            <HeaderStyle Width="50%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="QTY" HeaderText="数量"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
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
    <table width="98%" cellpadding="0" cellspacing="0" border="0">
        <tr id="tr2" runat="server">
            <td id="tdResultChart" runat="server" align="left">
                <div id="ReportDiv" runat="server">
                </div>
            </td>
        </tr>
    </table>

</asp:Content>
