<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ChangeAcceptedNumCounter.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.ChangeAcceptedNumCounter" Title="变更受理数量统计" %>

<%@ Register src="../Controls/DeptPickerRight.ascx" tagname="DeptPickerRight" tagprefix="uc4" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register src="../Controls/ctrDateSelectTimeV2.ascx" tagname="ctrDateSelectTime" tagprefix="uc1" %>
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
        function btnClick() {
            document.getElementById("<%=btnSelect.ClientID %>").click();
        }
        function DeptOnChangeSelect()
        {
            document.getElementById("<%=btnSelect.ClientID %>").click();
        }    
    </script>

    <div style="display: none;">
        <asp:Button ID="btnSelect" Width="0px" runat="server" OnClick="btnSelect_Click" />
    </div>
    
    <table id="Table1" width="98%" class="listContent" cellpadding="2">
        <tr>
            <td class="listTitleRight" width="12%">
                部门:
            </td>
            <td class="list" width="35%">
                <uc4:DeptPickerRight ID="DeptPicker1" runat="server" />
                <asp:CheckBox ID="chkIncludeSub" Text="包含子部门" runat="server" Checked="true" />
            </td>   
            <td class="listTitleRight" width="12%">
                统计类型:
            </td>
            <td class="list">
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="dpdMastCustomer_SelectedIndexChanged">
                    <asp:ListItem Value="0" Selected>按周</asp:ListItem>
                    <asp:ListItem Value="1">按月</asp:ListItem>
                </asp:DropDownList>
                <asp:CheckBox ID="chkGroupType" Text="是否按变更类别统计" runat="server" Checked="true" />
            </td>        
        </tr>
        <tr>
            <td class="listTitleRight">
                登记时间
            </td>
            <td class="list" colspan="3">                
                <uc1:ctrDateSelectTime ID="ctrDateTime" runat="server" OnChangeScript="btnClick()"  />                
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
    <table width="98%" border="0" cellpadding="0" border="0">
        <tr id="tr1" runat="server" style="display: none;">
            <td align="left">
                <asp:DataGrid ID="dgTypesCount" runat="server" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" CssClass="Gridtable">
                    <Columns>
                        <asp:BoundColumn DataField="CountName" HeaderText="区间">
                            <HeaderStyle Width="50%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CountNum" HeaderText="数量"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
                <asp:DataGrid ID="dgTypesCount2" runat="server" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" CssClass="Gridtable">
                    <Columns>
                        <asp:BoundColumn DataField="CountName" HeaderText="区间">
                            <HeaderStyle Width="50%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="changetypename" HeaderText="变更类别"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CountNum" HeaderText="数量"></asp:BoundColumn>
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
    
<script type="text/javascript" src="../js/epower.analsysforms.changeacceptednumcounter.js"></script>      
</asp:Content>