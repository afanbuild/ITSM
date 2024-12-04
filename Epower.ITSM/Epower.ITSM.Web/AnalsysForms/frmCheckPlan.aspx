<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmCheckPlan.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmCheckPlan"
    Title="资产盘点计划表" %>

<%@ Register Src="../Controls/ctrdateandtime.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="../Controls/ctrEquCataDropList.ascx" TagName="ctrEquCataDropList"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc4" %>
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
        function ShowTable2(imgCtrl)
        {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","Table");
              //var className;
              //var objectFullName;
              var tableCtrl;
              //objectFullName = <%=tr2.ClientID%>.id;
              //className = objectFullName.substring(0,objectFullName.indexOf("tr2")-1);
              tableCtrl = document.all.item(TableID);
              if(imgCtrl.src.indexOf("icon_expandall") != -1)
              {
                tableCtrl.style.display ="";
                imgCtrl.src = ImgMinusScr ;
                document.all.<%=hidTable.ClientID%>.value =document.all.<%=hidTable.ClientID%>.value + "," + tableCtrl.id;
              }
              else
              {
                tableCtrl.style.display ="none";
                imgCtrl.src = ImgPlusScr ;	
                var temp = document.all.<%=hidTable.ClientID%>.value;
                document.all.<%=hidTable.ClientID%>.value = temp.replace(","+tableCtrl.id,""); 
              }
        }
    </script>

    <table cellpadding='2' cellspacing='0' width="98%" class="listContent">
        <tr>
            <td class='listTitleRight' style='width: 12%;'>
                <asp:Literal ID="LitEquDeskName" runat="server" Text="资产名称"></asp:Literal>
            </td>
            <td class='list'>
                <asp:TextBox ID='txtName' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight' style='width: 12%;'>
                <asp:Literal ID="LitEquDeskCode" runat="server" Text="资产编号"></asp:Literal>
            </td>
            <td class='list'>
                <asp:TextBox ID='txtCode' runat='server'></asp:TextBox>
            </td>
        </tr>
    </table>
    <table id="Table12" width="98%" align="center" runat="server" class="listNewContent">
        <tr id="tr2" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable2(this);" height="16" src="../Images/icon_expandall.gif"
                                width="16" align="absbottom" />高级条件
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="Table2" width="98%" class="listContent" style="display: none" cellpadding="2"
        cellspacing="0">
        <tr id="ShowMastCust" runat="server">
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="dpdMastShortName" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
            <td class='listTitleRight' style="width: 12%">
                <asp:Literal ID="LitEquDeskType" runat="server" Text="资产类别"></asp:Literal>&nbsp;
            </td>
            <td class='list'>
                <uc2:ctrEquCataDropList ID="ctrEquCataDropList1" runat="server" RootID="1" />
                <asp:CheckBox ID="chkIncludeSub" Text="包含子类" Checked="true" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                客户信息
            </td>
            <td class="list">
                <asp:TextBox ID='txtCust' runat='server'></asp:TextBox>
            </td>
            <td class='listTitleRight'>
                <asp:Literal ID="LitEquDeskEquStatus" runat="server" Text="资产状态"></asp:Literal>
            </td>
            <td class='list'>
                <uc6:ctrFlowCataDropList ID="ctrCataEquStatus" runat="server" RootID="1018" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                基本配置项
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlSchemaItemJB" runat="server" Width="152px">
                </asp:DropDownList>
                <asp:TextBox ID="txtItemValue" runat="server"></asp:TextBox>
            </td>
            <td class="listTitleRight">
                关联配置项
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlSchemaItemGL" runat="server" Width="152px">
                </asp:DropDownList>
                <asp:CheckBox ID="chkItemValue" Text="是否配置" Checked="true" runat="server" />
            </td>
        </tr>
    </table>
    <input id="hidTable" value="" runat="server" type="hidden" />
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
                           <asp:BoundColumn DataField="mastcustname" HeaderText="服务单位">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="costomname" HeaderText="所属客户">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EquStatusName" HeaderText="资产状态">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="partBankName" HeaderText="维护机构">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="详情">
                            <ItemTemplate>
                                <asp:Button ID="lnkLook" SkinID="btnClass1" runat="server" Text="详情" OnClientClick="OpenDetail(this)" />
                                <input type="hidden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.ID")%>'
                                    id="hidID" />
                            </ItemTemplate>
                            <HeaderStyle Width="44"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc4:ControlPageFoot ID="cpfECustomerInfo" runat="server" />
            </td>
        </tr>
    </table>

    <script language="javascript">	
    if(typeof(document.all.<%=hidTable.ClientID%>) != "undefined")
    {
        var temp = document.all.<%=hidTable.ClientID%>.value;
        var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
        var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus -
        if(temp!="")
        {
            var arr=temp.split(",");
            for(i=1;i<arr.length;i++)
            {
                var tableid=arr[i];
                var tableCtrl = document.all.item(tableid);
                tableCtrl.style.display ="";
                var ImgID = tableid.replace("Table","Img");
                var imgCtrl = document.all.item(ImgID)
                imgCtrl.src = ImgMinusScr ;	
            }
        }
        else
        { 
            var tableid="Table2";
            var tableCtrl = document.all.item(tableid);
            var ImgID = "Img2";
            var imgCtrl = document.all.item(ImgID)
            tableCtrl.style.display ="none";
            imgCtrl.src = ImgPlusScr ;	
        }
    }
    </script>

</asp:Content>
