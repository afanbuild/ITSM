<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmzhbytsdirection.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmzhbytsdirection"
    Title="全年投诉趋势分析" %>

<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/common/DictionaryPicker.ascx" TagName="DictionaryPicker"
    TagPrefix="uc2" %>
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
		    document.all.<%=btnOk.ClientID%>.click();
		}
    </script>

    <table id="Table1" width="98%" class="listContent" cellpadding="2">
        <tr>
            <td class="listTitleRight" style="width:12%">
                选择年度:
            </td>
            <td class="list">
                <table cellpadding="2" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <asp:DropDownList ID="dpdYear" runat="server" AutoPostBack="True" Width="80px" OnSelectedIndexChanged="dpdYear_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="listTitleRight" style="width:12%">>
                <asp:Label ID="labManageOffice" runat="server" Text="被投诉人:"></asp:Label>
            </td>
            <td class="list">
                <uc3:UserPicker ID="UserPicker1" runat="server" />
            </td>
            <td class="listTitleRight" style="width:12%">>
                投诉性质:
            </td>
            <td class="list">
                <uc6:ctrFlowCataDropList ID="CataKind" runat="server" RootID="1011" />
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
            <td width="60%" align="left" class="list" valign="top">
                <asp:DataGrid ID="dgTypesCount" runat="server" Width="100%">
                    <Columns>
                        <asp:BoundColumn DataField="投诉性质" HeaderText="投诉性质"></asp:BoundColumn>
                        <asp:BoundColumn DataField="1月" HeaderText="1月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="2月" HeaderText="2月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="3月" HeaderText="3月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="4月" HeaderText="4月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="5月" HeaderText="5月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="6月" HeaderText="6月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="7月" HeaderText="7月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="8月" HeaderText="8月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="9月" HeaderText="9月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="10月" HeaderText="10月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="11月" HeaderText="11月"></asp:BoundColumn>
                        <asp:BoundColumn DataField="12月" HeaderText="12月"></asp:BoundColumn>
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
                        <asp:BoundColumn DataField="qty" ReadOnly="True" HeaderText="投诉次数">
                            <HeaderStyle Width="60%"></HeaderStyle>
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Left" Mode="NumericPages" VerticalAlign="top"></PagerStyle>
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
                                width="16" align="absbottom" />展示图表
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="98%" cellpadding="0" cellspacing="0" border="0">
        <tr id="tr2" runat="server">
            <td id="tdResultChart" runat="server" align="left" class="list">
                <div id="ReportDiv" runat="server">
                </div>
            </td>
        </tr>
    </table>
    <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" Width="0px" Height="0px" />
</asp:Content>
