<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True"
    CodeBehind="frmzhCstWork.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmzhCstWork"
    Title="工作量统计" %>

<%@ Register Src="../Controls/ServiceStaffMastCust.ascx" TagName="ServiceStaffMastCust"
    TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrdateandtime.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew"
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
        
        function UserPickerChange()
        {
            alert("");
           __doPostBack('datarange', '');
        }
    </script>

    <script language="javascript" type="text/javascript">
        function btnClick()
        {
            document.getElementById("<%=btnSelect.ClientID %>").click();
        }    
    </script>

    <div style="display: none;">
        <asp:Button ID="btnSelect" Width="0px" runat="server" OnClick="btnSelect_Click" />
    </div>
    <table id="Table1" width="98%" class="listContent" cellpadding="2">
        <tr>
            <td class="listTitleRight" style="width: 12%">
                时间范围:
            </td>
            <td class="list">
                <uc5:ctrDateSelectTime ID="ctrDateTime" runat="server" OnChangeScript="btnClick()" />
            </td>
            <td class="listTitleRight" style="width: 12%">
                服务状态:
            </td>
            <td class="list">
                <uc6:ctrFlowCataDropListNew ID="CtrFCDDealStatus" runat="server" RootID="1017" ShowType="2" />
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="dpdMastShortName" runat="server" AutoPostBack="True" Width="150px"
                    OnSelectedIndexChanged="dpdMastShortName_SelectedIndexChanged">
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
                <asp:DataGrid ID="dgMaterialStat" runat="server" Width="100%" cssclass="GridTable" AllowCustomPaging="True"
                    PageSize="100" OnItemCreated="dgMaterialStat_ItemCreated" OnItemDataBound="dgMaterialStat_ItemDataBound">
                    <Columns>
                        <asp:BoundColumn DataField="Name" HeaderText="工程师名称">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="BlongDeptName" HeaderText="服务单位">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Num" HeaderText="事件单数"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TotalHours" HeaderText="总时间"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TotalAmount" HeaderText="总费用"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
